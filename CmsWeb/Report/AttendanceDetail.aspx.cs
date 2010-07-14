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

            var q = from m in DbUtil.Db.Meetings
                    where m.MeetingDate >= dt1 && m.MeetingDate < dt2
                    where divid == 0 || m.Organization.DivOrgs.Any(di => di.DivId == divid)
                    where schedid == 0 || m.Organization.ScheduleId == schedid
                    where campusid == null || m.Organization.CampusId == campusid
                    where name == "" || name == null || m.Organization.OrganizationName.Contains(name) || m.Organization.LeaderName.Contains(name)
                    orderby m.Organization.OrganizationName, m.MeetingDate
                    select new
                    {
                        OrgName = m.Organization.OrganizationName,
                        Leader = m.Organization.LeaderName,
                        location = m.Organization.Location,
                        Date = m.MeetingDate,
                        Count = m.NumPresent,
                        Description = m.Description
                    };
            var list = q.ToList();
            var t = new
            {
                OrgName = "Total",
                Leader = string.Empty,
                location = string.Empty,
                Date = (DateTime?)null,
                Count = list.Sum(i => i.Count),
                Description = string.Empty
            };
            list.Add(t);

            var dg = new DataGrid();
            bd.Controls.Add(dg);
            dg.DataSource = list;
            dg.DataBind();
        }
    }
}
