using System;
using DiscData;
using UtilityExtensions;

public partial class Blog_New : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int? id = Request.QueryString<int?>("id");
        if (!id.HasValue)
            Response.Redirect("/");

        BlogEdit1.Blog = Blog.LoadById(id.Value);

        BlogEdit1.CancelUrl = "/Blog/{0}.aspx".Fmt(BlogEdit1.Blog.Name);

        ((BellevueTeachers.Site)Master).AddCrumb("Blog", BlogEdit1.CancelUrl);

    }
}
