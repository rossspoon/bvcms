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
        //
        // GET: /Checkin/

        public ActionResult Match(string id)
        {
            var ph = Util.GetDigits(id).PadLeft(10,'0');
            var p7 = ph.Substring(3);
            var ac = ph.Substring(0, 3);
            var q1 = from f in DbUtil.Db.Families
                     where f.HomePhoneLU.StartsWith(p7)
                        || f.HeadOfHousehold.CellPhoneLU.StartsWith(p7)
                        || f.HeadOfHouseholdSpouse.CellPhoneLU.StartsWith(p7)
                     select new { f.FamilyId, f.HomePhoneAC } ;
            var matches = q1.ToList();
            int? famid = null;
            if (matches.Count > 1)
                matches = matches.Where(m => m.HomePhoneAC == ac).ToList();
            if (matches.Count == 1)
                famid = matches[0].FamilyId;

            var now = DateTime.Now;

            var q2 = from om in DbUtil.Db.OrganizationMembers
                     where om.Organization.CanSelfCheckin.Value
                     where om.Person.FamilyId == famid
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
                     where a.Person.FamilyId == famid
                     where a.Organization.CanSelfCheckin.Value
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
                     where p.FamilyId == famid
                     where !pids.Contains(p.PeopleId)
                     select p;
            foreach (var i in q4)
                list.Add(new Attendee
                {
                    Id = i.PeopleId,
                    Name = i.Name,
                    Birthday = Util.FormatBirthday(i.BirthYear, i.BirthMonth, i.BirthDay),
                    Class = "Please visit Welcome Center",
                    Age = i.Age ?? 0,
                    Gender = i.Gender.Code,
                    NumLabels = 1
                });
            var list2 = list.OrderByDescending(a => a.Age);

            return View(list2);
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
            var ctl = new CMSPresenter.AttendController();
            ctl.RecordAttendance(PeopleId, meeting.MeetingId, Present);
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
    }
}
