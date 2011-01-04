using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using System.Text;
using System.Collections;
using UtilityExtensions;
using System.Text.RegularExpressions;

namespace CmsWeb.Areas.Main.Models.Report
{
    public class ChurchAttendance2Model
    {
        public DateTime? Dt1 { get; set; }
        public DateTime? Dt2 { get; set; }

        private List<DateTime> weeks;

        public ChurchAttendance2Model(DateTime? dt1, DateTime? dt2, string SkipWeeks)
        {
            Dt1 = dt1;
            Dt2 = dt2;
            weeks = DbUtil.Db.SundayDates(Dt1, Dt2).Select(w => w.Dt.Value).ToList();
            if (SkipWeeks.HasValue())
                foreach (var wk in SkipWeeks.Split(','))
                {
                    var dt = wk.ToDate();
                    if (dt.HasValue)
                        weeks.Remove(dt.Value);
                }
        }


        public class ProgInfo
        {
            public string Name { get; set; }
            public string RptGroup { get; set; }
            public decimal? StartHour { get; set; }
            public decimal? EndHour { get; set; }
            List<DateTime> _cols;
            public List<DateTime> Cols
            {
                get
                {
                    if (_cols == null)
                    {
                        _cols = new List<DateTime>();
                        var re = new Regex(@"(\d+:\d+ [AP]M),?");
                        var m = re.Match(RptGroup);
                        while (m.Success)
                        {
                            _cols.Add(DateTime.Parse(m.Groups[1].Value));
                            m = m.NextMatch();
                        }
                    }
                    return _cols;
                }
            }
            int? _line;
            public int Line
            {
                get
                {
                    if (!_line.HasValue)
                        _line = Regex.Match(RptGroup, @"\A\d+").Value.ToInt();
                    return _line.Value;
                }
            }
            public List<DateTime> weeks { get; set; }

            public IEnumerable<DivInfo> Divs;

            public IEnumerable<WeekInfo> Weeks
            {
                get
                {
                    var q = from w in weeks
                            select new WeekInfo
                            {
                                Sunday = w,
                                Meetings = from d in Divs
                                           from m in d.Meetings
                                           where m.date >= w.AddHours((double)StartHour.Value)
                                           where m.date <= w.AddHours((double)EndHour.Value)
                                           select m
                            };
                    return q.Where(w => w.Meetings.Sum(m => m.Present) > 0);
                }
            }

            public double Total()
            {
                var q = from w in Weeks
                        from m in w.Meetings
                        group m by w.Sunday into g
                        select g.Sum(mm => mm.Present);
                if (q.Count() == 0)
                    return 0;
                return q.Average();
            }
            public double Column(TimeSpan c)
            {
                var q = from w in Weeks
                        from m in w.Meetings
                        where m.date.TimeOfDay == c
                        group m by w.Sunday into g
                        select g.Sum(mm => mm.Present);
                if (q.Count() == 0)
                    return 0;
                return q.Average();
            }
        }
        public class WeekInfo
        {
            public DateTime Sunday { get; set; }
            public IEnumerable<MeetInfo> Meetings;
        }
        public class DivInfo
        {
            public ProgInfo Prog { get; set; }
            public int DivId { get; set; }
            public string Name { get; set; }
            public int Line { get; set; }
            public IEnumerable<MeetInfo> Meetings;
            public List<DateTime> weeks { get; set; }
            public IEnumerable<WeekInfo> Weeks
            {
                get
                {
                    var q = from w in weeks
                            select new WeekInfo
                            {
                                Sunday = w,
                                Meetings = from m in Meetings
                                           where m.date >= w.AddHours((double)Prog.StartHour.Value)
                                           where m.date <= w.AddHours((double)Prog.EndHour.Value)
                                           select m
                            };
                    return q.Where(w => w.Meetings.Sum(m => m.Present) > 0);
                }
            }
            public double Total()
            {
                var q = from w in Weeks
                        from m in w.Meetings
                        group m by w.Sunday into g
                        select g.Sum(mm => mm.Present);
                if (q.Count() == 0)
                    return 0;
                return q.Average();
            }
            public double Column(TimeSpan c)
            {
                var q = from w in Weeks
                        from m in w.Meetings
                        where m.date.TimeOfDay == c
                        group m by w.Sunday into g
                        select g.Sum(mm => mm.Present);
                if (q.Count() == 0)
                    return 0;
                return q.Average();
            }
        }

        public class MeetInfo
        {
            public IEnumerable<DivInfo> Divs { get; set; }
            public int OrgId { get; set; }
            public string OrgName { get; set; }
            public DateTime date { get; set; }
            public int Present { get; set; }
        }
        public IEnumerable<ProgInfo> FetchInfo()
        {
            var q = from p in DbUtil.Db.Programs
                    where p.RptGroup != null && p.RptGroup.Length > 0
                    from pd in p.ProgDivs
                    where pd.Division.ReportLine > 0
                    group pd by p into g
                    orderby g.Key.RptGroup
                    select new ProgInfo
                    {
                        Name = g.Key.Name,
                        RptGroup = g.Key.RptGroup,
                        StartHour = g.Key.StartHoursOffset,
                        EndHour = g.Key.EndHoursOffset,
                        Divs = from pd in g
                               orderby pd.Division.ReportLine
                               select new DivInfo
                               {
                                   DivId = pd.DivId,
                                   Name = pd.Division.Name,
                                   Meetings = from dg in pd.Division.DivOrgs
                                              from m in dg.Organization.Meetings
                                              where m.MeetingDate
                                                >= Dt1.Value.AddDays((double)-6)
                                              where m.MeetingDate
                                                <= Dt2.Value.AddDays((double)6)
                                              select new MeetInfo
                                              {
                                                  date = m.MeetingDate.Value,
                                                  OrgId = m.OrganizationId,
                                                  OrgName = m.Organization.OrganizationName,
                                                  Present = m.NumPresent,
                                              }
                               },
                    };
            var list = q.ToList();
            foreach (var p in list)
            {
                p.weeks = weeks;
                foreach (var d in p.Divs)
                {
                    d.Prog = p;
                    d.weeks = weeks;
                }
            }
            return list;
        }
    }
}
