using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UtilityExtensions;
using CmsData;

namespace CmsWeb.Report
{
    public partial class AttendanceDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var dt1 = this.QueryString<DateTime>("dt1");
            var dt2 = this.QueryString<DateTime?>("dt2");
            if (dt2.HasValue)
                dt2 = dt2.Value.AddDays(1);
            else
                dt2 = dt1.AddDays(1);
            var name = this.QueryString<string>("name");
            name = Server.UrlDecode(name);
            var divid = this.QueryString<int?>("divid");
            var schedid = this.QueryString<int?>("schedid");
            var campusid = this.QueryString<int?>("campusid");

            var q = from dio in DbUtil.Db.DivOrgs
                    let sc = dio.Organization.OrgSchedules.FirstOrDefault() // SCHED
                    where dio.DivId == divid
                    from m in dio.Organization.Meetings
                    where m.MeetingDate >= dt1
                    where m.MeetingDate < dt2
                    where campusid == null || campusid == 0 || m.Organization.CampusId == campusid
                    where schedid == null || schedid == 0 || sc.ScheduleId == schedid
                    where name == null || name == "" || m.Organization.OrganizationName.Contains(name)
                    orderby m.Organization.OrganizationName, m.OrganizationId, m.MeetingDate descending
                    select new
                    {
                        OrgName = m.Organization.OrganizationName,
                        Leader = m.Organization.LeaderName,
                        location = m.Organization.Location,
                        date = m.MeetingDate.Value,
                        OrgId = m.OrganizationId,
                        Present = m.NumPresent,
                        Visitors = m.NumNewVisit + m.NumRepeatVst,
                        OutTowners = m.NumOutTown ?? 0
                    };
            var q2 = from m in q.ToList()
                     select new MeetingRow
                     {
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

            var dg = new DataGrid();
            bd.Controls.Add(dg);
            dg.DataSource = list;
            dg.DataBind();
        }
        public class MeetingRow
        {
            public string OrgName { get; set; }
            public string Leader { get; set; }
            public string Location { get; set; }
            public string date { get; set; }
            public string OrgId { get; set; }
            public int Present { get; set; }
            public int Visitors { get; set; }
            public int OutTowners { get; set; }
        }
    }
}
