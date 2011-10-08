using System;
using System.Collections.Generic;
using System.Xml;
using System.Web.Mvc;
using System.Xml.Linq;
using UtilityExtensions;
using System.Linq;
using CmsData;
using CmsData.Codes;

namespace CmsWeb.Models
{
    public class ClassesResult0 : ActionResult
    {
        private int thisday;
        private int campusid;
        private int page;
        private bool? kioskmode;
        private int familyid;
        private int? peopleid;
        private DateTime? bd;
        private int? grade;

        public bool noagecheck { get; set; }
        public ClassesResult0(bool? kioskmode, int id, int thisday, int campusid, int page, bool noagecheck)
        {
            this.thisday = thisday;
            this.campusid = campusid;
            var i = (from p in DbUtil.Db.People
                     where p.PeopleId == id
                     select new
                     {
                         p.FamilyId,
                         p.BirthDate,
                         p.Grade
                     }).SingleOrDefault();
            if (i == null)
                return;

            familyid = i.FamilyId;
            peopleid = id;
            bd = i.BirthDate;
            grade = i.Grade;

            this.page = page;
            this.noagecheck = noagecheck;
            this.kioskmode = kioskmode;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            if (!peopleid.HasValue)
                return;
            context.HttpContext.Response.ContentType = "text/xml";
            var settings = new XmlWriterSettings();
            settings.Encoding = new System.Text.UTF8Encoding(false);
            using (var w = XmlWriter.Create(context.HttpContext.Response.OutputStream, settings))
            {
                w.WriteStartElement("Results");
                w.WriteAttributeString("pid", peopleid.ToString());
                w.WriteAttributeString("fid", familyid.ToString());
                var q = DbUtil.Db.Organizations.AsQueryable();
                q = from o in q
                    let sc = o.OrgSchedules.FirstOrDefault() // SCHED
                    let Hour1 = DbUtil.Db.GetTodaysMeetingHours(o.OrganizationId, thisday).First().Hour
                    let bdaystart = o.BirthDayStart ?? DateTime.MaxValue
                    where bd == null || bd <= o.BirthDayEnd || o.BirthDayEnd == null || noagecheck
                    where bd == null || bd >= o.BirthDayStart || o.BirthDayStart == null || noagecheck
                    where o.CanSelfCheckin == true
                    where (o.ClassFilled ?? false) == false
                    where o.CampusId == campusid || campusid == 0
                    where o.OrganizationStatusId == OrgStatusCode.Active
                    where Hour1 != null
                    orderby sc.SchedTime.Value.TimeOfDay, bdaystart, o.OrganizationName
                    select o;

                var count = q.Count();
                const int INT_PageSize = 10;
                var startrow = (page - 1) * INT_PageSize;
                if (count > startrow + INT_PageSize)
                    w.WriteAttributeString("next", (page + 1).ToString());
                else
                    w.WriteAttributeString("next", "");
                if (page > 1)
                    w.WriteAttributeString("prev", (page - 1).ToString());
                else
                    w.WriteAttributeString("prev", "");

                var q2 = from o in q
                         let sc = o.OrgSchedules.FirstOrDefault() // SCHED
                         select new
                         {
                             o.LeaderName,
                             o.OrganizationId,
                             o.Location,
                             o.BirthDayStart,
                             o.BirthDayEnd,
                             sc.MeetingTime,
                             o.OrganizationName,
                             o.NumCheckInLabels,
                             o.Limit,
                             MemberCount = o.OrganizationMembers.Count(om =>
                                 om.MemberTypeId == MemberTypeCode.Member
                                 && om.Pending != true
                                 ),
                         };

                foreach (var o in q2.Skip(startrow).Take(INT_PageSize))
                {
                    w.WriteStartElement("class");
                    w.WriteAttributeString("orgid", o.OrganizationId.ToString());
                    var loc = o.Location;
                    if (loc.HasValue())
                        loc = ", " + loc;
                    var leader = o.LeaderName;
                    if (leader.HasValue())
                        leader = ":" + leader;
                    var bdays = " [{0:M/d/yy}-{1:M/d/yy}]".Fmt(o.BirthDayStart, o.BirthDayEnd);
                    if (bdays == " [-]")
                        bdays = null;
                    string display = null;
                    if (kioskmode == true)
                        display = "{0:hh:mm tt} {1}{2}{3}({4},{5})"
                            .Fmt(o.MeetingTime, o.OrganizationName, leader, loc, o.Limit, o.MemberCount);
                    else
                        display = "{0:hh:mm tt} {1}{2}{3}{4}"
                            .Fmt(o.MeetingTime, o.OrganizationName, leader, loc, bdays);
                    w.WriteAttributeString("display", display);
                    w.WriteAttributeString("nlabels", o.NumCheckInLabels.ToString());
                    w.WriteEndElement();
                }
                w.WriteEndElement();
            }
        }
    }
}