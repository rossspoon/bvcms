using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMSWeb.Models
{
    public class OrgMemberInfo
    {
        public int OrgId { get; set; }
        public int PeopleId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string LeaderName { get; set; }
        public DateTime? MeetingTime { get; set; }
        public string MemberType { get; set; }
        public int? LeaderId { get; set; }
        public DateTime? EnrollDate { get; set; }
        public Decimal? AttendPct { get; set; }
        public string DivisionName { get; set; }
    }
}
