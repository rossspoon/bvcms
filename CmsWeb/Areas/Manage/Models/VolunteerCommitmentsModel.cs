using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using CmsData;
using UtilityExtensions;

namespace CmsWeb.Models
{
    public class VolunteerCommitmentsModel
    {
        public class NameId
        {
            public string Name { get; set; }
            public int PeopleId { get; set; }
        }
        public class CellInfo
        {
            public string Sunday { get; set; }
            public string DayHour { get; set; }
            public List<NameId> Persons { get; set; }
            public int MeetingId { get; set; }
        }
        private class Attendance
        {
            public DateTime Sunday { get; set; }
            public TimeSpan TimeOfWeek { get; set; }
            public DateTime MeetingDate { get; set; }
            public int MeetingId { get; set; }
            public int PeopleId { get; set; }
            public string Name { get; set; }
        }
        private IEnumerable<Attendance> Attends;
        public IEnumerable<string> times;
        public IEnumerable<string> weeks;
        public IEnumerable<CellInfo> details;
        public string OrgName { get; set; }

        public VolunteerCommitmentsModel(int id)
        {
            OrgName = (from o in DbUtil.Db.Organizations where o.OrganizationId == id select o.OrganizationName).Single();
            var q = from a in DbUtil.Db.Attends
                    where a.MeetingDate > Util.Now.Date
                    where a.OrganizationId == id
                    orderby a.MeetingDate
                    select a;
            var list = q.ToList();
            if (list.Count == 0)
                return;
            var sunday = list.First().MeetingDate.Date;
            sunday = sunday.AddDays(-(int)sunday.DayOfWeek);
            Attends = from a in list
                     let Day = (int)a.MeetingDate.DayOfWeek
                     let Sunday = a.MeetingDate.Date.AddDays(-Day)
                     orderby a.MeetingDate
                     select new Attendance
                     {
                         MeetingId = a.MeetingId,
                         Sunday = Sunday,
                         TimeOfWeek = a.MeetingDate.Subtract(Sunday),
                         MeetingDate = a.MeetingDate,
                         PeopleId = a.PeopleId,
                         Name = a.Person.Name,
                     };
            const string DayHourFmt = "ddd hh:mm tt";
            details = from i in Attends
                          group new NameId 
                          { 
                              PeopleId = i.PeopleId, 
                              Name = i.Name 
                          } by new 
                          { 
                              i.Sunday, 
                              i.TimeOfWeek,
                              i.MeetingId
                          } into g
                          select new CellInfo
                          {
                              Sunday = g.Key.Sunday.ToShortDateString(),
                              DayHour = g.Key.Sunday.Add(g.Key.TimeOfWeek).ToString(DayHourFmt),
                              MeetingId = g.Key.MeetingId,
                              Persons = g.ToList()
                          };
            times = from i in Attends
                        let ts = i.MeetingDate.Subtract(i.Sunday)
                        group i by ts into g
                        orderby g.Key
                        select sunday.Add(g.Key).ToString(DayHourFmt);
            weeks = from i in Attends
                        group i by i.Sunday into g
                        orderby g.Key
                        select g.Key.ToShortDateString();
        }
    }
}
