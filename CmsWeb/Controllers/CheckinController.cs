using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CmsData;
using UtilityExtensions;

namespace CMSWeb.Controllers
{
    public class CheckinController : Controller
    {
        public int? thisday { get; set; }

        public ActionResult Match(string id, int? campus, int? thisday)
        {
            this.thisday = thisday.Value;

            var seconds = 10;
            Response.Cache.SetExpires(DateTime.Now.AddSeconds(seconds));
            Response.Cache.SetMaxAge(new TimeSpan(0, 0, seconds));
            Response.Cache.SetCacheability(HttpCacheability.Public);
            Response.Cache.SetValidUntilExpires(true);
            Response.Cache.SetSlidingExpiration(true);
            Response.Cache.SetETagFromFileDependencies();

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
                return ShowFamily(0, 0); // not found
            if (matches.Count == 1)
                return ShowFamily(matches[0].FamilyId, campus);
            return View("Multiple", matches);
        }
        public ActionResult Family(int id, int? campus)
        {
            return ShowFamily(id, campus);
        }
        public ActionResult ShowFamily(int id, int? campus)
        {
            var now = DateTime.Now;
            // get org members first
            var members =
                from om in DbUtil.Db.OrganizationMembers
                let meeting = om.Organization.Meetings.OrderByDescending(m => m.MeetingDate).FirstOrDefault()
                let sday = om.Organization.WeeklySchedule == null ? -1 : om.Organization.WeeklySchedule.Day
                where om.Organization.CanSelfCheckin.Value
                where om.Organization.CampusId == campus || campus == null
                where om.Person.FamilyId == id
                where om.Person.DeceasedDate == null
                where (thisday == sday)
                   || (meeting != null && (int)meeting.MeetingDate.Value.DayOfWeek == thisday)
                select new
                {
                    om.Organization,
                    om.Person,
                };
            var list = new List<Attendee>();
            foreach (var i in members)
            {
                var meeting = GetMeeting(i.Organization.OrganizationId, 
                    i.Organization.WeeklySchedule.MeetingTime, thisday);
                list.Add(new Attendee
                {
                    Id = i.Person.PeopleId,
                    Name = i.Person.Name,
                    Birthday = Util.FormatBirthday(i.Person.BirthYear, i.Person.BirthMonth, i.Person.BirthDay),
                    Class = i.Organization.FullName,
                    OrgId = i.Organization.OrganizationId,
                    Location = i.Organization.Location,
                    Age = i.Person.Age ?? 0,
                    Gender = i.Person.Gender.Code,
                    NumLabels = i.Organization.NumCheckInLabels ?? 1,
                    Hour = meeting == null ? null : meeting.MeetingDate
                });
            }

            // now get recent visitors
            var threeweeksago = DateTime.Now.AddDays(-27);
            var visitors =
                from a in DbUtil.Db.Attends
                let meeting = a.Organization.Meetings.OrderByDescending(m => m.MeetingDate).FirstOrDefault()
                let sday = a.Organization.WeeklySchedule.Day
                where a.Person.FamilyId == id
                where a.Person.DeceasedDate == null
                where a.Organization.CanSelfCheckin.Value
                where a.Organization.CampusId == campus || campus == null
                where a.AttendanceFlag && a.MeetingDate > threeweeksago
                where a.MemberTypeId == (int)Attend.MemberTypeCode.Visitor
                where thisday == sday
                   || (meeting != null && (int)meeting.MeetingDate.Value.DayOfWeek == thisday)
                group a by new { a.Person, a.Organization } into g
                let a = g.OrderByDescending(a => a.MeetingDate).First()
                select new
                {
                    a.Person,
                    a.Organization
                };
            foreach (var i in visitors)
            {
                var meeting = GetMeeting(i.Organization.OrganizationId, 
                    i.Organization.WeeklySchedule.MeetingTime, thisday);
                list.Add(new Attendee
                {
                    Id = i.Person.PeopleId,
                    Name = i.Person.Name,
                    Birthday = Util.FormatBirthday(i.Person.BirthYear, i.Person.BirthMonth, i.Person.BirthDay),
                    Class = i.Organization.FullName,
                    OrgId = i.Organization.OrganizationId,
                    Location = i.Organization.Location,
                    Age = i.Person.Age ?? 0,
                    Gender = i.Person.Gender.Code,
                    NumLabels = i.Organization.NumCheckInLabels ?? 1,
                    Hour = meeting == null ? null : meeting.MeetingDate
                });
            }

            var pids = list.Select(i => i.Id).ToArray();
            // now get rest of family
            var otherfamily = from p in DbUtil.Db.People
                              where p.FamilyId == id
                              where p.DeceasedDate == null
                              where !pids.Contains(p.PeopleId)
                              select p;
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
            foreach (var p in otherfamily)
            {
                bool oldervisitor = p.CampusId != campus && p.Age > 12;
                list.Add(new Attendee
                {
                    Id = p.PeopleId,
                    Name = p.Name,
                    Birthday = Util.FormatBirthday(p.BirthYear, p.BirthMonth, p.BirthDay),
                    Class = oldervisitor ? VisitorOrgName : PleaseVisit,
                    OrgId = oldervisitor ? VisitorOrgId : 0,
                    Age = p.Age ?? 0,
                    Gender = p.Gender.Code,
                    NumLabels = 1
                });
            }
            var list2 = list.OrderByDescending(a => a.Age);

            return View("Family", list2);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult RecordAttend(int PeopleId, int OrgId, bool Present)
        {
            var info = (from o in DbUtil.Db.Organizations
                       where o.OrganizationId == OrgId
                       select new { o.WeeklySchedule.MeetingTime, o.Location }).Single();
            var meeting = GetMeeting(OrgId, info.MeetingTime, thisday);
            if (meeting == null || meeting.OrganizationId == 0)
            {
                meeting = new CmsData.Meeting
                {
                    OrganizationId = OrgId,
                    MeetingDate = Util.Now.Date.Add(info.MeetingTime.TimeOfDay),
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
        internal static CmsData.Meeting GetMeeting(int OrgId, DateTime? DefaultHour, int? day)
        {
            if (!day.HasValue)
                day = (int)DateTime.Now.DayOfWeek;
            var prevMidnight = Util.Now.Date;
            var ninetyMinutesAgo = Util.Now.AddMinutes(-90);
            var nextMidnight = prevMidnight.AddDays(1);

            // try to find a meeting today that started no more than 90 minutes ago
            var meeting = (from m in DbUtil.Db.Meetings
                           where m.OrganizationId == OrgId
                           where m.MeetingDate >= ninetyMinutesAgo
                           where m.MeetingDate < nextMidnight
                           orderby m.MeetingDate
                           select m).FirstOrDefault();

            // try to find a meeting that started anytime today
            if (meeting == null)
                meeting = (from m in DbUtil.Db.Meetings
                           where m.OrganizationId == OrgId
                           where m.MeetingDate >= prevMidnight
                           where m.MeetingDate < nextMidnight
                           orderby m.MeetingDate
                           select m).FirstOrDefault();
            if (meeting == null && DefaultHour.HasValue 
                    && (int)DefaultHour.Value.DayOfWeek == (int)DateTime.Now.DayOfWeek)
                meeting = new CmsData.Meeting 
                    { MeetingDate = Util.Now.Date.Add(DefaultHour.Value.TimeOfDay) };
            return meeting;
        }
    }
    public class Attendee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Birthday { get; set; }
        public string Class { get; set; }
        public DateTime? Hour { get; set; }
        public int OrgId { get; set; }
        public string Location { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public int NumLabels { get; set; }
        public int? CampusId { get; set; }
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
                    return "{0} ({1:h:mm})".Fmt(Class, Hour);
                return Class;
            }
        }
    }
    public class Family
    {
        public int FamilyId { get; set; }
        public string Name { get; set; }
        public string AreaCode { get; set; }
    }
}
