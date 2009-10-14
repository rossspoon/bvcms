using System;
using DiscData;
using UtilityExtensions;

public partial class Admin_CMSPageList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int? pageID = Request.QueryString<int?>("id");
        if(pageID.HasValue)
        {
            var p = ContentService.GetPage(pageID.Value);
            if (p != null)
                Response.Redirect("/view/" + p.PageUrl);
        }
    }
}
