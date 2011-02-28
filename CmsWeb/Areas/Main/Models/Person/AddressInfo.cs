using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using System.Web.Mvc;
using UtilityExtensions;
using CMSPresenter;
using System.Data.Linq;
using System.Text;

namespace CmsWeb.Models.PersonPage
{
    public class AddressInfo
    {
        private CodeValueController cv = new CodeValueController();

        public int PeopleId { get; set; }
        public Person person { get; set; }

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

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string CityStateZip()
        {
            return Util.FormatCSZ(City, State, Zip);
        }
        public string CityStateZip4()
        {
            return Util.FormatCSZ4(City, State, Zip);
        }
        public string AddrCityStateZip()
        {
            return Address1 + " " + CityStateZip();
        }
        public string Addr2CityStateZip()
        {
            return Address2 + " " + CityStateZip();
        }
        public bool? BadAddress { get; set; }
        public int? ResCodeId { get; set; }
        public string ResCode
        {
            get { return CodeValueController.ResidentCodesWithZero().ItemValue(ResCodeId ?? 0); }
        }
        public bool Preferred { get; set; }
        public DateTime? FromDt { get; set; }
        public DateTime? ToDt { get; set; }

        public static IEnumerable<SelectListItem> ResCodes()
        {
            return QueryModel.ConvertToSelect(CodeValueController.ResidentCodesWithZero(), "Id");
        }
        public static IEnumerable<SelectListItem> States()
        {
            return QueryModel.ConvertToSelect(CodeValueController.GetStateList(), "Code");
        }

        public static AddressInfo GetAddressInfo(int id, string typeid)
        {
            var p = DbUtil.Db.LoadPersonById(id);
            var a = new AddressInfo();
            a.person = p;
            switch (typeid)
            {
                case "FamilyAddr":
                    a.Name = typeid;
                    a.PeopleId = p.PeopleId;
                    a.Address1 = p.Family.AddressLineOne;
                    a.Address2 = p.Family.AddressLineTwo;
                    a.ToDt = p.Family.AddressToDate;
                    a.BadAddress = p.Family.BadAddressFlag;
                    a.City = p.Family.CityName;
                    a.State = p.Family.StateCode;
                    a.Zip = p.Family.ZipCode;
                    a.ResCodeId = p.Family.ResCodeId;
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
                    a.State = p.StateCode;
                    a.Zip = p.ZipCode;
                    a.ResCodeId = p.ResCodeId;
                    a.Preferred = p.AddressTypeId == 30;
                    break;
            }
            return a;
        }

        public void UpdateAddress()
        {
            if (ResCodeId == 0)
                ResCodeId = null;
            var p = DbUtil.Db.LoadPersonById(PeopleId);
            var f = p.Family;
            switch (Name)
            {
                case "FamilyAddr":
                    UpdateValue(f, "AddressLineOne", Address1);
                    UpdateValue(f, "AddressLineTwo", Address2);
                    UpdateValue(f, "AddressToDate", ToDt);
                    UpdateValue(f, "CityName", City);
                    UpdateValue(f, "StateCode", State);
                    UpdateValue(f, "ResCodeId", ResCodeId);
                    UpdateValue(f, "ZipCode", Zip);
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
                    UpdateValue(p, "StateCode", State);
                    UpdateValue(p, "ResCodeId", ResCodeId);
                    UpdateValue(p, "ZipCode", Zip);
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
                    Data = "<table>\n" + psb.ToString() + "</table>",
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
                    Data = "<table>\n" + fsb.ToString() + "</table>",
                    Created = Util.Now
                };
                DbUtil.Db.ChangeLogs.InsertOnSubmit(c);
            }
            DbUtil.Db.SubmitChanges();
            if (!HttpContext.Current.User.IsInRole("Access"))
                if (psb.Length > 0 || fsb.Length > 0)
                {
                    var smtp = Util.Smtp();        
                    Util.Email(smtp, p.FromEmail, DbUtil.NewPeopleEmailAddress,
                        "Address Info Changed",
                        "{0} changed the following information:<br />\n<table>{1}{2}</table>"
                        .Fmt(Util.UserName, psb.ToString(),fsb.ToString()));
                }
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
        private void UpdateValue(Person p, string field, object value)
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
            return QueryModel.ConvertToSelect(CodeValueController.GetStateList(), "Code");
        }
    }
}
