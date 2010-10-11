using System;
using System.Collections.Generic;
using System.Xml;
using System.Web.Mvc;
using System.Xml.Linq;
using UtilityExtensions;
using System.Linq;
using CmsData;

namespace CmsWeb.Models
{
    public class ClassesResult : ActionResult
    {
        private int thisday;
        private int campusid;
        private int familyid;
        private int? peopleid;
        private DateTime? bd;
        private int? grade;

        public bool noagecheck { get; set; }
        public ClassesResult(int id, int thisday, int campusid, bool noagecheck)
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

            this.noagecheck = noagecheck;
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
                //if (kioskmode == true)
                //    q = from o in q
                //        let bdaystart = o.BirthDayStart ?? DateTime.MaxValue
                //        where bd == null || bd <= o.BirthDayEnd || o.BirthDayEnd == null || noagecheck
                //        where bd == null || bd >= o.BirthDayStart || o.BirthDayStart == null || noagecheck
                //        where grade <= o.GradeAgeEnd || o.GradeAgeEnd == null || noagecheck
                //        where grade >= o.GradeAgeStart || o.GradeAgeStart == null || noagecheck
                //        where o.AllowKioskRegister == true
                //        where (o.ClassFilled ?? false) == false
                //        where o.CampusId == campusid || campusid == 0
                //        where o.OrganizationStatusId == (int)CmsData.Organization.OrgStatusCode.Active
                //        orderby bdaystart, o.OrganizationName
                //        select o;
                //else
                q = from o in q
                    let Hour1 = DbUtil.Db.GetTodaysMeetingHour(o.OrganizationId, thisday)
                    let bdaystart = o.BirthDayStart ?? DateTime.MaxValue
                    where bd == null || bd <= o.BirthDayEnd || o.BirthDayEnd == null || noagecheck
                    where bd == null || bd >= o.BirthDayStart || o.BirthDayStart == null || noagecheck
                    where o.CanSelfCheckin == true
                    where (o.ClassFilled ?? false) == false
                    where o.CampusId == campusid || campusid == 0
                    where o.OrganizationStatusId == (int)CmsData.Organization.OrgStatusCode.Active
                    where Hour1 != null
                    orderby o.SchedTime.Value.TimeOfDay, bdaystart, o.OrganizationName
                    select o;

                var q2 = from o in q
                         select new
                         {
                             o.LeaderName,
                             o.OrganizationId,
                             o.Location,
                             o.BirthDayStart,
                             o.BirthDayEnd,
                             o.MeetingTime,
                             o.OrganizationName,
                             o.NumCheckInLabels,
                             o.Limit,
                             MemberCount = o.OrganizationMembers.Count(om =>
                                 om.MemberTypeId == (int)OrganizationMember.MemberTypeCode.Member
                                 && om.Pending != true
                                 ),
                         };

                foreach (var o in q2)
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
                    w.WriteAttributeString("display", "{0:hh:mm tt} {1}{2}{3}{4}"
                            .Fmt(o.MeetingTime, o.OrganizationName, leader, loc, bdays));
                    w.WriteAttributeString("nlabels", o.NumCheckInLabels.ToString());
                    w.WriteEndElement();
                }
                w.WriteEndElement();
            }
        }
    }
}