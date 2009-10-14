using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DiscData;
using System.Web.Security;
using UtilityExtensions;

namespace BellevueTeachers.Admin
{
    public partial class Role_edit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public void SaveClick(object sender, EventArgs e)
        {
            var g = Group.LoadById(Request.QueryString<int>("id"));
            foreach (GridViewRow row in GridView1.Rows)
            {
                string u = (string)GridView1.DataKeys[row.RowIndex].Value;
                var user = DbUtil.Db.GetUser(u);
                g.SetAdmin(user, ((CheckBox)row.FindControl("cbAdmin")).Checked);
                g.SetMember(user, ((CheckBox)row.FindControl("cbMember")).Checked);
                g.SetBlogger(user, ((CheckBox)row.FindControl("cbBlogger")).Checked);
            }
            DbUtil.Db.SubmitChanges();
            GridView1.DataBind();
        }
    }
}
