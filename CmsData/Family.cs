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
    public partial class Family
    {
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
            if (value is string)
                value = ((string)value).TrimEnd();
            var o = Util.GetProperty(this, field);
            if (o is string)
                o = ((string)o).TrimEnd();
            if (o == null && value == null)
                return;
            if (o != null && o.Equals(value))
                return;
            if (o == null && value is string && !((string)value).HasValue())
                return;
            if (value == null && o is string && !((string)o).HasValue())
                return;
            fsb.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>\n", field, o, value ?? "(null)");
            Util.SetProperty(this, field, value);
        }
        public void UpdateValueFromText(StringBuilder fsb, string field, string value)
        {
            value = value.TrimEnd();
            var o = Util.GetProperty(this, field);
            if (o is string)
                o = ((string)o).TrimEnd();
            if (o == null && value == null)
                return;
            if (o != null && o.Equals(value))
                return;
            if (o == null && value is string && !((string)value).HasValue())
                return;
            if (value == null && o is string && !((string)o).HasValue())
                return;
            fsb.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>\n", field, o, value ?? "(null)");
            Util.SetPropertyFromText(this, field, value);
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
                    Field = "Family",
                    Data = "<table>\n" + fsb.ToString() + "</table>",
                    Created = Util.Now
                };
                DbUtil.Db.ChangeLogs.InsertOnSubmit(c);
            }
        }
    }
}

