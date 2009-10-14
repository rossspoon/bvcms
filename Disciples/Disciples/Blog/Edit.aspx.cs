using System;
using System.Web.UI.WebControls;
using DiscData;
using System.Collections.Generic;
using UtilityExtensions;

public partial class Blog_Edit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int? id = Request.QueryString<int?>("id");
        if (!id.HasValue)
            Response.Redirect("/");
        var post = BlogPost.LoadFromId(id.Value);

        EditBlogPost.Post = post;

        EditBlogPost.CancelUrl = "/Blog/{0}.aspx".Fmt(post.Id);

        ((BellevueTeachers.Site)Master).AddCrumb("Blog", "/Blog/{0}.aspx", post.BlogCached.Name)
          .Add("Post", EditBlogPost.CancelUrl);
    }
    
}
