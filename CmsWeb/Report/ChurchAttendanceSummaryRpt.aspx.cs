using System;
using System.Linq;
using System.Web.UI.WebControls;
using CMSPresenter;
using UtilityExtensions;
using System.Web.Configuration;
using System.Web;
using System.Collections;
using CmsData;
using System.Data;

namespace CmsWeb
{
    public partial class ChurchAttendanceSummary : System.Web.UI.Page
    {
        private ChurchAttendanceSummaryController ods = new ChurchAttendanceSummaryController();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                DbUtil.LogActivity("Viewing Church Attendance Summary Rpt");
            var dt = DateTime.MinValue;
            if (IsPostBack)
                DateTime.TryParse(SundayDate.Text, out dt);
            if (!IsPostBack || dt.Year < 1900)
            {
                dt = Reports.ChurchAttendanceRpt.MostRecentAttendedSunday();
            }
            var reportDate = dt.AddDays(-(int)dt.DayOfWeek); //Sunday Date equal/before date selected
            SundayDate.Text = reportDate.ToString("d");
        }

        protected void ODS_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
        {
            e.ObjectInstance = ods;
        }
    }
}
