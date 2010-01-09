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

namespace CMSWeb.Models
{
    public class StepClassModel
    {
        public string first { get; set; }
        public string last { get; set; }
        public string dob { get; set; }
        private DateTime _Birthday;
        private DateTime birthday
        {
            get
            {
                if (_Birthday == DateTime.MinValue)
                    Util.DateValid(dob, out _Birthday);
                return _Birthday;
            }
        }
        public string zip { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
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
            int count;
            person = CMSWeb.Models.SearchPeopleModel
                .FindPerson(phone, first, last, birthday, out count);
            return count;
        }

        public void ValidateModel(ModelStateDictionary modelState)
        {
            CMSWeb.Models.SearchPeopleModel
                .ValidateFindPerson(modelState, first, last, birthday, phone);

            if (!phone.HasValue())
                modelState.AddModelError("phone", "phone required");
            if (!email.HasValue() || !Util.ValidEmail(email))
                modelState.AddModelError("email", "Please specify a valid email address.");
            if (meetingid == 0)
                modelState.AddModelError("meetingid", "Please select a class date");
        }

        public IEnumerable<SelectListItem> AvailableClasses(string name)
        {
            var q = from m in DbUtil.Db.Meetings
                    where m.Organization.OrganizationName == name
                    where m.MeetingDate > Util.Now.AddDays(6)
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
                && a.MeetingDate > Util.Now);
            if (attend != null)
                Attend.RecordAttendance(person.PeopleId, attend.MeetingId, false);
            Attend.RecordAttendance(person.PeopleId, meeting.MeetingId, true);
            DbUtil.Db.UpdateMeetingCounters(meeting.MeetingId);
        }
    }
}
