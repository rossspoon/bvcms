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
using CMSPresenter;

namespace CMSWeb.Models
{
    public class RecreationModel
    {
        public Division division { get; set; }
        public int? divid
        {
            get
            {
                return division.Id;
            }
            set
            {
                division = DbUtil.Db.Divisions.Single(d => d.Id == value);
            }
        }
        public Participant registration { get; set; }
        public int? regid
        {
            get
            {
                return registration.Id;
            }
            set
            {
                registration = DbUtil.Db.Participants.Single(d => d.Id == value);
                participant = registration.Person;
                OrgId = registration.OrgId;
                divid = registration.DivId;
                if (participant.Family.HeadOfHouseholdId != null)
                {
                    var p1 = participant.Family.HeadOfHousehold;
                    if (p1.GenderId == 1)
                        fname = p1.Name;
                    else
                        mname = p1.Name;
                    var p2 = participant.Family.HeadOfHouseholdSpouse;
                    if (p2 != null)
                        if (p2.GenderId == 1)
                            fname = p2.Name;
                        else
                            mname = p2.Name;
                }
            }
        }
        public CmsData.Organization organization { get; set; }
        public int? OrgId
        {
            get
            {
                return organization.OrganizationId;
            }
            set
            {
                organization = DbUtil.Db.Organizations.Single(o => o.OrganizationId == value);
            }
        }

        public int? gender { get; set; }
        public string first { get; set; }
        public string last { get; set; }
        public string dob { get; set; }
        private DateTime birthday;

        public string phone { get; set; }
        public string email { get; set; }
        public bool preferredEmail { get; set; }
        public bool shownew { get; set; }
        public string addr { get; set; }
        public string zip { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public Person participant { get; set; }

        public string shirtsize { get; set; }
        public string emcontact { get; set; }
        public string emphone { get; set; }
        public string insurance { get; set; }
        public string policy { get; set; }
        public string doctor { get; set; }
        public string docphone { get; set; }
        public string request { get; set; }
        public string medical { get; set; }
        public string mname { get; set; }
        public string fname { get; set; }
        public bool member { get; set; }
        public bool otherchurch { get; set; }
        public int? coaching { get; set; }

        public int FindMember()
        {
            var fone = Util.GetDigits(phone);
            var q = from p in DbUtil.Db.People
                    where (p.LastName == last || p.MaidenName == last)
                            && (p.FirstName == first
                            || p.NickName == first
                            || p.MiddleName == first)
                    where p.Family.People.Any(pp => pp.CellPhone.Contains(fone) || p.WorkPhone.Contains(fone)) || p.Family.HomePhone.Contains(fone)
                    where p.BirthDay == birthday.Day && p.BirthMonth == birthday.Month && p.BirthYear == birthday.Year
                    where p.GenderId == gender
                    select p;
            var count = q.Count();
            participant = null;
            if (count == 1)
                participant = q.Single();
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
            if (!DateTime.TryParse(dob, out birthday))
                modelState.AddModelError("dob", "valid birth date required");
            else if (birthday.Year == DateTime.Now.Year)
                modelState.AddModelError("dob", "valid birth year required");
            var d = phone.GetDigits().Length;
            if (d != 7 && d != 10)
                modelState.AddModelError("phone", "7 or 10 digits");
            if (!email.HasValue() || !Util.ValidEmail(email))
                modelState.AddModelError("email", "Please specify a valid email address.");
            if (!gender.HasValue)
                modelState.AddModelError("gender2", "gender required");
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
        public void ValidateModel2(ModelStateDictionary modelState)
        {
            if (!emcontact.HasValue())
                modelState.AddModelError("emcontact", "emergency contact required");
            if (!emphone.HasValue())
                modelState.AddModelError("emphone", "emergency phone # required");

            if (!insurance.HasValue())
                modelState.AddModelError("insurance", "insurance carrier required");
            if (!policy.HasValue())
                modelState.AddModelError("policy", "insurnace policy # required");
            
            if (!doctor.HasValue())
                modelState.AddModelError("doctor", "Doctor's name required");
            if (!docphone.HasValue())
                modelState.AddModelError("docphone", "Doctor's phone # required");

            if (shirtsize == "0")
                modelState.AddModelError("shirtsize", "please select a shirt size");
        }

        internal void EnrollInOrg(Person person)
        {
            OrgId = GetOrgId();
            var member = DbUtil.Db.OrganizationMembers.SingleOrDefault(om =>
                om.OrganizationId == 0 && om.PeopleId == person.PeopleId);
            if (member == null)
                OrganizationController.InsertOrgMembers(
                    OrgId.Value,
                    person.PeopleId,
                    (int)OrganizationMember.MemberTypeCode.Member,
                    DateTime.Today, null, false);
        }

        internal void AddPerson()
        {
            var f = new Family
            {
                AddressLineOne = addr,
                CityName = city,
                StateCode = state,
                ZipCode = zip,
                HomePhone = phone,
            };
            participant = Person.Add(f, 30,
                null, first, null, last, dob, false, gender.Value, 
                    DbUtil.Settings("RecOrigin").ToInt(), 
                    DbUtil.Settings("RecEntry").ToInt());
            DbUtil.Db.SubmitChanges();
        }

        private class RecItem
        {
            public int? StartAge { get; set; }
            public int? EndAge { get; set; }
            public int? OrgId { get; set; }
            public string AgeDate { get; set; }
            public DateTime agedate
            { 
                get 
                { 
                    var dt = DateTime.Parse(AgeDate);
                    if (dt.Subtract(DateTime.Now).TotalDays > 190)
                        dt = dt.AddYears(-1);
                    if (dt.Subtract(DateTime.Now).TotalDays < 190)
                        dt = dt.AddYears(1);
                    return dt;
                }
            }
        }
        internal int GetOrgId()
        {
            var q = from r in DbUtil.Db.Recreations
                    where r.DivId == divid
                    where r.GenderId == gender || r.GenderId == 0
                    select new RecItem
                    { 
                        OrgId = r.OrgId, 
                        StartAge = r.StartAge, 
                        EndAge = r.EndAge, 
                        AgeDate = r.AgeDate 
                    };
            var list = q.ToList();
            var q2 = from r in list
                     let age = birthday.AgeAsOf(r.agedate)
                     where age >= r.StartAge && age <= r.EndAge
                     select r.OrgId;
            return q2.Single().Value;
        }
        public IEnumerable<SelectListItem> StateList()
        {
            var q = from r in DbUtil.Db.StateLookups
                    select new SelectListItem
                    {
                        Text = r.StateCode,
                        Selected = r.StateCode == "TN",
                    };
            return q;
        }
        public IEnumerable<SelectListItem> ShirtSizes()
        {
            return new List<SelectListItem> 
            {
                new SelectListItem { Value="0", Text="(not specified)" },
                new SelectListItem { Value="yS", Text="Youth: Small (6-8)" },
                new SelectListItem { Value="yM", Text="Youth: Medium (10-12)" },
                new SelectListItem { Value="yL", Text="Youth: Large (14-16)" },
                new SelectListItem { Value="S", Text="Adult: Small" },
                new SelectListItem { Value="M", Text="Adult: Medium" },
                new SelectListItem { Value="L", Text="Adult: Large" },
                new SelectListItem { Value="X", Text="Adult: XLarge" },
            };
        }
        public string PrepareSummaryText()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("First:\t{0}\n", first);
            sb.AppendFormat("Last:\t{0}\n", last);
            sb.AppendFormat("Shirt:\t{0}\n", shirtsize);

            sb.AppendFormat("DOB:\t{0:d}\n", birthday);
            sb.AppendFormat("Gender:\t{0}\n", gender == 1 ? "M" : "F");
            sb.AppendFormat("Addr:\t{0}\n", addr);
            sb.AppendFormat("City:\t{0}\n", city);
            sb.AppendFormat("State:\t{0}\n", state);
            sb.AppendFormat("Zip:\t{0}\n", zip);
            sb.AppendFormat("Phone:\t{0}\n", phone.FmtFone());

            sb.AppendFormat("Email:\t{0}\n", email);
            sb.AppendFormat("Emerg Contact:\t{0}\n", emcontact);
            sb.AppendFormat("Emerg Phone:\t{0}\n", emphone.FmtFone());
            sb.AppendFormat("Physician Name:\t{0}\n", doctor);
            sb.AppendFormat("Physician Phone:\t{0}\n", docphone.FmtFone());
            sb.AppendFormat("Insurance Carrier:\t{0}\n", insurance);
            sb.AppendFormat("Insurance Policy:\t{0}\n", policy);
            sb.AppendFormat("Request:\t{0}\n", request);
            sb.AppendFormat("Medical:\t{0}\n", medical);
            sb.AppendFormat("Member:\t{0}\n", member);
            sb.AppendFormat("OtherChurch:\t{0}\n", otherchurch);

            return sb.ToString();
        }
    }
}
