using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web;
using System.Web.Mvc;
using CmsData;
using UtilityExtensions;

namespace CmsWeb.Areas.People.Models.Person
{
    public class AddressInfo
    {
        private CmsWeb.Models.CodeValueModel cv = new CmsWeb.Models.CodeValueModel();

        public int PeopleId { get; set; }
        public CmsData.Person person { get; set; }

        public string Name { get; set; }
        private bool? _CanUserEditAddress;
        public bool CanUserEditAddress
        {
            get
            {
                if (!_CanUserEditAddress.HasValue)
                {
                    switch (Name)
                    {
                        case "PersonalAddr":
                            _CanUserEditAddress = person.CanUserEditBasic;
                            break;
                        case "FamilyAddr":
                            _CanUserEditAddress = person.CanUserEditFamilyAddress;
                            break;
                    }
                }
                return _CanUserEditAddress.Value;
            }
        }

        public string Display
        {
            get
            {
                switch (Name)
                {
                    case "PersonalAddr": return "Personal Address";
                    case "FamilyAddr": return "Family Address";
                }
                return "Address";
            }
        }

        [UIHint("Text")]
        public string Address1 { get; set; }

        [UIHint("Text")]
        public string Address2 { get; set; }

        [UIHint("Text")]
        public string City { get; set; }

        [UIHint("Code")]
        public CodeInfo State { get; set; }

        [UIHint("Text")]
        public string Zip { get; set; }

        [UIHint("Code")]
        public CodeInfo Country { get; set; }

        public string CityStateZip()
        {
            return Util.FormatCSZ(City, State.Value, Zip);
        }
        public string CityStateZip4()
        {
            return Util.FormatCSZ4(City, State.Value, Zip);
        }
        public string AddrCityStateZip()
        {
            return Address1 + " " + CityStateZip();
        }
        public string Addr2CityStateZip()
        {
            return Address2 + " " + CityStateZip();
        }

        [UIHint("Bool")]
        public bool? BadAddress { get; set; }

        [UIHint("Code")]
        public CodeInfo ResCode { get; set; }

        [UIHint("Bool")]
        public bool Preferred { get; set; }

        [UIHint("Date")]
        public DateTime? FromDt { get; set; }

        [UIHint("Date")]
        public DateTime? ToDt { get; set; }

        public static IEnumerable<SelectListItem> ResCodes()
        {
            return CmsWeb.Models.QueryModel.ConvertToSelect(CmsWeb.Models.CodeValueModel.ResidentCodesWithZero(), "Id");
        }
        public static IEnumerable<SelectListItem> States()
        {
            return CmsWeb.Models.QueryModel.ConvertToSelect(CmsWeb.Models.CodeValueModel.GetStateList(), "Code");
        }
        public static IEnumerable<SelectListItem> Countries()
        {
            var list = CmsWeb.Models.QueryModel.ConvertToSelect(CmsWeb.Models.CodeValueModel.GetCountryList(), null);
            list.Insert(0, new SelectListItem { Text = "(not specified)", Value = "" });
            return list;
        }
        public static AddressInfo GetAddressInfo(int id, string typeid)
        {
            var p = DbUtil.Db.LoadPersonById(id);
            var a = new AddressInfo();
            a.person = p;
            switch (typeid)
            {
                case "PrimaryAddr":
                    a.Name = typeid;
                    a.PeopleId = p.PeopleId;
                    a.Address1 = p.PrimaryAddress;
                    a.Address2 = p.PrimaryAddress2;
                    a.BadAddress = p.PrimaryBadAddrFlag == 1;
                    a.City = p.PrimaryCity;
                    a.Zip = p.PrimaryZip;
                    a.State = new CodeInfo(p.PrimaryState, StateCodes());
                    a.Country = new CodeInfo(p.PrimaryCountry, Countries());
                    a.ResCode = new CodeInfo(p.PrimaryResCode, ResCodes());
                    break;
                case "FamilyAddr":
                    a.Name = typeid;
                    a.PeopleId = p.PeopleId;
                    a.Address1 = p.Family.AddressLineOne;
                    a.Address2 = p.Family.AddressLineTwo;
                    a.ToDt = p.Family.AddressToDate;
                    a.BadAddress = p.Family.BadAddressFlag;
                    a.City = p.Family.CityName;
                    a.Zip = p.Family.ZipCode;
                    a.State = new CodeInfo(p.Family.StateCode, StateCodes());
                    a.Country = new CodeInfo(p.Family.CountryName, Countries());
                    a.ResCode = new CodeInfo(p.Family.ResCodeId, ResCodes());
                    a.Preferred = p.AddressTypeId == 10;
                    break;
                case "PersonalAddr":
                    a.Name = typeid;
                    a.PeopleId = p.PeopleId;
                    a.Address1 = p.AddressLineOne;
                    a.Address2 = p.AddressLineTwo;
                    a.ToDt = p.AddressToDate;
                    a.BadAddress = p.BadAddressFlag;
                    a.City = p.CityName;
                    a.Zip = p.ZipCode;
                    a.State = new CodeInfo(p.StateCode, StateCodes());
                    a.Country = new CodeInfo(p.CountryName, Countries());
                    a.ResCode = new CodeInfo(p.ResCodeId, ResCodes());
                    a.Preferred = p.AddressTypeId == 30;
                    break;
            }
            return a;
        }

        public object UpdateAddress(Controller ctl)
        {
            var p = DbUtil.Db.LoadPersonById(PeopleId);
            var f = p.Family;

            bool addrok = City.HasValue() && State.Value.HasValue() || Zip.HasValue();

            if (!City.HasValue() && !State.Value.HasValue() && !Zip.HasValue())
                addrok = true;

            if (!addrok)
                return new { confirm = "city/state required or zip required" };

            if (Address1.HasValue() && (City.HasValue() || State.Value.HasValue() || Zip.HasValue())
                && (Country.Value == "United States" || !Country.Value.HasValue()))
            {
                var r = AddressVerify.LookupAddress(Address1, Address2, City, State.Value, Zip);
                if (r.Line1 != "error") // no network error
                {
                    if (!r.found)
                        return new { confirm = r.address.Replace("\n", "<br/>\n") + "<br/>\nIf your address will not validate, change the country to 'USA, Not Validated'"};
                    var b = false;
                    if (r.Line1 != Address1)
                    {
                        Address1 = r.Line1;
                        b = true;
                    }
                    if (r.Line2 != (Address2 ?? ""))
                    {
                        Address2 = r.Line2;
                        b = true;
                    }
                    if (r.City != (City ?? ""))
                    {
                        City = r.City;
                        b = true;
                    }
                    if (r.State != (State.Value ?? ""))
                    {
                        State.Value = r.State;
                        b = true;
                    }
                    if (r.Zip != (Zip ?? ""))
                    {
                        Zip = r.Zip;
                        b = true;
                    }
                    if (b)
                        return (new { confirm = r.address.Replace("\n", "<br/>\n") } );
                }
            }
            // at this point, we either have a network error to the validation web service or the address validated just fine.
            // in either case, we are going to save the results.

            switch (Name)
            {
                case "FamilyAddr":
                    UpdateValue(f, "AddressLineOne", Address1);
                    UpdateValue(f, "AddressLineTwo", Address2);
                    UpdateValue(f, "AddressToDate", ToDt);
                    UpdateValue(f, "CityName", City);
                    UpdateValue(f, "StateCode", State.Value);
                    UpdateValue(f, "ResCodeId", ResCode.Value.ToInt());
                    UpdateValue(f, "ZipCode", Zip ?? "");
                    UpdateValue(f, "CountryName", Country.Value);
                    if (Preferred)
                        UpdateValue(p, "AddressTypeId", 10);
                    if (fsb.Length > 0)
                        BadAddress = false;
                    UpdateValue(f, "BadAddressFlag", BadAddress);
                    break;
                case "PersonalAddr":
                    UpdateValue(p, "AddressLineOne", Address1);
                    UpdateValue(p, "AddressLineTwo", Address2);
                    UpdateValue(p, "AddressToDate", ToDt);
                    UpdateValue(p, "CityName", City);
                    UpdateValue(p, "StateCode", State.Value);
                    UpdateValue(p, "ResCodeId", ResCode.Value.ToInt());
                    UpdateValue(p, "ZipCode", Zip ?? "");
                    UpdateValue(p, "CountryName", Country.Value);
                    if (Preferred)
                        UpdateValue(p, "AddressTypeId", 30);
                    if (psb.Length > 0)
                        BadAddress = false;
                    UpdateValue(p, "BadAddressFlag", BadAddress);
                    break;
            }
            if (psb.Length > 0)
            {
                var c = new ChangeLog
                {
                    UserPeopleId = Util.UserPeopleId.Value,
                    PeopleId = PeopleId,
                    Field = Name,
                    Data = "<table>\n" + psb + "</table>",
                    Created = Util.Now
                };
                DbUtil.Db.ChangeLogs.InsertOnSubmit(c);
            }
            if (fsb.Length > 0)
            {
                var c = new ChangeLog
                {
                    FamilyId = p.FamilyId,
                    UserPeopleId = Util.UserPeopleId.Value,
                    PeopleId = PeopleId,
                    Field = Name,
                    Data = "<table>\n" + fsb + "</table>",
                    Created = Util.Now
                };
                DbUtil.Db.ChangeLogs.InsertOnSubmit(c);
            }
            try
            {
                DbUtil.Db.SubmitChanges();
    			DbUtil.LogActivity("Update Address for: {0}".Fmt(person.Name));
            }
            catch (InvalidOperationException ex)
            {
                return new {error = ex.Message};
            }

            if (!HttpContext.Current.User.IsInRole("Access"))
                if (psb.Length > 0 || fsb.Length > 0)
                {
                    DbUtil.Db.EmailRedacted(p.FromEmail, DbUtil.Db.GetNewPeopleManagers(),
                        "Address Info Changed",
                        "{0} changed the following information:<br />\n<table>{1}{2}</table>"
                        .Fmt(Util.UserName, psb.ToString(), fsb.ToString()));
                }
            return new 
            {
                addr = ViewExtensions2.RenderPartialViewToString(ctl, "DisplayTemplates/Address", this),
                primary = ViewExtensions2.RenderPartialViewToString(ctl, "DisplayTemplates/Address", GetAddressInfo(PeopleId, "PrimaryAddr"))
            };
        }
        private StringBuilder fsb = new StringBuilder();
        private void UpdateValue(Family f, string field, object value)
        {
            var o = Util.GetProperty(f, field);
            if (o == null && value == null)
                return;
            if (o != null && o.Equals(value))
                return;
            fsb.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>\n", field, o, value ?? "(null)");
            Util.SetProperty(f, field, value);
        }
        private StringBuilder psb = new StringBuilder();
        private void UpdateValue(CmsData.Person p, string field, object value)
        {
            var o = Util.GetProperty(p, field);
            if (o == null && value == null)
                return;
            if (o != null && o.Equals(value))
                return;
            psb.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>\n", field, o, value ?? "(null)");
            Util.SetProperty(p, field, value);
        }
        public static IEnumerable<SelectListItem> StateCodes()
        {
            return CmsWeb.Models.QueryModel.ConvertToSelect(CmsWeb.Models.CodeValueModel.GetStateList(), "Code");
        }
    }
}