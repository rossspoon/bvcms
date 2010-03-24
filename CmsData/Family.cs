using System;
using System.Linq;
using UtilityExtensions;
using System.Text;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Linq.Expressions;
using System.ComponentModel;

namespace CmsData
{
    public partial class Family : IAuditable
    {
        public enum PositionInFamily
        {
            PrimaryAdult = 10,
            SecondaryAdult = 20,
            Child = 30,
        }
        public string CityStateZip
        {
            get { return Util.FormatCSZ4(CityName,StateCode,ZipCode); }
        }
        public string AddrCityStateZip
        {
            get { return AddressLineOne + " " + CityStateZip; }
        }
        public string Addr2CityStateZip
        {
            get { return AddressLineTwo + " " + CityStateZip; }
        }


        public string HohName
        {
            get
            {
                if (HeadOfHouseholdId.HasValue) 
                    return DbUtil.Db.People.Single(p => p.PeopleId == HeadOfHouseholdId.Value).Name;
                return "";
            }
        }

        public string HohSpouseName
        {
            get
            {
                if (HeadOfHouseholdSpouseId.HasValue) 
                    return DbUtil.Db.People.Single(p => p.PeopleId == HeadOfHouseholdSpouseId.Value).Name;
                return "";
            }
        }
        public string FamilyName
        {
            get { return "The " + HohName + " Family"; }
        }

        public int MemberCount
        {
            get { return People.Count; }
        }
        private CMSDataContext _Db;
        public CMSDataContext Db
        {
            get
            {
                if (_Db == null)
                    _Db = this.GetDataContext() as CMSDataContext;
                return _Db;
            }
        }

        partial void OnZipCodeChanged()
        {
            ResCodeId = Person.FindResCode(ZipCode);
        }
        partial void OnAltZipCodeChanged()
        {
            AltResCodeId = Person.FindResCode(AltZipCode);
        }
    }
}

