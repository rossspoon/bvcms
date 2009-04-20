/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Linq;
using System.Text;
using CmsData;
using CmsData.View;
using CMSPresenter.InfoClasses;
using UtilityExtensions;
using System.Data.Linq.SqlClient;

namespace CMSPresenter
{
    [DataObject]
    public class ChurchAttendanceSummaryController : ChurchAttendanceConstants
    {
        public class AttendanceSummaryInfo
        {
            public string Name { get; set; }
            public string Link { get; set; }
            public int Count { get; set; }
            public int Sort { get; set; }
            public bool UseLink { get { return Link.HasValue(); } }
            public bool NoLink { get { return !Link.HasValue(); } }
        }
        private class MeetInfo
        {
            public int OrganizationId { get; set; }
            public DateTime MeetingDate { get; set; }
            public int NumPresent { get; set; }
            public int NumVisitors { get; set; }
            public int ProgramId { get; set; }
        }
        private List<MeetInfo> qlist;
        private void LoadMeetings(DateTime sunday)
        {
            if (qlist != null)
                return;
            var q = from m in Db.Meetings
                    where m.MeetingDate.Value.Date == sunday
                    where m.NumPresent > 0
                    select new MeetInfo
                    {
                        OrganizationId = m.OrganizationId,
                        MeetingDate = m.MeetingDate.Value,
                        NumPresent = m.NumPresent,
                        NumVisitors = m.NumNewVisit + m.NumRepeatVst,
                        ProgramId = m.Organization.DivOrgs.First(t => t.Division.Program.Name != DbUtil.MiscTagsString).Division.ProgId.Value
                    };
            qlist = q.ToList();
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<AttendanceSummaryInfo> AttendanceSummary(DateTime sunday)
        {
            LoadMeetings(sunday);

            var q1 = from m in qlist
                     where MorningWorship.Contains(m.OrganizationId) || ExtendedSessions.Contains(m.OrganizationId)
                     group m.NumPresent by true into g
                     select new AttendanceSummaryInfo
                     {
                         Count = g.Sum(),
                         Name = "Worship Attendance",
                         Link = "~/Reports/ChurchAttendanceRpt.aspx?date=" + sunday.ToShortDateString(),
                         Sort = 1
                     };

            var q2 = from m in qlist
                     where m.ProgramId == BFCProgramTagId
                     group m.NumPresent by true into g
                     select new AttendanceSummaryInfo
                     {
                         Count = g.Sum(),
                         Name = "Bible Fellowship Attendance",
                         Link = "~/Reports/BFCWeeklyAttendanceSummaryRpt.aspx?date=" + sunday.ToShortDateString(),
                         Sort = 2
                     };

            var q3 = from m in qlist
                     where MorningWorship.Contains(m.OrganizationId) || ExtendedSessions.Contains(m.OrganizationId)
                     group m.NumVisitors by true into g
                     select new AttendanceSummaryInfo
                     {
                         Count = g.Sum(),
                         Name = "Guest Attendance in Worship",
                         Link = "",
                         Sort = 3
                     };

            var q4 = from m in qlist
                     where guestsOutOfTown.Contains(m.OrganizationId)
                     group m.NumPresent by true into g
                     select new AttendanceSummaryInfo
                     {
                         Count = g.Sum(),
                         Name = "Out of Town Guests",
                         Link = "",
                         Sort = 4
                     };

            var q5 = from m in qlist
                     where m.ProgramId == BFCProgramTagId
                     group m.NumVisitors by true into g
                     select new AttendanceSummaryInfo
                     {
                         Count = g.Count(),
                         Name = "Guest Attendance in Bible Fellowship",
                         Link = "",
                         Sort = 5
                     };

            var q6 = from c in Db.Contactees
                     where c.contact.ContactDate.Date > sunday.AddDays(-6) && c.contact.ContactDate.Date < sunday.AddDays(1) && c.contact.ContactMade.Value
                     group c by true into g
                     select new AttendanceSummaryInfo
                     {
                         Count = g.Count(),
                         Name = "Contacts",
                         Link = "",
                         Sort = 6
                     };

            var q7 = from a in Db.Attends
                     where a.AttendanceFlag == true
                     where a.MeetingDate.Date > sunday.AddDays(-6) && a.MeetingDate.Date < sunday.AddDays(1)
                     where MorningWorship.Contains(a.OrganizationId) || ExtendedSessions.Contains(a.OrganizationId)
                     where a.Person.ResCodeId == 10
                     where a.Person.MemberAnyChurch != true
                        || a.Person.PleaseVisit
                        || a.Person.ChristAsSavior
                        || a.Person.InterestedInJoining
                        || a.Person.InfoBecomeAChristian
                     group a by true into g
                     select new AttendanceSummaryInfo
                     {
                         Count = g.Count(),
                         Name = "Prospects",
                         Link = "",
                         Sort = 7
                     };

            /*          var q7 = from x in
                                     (from p in Db.AttendTypes.Take(1)
                                      select new { Name = "Guest Enrollments", p.Id })
                                 group x by x.Name into y
                                 select new AttendanceSummaryInfo { StatCount = 0, StatName = y.Key, SortId = 6, StatLink = "~/Reports/ChurchAttendanceSummaryRpt.aspx" };
                        var q8 = from x in
                                     (from p in Db.AttendTypes.Take(1)
                                      select new { Name = "Guest Attendance Special Events", p.Id })
                                 group x by x.Name into y
                                 select new AttendanceSummaryInfo { StatCount = 0, StatName = y.Key, SortId = 8, StatLink = "~/Reports/ChurchAttendanceSummaryRpt.aspx" };
            */
            if (q1.Count() == 0)
                return q1;
            return q1.Union(q2).Union(q3).Union(q4).Union(q5).Union(q6).Union(q7).OrderBy(a => a.Sort);
        }
        public class GuestCentralInfo
        {
            public int? MetroCount { get; set; }
            public int? OutsideCount { get; set; }
            public string HourLabel { get; set; }
            public int? Total { get { return MetroCount + OutsideCount; } }
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<GuestCentralInfo> GuestCentral(DateTime sunday)
        {
            var q = from m in qlist
                    where GuestCentralMetroOrgs.Contains(m.OrganizationId) || GuestCentralOutsideOrgs.Contains(m.OrganizationId)
                    group m by m.MeetingDate.TimeOfDay into g
                    select new GuestCentralInfo
                    {
                        HourLabel = g.Key.Hours.ToString().PadLeft(2, ' ') + ":" + (g.Key.Minutes % 60).ToString().PadLeft(2, '0') + " AM",
                        MetroCount =
                           (from mm in g
                            where GuestCentralMetroOrgs.Contains(mm.OrganizationId)
                            select mm.NumPresent).Sum(),
                        OutsideCount =
                           (from mm in g
                            where GuestCentralOutsideOrgs.Contains(mm.OrganizationId)
                            select mm.NumPresent).Sum(),
                    };
            var list = q.ToList();
            var t = new GuestCentralInfo
            {
                HourLabel = "Total",
                MetroCount = list.Sum(i => i.MetroCount),
                OutsideCount = list.Sum(i => i.OutsideCount),
            };

            if (list.Count > 0)
                list.Add(t);

            return q;
        }

        public class InterestPointInfo
        {
            public string Interest { get; set; }
            public int? ProspectCount { get; set; }
            public int? GuestCount { get; set; }
            public decimal? Pct { get; set; }
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<InterestPointInfo> GuestInterestPoints(DateTime sunday)
        {
            var q = from a in Db.Attends
                    where MorningWorship.Contains(a.OrganizationId)
                    where a.AttendanceFlag == true
                    where a.MeetingDate.Date == sunday.Date
                    select new
                    {
                        InterestPoint = a.Person.InterestPointId == null ? "Unspecified" : a.Person.InterestPoint.Description,
                        a.Person.PleaseVisit,
                        a.Person.ChristAsSavior,
                        a.Person.InterestedInJoining,
                        a.Person.InfoBecomeAChristian,
                        MemberAnyChurch = a.Person.MemberAnyChurch ?? false
                    };
            var qlist = q.ToList();

            var q2 = from a in qlist
                     group a by a.InterestPoint into g
                     select new InterestPointInfo
                     {
                         Interest = g.Key,
                         GuestCount = g.Count(),
                         ProspectCount = g.Count(p => !p.MemberAnyChurch
                             || p.PleaseVisit
                             || p.ChristAsSavior
                             || p.InterestedInJoining
                             || p.InfoBecomeAChristian)
                     };
            var list = q2.ToList();

            var t = new InterestPointInfo
            {
                Interest = "Total",
                GuestCount = q2.Sum(i => i.GuestCount),
                ProspectCount = q2.Sum(i => i.ProspectCount),
                Pct = 100
            };

            if (list.Count > 0)
            {
                foreach (var i in list)
                    if (t.ProspectCount > 0)
                        i.Pct = (decimal)i.ProspectCount / t.ProspectCount * 100;
                list.Add(t);
            }
            return list;
        }

    }
}
