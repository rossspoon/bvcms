/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Collections;
using CmsData;
using UtilityExtensions;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Diagnostics;
using System.Text;

namespace CmsWeb.Areas.Main.Models.Report
{
    public class OrgLeadersResult : ActionResult
    {
        public int? div, schedule, meetingid, orgid;
        public string name;

        public override void ExecuteResult(ControllerContext context)
        {
            var Response = context.HttpContext.Response;

            var list1 = ReportList(orgid, div, schedule, name);

            if (list1.Count() == 0)
            {
                Response.Write("no data found");
                return;
            }
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "filename=foo.pdf");

            doc = new Document(PageSize.LETTER, 36, 36, 36, 36);
            var w = PdfWriter.GetInstance(doc, Response.OutputStream);
            w.PageEvent = pageEvents;
            doc.Open();
            dc = w.DirectContent;

            var ctl = new RollsheetModel();
            foreach (var o in list1)
            {
                var t = StartPageSet(o);

                var color = Color.BLACK;
                foreach (var m in ctl.FetchOrgMembers(o.OrgId, null)
                    .Where(om => om.MemberTypeId != (int)OrganizationMember.MemberTypeCode.Member))
                {
                    if (color == Color.WHITE)
                        color = new GrayColor(240);
                    else
                        color = Color.WHITE;
                    AddRow(t,
                            m.PeopleId,
                            m.Name,
                            m.Email,
                            m.HomePhone,
                            m.CellPhone,
                            m.WorkPhone,
                            m.MemberType,
                            color);
                }
                doc.Add(t);
            }
            pageEvents.EndPageSet();
            doc.Close();
        }

        private Font boldfont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD);
        private Font font = FontFactory.GetFont(FontFactory.HELVETICA);
        private Font smallfont = FontFactory.GetFont(FontFactory.HELVETICA, 7);
        private Font medfont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
        private PageEvent pageEvents = new PageEvent();
        private Document doc;
        private PdfContentByte dc;

        float[] HeaderWids = new float[] { 12, 40, 25, 20 };

        private PdfPTable StartPageSet(OrgInfo o)
        {
            var t = new PdfPTable(HeaderWids);
            t.DefaultCell.SetLeading(2.0f, 1f);
            t.DefaultCell.Border = PdfPCell.NO_BORDER;
            t.WidthPercentage = 100;
            t.DefaultCell.Padding = 5;
            pageEvents.StartPageSet("Leader Report: {0} - {1} ({2})".Fmt(o.Division, o.Name, o.Teacher));

            t.AddCell(new Phrase("\nPeopleId", boldfont));
            t.AddCell(new Phrase("Name\nEmail", boldfont));
            t.AddCell(new Phrase("\nPhones", boldfont));
            t.AddCell(new Phrase("\nMember Type", boldfont));
            return t;
        }

        private void AddRow(PdfPTable t, int PeopleId, string Name, string Email, string HomePhone, string CellPhone, string WorkPhone, string MemberType, Color color)
        {
            t.DefaultCell.BackgroundColor = color;

            t.AddCell(new Phrase(PeopleId.ToString(), font));

            t.AddCell(new Phrase(Name + "\n" + Email, font));

            var sb = new StringBuilder();
            AddPhone(sb, HomePhone, "h ");
            AddPhone(sb, CellPhone, "c ");
            AddPhone(sb, WorkPhone, "w ");
            t.AddCell(new Phrase(sb.ToString(), font));

            t.AddCell(new Phrase(MemberType));
        }
        private void AddPhone(StringBuilder sb, string value, string prefix)
        {
            if (value.HasValue())
            {
                value = value.FmtFone(prefix);
                if (sb.Length > 0)
                    sb.Append("\n");
                sb.Append(value);
            }
        }
        private class OrgInfo
        {
            public int OrgId { get; set; }
            public string Division { get; set; }
            public string Name { get; set; }
            public string Teacher { get; set; }
            public string Location { get; set; }
        }
        private IEnumerable<OrgInfo> ReportList(int? orgid, int? divid, int? schedule, string name)
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
            public void StartPageSet(string header1)
            {
                EndPageSet();
                document.NewPage();
                document.ResetPageCount();
                this.HeadText = header1;
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
                //dc.BeginText();
                //dc.SetFontAndSize(font, HeadFontSize);
                //dc.SetTextMatrix(30, document.PageSize.Height - 30 - (HeadFontSize * 1.5f));
                //dc.ShowText(HeadText2);
                //dc.EndText();

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
                text = HeadText;
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


