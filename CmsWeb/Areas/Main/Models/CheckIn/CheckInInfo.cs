using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UtilityExtensions;

namespace CmsWeb.Models
{
    public class Attendee
    {
        public int Id { get; set; }
        public int Position { get; set; }
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
        public string MemberVisitor { get; set; }
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
                string s = "";
                if (Location.HasValue())
                    if (!Class.StartsWith(Location))
                        s = Location + ", ";
                s += Class;
                if (Leader.HasValue())
                    s += ", " + Leader;
                return s;
            }
        }
        public bool CheckedIn { get; set; }
        public bool Custody { get; set; }
        public bool Transport { get; set; }
        public bool RequiresSecurityLabel { get; set; }

        public string dob
        {
            get
            {
                var dt = DateTime.MinValue;
                DateTime? bd = null;
                if (DateTime.TryParse(BirthDay, out dt))
                    bd = dt;
                return bd.FormatDate2();
            }
        }
        public string goesby { get; set; }
        public string email { get; set; }
        public string addr { get; set; }
        public string zip { get; set; }
        public string home { get; set; }
        public string cell { get; set; }
        public int gender { get; set; }
        public int marital { get; set; }
        public string allergies { get; set; }
        public int? grade { get; set; }
        public string parent { get; set; }
        public string emfriend { get; set; }
        public string emphone { get; set; }
        public bool activeother { get; set; }
        public bool HasPicture { get; set; }
    }
    public class FamilyInfo
    {
        public int FamilyId { get; set; }
        public string Name { get; set; }
        public string AreaCode { get; set; }
        public string Phone { get; set; }
        public bool Locked { get; set; }
        public int seconds { get; set; }
    }
}
