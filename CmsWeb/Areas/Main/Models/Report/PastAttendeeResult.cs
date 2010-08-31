/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using CmsData;
using UtilityExtensions;
using CMSPresenter;
using System.Text;
using System.Web.Mvc;
using System.Collections.Generic;
using CmsWeb.Models;

namespace CmsWeb.Areas.Main.Models.Report
{
    public class PastAttendeeResult : ActionResult
    {
        public class AttendInfo
        {
            public int PeopleId { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public string Address2 { get; set; }
            public string Email { get; set; }
            public string Birthday { get; set; }
            public string HomePhone { get; set; }
            public string CellPhone { get; set; }
            public string CSZ { get; set; }
            public bool visitor { get; set; }
            public DateTime? LastAttend { get; set; }
            public decimal? AttendPct { get; set; }
            public string AttendStr { get; set; }
            public string AttendType { get; set; }
        }
        private Font monofont = FontFactory.GetFont(FontFactory.COURIER, 8);
        private Font boldfont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
        private Font bigboldfont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
        private Font font = FontFactory.GetFont(FontFactory.HELVETICA, 10);
        private Font smallfont = FontFactory.GetFont(FontFactory.HELVETICA, 8, new GrayColor(50));
        private Document doc;
        private DateTime dt;

        private int? orgid;
        public PastAttendeeResult(int? id)
        {
            orgid = id;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var Response = context.HttpContext.Response;
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "filename=foo.pdf");

            dt = Util.Now;

            doc = new Document(PageSize.LETTER.Rotate(), 36, 36, 64, 64);
            var w = PdfWriter.GetInstance(doc, Response.OutputStream);

            var i = (from o in DbUtil.Db.Organizations
                     where o.OrganizationId == orgid
                     select new
                     {
                         o.OrganizationName,
                         o.LeaderName,
                     }).SingleOrDefault();

            w.PageEvent = new HeadFoot
            {
                HeaderText = "Recent Attendee Report: {0} - {1}".Fmt(
                    i.OrganizationName, i.LeaderName),
                FooterText = "Recent Attendee Report"
            };
            doc.Open();

            var q = Attendees(orgid.Value);

            if (!orgid.HasValue || i == null || q.Count() == 0)
                doc.Add(new Phrase("no data"));
            else
            {
                var mt = new PdfPTable(1);
                mt.SetNoPadding();
                mt.HeaderRows = 1;

                float[] widths = new float[] { 4f, 6f, 7f, 2.6f, 2f, 3f };
                var t = new PdfPTable(widths);
                t.DefaultCell.Border = PdfPCell.NO_BORDER;
                t.DefaultCell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                t.DefaultCell.SetLeading(2.0f, 1f);
                t.WidthPercentage = 100;

                t.AddHeader("Name", boldfont);
                t.AddHeader("Address", boldfont);
                t.AddHeader("Phone/Email", boldfont);
                t.AddHeader("Last Att.", boldfont);
                t.AddHeader("Birthday", boldfont);
                t.AddHeader("Status", boldfont);
                mt.AddCell(t);

                var color = Color.BLACK;
                bool? v = null;

                foreach (var p in q)
                {
                    if (color == Color.WHITE)
                        color = new GrayColor(240);
                    else
                        color = Color.WHITE;

                    t = new PdfPTable(widths);
                    t.SetNoBorder();
                    t.DefaultCell.VerticalAlignment = Element.ALIGN_TOP;
                    t.DefaultCell.BackgroundColor = color;

                    if (v != p.visitor)
                        t.Add("             ------ {0} ------".Fmt(p.visitor == true ? "Visitors" : "Members"), 6, bigboldfont);
                    v = p.visitor;

                    t.Add(p.Name, font);

                    var ph = new Paragraph();
                    ph.AddLine(p.Address, font);
                    ph.AddLine(p.Address2, font);
                    ph.AddLine(p.CSZ, font);
                    t.AddCell(ph);

                    ph = new Paragraph();
                    ph.AddLine(p.HomePhone.FmtFone("H"), font);
                    ph.AddLine(p.CellPhone.FmtFone("C"), font);
                    ph.AddLine(p.Email, font);
                    t.AddCell(ph);

                    t.Add(p.LastAttend.FormatDate(), font);
                    t.Add(p.Birthday, font);
                    t.Add(p.AttendType, font);
                    t.CompleteRow();

                    t.Add("", font);
                    t.Add(p.AttendStr, 4, monofont);
                    t.AddRight("{0:n1}{1}".Fmt(p.AttendPct, p.AttendPct.HasValue ? "%" : ""), font);

                    mt.AddCell(t);
                }
                doc.Add(mt);
            }
            doc.Close();
            Response.End();
        }
        public static IEnumerable<AttendInfo> Attendees(int orgid)
        {
            var q = from a in DbUtil.Db.Attends
                    where a.Meeting.OrganizationId == orgid
                    where a.MeetingDate >= a.Organization.FirstMeetingDate
                    where a.EffAttendFlag == true
                    group a by a.Person into g
                    let a = g.OrderByDescending(aa => aa.MeetingDate).First()
                    let p = a.Person
                    let lastattend = a.MeetingDate
                    let attendpct = a.Organization.OrganizationMembers
                                    .Where(aa => aa.PeopleId == a.PeopleId)
                                    .Select(aa => aa.AttendPct)
                                    .SingleOrDefault()
                    let attendstr = a.Organization.OrganizationMembers
                                    .Where(aa => aa.PeopleId == a.PeopleId)
                                    .Select(aa => aa.AttendStr)
                                    .SingleOrDefault()
                    let visitor = !a.Organization.OrganizationMembers.Any(m => m.PeopleId == p.PeopleId)
                    orderby !visitor, lastattend descending

                    select new AttendInfo
                    {
                        PeopleId = p.PeopleId,
                        Name = p.Name,
                        Address = p.PrimaryAddress,
                        Birthday = p.DOB.ToDate().ToString2("M/d"),
                        Email = p.EmailAddress,
                        HomePhone = p.HomePhone,
                        CellPhone = p.CellPhone,
                        CSZ = Util.FormatCSZ4(p.PrimaryCity, p.PrimaryState, p.PrimaryZip),
                        visitor = visitor,
                        LastAttend = lastattend,
                        AttendPct = attendpct,
                        AttendStr = attendstr,
                        AttendType = a.AttendType.Description
                    };
            return q;
        }
   }
}

