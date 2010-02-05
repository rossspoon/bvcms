using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UtilityExtensions;
using CmsData;
using System.Text;
using System.Data.Linq;

namespace CMSWeb.Models
{
    public class RetreatModel
    {
        public string request { get; set; }
        public string first { get; set; }
        public string last { get; set; }
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

        public int orgid { get; set; }
        public int option { get; set; }
        public bool? Found { get; set; }
        public bool IsNew { get; set; }
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
        private decimal? _amountpaid;
        public decimal amountpaid
        {
            get
            {
                if (!_amountpaid.HasValue)
                {
                    var om = DbUtil.Db.OrganizationMembers.SingleOrDefault(m =>
                        m.PeopleId == person.PeopleId && m.OrganizationId == orgid);
                    _amountpaid = om != null ? om.Amount ?? 0 : 0;
                }
                return _amountpaid.Value;
            }
        }
        public decimal ComputeFee()
        {
            decimal fee = 0;
            if (option == 1)
                fee = 50;
            else
                fee = 180 - amountpaid;
            return fee;
        }
        public decimal BalanceDue()
        {
            return 180 - amountpaid;
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
                }
                else if (count > 1)
                    ModelState.AddModelError("find", "More than one match, sorry");
                else if (count == 0)
                    ModelState.AddModelError("find", "record not found");
            }
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
            if (!address.HasValue())
                ModelState.AddModelError("address", "address required.");
            if (!city.HasValue())
                ModelState.AddModelError("city", "city required.");
            if (zip.GetDigits().Length < 5)
                ModelState.AddModelError("zip", "zip needs at least 5 digits.");
            if (!state.HasValue())
                ModelState.AddModelError("state", "state required");
        }
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0}({1},{2},{3},{4}), Birthday: {5}({6}), Phone: {7}, {8}<br />\n".Fmt(
                person.Name, person.PeopleId, person.Gender.Code, person.MaritalStatus.Code, option == 1 ? "5K" : "FunRun",
                person.DOB, person.Age, phone.FmtFone(homecell), email));
            if (ShowAddress)
                sb.AppendFormat("&nbsp;&nbsp;{0}; {1}<br />\n", person.PrimaryAddress, person.CityStateZip);
            return sb.ToString();
        }
        internal void AddPerson()
        {
            var f = new Family
            {
                AddressLineOne = address,
                CityName = city,
                StateCode = state,
                ZipCode = zip,
            };

            _Person = Person.Add(f, 30,
                null, first.Trim(), null, last.Trim(), dob, married.Value == 20, gender.Value,
                    DbUtil.Settings("RetreatOrigin", "0").ToInt(),
                    DbUtil.Settings("RetreatEntry", "0").ToInt());
            person.EmailAddress = email;
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
