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
using System.Diagnostics;
using System.Web;

namespace CMSPresenter
{
    public class MeetingController
    {
        public class MeetingInfo2
        {
            public string Program { get; set; }
            public string Division { get; set; }
            public int OrganizationId { get; set; }
            public int? MeetingId { get; set; }
            public string Organization { get; set; }
            public string Tracking { get; set; }
            public DateTime? Time { get; set; }
            public int? Attended { get; set; }
            public string Leader { get; set; }
        }
        private CMSDataContext Db;
        public MeetingController()
        {
            Db = DbUtil.Db;
            Db.CommandTimeout = 900;
        }

        //private List<CodeValueItem> codes = (new CodeValueController()).MeetingStatusCodes();
        private int _count;

        public IEnumerable<MeetingInfo0> Meetings(int orgid, string sortExpression, int maximumRows, int startRowIndex)
        {
            var q = from m in Db.Meetings
                    where m.OrganizationId == orgid
                    select m;
            _count = q.Count();
            q = ApplySort(q, sortExpression);
            var q2 = q.Select(m =>
                   new MeetingInfo0
                   {
                       MeetingId = m.MeetingId,
                       MeetingDate = m.MeetingDate,
                       NumVisitors = m.NumNewVisit + m.NumRepeatVst + m.NumVstMembers,
                       NumPresent = m.NumPresent,
                       Location = m.Location,
                       OrganizationId = m.OrganizationId,
                       GroupMeeting = m.GroupMeetingFlag,
                   });
            q2 = q2.Skip(startRowIndex).Take(maximumRows);
            return q2;
        }
        public IEnumerable<MeetingInfo2> MeetingsForDate(DateTime MeetingDate, string Name, int ProgId, int DivId, int SchedId)
        {
            var name = HttpContext.Current.Server.UrlDecode(Name);
            var q = Db.Organizations.Select(o => o);
            q = OrganizationSearchController.ApplySearch(q, name, ProgId, DivId, SchedId, (int)Organization.OrgStatusCode.Active);
            var q2 = from o in q
                     where o.AttendTrkLevelId != 0 && o.AttendTrkLevelId != null
                     join m in Db.Meetings on o.OrganizationId equals m.OrganizationId into mr
                     from m in mr.Where(m => m.MeetingDate.Value.Date == MeetingDate).DefaultIfEmpty()
                     let div = o.DivOrgs.First(t => t.Division.Program.Name != DbUtil.MiscTagsString).Division
                     orderby div.Program.Name, div.Name, o.OrganizationName
                     select new MeetingInfo2
                     {
                         Program = div.Program.Name,
                         Division = div.Name,
                         OrganizationId = o.OrganizationId,
                         Organization = o.OrganizationName,
                         MeetingId = m.MeetingId,
                         Tracking = o.AttendTrackLevel.Description,
                         Time = m.MeetingDate,
                         Attended = m.NumPresent,
                         Leader = o.LeaderName
                     };
            return q2;
        }
        public IEnumerable<MeetingInfo> Meeting(int mtgid)
        {
            var q = Db.Meetings.Where(m => m.MeetingId == mtgid);
            _count = q.Count();
            var q2 = q.Select(m =>
                   new MeetingInfo
                   {
                       MeetingId = m.MeetingId,
                       MeetingDate = m.MeetingDate,
                       NumVisitors = m.NumNewVisit + m.NumRepeatVst + m.NumVstMembers,
                       NumPresent = m.NumPresent,
                       Location = m.Location,
                       OrganizationId = m.OrganizationId,
                       OrganizationName = m.Organization.OrganizationName,
                       NewVisitors = m.NumNewVisit,
                       RepeatVisitors = m.NumRepeatVst,
                       VisitingMembers = m.NumVstMembers,
                       LeaderName = m.Organization.LeaderName,
                       GroupMeeting = m.GroupMeetingFlag,
                   });
            return q2;
        }

        public IEnumerable<PastAttendeeInfo> Attendees(int mtgid)
        {
            return Attendees(mtgid, "Default");
        }

        public IEnumerable<PastAttendeeInfo> Attendees(int mtgid, string sort)
        {
            // get list of people who have attended an event for this organization (including visitors)
            var pids = from a in Db.Attends
                       where a.AttendanceFlag == true
                       where a.MeetingId == mtgid
                       select new { a.PeopleId, a.Meeting.Organization.OrganizationId, status = a.AttendType.Description };

            var q1 = from p in Db.People
                     join pid in pids on p.PeopleId equals pid.PeopleId
                     let attendct = Db.Attends
                                     .Count(a => a.Meeting.OrganizationId == pid.OrganizationId && a.PeopleId == p.PeopleId && a.AttendanceFlag == true)
                     let lastattend = Db.Attends
                                     .Where(a => a.Meeting.OrganizationId == pid.OrganizationId && a.PeopleId == p.PeopleId && a.AttendanceFlag == true)
                                     .Max(a => a.MeetingDate)
                     let status = pid.status
                     let attendpct = Db.OrganizationMembers
                                     .Where(ap => ap.PeopleId == p.PeopleId && ap.OrganizationId == pid.OrganizationId)
                                     .Max(ap => ap.AttendPct)
                     let attendstr = Db.OrganizationMembers
                                     .Where(astr => astr.PeopleId == p.PeopleId && astr.OrganizationId == pid.OrganizationId)
                                     .Max(astr => astr.AttendStr)
                     select new
                     {
                         p.PeopleId,
                         p.LastName,
                         p.NickName,
                         p.FirstName,
                         p.PrimaryAddress,
                         p.BirthYear,
                         p.BirthMonth,
                         p.BirthDay,
                         p.EmailAddress,
                         p.HomePhone,
                         p.PrimaryCity,
                         p.PrimaryState,
                         p.PrimaryZip,
                         status,
                         attendct,
                         lastattend,
                         attendpct,
                         attendstr
                     };
            switch (sort)
            {
                case "Alpha":
                    q1 = q1.OrderBy(a => a.LastName).ThenBy(b => b.FirstName);
                    break;
                case "VisitorsFirst":
                    q1 = q1.OrderByDescending(a => a.status == @"Visitor" ? 0 : 1).ThenBy(b => b.LastName).ThenBy(c => c.NickName != null ? c.NickName : c.FirstName);
                    break;
                default:
                    q1 = q1.OrderByDescending(a => a.status).ThenByDescending(b => b.lastattend);
                    break;
            }

            var q = from p in q1
                    select new PastAttendeeInfo
                    {
                        PeopleId = p.PeopleId,
                        LastName = p.LastName,
                        FirstName = p.NickName != null ? p.NickName : p.FirstName,
                        Street = p.PrimaryAddress,
                        Birthday = UtilityExtensions.Util.FormatBirthday(p.BirthYear.Value, p.BirthMonth.Value, p.BirthDay.Value),
                        EmailHome = p.EmailAddress,
                        Phone = p.HomePhone.FmtFone(),
                        City = p.PrimaryCity,
                        State = p.PrimaryState,
                        Zip = p.PrimaryZip.FmtZip(),
                        Status = p.status,
                        AttendCt = p.attendct,
                        LastAttend = p.lastattend,
                        AttendPct = p.attendpct,
                        AttendStr = p.attendstr.FmtAttendStr()
                    };


            return q;
        }

        public IEnumerable<PastAttendeeInfo> AbsenteesAndVisitors(int mtgid)
        {
            var visitors = Attendees(mtgid).Where(a => a.Status == "New Visitor" || a.Status == "Recent Visitor" || a.Status == "Visiting Member");
            var attendees = Attendees(mtgid).Where(a => a.Status == "Member" || a.Status == "Leader" || a.Status == "In-Service");

            var absentees = from m in Db.Meetings
                            from om in m.Organization.OrganizationMembers
                            join p in Db.People on om.PeopleId equals p.PeopleId
                            let attendct = Db.Attends
                                            .Count(a => a.OrganizationId == om.OrganizationId && a.PeopleId == p.PeopleId && a.AttendanceFlag == true)
                            let lastattend = Db.Attends
                                            .Where(a => a.OrganizationId == om.OrganizationId && a.PeopleId == p.PeopleId && a.AttendanceFlag == true)
                                            .Max(a => a.MeetingDate)
                            let status = "Absent"
                            let attendpct = Db.OrganizationMembers
                                            .Where(ap => ap.PeopleId == p.PeopleId && ap.OrganizationId == om.OrganizationId)
                                            .Max(ap => ap.AttendPct)
                            let attendstr = Db.OrganizationMembers
                                            .Where(astr => astr.PeopleId == p.PeopleId && astr.OrganizationId == om.OrganizationId)
                                            .Max(astr => astr.AttendStr)
                            where m.MeetingId == mtgid
                            where !(from a in attendees
                                    select a.PeopleId).Contains(om.PeopleId)
                            select new PastAttendeeInfo
                            {
                                PeopleId = p.PeopleId,
                                LastName = p.LastName,
                                FirstName = p.NickName != null ? p.NickName : p.FirstName,
                                Street = p.PrimaryAddress,
                                Birthday = UtilityExtensions.Util.FormatBirthday(p.BirthYear.Value, p.BirthMonth.Value, p.BirthDay.Value),
                                EmailHome = p.EmailAddress,
                                Phone = p.HomePhone.FmtFone(),
                                City = p.PrimaryCity,
                                State = p.PrimaryState,
                                Zip = p.PrimaryZip.FmtZip(),
                                Status = status,
                                AttendCt = attendct,
                                LastAttend = lastattend,
                                AttendPct = attendpct,
                                AttendStr = attendstr.FmtAttendStr()
                            };

            var q = from v in visitors.Concat(absentees)
                    select v;

            return q;

        }

        public int MeetingCount(int orgid, string sortExpression, int maximumRows, int startRowIndex)
        {
            return _count;
        }
        private static IQueryable<Meeting> ApplySort(IQueryable<Meeting> q, string sort)
        {
            switch (sort)
            {
                case "MeetingDate":
                    q = q.OrderBy(m => m.MeetingDate);
                    break;
                case "NumPresent":
                    q = q.OrderBy(m => m.NumPresent);
                    break;
                case "NumVisitors":
                    q = q.OrderBy(m => m.NumNewVisit + m.NumRepeatVst + m.NumVstMembers);
                    break;
                case "Location":
                    q = q.OrderBy(m => m.Location);
                    break;
                case "NumPresent DESC":
                    q = q.OrderByDescending(m => m.NumPresent);
                    break;
                case "NumVisitors DESC":
                    q = q.OrderByDescending(m => m.NumNewVisit + m.NumRepeatVst + m.NumVstMembers);
                    break;
                case "Location DESC":
                    q = q.OrderByDescending(m => m.Location);
                    break;
                case "MeetingDate DESC":
                    q = q.OrderByDescending(m => m.MeetingDate);
                    break;
            }
            return q;
        }

        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public void DeleteMeeting(int MeetingId)
        {
            var meeting = Db.Meetings.SingleOrDefault(m => m.MeetingId == MeetingId);
            if (meeting == null)
                return;
            var q = from a in Db.Attends
                    where a.MeetingId == MeetingId
                    select a.PeopleId;
            var list = q.ToList();

            var attendees = Db.Attends.Where(a => a.MeetingId == MeetingId);
            var attendcontrol = new AttendController();
            foreach (var a in attendees)
                if (a.AttendanceFlag == true)
                    attendcontrol.RecordAttendance(a.PeopleId, MeetingId, false);
            Db.Attends.DeleteAllOnSubmit(attendees);
            Db.Meetings.DeleteOnSubmit(meeting);
            Db.SubmitChanges();
        }
    }
}
