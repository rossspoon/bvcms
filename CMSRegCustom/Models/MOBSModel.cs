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
using System.Data.Linq.SqlClient;

namespace CMSRegCustom.Models
{
    public class MOBSModel
    {
        public MOBSModel()
        {
            tickets = 2;
        }
        public int? meetingid { get; set; }
        public decimal Amount
        {
            get
            {
                decimal cost;
                if (decimal.TryParse((string)DbUtil.Settings("MOBSTicketCost", "6"), out cost))
                    return cost * tickets;
                else
                    return 6 * tickets;
            }
        }
        public int tickets { get; set; }
        public int? peopleid { get; set; }
        public string phone { get; set; }
        public string homecell { get; set; }
        public string email { get; set; }
        public bool preferredEmail { get; set; }
        public string TransactionId { get; set; }

        private Meeting _meeting;
        public Meeting meeting
        {
            get
            {
                var orgid = DbUtil.Settings("MOBSOrgId", "0").ToInt();
                if (orgid == 0)
                    orgid = DbUtil.Db.Organizations.Where(o => o.OrganizationName.Contains("MOBS")).Select(o => o.OrganizationId).Single();
                if (_meeting == null)
                {
                    var q = from m in DbUtil.Db.Meetings
                            where m.Organization.OrganizationId == orgid
                            where m.MeetingDate > DateTime.Now.AddHours(6)
                            orderby m.MeetingDate
                            select m;
                    _meeting = q.FirstOrDefault();
                }
                return _meeting;
            }
        }
        public string MeetingTime
        {
            get
            {
                return meeting.MeetingDate.Value.ToString("ddd, MMM d h:mm tt");
            }
        }
        private bool? _filled;
        private bool filled
        {
            get
            {
                if (!_filled.HasValue && meeting != null)
                    _filled = meeting.Organization.ClassFilled == true;
                return _filled ?? false;
            }
        }
        public string disabled
        {
            get
            {
                return filled ? "disabled = \"disabled\"" : "";
            }
        }
        public string Description
        {
            get
            {
                if (meeting == null)
                    return "No Events upcoming";
                else if (filled)
                    return "Event is Filled, Sorry";
                else
                    return "Register for {0} Event on {1}".Fmt(meeting.Organization.OrganizationName, MeetingTime);
            }
        }
        internal MOBSReg registration;
        public int? regid
        {
            get
            {
                return registration.Id;
            }
            set
            {
                registration = DbUtil.Db.MOBSRegs.Single(d => d.Id == value);
                peopleid = registration.PeopleId;
                meetingid = registration.MeetingId;
                tickets = registration.NumTickets;
                email = registration.Email;
            }
        }

        private Person _person;
        public Person person
        {
            get
            {
                if (_person == null)
                    _person = DbUtil.Db.People.SingleOrDefault(p => p.PeopleId == peopleid);
                return _person;
            }
        }
        public string first { get; set; }
        public string last { get; set; }
        public string dob { get; set; }
        public DateTime birthday;

        public bool shownew { get; set; }
        public string addr { get; set; }
        public string zip { get; set; }
        public string city { get; set; }
        public string state { get; set; }

        public int FindMember()
        {
            first = first.Trim();
            last = last.Trim();
            var fone = Util.GetDigits(phone);
            var q = from p in DbUtil.Db.People
                    where (p.FirstName == first || p.NickName == first || p.MiddleName == first)
                    where (p.LastName == last || p.MaidenName == last)
                    where p.BirthDay == birthday.Day && p.BirthMonth == birthday.Month && p.BirthYear == birthday.Year
                    select p;
            var count = q.Count();
            if (count > 1)
                q = from p in q
                    where p.CellPhone.Contains(fone)
                            || p.WorkPhone.Contains(fone)
                            || p.Family.HomePhone.Contains(fone)
                    select p;
            count = q.Count();

            peopleid = null;
            if (count == 1)
                peopleid = q.Select(p => p.PeopleId).Single();
            return count;
        }

        public void ValidateModel(ModelStateDictionary modelState)
        {
            first = first.Trim();
            last = last.Trim();
            if (!first.HasValue())
                modelState.AddModelError("first", "first name required");
            if (!last.HasValue())
                modelState.AddModelError("last", "last name required");
            if (!Util.DateValid(dob, out birthday))
                modelState.AddModelError("dob", "valid birth date required");

            var d = phone.GetDigits().Length;
            if (d != 7 && d != 10)
                modelState.AddModelError("phone", "7 or 10 digits");
            if (!email.HasValue() || !Util.ValidEmail(email))
                modelState.AddModelError("email", "Please specify a valid email address.");
            //if (!gender.HasValue)
            //    modelState.AddModelError("gender", "gender required");
            if (shownew)
            {
                if (!addr.HasValue())
                    modelState.AddModelError("addr", "need address");
                if (zip.GetDigits().Length != 5)
                    modelState.AddModelError("zip", "need 5 digit zip");
                if (!city.HasValue())
                    modelState.AddModelError("city", "need city");
                if (!state.HasValue())
                    modelState.AddModelError("state", "need state");
            }
        }
        internal void AddPerson()
        {
            var f = new Family
            {
                AddressLineOne = addr,
                CityName = city,
                StateCode = state,
                ZipCode = zip,
            };
            var p = Person.Add(f, 30,
                null, first, null, last, dob, false, 1,
                    DbUtil.Settings("RecOrigin", "0").ToInt(),
                    DbUtil.Settings("RecEntry", "0").ToInt());
            p.MaritalStatusId = (int)Person.MaritalStatusCode.Unknown;
            p.EmailAddress = email;
            p.CampusId = DbUtil.Settings("DefaultCampusId", "").ToInt2();
            if (p.Age >= 18)
                p.PositionInFamilyId = (int)Family.PositionInFamily.PrimaryAdult;
            switch (homecell)
            {
                case "h":
                    f.HomePhone = phone.GetDigits();
                    break;
                case "c":
                    p.CellPhone = phone.GetDigits();
                    break;
            }
            DbUtil.Db.SubmitChanges();
            peopleid = p.PeopleId;
        }
        internal void AttendEvent()
        {
            Attend.RecordAttendance(peopleid.Value, meeting.MeetingId, true);
        }
        public IEnumerable<Attendee> Attendees()
        {
            var q = from a in meeting.Attends
                    where a.AttendanceFlag == true
                    join r in DbUtil.Db.MOBSRegs 
                        on new { a.PeopleId, a.MeetingId }
                        equals new { PeopleId = r.PeopleId.Value, MeetingId = r.MeetingId.Value }
                    select new Attendee
                    {
                        PeopleId = a.PeopleId,
                        Name = a.Person.Name,
                        RegisteredOn = r.Created.Value,
                        Tickets = r.NumTickets,
                        TransactionId = r.TransactionId,
                    };
            return q;
        }
    }
    public class Attendee
    {
        public int PeopleId { get; set; }
        public string Name { get; set; }
        public int Tickets { get; set; }
        public string TransactionId { get; set; }
        public DateTime RegisteredOn { get; set; }
    }
}
