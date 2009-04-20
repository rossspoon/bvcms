using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UtilityExtensions;
using CmsData;

namespace CMSWeb.Reports
{
    public partial class BFCWeeklyAttendanceSummaryRpt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DbUtil.LogActivity("Viewing Weekly Attendance Summary Rpt");
                var date = this.QueryString<DateTime?>("date");
                SundayDate.Text = date.HasValue ? date.Value.ToString("d") : Util.Now.Date.ToString("d");
            }
            DateTime reportDate = DateTime.Parse(SundayDate.Text);
            reportDate = reportDate.AddDays(-(int)reportDate.DayOfWeek);
            SundayDate.Text = reportDate.ToString("d");
        }
    }
}
