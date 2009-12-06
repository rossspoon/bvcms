using System;
using CmsData;
using System.Linq;
using System.Web.Security;
using UtilityExtensions;

public partial class Blog_Post : System.Web.UI.Page
{
    public Blog blog;
    public bool CanEditComments;
    protected void Page_Load(object sender, EventArgs e)
    {
        var site = Master as Disciples.Site;

        Item1.PostId = Request.QueryString["id"];
        if (Item1.BlogPost == null)
            return;
        blog = Item1.BlogPost.BlogCached;
        Archives1.blog = blog;
        site.HeadTitleText = blog.Title;
        site.HeadTitleLink = "~/Blog/{0}.aspx".Fmt(blog.Name);
        site.HeadBylineText = blog.Description;

        site.AddCrumb("Blogs", "~/Blog/")
            .Add(blog.Name, "~/Blog/{0}.aspx", blog.Name);
        postcomment.Disabled = !User.Identity.IsAuthenticated;
        PreviewArea.Visible = false;

        var nextpost = Item1.BlogPost.NextPost();
        NextListItem.Visible = nextpost != null;
        if (NextListItem.Visible)
        {
            NextEntry.NavigateUrl = "~/Blog/{0}.aspx".Fmt(nextpost.Id);
            NextEntry.Text = nextpost.Title;
        }

        var prevpost = Item1.BlogPost.PrevPost();
        PrevListItem.Visible = prevpost != null;
        if (PrevListItem.Visible)
        {
            PrevEntry.NavigateUrl = "~/Blog/{0}.aspx".Fmt(prevpost.Id);
            PrevEntry.Text = prevpost.Title;
        }

        AddEntry.NavigateUrl = "~/Blog/New.aspx?id=" + blog.Id;
        AddEntry.Visible = blog.IsBlogger;
        CanEditComments = blog.IsBlogger || Roles.IsUserInRole("BlogAdministrator");
        
        Repeater1.DataSource = from bp in blog.BlogPosts
                               from x in bp.BlogCategoryXrefs
                               group x by x.BlogCategory into g
                               let c = g.Count()
                               select new
                               {
                                   Category = g.Key.Name,
                                   BlogName = blog.Name,
                                   Count = c,
                                   Size = c < 3 ? "10pt" :
                                        c < 10 ? "12pt" :
                                        c < 30 ? "14pt" :
                                        "16pt"
                               };
        Repeater1.DataBind();
    }

    protected void PostComment_Click(object sender, EventArgs e)
    {
        var bc = Item1.BlogPost.AddComment(Comments.Text);
        DbUtil.Db.SubmitChanges();
        bc.NotifyEmail();
        ListView2.DataBind();
        this.Comments.Text = "";
    }

    protected void Preview_Click(object sender, EventArgs e)
    {
        PreviewArea.Visible = true;
        PreviewComment.Text = Util.SafeFormat(Comments.Text);
    }
}
