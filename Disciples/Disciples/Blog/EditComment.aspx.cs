using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using DiscData;
using UtilityExtensions;

public partial class EditComment : System.Web.UI.Page
{
    public BlogComment comment;
    protected void Page_Load(object sender, EventArgs e)
    {
        comment = BlogComment.LoadById(Request.QueryString<int>("id"));
        if (DbUtil.Db.CurrentUser.UserId != comment.BlogPost.PosterId && !Roles.IsUserInRole("Administrator"))
            Response.Redirect("/");
        if (!Page.IsPostBack)
        {
            PreviewComment.Text = Util.SafeFormat(comment.Comment);
            Comments.Text = comment.Comment;
        }
    }

    protected void Preview_Click(object sender, EventArgs e)
    {
        PreviewComment.Text = Util.SafeFormat(Comments.Text);
    }

    protected void PostComment_Click(object sender, EventArgs e)
    {
        comment.Comment = Comments.Text;
        DbUtil.Db.SubmitChanges();
        Response.Redirect("/Blog/{0}.aspx#comments".Fmt(comment.BlogPostId));
    }

    protected void Delete_Click(object sender, EventArgs e)
    {
        string page = "/Blog/{0}.aspx#comments".Fmt(comment.BlogPostId);
        DbUtil.Db.BlogComments.DeleteOnSubmit(comment);
        DbUtil.Db.SubmitChanges();
        Response.Redirect(page);
    }
}
