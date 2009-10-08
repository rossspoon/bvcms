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
        DateTime fdt, tdt;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                DbUtil.LogActivity("Viewing Weekly Attendance Rpt");
            fdt = DateTime.MinValue;
            tdt = DateTime.MinValue;
            if (IsPostBack)
            {
                DateTime.TryParse(FromDate.Text, out fdt);
                DateTime.TryParse(ToDate.Text, out tdt);
            }
            if (!IsPostBack || fdt.Year < 1900 || tdt.Year < 1900)
            {
                var fdate = this.QueryString<DateTime?>("fdate");
                var tdate = this.QueryString<DateTime?>("fdate");
                var caids = new ChurchAttendanceConstants();
                var today = caids.MostRecentAttendedSunday();
                fdt = fdate.HasValue ? fdate.Value : new DateTime(today.Year, today.Month, 1);
                tdt = tdate.HasValue ? tdate.Value : new DateTime(today.Year, today.Month, 1).AddMonths(1).AddDays(-1);
            }
            FromDate.Text = fdt.ToString("d");
            ToDate.Text = tdt.ToString("d");
            if (User.IsInRole("Admin"))
                TestButton.Visible = true;
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            ListView1.DataBind();
        }

        protected void TestButton_Click(object sender, EventArgs e)
        {
            ListView1.DataBind();
            var bfc = new CMSPresenter.BFCAttendSummaryController();
            TestGrid.DataSource = bfc.CheckAverages(fdt, tdt);
            TestGrid.DataBind();
            TestGrid.Visible = true;
        }
    }
}
