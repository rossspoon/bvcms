using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.DynamicData;

namespace CMSWeb.Admin
{
    public partial class ChurchAttReportIds : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            DynamicDataManager1.RegisterControl(GridView1);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
