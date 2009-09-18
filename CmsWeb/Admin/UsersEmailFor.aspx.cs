using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.DynamicData;
using UtilityExtensions;
using CmsData;

namespace CMSWeb.Admin
{
    public partial class UsersEmailFor : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            DynamicDataManager1.RegisterControl(GridView1);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Insert_Click(object sender, EventArgs e)
        {
            var r = new CmsData.UserCanEmailFor
            {
                CanEmailFor = CanEmailFor.Text.ToInt(),
                UserId = UserId.Text.ToInt(),
            };
            DbUtil.Db.UserCanEmailFors.InsertOnSubmit(r);
            DbUtil.Db.SubmitChanges();
            GridView1.DataBind();
        }
    }
}
