using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using CmsData;
using System.Web.Mvc;
using System.Text;
using System.Configuration;
using UtilityExtensions;

namespace CMSRegCustom.Models
{
    public class StepClassModel
    {
        public string first { get; set; }
        public string lastname { get; set; }
        public string dob { get; set; }
        private DateTime _dob;
        public DateTime DOB { get { return _dob; } }
        public string zip { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public bool preferredEmail { get; set; }
        public Person person { get; set; }
        internal CmsData.Meeting meeting;
        public int meetingid
        {
            get
            {
                if (meeting == null)
                    return 0;
                return meeting.MeetingId;
            }
            set
            {
                if (value == 0)
                    meeting = null;
                else
                    meeting = DbUtil.Db.Meetings.Single(mt => mt.MeetingId == value);
            }
        }

        public int FindMember()
        {
            var fone = Util.GetDigits(phone);
            var q = from p in DbUtil.Db.People
                    where (p.FirstName == first || p.NickName == first || p.MiddleName == first)
                    where (p.LastName == lastname || p.MaidenName == lastname)
                    where p.CellPhone.Contains(fone)
                            || p.WorkPhone.Contains(fone)
                            || p.Family.HomePhone.Contains(fone)
                    where p.BirthDay == DOB.Day && p.BirthMonth == DOB.Month && p.BirthYear == DOB.Year
                    select p;
            var count = q.Count();
            if (count == 1)
                person = q.Single();
            return count;
        }

        public void ValidateModel(ModelStateDictionary ModelState)
        {
            if (!first.HasValue())
                ModelState.AddModelError("first", "first name required");
            if (!lastname.HasValue())
                ModelState.AddModelError("lastname", "last name required");
            if (!Util.DateValid(dob, out _dob))
                ModelState.AddModelError("dob", "valid birth date required");

            var d = phone.GetDigits().Length;
            if (d != 7 && d != 10)
                ModelState.AddModelError("phone", "7 or 10 digits");
            if (!email.HasValue() || !Util.ValidEmail(email))
                ModelState.AddModelError("email", "Please specify a valid email address.");
            if (meetingid == 0)
                ModelState.AddModelError("meetingid", "Please select a class date");
        }

        public IEnumerable<SelectListItem> AvailableClasses(string name)
        {
            var q = from m in DbUtil.Db.Meetings
                    where m.Organization.OrganizationName == name
                    where m.MeetingDate > DateTime.Now.AddDays(6)
                    orderby m.MeetingDate ascending
                    select new { Dt = m.MeetingDate.Value, Id = m.MeetingId.ToString() };
            var q2 = from m in q.ToList()
                     select new SelectListItem
                     {
                         Text = m.Dt.ToString("ddd, MMM d h:mm tt"),
                         Value = m.Id,
                     };
            var list = q2.ToList();
            list.Insert(0, new SelectListItem { Value = "0", Text = "Select a Date" });
            return list;
        }

        internal void EnrollInClass()
        {
            var member = DbUtil.Db.OrganizationMembers.SingleOrDefault(om =>
                om.OrganizationId == meeting.OrganizationId && om.PeopleId == person.PeopleId);
            if (member == null)
                OrganizationMember.InsertOrgMembers(
                    meeting.OrganizationId,
                    person.PeopleId,
                    (int)OrganizationMember.MemberTypeCode.Member,
                    DateTime.Today, null, false);

            var attend = DbUtil.Db.Attends.SingleOrDefault(a =>
                a.OrganizationId == meeting.OrganizationId
                && a.PeopleId == person.PeopleId
                && a.AttendanceFlag == true
                && a.MeetingDate > DateTime.Now);
            if (attend != null)
                Attend.RecordAttendance(person.PeopleId, attend.MeetingId, false);
            Attend.RecordAttendance(person.PeopleId, meeting.MeetingId, true);
            DbUtil.Db.UpdateMeetingCounters(meeting.MeetingId);
        }
    }
}
