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
using System.Collections;

namespace CmsWeb.Models
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
                if (_org == null && classid.HasValue)
                    _org = DbUtil.Db.LoadOrganizationById(classid.Value);
                if (_org == null && divid.HasValue && (Found == true || IsValidForNew))
                    _org = GetAppropriateOrg();
                return _org;
            }
        }

        public int? divid { get; set; }
        public int? orgid { get; set; }
        public int? classid { get; set; }

        public string first { get; set; }
        public string last { get; set; }
        public string suffix { get; set; }
        public string dob { get; set; }
        public string phone { get; set; }
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
        public int TryCount { get; set; }
        public bool ShowAddress { get; set; }
        private Dictionary<string, string> _ExtraQuestion = new Dictionary<string, string>();
        public Dictionary<string, string> ExtraQuestion
        {
            get { return _ExtraQuestion; }
            set { _ExtraQuestion = value; }
        }
        public string ExtraQuestionValue(string s)
        {
            if (ExtraQuestion.ContainsKey(s))
                return ExtraQuestion[s];
            return null;
        }
        private Dictionary<string, bool?> _YesNoQuestion = new Dictionary<string, bool?>();
        public Dictionary<string, bool?> YesNoQuestion
        {
            get { return _YesNoQuestion; }
            set { _YesNoQuestion = value; }
        }
        public string YesNoChecked(string key, bool value)
        {
            if (YesNoQuestion != null && YesNoQuestion.ContainsKey(key))
                return YesNoQuestion[key] == value ? "checked='checked'" : "";
            return "";
        }

        [NonSerialized]
        private DateTime _Birthday;
        public DateTime? birthday
        {
            get
            {
                if (_Birthday == DateTime.MinValue)
                    Util.DateValid(dob, out _Birthday);
                return _Birthday == DateTime.MinValue ? (DateTime?)null : _Birthday;
            }
        }
        public int age
        {
            get
            {
                if (person != null && person.BirthDate.HasValue)
                    return person.BirthDate.Value.AgeAsOf(Util.Now);
                return _Birthday.AgeAsOf(Util.Now);
            }
        }
        public string genderdisplay
        {
            get { return gender == 1 ? "Male" : "Female"; }
        }
       public string marrieddisplay
        {
            get { return married == 10 ? "Single" : "Married"; }
        }
        public bool IsValidForExisting { get; set; }
        public bool IsValidForContinue { get; set; }
        public void ValidateModelForFind(ModelStateDictionary ModelState)
        {
            IsValidForContinue = true; // true till proven false
            CmsWeb.Models.SearchPeopleModel
                .ValidateFindPerson(ModelState, first, last, birthday, email, phone);
            if (UserSelectsOrganization())
                if ((classid ?? 0) == 0)
                    ModelState.AddModelError("classid", "choose a class");
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

                    if (ComputesOrganizationByAge() && org == null)
                    {
                        ModelState.AddModelError("dob", "Sorry, cannot find an appropriate age group");
                        IsValidForContinue = false;
                    }
                    else if (MemberOnly() && person.MemberStatusId != (int)Person.MemberStatusCode.Member)
                    {
                        ModelState.AddModelError("find", "Sorry, must be a member of church");
                        IsValidForContinue = false;
                    }
                    else if (org != null)
                    {
                        var m = org.OrganizationMembers.SingleOrDefault(mm => mm.PeopleId == PeopleId);
                        if (m != null)
                        {
                            ModelState.AddModelError("find", "This person is already registered");
                            IsValidForContinue = false;
                        }
                        else if (org.ValidateOrgs.HasValue())
                        {
                            var q = from s in org.ValidateOrgs.Split(',')
                                    select s.ToInt();
                            var a = q.ToArray();
                            if (!person.OrganizationMembers.Any(om => a.Contains(om.OrganizationId)))
                            {
                                ModelState.AddModelError("find", "Must be member of specified organization");
                                IsValidForContinue = false;
                            }
                        }
                    }
                }
                else if (count > 1)
                {
                    ModelState.AddModelError("find", "More than one match, sorry");
                    IsValidForContinue = false;
                }
                else if (count == 0)
                {
                    ModelState.AddModelError("find", "record not found");
                    NotFoundText = CmsWeb.Models.SearchPeopleModel.NotFoundText;
                }
            }
            IsValidForExisting = ModelState.IsValid;
        }
        public string NotFoundText;
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
                        _Person = SearchPeopleModel.FindPerson(first, last, birthday, email, phone, out count);
                        if (_Person != null)
                            PeopleId = _Person.PeopleId;
                    }
                return _Person;
            }
        }

        public OrganizationMember GetOrgMember()
        {
            if (org != null)
                return DbUtil.Db.OrganizationMembers.SingleOrDefault(m2 => 
                    m2.PeopleId == PeopleId && m2.OrganizationId == org.OrganizationId);
            return null;
        }
        public IEnumerable<SelectListItem> StateCodes()
        {
            var cv = new CodeValueController();
            return QueryModel.ConvertToSelect(cv.GetStateListUnknown(), "Code");
        }
        public IEnumerable<SelectListItem> Classes()
        {
            var q = from o in DbUtil.Db.Organizations
                    where o.DivisionId == divid
                    where o.ClassFilled != true
                    where o.RegistrationTypeId == (int)CmsData.Organization.RegistrationEnum.UserSelectsOrganization
                    where (o.Limit ?? 0) == 0 || o.Limit > o.MemberCount
                    orderby o.OnLineCatalogSort, o.OrganizationName
                    select new SelectListItem
                    {
                        Value = o.OrganizationId.ToString(),
                        Text = o.OrganizationName,
                    };
            var list = q.ToList();
            list.Insert(0, new SelectListItem { Value = "0", Text = "(not specifed)" });
            return list;
        }

        public bool IsValidForNew { get; set; }
        internal void ValidateModelForNew(ModelStateDictionary ModelState)
        {
            CmsWeb.Models.SearchPeopleModel
                .ValidateFindPerson(ModelState, first, last, birthday, email, phone);
            if (!birthday.HasValue && DbUtil.Settings("DobNotRequired", "true") == "true")
                ModelState.AddModelError("DOB", "birthday required");
            if (!phone.HasValue())
                ModelState.AddModelError("phone", "phone required");
            if (email.HasValue())
                email = email.Trim();
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
            IsValidForNew = ModelState.IsValid;
        }
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0}({1},{2},{3}), Birthday: {4}({5}), Phone: {6}, {7}<br />\n".Fmt(
                person.Name, person.PeopleId, person.Gender.Code, person.MaritalStatus.Code,
                person.DOB, person.Age, phone.FmtFone(), email));
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
            if (f.HomePhone.HasValue())
                person.CellPhone = phone.GetDigits();
            else
                f.HomePhone = phone.GetDigits();

            DbUtil.Db.SubmitChanges();
            DbUtil.Db.Refresh(RefreshMode.OverwriteCurrentValues, person);
            PeopleId = person.PeopleId;
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
        public bool memberus { get; set; }
        public bool otherchurch { get; set; }
        public bool? coaching { get; set; }
        public bool? tylenol { get; set; }
        public bool? advil { get; set; }
        public bool? maalox { get; set; }
        public bool? robitussin { get; set; }
        public bool? paydeposit { get; set; }
        public string request { get; set; }
        public string grade { get; set; }
        public int? ntickets { get; set; }
        public string option { get; set; }
        public string option2 { get; set; }
        public string gradeoption { get; set; }

        public decimal AmountToPay()
        {
            if (paydeposit == true && org.Deposit.HasValue && org.Deposit > 0)
                return org.Deposit.Value;
            return TotalAmount();
        }
        public decimal AmountDue()
        {
            return TotalAmount() - AmountToPay();
        }
        public decimal TotalAmount()
        {
            if (org == null)
                return 0;
            decimal amt = 0;
            if (amt == 0 && org.AskTickets == true)
                amt = (org.Fee ?? 0) * (ntickets ?? 0);
            if (amt == 0 && org.AskOptions.HasValue())
            {
                var q = from o in org.AskOptions.Split(',')
                        let a = o.Split('=')
                        where option == a[0] && a.Length > 1
                        select decimal.Parse(a[1]);
                if (q.Count() > 0)
                    amt = q.First();
            }
            if (amt == 0 && org.AgeFee.HasValue())
            {
                var q = from o in org.AgeFee.Split(',')
                        let b = o.Split('=')
                        let a = b[0].Split('-')
                        where b.Length > 1
                        where age >= a[0].ToInt()
                        where a.Length > 1 && age <= a[1].ToInt()
                        select decimal.Parse(b[1]);
                if (q.Count() > 0)
                    amt = q.First();
            }
            if (amt == 0 && org.OrgMemberFees.HasValue())
            {
                var q = from o in org.OrgMemberFees.Split(',')
                        let b = o.Split('=')
                        where b.Length > 1
                        where person != null && person.OrganizationMembers.Any(om => om.OrganizationId.ToString() == b[0])
                        select decimal.Parse(b[1]);
                if (q.Count() > 0)
                    amt = q.First();
            }
            if (amt == 0)
                amt = org.Fee ?? 0;
            if (org.LastDayBeforeExtra.HasValue && org.ExtraFee.HasValue)
                if (Util.Now > org.LastDayBeforeExtra.Value.AddHours(24))
                    amt += org.ExtraFee.Value;
            if (shirtsize != "lastyear" && org.ShirtFee.HasValue)
                amt += org.ShirtFee.Value;
            return amt;
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

            if (org.AskGrade == true)
            {
                int g = 0;
                if (!int.TryParse(grade, out g))
                    modelState.AddModelError("grade", "please enter a grade");
                else if (g < org.GradeAgeStart || g > org.GradeAgeEnd)
                    modelState.AddModelError("grade", "only grades from {0} to {1}".Fmt(org.GradeAgeStart, org.GradeAgeEnd));
            }

            if (org.AskCoaching == true)
                if (!coaching.HasValue)
                    modelState.AddModelError("coaching", "please indicate");

            if (org.AskOptions.HasValue())
                if (option == "0")
                    modelState.AddModelError("option", "please select an option");

            if (org.ExtraOptions.HasValue())
                if (option2 == "0")
                    modelState.AddModelError("option2", "please select an option");

            if (org.GradeOptions.HasValue())
                if (gradeoption == "00")
                    modelState.AddModelError("gradeoption", "please select a grade option");

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
            if (org.AskTickets == true)
                if ((ntickets ?? 0) == 0)
                    modelState.AddModelError("ntickets", "please enter a number of tickets");

            if (org.Deposit > 0)
                if (!paydeposit.HasValue)
                    modelState.AddModelError("paydeposit", "please indicate");

            foreach (var a in YesNoQuestions())
            {
                if (YesNoQuestion == null || !YesNoQuestion.ContainsKey(a.name))
                    modelState.AddModelError(a.name + "-YNError", "please select yes or no");
            }
        }

        public List<SelectListItem> ShirtSizes()
        {
            return ShirtSizes(org);
        }
        public class YesNoQuestionItem
        {
            public string name { get; set; }
            public string desc { get; set; }
            public int n { get; set; }
        }
        public IEnumerable<YesNoQuestionItem> YesNoQuestions()
        {
            var i = 0;
            var q = from s in (org.YesNoQuestions ?? string.Empty).Split(',')
                    let a = s.Split('=')
                    where s.HasValue()
                    select new YesNoQuestionItem { name = a[0].Trim(), desc = a[1], n = i++ };
            return q;
        }
        public class ExtraQuestionItem
        {
            public string question { get; set; }
            public int n { get; set; }
        }
        public IEnumerable<ExtraQuestionItem> ExtraQuestions()
        {
            var i = 0;
            var q = from s in (org.ExtraQuestions ?? string.Empty).Split(',')
                    where s.HasValue()
                    select new ExtraQuestionItem { question = s, n = i++ };
            return q;
        }
        public IEnumerable<SelectListItem> Options()
        {
            var q = from s in (org.AskOptions ?? string.Empty).Split(',')
                    let a = s.Split('=')
                    where s.HasValue()
                    let amt = a.Length > 1 ? " ({0:C})".Fmt(decimal.Parse(a[1])) : ""
                    select new SelectListItem { Text = a[0].Trim() + amt, Value = a[0].Trim() };
            var list = q.ToList();
            list.Insert(0, new SelectListItem { Text = "(not specified)", Value = "0" });
            return list;
        }
        public IEnumerable<SelectListItem> ExtraOptions()
        {
            var q = from s in (org.ExtraOptions ?? string.Empty).Split(',')
                    where s.HasValue()
                    let a = s.Split('=')
                    where a.Length > 1
                    select new SelectListItem { Text = a[1].Trim(), Value = a[0].Trim() };
            var list = q.ToList();
            list.Insert(0, new SelectListItem { Text = "(not specified)", Value = "00" });
            return list;
        }
        public IEnumerable<SelectListItem> GradeOptions()
        {
            var q = from s in (org.GradeOptions ?? string.Empty).Split(',')
                    where s.HasValue()
                    let a = s.Split('=')
                    where a.Length > 1
                    select new SelectListItem { Text = a[1].Trim(), Value = a[0].ToInt().ToString() };
            var list = q.ToList();
            list.Insert(0, new SelectListItem { Text = "(not specified)", Value = "00" });
            return list;
        }
        public class AgeGroupItem
        {
            public int StartAge { get; set; }
            public int EndAge { get; set; }
            public string Name { get; set; }
        }
        public IEnumerable<AgeGroupItem> AgeGroups()
        {
            var q = from o in (org.AgeGroups ?? string.Empty).Split(',')
                    where o.HasValue()
                    let b = o.Split('=')
                    let a = b[0].Split('-')
                    select new AgeGroupItem
                    {
                        StartAge = a[0].ToInt(),
                        EndAge = a[1].ToInt(),
                        Name = b[1]
                    };
            return q;
        }
        public static List<SelectListItem> ShirtSizes(CmsData.Organization org)
        {
            const string sizes = "YT-S=Youth: Small (6-8),YT-M=Youth: Medium (10-12),YT-L=Youth: Large (14-16),AD-S=Adult: Small,AD-M=Adult: Medium,AD-L=Adult: Large,AD-XL=Adult: X-Large,AD-XXL=Adult: XX-Large,AD-XXXL=Adult: XXX-Large";
            var shirtsizes = Util.PickFirst(org.ShirtSizes, sizes);
            var q = from ss in shirtsizes.Split(',')
                    let a = ss.Split('=')
                    select new SelectListItem
                    {
                        Value = a[0],
                        Text = a[1]
                    };
            var list = q.ToList();
            list.Insert(0, new SelectListItem { Value = "0", Text = "(not specified)" });
            if (org != null && org.AllowLastYearShirt == true)
                list.Add(new SelectListItem { Value = "lastyear", Text = "Use shirt from last year" });
            return list;
        }

        public string PrepareSummaryText()
        {
            var om = GetOrgMember();
            var sb = new StringBuilder();
            sb.Append("<table>");
            sb.AppendFormat("<tr><td>Org:</td><td>{0}</td></tr>\n", org.OrganizationName);
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
                sb.AppendFormat("<tr><td>Shirt:</td><td>{0}</td></tr>\n", om.ShirtSize);
            if (org.AskEmContact == true)
            {
                sb.AppendFormat("<tr><td>Emerg Contact:</td><td>{0}</td></tr>\n", rr.Emcontact);
                sb.AppendFormat("<tr><td>Emerg Phone:</td><td>{0}</td></tr>\n", rr.Emphone);
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
                sb.AppendFormat("<tr><td>Request:</td><td>{0}</td></tr>\n", om.Request);
            if (org.AskAllergies == true)
                sb.AppendFormat("<tr><td>Medical:</td><td>{0}</td></tr>\n", rr.MedicalDescription);

            if (org.AskTylenolEtc == true)
            {
                sb.AppendFormat("<tr><td>Tylenol?: {0},", tylenol == true ? "Yes" : tylenol == false ? "No" : "");
                sb.AppendFormat(" Advil?: {0},", advil == true ? "Yes" : advil == false ? "No" : "");
                sb.AppendFormat(" Robitussin?: {0},", robitussin == true ? "Yes" : robitussin == false ? "No" : "");
                sb.AppendFormat(" Maalox?: {0}</td></tr>\n", maalox == true ? "Yes" : maalox == false ? "No" : "");
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
            if (org.AskGrade == true)
                sb.AppendFormat("<tr><td>Grade:</td><td>{0}</td></tr>\n", om.Grade);

            if (org.AgeGroups.HasValue())
                sb.AppendFormat("<tr><td>AgeGroup:</td><td>{0}</td></tr>\n", AgeGroup());

            if (org.AskOptions.HasValue())
                sb.AppendFormat("<tr><td>Option:</td><td>{0}</td></tr>\n", option);
            if (org.ExtraOptions.HasValue())
                sb.AppendFormat("<tr><td>Extra Option:</td><td>{0}</td></tr>\n", option2);
            if (org.GradeOptions.HasValue())
                sb.AppendFormat("<tr><td>GradeOption:</td><td>{0}</td></tr>\n", 
                    GradeOptions().SingleOrDefault(s => s.Value == (gradeoption ?? "00")).Text);
            if (org.YesNoQuestions.HasValue())
                foreach (var a in YesNoQuestions())
                    sb.AppendFormat("<tr><td>{0}:</td><td>{1}</td></tr>\n".Fmt(a.desc, YesNoQuestion[a.name] == true ? "Yes" : "No"));
            if (org.ExtraQuestions.HasValue())
                foreach (var a in ExtraQuestion)
                    if (a.Value.HasValue())
                        sb.AppendFormat("<tr><td>{0}:</td><td>{1}</td></tr>\n".Fmt(a.Key, a.Value));

            var amt = AmountToPay();
            if (amt > 0)
                sb.AppendFormat("<tr><td>Amount Paid:</td><td>{0:c}</td></tr>\n", amt);
            sb.Append("</table>");

            return sb.ToString();
        }
        private string AgeGroup()
        {
            foreach (var i in AgeGroups())
                if (person.Age >= i.StartAge && person.Age <= i.EndAge)
                    return i.Name;
            return string.Empty;
        }
        public OrganizationMember Enroll(string TransactionID, string paylink, bool? testing, string others)
        {
            //(int)RegistrationEnum.AttendMeeting)
            var om = OrganizationMember.InsertOrgMembers(org.OrganizationId, person.PeopleId,
                (int)OrganizationMember.MemberTypeCode.Member, DateTime.Now, null, false);
            om.Amount = TotalAmount();
            om.AmountPaid = AmountToPay();

            var reg = person.RecRegs.SingleOrDefault();

            if (reg == null)
            {
                reg = new RecReg();
                person.RecRegs.Add(reg);
            }
            if (org.AskShirtSize == true)
            {
                om.ShirtSize = shirtsize;
                reg.ShirtSize = shirtsize;
            }
            if (org.AskChurch == true)
            {
                reg.ActiveInAnotherChurch = otherchurch;
                reg.Member = memberus;
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
            if (org.AskGrade == true)
                om.Grade = grade.ToInt();
            if (org.AskTickets == true)
                om.Tickets = ntickets;

            foreach (var yn in YesNoQuestions())
            {
                om.RemoveFromGroup("Yes:" + yn.name);
                om.RemoveFromGroup("No:" + yn.name);
            }
            if (org.YesNoQuestions.HasValue())
                foreach (var g in YesNoQuestion)
                    om.AddToGroup((g.Value == true ? "Yes:" : "No:") + g.Key);
            if (org.ExtraQuestions.HasValue())
                foreach (var g in ExtraQuestion)
                    if (g.Value.HasValue())
                        AddToMemberData("{0}: {1}".Fmt(g.Key, g.Value), om);

            foreach (var op in Options())
                om.RemoveFromGroup(op.Value);
            if (org.AskOptions.HasValue())
                om.AddToGroup(option);

            foreach (var op in ExtraOptions())
                om.RemoveFromGroup(op.Value);
            if (org.ExtraOptions.HasValue())
                om.AddToGroup(option2);

            if (org.GradeOptions.HasValue())
                om.Grade = gradeoption.ToInt();

            foreach (var ag in AgeGroups())
                om.RemoveFromGroup(ag.Name);
            if (org.AgeGroups.HasValue())
                om.AddToGroup(AgeGroup());

            if (org.LinkGroupsFromOrgs.HasValue())
            {
                var a = org.LinkGroupsFromOrgs.Split(',').Select(s => s.ToInt()).ToArray();
                var q = from omt in DbUtil.Db.OrgMemMemTags
                        where a.Contains(omt.OrgId) && omt.PeopleId == om.PeopleId
                        select omt.MemberTag.Name;
                foreach(var name in q)
                    om.AddToGroup(name);
            }

            string tstamp = Util.Now.ToString("MMM d yyyy h:mm tt");
            AddToMemberData(tstamp, om);
            var tran = "{0:C} ({1}{2})".Fmt(
                    om.AmountPaid.ToString2("C"), TransactionID, testing == true ? " test" : "");
            if (om.AmountPaid > 0)
            {
                AddToMemberData(tran, om);
                if (others.HasValue())
                    AddToMemberData("Others: " + others, om);
            }

            if (org.AskTylenolEtc == true)
            {
                reg.Tylenol = tylenol;
                reg.Advil = advil;
                reg.Robitussin = robitussin;
                reg.Maalox = maalox;
            }

            AddToRegistrationComments("-------------", reg);
            AddToRegistrationComments(email, reg);
            if (request.HasValue())
            {
                AddToRegistrationComments("Request: " + request, reg);
                om.Request = request;
            }

            if (om.AmountPaid > 0)
            {
                if (AmountDue() > 0)
                    AddToRegistrationComments("{0:C} due".Fmt(AmountDue().ToString("C")), reg);
                AddToRegistrationComments(tran, reg);
            }
            if (paylink.HasValue())
            {
                om.PayLink = paylink;
                AddToRegistrationComments(paylink, reg);
            }
            AddToRegistrationComments(tstamp, reg);
            AddToRegistrationComments("{0} - {1}".Fmt(org.DivisionName, org.OrganizationName), reg);

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
        public bool UserSelectsOrganization()
        {
            return divid != null && DbUtil.Db.Organizations.Any(o => o.DivisionId == divid &&
                    o.RegistrationTypeId == (int)CmsData.Organization.RegistrationEnum.UserSelectsOrganization);
        }
        public bool ComputesOrganizationByAge()
        {
            return divid != null && DbUtil.Db.Organizations.Any(o => o.DivisionId == divid &&
                    o.RegistrationTypeId == (int)CmsData.Organization.RegistrationEnum.ComputeOrganizationByAge);
        }
        public bool MemberOnly()
        {
            if (org != null)
                return org.MemberOnly == true;
            return divid != null && DbUtil.Db.Organizations.Any(o => o.DivisionId == divid &&
                    o.MemberOnly == true);
        }
        private CmsData.Organization GetAppropriateOrg()
        {
            var q = from o in DbUtil.Db.Organizations
                    where o.RegistrationTypeId == (int)CmsData.Organization.RegistrationEnum.ComputeOrganizationByAge
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
        public bool AnyOtherInfo()
        {
            if (org != null)
                return (org.AskShirtSize == true ||
                    org.AskRequest == true ||
                    org.AskGrade == true ||
                    org.AskEmContact == true ||
                    org.AskInsurance == true ||
                    org.AskDoctor == true ||
                    org.AskAllergies == true ||
                    org.AskTylenolEtc == true ||
                    org.AskParents == true ||
                    org.AskCoaching == true ||
                    org.AskChurch == true ||
                    org.AskTickets == true ||
                    org.AskOptions.HasValue() ||
                    org.YesNoQuestions.HasValue() ||
                    org.Deposit > 0);
            var q = from o in DbUtil.Db.Organizations
                    where o.DivisionId == divid
                    where o.AskShirtSize == true ||
                        o.AskRequest == true ||
                        o.AskGrade == true ||
                        o.AskEmContact == true ||
                        o.AskInsurance == true ||
                        o.AskDoctor == true ||
                        o.AskAllergies == true ||
                        o.AskTylenolEtc == true ||
                        o.AskParents == true ||
                        o.AskCoaching == true ||
                        o.AskChurch == true ||
                        o.AskTickets == true ||
                        o.AskOptions.HasValue() ||
                        o.YesNoQuestions.Length > 0 ||
                        o.Deposit > 0
                    select o;
            return q.Count() > 0;
        }
        public static void CheckNotifyDiffEmails(Person person, string fromemail, string regemail, string orgname, string phone)
        {
            if (person.EmailAddress.HasValue() && string.Compare(regemail, person.EmailAddress.Trim(), true) == 0)
                return;
            var flist = (from fm in person.Family.People
                         where fm.PositionInFamilyId == (int)Family.PositionInFamily.PrimaryAdult
                         select fm.EmailAddress).ToList();
            if (flist.Any(e => string.Compare(trim(e), regemail, true) == 0))
                return;

            var smtp = Util.Smtp();
            if (person.EmailAddress.HasValue())
            {
                string subj = "{0}, different email address than one on record".Fmt(orgname);
                string msg = @"Hi {0},
<p>You registered for {1} using a different email address than the one we have on record.
It is important that you call the church <strong>{2}</strong> to update our records
so that you will receive future important notices regarding this registration.</p>"
                    .Fmt(person.Name, orgname, phone.FmtFone());

                Util.Email2(smtp, fromemail, regemail, subj, msg);
                Util.Email2(smtp, fromemail, person.EmailAddress, subj, msg);
            }
            else
            {
                string subj = "{0}, no email on your record".Fmt(orgname);
                string msg = @"Hi {0},
<p>You registered for {1}, and we found your record, 
but there was no email address on your existing record in our database.
If you would like for us to update your record with this email address or another,
Please contact the church at <strong>{2}</strong> to let us know.
It is important that we have your email address so that
you will receive future important notices regarding this registration.
But we won't add that to your record without your permission.

Thank you</p>"
                    .Fmt(person.Name, orgname, phone.FmtFone());

                Util.Email2(smtp, fromemail, regemail, subj, msg);
            }
        }
        private static string trim(string s)
        {
            if (s != null)
                return s.Trim();
            return s;
        }
    }
}
