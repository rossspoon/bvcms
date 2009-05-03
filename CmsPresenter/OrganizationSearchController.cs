/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CmsData;
using CmsData.View;
using System.ComponentModel;
using System.Collections;
using UtilityExtensions;

namespace CMSPresenter
{
    [DataObject]
    public class OrganizationSearchController
    {
        private CMSDataContext Db;

        public OrganizationSearchController()
        {
            Db = new CMSDataContext(Util.ConnectionString);
        }
        public int count;
        private Dictionary<int, CodeValueItem> dict = (new CodeValueController()).AttendanceTrackLevelCodes().ToDictionary(cv => cv.Id);
        private int[] ChildSecurityRollSheets = new int[] { 4, 5, 6, 7 };

        public IEnumerable<OrganizationInfo> FetchOrganizationList(IQueryable<Organization> query, string tagstr)
        {
            var tagid = TagSubDiv(tagstr);
            var q = from o in query
                    select new OrganizationInfo
                    {
                        OrganizationId = o.OrganizationId,
                        OrganizationStatus = o.OrganizationStatusId,
                        OrganizationName = o.OrganizationName,
                        LeaderName = o.LeaderName,
                        LeaderId = o.LeaderId,
                        MemberCount = o.OrganizationMembers.Count(),
                        AttendanceTrackingLevel = dict[o.AttendTrkLevelId].Value,
                        DivisionId = o.DivOrgs.FirstOrDefault().Division.Id,
                        DivisionName = o.DivOrgs.FirstOrDefault().Division.Name,
                        FirstMeetingDate = o.FirstMeetingDate.FormatDate(),
                        LastMeetingDate = o.LastMeetingDate.FormatDate(),
                        Schedule = o.WeeklySchedule.Description,
                        Location = o.Location,
                        HasTag = o.DivOrgs.Any(ot => ot.DivId == tagid),
                        ChildRollSheet = o.RollSheetTypeId > 2,
                    };
            return q;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<OrganizationInfo> FetchOrganizationList(int startRowIndex,
            int maximumRows,
            string sortExpression,
            string NameSearch,
            int OrgSubDivId,
            int scheduleid,
            int statusid,
            string tagid)
        {
            var query = from o in Db.Organizations select o;
            query = ApplySearch(query, NameSearch, 0, OrgSubDivId, scheduleid, statusid);
            count = query.Count();
            query = ApplySort(query, sortExpression).Skip(startRowIndex).Take(maximumRows);
            return FetchOrganizationList(query, tagid);
        }

        public int Count(int startRowIndex, int maximumRows, string sortExpression, string NameSearch, int OrgSubDivId, int scheduleid, int statusid, string tagid)
        {
            return count;
        }

        public IEnumerable<OrganizationInfo> FetchOrganizationExcelList(string Name, int DivId, int SchedId, int StatusId, DateTime MeetingDate)
        {
            var q = from o in Db.Organizations select o;
            q = ApplySearch(q, Name, 0, DivId, SchedId, StatusId);
            var q2 = from o in q
                     let LookbackDt = MeetingDate.AddDays(-7 * o.RollSheetVisitorWks ?? 3)
                     select new OrganizationInfo
                     {
                         OrganizationId = o.OrganizationId,
                         OrganizationStatus = o.OrganizationStatusId,
                         OrganizationName = o.OrganizationName,
                         LeaderName = o.LeaderName,
                         LeaderId = o.LeaderId,
                         MemberCount = o.OrganizationMembers.Count(),
                         AttendanceTrackingLevel = dict[o.AttendTrkLevelId].Value,
                         DivisionName = o.DivOrgs.First(d => d.Division.Program.Name != DbUtil.MiscTagsString).Division.Name,
                         FirstMeetingDate = o.FirstMeetingDate.FormatDate(),
                         LastMeetingDate = o.LastMeetingDate.FormatDate(),
                         Schedule = o.WeeklySchedule.Description,
                         Location = o.Location,
                         VisitorCount = (from p in Db.People
                            where p.Attends.Any(a => 
                                a.AttendanceFlag == true
                                && !p.OrganizationMembers.Any(om => om.OrganizationId == o.OrganizationId)
                                && AttendController.VisitAttendTypes.Contains(a.AttendanceTypeId.Value)
                                && a.OrganizationId == o.OrganizationId
                                && a.MeetingDate <= MeetingDate
                                && a.MeetingDate >= LookbackDt)
                                select p).Count(),
                         ChildRollSheet = o.RollSheetTypeId > 2,
                     };
            return q2;
        }
        public IEnumerable<OrganizationInfo> FetchOrganizationExcelList(string Name, int DivId, int SchedId, int StatusId)
        {
            return FetchOrganizationExcelList(Name, DivId, SchedId, StatusId, Util.Now.Date);
        }

            #region Search and Sort
        public static IQueryable<Organization> ApplySearch(IQueryable<Organization> query, string NameSearch, int ProgId, int DivId, int scheduleid, int statusid)
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
                        where o.ScheduleId == scheduleid
                        select o;

            if (statusid > 0)
                query = from o in query
                        where o.OrganizationStatusId == statusid
                        select o;

            return query;
        }

        public static IQueryable<Organization> ApplySort(IQueryable<Organization> query, string sort)
        {
            switch (sort)
            {
                case "ID":
                    query = from o in query
                            orderby o.OrganizationId
                            select o;
                    break;
                case "Division":
                    query = from o in query
                            orderby o.DivOrgs.FirstOrDefault(d => d.Division.Program.Name != DbUtil.MiscTagsString).Division.Name,
                            o.OrganizationName
                            select o;
                    break;
                case "Name":
                    query = from o in query
                            orderby o.OrganizationName
                            select o;
                    break;
                case "LeaderName":
                    query = from o in query
                            orderby o.LeaderName,
                            o.OrganizationName
                            select o;
                    break;
                case "MemberCount":
                    query = from o in query
                            orderby o.OrganizationMembers.Count(),
                            o.OrganizationName
                            select o;
                    break;
                case "FirstMeetingDate":
                    query = from o in query
                            orderby o.FirstMeetingDate,
                            o.LastMeetingDate
                            select o;
                    break;
                case "LastMeetingDate":
                    query = from o in query
                            orderby o.LastMeetingDate,
                            o.FirstMeetingDate
                            select o;
                    break;
                case "ID DESC":
                    query = from o in query
                            orderby o.OrganizationId descending
                            select o;
                    break;
                case "Division DESC":
                    query = from o in query
                            orderby o.DivOrgs.FirstOrDefault(d => d.Division.Program.Name != DbUtil.MiscTagsString).Division.Name descending,
                            o.OrganizationName descending
                            select o;
                    break;
                case "Name DESC":
                    query = from o in query
                            orderby o.OrganizationName descending
                            select o;
                    break;
                case "LeaderName DESC":
                    query = from o in query
                            orderby o.LeaderName descending,
                            o.OrganizationName descending
                            select o;
                    break;
                case "MemberCount DESC":
                    query = from o in query
                            orderby o.OrganizationMembers.Count() descending,
                            o.OrganizationName descending
                            select o;
                    break;
                case "FirstMeetingDate DESC ":
                    query = from o in query
                            orderby o.FirstMeetingDate descending,
                            o.LastMeetingDate descending
                            select o;
                    break;
                case "LastMeetingDate DESC":
                    query = from o in query
                            orderby o.LastMeetingDate descending,
                            o.FirstMeetingDate descending
                            select o;
                    break;
            }
            return query;
        }
        public static int TagSubDiv(string s)
        {
            if (!s.HasValue())
                return 0;
            var a = s.Split(':');
            if (a.Length > 1)
                return a[1].ToInt();
            return 0;
        }

        #endregion

    }
}
