using System;
using DiscData;

public partial class NewEntry : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int? id = Request.QueryString<int?>("id");
        if (!id.HasValue)
            Response.Redirect("/");
        EditForumEntry1.Forum = Forum.LoadFromId(id.Value);
        EditForumEntry1.CancelUrl = "/Forum/{0}.aspx".Fmt(id);
        ((BellevueTeachers.Site)Master).AddCrumb("Topics", EditForumEntry1.CancelUrl);
    }
}
