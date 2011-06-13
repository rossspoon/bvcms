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
            public string Name { get; set; }
            public int? ProgramId { get; set; }
        }
        private List<MeetInfo> qlist;
        private void LoadMeetings(DateTime sunday)
        {
            if (qlist != null)
                return;
            var q = from m in DbUtil.Db.Meetings
                    where m.MeetingDate.Value.Date == sunday
                    where m.NumPresent > 0
                    select new MeetInfo
                    {
                        OrganizationId = m.OrganizationId,
                        MeetingDate = m.MeetingDate.Value,
                        Name = m.Organization.OrganizationName,
                        NumPresent = m.NumPresent,
                        NumVisitors = m.NumNewVisit + m.NumRepeatVst,
                        ProgramId = m.Organization.Division.ProgId.Value
                    };
            qlist = q.ToList();
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
            if (qlist == null)
            {
                var sunday1200 = sunday.AddHours(12);
                LoadMeetings(sunday);
            }
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
            var q = from a in DbUtil.Db.Attends
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
