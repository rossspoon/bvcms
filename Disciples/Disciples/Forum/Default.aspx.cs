using System;
using DiscData;
using System.Linq;

public partial class Forum_Default : System.Web.UI.Page
{
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        DropDownCC1.DataSource = Group.FetchAdminGroups();
        DropDownCC1.DataTextField = "Name";
        DropDownCC1.DataValueField = "Id";
        DropDownCC1.DataBind();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        NewForum.Visible = Group.FetchAdminGroups().Count() > 0;
        Panel1.Visible = false;
    }

    protected void NewForum_Click(object sender, EventArgs e)
    {
        Panel1.Visible = true;
    }
    protected void NewForumSave_Click(object sender, EventArgs e)
    {
        var f = new Forum();
        f.Description = TextBox2.Text;
        var g = Group.LoadById(DropDownCC1.SelectedValue.ToInt());
        g.Forums.Add(f);
        f.CreatedBy = Util.CurrentUser.UserId;
        f.CreatedOn = DateTime.Now;
        DbUtil.Db.SubmitChanges();
        Panel1.Visible = false;
    }
}
