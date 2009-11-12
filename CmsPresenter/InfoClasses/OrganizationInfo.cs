using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilityExtensions;

namespace CMSPresenter
{
    public class OrganizationInfo
    {
        public int OrganizationId { get; set; }
        public int? OrganizationStatus { get; set; }
        public string OrganizationName { get; set; }
        public string LeaderName { get; set; }
        public int? LeaderId { get; set; }
        public int? MemberCount { get; set; }
        public string AttendanceTrackingLevel { get; set; }
        public int? DivisionId { get; set; }
        public string DivisionName { get; set; }
        public string FirstMeetingDate { get; set; }
        public string LastMeetingDate { get; set; }
        public string Schedule { get; set; }
        public string Location { get; set; }
        public bool HasTag { get; set; }
        public int? VisitorCount { get; set; }
        public bool AllowSelfCheckIn { get; set; }
    }
    public class OrganizationInfoExcel
    {
        public int OrgId { get; set; }
        public string Status { get; set; }
        public string Name { get; set; }
        public string Leader { get; set; }
        public int Members { get; set; }
        public string Tracking { get; set; }
        public string Division { get; set; }
        public string FirstMeeting { get; set; }
        public string Schedule { get; set; }
        public string Location { get; set; }
    }
}
