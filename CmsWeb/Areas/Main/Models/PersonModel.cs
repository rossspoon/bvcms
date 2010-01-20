using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using System.Web.Mvc;
using UtilityExtensions;

namespace CMSWeb.Models
{
    public class PersonModel
    {
        public Person person;
        public PersonModel(int? id)
        {
            person = DbUtil.Db.LoadPersonById(id.Value);
        }
        public string Name
        {
            get { return person.Name; }
            set { person.Name = value; }
        }
        public IEnumerable<SelectListItem> PreferredAddresses()
        {
            var q = from a in DbUtil.Db.AddressTypes
                    select new SelectListItem
                    {
                        Text = a.Description,
                        Value = a.Id.ToString(),
                    };
            return q;
        }
        private RecReg recreg;
        public bool HasRecReg
        {
            get
            {
                if (recreg == null)
                    if (HttpContext.Current.User.IsInRole("Attendance"))
                        recreg = person.RecRegs.OrderByDescending(v => v.Uploaded).FirstOrDefault();
                return recreg != null;
            }
        }
        public string RecRegLink
        {
            get
            {
                if (recreg != null)
                    return "/Recreation/Detail/{0}".Fmt(recreg.Id);
                return "";
            }
        }
        private int? ckorg;
        public bool CanCheckIn
        {
            get
            {
                ckorg = (int?)HttpContext.Current.Session["CheckInOrgId"];
                return ckorg.HasValue;
            }
        }
        public string CheckInLink
        {
            get { return "/CheckIn/CheckIn/{0}?pid={1}".Fmt(ckorg, person.PeopleId); }
        }
        public bool IsFinance
        {
            get
            {
                return HttpContext.Current.User.IsInRole("Finance");
            }
        }
        public string ContributionsLink
        {
            get { return "/Contributions/Years.aspx?id={0}".Fmt(person.PeopleId); }
        }
        public bool IsAdmin
        {
            get { return HttpContext.Current.User.IsInRole("Admin"); }
        }
        public bool IsEdit
        {
            get { return HttpContext.Current.User.IsInRole("Edit"); }
        }
        public int SmallPic
        {
            get { return person.PictureId == null ? 0 : person.Picture.SmallId.Value; }
        }
        public class FamilyMember
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int? Age { get; set; }
            public string Color { get; set; }
            public string PositionInFamily { get; set; }
            public string SpouseIndicator { get; set; }
        }
        public IEnumerable<FamilyMember> FamilyMembers()
        {
            var q = from m in person.Family.People
                    orderby
                        m.PeopleId == m.Family.HeadOfHouseholdId ? 1 :
                        m.PeopleId == m.Family.HeadOfHouseholdSpouseId ? 2 :
                        3, m.Age descending, m.Name2
                    select new FamilyMember
                    {
                        Id = m.PeopleId,
                        Name = m.Name,
                        Age = m.Age,
                        Color = m.DeceasedDate != null ? "red" : "black",
                        PositionInFamily = m.FamilyPosition.Code,
                        SpouseIndicator = m.PeopleId == person.SpouseId ? "*" : "&nbsp;"
                    };
            return q;
        }
        public string addrtab { get; set; }
        public AddressInfo GetAddress(string tab)
        {
            var a = new AddressInfo();
            switch (tab)
            {
                case "family1-tab":
                    a.AddressLine1 = person.Family.AddressLineOne;
                    a.AddressLine2 = person.Family.AddressLineTwo;
                    a.BadAddressFlag = person.Family.BadAddressFlag;
                    a.City = person.Family.CityName;
                    a.State = person.Family.StateCode;
                    a.Zip = person.Family.ZipCode;
                    a.ResCodeId = person.Family.ResCodeId;
                    a.FromDate = person.Family.AddressFromDate;
                    a.ToDate = person.Family.AddressToDate;
                    a.PreferredFlag = person.AddressTypeId == (int)Address.AddressTypes.Family;
                    if (a.PreferredFlag)
                        addrtab = tab;
                    break;
                case "family2-tab":
                    a.AddressLine1 = person.Family.AltAddressLineOne;
                    a.AddressLine2 = person.Family.AltAddressLineTwo;
                    a.BadAddressFlag = person.Family.AltBadAddressFlag;
                    a.City = person.Family.AltCityName;
                    a.State = person.Family.AltStateCode;
                    a.Zip = person.Family.AltZipCode;
                    a.ResCodeId = person.Family.AltResCodeId;
                    a.FromDate = person.Family.AltAddressFromDate;
                    a.ToDate = person.Family.AltAddressToDate;
                    a.PreferredFlag = person.AddressTypeId == (int)Address.AddressTypes.FamilyAlternate;
                    if (a.PreferredFlag)
                        addrtab = tab;
                    break;
                case "personal1-tab":
                    a.AddressLine1 = person.AddressLineOne;
                    a.AddressLine2 = person.AddressLineTwo;
                    a.BadAddressFlag = person.BadAddressFlag;
                    a.City = person.CityName;
                    a.State = person.StateCode;
                    a.Zip = person.ZipCode;
                    a.ResCodeId = person.ResCodeId;
                    a.FromDate = person.AddressFromDate;
                    a.ToDate = person.AddressToDate;
                    a.PreferredFlag = person.AddressTypeId == (int)Address.AddressTypes.Personal;
                    if (a.PreferredFlag)
                        addrtab = tab;
                    break;
                case "personal2-tab":
                    a.AddressLine1 = person.AltAddressLineOne;
                    a.AddressLine2 = person.AltAddressLineTwo;
                    a.BadAddressFlag = person.AltBadAddressFlag;
                    a.City = person.AltCityName;
                    a.State = person.AltStateCode;
                    a.Zip = person.AltZipCode;
                    a.ResCodeId = person.AltResCodeId;
                    a.FromDate = person.AltAddressFromDate;
                    a.ToDate = person.AltAddressToDate;
                    a.PreferredFlag = person.AddressTypeId == (int)Address.AddressTypes.PersonalAlternate;
                    if (a.PreferredFlag)
                        addrtab = tab;
                    break;
            }
            return a;
        }
        public class AddressInfo
        {
            public string AddressLine1 { get; set; }
            public string AddressLine2 { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string Zip { get; set; }
            public bool? BadAddressFlag { get; set; }
            public string BadAddress
            {
                get { return (BadAddressFlag ?? false) ? "checked=\"checked\"" : ""; }
            }
            public int? ResCodeId { get; set; }
            public string ResCode
            {
                get
                {
                    if (ResCodeId.HasValue)
                        return ResCodes().Single(rc => rc.Value == ResCodeId.ToString()).Text;
                    return "(not specified)";
                }
            }
            public bool PreferredFlag { get; set; }
            public string Preferred
            {
                get { return PreferredFlag ? "checked=\"checked\"" : ""; }
            }
            public DateTime? FromDate { get; set; }
            public DateTime? ToDate { get; set; }
        }
        public static IEnumerable<SelectListItem> ResCodes()
        {
            var q = from rc in DbUtil.Db.ResidentCodes
                    orderby rc.Id
                    select new SelectListItem
                    {
                        Value = rc.Id.ToString(),
                        Text = rc.Description
                    };
            return q;
        }
    }
}
