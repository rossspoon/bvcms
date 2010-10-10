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
    public class ChurchAttendanceModel
    {
        public DateTime Sunday { get; set; }

        public ChurchAttendanceModel(DateTime sunday)
        {
            Sunday = sunday;
        }

        public class ProgInfo
        {
            public string Name { get; set; }
            public IEnumerable<DivInfo> Divs { get; set; }
            public string RptGroup { get; set; }
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
        }
        public class DivInfo
        {
            public int DivId { get; set; }
            public string Name { get; set; }
            public DateTime Dt1 { get; set; }
            public DateTime Dt2 { get; set; }
            public IEnumerable<MeetInfo> Meetings { get; set; }
        }
        public class MeetInfo
        {
            public int OrgId { get; set; }
            public string OrgName { get; set; }
            public DateTime date { get; set; }
            public int Present { get; set; }
            public int Visitors { get; set; }
            public int OutTowners { get; set; }
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
                        Divs = from pd in g
                               let dt1 = Sunday.AddHours((double)g.Key.StartHoursOffset)
                               let dt2 = Sunday.AddHours((double)g.Key.EndHoursOffset)
                               orderby pd.Division.ReportLine
                               select new DivInfo
                               {
                                   DivId = pd.DivId,
                                   Name = pd.Division.Name,
                                   Dt1 = dt1,
                                   Dt2 = dt2,
                                   Meetings = from dg in pd.Division.DivOrgs
                                              from m in dg.Organization.Meetings
                                              where m.MeetingDate >= dt1
                                              where m.MeetingDate < dt2
                                              select new MeetInfo
                                              {
                                                  date = m.MeetingDate.Value,
                                                  OrgId = m.OrganizationId,
                                                  OrgName = m.Organization.OrganizationName,
                                                  Present = m.NumPresent,
                                                  Visitors = m.NumNewVisit + m.NumRepeatVst,
                                                  OutTowners = m.NumOutTown ?? 0
                                              }
                               },
                    };
            return q;
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
    }
}
