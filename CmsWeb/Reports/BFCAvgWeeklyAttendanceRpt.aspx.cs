using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UtilityExtensions;
using CMSPresenter;
using CmsData;

namespace CMSWeb.Reports
{
    public partial class BFCAvgWeeklyAttendanceRpt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DbUtil.LogActivity("Viewing Weekly Attendance Rpt");
                var fdate = this.QueryString<DateTime?>("fdate");
                var tdate = this.QueryString<DateTime?>("fdate");
                var caids = new ChurchAttendanceConstants();
                DateTime today = caids.MostRecentAttendedSunday();
                FromDate.Text = fdate.HasValue ? fdate.Value.ToString("d") : new DateTime(today.Year, today.Month, 1).ToString("d");
                ToDate.Text = tdate.HasValue ? tdate.Value.ToString("d") : new DateTime(today.Year, today.Month, 1).AddMonths(1).AddDays(-1).ToString("d");
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            ListView1.DataBind();
        }
    }
}
