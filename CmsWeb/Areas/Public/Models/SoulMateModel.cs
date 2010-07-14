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
using System.Threading;
using System.Text.RegularExpressions;

namespace CmsWeb.Models
{
    public class ChildItem
    {
        public string Name { get; set; }
        public string Birthday { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
    }
    public class SoulMateModel
    {
        public int? SoulMateId { get; set; }
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
                        sm.ChildCareMeeting,
                        sm.HisEmail,
                        sm.HerEmail,
                        sm.HisEmailPreferred,
                        sm.HerEmailPreferred
                    };
            var s = q.Single();
            _Person1 = s.Him;
            _Person2 = s.Her;
            _meeting = s.Meeting;
            _ChildCareMeeting = s.ChildCareMeeting;
            email1 = s.HisEmail;
            email2 = s.HerEmail;
        }
        public SoulMateModel()
        {

        }
        public int? gender { get; set; }
        public string first1 { get; set; }
        public string lastname1 { get; set; }
        public string dob1 { get; set; }
        private DateTime _BDay1;
        public DateTime BDay1 { get { return _BDay1; } }
        public string phone1 { get; set; }
        public string homecell1 { get; set; }
        public string email1 { get; set; }
        private string _Shownew1;
        public string shownew1
        {
            get
            {
                return _Shownew1;
            }
            set
            {
                _Shownew1 = value;
            }
        }
        public string addr1 { get; set; }
        public string zip1 { get; set; }
        public string city1 { get; set; }
        public string state1 { get; set; }
        private Person _Person1;
        public Person person1
        {
            get { return _Person1; }
        }
        public int? ChildParent { get; set; }

        public string first2 { get; set; }
        public string lastname2 { get; set; }
        public string dob2 { get; set; }
        private DateTime _BDay2;
        public DateTime BDay2 { get { return _BDay2; } }
        public string phone2 { get; set; }
        public string homecell2 { get; set; }
        public string email2 { get; set; }
        public string shownew2 { get; set; }
        public string addr2 { get; set; }
        public string zip2 { get; set; }
        public string city2 { get; set; }
        public string state2 { get; set; }
        private Person _Person2;
        public Person person2
        {
            get { return _Person2; }
        }

        public int Relation { get; set; }

        internal int? childcaremeetingid
        {
            get { return childcaremeeting == null ? null : (int?)childcaremeeting.MeetingId; }
        }
        internal CmsData.Meeting _ChildCareMeeting;
        public CmsData.Meeting childcaremeeting
        {
            get
            {
                if (_ChildCareMeeting == null)
                {
                    var q = from m in DbUtil.Db.Meetings
                            where m.Organization.OrganizationId == DbUtil.Settings("SmlChildcareId", "0").ToInt()
                            where m.MeetingDate == meeting.MeetingDate
                            select m;
                    _ChildCareMeeting = q.FirstOrDefault();
                }
                return _ChildCareMeeting;
            }
        }
        internal CmsData.Meeting _meeting;
        public CmsData.Meeting meeting
        {
            get
            {
                if (_meeting == null)
                {
                    var q = from m in DbUtil.Db.Meetings
                            where m.Organization.OrganizationId == DbUtil.Settings("SmlGroupId", "0").ToInt()
                            where m.MeetingDate > Util.Now
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
            int count;
            _Person1 = CmsWeb.Models.SearchPeopleModel
                .FindPerson(phone1, first1, lastname1, BDay1, out count);
            return count;
        }
        public int FindMember2()
        {
            int count;
            _Person2 = CmsWeb.Models.SearchPeopleModel
                .FindPerson(phone2, first2, lastname2, BDay2, out count);
            return count;
        }
        public int FindMember(string phone, string first, string last, DateTime dob, out Person person)
        {
            int count;
            person = CmsWeb.Models.SearchPeopleModel
                .FindPerson(phone, first, last, dob, out count);
            return count;
        }

        public void ValidateModel(ModelStateDictionary ModelState)
        {
            if (!first1.HasValue())
                ModelState.AddModelError("first1", "first name required");
            if (!lastname1.HasValue())
                ModelState.AddModelError("lastname1", "last name required");
            else if (lastname1.ToUpper() == lastname1 || lastname1.ToLower() == lastname1)
                ModelState.AddModelError("lastname1", "Please use Proper Casing");
            if (!Util.DateValid(dob1, out _BDay1))
                ModelState.AddModelError("dob1", "valid birth date required");
            var d = phone1.GetDigits().Length;
            if (d != 7 && d != 10)
                ModelState.AddModelError("phone1", "7 or 10 digits");
            else if (!homecell1.HasValue())
                ModelState.AddModelError("phone1", "pick a phone type");
            if (!email1.HasValue() || !Util.ValidEmail(email1))
                ModelState.AddModelError("email1", "Please specify a valid email address.");
            if (shownew1.ToInt() >= 2)
            {
                if (!addr1.HasValue())
                    ModelState.AddModelError("addr1", "need address");
                if (zip1.GetDigits().Length != 5)
                    ModelState.AddModelError("zip1", "need 5 digit zip");
                if (!city1.HasValue())
                    ModelState.AddModelError("city1", "need city");
                if (!state1.HasValue())
                    ModelState.AddModelError("state1", "need state");
            }

            if (!first2.HasValue())
                ModelState.AddModelError("first2", "first name required");
            if (!lastname2.HasValue())
                ModelState.AddModelError("lastname2", "last name required");
            else if (lastname2.ToUpper() == lastname2 || lastname2.ToLower() == lastname2)
                ModelState.AddModelError("lastname2", "Please use Proper Casing");
            if (!Util.DateValid(dob2, out _BDay2))
                ModelState.AddModelError("dob2", "valid birth date required");
            d = phone2.GetDigits().Length;
            if (d != 7 && d != 10)
                ModelState.AddModelError("phone2", "7 or 10 digits");
            else if (!homecell2.HasValue())
                ModelState.AddModelError("phone2", "pick a phone type");
            if (!email2.HasValue() || !Util.ValidEmail(email2))
                ModelState.AddModelError("email2", "Please specify a valid email address.");
            if (shownew2.ToInt() >= 2)
            {
                if (!addr2.HasValue())
                    ModelState.AddModelError("addr2", "need address");
                if (zip2.GetDigits().Length != 5)
                    ModelState.AddModelError("zip2", "need 5 digit zip");
                if (!city2.HasValue())
                    ModelState.AddModelError("city2", "need city");
                if (!state2.HasValue())
                    ModelState.AddModelError("state2", "need state");
            }
            if (Relation > 0 && Relation < 3 
                && homecell1 == "h" && homecell2 == "h" 
                && phone1.GetDigits() != phone2.GetDigits())
                ModelState.AddModelError("phone2", "Home phones cannot be different");
            if (Relation == 0)
                ModelState.AddModelError("Relation", "Please select a relationship");
        }
        public void ValidateChild(ModelStateDictionary modelState)
        {
            if (!first1.HasValue())
                modelState.AddModelError("first1", "first name required");
            if (!lastname1.HasValue())
                modelState.AddModelError("lastname1", "last name required");
            else if (lastname1.ToUpper() == lastname1 || lastname1.ToLower() == lastname1)
                modelState.AddModelError("lastname1", "Please use Proper Casing");
            if (!Util.DateValid(dob1, out _BDay1))
                modelState.AddModelError("dob1", "valid birth date required");

            if (!gender.HasValue)
                modelState.AddModelError("gender2", "gender required");
            if (!ChildParent.HasValue)
                modelState.AddModelError("ChildParent", "choose a parent");
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
                new SelectListItem { Value="5", Text="Seriously dating" },
            };
        }
        public IEnumerable<SelectListItem> Parents()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value="0", Text="(select parent)" },
                new SelectListItem { Value=person2.PeopleId.ToString(), Text=person2.Name },
                new SelectListItem { Value=person1.PeopleId.ToString(), Text=person1.Name },
            };
        }
        public IEnumerable<ChildItem> Children()
        {
            var q = from c in DbUtil.Db.People
                    where c.Attends.Any(a => a.MeetingId == childcaremeeting.MeetingId)
                    where c.Family.People.Any(p => p.PeopleId == person1.PeopleId || p.PeopleId == person2.PeopleId)
                    select new ChildItem
                    {
                        Name = c.Name,
                        Birthday = c.BirthMonth + "/" + c.BirthDay + "/" + c.BirthYear,
                        Age = c.Age ?? 0,
                        Gender = c.Gender.Description
                    };
            return q;
        }
        public IEnumerable<ChildItem> Children(Person par)
        {
            var q = from c in DbUtil.Db.People
                    where c.Attends.Any(a => a.MeetingId == childcaremeetingid)
                    where c.Family.People.Any(p => p.PeopleId == par.PeopleId)
                    select new ChildItem
                    {
                        Name = c.Name,
                        Birthday = c.BirthMonth + "/" + c.BirthDay + "/" + c.BirthYear,
                        Age = c.Age ?? 0,
                        Gender = c.Gender.Description
                    };
            return q;
        }

        internal void EnrollInClass(Person person)
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

        public void EnrollInChildcare(Person c)
        {
            var member = DbUtil.Db.OrganizationMembers.SingleOrDefault(om =>
                om.OrganizationId == childcaremeeting.OrganizationId && om.PeopleId == c.PeopleId);
            if (member == null)
                OrganizationMember.InsertOrgMembers(
                    childcaremeeting.OrganizationId,
                    c.PeopleId,
                    (int)OrganizationMember.MemberTypeCode.Member,
                    DateTime.Today, null, false);

            var attend = DbUtil.Db.Attends.SingleOrDefault(a =>
                a.OrganizationId == childcaremeeting.OrganizationId
                && a.PeopleId == c.PeopleId
                && a.AttendanceFlag == true
                && a.MeetingDate > Util.Now);
            if (attend != null)
                Attend.RecordAttendance(c.PeopleId, attend.MeetingId, false);
            Attend.RecordAttendance(c.PeopleId, childcaremeeting.MeetingId, true);
            DbUtil.Db.UpdateMeetingCounters(childcaremeeting.MeetingId);
        }
        internal void AddPeople()
        {
            var married = Relation < 3;
            if (person1 != null && person2 != null)
                return;
            if (married && person1 != null && person2 == null)
                _Person2 = AddPersonToPerson(person1, first2, lastname2, dob2, phone2, homecell2, email2);
            else if (married && person1 == null && person2 != null)
                _Person1 = AddPersonToPerson(person2, first1, lastname1, dob1, phone1, homecell1, email1);
            else if (married && person1 == null && person2 == null)
            {
                _Person1 = AddPerson(1, first1, lastname1, dob1, addr1, city1, state1, zip1, phone1, homecell1, married, email1);
                _Person2 = AddPersonToPerson(person1, first2, lastname2, dob2, phone2, homecell2, email2);
            }
            else if (!married && person1 != null && person2 == null)
                _Person2 = AddPerson(2, first2, lastname2, dob2, addr2, city2, state2, zip2, phone2, homecell2, false, email2);
            else if (!married && person1 == null && person2 != null)
                _Person1 = AddPerson(1, first1, lastname1, dob1, addr1, city1, state1, zip1, phone1, homecell1, false, email1);
            else if (!married && person1 == null && person2 == null)
            {
                _Person1 = AddPerson(1, first1, lastname1, dob1, addr1, city1, state1, zip1, phone1, homecell1, married, email1);
                _Person2 = AddPerson(2, first2, lastname2, dob2, addr2, city2, state2, zip2, phone2, homecell2, married, email2);
            }
        }
        internal Person AddPersonToPerson(Person p, string first, string last, string dob, string phone, string homecell, string email)
        {
            var np = Person.Add(p.Family, (int)Family.PositionInFamily.PrimaryAdult,
                null, first, null, last, dob, true, p.GenderId == 1? 2 : 1,
                    DbUtil.Settings("SmlOrigin", "0").ToInt(), 
                    DbUtil.Settings("SmlEntry", "0").ToInt());
            switch (homecell)
            {
                case "h":
                    p.Family.HomePhone = phone.GetDigits();
                    break;
                case "c":
                    np.CellPhone = phone.GetDigits();
                    break;
            }
            np.EmailAddress = email;
            np.CampusId = meeting.Organization.CampusId;
            DbUtil.Db.SubmitChanges();
            DbUtil.Db.Refresh(RefreshMode.OverwriteCurrentValues, np);
            return np;
        }
        internal Person AddPerson(int gender, string first, string last, string dob, 
            string addr, string city, string state, string zip, string phone, string homecell, bool married, string email)
        {
            var f = new Family
            {
                AddressLineOne = addr,
                CityName = city,
                StateCode = state,
                ZipCode = zip,
            };

            var np = Person.Add(f, (int)Family.PositionInFamily.PrimaryAdult, null, 
                    first, 
                    null, 
                    last, 
                    dob, married, gender, 
                    DbUtil.Settings("SmlOrigin", "0").ToInt(), 
                    DbUtil.Settings("SmlEntry", "0").ToInt());
            switch (homecell)
            {
                case "h":
                    f.HomePhone = phone.GetDigits();
                    break;
                case "c":
                    np.CellPhone = phone.GetDigits();
                    break;
            }
            np.EmailAddress = email;
            np.CampusId = meeting.Organization.CampusId;
            DbUtil.Db.SubmitChanges();
            DbUtil.Db.Refresh(RefreshMode.OverwriteCurrentValues, np);
            return np;
        }
        public Person AddChild(Person p)
        {
            var np = Person.Add(p.Family, 30, null, 
                first1, 
                null, 
                lastname1, 
                dob1, false, gender.Value, 0, null);
            np.CampusId = meeting.Organization.CampusId;
            DbUtil.Db.SubmitChanges();
            DbUtil.Db.Refresh(RefreshMode.OverwriteCurrentValues, np);
            return np;
        }
    }
}
