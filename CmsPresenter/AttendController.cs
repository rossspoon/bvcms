/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using UtilityExtensions;
using CmsData;
using System.ComponentModel;
using System.Collections;
using System.Transactions;
using CMSPresenter.InfoClasses;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Web;

namespace CMSPresenter
{
    [DataObject]
    public class AttendController
    {
        private List<CodeValueItem> AttendCodes;
        private List<MemberTypeItem> MemberCodes;
        public static int[] VisitAttendTypes = new int[] 
        { 
            (int)Attend.AttendTypeCode.VisitingMember, 
            (int)Attend.AttendTypeCode.RecentVisitor, 
            (int)Attend.AttendTypeCode.NewVisitor 
        };
        public AttendController()
        {
            var c = new CodeValueController();
            AttendCodes = c.AttendanceTypeCodes();
            MemberCodes = CodeValueController.MemberTypeCodes2();
        }

        private int _count;

        public IEnumerable<AttendInfo> Attendees(int meetid, bool inEditMode)
        {
            var meeting = DbUtil.Db.Meetings.Single(m => m.MeetingId == meetid); // this will come out of cache, because Db has already read it

            var wks = 3; // default lookback
            if (meeting.Organization.RollSheetVisitorWks.HasValue)
                wks = meeting.Organization.RollSheetVisitorWks.Value;

            var dt = meeting.MeetingDate.Value.AddDays(wks * -7);

            var q = from p in DbUtil.Db.People
                    let membership = p.OrganizationMembers.SingleOrDefault(om => om.OrganizationId == meeting.OrganizationId)
                    let ismember = p.OrganizationMembers.Any(om => om.OrganizationId == meeting.OrganizationId && (om.Pending ?? false) == false)
                    let attend = p.Attends.SingleOrDefault(a => a.OrganizationId == meeting.OrganizationId && a.MeetingId == meetid)
                    where
                        // anybody who attended this meeting
                        p.Attends.Any(a => a.MeetingId == meetid && a.AttendanceFlag == true)
                    || ( // visitors who attended in recent weeks
                        inEditMode &&
                        p.Attends.Any(a =>
                            a.AttendanceFlag != false
                            && (a.MeetingDate >= dt && a.MeetingDate <= meeting.MeetingDate)
                            && a.MeetingDate >= a.Organization.FirstMeetingDate
                            && a.OrganizationId == meeting.OrganizationId
                            && VisitAttendTypes.Contains(a.AttendanceTypeId.Value)
                            )
                        )
                    || // members
                        (inEditMode && !meeting.GroupMeetingFlag
                        && p.OrganizationMembers.Any(om => om.OrganizationId == meeting.OrganizationId
                            && om.MemberTypeId != (int)OrganizationMember.MemberTypeCode.InActive))

                    select new AttendInfo
                    {
                        AttendType = DbUtil.Db.AttendDesc(attend == null ?
                            (ismember ?
                                (int)Attend.AttendTypeCode.Member :
                                (int)Attend.AttendTypeCode.RecentVisitor) :
                            attend.AttendanceTypeId),
                        MeetingName = "Test",
                        MeetingDate = meeting.MeetingDate,
                        MemberType = DbUtil.Db.MemberDesc(attend == null ?
                            (ismember ?
                                membership.MemberTypeId :
                                (int)OrganizationMember.MemberTypeCode.Visitor) :
                            attend.MemberTypeId),
                        Name = p.Name2,
                        PeopleId = p.PeopleId,
                        AttendFlag = attend == null ? false : attend.AttendanceFlag,
                        RollSheetSectionId = ismember ? 1 : 2,
                    };

            q = from a in q
                orderby a.RollSheetSectionId, a.Name
                select a;
            return q;
        }

        public class AttendedInfo
        {
            public int PeopleId;
            public bool? AttendFlag;
            public bool noRecord;
            public bool isMember;
        }
        public IEnumerable<AttendedInfo> RollSheetAttendees(int meetid)
        {
            var meeting = DbUtil.Db.Meetings.Single(m => m.MeetingId == meetid); // this will come out of cache, because Db has already read it

            var wks = 3; // default lookback
            if (meeting.Organization.RollSheetVisitorWks.HasValue)
                wks = meeting.Organization.RollSheetVisitorWks.Value;

            var dt = meeting.MeetingDate.Value.AddDays(wks * -7);

            var q = from p in DbUtil.Db.People
                    let attend = p.Attends.SingleOrDefault(a => a.OrganizationId == meeting.OrganizationId && a.MeetingId == meetid)
                    where
                        // anybody who attended this meeting
                        p.Attends.Any(a => a.MeetingId == meetid && a.AttendanceFlag == true)
                    || (// visitors who attended in recent weeks
                        p.Attends.Any(a =>
                            a.AttendanceFlag != false
                            && (a.MeetingDate >= dt && a.MeetingDate <= meeting.MeetingDate)
                            && a.MeetingDate >= a.Organization.FirstMeetingDate
                            && a.OrganizationId == meeting.OrganizationId
                            && VisitAttendTypes.Contains(a.AttendanceTypeId.Value)
                            )
                        )
                    || // members
                        (p.OrganizationMembers.Any(om => om.OrganizationId == meeting.OrganizationId && (om.Pending ?? false) == false))
                    select new AttendedInfo
                    {
                        PeopleId = p.PeopleId,
                        AttendFlag = attend == null ? (bool?)false : attend.AttendanceFlag,
                        noRecord = attend == null,
                        isMember = p.OrganizationMembers.Any(om => om.OrganizationId == meeting.OrganizationId)
                    };
            return q;
        }

        private static IQueryable<Attend> ApplySort(IQueryable<Attend> q, string sort)
        {
            switch (sort)
            {
                case "MemberType":
                    q = q.OrderBy(a => a.MemberTypeId);
                    break;
                case "AttendType":
                    q = q.OrderBy(a => a.AttendanceTypeId);
                    break;
                case "Name":
                    q = q.OrderBy(a => a.Person.LastName).ThenBy(a => a.Person.FirstName);
                    break;
                case "MemberType DESC":
                    q = q.OrderByDescending(a => a.MemberTypeId);
                    break;
                case "AttendType DESC":
                    q = q.OrderByDescending(a => a.AttendanceTypeId);
                    break;
                case "Name DESC":
                    q = q.OrderByDescending(a => a.Person.LastName).ThenByDescending(a => a.Person.FirstName);
                    break;
                case "Organization":
                    q = q.OrderBy(a => a.Meeting.OrganizationId).ThenBy(a => a.MeetingDate);
                    break;
                case "MeetingDate":
                    q = q.OrderBy(a => a.MeetingDate);
                    break;
                case "Organization DESC":
                    q = q.OrderByDescending(a => a.Meeting.OrganizationId).ThenBy(a => a.MeetingDate);
                    break;
                case "MeetingDate DESC":
                default:
                    q = q.OrderByDescending(a => a.MeetingDate);
                    break;
            }
            return q;
        }

        public int HistoryCount(int pid, string sortExpression, int maximumRows, int startRowIndex)
        {
            return _count;
        }

        public static int GetAttendanceTypeId(int MemberTypeId)
        {
            return CodeValueController.MemberTypeCodes2().Single(mt => mt.Id == MemberTypeId).AttendanceTypeId;
        }

        public IEnumerable<AttendInfo> AttendHistory(int pid, string sortExpression, int maximumRows, int startRowIndex)
        {
            var q = from a in DbUtil.Db.Attends
                    where a.PeopleId == pid
                    where !(a.Meeting.Organization.SecurityTypeId == 3 && Util.OrgMembersOnly)
                    where a.AttendanceFlag == true || a.AttendanceFlag == null
                    select a;
            _count = q.Count();
            q = ApplySort(q, sortExpression);
            q = q.Skip(startRowIndex).Take(maximumRows);
            var q2 = from a in q
                     let o = a.Meeting.Organization
                     select new AttendInfo
                     {
                         MeetingId = a.MeetingId,
                         OrganizationId = a.Meeting.OrganizationId,
                         OrganizationName = Organization
                            .FormatOrgName(o.OrganizationName, o.LeaderName,
                                o.Location),
                         AttendType = AttendCodes.ItemValue(a.AttendanceTypeId),
                         MeetingName = o.DivOrgs.First(d => d.Division.Program.Name != DbUtil.MiscTagsString).Division.Name + ": " + o.OrganizationName,
                         MeetingDate = a.MeetingDate,
                         MemberType = MemberCodes.ItemValue(a.MemberTypeId),
                     };
            return q2;
        }

        public string RecordAttendance(int PeopleId, int MeetingId, bool attended)
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
            foreach(var m in OtherMeetings)
            {
                DbUtil.Db.UpdateAttendStr(m.OrganizationId, PeopleId);
                DbUtil.Db.UpdateMeetingCounters(m.MeetingId);
            }
            return null; // no error
        }
        private Attend CreateOtherAttend(Meeting meeting, OrganizationMember member)
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
        private int? GetAttendType(bool attended, int? memberTypeId, Meeting meeting)
        {
            if (meeting != null && meeting.GroupMeetingFlag == true)
                return (int)Attend.AttendTypeCode.Group;
            if (!attended)
                return null;
            var q = from m in CodeValueController.MemberTypeCodes2()
                    where m.Id == memberTypeId
                    select m;
            return q.Single().AttendanceTypeId;
        }
    }
}
