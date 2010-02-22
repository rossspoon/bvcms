using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UtilityExtensions;

namespace CMSWeb.Models
{
    public class Attendee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string First { get; set; }
        public string Last { get; set; }
        public string Birthday { get; set; }
        public int? BMon { get; set; }
        public int? BDay { get; set; }
        public int? BYear { get; set; }
        public string BirthDay
        {
            get { return Util.FormatBirthday(BYear, BMon, BDay); }
        }
        public string Class { get; set; }
        public DateTime? Hour { get; set; }
        public int OrgId { get; set; }
        public string Location { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public int NumLabels { get; set; }
        public int? CampusId { get; set; }
        public string Leader { get; set; }
        public string DisplayName
        {
            get
            {
                if (Age <= 18)
                    return "{0} ({1})".Fmt(Name, Age);
                return Name;
            }
        }
        public string DisplayClass
        {
            get
            {
                if (Hour.HasValue)
                    return "{0} ({1:h:mm})".Fmt(
                        CmsData.Organization.FormatOrgName(Class, Leader, Location), Hour);
                return Class;
            }
        }
        public bool CheckedIn { get; set; }
    }
    public class FamilyInfo
    {
        public int FamilyId { get; set; }
        public string Name { get; set; }
        public string AreaCode { get; set; }
    }
}
