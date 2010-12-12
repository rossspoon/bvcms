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
using System.Collections;

namespace CmsWeb.Models
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
        public int? OnlineReg { get; set; }

        public OrgSearchModel()
        {
            StatusId = (int)CmsData.Organization.OrgStatusCode.Active;
            GetCount = Count;
        }
        public Division Division()
        {
            var d = DbUtil.Db.Divisions.SingleOrDefault(dd => dd.Id == DivisionId);
            return d;
        }

        private IQueryable<CmsData.Organization> organizations;
        public IEnumerable<OrganizationInfo> OrganizationList()
        {
            organizations = FetchOrgs();
            if (!_count.HasValue)
                _count = organizations.Count();
            organizations = ApplySort(organizations).Skip(StartRow).Take(PageSize);
            return OrganizationList(organizations);
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
                        ProgramName = o.Division.Program.Name,
                        DivisionId = o.DivisionId,
                        DivisionName = o.Division.Name,
                        Divisions = string.Join(",", o.DivOrgs.Select(d => d.Division.Name).ToArray()),
                        FirstMeetingDate = o.FirstMeetingDate.FormatDate(),
                        LastMeetingDate = o.LastMeetingDate.FormatDate(),
                        MeetingTime = o.MeetingTime,
                        Location = o.Location,
                        AllowSelfCheckIn = o.CanSelfCheckin ?? false,
                        BDayStart = o.BirthDayStart.FormatDate("na"),
                        BDayEnd = o.BirthDayEnd.FormatDate("na"),
                        Tag = TagDiv == null ? "" : o.DivOrgs.Any(ot => ot.DivId == TagDiv) ? "Remove" : "Add",
                        ChangeMain = (o.DivisionId == null || o.DivisionId != TagDiv) && o.DivOrgs.Any(d => d.DivId == TagDiv),
                    };
            return q;
        }
        public IEnumerable OrganizationExcelList()
        {
            var q = from o in DbUtil.Db.Organizations select o;
            q = FetchOrgs();
            var q2 = from o in q
                     select new
                     {
                         OrgId = o.OrganizationId,
                         Name = o.OrganizationName,
                         Description = o.Description,
                         Leader = o.LeaderName,
                         Members = o.MemberCount ?? 0,
                         Tracking = o.AttendTrackLevel.Description,
                         Division = o.DivOrgs.First(d => d.Division.Program.Name != DbUtil.MiscTagsString).Division.Name,
                         FirstMeeting = o.FirstMeetingDate.FormatDate(),
                         LastMeeting = o.LastMeetingDate.FormatDate(),
                         Schedule = "{0:dddd h:mm tt}".Fmt(o.MeetingTime),
                         o.Location,
                         RollSheetVisitorWks = o.RollSheetVisitorWks ?? 0,
                         o.AgeFee,
                         o.AgeGroups,
                         Limit = o.Limit.ToString(),
                         CanSelfCheckin = o.CanSelfCheckin ?? false,
                         AllowKioskRegister = o.AllowKioskRegister ?? false,
                         AllowLastYearShirt = o.AllowLastYearShirt ?? false,
                         AskAllergies = o.AskAllergies ?? false,
                         AskChurch = o.AskChurch ?? false,
                         AskCoaching = o.AskCoaching ?? false,
                         AskDoctor = o.AskDoctor ?? false,
                         AskEmContact = o.AskEmContact ?? false,
                         AskGrade = o.AskGrade ?? false,
                         AskInsurance = o.AskInsurance ?? false,
                         o.AskOptions,
                         AskParents = o.AskParents ?? false,
                         AskRequest = o.AskRequest ?? false,
                         AskShirtSize = o.AskShirtSize ?? false,
                         AskTickets = o.AskTickets ?? false,
                         AskTylenolEtc = o.AskTylenolEtc ?? false,
                         BirthDayStart = o.BirthDayStart.FormatDate2(),
                         BirthDayEnd = o.BirthDayEnd.FormatDate2(),
                         Deposit = o.Deposit ?? 0,
                         Fee = o.Fee ?? 0,
                         GenderId = o.GenderId ?? 0,
                         GradeAgeStart = o.GradeAgeStart ?? 0,
                         GradeAgeEnd = o.GradeAgeEnd ?? 0,
                         o.EmailAddresses,
                         LastDayBeforeExtra = o.LastDayBeforeExtra.FormatDate2(),
                         MaximumFee = o.MaximumFee ?? 0,
                         MemberOnly = o.MemberOnly ?? false,
                         NoSecurityLabel = o.NoSecurityLabel ?? false,
                         NumCheckInLabels = o.NumCheckInLabels ?? 0,
                         o.NumItemsLabel,
                         NumWorkerCheckInLabels = o.NumWorkerCheckInLabels ?? 0,
                         o.PhoneNumber,
                         RegistrationTypeId = o.RegistrationTypeId ?? 0,
                         ShirtFee = o.ShirtFee ?? 0,
                         o.YesNoQuestions,
                         IsBibleFellowshipOrg = o.IsBibleFellowshipOrg ?? false,
                         NoReqAddr = o.NotReqAddr,
                         NoReqDOB = o.NotReqDOB,
                         NoReqGender = o.NotReqGender,
                         NoReqMarital = o.NotReqMarital,
                         NoReqPhone = o.NotReqPhone,
                         NoReqZip = o.NotReqZip,
                     };
            return q2;
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
                _count = FetchOrgs().Count();
            return _count.Value;
        }
        private IQueryable<CmsData.Organization> FetchOrgs()
        {
            if (organizations != null)
                return organizations;

            organizations = DbUtil.Db.Organizations.AsQueryable();
            //if (OnlineReg)
            //{
            //    organizations = from o in organizations
            //                    where
            //}
            if (Name.HasValue())
            {
                if (Name.AllDigits())
                    organizations = from o in organizations
                                    where o.OrganizationId == Name.ToInt()
                                    select o;
                else
                    organizations = from o in organizations
                                    where o.OrganizationName.Contains(Name)
                                        || o.LeaderName.Contains(Name)
                                        || o.Location.Contains(Name)
                                        || o.DivOrgs.Any(t => t.Division.Name.Contains(Name))
                                    select o;
            }
            if (DivisionId > 0)
                organizations = from o in organizations
                                where o.DivOrgs.Any(t => t.DivId == DivisionId)
                                select o;
            else if (ProgramId > 0)
                organizations = from o in organizations
                                where o.DivOrgs.Any(d=> d.Division.ProgDivs.Any(p => p.ProgId == ProgramId))
                                || o.Division.ProgId == ProgramId
                                select o;

            if (ScheduleId > 0)
                organizations = from o in organizations
                                where o.ScheduleId == ScheduleId
                                select o;

            if (StatusId > 0)
                organizations = from o in organizations
                                where o.OrganizationStatusId == StatusId
                                select o;

            if (CampusId > 0)
                organizations = from o in organizations
                                where o.CampusId == CampusId
                                select o;

            if (this.OnlineReg == 99)
                organizations = from o in organizations
                                where o.RegistrationTypeId > 0
                                select o;
            else if (this.OnlineReg > 0)
                organizations = from o in organizations
                                where o.RegistrationTypeId == OnlineReg
                                select o;

            return organizations;
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
                    orderby c.Name
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
        public IEnumerable<SelectListItem> DivisionIds()
        {
            return DivisionIds(ProgramId ?? 0);
        }
        public static IEnumerable<SelectListItem> DivisionIds(int ProgId)
        {
            var q = from d in DbUtil.Db.Divisions
                    where d.ProgId == ProgId
                    || d.ProgDivs.Any(p => p.ProgId == ProgId)
                    orderby d.Name
                    select new SelectListItem
                    {
                        Value = d.Id.ToString(),
                        Text = d.Name
                    };
            var list = q.ToList();
            list.Insert(0, new SelectListItem
            {
                Value = "0",
                Text = ProgId == 0 ? "(select a program)" : "(not specified)",
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
        public IEnumerable<SelectListItem> RegistrationTypeIds()
        {
            var q = from o in DbUtil.Db.RegistrationTypes
                    orderby o.Id
                    select new SelectListItem
                    {
                        Value = o.Id.ToString(),
                        Text = o.Description
                    };
            var list = q.ToList();
            list.Insert(0, new SelectListItem
            {
                Value = "99",
                Text = "(any online reg)",
            });
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
            public string ProgramName { get; set; }
            public int? DivisionId { get; set; }
            public string DivisionName { get; set; }
            public string Divisions { get; set; }
            public string FirstMeetingDate { get; set; }
            public string LastMeetingDate { get; set; }
            public int SchedDay { get; set; }
            public DateTime? MeetingTime { get; set; }
            public string Schedule { get { return "{0:ddd h:mm tt}".Fmt(MeetingTime); } }
            public string Location { get; set; }
            public string Tag { get; set; }
            public bool? ChangeMain { get; set; }
            public int? VisitorCount { get; set; }
            public bool AllowSelfCheckIn { get; set; }
            public string BDayStart { get; set; }
            public string BDayEnd { get; set; }
            public string ToolTip
            {
                get
                {
                    return "{0} ({1})|Program:{2}|Division: {3} ({4})|Leader: {5}|Tracking: {6}|First Meeting: {7}|Last Meeting: {8}|Schedule: {9}|Location: {10}|Divisions: {11}".Fmt(
                               OrganizationName,
                               Id,
                               ProgramName,
                               DivisionName,
                               DivisionId,
                               LeaderName,
                               AttendanceTrackingLevel,
                               FirstMeetingDate,
                               LastMeetingDate,
                               Schedule,
                               Location,
                               Divisions
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
