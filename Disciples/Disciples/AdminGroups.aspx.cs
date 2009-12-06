using System;
using System.Web.Security;
using System.Web.UI.WebControls;
using CmsData;
using System.Linq;
using UtilityExtensions;

public partial class AdminGroups : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Group.FetchAdminGroups().Count() == 0)
            Response.Redirect("~/Default.aspx");
        HiddenField1.Value = User.Identity.Name;
    }
    public void EnabledChanged(object sender, EventArgs e)
    {
        string userID = null;
        CheckBox checkBox = sender as CheckBox;
        if (checkBox == null)
            return;

        if (!string.IsNullOrEmpty(checkBox.Attributes["Value"]))
            userID = checkBox.Attributes["Value"];

        if (userID == null)
            return;

        MembershipUser user = Membership.FindUsersByName(userID)[userID];
        user.IsApproved = checkBox.Checked;

        Membership.UpdateUser(user);
    }
    protected void AddInvite_Click(object sender, EventArgs e)
    {
        var g = Group.LoadById(DropDownList1.SelectedValue.ToInt());
        if (TextBox1.Text.HasValue())
        {
            g.AddInvitation(TextBox1.Text);
            DbUtil.Db.SubmitChanges();
            Invitations.DataBind();
        }
    }
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        HyperLink1.NavigateUrl = "~/Default.aspx?group=" + DropDownList1.SelectedValue;
    }
    protected void DropDownList1_DataBound(object sender, EventArgs e)
    {
        HyperLink1.NavigateUrl = "~/Default.aspx?group=" + DropDownList1.SelectedValue;
    }
}
