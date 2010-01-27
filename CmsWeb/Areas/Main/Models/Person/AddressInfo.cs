using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using System.Web.Mvc;
using UtilityExtensions;
using CMSPresenter;

namespace CMSWeb.Models
{
    public class AddressInfo
    {
        private CodeValueController cv = new CodeValueController();

        public int PeopleId { get; set; }
        public string Name { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string CityStateZip()
        {
            return Util.FormatCSZ4(City, State, Zip);
        }
        public string AddrCityStateZip()
        {
            return Address1 + ";" + CityStateZip();
        }
        public string Addr2CityStateZip()
        {
            return Address2 + ";" + CityStateZip();
        }
        public bool? BadAddress { get; set; }
        public int? ResCodeId { get; set; }
        public string ResCode
        {
            get { return CodeValueController.ResidentCodes().ItemValue(ResCodeId); }
        }
        public bool Preferred { get; set; }
        public DateTime? FromDt { get; set; }
        public DateTime? ToDt { get; set; }

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

        public static AddressInfo GetAddressInfo(int id, string typeid)
        {
            var p = DbUtil.Db.LoadPersonById(id);
            var a = new AddressInfo();
            switch (typeid)
            {
                case "PersonalAddr":
                    a.Address1 = p.AddressLineOne;
                    a.Address2 = p.AddressLineTwo;
                    a.ToDt = p.AddressToDate;
                    a.BadAddress = p.BadAddressFlag;
                    a.City = p.CityName;
                    a.State = p.StateCode;
                    a.Zip = p.ZipCode;
                    a.ResCodeId = p.ResCodeId;
                    break;
                case "FamilyAddr":
                    a.Address1 = p.Family.AddressLineOne;
                    a.Address2 = p.Family.AddressLineTwo;
                    a.ToDt = p.Family.AddressToDate;
                    a.BadAddress = p.Family.BadAddressFlag;
                    a.City = p.Family.CityName;
                    a.State = p.Family.StateCode;
                    a.Zip = p.Family.ZipCode;
                    a.ResCodeId = p.Family.ResCodeId;
                    break;
                case "AltPersonalAddr":
                    a.Address1 = p.AltAddressLineOne;
                    a.Address2 = p.AltAddressLineTwo;
                    a.ToDt = p.AltAddressToDate;
                    a.BadAddress = p.AltBadAddressFlag;
                    a.City = p.AltCityName;
                    a.State = p.AltStateCode;
                    a.Zip = p.AltZipCode;
                    a.ResCodeId = p.AltResCodeId;
                    break;
                case "AltFamilyAddr":
                    a.Address1 = p.Family.AltAddressLineOne;
                    a.Address2 = p.Family.AltAddressLineTwo;
                    a.ToDt = p.Family.AltAddressToDate;
                    a.BadAddress = p.Family.AltBadAddressFlag;
                    a.City = p.Family.AltCityName;
                    a.State = p.Family.AltStateCode;
                    a.Zip = p.Family.AltZipCode;
                    a.ResCodeId = p.Family.AltResCodeId;
                    break;
            }
            return a;
        }

        public void UpdateAddress()
        {
            var p = DbUtil.Db.LoadPersonById(PeopleId);
            switch (Name)
            {
                case "PersonalAddr":
                    p.AddressLineOne = Address1;
                    p.AddressLineTwo = Address2;
                    p.AddressToDate = ToDt;
                    p.BadAddressFlag = BadAddress;
                    p.CityName = City;
                    p.StateCode = State;
                    p.ZipCode = Zip;
                    p.ResCodeId = ResCodeId;
                    break;
                case "FamilyAddr":
                    p.Family.AddressLineOne = Address1;
                    p.Family.AddressLineTwo = Address2;
                    p.Family.AddressToDate = ToDt;
                    p.Family.BadAddressFlag = BadAddress;
                    p.Family.CityName = City;
                    p.Family.StateCode = State;
                    p.Family.ZipCode = Zip;
                    p.Family.ResCodeId = ResCodeId;
                    break;
                case "AltPersonalAddr":
                    p.AltAddressLineOne = Address1;
                    p.AltAddressLineTwo = Address2;
                    p.AltAddressToDate = ToDt;
                    p.AltBadAddressFlag = BadAddress;
                    p.AltCityName = City;
                    p.AltStateCode = State;
                    p.AltZipCode = Zip;
                    p.AltResCodeId = ResCodeId;
                    break;
                    break;
                case "AltFamilyAddr":
                    p.Family.AltAddressLineOne = Address1;
                    p.Family.AltAddressLineTwo = Address2;
                    p.Family.AltAddressToDate = ToDt;
                    p.Family.AltBadAddressFlag = BadAddress;
                    p.Family.AltCityName = City;
                    p.Family.AltStateCode = State;
                    p.Family.AltZipCode = Zip;
                    p.Family.AltResCodeId = ResCodeId;
                    break;
            }
            DbUtil.Db.SubmitChanges();
        }
    }
}
