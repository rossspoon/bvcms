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
using System.Drawing;
using CMSWebCommon.Models;

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
        public string TransactionId { get; set; }
        public string ServiceUOrgID
        {
            get
            {
                if ((string)HttpContext.Current.Session["test"] == "1")
                    return DbUtil.Settings("ServiceUOrgIDTest", "0");
                return DbUtil.Settings("ServiceUOrgID", "0");
            }
        }
        public string ServiceUOrgAccountID
        {
            get
            {
                if ((string)HttpContext.Current.Session["test"] == "1")
                    return DbUtil.Settings("ServiceUOrgAccountIDTest", "0");
                return DbUtil.Settings("ServiceUOrgAccountID", "0");
            }
        }

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
                            where m.MeetingDate > Util.Now.AddHours(-12)
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

        public bool shownew { get; set; }
        public string addr { get; set; }
        public string zip { get; set; }
        public string city { get; set; }
        public string state { get; set; }

        public int FindMember()
        {
            int count;
            _person = SearchPeopleModel.FindPerson(phone, first, last, birthday, out count);
            if(count == 1)
                peopleid = _person.PeopleId;
            return count;
        }

        public void ValidateModel(ModelStateDictionary modelState)
        {
            SearchPeopleModel.ValidateFindPerson(modelState, first, last, birthday, phone);

            if (!phone.HasValue())
                modelState.AddModelError("phone", "phone required");
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
                null, first.Trim(), null, last.Trim(), dob, false, 1,
                    DbUtil.Settings("RecOrigin", "0").ToInt(),
                    DbUtil.Settings("RecEntry", "0").ToInt());
            p.MaritalStatusId = (int)Person.MaritalStatusCode.Unknown;
            p.FixTitle();
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
