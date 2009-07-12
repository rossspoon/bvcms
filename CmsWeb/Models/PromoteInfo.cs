using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UtilityExtensions;

namespace CMSWeb.Models
{
    public class PromoteInfo
    {
        public bool IsSelected { get; set; }
        public string Checked
        {
            get { return IsSelected ? "checked='checked'" : ""; }
        }
        public int PeopleId { get; set; }
        public string Name { get; set; }
        public string Name2 { get; set; }
        public int CurrClassId { get; set; }
        public string CurrClassName 
        { 
            get { return CmsData.Organization.FormatOrgName(CurrOrgName, CurrLeader, CurrLoc); } 
        }
        public string CurrOrgName { get; set; }
        public string CurrLeader { get; set; }
        public string CurrLoc { get; set; }
        public int? PendingClassId { get; set; }
        public string PendingClassName
        {
            get { return CmsData.Organization.FormatOrgName(PendingOrgName, PendingLeader, PendingLoc); }
        }
        public string PendingOrgName { get; set; }
        public string PendingLeader { get; set; }
        public string PendingLoc { get; set; }
        public decimal? AttendPct { get; set; }
        public string AttendIndicator
        {
            get { return AttendPct > 80 ? "Hi" : AttendPct > 40 ? "Med" : "Lo"; }
        }
        public string Gender { get; set; }
        public int? BDay { get; set; }
        public int? BMon { get; set; }
        public int? BYear { get; set; }
        public string Birthday 
        { 
            get { return Util.FormatBirthday(BYear, BMon, BDay); } 
        }
        public int Hash { get; set; }
    }
}
