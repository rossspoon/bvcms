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
using CMSPresenter;

namespace CMSWeb.Models
{
    public class ChildItem
    {
        public string Name { get; set; }
        public string Birthday { get; set; }
        public int Age { get; set; }
    }
    public class SoulMateModel
    {
        int? SoulMateId { get; set; }
        public SoulMateModel(int id)
        {
            SoulMateId = id;
            var q = from sm in DbUtil.Db.SoulMates
                    where sm.Id == id
                    select new
                    {
                        sm.Him,
                        sm.Her,
                        sm.Meeting,
                        sm.ChildCareMeeting
                    };
            var s = q.Single();
            _Person1 = s.Him;
            _Person2 = s.Her;
            _meeting = s.Meeting;
            _ChildCareMeeting = s.ChildCareMeeting;
        }
        public SoulMateModel()
        {

        }
        public string first1 { get; set; }
        public string lastname1 { get; set; }
        public string dob1 { get; set; }
        private DateTime _dob1;
        public DateTime DOB1 { get { return _dob1; } }
        public string phone1 { get; set; }
        public string email1 { get; set; }
        public bool preferredEmail1 { get; set; }
        private Person _Person1;
        public Person person1
        {
            get { return _Person1; }
        }
        public int? ChildParent { get; set; }

        public string first2 { get; set; }
        public string lastname2 { get; set; }
        public string dob2 { get; set; }
        private DateTime _dob2;
        public DateTime DOB2 { get { return _dob2; } }
        public string phone2 { get; set; }
        public string email2 { get; set; }
        public bool preferredEmail2 { get; set; }
        private Person _Person2;
        public Person person2
        {
            get { return _Person2; }
        }

        public int Relation { get; set; }

        internal CmsData.Meeting _ChildCareMeeting;
        internal CmsData.Meeting _meeting;
        public CmsData.Meeting meeting
        {
            get
            {
                if (_meeting == null)
                {
                    var q = from m in DbUtil.Db.Meetings
                            where m.Organization.OrganizationName == "Soulmate Live"
                            where m.MeetingDate > DateTime.Now.AddDays(3)
                            orderby m.MeetingDate ascending
                            select m;
                    _meeting = q.FirstOrDefault();
                }
                return _meeting;
            }
        }
        public DateTime NextEvent
        {
            get
            {
                if (meeting == null)
                    return DateTime.MaxValue;
                return meeting.MeetingDate.Value;
            }
        }

        public int FindMember1()
        {
            return FindMember(phone1, lastname1, first1, DOB1, out _Person1);
        }
        public int FindMember2()
        {
            return FindMember(phone2, lastname2, first2, DOB2, out _Person2);
        }
        private int FindMember(string phone, string last, string first, DateTime DOB,
            out Person person)
        {
            var fone = Util.GetDigits(phone);
            var q = from p in DbUtil.Db.People
                    where (p.LastName.StartsWith(last) || p.MaidenName.StartsWith(last))
                            && (p.FirstName.StartsWith(first)
                            || p.NickName.StartsWith(first)
                            || p.MiddleName.StartsWith(first))
                    where p.CellPhone.Contains(phone)
                            || p.WorkPhone.Contains(phone)
                            || p.Family.HomePhone.Contains(phone)
                    where p.BirthDay == DOB.Day && p.BirthMonth == DOB.Month && p.BirthYear == DOB.Year
                    select p;
            var count = q.Count();
            person = null;
            if (count == 1)
                person = q.Single();
            return count;
        }

        public void ValidateModel(ModelStateDictionary ModelState)
        {
            if (!first1.HasValue())
                ModelState.AddModelError("first1", "first name required");
            if (!lastname1.HasValue())
                ModelState.AddModelError("lastname1", "last name required");
            if (!DateTime.TryParse(dob1, out _dob1))
                ModelState.AddModelError("dob1", "valid birth date required");
            var d = phone1.GetDigits().Length;
            if (d != 7 && d != 10)
                ModelState.AddModelError("phone1", "7 or 10 digits");
            if (!email1.HasValue() || !Util.ValidEmail(email1))
                ModelState.AddModelError("email1", "Please specify a valid email address.");

            if (!first2.HasValue())
                ModelState.AddModelError("first2", "first name required");
            if (!lastname2.HasValue())
                ModelState.AddModelError("lastname2", "last name required");
            if (!DateTime.TryParse(dob2, out _dob2))
                ModelState.AddModelError("dob2", "valid birth date required");
            d = phone2.GetDigits().Length;
            if (d != 7 && d != 10)
                ModelState.AddModelError("phone2", "7 or 10 digits");
            if (!email2.HasValue() || !Util.ValidEmail(email2))
                ModelState.AddModelError("email2", "Please specify a valid email address.");

            if (Relation == 0)
                ModelState.AddModelError("Relation", "Please select a relationship");
        }

        public IEnumerable<SelectListItem> Relations()
        {
            return new List<SelectListItem> 
            {
                new SelectListItem { Value="0", Text="(describe your relationship)" },
                new SelectListItem { Value="1", Text="Married under 5 years" },
                new SelectListItem { Value="2", Text="Married 5 years or more" },
                new SelectListItem { Value="3", Text="Engaged" },
                new SelectListItem { Value="4", Text="Might as well be married" },
            };
        }
        public IEnumerable<SelectListItem> Parents()
        {
            return new List<SelectListItem> 
            {
                new SelectListItem { Value=person2.PeopleId.ToString(), Text=person2.Name },
                new SelectListItem { Value=person1.PeopleId.ToString(), Text=person1.Name },
            };
        }
        public IEnumerable<ChildItem> Children()
        {
            var q = from c in DbUtil.Db.People
                    where c.Attends.Any(a => a.MeetingId == _ChildCareMeeting.MeetingId)
                    where c.Family.People.Any(p => p.PeopleId == person1.PeopleId || p.PeopleId == person2.PeopleId)
                    select new ChildItem
                    {
                        Name = c.Name,
                        Birthday = Util.FormatBirthday(c.BirthYear, c.BirthMonth, c.BirthDay),
                        Age = c.Age ?? 0
                    };
            return q;
        }

        internal void EnrollInClass(Person person)
        {
            var member = DbUtil.Db.OrganizationMembers.SingleOrDefault(om =>
                om.OrganizationId == meeting.OrganizationId && om.PeopleId == person.PeopleId);
            if (member == null)
                OrganizationController.InsertOrgMembers(
                    meeting.OrganizationId,
                    person.PeopleId,
                    (int)OrganizationMember.MemberTypeCode.Member,
                    DateTime.Today, null);

            var attend = DbUtil.Db.Attends.SingleOrDefault(a =>
                a.OrganizationId == meeting.OrganizationId
                && a.PeopleId == person.PeopleId
                && a.AttendanceFlag == true
                && a.MeetingDate > DateTime.Now);
            var actl = new CMSPresenter.AttendController();
            if (attend != null)
                actl.RecordAttendance(person.PeopleId, attend.MeetingId, false);
            actl.RecordAttendance(person.PeopleId, meeting.MeetingId, true);
            DbUtil.Db.UpdateMeetingCounters(meeting.MeetingId);
        }
    }
}
