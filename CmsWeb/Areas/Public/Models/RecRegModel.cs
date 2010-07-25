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
using System.Net.Mail;

namespace CmsWeb.Models
{
    [Serializable]
    public class RecRegModel
    {
        public bool ended { get; set; }
        public string first { get; set; }
        public string last { get; set; }
        public string suffix { get; set; }
        public string dob { get; set; }
        public string phone { get; set; }
        public string homecell { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public int? gender { get; set; }
        public int? married { get; set; }

        public bool? Found { get; set; }
        public bool IsNew { get; set; }
        public bool OtherOK { get; set; }
        public bool ShowAddress { get; set; }

        private DateTime _Birthday;
        public DateTime birthday
        {
            get
            {
                if (_Birthday == DateTime.MinValue)
                    Util.DateValid(dob, out _Birthday);
                return _Birthday;
            }
        }
        public int age
        {
            get { return birthday.AgeAsOf(Util.Now); }
        }
        public string genderdisplay
        {
            get { return gender == 1 ? "Male" : "Female"; }
        }
        public string marrieddisplay
        {
            get { return married == 10 ? "Single" : "Married"; }
        }
        public void ValidateModelForFind(ModelStateDictionary ModelState)
        {
            CmsWeb.Models.SearchPeopleModel
                .ValidateFindPerson(ModelState, first, last, birthday, email, phone);
            if (!phone.HasValue())
                ModelState.AddModelError("phone", "phone required");
            if (!email.HasValue() || !Util.ValidEmail(email))
                ModelState.AddModelError("email", "Please specify a valid email address.");
            if (ModelState.IsValid)
            {
                Found = person != null;
                if (count == 1)
                {
                    address = person.PrimaryAddress;
                    city = person.PrimaryCity;
                    state = person.PrimaryState;
                    zip = person.PrimaryZip;
                    gender = person.GenderId;
                    married = person.MaritalStatusId == 2 ? 2 : 1;
                }
                else if (count > 1)
                    ModelState.AddModelError("find", "More than one match, sorry");
                else if (count == 0)
                    ModelState.AddModelError("find", "record not found");
            }
        }
        public int? PeopleId { get; set; }
        private int count;
        [NonSerialized]
        private Person _Person;
        public Person person
        {
            get
            {
                if (_Person == null)
                    if (PeopleId.HasValue)
                        _Person = DbUtil.Db.LoadPersonById(PeopleId.Value);
                    else
                    {
                        _Person = SearchPeopleModel.FindPerson(first, last, birthday, string.Empty, phone, out count);
                        if (_Person != null)
                            PeopleId = _Person.PeopleId;
                    }
                return _Person;
            }
        }

        public OrganizationMember GetOrgMember()
        {
            var om = DbUtil.Db.OrganizationMembers.SingleOrDefault(m2 =>
                m2.PeopleId == PeopleId && m2.OrganizationId == RecAgeDiv.OrganizationId);
            return om;
        }
        internal void ValidateModelForNew(ModelStateDictionary ModelState)
        {
            CmsWeb.Models.SearchPeopleModel
                .ValidateFindPerson(ModelState, first, last, birthday, email, phone);
            if (!phone.HasValue())
                ModelState.AddModelError("phone", "phone required");
            if (!email.HasValue() || !Util.ValidEmail(email))
                ModelState.AddModelError("email", "Please specify a valid email address.");
            if (!address.HasValue())
                ModelState.AddModelError("address", "address required.");
            if (!city.HasValue())
                ModelState.AddModelError("city", "city required.");
            if (zip.GetDigits().Length < 5)
                ModelState.AddModelError("zip", "zip needs at least 5 digits.");
            if (!state.HasValue())
                ModelState.AddModelError("state", "state required");
            if (!gender.HasValue)
                ModelState.AddModelError("gender", "Please specify gender");
            if (!married.HasValue)
                ModelState.AddModelError("married", "Please specify marital status");
        }
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0}({1},{2},{3}), Birthday: {4}({5}), Phone: {6}, {7}<br />\n".Fmt(
                person.Name, person.PeopleId, person.Gender.Code, person.MaritalStatus.Code,
                person.DOB, person.Age, phone.FmtFone(homecell), email));
            if (ShowAddress)
                sb.AppendFormat("&nbsp;&nbsp;{0}; {1}<br />\n", person.PrimaryAddress, person.CityStateZip);
            return sb.ToString();
        }
        internal void AddPerson(int entrypoint)
        {
            Family f = new Family
            {
                AddressLineOne = address,
                CityName = city,
                StateCode = state,
                ZipCode = zip,
            };

            _Person = Person.Add(f, 30,
                null, first.Trim(), null, last.Trim(), dob, married == 20, gender ?? 0,
                    (int)Person.OriginCode.Enrollment, entrypoint);
            person.EmailAddress = email;
            person.SuffixCode = suffix;
            person.CampusId = DbUtil.Settings("DefaultCampusId", "").ToInt2();
            if (person.Age >= 18)
                person.PositionInFamilyId = (int)Family.PositionInFamily.PrimaryAdult;
            switch (homecell)
            {
                case "h":
                    f.HomePhone = phone.GetDigits();
                    break;
                case "c":
                    person.CellPhone = phone.GetDigits();
                    break;
            }
            DbUtil.Db.SubmitChanges();
            DbUtil.Db.Refresh(RefreshMode.OverwriteCurrentValues, person);
        }

        public int? divid { get; set; }
        public int orgid { get; set; }
        public decimal? amtpaid { get; set; }

        [NonSerialized]
        private Division _division;
        public Division division
        {
            get
            {
                if (_division == null)
                    _division = DbUtil.Db.Divisions.SingleOrDefault(d => d.Id == divid);
                return _division;
            }
        }
        public bool IsFilled { get; set; }
        [NonSerialized]
        private CmsData.Organization _RecAgeDiv;
        public CmsData.Organization RecAgeDiv
        {
            get
            {
                if (_RecAgeDiv == null)
                    _RecAgeDiv = GetRecAgeDivision();
                return _RecAgeDiv;
            }
        }

        public string shirtsize { get; set; }
        public string emcontact { get; set; }
        public string emphone { get; set; }
        public string insurance { get; set; }
        public string policy { get; set; }
        public string doctor { get; set; }
        public string docphone { get; set; }
        public string medical { get; set; }
        public string mname { get; set; }
        public string fname { get; set; }
        public bool member { get; set; }
        public bool otherchurch { get; set; }
        public int? coaching { get; set; }

        public string request { get; set; }

        public decimal Amount
        {
            get
            {
                var league = RecAgeDiv.Division.RecLeagues.Single();
                var dt = DateTime.Parse(league.ExpirationDt);
                if (Util.Now.Subtract(dt).TotalDays > 180)
                    dt = dt.AddYears(1);
                var amt = RecAgeDiv.Fee.Value;
                if (Util.Now > dt.AddHours(24))
                    amt += league.ExtraFee ?? 0;
                if (shirtsize != "lastyear")
                    amt += league.ShirtFee ?? 0;
                return amt;
            }
        }

        public int TransactionId { get; set; }

        public void ValidateModelForOther(ModelStateDictionary modelState)
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

        public bool IsAdult()
        {
            var q = from r in DbUtil.Db.Organizations
                    where r.DivisionId == divid
                    where r.GenderId == gender || r.GenderId == 0
                    select r;
            var list = q.ToList();
            var q2 = from r in list
                     let league = r.Division.RecLeagues.Single()
                     let age = birthday.AgeAsOf(league.agedate)
                     where age >= r.GradeAgeStart && age <= r.GradeAgeEnd
                     where age > 18
                     select r;
            var rec = q2.SingleOrDefault();
            return rec != null;
        }
        public CmsData.Organization GetRecAgeDivision()
        {
            var q = from o in DbUtil.Db.Organizations
                    where o.DivisionId == divid
                    where gender == null || o.GenderId == gender || o.GenderId == 0
                    select o;
            var list = q.ToList();
            var q2 = from o in list
                     let league = o.Division.RecLeagues.Single()
                     let age = birthday.AgeAsOf(league.agedate)
                     where age >= o.GradeAgeStart && age <= o.GradeAgeEnd
                     select o;
            return q2.FirstOrDefault();
        }
        public static List<SelectListItem> ShirtSizes()
        {
            var q = from ss in DbUtil.Db.ShirtSizes
                    orderby ss.Id
                    select new SelectListItem
                    {
                        Value = ss.Code,
                        Text = ss.Description
                    };
            var list = q.ToList();
            list.Insert(0, new SelectListItem { Value = "0", Text = "(not specified)" });
            return list;
        }
        public static IEnumerable<SelectListItem> ShirtSizes(CmsData.Organization RecAgeDiv)
        {
            var list = ShirtSizes();
            if (RecAgeDiv != null)
            {
                var league = RecAgeDiv.Division.RecLeagues.Single();
                if (RecAgeDiv != null && league.ShirtFee > 0)
                    list.Add(new SelectListItem { Value = "lastyear", Text = "Use shirt from last year" });
            }
            else
                list.Add(new SelectListItem { Value = "lastyear", Text = "Use shirt from last year" });
            return list;
        }
        public string PrepareSummaryText(OrganizationMember om)
        {
            var sb = new StringBuilder();
            sb.Append("<table>");
            sb.AppendFormat("<tr><td>First:</td><td>{0}</td></tr>\n", person.PreferredName);
            sb.AppendFormat("<tr><td>Last:</td><td>{0}</td></tr>\n", person.LastName);

            sb.AppendFormat("<tr><td>DOB:</td><td>{0:d}</td></tr>\n", person.DOB);
            sb.AppendFormat("<tr><td>Gender:</td><td>{0}</td></tr>\n", person.GenderId == 1 ? "M" : "F");
            sb.AppendFormat("<tr><td>Addr:</td><td>{0}</td></tr>\n", person.PrimaryAddress);
            sb.AppendFormat("<tr><td>City:</td><td>{0}</td></tr>\n", person.PrimaryCity);
            sb.AppendFormat("<tr><td>State:</td><td>{0}</td></tr>\n", person.PrimaryState);
            sb.AppendFormat("<tr><td>Zip:</td><td>{0}</td></tr>\n", person.PrimaryZip.Zip5());
            sb.AppendFormat("<tr><td>Home Phone:</td><td>{0}</td></tr>\n", person.Family.HomePhone.FmtFone());
            sb.AppendFormat("<tr><td>Cell Phone:</td><td>{0}</td></tr>\n", person.CellPhone.FmtFone());

            var rr = person.RecRegs.Single();
            sb.AppendFormat("<tr><td>Shirt:</td><td>{0}</td></tr>\n", rr.ShirtSize);
            sb.AppendFormat("<tr><td>Email:</td><td>{0}</td></tr>\n", rr.Email);
            sb.AppendFormat("<tr><td>Emerg Contact:</td><td>{0}</td></tr>\n", rr.Emcontact);
            sb.AppendFormat("<tr><td>Emerg Phone:</td><td>{0}</td></tr>\n", rr.Emphone);
            sb.AppendFormat("<tr><td>Physician Name:</td><td>{0}</td></tr>\n", rr.Doctor);
            sb.AppendFormat("<tr><td>Physician Phone:</td><td>{0}</td></tr>\n", rr.Docphone);
            sb.AppendFormat("<tr><td>Insurance Carrier:</td><td>{0}</td></tr>\n", rr.Insurance);
            sb.AppendFormat("<tr><td>Insurance Policy:</td><td>{0}</td></tr>\n", rr.Policy);
            sb.AppendFormat("<tr><td>Request:</td><td>{0}</td></tr>\n", om.Request);
            sb.AppendFormat("<tr><td>Medical:</td><td>{0}</td></tr>\n", rr.MedicalDescription);
            sb.AppendFormat("<tr><td>Member:</td><td>{0}</td></tr>\n", rr.Member);
            sb.AppendFormat("<tr><td>OtherChurch:</td><td>{0}</td></tr>\n", rr.ActiveInAnotherChurch);

            sb.AppendFormat("<tr><td>Mother's name:</td><td>{0}</td></tr>\n", rr.Mname);
            sb.AppendFormat("<tr><td>Father's name:</td><td>{0}</td></tr>\n", rr.Fname);
            sb.AppendFormat("<tr><td>Coaching:</td><td>{0}</td></tr>\n", rr.Coaching);
            sb.AppendFormat("<tr><td>Amount Paid:</td><td>{0}</td></tr>\n", om.Amount);

            sb.Append("</table>");

            return sb.ToString();
        }
        public void Confirm(string TransactionID)
        {
            var org = DbUtil.Db.LoadOrganizationById(orgid);

            if (IsNew)
                AddPerson(org.EntryPointId ?? 0);
            var om = OrganizationMember.InsertOrgMembers(orgid, person.PeopleId,
                (int)OrganizationMember.MemberTypeCode.Member, DateTime.Now, null, false);

            var reg = person.RecRegs.SingleOrDefault();

            if (reg == null)
            {
                reg = new RecReg();
                person.RecRegs.Add(reg);
            }
            reg.ShirtSize = shirtsize;
            om.ShirtSize = shirtsize;
            reg.ActiveInAnotherChurch = otherchurch;
            reg.MedAllergy = medical.HasValue();
            reg.Member = member;
            reg.Mname = mname;
            reg.Fname = fname;
            reg.Emcontact = emcontact;
            reg.Emphone = emphone;
            reg.Docphone = docphone;
            reg.Doctor = doctor;
            reg.Coaching = coaching > 0;
            reg.Insurance = insurance;
            reg.Policy = policy;
            reg.MedicalDescription = medical;

            AddToRegistrationComments("-------------", reg);
            AddToRegistrationComments(email, reg);
            if (request.HasValue())
            {
                AddToRegistrationComments("Request: " + request, reg);
                om.Request = request;
            }
            om.Amount = amtpaid;
            AddToRegistrationComments("{0:C} ({1})".Fmt(om.Amount.Value.ToString("C"), TransactionId), reg);
            AddToRegistrationComments(Util.Now.ToString("MMM d yyyy h:mm tt"), reg);
            AddToRegistrationComments("{0} - {1}".Fmt(division.Name, org.OrganizationName), reg);

            DbUtil.Db.SubmitChanges();

            var staffemail = Util.PickFirst(org.EmailAddresses,
                DbUtil.Settings("RecMail", DbUtil.SystemEmailAddress));

            var smtp = Util.Smtp();
            var league = division.RecLeagues.Single();
            var subject = Util.PickFirst(league.EmailSubject, "Recreation Registration");
            var message = Util.PickFirst(league.EmailMessage,
                @"<p>Thank you for registering for {league}: {agedivision}
You will receive another email with team information once they have been established.</p>
<p>You will need to download the <a href=""{cmshost}/Upload/MedicalRelease.pdf"">Medical Release Form</a>,
print, sign, and return it to the Recreation Ministry in order to complete your registration.</p>
<p>We have the following information:
{summary}");
            message = message.Replace("{league}", division.Name);
            message = message.Replace("{agedivision}", org.OrganizationName);
            message = message.Replace("{cmshost}", Util.CmsHost);
            message = message.Replace("{summary}", PrepareSummaryText(om));

            Util.Email2(smtp, staffemail, email, subject, message);
            Util.Email2(smtp, email, staffemail,
                "{0} Registration".Fmt(division.Name),
                "{0}({1}) has registered for {2}<br/>Feepaid: {3:C}"
                .Fmt(person.Name, person.PeopleId, org.OrganizationName, om.Amount));

            Util.SendIfEmailDifferent(smtp, staffemail, email,
                person.PeopleId, person.Name, person.EmailAddress, subject, message);
        }
        private static void AddToMemberData(string s, OrganizationMember om)
        {
            if (om.UserData.HasValue())
                om.UserData += "\n";
            om.UserData += s;
        }
        private static void AddToRegistrationComments(string s, RecReg rr)
        {
            rr.Comments = s + "\n" + rr.Comments;
        }
    }
}
