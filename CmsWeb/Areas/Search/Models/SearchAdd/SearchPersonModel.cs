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
        public string First { get; set; }

        [UIHint("Text")]
        [DisplayName("Goes By")]
        public string GoesBy { get; set; }

        [UIHint("Text")]
        public string Middle { get; set; }

        [UIHint("Text")]
        public string Last { get; set; }

        public CodeInfo Title { get; set; }

        [UIHint("Text")]
        public string Suffix { get; set; }

        [UIHint("Date")]
        [DisplayName("Birthday")]
        public string dob { get; set; }

        [UIHint("Text")]
        public string Phone { get; set; }

        [UIHint("Text")]
        public string Email { get; set; }

        public CodeInfo Gender { get; set; }

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

        internal void ValidateModelForNew(ModelStateDictionary ModelState)
        {
            if (!First.HasValue())
                ModelState.AddModelError("name", "first name required");

            if (!Last.HasValue())
                ModelState.AddModelError("name", "last name required");

            if (!birthday.HasValue && dob.NotEqual("na"))
                ModelState.AddModelError("dob", "valid birthday (or \"na\")");

            var d = Phone.GetDigits().Length;
            if (d != 7 && d < 10 && Phone.NotEqual("na"))
                ModelState.AddModelError("phone", "7 or 10+ digits (or \"na\")");

            int count = 0;
            var pids = DbUtil.Db.FindPerson(First, Last, birthday, Email, Phone.GetDigits());
            if (pids.Any())
                ModelState.AddModelError("name", "name/dob already exists in db");

            if (!Util.ValidEmail(Email) && Email.NotEqual("na"))
                ModelState.AddModelError("email", "valid email address (or \"na\")");

            if (Gender.Value == "99")
                ModelState.AddModelError("gender", "specify gender");

            if (Marital.Value == "99")
                ModelState.AddModelError("marital", "specify marital status");

            if (AddressInfo == null)
                return;
            AddressInfo.ValidateAddress();
            if (AddressInfo.ResultNotFound)
                ModelState.AddModelError("address", "Address Not Found");
            else if (AddressInfo.ResultChanged)
                ModelState.AddModelError("address", "Address Found and Adjusted by USPS");
            else if (AddressInfo.Addrok == false)
                ModelState.AddModelError("address", "City and State are required or Zip is required");
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

        internal void LoadFamily()
        {
            if (FamilyId <= 0)
                return;
            HomePhone = family.HomePhone;
            AddressInfo.Address1 = family.AddressLineOne;
            AddressInfo.Address2 = family.AddressLineTwo;
            AddressInfo.City = family.CityName;
            AddressInfo.State = new CodeInfo(family.StateCode, "State");
            AddressInfo.Zip = family.ZipCode;
            AddressInfo.Country = new CodeInfo(family.CountryName, "Country");
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

    }
}
