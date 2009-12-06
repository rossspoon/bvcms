using System;
using System.Web.Security;
using System.Web.UI.WebControls;
using CmsData;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading;
using System.Web;
using UtilityExtensions;

public partial class Admin_admin_users : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //pager1.PageSize = Util.GetPageSizeCookie();
        //pager2.PageSize = Util.GetPageSizeCookie();
        Location.Text = Server.MapPath("~/");
    }

    protected void ListView1_ItemCreated(object sender, ListViewItemEventArgs e)
    {
        //var r = e.Item as ListViewDataItem;
        //var d = r.DataItem as User;
        //if ((selectedId.HasValue && d.Id == selectedId.Value) || r.DisplayIndex == TaskList.SelectedIndex)
    }

    public void SearchForUsers(object sender, EventArgs e)
    {
        ListView1.DataBind();
    }

    protected void CheckNewUser_ServerValidate(object source, ServerValidateEventArgs args)
    {

    }

    protected void ListView1_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        if (e.CommandName == "Deselect")
            ListView1.SelectedIndex = -1;
        if (e.CommandName == "Imp")
        {
            HttpContext.Current.User = new RolePrincipal(new GenericIdentity(e.CommandArgument.ToString()));
            Thread.CurrentPrincipal = HttpContext.Current.User;
            FormsAuthentication.SetAuthCookie(e.CommandArgument.ToString(), true);
        }
    }

    protected void ListView1_Sorting(object sender, ListViewSortEventArgs e)
    {
        ListView1.SelectedIndex = -1;
    }

    protected void ListView1_ItemDeleted(object sender, ListViewDeletedEventArgs e)
    {
        ListView1.SelectedIndex = -1;
    }

    protected void ListView1_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
    {
        ListView1.SelectedIndex = -1;
    }
}
