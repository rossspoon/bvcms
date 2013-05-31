using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsWeb.Areas.People.Models.Person;
using CmsWeb.Code;
using UtilityExtensions;
using CmsData;
using System.Text;
using System.Data.Linq;
using CmsData.Codes;

namespace CmsWeb.Areas.Search.Models
{
    public class SearchPersonModel
    {
        private static CodeValueModel cv = new CodeValueModel();

        public int index { get; set; }
        public string context { get; set; }

        [UIHint("Text")]
        [Required]
        public string First { get; set; }

        [UIHint("Text")]
        [DisplayName("Goes By")]
        public string GoesBy { get; set; }

        [UIHint("Text")]
        public string Middle { get; set; }

        [UIHint("Text")]
        [Required]
        public string Last { get; set; }

        public CodeInfo Title { get; set; }

        [UIHint("Text")]
        public string Suffix { get; set; }

        [UIHint("Date")]
        [DisplayName("Birthday")]
        [RequiredDate]
        public string dob { get; set; }

        [UIHint("Text")]
        [RequiredPhone]
        public string Phone { get; set; }

        [UIHint("Text")]
        [RequiredEmail]
        public string Email { get; set; }

        [RequiredGender]
        public CodeInfo Gender { get; set; }

        [RequiredMarital]
        public CodeInfo Marital { get; set; }

        public CodeInfo Campus { get; set; }

        [DisplayName("Entry Point")]
        public CodeInfo EntryPoint { get; set; }

        [UIHint("Text")]
        public string HomePhone { get; set; }

        private DateTime? _Birthday;

        public DateTime? birthday
        {
            get
            {
                DateTime dt;
                if (!_Birthday.HasValue && dob.NotEqual("na"))
                    if (Util.DateValid(dob, out dt))
                        _Birthday = dt;
                return _Birthday;
            }
        }

        public int? age
        {
            get
            {
                if (birthday.HasValue)
                    return birthday.Value.AgeAsOf(Util.Now);
                return null;
            }
        }

        public bool IsNew
        {
            get { return !PeopleId.HasValue; }
        }

        public int FamilyId { get; set; }
        private Family _family;
        public Family family
        {
            get
            {
                if (_family == null && FamilyId > 0)
                    _family = DbUtil.Db.Families.Single(f => f.FamilyId == FamilyId);
                return _family;
            }
        }

        public int? PeopleId { get; set; }
        private Person _Person;
        public Person person
        {
            get
            {
                if (_Person == null && PeopleId.HasValue)
                    _Person = DbUtil.Db.LoadPersonById(PeopleId.Value);
                return _Person;
            }
        }

        public AddressInfo AddressInfo { get; set; }

        public string PotentialDuplicate { get; set; }

        internal void CheckDuplicate(ModelStateDictionary ModelState)
        {
            var pids = DbUtil.Db.FindPerson(First, Last, birthday, null, Phone.GetDigits());
            if (pids.Any())
            {
                var p = DbUtil.Db.LoadPersonById(pids.First().PeopleId.Value);
                PotentialDuplicate = "e.g. {0} ({1}), {2}, age:{3}".Fmt(p.Name, p.PeopleId, p.PrimaryAddress, p.Age);
            }

//            if (AddressInfo == null)
//                return;
//            AddressInfo.ValidateAddress();
//            if (AddressInfo.ResultNotFound)
//                ModelState.AddModelError("address", "Address Not Found");
//            else if (AddressInfo.ResultChanged)
//                ModelState.AddModelError("address", "Address Found and Adjusted by USPS");
//            else if (AddressInfo.Addrok == false)
//                ModelState.AddModelError("address", "City and State are required or Zip is required");
        }

        internal void AddPerson(int originid, int? entrypointid, int? campusid)
        {
            Family f;
            string na = "na";
            if (FamilyId > 0)
                f = family;
            else
                f = new Family
                        {
                            HomePhone = HomePhone.GetDigits(),
                            AddressLineOne = AddressInfo.Address1.Disallow(na),
                            AddressLineTwo = AddressInfo.Address2,
                            CityName = AddressInfo.City.Disallow(na),
                            StateCode = AddressInfo.State.Value.Disallow(na),
                            ZipCode = AddressInfo.Zip.Disallow(na),
                            CountryName = AddressInfo.Country.Value,
                        };

            if (GoesBy != null)
                GoesBy = GoesBy.Trim();
            var position = PositionInFamily.Child;
            if (!birthday.HasValue)
                position = PositionInFamily.PrimaryAdult;
            if (age >= 18)
                if (f.People.Count(per =>
                                   per.PositionInFamilyId == PositionInFamily.PrimaryAdult)
                    < 2)
                    position = PositionInFamily.PrimaryAdult;
                else
                    position = PositionInFamily.SecondaryAdult;

            _Person = CmsData.Person.Add(f, position,
                                 null, First.Trim(), GoesBy, Last.Trim(), dob, false, Gender.Value.ToInt(),
                                 originid, entrypointid);
            if (Title.Value.HasValue())
                person.TitleCode = Title.Value;
            person.EmailAddress = Email.Disallow(na);
            person.MaritalStatusId = Marital.Value.ToInt();
            person.SuffixCode = Suffix;
            person.MiddleName = Middle;
            if (campusid == 0)
                campusid = null;
            person.CampusId = Util.PickFirst(campusid.ToString(), DbUtil.Db.Setting("DefaultCampusId", "")).ToInt2();
            if (person.CampusId == 0)
                person.CampusId = null;

            person.CellPhone = Phone.GetDigits();
            DbUtil.Db.SubmitChanges();
            DbUtil.Db.Refresh(RefreshMode.OverwriteCurrentValues, person);
            PeopleId = person.PeopleId;
        }

        public void LoadAddress()
        {
            if (FamilyId <= 0)
            {
                AddressInfo = new AddressInfo();
                HomePhone = "";
                return;
            }
            var f = family;
            AddressInfo = new AddressInfo(f.AddressLineOne, f.AddressLineTwo, f.CityName, f.StateCode, f.ZipCode, f.CountryName);
            HomePhone = f.HomePhone;
        }

        //public string ToolTip
        //{
        //    get
        //    {
        //        return "{0} {1}|{2}|{3}|Birthday: {4}|c {5}|h {6}|{7}|Gender: {8}|Marital: {9}".Fmt(
        //            GoesBy ?? First, Last,
        //            AddressInfo.Address,
        //            CityStateZip,
        //            birthday.FormatDate(),
        //            Phone.FmtFone(),
        //            HomePhone.FmtFone(),
        //            Email,
        //            Gender.ToString(),
        //            Marital.ToString());
        //    }
        //}

        public string CityStateZip
        {
            get { return "{0}, {1} {2}".Fmt(AddressInfo.City, AddressInfo.State.Value, AddressInfo.Zip); }
        }
        [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
        sealed public class RequiredDateAttribute : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                var sval = (string)value;
                if (sval == null) return false;
                if (sval == "na") return true;
                DateTime dt;
                return DateTime.TryParse(sval, out dt);
            }
            public override string FormatErrorMessage(string name)
            {
                return "enter a valid date or na";
            }
        }
        [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
        sealed public class RequiredPhoneAttribute : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                var sval = (string)value;
                if (sval == null) return false;
                if (sval == "na") return true;
                var d = sval.GetDigits().Length;
                return d == 7 || d >= 10;
            }
            public override string FormatErrorMessage(string name)
            {
                return "Phone requires 7 or 10+ digits (or na)";
            }
        }
        [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
        sealed public class RequiredEmailAttribute : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                var sval = (string)value;
                if (sval == null) return false;
                if (sval == "na") return true;
                return Util.ValidEmail(sval);
            }
            public override string FormatErrorMessage(string name)
            {
                return "valid email address (or na)";
            }
        }
        [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
        sealed public class RequiredGenderAttribute : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                var sval = (CodeInfo)value;
                return sval.Value != "99";
            }
            public override string FormatErrorMessage(string name)
            {
                return "specify gender (or unknown)";
            }
        }
        [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
        sealed public class RequiredMaritalAttribute : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                var sval = (CodeInfo)value;
                return sval.Value != "99";
            }
            public override string FormatErrorMessage(string name)
            {
                return "specify marital status (or unknown)";
            }
        }
    }
}
