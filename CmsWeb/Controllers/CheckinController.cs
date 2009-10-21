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
        public ActionResult Match(string id, int? campus)
        {
            var ph = Util.GetDigits(id).PadLeft(10, '0');
            var p7 = ph.Substring(3);
            var ac = ph.Substring(0, 3);
            var q1 = from f in DbUtil.Db.Families
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

            var q2 = from om in DbUtil.Db.OrganizationMembers
                     where om.Organization.CanSelfCheckin.Value
                     where om.Organization.CampusId == campus || campus == null
                     where om.Person.FamilyId == id
                     where om.Person.DeceasedDate == null
                     //where now.TimeOfDay > om.Organization.WeeklySchedule.MeetingTime.AddDays(-1).TimeOfDay
                     //where now.TimeOfDay < om.Organization.WeeklySchedule.MeetingTime.AddHours(1).TimeOfDay
                     select new
                     {
                         om.Organization,
                         om.Person,
                     };
            var list = new List<Attendee>();
            foreach (var i in q2)
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
                    NumLabels = i.Organization.NumCheckInLabels ?? 1
                });

            var threeweeksago = DateTime.Now.AddDays(-27);
            var q3 = from a in DbUtil.Db.Attends
                     where a.Person.FamilyId == id
                     where a.Person.DeceasedDate == null
                     where a.Organization.CanSelfCheckin.Value
                     where a.Organization.CampusId == campus || campus == null
                     //where now.TimeOfDay > a.Organization.WeeklySchedule.MeetingTime.AddHours(-1).TimeOfDay
                     //where now.TimeOfDay < a.Organization.WeeklySchedule.MeetingTime.AddHours(1).TimeOfDay
                     where a.AttendanceFlag && a.MeetingDate > threeweeksago
                     where a.MemberTypeId == (int)Attend.MemberTypeCode.Visitor
                     group a by new { a.Person, a.Organization } into g
                     let a = g.OrderByDescending(a => a.MeetingDate).First()
                     select new
                     {
                         a.Person,
                         a.Organization
                     };
            foreach (var i in q3)
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
                    NumLabels = i.Organization.NumCheckInLabels ?? 1
                });
            var pids = list.Select(i => i.Id).ToArray();
            var q4 = from p in DbUtil.Db.People
                     where p.FamilyId == id
                     where p.DeceasedDate == null
                     where !pids.Contains(p.PeopleId)
                     select p;
            const string PleaseVisit = "Please visit Welcome Center";
            var VisitorOrgName = PleaseVisit;
            var VisitorOrgId = 0;
            if (campus.HasValue)
            {
                var qv = from o in DbUtil.Db.Organizations
                         where o.CampusId == campus
                         where o.CanSelfCheckin == true
                         where o.AllowNonCampusCheckIn == true
                         select o;

                var vo = qv.FirstOrDefault();
                if (vo != null)
                {
                    VisitorOrgName = vo.OrganizationName;
                    VisitorOrgId = vo.OrganizationId;
                }
            }
            foreach (var p in q4)
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
            var ret = (from o in DbUtil.Db.Organizations
                       where o.OrganizationId == OrgId
                       select new { o.WeeklySchedule, o.Location }).Single();
            var dt = Util.Now.Date;
            dt = dt.Add(ret.WeeklySchedule.MeetingTime.TimeOfDay);

            var meeting = (from m in DbUtil.Db.Meetings
                           where m.OrganizationId == OrgId
                           where m.MeetingDate == dt
                           select m).SingleOrDefault();

            if (meeting == null)
            {
                meeting = new CmsData.Meeting
                {
                    OrganizationId = OrgId,
                    MeetingDate = dt,
                    CreatedDate = DateTime.Now,
                    CreatedBy = Util.UserId1,
                    GroupMeetingFlag = false,
                    Location = ret.Location,
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
        public string Class { get; set; }
        public int OrgId { get; set; }
        public string Location { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public int NumLabels { get; set; }
        public int? CampusId { get; set; }
    }
    public class Family
    {
        public int FamilyId { get; set; }
        public string Name { get; set; }
        public string AreaCode { get; set; }
    }
}
