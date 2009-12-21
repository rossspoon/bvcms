using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UtilityExtensions;
using CmsData;

namespace CMSWeb.Report
{
    public partial class AttendanceDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var dt1 = this.QueryString<DateTime>("dt1");
            var dt2 = this.QueryString<DateTime>("dt2").AddDays(1);
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
                        Date = m.MeetingDate.Value,
                        Count = m.NumPresent,
                        Description = m.Description
                    };
            var list = q.ToList();

            var dg = new DataGrid();
            bd.Controls.Add(dg);
            dg.DataSource = list;
            dg.DataBind();
        }
    }
}
