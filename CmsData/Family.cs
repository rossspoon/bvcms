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

        public void UpdateValue(StringBuilder fsb, string field, object value)
        {
            var o = Util.GetProperty(this, field);
            if (o == null && value == null)
                return;
            if (o != null && o.Equals(value))
                return;
            fsb.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>\n", field, o, value ?? "(null)");
            Util.SetProperty(this, field, value);
        }
        public void LogChanges(CMSDataContext Db, StringBuilder fsb, int PeopleId, int UserPeopleId)
        {
            if (fsb.Length > 0)
            {
                var c = new ChangeLog
                {
                    FamilyId = FamilyId,
                    UserPeopleId = UserPeopleId,
                    PeopleId = PeopleId,
                    Field = "HomePhone",
                    Data = "<table>\n" + fsb.ToString() + "</table>",
                    Created = Util.Now
                };
                DbUtil.Db.ChangeLogs.InsertOnSubmit(c);
            }
        }
    }
}

