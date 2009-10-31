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
    [Serializable]
    public class RecRegModel
    {
        public bool testing { get; set; }
        public string ServiceUOrgID
        {
            get
            {
                if (testing)
                    return DbUtil.Settings("ServiceUOrgIDTest", "0");
                return DbUtil.Settings("ServiceUOrgID", "0");
            }
        }
        public string ServiceUOrgAccountID
        {
            get
            {
                if (testing)
                    return DbUtil.Settings("ServiceUOrgAccountIDTest", "0");
                return DbUtil.Settings("ServiceUOrgAccountID", "0");
            }
        }
        private Division _division { get; set; }
        public bool? _filled;
        public int? divid { get; set; }
        public Division division
        {
            get
            {
                if (_division == null)
                {
                    _division = DbUtil.Db.Divisions.SingleOrDefault(d => d.Id == divid);
                }
                return _division;
            }
        }
        public bool filled
        {
            get
            {
                if (!_filled.HasValue)
                {
                    var id = division.Id;
                    _filled = DbUtil.Db.Organizations.Any(o => 
                        o.DivOrgs.Any(od => od.DivId == id) && o.ClassFilled == true);
                }
                return _filled.Value;
            }
        }
        public string disabled
        {
            get
            {
                return filled? "disabled = \"disabled\"" : "";
            }
        }
        private RecAgeDivision _RecAgeDiv;
        public RecAgeDivision RecAgeDiv
        {
            get
            {
                if (_RecAgeDiv == null)
                    _RecAgeDiv = GetRecAgeDivision();
                return _RecAgeDiv;
            }
        }
        public int? regid { get; set; }
        private RecReg _registration;
        public RecReg registration 
        {
            get
            {
                if (_registration == null)
                {
                    _registration = DbUtil.Db.RecRegs.Single(d => d.Id == regid);
                    peopleid = registration.PeopleId;
                    orgid = _registration.OrgId;
                    divid = _registration.DivId;
                }
                return _registration;
            }
        }
        public int? orgid { get; set; }
        private CmsData.Organization _organization;
        public CmsData.Organization organization 
        {
            get
            {
                if(_organization == null)
                    _organization = DbUtil.Db.Organizations.SingleOrDefault(o => o.OrganizationId == orgid);
                return _organization;
            }
        }

        public int? peopleid { get; set; }
        private Person _Participant;
        public Person participant
        {
            get
            {
                if (_Participant == null && peopleid.HasValue)
                {
                    _Participant = DbUtil.Db.People.SingleOrDefault(p => p.PeopleId == peopleid);
                    if (_Participant != null && _Participant.PeopleId != _Participant.Family.HeadOfHouseholdId
                        && _Participant.Family.HeadOfHouseholdId != null)
                    {
                        var p1 = _Participant.Family.HeadOfHousehold;
                        if (p1.GenderId == 1)
                            fname = p1.Name;
                        else
                            mname = p1.Name;
                        var p2 = _Participant.Family.HeadOfHouseholdSpouse;
                        if (p2 != null)
                            if (p2.GenderId == 2)
                                mname = p2.Name;
                            else
                                fname = p2.Name;
                    }
                }
                return _Participant;
            }
        }
        public int? gender { get; set; }
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

        public string phone { get; set; }
        public string homecell { get; set; }
        public string email { get; set; }
        public bool preferredEmail { get; set; }
        public bool shownew { get; set; }
        public string addr { get; set; }
        public string zip { get; set; }
        public string city { get; set; }
        public string state { get; set; }

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
        public decimal? Amount
        {
            get
            {
                var dt = DateTime.Parse(RecAgeDiv.ExpirationDt);
                return RecAgeDiv.Fee + (dt < DateTime.Now ? RecAgeDiv.ExtraFee : 0);
            }
        }

        public int TransactionId { get; set; }

        public int FindMember()
        {
            first = first.Trim();
            last = last.Trim();
            var fone = Util.GetDigits(phone);
            var q = from p in DbUtil.Db.People
                    where (p.FirstName == first || p.NickName == first || p.MiddleName == first)
                    where (p.LastName == last || p.MaidenName == last)
                    where p.BirthDay == birthday.Day && p.BirthMonth == birthday.Month && p.BirthYear == birthday.Year
                    where p.GenderId == gender
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
            if (!first.HasValue())
                modelState.AddModelError("first", "first name required");
            if (!last.HasValue())
                modelState.AddModelError("last", "last name required");
            if (birthday.Equals(DateTime.MinValue))
                modelState.AddModelError("dob", "valid birth date required");

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
            if (GetRecAgeDivision() == null)
                modelState.AddModelError("find", "Sorry, cannot find an appropriate age division");
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

            if (!IsAdult() && !mname.HasValue() && !fname.HasValue())
                modelState.AddModelError("fname", "please provide either mother or father name");
        }

        internal bool EnrollInOrg(Person person)
        {
            if (RecAgeDiv == null)
                return false;
            var oid = RecAgeDiv.OrgId;
            orgid = oid;
            var member = DbUtil.Db.OrganizationMembers.SingleOrDefault(om =>
                om.OrganizationId == orgid && om.PeopleId == person.PeopleId);
            if (member == null)
                OrganizationMember.InsertOrgMembers(
                    orgid.Value,
                    person.PeopleId,
                    (int)OrganizationMember.MemberTypeCode.Member,
                    DateTime.Today, null, false);
            return true;
        }

        internal void AddPerson()
        {
            var org = DbUtil.Db.Organizations.SingleOrDefault(o => o.OrganizationId == RecAgeDiv.OrgId);
            var f = new Family
            {
                AddressLineOne = addr,
                CityName = city,
                StateCode = state,
                ZipCode = zip,
            };
            _Participant = Person.Add(f, (int)Family.PositionInFamily.Child,
                null, first, null, last, dob, false, gender.Value,
                    DbUtil.Settings("RecOrigin", "0").ToInt(), 
                    DbUtil.Settings("RecEntry", "0").ToInt());
            participant.MaritalStatusId = (int)Person.MaritalStatusCode.Unknown;
            participant.FixTitle();
            if (participant.Age >= 18)
                participant.PositionInFamilyId = (int)Family.PositionInFamily.PrimaryAdult;
            peopleid = participant.PeopleId;
            participant.EmailAddress = email;
            participant.CampusId = org.CampusId;
            switch (homecell)
            {
                case "h":
                    f.HomePhone = phone.GetDigits();
                    break;
                case "c":
                    participant.CellPhone = phone.GetDigits();
                    break;
            }
            DbUtil.Db.SubmitChanges();
        }
        public bool IsAdult()
        {
            var q = from r in DbUtil.Db.RecAgeDivisions
                    where r.DivId == divid
                    where r.GenderId == gender || r.GenderId == 0
                    select r;
            var list = q.ToList();
            var q2 = from r in list
                     let age = birthday.AgeAsOf(r.agedate)
                     where age >= r.StartAge && age <= r.EndAge
                     where age > 18
                     select r;
            var rec = q2.SingleOrDefault();
            return rec != null;
        }
        internal RecAgeDivision GetRecAgeDivision()
        {
            var q = from r in DbUtil.Db.RecAgeDivisions
                    where r.DivId == divid
                    where r.GenderId == gender || r.GenderId == 0
                    select r;
            var list = q.ToList();
            var q2 = from r in list
                     let age = birthday.AgeAsOf(r.agedate)
                     where age >= r.StartAge && age <= r.EndAge
                     select r;
            return q2.SingleOrDefault();
        }
        public static IEnumerable<SelectListItem> ShirtSizes()
        {
            return new List<SelectListItem> 
            {
                new SelectListItem { Value="0", Text="(not specified)" },
                new SelectListItem { Value="YT-S", Text="Youth: Small (6-8)" },
                new SelectListItem { Value="YT-M", Text="Youth: Medium (10-12)" },
                new SelectListItem { Value="YT-L", Text="Youth: Large (14-16)" },
                new SelectListItem { Value="AD-S", Text="Adult: Small" },
                new SelectListItem { Value="AD-M", Text="Adult: Medium" },
                new SelectListItem { Value="AD-L", Text="Adult: Large" },
                new SelectListItem { Value="AD-XL", Text="Adult: XLarge" },
            };
        }
        public string PrepareSummaryText()
        {
            var sb = new StringBuilder();
            sb.Append("<table>");
            sb.AppendFormat("<tr><td>First:</td><td>{0}</td></tr>\n", participant.NickName.HasValue()? participant.NickName : participant.FirstName);
            sb.AppendFormat("<tr><td>Last:</td><td>{0}</td></tr>\n", participant.LastName);
            sb.AppendFormat("<tr><td>Shirt:</td><td>{0}</td></tr>\n", shirtsize);

            sb.AppendFormat("<tr><td>DOB:</td><td>{0:d}</td></tr>\n", participant.DOB);
            sb.AppendFormat("<tr><td>Gender:</td><td>{0}</td></tr>\n", participant.GenderId == 1 ? "M" : "F");
            sb.AppendFormat("<tr><td>Addr:</td><td>{0}</td></tr>\n", participant.PrimaryAddress);
            sb.AppendFormat("<tr><td>City:</td><td>{0}</td></tr>\n", participant.PrimaryCity);
            sb.AppendFormat("<tr><td>State:</td><td>{0}</td></tr>\n", participant.PrimaryState);
            sb.AppendFormat("<tr><td>Zip:</td><td>{0}</td></tr>\n", participant.PrimaryZip.Zip5());
            sb.AppendFormat("<tr><td>Home Phone:</td><td>{0}</td></tr>\n", participant.Family.HomePhone.FmtFone());
            sb.AppendFormat("<tr><td>Cell Phone:</td><td>{0}</td></tr>\n", participant.CellPhone.FmtFone());

            sb.AppendFormat("<tr><td>Email:</td><td>{0}</td></tr>\n", email);
            sb.AppendFormat("<tr><td>Emerg Contact:</td><td>{0}</td></tr>\n", emcontact);
            sb.AppendFormat("<tr><td>Emerg Phone:</td><td>{0}</td></tr>\n", emphone);
            sb.AppendFormat("<tr><td>Physician Name:</td><td>{0}</td></tr>\n", doctor);
            sb.AppendFormat("<tr><td>Physician Phone:</td><td>{0}</td></tr>\n", docphone);
            sb.AppendFormat("<tr><td>Insurance Carrier:</td><td>{0}</td></tr>\n", insurance);
            sb.AppendFormat("<tr><td>Insurance Policy:</td><td>{0}</td></tr>\n", policy);
            sb.AppendFormat("<tr><td>Request:</td><td>{0}</td></tr>\n", request);
            sb.AppendFormat("<tr><td>Medical:</td><td>{0}</td></tr>\n", medical);
            sb.AppendFormat("<tr><td>Member:</td><td>{0}</td></tr>\n", member);
            sb.AppendFormat("<tr><td>OtherChurch:</td><td>{0}</td></tr>\n", otherchurch);

            sb.AppendFormat("<tr><td>Mother's name:</td><td>{0}</td></tr>\n", mname);
            sb.AppendFormat("<tr><td>Father's name:</td><td>{0}</td></tr>\n", fname);
            sb.AppendFormat("<tr><td>Coaching:</td><td>{0}</td></tr>\n", coaching);

            sb.Append("</table>");

            return sb.ToString();
        }
    }
}
