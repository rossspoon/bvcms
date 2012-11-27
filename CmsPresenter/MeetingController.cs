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
using CmsData.Codes;

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
            public DateTime? Time { get; set; }
            public int? Attended { get; set; }
            public string Leader { get; set; }
            public string Description { get; set; }
        }

        private int _count;
        public IEnumerable<MeetingInfo0> Meetings(int orgid, string sortExpression, int maximumRows, int startRowIndex, bool future)
        {
            DateTime midnight = Util.Now.Date.AddDays(1);
            var q = from m in DbUtil.Db.Meetings
                    where m.OrganizationId == orgid
                    select m;
            if (future)
                q = q.Where(m => m.MeetingDate >= midnight);
            else
                q = q.Where(m => m.MeetingDate < midnight);
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
                       Description = m.Description,
                   });
            q2 = q2.Skip(startRowIndex).Take(maximumRows);
            return q2;
        }
        public int MeetingCount(int orgid, string sortExpression, int maximumRows, int startRowIndex, bool future)
        {
            return _count;
        }

        private int meetingsfordatecount;
        public int MeetingsForDateCount(DateTime MeetingDate, string Name, int ProgId, int DivId, int SchedId, int CampusId, string SortOn)
        {
            return meetingsfordatecount;
        }
        public IQueryable<Organization> ApplySearch(IQueryable<Organization> query, string NameSearch, int ProgId, int DivId, int scheduleid, int statusid, int campusid)
        {
            if (NameSearch.HasValue())
            {
                if (NameSearch.AllDigits())
                    query = from o in query
                            where o.OrganizationId == NameSearch.ToInt()
                            select o;
                else
                    query = from o in query
                            where o.OrganizationName.Contains(NameSearch)
                                || o.LeaderName.Contains(NameSearch)
                                || o.Location.Contains(NameSearch)
                                || o.DivOrgs.Any(t => t.Division.Name.Contains(NameSearch))
                            select o;
            }
            if (DivId > 0)
                query = from o in query
                        where o.DivOrgs.Any(t => t.DivId == DivId)
                        select o;
            else if (ProgId > 0)
                query = from o in query
                        where o.DivOrgs.Any(t => t.Division.ProgId == ProgId)
                        select o;

            if (scheduleid > 0)
                query = from o in query
                        where o.OrgSchedules.Any(os => os.ScheduleId == scheduleid)
                        select o;

            if (statusid > 0)
                query = from o in query
                        where o.OrganizationStatusId == statusid
                        select o;

            if (campusid > 0)
                query = from o in query
                        where o.CampusId == campusid
                        select o;

            return query;
        }
        public IEnumerable<MeetingInfo2> MeetingsForDate(DateTime MeetingDate, string Name, int StatusId, int ProgId, int DivId, int SchedId, int CampusId, string SortOn)
        {
            var name = HttpContext.Current.Server.UrlDecode(Name);
            var q = DbUtil.Db.Organizations.Select(o => o);
            q = ApplySearch(q, name, ProgId, DivId, SchedId, StatusId, CampusId);
            var q2 = from o in q
                     //where o.AttendTrkLevelId != 0 && o.AttendTrkLevelId != null
                     join m in DbUtil.Db.Meetings on o.OrganizationId equals m.OrganizationId into mr
                     from m in mr.Where(m => m.MeetingDate.Value.Date == MeetingDate).DefaultIfEmpty()
                     let div = o.Division
                     orderby div.Program.Name, div.Name, o.OrganizationName
                     select new MeetingInfo2
                     {
                         Program = div.Program.Name,
                         Division = div.Name,
                         OrganizationId = o.OrganizationId,
                         Organization = o.OrganizationName,
                         MeetingId = m.MeetingId,
                         Time = m.MeetingDate,
                         Attended = m.NumPresent,
                         Leader = o.LeaderName,
                         Description = m.Description
                     };
            meetingsfordatecount = q2.Count();
            switch (SortOn)
            { 
                case "Attended":
                    q2 = q2.OrderBy(m => m.Attended);
                    break;
                case "Leader":
                    q2 = q2.OrderBy(m => m.Leader);
                    break;
                case "Time":
                    q2 = q2.OrderBy(m => m.Time);
                    break;
                case "Division":
                    q2 = q2.OrderBy(m => m.Division);
                    break;
                case "Organization":
                    q2 = q2.OrderBy(m => m.Organization);
                    break;
                case "Attended DESC":
                    q2 = q2.OrderByDescending(m => m.Attended);
                    break;
                case "Leader DESC":
                    q2 = q2.OrderByDescending(m => m.Leader);
                    break;
                case "Time DESC":
                    q2 = q2.OrderByDescending(m => m.Time);
                    break;
                case "Division DESC":
                    q2 = q2.OrderByDescending(m => m.Division);
                    break;
                case "Organization DESC":
                    q2 = q2.OrderByDescending(m => m.Organization);
                    break;
            }
            var list = q2.ToList();
            list.Add(new MeetingInfo2 
            { 
                Attended = list.Sum(m => m.Attended),
                Division = "Total" 
            });
            return list;
        }
        public IEnumerable<MeetingInfo> Meeting(int mtgid)
        {
            var q = DbUtil.Db.Meetings.Where(m => m.MeetingId == mtgid);
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
                       Description = m.Description
                   });
            return q2;
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
    }
}
