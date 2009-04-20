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
        public bool ChildRollSheet { get; set; }
    }
}
