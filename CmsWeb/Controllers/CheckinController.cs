using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CmsData;
using UtilityExtensions;

namespace CMSWeb.Controllers
{
    public class CheckinController : CMSWebCommon.Controllers.CmsController
    {
        public int? thisday { get; set; }

        public ActionResult Match(string id, int? campus, int? thisday)
        {
            NoCache();
            this.thisday = thisday;

            var ph = Util.GetDigits(id).PadLeft(10, '0');
            var p7 = ph.Substring(3);
            var ac = ph.Substring(0, 3);
            var q1 = from f in DbUtil.Db.Families
                     where f.HeadOfHousehold.DeceasedDate == null
                     where f.HomePhoneLU.StartsWith(p7)
                        || f.HeadOfHousehold.CellPhoneLU.StartsWith(p7)
                        || f.HeadOfHouseholdSpouse.CellPhoneLU.StartsWith(p7)
                     select new Family
                     {
                         FamilyId = f.FamilyId,
                         AreaCode = f.HomePhoneAC,
                         Name = f.HeadOfHousehold.Name
                     };
            var matches = q1.ToList();
            if (matches.Count > 1)
                matches = matches.Where(m => m.AreaCode == ac || ac == "000").ToList();
            if (matches.Count == 0)
                return ShowFamily(0, campus); // not found
            if (matches.Count == 1)
                return ShowFamily(matches[0].FamilyId, campus);
            return View("Multiple", matches);
        }
        public ActionResult Family(int id, int? campus, int? thisday)
        {
            NoCache();
            this.thisday = thisday;
            return ShowFamily(id, campus);
        }
        private ActionResult ShowFamily(int id, int? campus)
        {
            var now = DateTime.Now;
            // get org members first
            var members =
                from om in DbUtil.Db.OrganizationMembers
                let Hour = DbUtil.Db.GetTodaysMeetingHour(om.OrganizationId, thisday)
                let CheckedIn = DbUtil.Db.GetAttendedTodaysMeeting(om.OrganizationId, thisday, om.PeopleId)
                where om.Organization.CanSelfCheckin.Value
                where om.Organization.CampusId == campus || campus == null
                where om.Person.FamilyId == id
                where om.Person.DeceasedDate == null
                where Hour != null
                select new Attendee
                {
                    Id = om.PeopleId,
                    Name = om.Person.Name,
                    BYear = om.Person.BirthYear,
                    BMon = om.Person.BirthMonth,
                    BDay = om.Person.BirthDay,
                    Class = om.Organization.OrganizationName,
                    Leader = om.Organization.LeaderName,
                    OrgId = om.OrganizationId,
                    Location = om.Organization.Location,
                    Age = om.Person.Age ?? 0,
                    Gender = om.Person.Gender.Code,
                    NumLabels = om.Organization.NumCheckInLabels ?? 1,
                    Hour = Hour,
                    CheckedIn = CheckedIn ?? false
                };
                    
            // now get recent visitors
            var threeweeksago = DateTime.Now.AddDays(-27);

            var visitors =
                from a in DbUtil.Db.Attends
                let Hour1 = DbUtil.Db.GetTodaysMeetingHour(a.OrganizationId, thisday)
                where a.Person.FamilyId == id
                where a.Person.DeceasedDate == null
                where a.Organization.CanSelfCheckin.Value
                where a.Organization.CampusId == campus || campus == null
                where a.AttendanceFlag && a.MeetingDate > threeweeksago
                where a.MemberTypeId == (int)Attend.MemberTypeCode.Visitor
                where !a.Organization.OrganizationMembers.Any(om => om.PeopleId == a.PeopleId)
                where Hour1 != null
                group a by new { a.PeopleId, a.OrganizationId } into g
                let a = g.OrderByDescending(att => att.MeetingDate).First()
                select new Attendee
                {
                    Id = a.PeopleId,
                    Name = a.Person.Name,
                    BYear = a.Person.BirthYear, 
                    BMon = a.Person.BirthMonth, 
                    BDay = a.Person.BirthDay,
                    Class = a.Organization.OrganizationName,
                    Leader = a.Organization.LeaderName,
                    OrgId = a.OrganizationId,
                    Location = a.Organization.Location,
                    Age = a.Person.Age ?? 0,
                    Gender = a.Person.Gender.Code,
                    NumLabels = a.Organization.NumCheckInLabels ?? 1,
                    Hour = DbUtil.Db.GetTodaysMeetingHour(a.OrganizationId, thisday),
                    CheckedIn = DbUtil.Db.GetAttendedTodaysMeeting(a.OrganizationId, thisday, a.PeopleId) ?? false
                };

            var list = members.ToList();
            list.AddRange(visitors.ToList());

            // now get rest of family
            const string PleaseVisit = "No self check-in meetings available";
            var VisitorOrgName = PleaseVisit;
            var VisitorOrgId = 0;
            if (campus.HasValue)
            {
                // find a org on campus that allows an older, new visitor to check in to
                var qv = from o in DbUtil.Db.Organizations
                         where o.CampusId == campus
                         where o.CanSelfCheckin == true
                         where o.AllowNonCampusCheckIn == true
                         where o.WeeklySchedule.Day == thisday
                         select o;
                var vo = qv.FirstOrDefault();
                if (vo != null)
                {
                    VisitorOrgName = vo.OrganizationName;
                    VisitorOrgId = vo.OrganizationId;
                }
            }
            var otherfamily =
                from p in DbUtil.Db.People
                where p.FamilyId == id
                where p.DeceasedDate == null
                where !list.Select(a => a.Id).Contains(p.PeopleId)
                let oldervisitor = p.CampusId != campus && p.Age > 12
                select new Attendee
                {
                    Id = p.PeopleId,
                    Name = p.Name,
                    BYear = p.BirthYear,
                    BMon = p.BirthMonth,
                    BDay = p.BirthDay,
                    Class = oldervisitor ? VisitorOrgName : PleaseVisit,
                    OrgId = oldervisitor ? VisitorOrgId : 0,
                    Age = p.Age ?? 0,
                    Gender = p.Gender.Code,
                    NumLabels = 1
                };
            list.AddRange(otherfamily.ToList());
            var list2 = list.OrderByDescending(a => a.Age);

            return View("Family", list2);
        }
        private void NoCache()
        {
            var seconds = 10;
            Response.Cache.SetExpires(DateTime.Now.AddSeconds(seconds));
            Response.Cache.SetMaxAge(new TimeSpan(0, 0, seconds));
            Response.Cache.SetCacheability(HttpCacheability.Public);
            Response.Cache.SetValidUntilExpires(true);
            Response.Cache.SetSlidingExpiration(true);
            Response.Cache.SetETagFromFileDependencies();
        }
        public ActionResult Campuses()
        {
            var q = from c in DbUtil.Db.Campus
                    where c.Organizations.Any(o => o.CanSelfCheckin == true)
                    orderby c.Id
                    select c;
            return View(q);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult RecordAttend(int PeopleId, int OrgId, bool Present)
        {
            var q = from o in DbUtil.Db.Organizations
                    where o.OrganizationId == OrgId
                    select new
                    {
                        MeetingId = DbUtil.Db.GetTodaysMeetingId(OrgId, thisday),
                        MeetingTime = DbUtil.Db.GetTodaysMeetingHour(OrgId, thisday),
                        o.Location
                    };
            var info = q.Single();
            var meeting = DbUtil.Db.Meetings.SingleOrDefault(m => m.MeetingId == info.MeetingId);
            if (meeting == null)
            {
                meeting = new CmsData.Meeting
                {
                    OrganizationId = OrgId,
                    MeetingDate = info.MeetingTime,
                    CreatedDate = DateTime.Now,
                    CreatedBy = Util.UserId1,
                    GroupMeetingFlag = false,
                    Location = info.Location,
                };
                DbUtil.Db.Meetings.InsertOnSubmit(meeting);
                DbUtil.Db.SubmitChanges();
            }
            Attend.RecordAttendance(PeopleId, meeting.MeetingId, Present);
            DbUtil.Db.UpdateMeetingCounters(meeting.MeetingId);

            var r = new ContentResult();
            r.Content = "success";
            return r;
        }
    }
    public class Attendee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Birthday { get; set; }
        public int? BMon { get; set; }
        public int? BDay { get; set; }
        public int? BYear { get; set; }
        public string BirthDay
        {
            get { return Util.FormatBirthday(BYear, BMon, BDay); }
        }
        public string Class { get; set; }
        public DateTime? Hour { get; set; }
        public int OrgId { get; set; }
        public string Location { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public int NumLabels { get; set; }
        public int? CampusId { get; set; }
        public string Leader { get; set; }
        public string DisplayName
        {
            get
            {
                if (Age <= 18)
                    return "{0} ({1})".Fmt(Name, Age);
                return Name;
            }
        }
        public string DisplayClass
        {
            get
            {
                if (Hour.HasValue)
                    return "{0} ({1:h:mm})".Fmt(
                        CmsData.Organization.FormatOrgName(Class, Leader, Location), Hour);
                return Class;
            }
        }
        public bool CheckedIn { get; set; }
    }
    public class Family
    {
        public int FamilyId { get; set; }
        public string Name { get; set; }
        public string AreaCode { get; set; }
    }
}
