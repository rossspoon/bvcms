using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UtilityExtensions;
using CmsData;
using CMSPresenter;

namespace CMSWeb.Reports
{
    public partial class BFCWeeklyAttendanceSummaryRpt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                DbUtil.LogActivity("Viewing Weekly Attendance Summary Rpt");
            var dt = DateTime.MinValue;
            if (IsPostBack)
                DateTime.TryParse(SundayDate.Text, out dt);
            if (!IsPostBack || dt == DateTime.MinValue)
            {
                var caids = new ChurchAttendanceConstants();
                var date = this.QueryString<DateTime?>("date");
                dt = date.HasValue ? date.Value : caids.MostRecentAttendedSunday();
            }
            var reportDate = dt.AddDays(-(int)dt.DayOfWeek); //Sunday Date equal/before date selected
            SundayDate.Text = reportDate.ToString("d");
        }
    }
}
