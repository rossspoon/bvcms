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
                            && VisitAttendTypes.Contains(a.AttendanceTypeId)
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
                                (int)OrganizationMember.MemberTypeCode.Member :
                                (int)OrganizationMember.MemberTypeCode.Visitor) :
                            attend.MemberTypeId),
                        Name = p.Name2,
                        PeopleId = p.PeopleId,
                        _AttendFlag = attend == null ? null : attend.AttendanceFlag,
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
                            && VisitAttendTypes.Contains(a.AttendanceTypeId)
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

        public static bool IsRecentVisitor(CmsData.Meeting theMeeting, int peopleId)
        {
            var wks = 3; // default lookback
            if (theMeeting.Organization.RollSheetVisitorWks.HasValue)
                wks = theMeeting.Organization.RollSheetVisitorWks.Value;

            var dt = theMeeting.MeetingDate.Value.AddDays(wks * -7);

            var matchingPerson = DbUtil.Db.People.SingleOrDefault(
                        p => p.PeopleId == peopleId
                    &&
                        p.Attends.Any(a =>
                            a.AttendanceFlag != false
                            && a.MeetingDate >= dt && a.MeetingDate <= theMeeting.MeetingDate
                            && a.Meeting.OrganizationId == theMeeting.OrganizationId
                            && VisitAttendTypes.Contains(a.AttendanceTypeId)
                            )
                    && // not members
                        !(p.OrganizationMembers.Any(om => om.OrganizationId == theMeeting.OrganizationId))
                    );

            return (matchingPerson != null);
        }

        public string RecordAttendance(int PeopleId, int MeetingId, bool attended)
        {
			Meeting OtherMeeting = null;

            var r = Db.AttendMeetingInfo(MeetingId, PeopleId);

            var o = r.GetResult<CMSDataContext.AttendMeetingInfo2>().First();
            var Attendance = r.GetResult<Attend>().FirstOrDefault();
            var Meeting = r.GetResult<Meeting>().First();
            var BFCMember = r.GetResult<OrganizationMember>().FirstOrDefault();
            var InSvcMember = r.GetResult<OrganizationMember>().FirstOrDefault();

            bool haveExcuse = o.IsRegularHour.Value && o.IsSameHour.Value 
                && (InSvcMember != null 
                        || (BFCMember != null 
                            && o.MemberTypeId == (int)OrganizationMember.MemberTypeCode.VIP));

            if (o.AttendedElsewhere.Value && attended && !haveExcuse)
                return "{0}({1}) already attended elsewhere".Fmt(o.Name, PeopleId);

            if (Attendance == null)
            {
                Attendance = new Attend
                {
                    OrganizationId = Meeting.OrganizationId,
                    MeetingId = MeetingId,
                    PeopleId = PeopleId,
                    MeetingDate = Meeting.MeetingDate.Value,
                    CreatedDate = DateTime.Now,
                    CreatedBy = Db.CurrentUser.UserId,
                };
                Db.Attends.InsertOnSubmit(Attendance);
            }
            Attendance.AttendanceFlag = attended;

            if (o.IsOffSite.Value && attended == false)
            {
                Attendance.AttendanceFlag = null;
                Attendance.AttendanceTypeId = (int)Attend.AttendTypeCode.Offsite;
                Attendance.MemberTypeId = o.MemberTypeId.Value;
            }
            else if (o.MemberTypeId.HasValue) // member of this class
            {
                Attendance.MemberTypeId = o.MemberTypeId.Value;
                Attendance.AttendanceTypeId = CodeValueController.MemberTypeCodes2()
                    .Single(mt => mt.Id == Attendance.MemberTypeId).AttendanceTypeId;

                if (o.IsRegularHour.Value && InSvcMember != null && o.IsSameHour.Value) // rarely true
                    OtherMeeting = RecordAttendanceInOtherClass(Db, Meeting, InSvcMember, attended,
                            (int)Attend.AttendTypeCode.InService);
                else if (o.IsRegularHour.Value && o.MemberTypeId == (int)OrganizationMember.MemberTypeCode.VIP
                        && BFCMember != null && o.IsSameHour.Value)
                    OtherMeeting = RecordAttendanceInOtherClass(Db, Meeting, BFCMember, attended,
                                (int)Attend.AttendTypeCode.Volunteer);
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
                    if (o.IsRegularHour.Value && o.IsSameHour.Value)
                        OtherMeeting = RecordAttendanceInOtherClass(Db, Meeting, BFCMember, attended,
                                (int)Attend.AttendTypeCode.OtherClass);
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
            if (OtherMeeting != null)
			{
				Db.UpdateAttendStr(OtherMeeting.OrganizationId, PeopleId);
				Db.UpdateMeetingCounters(OtherMeeting.MeetingId);
			}
            return null; // no error
        }
        private static Meeting RecordAttendanceInOtherClass(CMSDataContext Db,
            CmsData.Meeting meeting,
            OrganizationMember om,
            bool attended,
            int attendType)
        {
            if (attended)
            {
                var oa = CreateOtherAttendance(Db, meeting, om);
                oa.MemberTypeId = om.MemberTypeId;
                oa.AttendanceTypeId = attendType;
				return oa.Meeting;
            }
            return RemoveOtherAttendance(Db, meeting, om);
        }
        private static Meeting RemoveOtherAttendance(CMSDataContext Db, CmsData.Meeting meeting, OrganizationMember member)
        {
            var oMeeting = Db.Meetings.SingleOrDefault(m =>
                m.MeetingDate == meeting.MeetingDate
                && m.OrganizationId == member.OrganizationId);
            if (oMeeting == null)
                return null;
            var oa = oMeeting.Attends.SingleOrDefault(a => a.PeopleId == member.PeopleId);
            if (oa != null)
			{
				Db.Attends.DeleteOnSubmit(oa);
				return oMeeting;
			}
			return null;
        }
        private static Attend CreateOtherAttendance(CMSDataContext Db, CmsData.Meeting meeting, OrganizationMember member)
        {
            var oMeeting = Db.Meetings.SingleOrDefault(m =>
                m.MeetingDate == meeting.MeetingDate
                && m.OrganizationId == member.OrganizationId);
            if (oMeeting == null)
            {
                oMeeting = new CmsData.Meeting
                {
                    OrganizationId = member.OrganizationId,
                    MeetingDate = meeting.MeetingDate,
                    CreatedDate = DateTime.Now,
                    CreatedBy = DbUtil.Db.CurrentUser.UserId,
                    GroupMeetingFlag = false,
                    Location = member.Organization.Location,
                };
                Db.Meetings.InsertOnSubmit(oMeeting);
            }
            var oAttend = Db.Attends.SingleOrDefault(a => a.PeopleId == member.PeopleId
                && a.MeetingId == oMeeting.MeetingId);
            if (oAttend == null)
            {
                oAttend = new Attend
                {
                    PeopleId = member.PeopleId,
                    OrganizationId = member.OrganizationId,
                    MeetingDate = meeting.MeetingDate.Value,
                    CreatedDate = DateTime.Now,
                    CreatedBy = Db.CurrentUser.UserId,
                };
                oMeeting.Attends.Add(oAttend);
            }
            oAttend.AttendanceFlag = null;
            return oAttend;
        }
    }
}
