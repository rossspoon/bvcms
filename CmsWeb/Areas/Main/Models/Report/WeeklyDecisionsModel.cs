using System;
using System.Collections.Generic;
using System.Linq;
using CmsData;
using UtilityExtensions;

namespace CmsWeb.Areas.Main.Models.Report
{
    public class WeeklyDecisionsModel
    {
        public DateTime Sunday { get; set; }

        public WeeklyDecisionsModel(DateTime? dt)
        {
            Sunday = dt ?? MostRecentAttendedSunday();
        }
        public static DateTime MostRecentAttendedSunday()
        {
            var q = from m in DbUtil.Db.Meetings
                    where m.MeetingDate.Value.Date.DayOfWeek == 0
                    where m.NumPresent > 0
                    where m.MeetingDate < Util.Now
                    orderby m.MeetingDate descending
                    select m.MeetingDate.Value.Date;
            var dt = q.FirstOrDefault();
            if (dt == DateTime.MinValue) //Sunday Date equal/before today
                dt = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            return dt;
        }
        public class NameCount
        {
            public int? Count { get; set; }
            public string Name { get; set; }
        }

        public NameCount TotalBaptisms;
        public NameCount TotalDecisions;
        public IEnumerable<NameCount> Baptisms()
        {
            var dt1 = Sunday.AddDays(-4);
            var dt2 = Sunday.AddDays(2);
            var q3 = from p in DbUtil.Db.People
                     where p.BaptismDate >= dt1 && p.BaptismDate <= dt2
                     group p by p.BaptismType.Description into g
                     orderby g.Key
                     select new NameCount
                     {
                         Name = g.Key,
                         Count = g.Count()
                     };
            var list = q3.ToList();
            TotalBaptisms = new NameCount
            {
                Name = "Total",
                Count = q3.Sum(i => i.Count)
            };
            return list;
        }

        public IEnumerable<NameCount> Decisions()
        {
            var dt1 = Sunday.AddDays(-4);
            var dt2 = Sunday.AddDays(2);
            var q3 = from p in DbUtil.Db.People
                     where p.DecisionDate >= dt1 && p.DecisionDate <= dt2
                     group p by p.DecisionType.Description into g
                     orderby g.Key
                     select new NameCount
                     {
                         Name = g.Key,
                         Count = g.Count()
                     };
            var list = q3.ToList();
            TotalDecisions = new NameCount
            {
                Name = "Total",
                Count = q3.Sum(i => i.Count)
            };
            return list;
        }
    }
}
