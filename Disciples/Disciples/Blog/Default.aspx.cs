using System;
using CmsData;
using System.Linq;
using UtilityExtensions;

public partial class Blog_Default : System.Web.UI.Page
{
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        DropDownCC1.DataSource = Group.FetchAllGroupsWhereAdmin();
        DropDownCC1.DataTextField = "Name";
        DropDownCC1.DataValueField = "Id";
        DropDownCC1.DataBind();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        NewBlog.Visible = User.IsInRole("Administrator") || Group.FetchAdminGroups().Count() > 0;
        Panel1.Visible = false;
    }
    protected void NewBlogSave_Click(object sender, EventArgs e)
    {
        Blog b = new Blog();
        b.Name = BlogName.Text;
        b.Title = BlogTitle.Text;
        b.Description = BlogDesc.Text;
        b.GroupId = DropDownCC1.SelectedValue.ToInt();
        DbUtil.Db.Blogs.InsertOnSubmit(b);
        DbUtil.Db.SubmitChanges();
        Panel1.Visible = false;
    }
    protected void NewBlog_Click(object sender, EventArgs e)
    {
        Panel1.Visible = true;
    }
}

