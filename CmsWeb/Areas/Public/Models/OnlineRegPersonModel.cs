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

namespace CMSWeb.Models
{
    [Serializable]
    public class OnlineRegPersonModel
    {
        public int index { get; set; }
        public bool LastItem { get; set; }

        [NonSerialized]
        private CmsData.Organization _org;
        public CmsData.Organization org
        {
            get
            {
                if (_org == null && orgid.HasValue)
                    _org = DbUtil.Db.LoadOrganizationById(orgid.Value);
                return _org;
            }
        }

        public int? divid { get; set; }
        public int? orgid { get; set; }

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

        [NonSerialized]
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
            CMSWeb.Models.SearchPeopleModel
                .ValidateFindPerson(ModelState, first, last, birthday, phone);
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
                        _Person = SearchPeopleModel.FindPerson(phone, first, last, birthday, out count);
                        if (_Person != null)
                            PeopleId = _Person.PeopleId;
                    }
                return _Person;
            }
        }

        public OrganizationMember GetOrgMember()
        {
            var om = DbUtil.Db.OrganizationMembers.SingleOrDefault(m2 =>
                m2.PeopleId == PeopleId && m2.OrganizationId == orgid);
            return om;
        }
        public IEnumerable<SelectListItem> StateCodes()
        {
            var cv = new CodeValueController();
            return QueryModel.ConvertToSelect(cv.GetStateListUnknown(), "Code");
        }

        internal void ValidateModelForNew(ModelStateDictionary ModelState)
        {
            CMSWeb.Models.SearchPeopleModel
                .ValidateFindPerson(ModelState, first, last, birthday, phone);
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
        internal void AddPerson(Person p, int entrypoint)
        {
            Family f;
            if (p == null)
                f = new Family
                {
                    AddressLineOne = address,
                    CityName = city,
                    StateCode = state,
                    ZipCode = zip,
                };
            else
                f = p.Family;

            _Person = Person.Add(f, (int)Family.PositionInFamily.Child,
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

        public bool IsFilled { get; set; }

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
        public bool? coaching { get; set; }
        public bool? tylenol { get; set; }
        public bool? advil { get; set; }
        public bool? maalox { get; set; }
        public bool? robitussin { get; set; }
        public bool? paydeposit { get; set; }

        public string request { get; set; }

        public decimal Amount()
        {
            decimal amt;
            if (paydeposit == true && org.Deposit.HasValue && org.Deposit > 0)
                amt = org.Deposit.Value;
            else
                amt = org.Fee ?? 0;
            if (org.LastDayBeforeExtra.HasValue && org.ExtraFee.HasValue)
                if (Util.Now > org.LastDayBeforeExtra.Value.AddHours(24))
                    amt += org.ExtraFee.Value;
            if (shirtsize != "lastyear" && org.ShirtFee.HasValue)
                amt += org.ShirtFee.Value;
            return amt;
        }
        public decimal AmountDue()
        {
            return (org.Fee ?? 0) - Amount();
        }

        public void ValidateModelForOther(ModelStateDictionary modelState)
        {
            if (org.AskEmContact == true)
            {
                if (!emcontact.HasValue())
                    modelState.AddModelError("emcontact", "emergency contact required");
                if (!emphone.HasValue())
                    modelState.AddModelError("emphone", "emergency phone # required");
            }

            if (org.AskInsurance == true)
            {
                if (!insurance.HasValue())
                    modelState.AddModelError("insurance", "insurance carrier required");
                if (!policy.HasValue())
                    modelState.AddModelError("policy", "insurnace policy # required");
            }

            if (org.AskDoctor == true)
            {
                if (!doctor.HasValue())
                    modelState.AddModelError("doctor", "Doctor's name required");
                if (!docphone.HasValue())
                    modelState.AddModelError("docphone", "Doctor's phone # required");
            }
            if (org.AskTylenolEtc == true)
            {
                if (!tylenol.HasValue)
                    modelState.AddModelError("tylenol", "please indicate");
                if (!advil.HasValue)
                    modelState.AddModelError("advil", "please indicate");
                if (!maalox.HasValue)
                    modelState.AddModelError("maalox", "please indicate");
                if (!robitussin.HasValue)
                    modelState.AddModelError("robitussin", "please indicate");
            }

            if (org.AskShirtSize == true)
                if (shirtsize == "0")
                    modelState.AddModelError("shirtsize", "please select a shirt size");

            if (org.AskCoaching == true)
                if (!coaching.HasValue)
                    modelState.AddModelError("coaching", "please indicate");

            if (org.AskParents == true)
            {
                if (!mname.HasValue() && !fname.HasValue())
                    modelState.AddModelError("fname", "please provide either mother or father name");
                else
                {
                    string mfirst, mlast;
                    Person.NameSplit(mname, out mfirst, out mlast);
                    if (mname.HasValue() && !mfirst.HasValue())
                        modelState.AddModelError("mname", "provide first and last names");
                    string ffirst, flast;
                    Person.NameSplit(fname, out ffirst, out flast);
                    if (fname.HasValue() && !ffirst.HasValue())
                        modelState.AddModelError("fname", "provide first and last names");
                }
            }

            if (org.Deposit > 0)
                if (!paydeposit.HasValue)
                    modelState.AddModelError("paydeposit", "please indicate");
        }

        public List<SelectListItem> ShirtSizes()
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
            if (org != null && org.AllowLastYearShirt == true)
                list.Add(new SelectListItem { Value = "lastyear", Text = "Use shirt from last year" });
            return list;
        }
        public string PrepareSummaryText()
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

            if (org.AskShirtSize == true)
                sb.AppendFormat("<tr><td>Shirt:</td><td>{0}</td></tr>\n", rr.ShirtSize);
            if (org.AskEmContact == true)
            {
                sb.AppendFormat("<tr><td>Emerg Contact:</td><td>{0}</td></tr>\n", rr.Emcontact);
                sb.AppendFormat("<tr><td>Emerg Phone:</td><td>{0}</td></tr>\n", rr.Emphone.FmtFone());
            }
            if (org.AskDoctor == true)
            {
                sb.AppendFormat("<tr><td>Physician Name:</td><td>{0}</td></tr>\n", rr.Doctor);
                sb.AppendFormat("<tr><td>Physician Phone:</td><td>{0}</td></tr>\n", rr.Docphone);
            }
            if (org.AskInsurance == true)
            {
                sb.AppendFormat("<tr><td>Insurance Carrier:</td><td>{0}</td></tr>\n", rr.Insurance);
                sb.AppendFormat("<tr><td>Insurance Policy:</td><td>{0}</td></tr>\n", rr.Policy);
            }
            if (org.AskRequest == true)
                sb.AppendFormat("<tr><td>Request:</td><td>{0}</td></tr>\n", request);
            if (org.AskAllergies == true)
                sb.AppendFormat("<tr><td>Medical:</td><td>{0}</td></tr>\n", rr.MedicalDescription);

            if (org.AskTylenolEtc == true)
            {
                sb.AppendFormat("<tr><td>Tylenol:</td><td>{0}</td></tr>\n", rr.Tylenol == true ? "OK" : "Not OK");
                sb.AppendFormat("<tr><td>Advil:</td><td>{0}</td></tr>\n", rr.Advil == true ? "OK" : "Not OK");
                sb.AppendFormat("<tr><td>Maalox:</td><td>{0}</td></tr>\n", rr.Maalox == true ? "OK" : "Not OK");
                sb.AppendFormat("<tr><td>Robitussin:</td><td>{0}</td></tr>\n", rr.Robitussin == true ? "OK" : "Not OK");
            }
            if (org.AskChurch == true)
            {
                sb.AppendFormat("<tr><td>Member:</td><td>{0}</td></tr>\n", rr.Member);
                sb.AppendFormat("<tr><td>OtherChurch:</td><td>{0}</td></tr>\n", rr.ActiveInAnotherChurch);
            }
            if (org.AskParents == true)
            {
                sb.AppendFormat("<tr><td>Mother's name:</td><td>{0}</td></tr>\n", rr.Mname);
                sb.AppendFormat("<tr><td>Father's name:</td><td>{0}</td></tr>\n", rr.Fname);
            }
            if (org.AskCoaching == true)
                sb.AppendFormat("<tr><td>Coaching:</td><td>{0}</td></tr>\n", rr.Coaching);

            sb.AppendFormat("<tr><td>Amount Paid:</td><td>{0}</td></tr>\n", Amount());
            sb.Append("</table>");

            return sb.ToString();
        }
        public OrganizationMember Enroll(string TransactionID, string paylink)
        {
            var om = OrganizationMember.InsertOrgMembers(orgid.Value, person.PeopleId,
                (int)OrganizationMember.MemberTypeCode.Member, DateTime.Now, null, false);

            var reg = person.RecRegs.SingleOrDefault();

            if (reg == null)
            {
                reg = new RecReg();
                person.RecRegs.Add(reg);
            }
            if (org.AskShirtSize == true)
                reg.ShirtSize = shirtsize;
            if (org.AskChurch == true)
            {
                reg.ActiveInAnotherChurch = otherchurch;
                reg.Member = member;
            }
            if (org.AskAllergies == true)
            {
                reg.MedAllergy = medical.HasValue();
                reg.MedicalDescription = medical;
            }
            if (org.AskParents == true)
            {
                reg.Mname = mname;
                reg.Fname = fname;
            }
            if (org.AskEmContact == true)
            {
                reg.Emcontact = emcontact;
                reg.Emphone = emphone;
            }
            if (org.AskDoctor == true)
            {
                reg.Docphone = docphone;
                reg.Doctor = doctor;
            }
            if (org.AskCoaching == true)
                reg.Coaching = coaching;
            if (org.AskInsurance == true)
            {
                reg.Insurance = insurance;
                reg.Policy = policy;
            }
            if (org.AskTylenolEtc == true)
            {
                reg.Advil = advil;
                reg.Maalox = maalox;
                reg.Robitussin = robitussin;
                reg.Tylenol = tylenol;
            }

            AddToRegistrationComments("-------------", reg);
            AddToRegistrationComments(email, reg);
            if (request.HasValue())
            {
                AddToRegistrationComments("Request: " + request, reg);
                om.Request = request;
            }
            om.Amount = Amount();
            AddToRegistrationComments("{0:C} ({1})".Fmt(om.Amount.ToString2("C"), TransactionID), reg);
            if (paylink.HasValue())
                AddToRegistrationComments(paylink, reg);
            AddToRegistrationComments(Util.Now.ToString("MMM d yyyy h:mm tt"), reg);
            AddToRegistrationComments("{0} - {1}".Fmt(org.Division.Name, org.OrganizationName), reg);

            DbUtil.Db.SubmitChanges();
            return om;
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
        public CmsData.Organization GetAppropriateOrg()
        {
            if (org != null)
                return org;
            var q = from o in DbUtil.Db.Organizations
                    where o.DivisionId == divid
                    where gender == null || o.GenderId == gender || o.GenderId == 0
                    select o;
            var list = q.ToList();
            var q2 = from o in list
                     where birthday >= o.BirthDayStart || o.BirthDayStart == null
                     where birthday <= o.BirthDayEnd || o.BirthDayEnd == null
                     select o;
            return q2.FirstOrDefault();
        }
    }
}
