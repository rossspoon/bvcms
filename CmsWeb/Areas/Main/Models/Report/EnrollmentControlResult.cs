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
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Diagnostics;

namespace CmsWeb.Areas.Main.Models.Report
{
    public class EnrollmentControlResult : ActionResult
    {
        public int div, subdiv, schedule;

        public override void ExecuteResult(ControllerContext context)
        {
            var Response = context.HttpContext.Response;

            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "filename=foo.pdf");
            var doc = new Document(PageSize.LETTER, 36, 36, 36, 42);
            var w = PdfWriter.GetInstance(doc, Response.OutputStream);
            w.PageEvent = new HeadFoot();

            string divtext = "", subdivtext = "";
            var divt = DbUtil.Db.Tags.SingleOrDefault(tag => tag.Id == div);
            if (divt != null)
                divtext = divt.Name;
            var subdivt = DbUtil.Db.Tags.SingleOrDefault(tag => tag.Id == subdiv);
            if (subdivt != null)
                subdivtext = subdivt.Name;

            string scheduletext = String.Empty;
            var sdt = CmsData.Organization.GetDateFromScheduleId(schedule);
            if (sdt.HasValue)
                scheduletext = sdt.Value.ToString("dddd h:mm tt");

            var headtext = "Enrollment Control for {0}:{1} {2}".Fmt(divtext, subdivtext, scheduletext);

            var boldfont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8);
            var header = new HeaderFooter(new Phrase(headtext, boldfont), false);
            header.Border = Rectangle.NO_BORDER;
            doc.Header = header;

            doc.Open();

            var t = new PdfPTable(4);
            t.HeaderRows = 1;
            t.WidthPercentage = 100;
            t.SetWidths(new int[] { 20, 30, 10, 15 });

            var font = FontFactory.GetFont(FontFactory.HELVETICA, 8);
            t.AddCell(new Phrase("Name", boldfont));
            t.AddCell(new Phrase("Organization", boldfont));
            t.AddCell(new Phrase("Location", boldfont));
            t.AddCell(new Phrase("Member Type", boldfont));

            foreach (var m in list(subdiv, div, schedule))
            {
                t.AddCell(new Phrase(m.Name, font));
                t.AddCell(new Phrase(m.Organization, font));
                t.AddCell(new Phrase(m.Location, font));
                t.AddCell(new Phrase(m.MemberType, font));
            }
            if (t.Rows.Count > 1)
                doc.Add(t);
            else
                doc.Add(new Phrase("no data"));
            doc.Close();
        }
        public class MemberInfo
        {
            public string Name { get; set; }
            public int Id { get; set; }
            public string Organization { get; set; }
            public string Location { get; set; }
            public string MemberType { get; set; }
        }
        IEnumerable<MemberInfo> list(int divid, int progid, int schedule)
        {
            var q = from m in DbUtil.Db.OrganizationMembers
                    where m.Organization.DivOrgs.Any(t => t.DivId == divid) || divid == 0
                    where m.Organization.DivOrgs.Any(t => t.Division.ProgId == progid)
                    where m.Organization.ScheduleId == schedule || schedule == 0
                    where m.Organization.OrganizationStatusId == (int)CmsData.Organization.OrgStatusCode.Active
                    orderby m.Person.Name2
                    select new MemberInfo
                    {
                        Name = m.Person.Name2,
                        Id = m.PeopleId,
                        Organization = m.Organization.OrganizationName,
                        Location = m.Organization.Location,
                        MemberType = m.MemberType.Description,
                    };
            return q;
        }
        class HeadFoot : PdfPageEventHelper
        {
            private PdfTemplate tpl;
            private PdfContentByte dc;
            private BaseFont font;

            public override void OnOpenDocument(PdfWriter writer, Document document)
            {
                base.OnOpenDocument(writer, document);
                font = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                dc = writer.DirectContent;
                tpl = dc.CreateTemplate(50, 50);
            }
            public override void OnEndPage(PdfWriter writer, Document document)
            {
                base.OnEndPage(writer, document);

                string sText;
                float fLen;

                //---Column 1: Title
                sText = "Enrollment Control";
                fLen = font.GetWidthPoint(sText, 8);
                dc.BeginText();
                dc.SetFontAndSize(font, 8);
                dc.SetTextMatrix(30, 30);
                dc.ShowText(sText);
                dc.EndText();

                //---Column 2: Date/Time
                sText = Util.Now.ToShortDateString();
                fLen = font.GetWidthPoint(sText, 8);
                dc.BeginText();
                dc.SetFontAndSize(font, 8);
                dc.SetTextMatrix(document.PageSize.Width / 2 - fLen / 2, 30);
                dc.ShowText(sText);
                dc.EndText();

                //---Column 3: Page Number
                sText = "Page " + writer.PageNumber + " of ";
                fLen = font.GetWidthPoint(sText, 8);
                dc.BeginText();
                dc.SetFontAndSize(font, 8);
                dc.SetTextMatrix(document.PageSize.Width - 90, 30);
                dc.ShowText(sText);
                dc.EndText();
                dc.AddTemplate(tpl, document.PageSize.Width - 90 + fLen, 30);
            }
            public override void OnCloseDocument(PdfWriter writer, Document document)
            {
                tpl.BeginText();
                tpl.SetFontAndSize(font, 8);
                tpl.ShowText((writer.PageNumber - 1).ToString());
                tpl.EndText();
                base.OnCloseDocument(writer, document);
            }
        }
    }
}

