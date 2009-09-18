using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UtilityExtensions;

namespace CMSWeb.Report
{
    public partial class WorshipDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var dt = new DateTime(this.QueryString<long>("dt"));
            var ctl = new CMSPresenter.ChurchAttendanceController();
            var dg = new DataGrid();
            bd.Controls.Add(dg);
            dg.DataSource = ctl.ChurchAttendanceDetail(dt);
            dg.DataBind();
        }
    }
}
