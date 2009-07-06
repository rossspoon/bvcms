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
            var ph = Util.GetDigits(id);
            var q = from f in DbUtil.Db.Families
                    where f.HomePhoneLU.StartsWith(ph)
                    || f.HeadOfHousehold.CellPhoneLU.StartsWith(ph)
                    select new Fam
                    {
                        FamId = f.FamilyId,
                        Last = f.HeadOfHousehold.LastName,
                        Addr = f.AddressLineOne,
                        City = f.CityName,
                        St = f.StateCode,
                        Zip = f.ZipCode.Substring(0, 5),
                    };
            var list = q.ToList().Where(f => f.NumGrade2 > 0);
            return View(list);
        }
        public ActionResult Children(int id)
        {
            var q = from p in DbUtil.Db.People
                    where p.FamilyId == id
                    where p.PositionInFamilyId == 30
                    where p.Age <= 9
                    let att = p.Attends.Where(a => a.AttendanceFlag
                        && a.Organization.OrganizationStatusId == (int)CmsData.Organization.OrgStatusCode.Active
                        && a.Organization.DivOrgs.Any(dor => dor.Division.ProgId == DbUtil.BFClassOrgTagId))
                        .OrderByDescending(a => a.MeetingDate).FirstOrDefault()
                    select new
                    {
                        p.PeopleId,
                        p.Name,
                        p.BirthYear,
                        p.BirthMonth,
                        p.BirthDay,
                        p.Age,
                        p.Gender.Code,
                        att,
                    };
            var list = q.ToList();
            var q2 = from p in list
                     select new Child
                     {
                         Id = p.PeopleId,
                         Name = p.Name,
                         Birthday = Util.FormatBirthday(p.BirthYear, p.BirthMonth, p.BirthDay),
                         Class = p.att == null ? "Please Visit Welcome Center" : p.att.Organization.FullName,
                         OrgId = p.att == null ? 0 : p.att.OrganizationId,
                         Location = p.att == null ? "loc:" : p.att.Organization.Location,
                         Age = p.Age ?? 0,
                         Gender = p.Code
                     };
            return View(q2);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult RecordAttend(int PeopleId, int OrgId)
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
            ctl.RecordAttendance(PeopleId, meeting.MeetingId, true);
            DbUtil.Db.UpdateMeetingCounters(meeting.MeetingId);

            var r = new ContentResult();
            r.Content = "success";
            return r;
        }
    }
    public class Child
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Birthday { get; set; }
        public string Class { get; set; }
        public int OrgId { get; set; }
        public string Location { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
    }
    public class Fam
    {
        public int FamId { get; set; }
        public string Last { get; set; }
        public string Addr { get; set; }
        public string City { get; set; }
        public string St { get; set; }
        public string Zip { get; set; }
        int? _numgrade;
        public int NumGrade2
        {
            get
            {
                if (!_numgrade.HasValue)
                    _numgrade = DbUtil.Db.People.Count(p => p.FamilyId == FamId && p.Age <= 9);
                return _numgrade.Value;
            }
        }
        public override string ToString()
        {
            return "{0} Family, {1}, {2}, {3} {4} ({5})".Fmt(Last, Addr, City, St, Zip, NumGrade2);
        }
    }
}
