/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Collections;
using CmsData;
using UtilityExtensions;
using CMSPresenter;

namespace CMSWeb.Reports
{
    public partial class RosterReport : System.Web.UI.Page
    {
        public class MemberInfo
        {
            public string Name { get; set; }
            public int Id { get; set; }
            public string Organization { get; set; }
            public string Location { get; set; }
            public string MemberType { get; set; }
        }

        private Font boldfont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD);
        private Font font = FontFactory.GetFont(FontFactory.HELVETICA);
        private Font smallfont = FontFactory.GetFont(FontFactory.HELVETICA, 7);
        private PageEvent pageEvents = new PageEvent();
        private PdfPTable t;
        private Document doc;
        private DateTime dt;
        private PdfContentByte dc;

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "filename=foo.pdf");

            var org = this.QueryString<int?>("org");
            var qid = this.QueryString<int?>("queryid");
            var div = this.QueryString<int?>("div");
            var schedule = this.QueryString<int?>("schedule");
            dt = DateTime.Now;
            var tm = this.QueryString<string>("tm");

            doc = new Document(PageSize.LETTER, 36, 36, 64, 64);
            var w = PdfWriter.GetInstance(doc, Response.OutputStream);
            w.PageEvent = pageEvents;
            doc.Open();

            var ctl = new RollsheetController();
            dc = w.DirectContent;

            if (qid.HasValue)
            {
                var o = list(org, null, null).First();
                StartPageSet(o);
                var qB = DbUtil.Db.LoadQueryById(qid.Value);
                var q = from p in DbUtil.Db.People.Where(qB.Predicate())
                        join m in ctl.FetchOrgMembers(o.OrgId) on p.PeopleId equals m.PeopleId into j
                        from m in j.DefaultIfEmpty()
                        orderby p.Name2
                        select new
                        {
                            p.Name,
                            MembertypeCode = (m == null ? "V" : m.MemberTypeCode),
                            vbapp = p.VBSApps.OrderByDescending(v => v.Uploaded).FirstOrDefault(),
                            p.PeopleId,
                        };
                foreach (var i in q)
                    AddRow(i.MembertypeCode, i.Name, i.PeopleId, i.vbapp, font);
                if (t.Rows.Count > 1)
                    doc.Add(t);
                else
                    doc.Add(new Phrase("no data"));
            }
            else
                foreach (var o in list(org, div, schedule))
                {
                    var q = from m in ctl.FetchOrgMembers(o.OrgId)
                            let vbapp = DbUtil.Db.VBSApps.Where(v => v.PeopleId == m.PeopleId).OrderByDescending(v => v.Uploaded).FirstOrDefault()
                            orderby m.Name2
                            select new
                            {
                                m.Name2,
                                m.MemberType,
                                vbapp,
                                m.PeopleId,
                            };
                    if (q.Count() == 0)
                        continue;
                    StartPageSet(o);
                    foreach (var i in q)
                        AddRow(i.MemberType, i.Name2, i.PeopleId, i.vbapp, font);

                    if (t.Rows.Count > 1)
                        doc.Add(t);
                    else
                        doc.Add(new Phrase("no data"));
                }
            pageEvents.EndPageSet();
            doc.Close();
            Response.End();
        }

        private void StartPageSet(OrgInfo o)
        {
            t = new PdfPTable(4);
            t.WidthPercentage = 100;
            t.SetWidths(new int[] { 15, 30, 15, 40 });
            t.DefaultCell.Border = PdfPCell.NO_BORDER;
            pageEvents.StartPageSet(
                                    "{0}: {1}, {2} ({3})".Fmt(o.Division, o.Name, o.Location, o.Teacher),
                                    "({1})".Fmt(dt, o.OrgId));

            var boldfont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD);
            t.AddCell(new Phrase("PeopleId", boldfont));
            t.AddCell(new Phrase("Name", boldfont));
            t.AddCell(new Phrase("Member Type", boldfont));
            t.AddCell(new Phrase("Medical", boldfont));

        }
        private void AddRow(string Code, string name, int pid, VBSApp app, Font font)
        {
            t.AddCell(pid.ToString());
            t.AddCell(name);
            t.AddCell(Code);
            string med = "see registration card";
            if (app == null)
                med = "";
            else if (app.MedAllergy == false)
                med = "";
            else if (app.IsDocument == true)
            {
                var img = ImageData.DbUtil.Db.Images.SingleOrDefault(i => i.Id == app.ImgId);
                med = "";
                if (img != null && img.HasMedical())
                    med = img.Medical();
            }
            t.AddCell(med);
        }
        private class OrgInfo
        {
            public int OrgId { get; set; }
            public string Division { get; set; }
            public string Name { get; set; }
            public string Teacher { get; set; }
            public string Location { get; set; }
        }
        private IEnumerable<OrgInfo> list(int? orgid, int? divid, int? schedule)
        {
            var q = from o in DbUtil.Db.Organizations
                    where o.OrganizationId == orgid || orgid == 0 || orgid == null
                    where o.DivOrgs.Any(t => t.DivId == divid) || divid == 0 || divid == null
                    where o.ScheduleId == schedule || schedule == 0 || schedule == null
                    where o.OrganizationStatusId == (int)CmsData.Organization.OrgStatusCode.Active
                    let divorg = DbUtil.Db.DivOrgs.First(t => t.OrgId == o.OrganizationId && t.Division.Program.Name != DbUtil.MiscTagsString)
                    select new OrgInfo
                    {
                        OrgId = o.OrganizationId,
                        Division = divorg.Division.Name,
                        Name = o.OrganizationName,
                        Teacher = o.LeaderName,
                        Location = o.Location,
                    };
            return q;
        }
        class CellEvent : IPdfPCellEvent
        {
            public void CellLayout(PdfPCell cell, Rectangle pos, PdfContentByte[] canvases)
            {
                var cb = canvases[PdfPTable.BACKGROUNDCANVAS];
                cb.SetGrayStroke(0f);
                cb.SetLineWidth(.2f);
                cb.RoundRectangle(pos.Left + 4, pos.Bottom, pos.Width - 8, pos.Height - 4, 4);
                cb.Stroke();
                cb.ResetRGBColorStroke();
            }
        }
        class PageEvent : PdfPageEventHelper
        {
            private PdfTemplate npages;
            private PdfWriter writer;
            private Document document;
            private PdfContentByte dc;
            private BaseFont font;
            private string HeadText;
            private string HeadText2;

            public override void OnOpenDocument(PdfWriter writer, Document document)
            {
                this.writer = writer;
                this.document = document;
                base.OnOpenDocument(writer, document);
                font = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                dc = writer.DirectContent;
            }
            public void EndPageSet()
            {
                if (npages == null)
                    return;
                npages.BeginText();
                npages.SetFontAndSize(font, 8);
                npages.ShowText((writer.PageNumber + 1).ToString());
                npages.EndText();
            }
            public void StartPageSet(string header1, string header2)
            {
                EndPageSet();
                document.NewPage();
                document.ResetPageCount();
                this.HeadText = header1;
                this.HeadText2 = header2;
                npages = dc.CreateTemplate(50, 50);
            }
            public override void OnEndPage(PdfWriter writer, Document document)
            {
                base.OnEndPage(writer, document);

                string text;
                float len;

                //---Header left
                text = HeadText;
                const float HeadFontSize = 11f;
                len = font.GetWidthPoint(text, HeadFontSize);
                dc.BeginText();
                dc.SetFontAndSize(font, HeadFontSize);
                dc.SetTextMatrix(30, document.PageSize.Height - 30);
                dc.ShowText(text);
                dc.EndText();
                dc.BeginText();
                dc.SetFontAndSize(font, HeadFontSize);
                dc.SetTextMatrix(30, document.PageSize.Height - 30 - (HeadFontSize * 1.5f));
                dc.ShowText(HeadText2);
                dc.EndText();

                //---Column 1
                text = "Page " + (writer.PageNumber + 1) + " of ";
                len = font.GetWidthPoint(text, 8);
                dc.BeginText();
                dc.SetFontAndSize(font, 8);
                dc.SetTextMatrix(30, 30);
                dc.ShowText(text);
                dc.EndText();
                dc.AddTemplate(npages, 30 + len, 30);

                //---Column 2
                text = "Roster Report";
                len = font.GetWidthPoint(text, 8);
                dc.BeginText();
                dc.SetFontAndSize(font, 8);
                dc.SetTextMatrix(document.PageSize.Width / 2 - len / 2, 30);
                dc.ShowText(text);
                dc.EndText();

                //---Column 3
                text = Util.Now.ToShortDateString();
                len = font.GetWidthPoint(text, 8);
                dc.BeginText();
                dc.SetFontAndSize(font, 8);
                dc.SetTextMatrix(document.PageSize.Width - 30 - len, 30);
                dc.ShowText(text);
                dc.EndText();
            }
            private float PutText(string text, BaseFont font, float size, float x, float y)
            {
                dc.BeginText();
                dc.SetFontAndSize(font, size);
                dc.SetTextMatrix(x, y);
                dc.ShowText(text);
                dc.EndText();
                return font.GetWidthPoint(text, size);
            }
        }
    }
}
