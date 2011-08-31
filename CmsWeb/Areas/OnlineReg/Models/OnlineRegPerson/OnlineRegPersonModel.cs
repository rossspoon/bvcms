using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using CmsData;
using UtilityExtensions;
using System.Runtime.Serialization;
using System.Web.Mvc;
using CmsData.Codes;

namespace CmsWeb.Models
{
    [Serializable]
    public partial class OnlineRegPersonModel
    {
        public int index { get; set; }
        public bool LastItem { get; set; }
        public bool? CreatingAccount { get; set; }
        public bool SawExistingAccount;
        public bool CannotCreateAccount;
        public bool CreatedAccount;

        public int? divid { get; set; }
        public int? orgid { get; set; }
        public int? classid { get; set; }

        public string first { get; set; }
        [OptionalField]
        private string _Middle;
        public string middle
        {
            get { return _Middle; }
            set { _Middle = value; }
        }
        public string last { get; set; }
        public string suffix { get; set; }
        public string dob { get; set; }
        public string phone { get; set; }
        [OptionalField]
        private string _Homephone;
        public string homephone
        {
            get { return _Homephone; }
            set { _Homephone = value; }
        }
        public string email { get; set; }
        public string fromemail
        {
            get { return first + " " + last + " <" + email + ">"; }
        }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public int? gender { get; set; }
        public int? married { get; set; }

        public bool? Found { get; set; }
        public bool IsNew { get; set; }

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
        
        public string inputname(string field) 
        {
            return "m.List[" + index + "]." + field;
        }
        [OptionalField]
        private decimal? _Suggestedfee;
        public decimal? suggestedfee
        {
            get { return _Suggestedfee; }
            set { _Suggestedfee = value; }
        }
        [OptionalField]
        private string _Option2;
        public string option2
        {
            get { return _Option2; }
            set { _Option2 = value; }
        }
        [OptionalField]
        private string _Option3;
        public string option3
        {
            get { return _Option3; }
            set { _Option3 = value; }
        }
        public string gradeoption { get; set; }
        [OptionalField]
        private bool _IsFamily;
        public bool IsFamily
        {
            get { return _IsFamily; }
            set { _IsFamily = value; }
        }

        public string ErrorTarget { get { return IsFamily ? "findf" : "findn"; } }
        public bool OtherOK { get; set; }
        public bool ShowAddress { get; set; }
        [OptionalField]
        private Dictionary<string, int?> _MenuItem = new Dictionary<string, int?>();
        public Dictionary<string, int?> MenuItem
        {
            get { return _MenuItem; }
            set { _MenuItem = value; }
        }
        public int? MenuItemValue(string s)
        {
            if (MenuItem.ContainsKey(s))
                return MenuItem[s];
            return null;
        }
        [OptionalField]
        private Dictionary<int, decimal?> _FundItem = new Dictionary<int, decimal?>();
        public Dictionary<int, decimal?> FundItem
        {
            get { return _FundItem; }
            set { _FundItem = value; }
        }
        public decimal? FundItemValue(int n)
        {
            if (FundItem.ContainsKey(n))
                return FundItem[n];
            return null;
        }
        [OptionalField]
        private int? _Whatfamily;
        public int? whatfamily
        {
            get { return _Whatfamily; }
            set { _Whatfamily = value; }
        }
        [OptionalField]
        private bool? _LoggedIn;
        public bool? LoggedIn
        {
            get { return _LoggedIn; }
            set { _LoggedIn = value; }
        }

        [NonSerialized]
        private DateTime _Birthday;
        public DateTime? birthday
        {
            get
            {
                if (_Birthday == DateTime.MinValue)
                    Util.BirthDateValid(dob, out _Birthday);
                return _Birthday == DateTime.MinValue ? (DateTime?)null : _Birthday;
            }
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
                    {
                        _Person = DbUtil.Db.LoadPersonById(PeopleId.Value);
                        count = 1;
                    }
                    else
                    {
                        //_Person = SearchPeopleModel.FindPerson(first, last, birthday, email, phone, out count);

                        var list = DbUtil.Db.FindPerson(first, last, birthday, email, phone).ToList();
                        count = list.Count;
                        if (count == 1)
                            _Person = DbUtil.Db.LoadPersonById(list[0].PeopleId.Value);
                        if (_Person != null)
                            PeopleId = _Person.PeopleId;
                    }
                return _Person;
            }
        }
        public void AddPerson(Person p, int entrypoint)
        {
            Family f;
            if (p == null)
                f = new Family
                {
                    AddressLineOne = address,
                    CityName = city,
                    StateCode = state,
                    ZipCode = zip,
                    HomePhone = homephone,
                };
            else
                f = p.Family;

            _Person = Person.Add(f, PositionInFamily.Child,
                null, first.Trim(), null, last.Trim(), dob, married == 20, gender ?? 0,
                    OriginCode.Enrollment, entrypoint);
            person.EmailAddress = email.Trim();
            person.SuffixCode = suffix;
            person.MiddleName = middle;
            person.CampusId = DbUtil.Db.Setting("DefaultCampusId", "").ToInt2();
            if (person.Age >= 18)
                person.PositionInFamilyId = PositionInFamily.PrimaryAdult;
            person.CellPhone = phone.GetDigits();

            DbUtil.Db.SubmitChanges();
            DbUtil.Db.Refresh(RefreshMode.OverwriteCurrentValues, person);
            PeopleId = person.PeopleId;
        }
        public bool IsCreateAccount()
        {
            if (org != null)
                return org.RegistrationTypeId == RegistrationTypeCode.CreateAccount;
            return false;
        }
    }
}
