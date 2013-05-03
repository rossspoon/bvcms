using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using CmsWeb.Models;
using UtilityExtensions;

namespace CmsWeb.Areas.Main.Models.Report
{
    public class MeetingsModel
    {
        internal DateTime? _Dt1;
        public DateTime? Dt1
        {
            get
            {
                if (!_Dt1.HasValue)
                    _Dt1 = ChurchAttendanceModel.MostRecentAttendedSunday();
                return _Dt1;
            }
            set
            {
                _Dt1 = value;
            }
        }
        public DateTime? Dt2 { get; set; }
        public string Name { get; set; }
        public int? StatusId { get; set; }
        public int? ProgId { get; set; }
        public int? DivId { get; set; }
        public int? SchedId { get; set; }
        public int? CampusId { get; set; }
        public string Sort { get; set; }
        public string Dir { get; set; }
        public bool NoZero { get; set; }

        public MeetingsModel()
        {
            Dir = "asc";
            Sort = "Time";
        }

        public int MeetingsCount { get; set; }
        public int TotalAttends { get; set; }
        public int OtherAttends { get; set; }
        public int TotalPeople { get; set; }

        private IQueryable<CmsData.Organization> ApplySearch(IQueryable<CmsData.Organization> query)
        {
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
            if (DivId > 0)
                query = from o in query
                        where o.DivOrgs.Any(t => t.DivId == DivId)
                        select o;
            else if (ProgId > 0)
                query = from o in query
                        where o.DivOrgs.Any(t => t.Division.ProgId == ProgId)
                        select o;

            if (SchedId > 0)
                query = from o in query
                        where o.OrgSchedules.Any(os => os.ScheduleId == SchedId)
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
        public IEnumerable<MeetingInfo> MeetingsForDate()
        {
            var name = HttpContext.Current.Server.UrlDecode(Name);
            var q = DbUtil.Db.Organizations.Select(o => o);
            q = ApplySearch(q);
            var tdt2 = Dt2;
            if (!tdt2.HasValue)
                tdt2 = Dt1.Value.AddHours(24);
            var q2 = from o in q
                     join m in DbUtil.Db.Meetings on o.OrganizationId equals m.OrganizationId into mr
                     from m in mr.Where(m => m.MeetingDate >= Dt1 && m.MeetingDate < tdt2).DefaultIfEmpty()
                     where !(o.OrganizationStatusId == CmsData.Codes.OrgStatusCode.Inactive && (m == null || m.NumPresent == 0))
                     where (m != null && m.NumPresent > 0) || NoZero == false
                     let div = o.Division
                     select new MeetingInfo
                     {
                         Program = div.Program.Name,
                         Division = div.Name,
                         OrganizationId = o.OrganizationId,
                         Organization = o.OrganizationName,
                         MeetingId = m.MeetingId,
                         Tracking = m.GroupMeetingFlag ? "Group" : "Individual",
                         Time = m.MeetingDate,
                         Attended = m.NumPresent,
                         Leader = o.LeaderName,
                         Description = m.Description,
                         OtherAttends = m.NumOtherAttends,
                         Inactive = o.OrganizationStatusId == CmsData.Codes.OrgStatusCode.Inactive
                     };
            switch (Dir)
            {
                case "asc":
                    switch (Sort)
                    {
                        case "Attended":
                            q2 = q2.OrderBy(m => m.Attended);
                            break;
                        case "Leader":
                            q2 = q2.OrderBy(m => m.Leader);
                            break;
                        case "Time":
                            q2 = q2.OrderBy(m => m.Time).ThenBy(m => m.Division).ThenBy(m => m.Organization);
                            break;
                        case "Division":
                            q2 = q2.OrderBy(m => m.Division).ThenBy(m => m.Organization);
                            break;
                        case "Organization":
                            q2 = q2.OrderBy(m => m.Organization);
                            break;
                    }
                    break;
                case "desc":
                    switch (Sort)
                    {
                        case "Attended":
                            q2 = q2.OrderByDescending(m => m.Attended);
                            break;
                        case "Leader":
                            q2 = q2.OrderByDescending(m => m.Leader);
                            break;
                        case "Time":
                            q2 = q2.OrderByDescending(m => m.Time).ThenByDescending(m => m.Division).ThenByDescending(m => m.Organization);
                            break;
                        case "Division":
                            q2 = q2.OrderByDescending(m => m.Division).ThenByDescending(m => m.Organization);
                            break;
                        case "Organization":
                            q2 = q2.OrderByDescending(m => m.Organization);
                            break;
                    }
                    break;
            }
            var list = q2.ToList();

            MeetingsCount = list.Where(a => a.Attended > 0).Count();
            TotalAttends = list.Sum(m => m.Attended ?? 0);
            OtherAttends = list.Sum(m => m.OtherAttends ?? 0);
            var q3 = from o in q
                     join m in DbUtil.Db.Meetings on o.OrganizationId equals m.OrganizationId into mr
                     from m in mr.Where(m => m.MeetingDate >= Dt1 && m.MeetingDate < tdt2)
                     let div = o.Division
                     from a in m.Attends
                     where a.AttendanceFlag == true
                     select a.PeopleId;
            TotalPeople = q3.Distinct().Count();
            return q2;
        }
    }
    public class MeetingInfo
    {
        public string Program { get; set; }
        public string Division { get; set; }
        public int OrganizationId { get; set; }
        public int? MeetingId { get; set; }
        public string Organization { get; set; }
        public string Tracking { get; set; }
        public DateTime? Time { get; set; }
        public int? Attended { get; set; }
        public int? OtherAttends { get; set; }
        public string Leader { get; set; }
        public string Description { get; set; }
        public bool Inactive { get; set; }
    }
}