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

namespace CmsData
{
    public partial class Attend
    {
        public enum AttendTypeCode
        {
            Absent = 0,
            Leader = 10,
            Volunteer = 20,
            Member = 30,
            VisitingMember = 40,
            RecentVisitor = 50,
            NewVisitor = 60,
            InService = 70,
            Offsite = 80,
            Group = 90,
            Homebound = 100,
            OtherClass = 110,
        };
        public enum MemberTypeCode
        {
            VisitingMember = 300,
            Visitor = 310,
            InServiceMember = 500,
        }
        partial void OnValidate(System.Data.Linq.ChangeAction action)
        {
            var o = HttpContext.Current.Items["attendinfo"] as CMSDataContext.AttendMeetingInfo1;
            if (MemberTypeId == (int)OrganizationMember.MemberTypeCode.Visitor && AttendanceTypeId != 40 && AttendanceTypeId != 50 && AttendanceTypeId != 60 && AttendanceTypeId != 110)
            {
                var tw = new StringWriter();
                ObjectDumper.Write(o, 1, tw);
                var smtp = new SmtpClient();
                var u = DbUtil.Db.CurrentUser;
                var email = u.EmailAddress;
                var from = new MailAddress(u.EmailAddress, u.Name);
                var msg = new MailMessage();
                msg.From = from;
                msg.To.Add(WebConfigurationManager.AppSettings["senderrorsto"]);
                msg.Subject = "Attendance Oddity({0}, {1}, {2})".Fmt(MemberTypeId, AttendanceTypeId, o.path);
                msg.Body = "\n{0} ({1}, {2})\n".Fmt(u.EmailAddress, u.UserId, u.Name) + tw.ToString();
                smtp.Send(msg);
            }
        }
        public static string RecordAttendance(int PeopleId, int MeetingId, bool attended)
        {
            var OtherMeetings = new List<Attend>();

            var o = DbUtil.Db.AttendMeetingInfo0(MeetingId, PeopleId);

            if (o.info.AttendedElsewhere > 0 && attended)
                return "{0}({1}) already attended elsewhere".Fmt(o.info.Name, PeopleId);

            if (o.Attendance == null)
            {
                o.Attendance = new Attend
                {
                    OrganizationId = o.Meeting.OrganizationId,
                    PeopleId = PeopleId,
                    MeetingDate = o.Meeting.MeetingDate.Value,
                    CreatedDate = DateTime.Now,
                    CreatedBy = Util.UserId1,
                };
                o.Meeting.Attends.Add(o.Attendance);
            }
            o.Attendance.AttendanceFlag = attended;

            if (o.Meeting.GroupMeetingFlag && o.info.MemberTypeId.HasValue)
            {
                o.Attendance.MemberTypeId = o.info.MemberTypeId.Value;
                if (o.VIPAttendance.Count > 0 && o.VIPAttendance.Any(a => a.AttendanceFlag == true))
                    o.Attendance.AttendanceTypeId = (int)Attend.AttendTypeCode.Volunteer;
                else
                    o.Attendance.AttendanceTypeId = (int)Attend.AttendTypeCode.Group;
                o.path = 1;
            }
            else if (o.info.IsOffSite == true && attended == false)
            {
                o.Attendance.OtherAttends = 1;
                o.Attendance.AttendanceTypeId = (int)Attend.AttendTypeCode.Offsite;
                o.Attendance.MemberTypeId = o.info.MemberTypeId.Value;
                o.path = 2;
            }
            else if (o.info.MemberTypeId.HasValue) // member of this class
            {
                o.Attendance.MemberTypeId = o.info.MemberTypeId.Value;
                o.Attendance.AttendanceTypeId = GetAttendType(o.Attendance.AttendanceFlag, o.Attendance.MemberTypeId, null);
                o.Attendance.BFCAttendance = o.Attendance.OrganizationId == o.info.BFClassId;

                if (o.BFCMember != null && (attended || o.BFCAttendance != null)) // related BFC
                {
                    /* At this point I am recording attendance for a vip class 
                     * or for a class where I am doing InService (long term) teaching
                     * And now I am looking at the BFClass where I am a regular member or an InService Member
                     * I don't need to be here if I am reversing my attendance and there is no BFCAttendance to fix
                     */
                    if (o.BFCAttendance == null)
                        o.BFCAttendance = CreateOtherAttend(o.Meeting, o.BFCMember);

                    o.BFCAttendance.OtherAttends = attended ? 1 : 0;
                    o.Attendance.OtherAttends = o.BFCAttendance.AttendanceFlag ? 1 : 0;

                    if (o.info.MemberTypeId == (int)OrganizationMember.MemberTypeCode.VIP)
                    {
                        if (o.BFCAttendance.OtherAttends > 0)
                            o.BFCAttendance.AttendanceTypeId = (int)Attend.AttendTypeCode.Volunteer;
                        else
                            o.BFCAttendance.AttendanceTypeId = GetAttendType(o.BFCAttendance.AttendanceFlag, o.BFCMember.MemberTypeId, o.BFCMeeting);
                        o.path = 3;
                    }
                    else if (o.BFCMember.MemberTypeId == (int)OrganizationMember.MemberTypeCode.InServiceMember)
                    {
                        if (o.BFCAttendance.OtherAttends > 0)
                            o.BFCAttendance.AttendanceTypeId = (int)Attend.AttendTypeCode.InService;
                        else
                            o.BFCAttendance.AttendanceTypeId = (int)Attend.AttendTypeCode.Member;
                        o.path = 4;
                    }
                    OtherMeetings.Add(o.BFCAttendance);
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
                            o.Attendance.AttendanceTypeId = (int)Attend.AttendTypeCode.Volunteer;
                        OtherMeetings.Add(a);
                    }
                    o.Attendance.OtherAttends = o.VIPAttendance.Any(a => a.AttendanceFlag == true) ? 1 : 0;
                    o.path = 6;
                }
            }
            else // not a member of this class 
            {
                if (o.BFCMember == null)
                // not a member of another class (visitor)
                {
                    o.Attendance.MemberTypeId = (int)Attend.MemberTypeCode.Visitor;
                    if (o.info.IsRecentVisitor.Value)
                        o.Attendance.AttendanceTypeId = (int)Attend.AttendTypeCode.RecentVisitor;
                    else
                        o.Attendance.AttendanceTypeId = (int)Attend.AttendTypeCode.NewVisitor;
                    o.path = 7;
                }
                else
                // member of another class (visiting member)
                {
                    if (attended)
                    {
                        o.Attendance.MemberTypeId = (int)Attend.MemberTypeCode.VisitingMember;
                        o.Attendance.AttendanceTypeId = (int)Attend.AttendTypeCode.VisitingMember;
                    }
                    if (o.BFCAttendance == null)
                        o.BFCAttendance = CreateOtherAttend(o.Meeting, o.BFCMember);

                    o.BFCAttendance.OtherAttends = attended ? 1 : 0;

                    if (o.BFCAttendance.OtherAttends > 0)
                        o.BFCAttendance.AttendanceTypeId = (int)Attend.AttendTypeCode.OtherClass;
                    else
                        o.BFCAttendance.AttendanceTypeId = GetAttendType(o.BFCAttendance.AttendanceFlag, o.BFCMember.MemberTypeId, o.BFCMeeting);
                    o.path = 8;

                    OtherMeetings.Add(o.BFCAttendance);
                }
            }
            try
            {
                HttpContext.Current.Items["attendinfo"] = o;
                DbUtil.Db.SubmitChanges();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error recording attendance pid={0},dt={1}".Fmt(PeopleId, o.Meeting.MeetingDate), ex);
            }

            DbUtil.Db.UpdateAttendStr(o.Meeting.OrganizationId, PeopleId);
            foreach (var m in OtherMeetings)
            {
                DbUtil.Db.UpdateAttendStr(m.OrganizationId, PeopleId);
                DbUtil.Db.UpdateMeetingCounters(m.MeetingId);
            }
            return null; // no error
        }
        private static Attend CreateOtherAttend(Meeting meeting, OrganizationMember member)
        {
            var q = from m in DbUtil.Db.Meetings
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
                    CreatedDate = DateTime.Now,
                    CreatedBy = Util.UserId1,
                    GroupMeetingFlag = false,
                    Location = member.Organization.Location,
                };
                DbUtil.Db.Meetings.InsertOnSubmit(othMeeting);
            }
            var q2 = from a in DbUtil.Db.Attends
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
                    CreatedDate = DateTime.Now,
                    CreatedBy = Util.UserId1,
                    Meeting = othMeeting,
                };
                DbUtil.Db.Attends.InsertOnSubmit(othAttend);
            }
            return othAttend;
        }
        private static int? GetAttendType(bool attended, int? memberTypeId, Meeting meeting)
        {
            if (meeting != null && meeting.GroupMeetingFlag == true)
                return (int)Attend.AttendTypeCode.Group;
            if (!attended)
                return null;

            var q = from mt in DbUtil.Db.MemberTypes
                    where mt.Id == memberTypeId
                    select mt.AttendanceTypeId;
            return q.Single();
        }
    }
}
