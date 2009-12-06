using System;
using System.Web.Security;
using System.Web.UI.WebControls;
using CmsData;
using System.Linq;
using UtilityExtensions;

public partial class AdminUsers_admin_roles : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            DropDownList1.DataSource = Group.FetchAllGroupsWhereAdmin();
            DropDownList1.DataTextField = "Name";
            DropDownList1.DataValueField = "Id";
            DropDownList1.DataBind();
        }
    }
    protected void AddRole_Click(object sender, EventArgs e)
    {
        Group.InsertWithRolesOnSubmit(RoleName.Text);
        DbUtil.Db.SubmitChanges();
        GridView1.DataBind();
    }
    protected void AddInvite_Click(object sender, EventArgs e)
    {
        var g = Group.LoadById(DropDownList1.SelectedValue.ToInt());
        g.AddInvitation(TextBox1.Text);
        DbUtil.Db.SubmitChanges();
        Invitations.DataBind();
    }
}
