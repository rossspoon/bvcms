using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Web;
using UtilityExtensions;
using System.IO;
using System.Net.Mail;
using System.Web.Configuration;
using System.Data.SqlClient;
using CmsData.Codes;

namespace CmsData
{
    public partial class Attend
    {
        public static int[] VisitAttendTypes = new int[] 
        { 
            AttendTypeCode.VisitingMember, 
            AttendTypeCode.RecentVisitor, 
            AttendTypeCode.NewVisitor 
        };
        public static string RecordAttendance(int PeopleId, int MeetingId, bool attended)
        {
            return RecordAttendance(DbUtil.Db, PeopleId, MeetingId, attended);
        }
        public static string RecordAttendance(CMSDataContext Db, int PeopleId, int MeetingId, bool attended)
        {
            var OtherMeetings = new List<Attend>();

            var o = Db.AttendMeetingInfo0(MeetingId, PeopleId);

            if (o.info.AttendedElsewhere > 0 && attended)
                return "{0}({1}) already attended elsewhere".Fmt(o.info.Name, PeopleId);

            Attend attend = null;
            if (o.Attendance == null)
                attend = new Attend
                {
                    OrganizationId = o.Meeting.OrganizationId,
                    PeopleId = PeopleId,
                    MeetingDate = o.Meeting.MeetingDate.Value,
                    AttendanceFlag = attended,
                    CreatedDate = Util.Now,
                    CreatedBy = Util.UserId1,
                    AttendanceTypeId = 0,
                    BFCAttendance = null,
                    OtherAttends = 0,
                    MemberTypeId = 0,
                    OtherOrgId = 0,
                };
            else
                attend = new Attend
                {
                    OrganizationId = o.Attendance.OrganizationId,
                    PeopleId = o.Attendance.PeopleId,
                    MeetingDate = o.Attendance.MeetingDate,
                    AttendanceFlag = attended,
                    CreatedDate = o.Attendance.CreatedDate,
                    CreatedBy = o.Attendance.CreatedBy,
                    AttendanceTypeId = o.Attendance.AttendanceTypeId,
                    BFCAttendance = o.Attendance.BFCAttendance,
                    OtherAttends = o.Attendance.OtherAttends,
                    MemberTypeId = o.Attendance.MemberTypeId,
                    OtherOrgId = o.Attendance.OtherOrgId,
                };

            if (o.Meeting.GroupMeetingFlag && o.info.MemberTypeId.HasValue)
            {
                attend.MemberTypeId = o.info.MemberTypeId.Value;
                if (o.VIPAttendance.Count > 0 && o.VIPAttendance.Any(a => a.AttendanceFlag == true))
                    attend.AttendanceTypeId = AttendTypeCode.Volunteer;
                else
                    attend.AttendanceTypeId = AttendTypeCode.Group;
                o.path = 1;
            }
            else if (o.info.IsOffSite == true && attended == false)
            {
                attend.OtherAttends = 1;
                attend.AttendanceTypeId = AttendTypeCode.Offsite;
                attend.MemberTypeId = o.info.MemberTypeId.Value;
                o.path = 2;
            }
            else if (o.info.MemberTypeId.HasValue) 
            {
                attend.MemberTypeId = o.info.MemberTypeId.Value;
                attend.AttendanceTypeId = GetAttendType(Db, attend.AttendanceFlag, attend.MemberTypeId, null);
                attend.BFCAttendance = attend.OrganizationId == o.info.BFClassId;

                if (o.BFCMember != null && (attended || o.BFCAttendance != null)) // related BFC
                {
                    /* At this point I am recording attendance for a vip class 
                     * or for a class where I am doing InService (long term) teaching
                     * And now I am looking at the BFClass where I am a regular member or an InService Member
                     * I don't need to be here if I am reversing my attendance and there is no BFCAttendance to fix
                     */
                    if (o.BFCAttendance == null)
                        o.BFCAttendance = CreateOtherAttend(Db, o.Meeting, o.BFCMember);
                    o.BFCAttendance.OtherAttends = attended ? 1 : 0;

                    attend.OtherAttends = o.BFCAttendance.AttendanceFlag ? 1 : 0;

                    if (o.info.MemberTypeId == MemberTypeCode.VIP)
                    {
                        if (o.BFCAttendance.OtherAttends > 0)
                            o.BFCAttendance.AttendanceTypeId = AttendTypeCode.Volunteer;
                        else
                            o.BFCAttendance.AttendanceTypeId = GetAttendType(Db, o.BFCAttendance.AttendanceFlag, o.BFCMember.MemberTypeId, o.BFCMeeting);
                        o.path = 3;
                    }
                    else if (o.BFCMember.MemberTypeId == MemberTypeCode.InServiceMember)
                    {
                        if (o.BFCAttendance.OtherAttends > 0)
                            o.BFCAttendance.AttendanceTypeId = AttendTypeCode.InService;
                        else
                            o.BFCAttendance.AttendanceTypeId = AttendTypeCode.Member;
                        o.path = 4;
                    }
                    OtherMeetings.Add(o.BFCAttendance);
                    DbUtil.Db.SubmitChanges(); // commit other attendance
                }
                else if (o.VIPAttendance.Count > 0) // need to indicate BFCAttendance or not
                {
                    /* At this point I am recording attendance for a BFClass
                     * And now I am looking at the one or more VIP classes where I a sometimes volunteer
                     */
                    foreach (var a in o.VIPAttendance)
                    {
                        a.OtherAttends = attended ? 1 : 0;
                        if (a.AttendanceFlag == true)
                            attend.AttendanceTypeId = AttendTypeCode.Volunteer;
                        OtherMeetings.Add(a);
                    }
                    attend.OtherAttends = o.VIPAttendance.Any(a => a.AttendanceFlag == true) ? 1 : 0;
                    o.path = 6;
                    DbUtil.Db.SubmitChanges(); // commit other attendance
                }
            }
            else // not a member of this class 
            {
                if (o.BFCMember == null)
                // not a member of another class (visitor)
                {
                    attend.MemberTypeId = MemberTypeCode.Visitor;
                    if (o.info.IsRecentVisitor.Value)
                        attend.AttendanceTypeId = AttendTypeCode.RecentVisitor;
                    else
                        attend.AttendanceTypeId = AttendTypeCode.NewVisitor;
                    o.path = 7;
                }
                else
                // member of another class (visiting member)
                {
                    if (attended)
                    {
                        attend.MemberTypeId = MemberTypeCode.VisitingMember;
                        attend.AttendanceTypeId = AttendTypeCode.VisitingMember;
                    }
                    if (o.BFCAttendance == null)
                        o.BFCAttendance = CreateOtherAttend(Db, o.Meeting, o.BFCMember);
                    o.BFCAttendance.OtherAttends = attended ? 1 : 0;

                    if (o.BFCAttendance.OtherAttends > 0)
                        o.BFCAttendance.AttendanceTypeId = AttendTypeCode.OtherClass;
                    else
                        o.BFCAttendance.AttendanceTypeId = GetAttendType(Db, o.BFCAttendance.AttendanceFlag, o.BFCMember.MemberTypeId, o.BFCMeeting);
                    o.path = 8;

                    OtherMeetings.Add(o.BFCAttendance);
                    DbUtil.Db.SubmitChanges(); // commit other attendance
                }
            }
            if (o.Attendance == null) // add the new one
                o.Meeting.Attends.Add(attend);
            else // update existing
            {
                o.Attendance.AttendanceTypeId = attend.AttendanceTypeId;
                o.Attendance.BFCAttendance = attend.BFCAttendance;
                o.Attendance.OtherAttends = attend.OtherAttends;
                o.Attendance.MemberTypeId = attend.MemberTypeId;
                o.Attendance.OtherOrgId = attend.OtherOrgId;
                o.Attendance.AttendanceFlag = attend.AttendanceFlag;
            }
            try
            {
                if (HttpContext.Current != null)
                    HttpContext.Current.Items["attendinfo"] = o;
                Db.SubmitChanges();
                Db.UpdateAttendStr(o.Meeting.OrganizationId, PeopleId);
            }
            catch (SqlException ex)
            {
                throw new Exception("Error recording attendance pid={0},dt={1}".Fmt(PeopleId, o.Meeting.MeetingDate), ex);
            }

            foreach (var m in OtherMeetings)
            {
                Db.UpdateAttendStr(m.OrganizationId, PeopleId);
                Db.UpdateMeetingCounters(m.MeetingId);
            }
            return "ok"; // no error
        }
        private static Attend CreateOtherAttend(CMSDataContext Db, Meeting meeting, OrganizationMember member)
        {
            var q = from m in Db.Meetings
                    where m.MeetingDate == meeting.MeetingDate
                    where m.OrganizationId == member.OrganizationId
                    select m;
            var othMeeting = q.SingleOrDefault();

            if (othMeeting == null)
            {
                othMeeting = new Meeting
                {
                    OrganizationId = member.OrganizationId,
                    MeetingDate = meeting.MeetingDate,
                    CreatedDate = Util.Now,
                    CreatedBy = Util.UserId1,
                    GroupMeetingFlag = false,
                    Location = member.Organization.Location,
                };
                Db.Meetings.InsertOnSubmit(othMeeting);
                Db.SubmitChanges();
            }
            var q2 = from a in Db.Attends
                     where a.PeopleId == member.PeopleId
                     where a.MeetingId == othMeeting.MeetingId
                     select a;
            var othAttend = q2.SingleOrDefault();
            if (othAttend == null) // attendance not recorded yet
            {
                othAttend = new Attend
                {
                    PeopleId = member.PeopleId,
                    OrganizationId = member.OrganizationId,
                    MemberTypeId = member.MemberTypeId,
                    MeetingDate = meeting.MeetingDate.Value,
                    CreatedDate = Util.Now,
                    CreatedBy = Util.UserId1,
                    Meeting = othMeeting,
                };
                Db.Attends.InsertOnSubmit(othAttend);
            }
            return othAttend;
        }
        private static int? GetAttendType(CMSDataContext Db, bool attended, int? memberTypeId, Meeting meeting)
        {
            if (meeting != null && meeting.GroupMeetingFlag == true)
                return AttendTypeCode.Group;
            if (!attended)
                return null;

            var q = from mt in Db.MemberTypes
                    where mt.Id == memberTypeId
                    select mt.AttendanceTypeId;
            return q.Single();
        }
    }
}
