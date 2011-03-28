using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;

namespace CmsWeb.Areas.Main.Models.Report
{
    public class AttendanceDetailModel
    {  
        DateTime dt1;
        DateTime? dt2;
        string name;
        int? divid;
        int? schedid;
        int? campusid;

        public AttendanceDetailModel (DateTime dt1, DateTime? dt2, string name, int? divid, int? schedid, int? campusid)
    	{
            if (dt2.HasValue)
            {
                if (dt2.Value.TimeOfDay == TimeSpan.Zero)
                    dt2 = dt2.Value.AddDays(1);
            }
            else
                dt2 = dt1.AddDays(1);
            this.dt1 = dt1;
            this.dt2 = dt2;
            this.name = name;
            this.divid = divid;
            this.schedid = schedid;
            this.campusid = campusid;
    	}
        public IEnumerable<MeetingRow> FetchMeetings()
        {
            var q = from dio in DbUtil.Db.DivOrgs
                    where dio.DivId == divid
                    from m in dio.Organization.Meetings
                    where m.MeetingDate >= dt1
                    where m.MeetingDate < dt2
                    where campusid == null || campusid == 0 || m.Organization.CampusId == campusid
                    where schedid == null || schedid == 0 || m.Organization.ScheduleId == schedid
                    where name == null || name == "" || m.Organization.OrganizationName.Contains(name)
                    orderby m.Organization.OrganizationName, m.OrganizationId, m.MeetingDate descending
                    select new
                    {
                        MeetingId = m.MeetingId,
                        OrgName = m.Organization.OrganizationName,
                        Leader = m.Organization.LeaderName,
                        location = m.Organization.Location,
                        date = m.MeetingDate.Value,
                        OrgId = m.OrganizationId,
                        Present = m.NumPresent + (m.NumOtherAttends ?? 0),
                        Visitors = m.NumNewVisit + m.NumRepeatVst,
                        OutTowners = m.NumOutTown ?? 0
                    };
            var q2 = from m in q.ToList()
                     select new MeetingRow
                     {
                         MeetingId = m.MeetingId,
                         OrgName = m.OrgName,
                         Leader = m.Leader,
                         date = m.date.ToString("M/d/yy h:mm tt"),
                         OrgId = m.OrgId.ToString(),
                         Present = m.Present,
                         Visitors = m.Visitors,
                         OutTowners = m.OutTowners
                     };
            var list = q2.ToList();
            var t = new MeetingRow
            {
                OrgName = "Total",
                Leader = string.Empty,
                date = "",
                OrgId = "",
                Present = list.Sum(i => i.Present),
                Visitors = list.Sum(i => i.Visitors),
                OutTowners = list.Sum(i => i.OutTowners)
            };
            list.Add(t);
            return list;
        }
        public class MeetingRow
        {
            public int MeetingId { get; set; }
            public string OrgName { get; set; }
            public string Leader { get; set; }
            public string date { get; set; }
            public string OrgId { get; set; }
            public int Present { get; set; }
            public int Visitors { get; set; }
            public int OutTowners { get; set; }
        }
    }
}