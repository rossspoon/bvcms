using System;
using System.Web.UI.WebControls;
using DiscData;
using System.Web;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading;

public partial class BlogEdit : System.Web.UI.UserControl
{
    private string _CancelUrl;
    public string CancelUrl
    {
        get { return _CancelUrl; }
        set { _CancelUrl = value; }
    }
    private BlogPost _Post;
    public BlogPost Post
    {
        get { return _Post; }
        set
        {
            _Post = value;
            Blog = value.BlogCached;
            if (!Page.IsPostBack)
            {
                PostText.Value = value.Post;
                BlogTitle.Text = value.Title;
                EntryDate.Text = value.EntryDate.Value.ToString("M/d/yy h:mm tt");
            }
        }
    }
    private Blog _Blog;
    public Blog Blog
    {
        get { return _Blog; }
        set
        {
            _Blog = value;
            CheckMembership(value);
        }
    }
    private void CheckMembership(Blog b)
    {
        if (b == null || !b.IsMember)
            Response.Redirect("/");
    }
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        daterow.Visible = Post != null;
        Delete.Visible = daterow.Visible;
    }
    protected void Save_Click(object sender, EventArgs e)
    {
        if (Post == null)
            Post = Blog.NewPost(BlogTitle.Text, PostText.Value);
        else
        {
            Post.Title = BlogTitle.Text;
            Post.EntryDate = DateTime.Parse(EntryDate.Text);
            Post.Post = PostText.Value;
        }

        var postCategories = Post.GetBlogCategories();
        foreach (ListItem li in CheckBoxList1.Items)
        {
            BlogCategory bc = null;
            if (postCategories.ContainsKey(li.Text))
                bc = postCategories[li.Text];
            if (bc == null && li.Selected)
                Post.AddCategory(li.Text);
            else if (bc != null && !li.Selected)
                DbUtil.Db.BlogCategories.DeleteOnSubmit(bc);
        }
        if (TextBox2.Text.HasValue())
            Post.AddCategory(TextBox2.Text);
        DbUtil.Db.SubmitChanges();
        string returnloc2 = "/Blog/{0}.aspx".Fmt(Post.Id);
        if(NotifyByEmail.Checked)
            Post.NotifyEmail(false);
        Response.Redirect(returnloc2);
    }
    protected void CheckBoxList1_DataBound(object sender, EventArgs e)
    {
        if (Post != null)
            foreach (var bc in Post.BlogCategories)
                CheckBoxList1.Items.FindByText(bc.Category).Selected = true;
    }
    protected void Cancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("/Blog/{0}.aspx".Fmt(Blog.Name));
    }
    protected void Delete_Click(object sender, EventArgs e)
    {
        DbUtil.Db.BlogCategories.DeleteAllOnSubmit(Post.BlogCategories);
        DbUtil.Db.PodCasts.DeleteAllOnSubmit(Post.PodCasts);
        DbUtil.Db.BlogPosts.DeleteOnSubmit(Post);
        DbUtil.Db.SubmitChanges();
        Response.Redirect("/Blog/{0}.aspx".Fmt(Blog.Name));
    }
}
