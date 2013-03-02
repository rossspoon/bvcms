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
using CmsData.Codes;

namespace CmsWeb.Areas.Main.Models.Report
{
    public class RollsheetResult : ActionResult
    {
        public class PersonVisitorInfo
        {
            public int PeopleId { get; set; }
            public string Name2 { get; set; }
            public string BirthDate { get; set; }
            public DateTime? LastAttended { get; set; }
            public string NameParent1 { get; set; }
            public string NameParent2 { get; set; }
            public string VisitorType { get; set; }
        }
        public int? qid, pid, div, schedule, meetingid, orgid;
        public int[] groups;
        public bool? bygroup;
        public bool? altnames;
        public string name, sgprefix, highlightsg;
        public DateTime? dt;

        public override void ExecuteResult(ControllerContext context)
        {
            var Response = context.HttpContext.Response;

            CmsData.Meeting meeting = null;
            if (meetingid.HasValue)
            {
                meeting = DbUtil.Db.Meetings.Single(mt => mt.MeetingId == meetingid);
                dt = meeting.MeetingDate;
                orgid = meeting.OrganizationId;
            }

            IEnumerable<OrgInfo> list1;
            if (bygroup == true)
                list1 = ReportList2(orgid, pid, div, schedule, name, sgprefix);
            else
                list1 = ReportList(orgid, groups, pid, div, schedule, name);

            if (list1.Count() == 0)
            {
                Response.Write("no data found");
                return;
            }
            if (!dt.HasValue)
            {
                Response.Write("bad date");
                return;
            }
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

            foreach (var o in list1)
            {

                if (meeting != null)
                {
                    var Groups = o.Groups;
                    if (Groups[0] == 0)
                    {
                        var q = from at in meeting.Attends
                                where at.AttendanceFlag == true || at.Commitment == AttendCommitmentCode.Attending || at.Commitment == AttendCommitmentCode.Substitute
                                orderby at.Person.LastName, at.Person.FamilyId, at.Person.Name2
                                select new
                                           {
                                               at.MemberType.Code,
                                               Name2 = (altnames == true && at.Person.AltName != null && at.Person.AltName.Length > 0) ? at.Person.AltName : at.Person.Name2,
                                               at.PeopleId,
                                               at.Person.DOB,
                                           };
                        if (q.Count() > 0)
                            StartPageSet(o);
                        foreach (var a in q)
                            AddRow(a.Code, a.Name2, a.PeopleId, a.DOB, "", font);
                    }
                    else
                    {
                        var q = from at in meeting.Attends
                                let om =
                                    at.Organization.OrganizationMembers.SingleOrDefault(mm => mm.PeopleId == at.PeopleId)
                                let gc = om.OrgMemMemTags.Count(mt => Groups.Contains(mt.MemberTagId))
                                where gc == Groups.Length || Groups[0] <= 0
                                where gc > 0
                                where !Groups.Contains(-1) || (Groups.Contains(-1) && om.OrgMemMemTags.Count() == 0)
                                where
                                    at.AttendanceFlag == true || at.Commitment == AttendCommitmentCode.Attending ||
                                    at.Commitment == AttendCommitmentCode.Substitute
                                orderby at.Person.LastName, at.Person.FamilyId, at.Person.Name2
                                select new
                                           {
                                               at.MemberType.Code,
                                               Name2 = (altnames == true && at.Person.AltName != null && at.Person.AltName.Length > 0) ? at.Person.AltName : at.Person.Name2,
                                               at.PeopleId,
                                               at.Person.DOB,
                                           };
                        if (q.Count() > 0)
                            StartPageSet(o);
                        foreach (var a in q)
                            AddRow(a.Code, a.Name2, a.PeopleId, a.DOB, "", font);
                        
                    }
                }
                else
                {
                    var Groups = o.Groups;
                    if (Groups == null)
                        Groups = new int[] { 0 };
                    var q = from om in DbUtil.Db.OrganizationMembers
                            where om.OrganizationId == o.OrgId
                            let gc = om.OrgMemMemTags.Count(mt => Groups.Contains(mt.MemberTagId))
                            where gc == Groups.Length || Groups[0] <= 0
                            where !Groups.Contains(-1) || (Groups.Contains(-1) && om.OrgMemMemTags.Count() == 0)
                            where (om.Pending ?? false) == false
                            where om.MemberTypeId != MemberTypeCode.InActive
                            where om.EnrollmentDate <= Util.Now
                            orderby om.Person.LastName, om.Person.FamilyId, om.Person.Name2
                            let p = om.Person
                            let ch = altnames == true && p.AltName != null && p.AltName.Length > 0
                            select new
                            {
                                PeopleId = p.PeopleId,
                                Name2 = ch ? p.AltName : p.Name2,
                                BirthDate = Util.FormatBirthday(
                                    p.BirthYear,
                                    p.BirthMonth,
                                    p.BirthDay),
                                MemberTypeCode = om.MemberType.Code,
                                ch,
                                highlight = om.OrgMemMemTags.Any(mm => mm.MemberTag.Name == highlightsg) ? highlightsg : ""
                            };
                    if (q.Count() > 0)
                        StartPageSet(o);
                    foreach (var m in q)
                        AddRow(m.MemberTypeCode, m.Name2, m.PeopleId, m.BirthDate, m.highlight, m.ch ? china : font);
                }

                if (bygroup == false && groups[0] == 0 && meeting == null)
                {
                    if (t == null)
                        StartPageSet(o);
                    foreach (var m in RollsheetModel.FetchVisitors(o.OrgId, dt.Value, NoCurrentMembers: true))
                        AddRow(m.VisitorType, m.Name2, m.PeopleId, m.BirthDate, "", boldfont);
                    var wks = 3; // default lookback
                    var org = DbUtil.Db.Organizations.Single(oo => oo.OrganizationId == o.OrgId);
                    if (org.RollSheetVisitorWks.HasValue)
                        wks = org.RollSheetVisitorWks.Value;
                    var MeetingDate = dt.Value.AddDays(wks * -7);

                    var q = from p in DbUtil.Db.People
                            where p.Attends.Any(a => a.AttendanceFlag == true
                                                     && (a.MeetingDate >= dt && a.MeetingDate <= MeetingDate)
                                                     && a.OrganizationId == orgid
                                                     && VisitAttendTypes.Contains(a.AttendanceTypeId.Value)
                                                     && a.MeetingDate >= a.Organization.FirstMeetingDate)
                            let ch = altnames == true && p.AltName != null && p.AltName.Length > 0
                            where !p.OrganizationMembers.Any(om => om.OrganizationId == orgid)
                            orderby p.Name2, p.Name
                            orderby p.LastName, p.FamilyId, p.Name2
                            select new PersonVisitorInfo
                                       {
                                           VisitorType = p.MemberStatusId == (int)MemberStatusCode.Member ? "VM" : "VS",
                                           PeopleId = p.PeopleId,
                                           Name2 = ch ? p.AltName : p.Name2,
                                           BirthDate = Util.FormatBirthday(
                                               p.BirthYear,
                                               p.BirthMonth,
                                               p.BirthDay),
                                       };
                    foreach (var m in q)
                        AddRow(m.VisitorType, m.Name2, m.PeopleId, m.BirthDate, "", boldfont);
                }
                if (t == null)
                    continue;
                var col = 0;
                var ct = new ColumnText(dc);
                float gutter = 20f;
                float colwidth = (doc.Right - doc.Left - gutter) / 2;
                leftcol = new Rectangle(doc.Left, doc.Bottom, doc.Left + colwidth, doc.Top);
                rightcol = new Rectangle(doc.Right - colwidth, doc.Bottom, doc.Right, doc.Top);

                if (t.Rows.Count == 0)
                {
                    ct.SetSimpleColumn(leftcol);
                    ct.AddElement(new Phrase("no data"));
                    ct.Go();
                }
                else
                {
                    ct.AddElement(t);
                    var status = 0;
                    while (ColumnText.HasMoreText(status))
                    {
                        if (col == 0)
                            ct.SetSimpleColumn(leftcol);
                        else if (col == 1)
                            ct.SetSimpleColumn(rightcol);
                        status = ct.Go();
                        ++col;
                        if (col > 1)
                        {
                            col = 0;
                            doc.NewPage();
                        }
                    }
                }
            }
            Response.Flush();
            doc.Close();
        }
        private static int[] VisitAttendTypes = new int[] 
        { 
            AttendTypeCode.VisitingMember,
            AttendTypeCode.RecentVisitor, 
            AttendTypeCode.NewVisitor 
        };

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
        private Font china = null;
        private PageEvent pageEvents = new PageEvent();
        private PdfPTable t;
        private Document doc;
        private PdfContentByte dc;
        private Rectangle leftcol;
        private Rectangle rightcol;


        private void StartPageSet(OrgInfo o)
        {
            if (altnames == true)
            {
                BaseFont.AddToResourceSearch(HttpContext.Current.Server.MapPath("/iTextAsian.dll"));
                var bfchina = BaseFont.CreateFont("MHei-Medium",
                    "UniCNS-UCS2-H", BaseFont.EMBEDDED);
                china = new Font(bfchina, 12, Font.NORMAL);
            }
            t = new PdfPTable(4);
            t.WidthPercentage = 100;
            t.SetWidths(new int[] { 30, 4, 6, 30 });
            t.DefaultCell.Border = PdfPCell.NO_BORDER;
            pageEvents.StartPageSet(
                                    "{0}: {1}, {2} ({3})".Fmt(o.Division, o.Name, o.Location, o.Teacher),
                                    "{0:f} ({1})".Fmt(dt, o.OrgId),
                                    "M.{0}.{1:MMddyyHHmm}".Fmt(o.OrgId, dt));
        }
        private void AddRow(string Code, string name, int pid, string dob, string highlight, Font font)
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

            DateTime bd;
            DateTime.TryParse(dob, out bd);

            var p = new Phrase(name, font);
            p.Add("\n");
            p.Add(new Chunk(" ", medfont));
            p.Add(new Chunk("({0}) {1:MMM d}".Fmt(Code, bd), smallfont));
            if (highlight.HasValue())
                p.Add("\n" + highlight);
            t.AddCell(p);
        }
        private class OrgInfo
        {
            public int OrgId { get; set; }
            public string Division { get; set; }
            public string Name { get; set; }
            public string Teacher { get; set; }
            public string Location { get; set; }
            public int[] Groups { get; set; }
        }
        private IEnumerable<OrgInfo> ReportList(int? orgid, int[] groups, int? progid, int? divid, int? schedule, string name)
        {
            var roles = DbUtil.Db.CurrentRoles();
            var q = from o in DbUtil.Db.Organizations
                    where o.LimitToRole == null || roles.Contains(o.LimitToRole)
                    where o.OrganizationId == orgid || orgid == 0 || orgid == null
                    where o.DivOrgs.Any(t => t.Division.ProgDivs.Any(p => p.ProgId == progid)) || progid == 0 || progid == null
                    where o.DivOrgs.Any(t => t.DivId == divid) || divid == 0 || divid == null
                    where o.OrgSchedules.Any(sc => sc.ScheduleId == schedule) || schedule == 0 || schedule == null
                    where o.OrganizationStatusId == OrgStatusCode.Active
                    where o.OrganizationName.Contains(name) || o.LeaderName.Contains(name) || o.Location.Contains(name) || name == "" || name == null
                    orderby o.Division.Name, o.OrganizationName
                    select new OrgInfo
                    {
                        OrgId = o.OrganizationId,
                        Division = o.Division.Name,
                        Name = o.OrganizationName,
                        Teacher = o.LeaderName,
                        Location = o.Location,
                        Groups = groups
                    };
            return q;
        }
        private IEnumerable<OrgInfo> ReportList2(int? orgid, int? progid, int? divid, int? schedule, string name, string sgprefix)
        {
            var roles = DbUtil.Db.CurrentRoles();
            var q = from o in DbUtil.Db.Organizations
                    where o.LimitToRole == null || roles.Contains(o.LimitToRole)
                    from sg in o.MemberTags
                    where sgprefix == null || sgprefix == "" || sg.Name.StartsWith(sgprefix)
                    where o.OrganizationId == orgid || orgid == 0 || orgid == null
                    where o.DivOrgs.Any(t => t.Division.ProgDivs.Any(p => p.ProgId == progid)) || progid == 0 || progid == null
                    where o.DivOrgs.Any(t => t.DivId == divid) || divid == 0 || divid == null
                    where o.OrgSchedules.Any(sc => sc.ScheduleId == schedule) || schedule == 0 || schedule == null
                    where o.OrganizationStatusId == OrgStatusCode.Active
                    where o.OrganizationName.Contains(name) || o.LeaderName.Contains(name) || name == "" || name == null
                    select new OrgInfo
                    {
                        OrgId = o.OrganizationId,
                        Division = o.OrganizationName,
                        Name = sg.Name,
                        Teacher = "",
                        Location = o.Location,
                        Groups = new int[] { sg.Id }
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
            class NPages
            {
                public NPages(PdfContentByte dc)
                {
                    template = dc.CreateTemplate(50, 50);
                }
                public bool juststartednewset;
                public PdfTemplate template;
                public int n;
            }
            private NPages npages;
            private int pg;

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
                npages = new NPages(dc);
            }
            public void EndPageSet()
            {
                if (npages == null)
                    return;
                npages.template.BeginText();
                npages.template.SetFontAndSize(font, 8);
                npages.template.ShowText(npages.n.ToString());
                pg = 1;
                npages.template.EndText();
                npages = new NPages(dc);
            }
            public void StartPageSet(string header1, string header2, string barcode)
            {
                document.NewPage();
                this.HeadText = header1;
                this.HeadText2 = header2;
                this.Barcode = barcode;
                npages.juststartednewset = true;
            }
            public override void OnEndPage(PdfWriter writer, Document document)
            {
                base.OnEndPage(writer, document);
                if (npages.juststartednewset)
                    EndPageSet();

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
                bc.X = 1.2f;
                var img = bc.CreateImageWithBarcode(dc, null, null);
                var h = font.GetAscentPoint(text, HeadFontSize);
                img.SetAbsolutePosition(document.PageSize.Width - img.Width - 30, document.PageSize.Height - 30 - img.Height + h);
                dc.AddImage(img);

                //---Column 1
                text = "Page " + (pg) + " of ";
                len = font.GetWidthPoint(text, 8);
                dc.BeginText();
                dc.SetFontAndSize(font, 8);
                dc.SetTextMatrix(30, 30);
                dc.ShowText(text);
                dc.EndText();
                dc.AddTemplate(npages.template, 30 + len, 30);
                npages.n = pg++;

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
            public override void OnCloseDocument(PdfWriter writer, Document document)
            {
                base.OnCloseDocument(writer, document);
                EndPageSet();
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


