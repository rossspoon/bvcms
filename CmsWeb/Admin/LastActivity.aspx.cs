using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CmsData;
using UtilityExtensions;
using System.Text;
using System.Data.Linq.SqlClient;

namespace CmsWeb.Admin
{
    public partial class LastActivity : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Label2.Text = Server.MapPath("~/");
            var dt = Util.Now;
            var q = from u in DbUtil.Db.Users
                    orderby u.LastActivityDate descending
                    where u.LastActivityDate != null
                    select new { u.Person.Name, u.LastActivityDate, u.Host, u.UserId, u.Username };
            var sb = new StringBuilder();

            sb.AppendFormat("<tr><td></td><td>&nbsp;</td><td colspan=\"2\">{0}</td></tr>".Fmt(Util.Now));
            var n = 0;
            foreach(var i in q)
            {
                n += 1;
                if (n < 180 || dt.Subtract(i.LastActivityDate.Value).TotalHours <= 48)
                    sb.AppendFormat("<tr><td>{4}</td><td><a href='{3}'>{0}</a></td><td>{1}</td><td>{2}</td></tr>",
                        i.Name, i.LastActivityDate, i.Host,
                        ResolveUrl("~/Admin/Activity.aspx?uid={0}".Fmt(i.UserId)), i.Username);
            }
            Label1.Text = "<a href=\"/\">home</a><table>" + sb.ToString() + "</table>";
        }

        protected void disable_Click(object sender, EventArgs e)
        {
            System.IO.File.Move(Server.MapPath("~/App_Offline1.htm"), Server.MapPath("~/App_Offline.htm"));
            Response.Redirect("~/");
        }
    }
}
