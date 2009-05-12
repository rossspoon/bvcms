using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CMSWeb.Admin
{
    public partial class UrgentMessage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Message.Text = (string)Application["getoff"];
        }

        protected void SetMessage_Click(object sender, EventArgs e)
        {
            Application["getoff"] = Message.Text;
            Response.Redirect("/");
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            Application.Remove("getoff");
            Response.Redirect("/");
        }
    }
}
