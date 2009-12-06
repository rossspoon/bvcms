using System;
using System.Web;
using CmsData;
using System.Web.Security;
using System.Linq;
using System.Collections.Generic;
using UtilityExtensions;

public partial class Blog_Item : System.Web.UI.UserControl
{
    private BlogPost post;
    public BlogPost BlogPost
    {
        get { return post; }
    }
    public bool SingleItem { get; set; }
    public string PostId
    {
        set
        {
            post = BlogPost.LoadFromId(value.ToInt());
        }
    }

    public string PermaLink;
    public string BlogLink;
    public string BlogLink0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (post == null)
            return;
        PermaLink = Util.ResolveServerUrl("~/Blog/{0}.aspx".Fmt(post.Id));
        BlogLink0 = Util.ResolveServerUrl("~/Blog/{0}".Fmt(post.BlogCached.Name));
        BlogLink = BlogLink0 + ".aspx";
    }
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        if (post.BlogCached == null)
            Response.Redirect("~/");
        if (!(post.BlogCached.IsMember
            || Page.User.IsInRole("Administrator")
            || (Page.User.IsInRole("BlogAdministrator") && post.BlogCached.PrivacyLevel == 1)
            || post.BlogCached.PrivacyLevel == 0
            ))
            Response.Redirect("~/");

        Edit.NavigateUrl = "~/Blog/Edit.aspx?id=" + post.Id;
        Edit.Visible = BlogPost.BlogCached.IsBlogger || Roles.IsUserInRole("BlogAdministrator");
        CategoryList.DataSource = BlogCategoryController.GetCategoryListFromCache(post.Id);
        CategoryList.DataBind();
    }
}
