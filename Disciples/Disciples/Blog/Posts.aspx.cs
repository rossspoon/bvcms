using System;
using DiscData;
using System.Web.UI.WebControls;
using UtilityExtensions;

public partial class Blog_Posts : System.Web.UI.Page
{
    public Blog blog;
    protected void Page_Load(object sender, EventArgs e)
    {
        string Name = Request.QueryString<string>("name");
        string[] a = Name.Split('/');
        var name = a[0];
        var category = string.Empty;
        if (a.Length > 1)
            category = a[1];
        blog = Blog.LoadByName(name);
        Archives1.blog = blog;
        if (blog == null || !User.IsInRole("Administrator") && !blog.IsMember && !blog.IsPublic)
            Response.Redirect("~/");
        AddEntry.NavigateUrl = "~/Blog/New.aspx?id=" + blog.Id;
        AddEntry.Visible = blog.IsBlogger;


        Title = blog.Title;
        var site = Master as Disciples.Site;
        site.HeadTitleText = blog.Title;
        site.HeadTitleLink = "~/Blog/{0}.aspx".Fmt(blog.Name);
        site.HeadBylineText = blog.Description;

        site.AddCrumb("Blogs", "~/Blog/");
        site.AddCrumb("Blog", "~/Blog/{0}.aspx", blog.Name);
        int? p = Request.QueryString<int?>("page");
        if (!p.HasValue)
            p = 1;
        var mon = Request.QueryString<DateTime?>("mon");

        if (mon.HasValue)
        {
            SideStep.Visible = false;
            Posts.DataSource = BlogPostController.FetchMonthOfPosts(blog.Id, mon.Value);
            PostsOnPage.DataSource = Posts.DataSource;
            Posts.DataBind();
            PostsOnPage.DataBind();
        }
        else
        {
            SideStep.Visible = true;
            int tcount;
            Posts.DataSource = BlogPostController.FetchPageOfPosts(blog.Id, category, 7, p.Value, out tcount);
            PostsOnPage.DataSource = Posts.DataSource;
            Posts.DataBind();
            PostsOnPage.DataBind();

            const string STR_Blogaspxpage = "~/Blog/{0}.aspx?page={1}";
            CategoryPaging2.Visible = category.HasValue();
            if (CategoryPaging2.Visible)
                PageOf.Text = "Page {0} of {1} in the {2} category".Fmt(p, tcount, category);
            else
                Name = blog.Name;
            OlderPosts.NavigateUrl = STR_Blogaspxpage.Fmt(Name, p + 1);
            OlderPosts.Visible = p < tcount;
            NewerPosts.NavigateUrl = STR_Blogaspxpage.Fmt(Name, p - 1);
            NewerPosts.Visible = p > 1;
        }
    }
}
