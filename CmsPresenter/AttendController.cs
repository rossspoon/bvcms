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
        public static IEnumerable<AttendedInfo> RollSheetAttendees(int meetid)
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
    }
}
