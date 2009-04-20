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

namespace CMSWeb
{
    public partial class ChurchAttendanceSummary : System.Web.UI.Page
    {
        private ChurchAttendanceSummaryController ods = new ChurchAttendanceSummaryController();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DbUtil.LogActivity("Viewing Church Attendance Summary Rpt");
                SundayDate.Text = Util.Now.Date.ToString("d");
            }
            var reportDate = DateTime.Parse(SundayDate.Text);
            reportDate = reportDate.AddDays(-(int)reportDate.DayOfWeek); //Sunday Date equal/before date selected

            var caids = new ChurchAttendanceConstants();
            for (int n = 52; n > 0; n--)
                if (caids.HasData(reportDate))
                    break;
                else
                    reportDate = reportDate.AddDays(-7);
            SundayDate.Text = reportDate.ToString("d");
        }

        protected void ODS_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
        {
            e.ObjectInstance = ods;
        }
    }
}
