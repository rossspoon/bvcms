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

namespace CMSPresenter
{
    [DataObject]
    public class AttendController
    {
        private CMSDataContext Db;
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
            Db = DbUtil.Db;
            var c = new CodeValueController();
            AttendCodes = c.AttendanceTypeCodes();
            MemberCodes = CodeValueController.MemberTypeCodes2();
        }

        private int _count;

        public IEnumerable<AttendInfo> Attendees(int meetid, bool inEditMode)
        {
            var meeting = Db.Meetings.Single(m => m.MeetingId == meetid); // this will come out of cache, because Db has already read it

            var wks = 3; // default lookback
            if (meeting.Organization.RollSheetVisitorWks.HasValue)
                wks = meeting.Organization.RollSheetVisitorWks.Value;

            var dt = meeting.MeetingDate.Value.AddDays(wks * -7);

            var q = from p in Db.People
                    let membership = p.OrganizationMembers.SingleOrDefault(om => om.OrganizationId == meeting.OrganizationId)
                    let ismember = p.OrganizationMembers.Any(om => om.OrganizationId == meeting.OrganizationId)
                    let attend = p.Attends.SingleOrDefault(a => a.OrganizationId == meeting.OrganizationId && a.MeetingId == meetid)
                    where
                        // anybody who attended this meeting
                        p.Attends.Any(a => a.MeetingId == meetid && a.AttendanceFlag == true)
                    || ( // visitors who attended in recent weeks
                        inEditMode &&
                        p.Attends.Any(a =>
                            a.AttendanceFlag != false
                            && (a.MeetingDate >= dt && a.MeetingDate <= meeting.MeetingDate)
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
                        AttendType = Db.AttendDesc(attend == null ?
                            (ismember ?
                                (int)Attend.AttendTypeCode.Member :
                                (int)Attend.AttendTypeCode.RecentVisitor) :
                            attend.AttendanceTypeId),
                        MeetingName = "Test",
                        MeetingDate = meeting.MeetingDate,
                        MemberType = Db.MemberDesc(attend == null ?
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
            var meeting = Db.Meetings.Single(m => m.MeetingId == meetid); // this will come out of cache, because Db has already read it

            var wks = 3; // default lookback
            if (meeting.Organization.RollSheetVisitorWks.HasValue)
                wks = meeting.Organization.RollSheetVisitorWks.Value;

            var dt = meeting.MeetingDate.Value.AddDays(wks * -7);

            var q = from p in Db.People
                    let attend = p.Attends.SingleOrDefault(a => a.OrganizationId == meeting.OrganizationId && a.MeetingId == meetid)
                    where
                        // anybody who attended this meeting
                        p.Attends.Any(a => a.MeetingId == meetid && a.AttendanceFlag == true)
                    || (// visitors who attended in recent weeks
                        p.Attends.Any(a =>
                            a.AttendanceFlag != false
                            && (a.MeetingDate >= dt && a.MeetingDate <= meeting.MeetingDate)
                            && a.OrganizationId == meeting.OrganizationId
                            && VisitAttendTypes.Contains(a.AttendanceTypeId.Value)
                            )
                        )
                    || // members
                        (p.OrganizationMembers.Any(om => om.OrganizationId == meeting.OrganizationId))
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
            var q = from a in Db.Attends
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

            var r = Db.AttendMeetingInfo(MeetingId, PeopleId);

            var o = r.GetResult<CMSDataContext.AttendMeetingInfo2>().First();
            var Attendance = r.GetResult<Attend>().FirstOrDefault();
            var Meeting = r.GetResult<Meeting>().First();
            var VIPAttendance = r.GetResult<Attend>().ToList();
            var BFCMember = r.GetResult<OrganizationMember>().FirstOrDefault();
            var BFCAttendance = r.GetResult<Attend>().FirstOrDefault();
            var BFCMeeting = r.GetResult<Meeting>().FirstOrDefault();

            if (o.AttendedElsewhere > 0 && attended)
                return "{0}({1}) already attended elsewhere".Fmt(o.Name, PeopleId);

            if (Attendance == null)
            {
                Attendance = new Attend
                {
                    OrganizationId = Meeting.OrganizationId,
                    PeopleId = PeopleId,
                    MeetingDate = Meeting.MeetingDate.Value,
                    CreatedDate = DateTime.Now,
                    CreatedBy = Db.CurrentUser.UserId,
                };
                Meeting.Attends.Add(Attendance);
            }
            Attendance.AttendanceFlag = attended;

            if (Meeting.GroupMeetingFlag && o.MemberTypeId.HasValue)
            {
                Attendance.MemberTypeId = o.MemberTypeId.Value;
                if (VIPAttendance.Count > 0 && VIPAttendance.Any(a => a.AttendanceFlag == true))
                    Attendance.AttendanceTypeId = (int)Attend.AttendTypeCode.Volunteer;
                else
                    Attendance.AttendanceTypeId = (int)Attend.AttendTypeCode.Group;
            }
            else if (o.IsOffSite == true && attended == false)
            {
                Attendance.OtherAttends = 1;
                Attendance.AttendanceTypeId = (int)Attend.AttendTypeCode.Offsite;
                Attendance.MemberTypeId = o.MemberTypeId.Value;
            }
            else if (o.MemberTypeId.HasValue) // member of this class
            {
                Attendance.MemberTypeId = o.MemberTypeId.Value;
                Attendance.AttendanceTypeId = GetAttendType(Attendance.AttendanceFlag, Attendance.MemberTypeId, null);
                Attendance.BFCAttendance = Attendance.OrganizationId == o.BFClassId;

                if (BFCMember != null && (attended || BFCAttendance != null)) // related BFC
                {
                    /* At this point I am recording attendance for a vip class 
                     * or for a class where I am doing InService (long term) teaching
                     * And now I am looking at the BFClass where I am a regular member or an InService Member
                     * I don't need to be here if I am reversing my attendance and there is no BFCAttendance to fix
                     */
                    if (BFCAttendance == null)
                        BFCAttendance = CreateOtherAttend(Meeting, BFCMember);

                    BFCAttendance.OtherAttends = attended ? 1 : 0;
                    Attendance.OtherAttends = BFCAttendance.AttendanceFlag ? 1 : 0;

                    if (o.MemberTypeId == (int)OrganizationMember.MemberTypeCode.VIP)
                    {
                        if (BFCAttendance.OtherAttends > 0)
                            BFCAttendance.AttendanceTypeId = (int)Attend.AttendTypeCode.Volunteer;
                        else
                            BFCAttendance.AttendanceTypeId = GetAttendType(BFCAttendance.AttendanceFlag, BFCMember.MemberTypeId, BFCMeeting);
                    }
                    else if (BFCMember.MemberTypeId == (int)OrganizationMember.MemberTypeCode.InServiceMember)
                    {
                        if (BFCAttendance.OtherAttends > 0)
                            BFCAttendance.AttendanceTypeId = (int)Attend.AttendTypeCode.InService;
                        else
                            BFCAttendance.AttendanceTypeId = (int)Attend.AttendTypeCode.Member;
                    }
                    OtherMeetings.Add(BFCAttendance);
                }
                else if (VIPAttendance.Count > 0) // need to indicate BFCAttendance or not
                {
                    /* At this point I am recording attendance for a BFClass
                     * And now I am looking at the one or more VIP classes where I a sometimes volunteer
                     */
                    foreach (var a in VIPAttendance)
                    {
                        a.OtherAttends = attended ? 1 : 0;
                        if (a.AttendanceFlag == true)
                            Attendance.AttendanceTypeId = (int)Attend.AttendTypeCode.Volunteer;
                        OtherMeetings.Add(a);
                    }
                    Attendance.OtherAttends = VIPAttendance.Any(a => a.AttendanceFlag == true) ? 1 : 0;
                }
            }
            else // not a member of this class 
            {
                if (BFCMember != null) // member of another class (visiting member)
                {
                    if (attended)
                    {
                        Attendance.MemberTypeEnum = Attend.MemberTypeCode.VisitingMember;
                        Attendance.AttendTypeEnum = Attend.AttendTypeCode.VisitingMember;
                    }
                    if (BFCAttendance == null)
                        BFCAttendance = CreateOtherAttend(Meeting, BFCMember);

                    BFCAttendance.OtherAttends = attended ? 1 : 0;

                    if (BFCAttendance.OtherAttends > 0)
                        BFCAttendance.AttendanceTypeId = (int)Attend.AttendTypeCode.OtherClass;
                    else
                        BFCAttendance.AttendanceTypeId = GetAttendType(BFCAttendance.AttendanceFlag, BFCMember.MemberTypeId, BFCMeeting);

                    OtherMeetings.Add(BFCAttendance);
                }
                else // not a member of another class (visitor)
                {
                    Attendance.MemberTypeEnum = Attend.MemberTypeCode.Visitor;
                    if (o.IsRecentVisitor.Value)
                        Attendance.AttendTypeEnum = Attend.AttendTypeCode.RecentVisitor;
                    else
                        Attendance.AttendTypeEnum = Attend.AttendTypeCode.NewVisitor;
                }
            }
            try
            {
                Db.SubmitChanges();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error recording attendance pid={0},dt={1}".Fmt(PeopleId, Meeting.MeetingDate), ex);
            }

            Db.UpdateAttendStr(Meeting.OrganizationId, PeopleId);
            foreach(var m in OtherMeetings)
            {
                Db.UpdateAttendStr(m.OrganizationId, PeopleId);
                Db.UpdateMeetingCounters(m.MeetingId);
            }
            return null; // no error
        }
        private Attend CreateOtherAttend(Meeting meeting, OrganizationMember member)
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
                    CreatedDate = DateTime.Now,
                    CreatedBy = DbUtil.Db.CurrentUser.UserId,
                    GroupMeetingFlag = false,
                    Location = member.Organization.Location,
                };
                Db.Meetings.InsertOnSubmit(othMeeting);
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
                    CreatedDate = DateTime.Now,
                    CreatedBy = Db.CurrentUser.UserId,
                    Meeting = othMeeting,
                };
                Db.Attends.InsertOnSubmit(othAttend);
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
