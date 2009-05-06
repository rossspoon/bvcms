using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UtilityExtensions;
using CMSPresenter;
using CmsData;

namespace CMSWeb.WebParts
{
    public partial class MyInvolvement : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var u = DbUtil.Db.Users.SingleOrDefault(us => us.UserId == Util.UserId);
            if (u == null)
                return;
            dsMyInvolvement.SelectParameters.Clear();
            if (u.PeopleId.HasValue)
            {
                dsMyInvolvement.SelectParameters.Add("pid", u.PeopleId.Value.ToString());
                dsMyInvolvement.SelectParameters.Add("sortExpression", "Organization");
                dsMyInvolvement.SelectParameters.Add("maximumRows", "99999");
                dsMyInvolvement.SelectParameters.Add("startRowIndex", "0");
                grdMyInvolvement.DataBind();
            }
            else
                grdMyInvolvement.Visible = false;
        }
    }
}