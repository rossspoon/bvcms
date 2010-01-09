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
    public partial class Rollsheet : System.Web.UI.Page
    {
        public class MemberInfo
        {
            public string Name { get; set; }
            public int Id { get; set; }
            public string Organization { get; set; }
            public string Location { get; set; }
            public string MemberType { get; set; }
        }

        private PdfPCell box;
        private Font boldfont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD);
        private Font font = FontFactory.GetFont(FontFactory.HELVETICA);
        private Font smallfont = FontFactory.GetFont(FontFactory.HELVETICA, 7);
        private Font medfont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
        private PageEvent pageEvents = new PageEvent();
        private PdfPTable t;
        private Document doc;
        private DateTime? dt;
        private PdfContentByte dc;

        protected void Page_Load(object sender, EventArgs e)
        {
            int? org = null;
            int? group = null;
            if (this.QueryString<string>("org") == "curr")
            {
                org = Util.CurrentOrgId;
                group = Util.CurrentGroupId;
            }
            var div = this.QueryString<int?>("div");
            var schedule = this.QueryString<int?>("schedule");
            var name = this.QueryString<string>("name");
            dt = this.QueryString<DateTime?>("dt");

            var mid = this.QueryString<int?>("meetingid");


            CmsData.Meeting meeting = null;
            if (mid.HasValue)
            {
                meeting = DbUtil.Db.Meetings.Single(mt => mt.MeetingId == mid);
                dt = meeting.MeetingDate;
                org = meeting.OrganizationId;
            }
            var list1 = list(org, group, div, schedule, name);
            if (list1.Count() == 0)
            {
                Response.Write("no data found");
                return;
            }

            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "filename=foo.pdf");

            doc = new Document(PageSize.LETTER.Rotate(), 36, 36, 64, 64);
            var w = PdfWriter.GetInstance(doc, Response.OutputStream);
            w.PageEvent = pageEvents;
            doc.Open();
            dc = w.DirectContent;

            box = new PdfPCell();
            box.Border = PdfPCell.NO_BORDER;
            box.CellEvent = new CellEvent();

            var ctl = new RollsheetController();
            foreach (var o in list1)
            {
                var mct = StartPageSet(o);

                if (meeting != null)
                {
                    var q = from at in meeting.Attends
                            where at.AttendanceFlag == true || at.Registered == true
                            orderby at.Person.Name2
                            select at;
                    foreach (var a in q)
                        AddRow(a.MemberType.Code, a.Person.Name2, a.PeopleId, a.Person.DOB, font);
                }
                else
                    foreach (var m in ctl.FetchOrgMembers(o.OrgId, group))
                        AddRow(m.MemberTypeCode, m.Name2, m.PeopleId, m.BirthDate, font);
                if (!group.HasValue && meeting == null)
                {
                    foreach (var m in ctl.FetchVisitors(o.OrgId, dt.Value))
                        AddRow(m.VisitorType, m.Name2, m.PeopleId, m.BirthDate, boldfont);
                }
                if (t.Rows.Count > 0)
                    mct.AddElement(t);
                else
                    mct.AddElement(new Phrase("no data"));
                doc.Add(mct);
            }
            pageEvents.EndPageSet();
            doc.Close();
            Response.End();
        }

        private MultiColumnText StartPageSet(OrgInfo o)
        {
            var mct = new MultiColumnText();
            mct.AddRegularColumns(doc.Left, doc.Right, 20f, 2);
            t = new PdfPTable(4);
            t.WidthPercentage = 100;
            t.SetWidths(new int[] { 30, 4, 6, 30 });
            t.DefaultCell.Border = PdfPCell.NO_BORDER;
            pageEvents.StartPageSet(
                                    "{0}: {1}, {2} ({3})".Fmt(o.Division, o.Name, o.Location, o.Teacher),
                                    "{0:dddd M/d/yy h:mm tt} ({1})".Fmt(dt, o.OrgId),
                                    "M.{0}.{1:MMddyyHHmm}".Fmt(o.OrgId, dt));
            return mct;
        }
        private void AddRow(string Code, string name, int pid, string dob, Font font)
        {
            var bc = new Barcode39();
            bc.X = 1.2f;
            bc.Font = null;
            bc.Code = pid.ToString();
            var img = bc.CreateImageWithBarcode(dc, null, null);
            var c = new PdfPCell(img, false);
            c.PaddingTop = 3f;
            c.Border = PdfPCell.NO_BORDER;
            c.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            t.AddCell(c);

            t.AddCell("");
            t.AddCell(box);

            var bd = DateTime.Parse(dob);

            var p = new Phrase(name + "\n", font);
            p.Add(new Chunk(" ", medfont));
            p.Add(new Chunk("({0}) {1:MMM d}".Fmt(Code, bd), smallfont));
            t.AddCell(p);
        }
        private class OrgInfo
        {
            public int OrgId { get; set; }
            public string Division { get; set; }
            public string Name { get; set; }
            public string Teacher { get; set; }
            public string Location { get; set; }
        }
        private IEnumerable<OrgInfo> list(int? orgid, int? groupid, int? divid, int? schedule, string name)
        {
            var q = from o in DbUtil.Db.Organizations
                    where o.OrganizationId == orgid || orgid == 0 || orgid == null
                    where o.DivOrgs.Any(t => t.DivId == divid) || divid == 0 || divid == null
                    where o.ScheduleId == schedule || schedule == 0 || schedule == null
                    where o.OrganizationStatusId == (int)CmsData.Organization.OrgStatusCode.Active
                    where o.OrganizationName.Contains(name) || o.LeaderName.Contains(name) || name == "" || name == null
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
            private string Barcode;

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
            public void StartPageSet(string header1, string header2, string barcode)
            {
                EndPageSet();
                document.NewPage();
                document.ResetPageCount();
                this.HeadText = header1;
                this.HeadText2 = header2;
                this.Barcode = barcode;
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

                //---Barcode right
                var bc = new Barcode39();
                bc.Font = null;
                bc.Code = Barcode;
                bc.X = 1f;
                var img = bc.CreateImageWithBarcode(dc, null, null);
                var h = font.GetAscentPoint(text, HeadFontSize);
                img.SetAbsolutePosition(document.PageSize.Width - img.Width - 30, document.PageSize.Height - 30 - img.Height + h);
                dc.AddImage(img);

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
                text = "Attendance Rollsheet";
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
