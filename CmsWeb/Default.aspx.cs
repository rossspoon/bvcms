using System;
using UtilityExtensions;

namespace CMSWeb
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //EventLog.WriteEntry("LoginLog",
            //    "default.aspx",
            //    EventLogEntryType.Information);
            var db = this.QueryString<string>("DB");
            if (db.HasValue())
            {
                if (db != "reset")
                    Session["CMS"] = db;
            }
        }
    }
}
