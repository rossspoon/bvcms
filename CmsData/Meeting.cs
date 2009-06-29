using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilityExtensions;
using System.Data.Linq;

namespace CmsData
{
    public partial class Meeting
    {
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
        public bool UpdateMeetingCounters0()
        {
            var members = new int[] 
            {
                (int)Attend.AttendTypeCode.Leader, 
                (int)Attend.AttendTypeCode.Member,
                (int)Attend.AttendTypeCode.Volunteer,
            };

            var q = from m in Db.Meetings
                    where m.MeetingId == this.MeetingId
                    select new
                    {
                        NumPresent = m.Attends.Count(a => a.AttendanceFlag == true),
                        NumNewVisit = m.Attends.Count(a => a.AttendanceFlag == true && a.AttendanceTypeId == (int)Attend.AttendTypeCode.NewVisitor),
                        NumMembers = m.Attends.Count(a => a.AttendanceFlag == true && members.Contains(a.AttendanceTypeId.Value)),
                        NumVstMembers = m.Attends.Count(a => a.AttendanceFlag == true && a.AttendanceTypeId == (int)Attend.AttendTypeCode.VisitingMember),
                        NumRepeatVst = m.Attends.Count(a => a.AttendanceFlag == true && a.AttendanceTypeId == (int)Attend.AttendTypeCode.RecentVisitor),
                    };

            var changed = false;
            var r = q.Single();

            if (!GroupMeetingFlag)
            {
                if (NumPresent != r.NumPresent)
                {
                    changed = true;
                    NumPresent = r.NumPresent;
                }
                if (NumMembers != r.NumMembers)
                {
                    changed = true;
                    NumMembers = r.NumMembers;
                }
                if (NumVstMembers != r.NumVstMembers)
                {
                    changed = true;
                    NumVstMembers = r.NumVstMembers;
                }
            }
            if (NumNewVisit != r.NumNewVisit)
            {
                changed = true;
                NumNewVisit = r.NumNewVisit;
            }
            if (NumRepeatVst != r.NumRepeatVst)
            {
                changed = true;
                NumRepeatVst = r.NumRepeatVst;
            }
            return changed;
        }
        public bool AddAbsents()
        {
            if (GroupMeetingFlag != false)
                return false;

            var q0 = from om in Db.OrganizationMembers
                     where !Db.Attends.Any(a => om.PeopleId == a.PeopleId && a.MeetingId == MeetingId)
                     select new { om.PeopleId, om.MemberTypeId };
            var ret = false;
            foreach (var i in q0)
            {
                ret = true;
                Attends.Add(new Attend
                {
                    MeetingDate = MeetingDate.Value,
                    PeopleId = i.PeopleId,
                    CreatedBy = Util.UserId1,
                    CreatedDate = Util.Now,
                    AttendanceFlag = false,
                    OrganizationId = OrganizationId,
                    AttendanceTypeId = (int)Attend.AttendTypeCode.Member,
                    MemberTypeId = i.MemberTypeId,
                });
            }
            return ret;
        }
    }
}
