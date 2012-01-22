using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMSPresenter
{
    public class MeetingInfo0
    {
        public int MeetingId { get; set; }
        public DateTime? MeetingDate { get; set; }
        public DateTime? Time
        {
            get
            {
                if (MeetingDate.HasValue)
                    if (MeetingDate.Value.TimeOfDay.TotalSeconds > 0)
                        return MeetingDate;
                return null;
            }
        }
        //public string MeetingStatus { get; set; }
        public int NumPresent { get; set; }
        public int NumVisitors { get; set; }
        //public int? AttendPercent { get; set; }
        public int NewVisitors { get; set; }
        public int RepeatVisitors { get; set; }
        public int VisitingMembers { get; set; }
        //public int BeginningEnrollment { get; set; }
        //public int EndingEnrollment { get; set; }
        public bool GroupMeeting { get; set; }
        public string Location { get; set; }
        public int OrganizationId { get; set; }
        public string Description { get; set; }
    }
    public class MeetingInfo : MeetingInfo0
    {
        public string OrganizationName { get; set; }
        public string LeaderName { get; set; }
    }
}
