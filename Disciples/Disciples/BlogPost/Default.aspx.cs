using System;
using System.Web;
using System.Web.UI;
using DiscData;
using System.Diagnostics;

public partial class BlogPost_Default : System.Web.UI.Page
{
    public Blog blog;
    public BlogPost blogpost;
    protected void Page_Load(object sender, EventArgs e)
    {
        Title = Request.QueryString<string>("blogid");
        blog = Blog.LoadByName(Title);

        if (blog == null)
            return;
        blogpost = blog.LastPost();
        CommonContentInner.Visible = blogpost != null && blogpost.IsTempPost;
        if (CommonContentInner.Visible)
        {
            TimeSpan t = DateTime.Now - blogpost.EntryDate.Value;
            CommonContentInner.Visible = t.TotalSeconds < 30;
        }

        var l = new LiteralControl();
        l.Text = @"<link rel=""EditURI"" type=""application/rsd+xml"" title=""RSD"" href=""http://{0}/BlogPost/rsd.ashx?blogid={1}"" />"
            .Fmt(Request.Url.Authority, blog.Id);
        h.Controls.Add(l);
    }
    //protected Categories.BlogCategoryDataTable GetCategoryList(int BlogPostId)
    //{
    //    Categories.BlogCategoryDataTable dt;
    //    if (!HttpContext.Current.Items.Contains("categories"))
    //    {
    //        BlogCategoryTableAdapter da = new BlogCategoryTableAdapter();
    //        dt = da.GetData();
    //        HttpContext.Current.Items["categories"] = dt;
    //    }
    //    else
    //        dt = (Categories.BlogCategoryDataTable)HttpContext.Current.Items["categories"];

    //    dt.DefaultView.RowFilter = "BlogPostId = " + blogpost.Id;
    //    return dt;
    //}

}
