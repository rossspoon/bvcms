using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UtilityExtensions;
using CmsData;
using System.Text;
using System.Data.Linq;
using CMSPresenter;

namespace CMSWeb.Models
{
    public class SearchPersonModel
    {
        private static CodeValueController cv = new CodeValueController();

        public int index { get; set; }
        public string first { get; set; }
        public string goesby { get; set; }
        public string last { get; set; }
        public string title { get; set; }
        public string suffix { get; set; }
        public string dob { get; set; }
        public string phone { get; set; }
        public string homecell { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public int? gender { get; set; }
        public int? marital { get; set; }

        public bool? Found { get; set; }
        public bool IsNew { get; set; }
        public bool ShowAddress { get; set; }
        public bool LastItem { get; set; }

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
            get { return cv.GenderCodes().ItemValue(gender); }
        }
        public string marrieddisplay
        {
            get { return cv.MaritalStatusCodes().ItemValue(marital); }
        }
        private int count;
        private Person _Person;
        public Person person
        {
            get
            {
                if (_Person == null)
                    _Person = SearchPeopleModel.FindPerson(phone, first, last, birthday, out count);
                return _Person;
            }
        }

        internal void ValidateModelForNew(ModelStateDictionary ModelState)
        {
            CMSWeb.Models.SearchPeopleModel
                .ValidateFindPerson(ModelState, first, last, birthday, phone);
            if (!phone.HasValue())
                ModelState.AddModelError("phone", "phone required");
            if (!email.HasValue() || !Util.ValidEmail(email))
                ModelState.AddModelError("email", "Please specify a valid email address.");
            if (!gender.HasValue)
                ModelState.AddModelError("gender", "Please specify gender");
            if (!marital.HasValue)
                ModelState.AddModelError("married", "Please specify marital status");

            if (index == 0)
            {
                if (!address.HasValue())
                    ModelState.AddModelError("address", "address required.");
                if (!city.HasValue())
                    ModelState.AddModelError("city", "city required.");
                if (zip.GetDigits().Length < 5)
                    ModelState.AddModelError("zip", "zip needs at least 5 digits.");
                if (!state.HasValue())
                    ModelState.AddModelError("state", "state required");
            }
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

            _Person = Person.Add(f, 30,
                null, first.Trim(), null, last.Trim(), dob, marital == 20, gender ?? 0,
                    (int)Person.OriginCode.Enrollment, entrypoint);
            person.EmailAddress = email;
            person.MaritalStatusId = marital.Value;
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
    }
}
