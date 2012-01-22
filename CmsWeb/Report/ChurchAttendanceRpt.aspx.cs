using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UtilityExtensions;
using CMSPresenter;
using CmsData;


namespace CmsWeb.Reports
{
    public partial class ChurchAttendanceRpt : System.Web.UI.Page
    {
        private ChurchAttendanceController ods = new ChurchAttendanceController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                DbUtil.LogActivity("Viewing Church Attendance Rpt");
            var dt = DateTime.MinValue;
            if (IsPostBack)
                DateTime.TryParse(SundayDate.Text, out dt);
            if (!IsPostBack || dt.Year < 1900)
            {
                var date = this.QueryString<DateTime?>("date");
                dt = date.HasValue ? date.Value : MostRecentAttendedSunday();
            }
            var reportDate = dt.AddDays(-(int)dt.DayOfWeek); //Sunday Date equal/before date selected
            SundayDate.Text = reportDate.ToString("d");
        }

        protected void ODS_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
        {
            e.ObjectInstance = ods;
        }
        public static DateTime MostRecentAttendedSunday()
        {
            var q = from m in DbUtil.Db.Meetings
                    where m.MeetingDate.Value.Date.DayOfWeek == 0
                    where m.NumPresent > 0
                    orderby m.MeetingDate descending
                    select m.MeetingDate.Value.Date;
            var dt = q.FirstOrDefault();
            return dt == DateTime.MinValue ? DateTime.Today : dt;
        }
    }
}
