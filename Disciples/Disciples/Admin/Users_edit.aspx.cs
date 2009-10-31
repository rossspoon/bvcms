using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DiscData;
using System.Linq;
using Disciples;
using UtilityExtensions;

public partial class admin_user_edit : System.Web.UI.Page
{
    bool isEditMode = true;
    User mu;
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        string userName = HttpContext.Current.Request["username"];
        if (!userName.HasValue())
        {
            Page.Title = ActionTitle.Text = "Add User";
            isEditMode = false;
            Group.ContextUser = new User();
        }
        else
        {
            Group.ContextUser = DbUtil.Db.GetUser(userName);
            mu = DbUtil.Db.GetUser(userName);
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (isEditMode)
            {
                UserID.Text = mu.Username;
                //UserID.Enabled = false;

                FirstName.Text = mu.FirstName;
                LastName.Text = mu.LastName;
                if (mu.BirthDay.HasValue)
                    Birthday.Text = mu.BirthDay.Value.ToShortDateString();
                LastLogin.Text = mu.LastLoginDate.ToString();
                NotifyAll.Checked = mu.NotifyAll ?? true;
                NotifyEnabled.Checked = mu.NotifyEnabled ?? true;
                SiteAdministrator.Checked = IsUserInRole("Administrator");
                pid.Text = mu.PeopleId.ToString();

                if (mu == null)
                    return; // Review: scenarios where this happens.

                Email.Text = mu.EmailAddress;
                ActiveUser.Checked = mu.IsApproved;

                unlockUser.Enabled = mu.IsLockedOut;
            }
        }
    }

    public void SaveClick(object sender, EventArgs e)
    {
        if (!isEditMode)
            AddUser(sender, e);
        UpdateUser(sender, e);
        if (!isEditMode)
            Response.Redirect("~/Admin/Users_edit.aspx?username=" + mu.Username);
    }
    public void DeleteClick(object sender, EventArgs e)
    {
        if (!isEditMode || !Page.IsValid)
            return;
        try
        {
            DbUtil.Db.BlogNotifications.DeleteAllOnSubmit(mu.BlogNotifications);
            DbUtil.Db.PageVisits.DeleteAllOnSubmit(mu.PageVisits);
            DbUtil.Db.PrayerSlots.DeleteAllOnSubmit(mu.PrayerSlots);
            DbUtil.Db.UserRoles.DeleteAllOnSubmit(mu.UserRoles);
            DbUtil.Db.PendingNotifications.DeleteAllOnSubmit(mu.PendingNotifications);
            DbUtil.Db.Users.DeleteOnSubmit(mu);

            var q = from x in DbUtil.Db.VerseCategoryXrefs
                    where x.VerseCategory.CreatedBy == mu.UserId
                    select x;
            DbUtil.Db.VerseCategoryXrefs.DeleteAllOnSubmit(q);
            DbUtil.Db.VerseCategories.DeleteAllOnSubmit(mu.VerseCategories);

            DbUtil.Db.SubmitChanges();
            Response.Redirect("~/Admin/Users.aspx");
        }
        catch (Exception ex)
        {
            SetResultMessage(ex.Message);
        }
    }

    private void UpdateRoleMembership()
    {
        foreach (GridViewRow row in GridView1.Rows)
        {
            var g = Group.LoadByName((string)GridView1.DataKeys[row.RowIndex].Value);
            g.SetAdmin(mu, ((CheckBox)row.FindControl("cbAdmin")).Checked);
            g.SetMember(mu, ((CheckBox)row.FindControl("cbMember")).Checked);
            g.SetBlogger(mu, ((CheckBox)row.FindControl("cbBlogger")).Checked);
        }
        foreach (GridViewRow row in GridView2.Rows)
        {
            var blogid = GridView2.DataKeys[row.RowIndex].Value.ToInt();
            var ck = ((CheckBox)row.FindControl("optout")).Checked;
            if (!ck)
                UserController.DeleteOptOutOnSubmit(blogid, mu.UserId);
        }
        DbUtil.Db.SubmitChanges();
        GridView1.DataBind();
        GridView2.DataBind();
    }

    private void UpdateProfile()
    {
        mu.NotifyAll = NotifyAll.Checked;
        mu.NotifyEnabled = NotifyEnabled.Checked;
        mu.FirstName = FirstName.Text;
        mu.LastName = LastName.Text;
        if (pid.Text.HasValue())
            mu.PeopleId = pid.Text.ToInt();
        else
            mu.PeopleId = null;
        DateTime dt;
        if (DateTime.TryParse(Birthday.Text, out dt))
            mu.BirthDay = dt;
        DbUtil.Db.SubmitChanges();
    }
    public void UpdateUser(object sender, EventArgs e)
    {
        if (!Page.IsValid || mu == null)
            return;
        try
        {
            mu.Username = UserID.Text;
            mu.EmailAddress = Email.Text;
            mu.IsApproved = ActiveUser.Checked;
            UpdateProfile();
            const string Adminstrator = "Administrator";
            bool inrole = Roles.IsUserInRole(mu.Username, Adminstrator);
            if (SiteAdministrator.Checked && !inrole)
                Roles.AddUserToRole(mu.Username, Adminstrator);
            else if (!SiteAdministrator.Checked && inrole)
                Roles.RemoveUserFromRole(mu.Username, Adminstrator);

            DbUtil.Db.SubmitChanges();
            UpdateRoleMembership();
            if (pw.Text.HasValue())
            {
                BVMembershipProvider.provider.AdminOverride = true;
                var u = BVMembershipProvider.provider.GetUser(mu.Username, false);
                u.UnlockUser();
                u.ChangePassword(u.ResetPassword(), pw.Text);
                BVMembershipProvider.provider.AdminOverride = false;
            }
            SetResultMessage("User details has been successfully updated.");
        }
        catch (Exception ex)
        {
            SetResultMessage("Failed to update user details. Error message: " + ex.Message);
        }
    }

    public void AddUser(object sender, EventArgs e)
    {
        if (!Page.IsValid)
            return;
        try
        {
            mu = BVMembershipProvider.provider.NewUser(UserID.Text, pw.Text, Email.Text, ActiveUser.Checked, null);
            if (mu != null && mu.Username.HasValue())
            {
                mu.PasswordQuestion = "what is 1+1?";
                mu.PasswordAnswer = "2";
            }
        }
        catch (Exception ex)
        {
            SetResultMessage("Failed to create new user. Error message: " + ex.Message);
        }
    }

    private void SetResultMessage(string resultMsg)
    {
        trResultRow.Visible = resultMsg.HasValue();
        lblMessage.Text = resultMsg;
    }

    public bool IsUserInRole(string roleName)
    {
        if (mu == null || !roleName.HasValue())
            return false;

        return Roles.IsUserInRole(mu.Username, roleName);
    }
    protected void unlockAccount_Click(object sender, EventArgs e)
    {
        try
        {
            MembershipUser mu = Membership.GetUser(UserID.Text);
            if (mu.UnlockUser())
            {
                Membership.UpdateUser(mu);
                SetResultMessage("User account unlocked");
            }
            else
                SetResultMessage("Could not unlock user account.");
        }
        catch (Exception ex)
        {
            SetResultMessage("Could not unlock user account. " + ex.Message);
        }
    }

    protected void LastName_TextChanged(object sender, EventArgs e)
    {

    }
}
