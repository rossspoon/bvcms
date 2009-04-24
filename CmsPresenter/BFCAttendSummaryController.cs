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
using System.Diagnostics;

namespace CMSPresenter
{
    public class BFCAttendSummaryController
    {
        private int MiscTags;
        TimeSpan t800, t930, t1100;
        int dbUtilBFClassOrgTagId;
        private CMSDataContext Db;
        public BFCAttendSummaryController()
        {
            Db = DbUtil.Db;
            MiscTags = Db.Programs.Single(d => d.Name == DbUtil.MiscTagsString).Id;
            t800 = TimeSpan.Parse("8:00");
            t930 = TimeSpan.Parse("9:30");
            t1100 = TimeSpan.Parse("11:00");
            dbUtilBFClassOrgTagId = DbUtil.BFClassOrgTagId;
        }
        public class BFCAttendSummaryInfo
        {
            public int Order { get; set; }
            public string Name { get; set; }
            private int? _Cnt800;
            public int? Cnt800
            {
                get { return _Cnt800; }
                set { _Cnt800 = value == 0 ? null : value; }
            }
            private int? _Cnt930;
            public int? Cnt930
            {
                get { return _Cnt930; }
                set { _Cnt930 = value == 0 ? null : value; }
            }
            private int? _Cnt1100;
            public int? Cnt1100
            {
                get { return _Cnt1100; }
                set { _Cnt1100 = value == 0 ? null : value; }
            }
            public int? Total { get; set; }
            public string Class { get; set; }
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<BFCAttendSummaryInfo> BFCWeeklyAttendanceSummary(DateTime sunday)
        {
            var q2 = from m in Db.Meetings
                     let div = m.Organization.DivOrgs.First(t => t.Division.ProgId != MiscTags).Division
                     where div.ProgId == dbUtilBFClassOrgTagId
                     where m.NumPresent > 0
                     where m.MeetingDate.Value.Date == sunday.Date
                     group m by new { div.BFCSummaryOrgTags.First().SortOrder, m.MeetingDate } into g
                     select new
                     {
                         g.Key.SortOrder,
                         DateHour = g.Key.MeetingDate.Value,
                         Count = g.Sum(m => m.NumPresent),
                     };
            var qlist = q2.ToList();

            var qSortOrderName = from i in Db.BFCSummaryOrgTags
                                 select new { i.SortOrder, i.Tag.Name };
            var namelist = qSortOrderName.ToDictionary(i => i.SortOrder);

            // division rows
            var qRows = from m in qlist
                        group m by new { m.SortOrder } into g
                        select new BFCAttendSummaryInfo
                        {
                            Order = g.Key.SortOrder,
                            Name = namelist[g.Key.SortOrder].Name,
                            Class = "",
                            Cnt800 = qlist.Where(m => m.DateHour.TimeOfDay == t800 && m.SortOrder == g.Key.SortOrder).Sum(m => m.Count),
                            Cnt930 = qlist.Where(m => m.DateHour.TimeOfDay == t930 && m.SortOrder == g.Key.SortOrder).Sum(m => m.Count),
                            Cnt1100 = qlist.Where(m => m.DateHour.TimeOfDay == t1100 && m.SortOrder == g.Key.SortOrder).Sum(m => m.Count),
                            Total = qlist.Where(m => m.SortOrder == g.Key.SortOrder).Sum(m => m.Count),
                        };
            // total row
            var qTotal = from m in qlist
                         group m by true into g
                         select new BFCAttendSummaryInfo
                         {
                             Order = 99999, // total row goes last
                             Name = "Total",
                             Class = "totalrow",
                             Cnt800 = g.Where(m => m.DateHour.TimeOfDay == t800).Sum(m => m.Count),
                             Cnt930 = g.Where(m => m.DateHour.TimeOfDay == t930).Sum(m => m.Count),
                             Cnt1100 = g.Where(m => m.DateHour.TimeOfDay == t1100).Sum(m => m.Count),
                             Total = g.Sum(m => m.Count)
                         };
            // all rows sorted
            return qRows.Union(qTotal).OrderBy(m => m.Order);
        }
        public class BFCAvgAttendSummaryInfo
        {
            public int Order { get; set; }
            public string Name { get; set; }
            public double? Avg800 { get; set; }
            public double? Avg930 { get; set; }
            public double? Avg1100 { get; set; }
            public double? Total { get; set; }
            public string Class { get; set; }
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<BFCAvgAttendSummaryInfo> BFCAvgWeeklyAttendanceSummary(DateTime fromDate, DateTime toDate)
        {
            // all the matching meeting division/date/hour counts for the date range
            var q2 = from m in Db.Meetings
                     let div = m.Organization.DivOrgs.First(t => t.Division.ProgId != MiscTags).Division
                     where div.ProgId == dbUtilBFClassOrgTagId
                     where m.NumPresent > 0
                     where m.MeetingDate >= fromDate && m.MeetingDate < toDate.AddDays(1)
                     group m by new { div.BFCSummaryOrgTags.First().SortOrder, m.MeetingDate } into g
                     select new
                     {
                         g.Key.SortOrder,
                         DateHour = g.Key.MeetingDate.Value,
                         Count = g.Sum(m => m.NumPresent),
                     };
            var qlist = q2.ToList();

            // By Division, Time, Date (detail rows)
            var qDivTimeDate = from tm in qlist
                               group tm by new { tm.SortOrder, tm.DateHour.TimeOfDay, tm.DateHour.Date } into g
                               select new
                               {
                                   g.Key.SortOrder,
                                   g.Key.TimeOfDay,
                                   g.Key.Date,
                                   Count = g.Sum(tm => tm.Count),
                               };

            // By Division, Date (rightmost column)
            var qDivDate = from tm in qlist
                           group tm by new { tm.SortOrder, tm.DateHour.Date } into g
                           select new
                           {
                               g.Key.SortOrder,
                               Count = g.Sum(tm => tm.Count),
                           };

            // By Hour, Date (bottom row)
            var qHourDate = from tm in qlist
                            group tm by new { tm.DateHour.TimeOfDay, tm.DateHour.Date } into g
                            select new
                            {
                                TimeofDay = g.Key.TimeOfDay,
                                Count = g.Sum(tm => tm.Count),
                            };

            // By Date (bottom right corner)
            var qDate = from tm in qlist
                        group tm by tm.DateHour.Date into g
                        select g.Sum(m => m.Count);

            var qSortOrderName = from i in Db.BFCSummaryOrgTags
                                 select new { i.SortOrder, i.Tag.Name };
            var namelist = qSortOrderName.ToDictionary(i => i.SortOrder);

            // division rows
            var qRows = from m in qDivTimeDate
                        group m by new { m.SortOrder } into g
                        select new BFCAvgAttendSummaryInfo
                        {
                            Order = g.Key.SortOrder,
                            Name = namelist[g.Key.SortOrder].Name,
                            Class = "",
                            Avg800 = g.Where(m => m.TimeOfDay == t800 && m.SortOrder == g.Key.SortOrder).Average(m => (double?)m.Count),
                            Avg930 = g.Where(m => m.TimeOfDay == t930 && m.SortOrder == g.Key.SortOrder).Average(m => (double?)m.Count),
                            Avg1100 = g.Where(m => m.TimeOfDay == t1100 && m.SortOrder == g.Key.SortOrder).Average(m => (double?)m.Count),
                            Total = qDivDate.Where(m => m.SortOrder == g.Key.SortOrder).Average(m => (double?)m.Count),
                        };
            // total row
            var qTotal = from m in qHourDate
                         group m by true into g
                         select new BFCAvgAttendSummaryInfo
                         {
                             Order = 99999, // total row goes last
                             Name = "Total",
                             Class = "totalrow",
                             Avg800 = g.Where(m => m.TimeofDay == t800).Average(m => (double?)m.Count),
                             Avg930 = g.Where(m => m.TimeofDay == t930).Average(m => (double?)m.Count),
                             Avg1100 = g.Where(m => m.TimeofDay == t1100).Average(m => (double?)m.Count),
                             Total = (double?)qDate.Average()
                         };
            // all rows sorted
            return qRows.Union(qTotal).OrderBy(m => m.Order);
        }
    }
}
