/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UtilityExtensions;
using System.Web.Mvc;
using System.Web.Routing;
using System.Text.RegularExpressions;
using System.Threading;
using System.Data.Linq;
using CmsData;

namespace CMSWeb.Models
{
    public class OrgSearchModel : PagerModel2
    {
        public string Name { get; set; }
        public int? ProgramId { get; set; }
        public int? DivisionId { get; set; }
        public int? TagProgramId { get; set; }
        public int? TagDiv { get; set; }
        public int? ScheduleId { get; set; }
        public int? CampusId { get; set; }
        public int? StatusId { get; set; }
        public string tagstr { get; set; }

        public OrgSearchModel()
        {
            StatusId = (int)CmsData.Organization.OrgStatusCode.Active;
            GetCount = Count;
        }

        public IEnumerable<OrganizationInfo> OrganizationList()
        {
            var query = ApplySearch();
            if (!_count.HasValue)
                _count = query.Count();
            query = ApplySort(query).Skip(StartRow).Take(PageSize);
            return OrganizationList(query);
        }
        public IEnumerable<OrganizationInfo> OrganizationList(IQueryable<CmsData.Organization> query)
        {
            var q = from o in query
                    select new OrganizationInfo
                    {
                        Id = o.OrganizationId,
                        OrganizationStatus = o.OrganizationStatusId,
                        OrganizationName = o.OrganizationName,
                        LeaderName = o.LeaderName,
                        LeaderId = o.LeaderId,
                        MemberCount = o.MemberCount,
                        AttendanceTrackingLevel = o.AttendTrackLevel.Description,
                        DivisionId = o.Division.Id,
                        DivisionName = o.Division.Name,
                        FirstMeetingDate = o.FirstMeetingDate.FormatDate(),
                        LastMeetingDate = o.LastMeetingDate.FormatDate(),
                        MeetingTime = o.MeetingTime,
                        Location = o.Location,
                        AllowSelfCheckIn = o.CanSelfCheckin ?? false,
                        BDayStart = o.BirthDayStart.FormatDate(),
                        BDayEnd = o.BirthDayEnd.FormatDate(),
                        Tag = TagDiv == null ? "" : o.DivOrgs.Any(ot => ot.DivId == TagDiv) ? "Remove" : "Add",
                    };
            return q;
        }
        public IEnumerable<OrganizationInfoExcel> OrganizationExcelList(DateTime MeetingDate)
        {
            var q = from o in DbUtil.Db.Organizations select o;
            q = ApplySearch();
            var q2 = from o in q
                     let LookbackDt = MeetingDate.AddDays(-7 * o.RollSheetVisitorWks ?? 3)
                     select new OrganizationInfoExcel
                     {
                         OrgId = o.OrganizationId,
                         Status = o.OrganizationStatus.Description,
                         Name = o.OrganizationName,
                         Leader = o.LeaderName,
                         Members = o.MemberCount ?? 0,
                         Tracking = o.AttendTrackLevel.Description,
                         Division = o.DivOrgs.First(d => d.Division.Program.Name != DbUtil.MiscTagsString).Division.Name,
                         FirstMeeting = o.FirstMeetingDate.FormatDate(),
                         MeetingTime = o.MeetingTime,
                         Location = o.Location,
                     };
            return q2;
        }
        public IEnumerable<OrganizationInfoExcel> OrganizationExcelList()
        {
            return OrganizationExcelList(Util.Now.Date);
        }

        private int TagSubDiv(string s)
        {
            if (!s.HasValue())
                return 0;
            var a = s.Split(':');
            if (a.Length > 1)
                return a[1].ToInt();
            return 0;
        }

        private int? _count;
        public int Count()
        {
            if (!_count.HasValue)
                _count = ApplySearch().Count();
            return _count.Value;
        }
        public IQueryable<CmsData.Organization> ApplySearch()
        {
            var query = DbUtil.Db.Organizations.AsQueryable();
            if (Name.HasValue())
            {
                if (Name.AllDigits())
                    query = from o in query
                            where o.OrganizationId == Name.ToInt()
                            select o;
                else
                    query = from o in query
                            where o.OrganizationName.Contains(Name)
                                || o.LeaderName.Contains(Name)
                                || o.Location.Contains(Name)
                                || o.DivOrgs.Any(t => t.Division.Name.Contains(Name))
                            select o;
            }
            if (DivisionId > 0)
                query = from o in query
                        where o.DivOrgs.Any(t => t.DivId == DivisionId)
                        select o;
            else if (ProgramId > 0)
                query = from o in query
                        where o.DivOrgs.Any(t => t.Division.ProgId == ProgramId)
                        select o;

            if (ScheduleId > 0)
                query = from o in query
                        where o.ScheduleId == ScheduleId
                        select o;

            if (StatusId > 0)
                query = from o in query
                        where o.OrganizationStatusId == StatusId
                        select o;

            if (CampusId > 0)
                query = from o in query
                        where o.CampusId == CampusId
                        select o;

            return query;
        }
        public IQueryable<CmsData.Organization> ApplySort(IQueryable<CmsData.Organization> query)
        {
            if (Direction == "asc")
                switch (Sort)
                {
                    case "ID":
                        query = from o in query
                                orderby o.OrganizationId
                                select o;
                        break;
                    case "Division":
                        query = from o in query
                                orderby o.Division.Name,
                                o.OrganizationName
                                select o;
                        break;
                    case "Name":
                        query = from o in query
                                orderby o.OrganizationName
                                select o;
                        break;
                    case "Location":
                        query = from o in query
                                orderby o.Location
                                select o;
                        break;
                    case "Schedule":
                        query = from o in query
                                orderby o.ScheduleId
                                select o;
                        break;
                    case "Self CheckIn":
                        query = from o in query
                                orderby (o.CanSelfCheckin ?? false)
                                select o;
                        break;
                    case "Leader":
                        query = from o in query
                                orderby o.LeaderName,
                                o.OrganizationName
                                select o;
                        break;
                    case "Members":
                        query = from o in query
                                orderby o.MemberCount,
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
                }
            else
                switch (Sort)
                {
                    case "ID":
                        query = from o in query
                                orderby o.OrganizationId descending
                                select o;
                        break;
                    case "Division":
                        query = from o in query
                                orderby o.Division.Name descending,
                                o.OrganizationName descending
                                select o;
                        break;
                    case "Name":
                        query = from o in query
                                orderby o.OrganizationName descending
                                select o;
                        break;
                    case "Location":
                        query = from o in query
                                orderby o.Location descending
                                select o;
                        break;
                    case "Schedule":
                        query = from o in query
                                orderby o.ScheduleId descending
                                select o;
                        break;
                    case "Self CheckIn":
                        query = from o in query
                                orderby (o.CanSelfCheckin ?? false) descending
                                select o;
                        break;
                    case "Leader":
                        query = from o in query
                                orderby o.LeaderName descending,
                                o.OrganizationName descending
                                select o;
                        break;
                    case "Members":
                        query = from o in query
                                orderby o.MemberCount descending,
                                o.OrganizationName descending
                                select o;
                        break;
                    case "FirstMeetingDate":
                        query = from o in query
                                orderby o.FirstMeetingDate descending,
                                o.LastMeetingDate descending
                                select o;
                        break;
                    case "LastMeetingDate":
                        query = from o in query
                                orderby o.LastMeetingDate descending,
                                o.FirstMeetingDate descending
                                select o;
                        break;
                }
            return query;
        }

        public IEnumerable<SelectListItem> StatusIds()
        {
            var q = from s in DbUtil.Db.OrganizationStatuses
                    select new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Description
                    };
            var list = q.ToList();
            list.Insert(0, new SelectListItem { Value = "0", Text = "(not specified)" });
            return list;
        }
        public IEnumerable<SelectListItem> CampusIds()
        {
            var q = from c in DbUtil.Db.Campus
                    select new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Description
                    };
            var list = q.ToList();
            list.Insert(0, new SelectListItem
            {
                Value = "0",
                Text = "(not specified)"
            });
            return list;
        }
        public IEnumerable<SelectListItem> ProgramIds()
        {
            var q = from c in DbUtil.Db.Programs
                    select new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    };
            var list = q.ToList();
            list.Insert(0, new SelectListItem
            {
                Value = "0",
                Text = "(not specified)",
            });
            return list;
        }
        public static IEnumerable<SelectListItem> DivisionIds(int ProgId)
        {
            var q = from d in DbUtil.Db.Divisions
                    where d.ProgId == ProgId
                    select new SelectListItem
                    {
                        Value = d.Id.ToString(),
                        Text = d.Name
                    };
            var list = q.ToList();
            list.Insert(0, new SelectListItem
            {
                Value = "0",
                Text = "(not specified)",
            });
            return list;
        }
        public IEnumerable<SelectListItem> ScheduleIds()
        {
            var q = from o in DbUtil.Db.Organizations
                    where o.ScheduleId != null
                    group o by new { o.ScheduleId, o.MeetingTime } into g
                    orderby g.Key.ScheduleId
                    select new SelectListItem
                    {
                        Value = g.Key.ScheduleId.Value.ToString(),
                        Text = "{0:dddd h:mm tt}".Fmt(g.Key.MeetingTime)
                    };
            var list = q.ToList();
            list.Insert(0, new SelectListItem
            {
                Value = "0",
                Text = "(not specified)",
            });
            return list;
        }
        public static DateTime DefaultMeetingDate(int scheduleid)
        {
            var sdt = CmsData.Organization.GetDateFromScheduleId(scheduleid);
            if (sdt == null)
                return DateTime.Now.Date.AddHours(8);
            var dt = Util.Now.Date;
            dt = dt.AddDays(-(int)dt.DayOfWeek); // prev sunday
            dt = dt.AddDays((int)sdt.Value.Day);
            if (dt < Util.Now.Date)
                dt = dt.AddDays(7);
            return dt.Add(sdt.Value.TimeOfDay);
        }

        public class OrganizationInfo
        {
            public int Id { get; set; }
            public int? OrganizationStatus { get; set; }
            public string OrganizationName { get; set; }
            public string LeaderName { get; set; }
            public int? LeaderId { get; set; }
            public int? MemberCount { get; set; }
            public string AttendanceTrackingLevel { get; set; }
            public int? DivisionId { get; set; }
            public string DivisionName { get; set; }
            public string FirstMeetingDate { get; set; }
            public string LastMeetingDate { get; set; }
            public int SchedDay { get; set; }
            public DateTime? MeetingTime { get; set; }
            public string Schedule { get { return "{0:ddd h:mm tt}".Fmt(MeetingTime); } }
            public string Location { get; set; }
            public string Tag { get; set; }
            public int? VisitorCount { get; set; }
            public bool AllowSelfCheckIn { get; set; }
            public string BDayStart { get; set; }
            public string BDayEnd { get; set; }
            public string ToolTip
            {
                get
                {
                    return "{0} ({1})|Division: {2} ({3})|Leader: {4}|Tracking: {5}|First Meeting: {6}|Last Meeting: {7}|Schedule: {8}|Location: {9}".Fmt(
                               OrganizationName,
                               Id,
                               DivisionName,
                               DivisionId,
                               LeaderName,
                               AttendanceTrackingLevel,
                               FirstMeetingDate,
                               LastMeetingDate,
                               Schedule,
                               Location
                               );
                }
            }
        }
        public class OrganizationInfoExcel
        {
            public int OrgId { get; set; }
            public string Status { get; set; }
            public string Name { get; set; }
            public string Leader { get; set; }
            public int Members { get; set; }
            public string Tracking { get; set; }
            public string Division { get; set; }
            public string FirstMeeting { get; set; }
            public DateTime? MeetingTime { get; set; }
            public string Schedule { get { return "{0:ddd h:mm tt}".Fmt(MeetingTime); } }
            public string Location { get; set; }
        }
    }
}
