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
    public partial class ChurchAttendanceRpt : System.Web.UI.Page
    {
        private ChurchAttendanceController ods = new ChurchAttendanceController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DbUtil.LogActivity("Viewing Church Attendance Rpt");
                var date = this.QueryString<DateTime?>("date");
                var caids = new ChurchAttendanceConstants();
                SundayDate.Text = date.HasValue ? date.Value.ToString("d") 
                    : caids.MostRecentAttendedSunday().ToString("d");
            }
            DateTime reportDate = DateTime.Parse(SundayDate.Text);
            reportDate = reportDate.AddDays(-(int)reportDate.DayOfWeek); //Sunday Date equal/before date selected
            SundayDate.Text = reportDate.ToString("d");
        }

        protected void ODS_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
        {
            e.ObjectInstance = ods;
        }
    }
}
