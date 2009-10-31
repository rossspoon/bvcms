using System;
using System.Web.UI;
using System.Web.Security;
using System.Web;
using System.Web.UI.WebControls;
using DiscData;
using System.Text;
using System.Linq;
using UtilityExtensions;

namespace Disciples
{
    public partial class ProfileEdit : System.Web.UI.Page
    {
        User mu = DbUtil.Db.CurrentUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                txtEmail.Text = mu.EmailAddress;
                NotifyAll.Checked = mu.NotifyAll ?? true;
                NotifyEnabled.Checked = mu.NotifyEnabled ?? true;
                if (mu.BirthDay.HasValue)
                    txtBirthday.Text = mu.BirthDay.Value.ToShortDateString();
            }
        }
        protected void PasswordWasChanged(object sender, EventArgs e)
        {
            ChangePasswordResult.ShowSuccess("Password was changed");
        }
        protected void SaveEmail_Click(object sender, EventArgs e)
        {
            mu.EmailAddress = txtEmail.Text;
            mu.NotifyEnabled = NotifyEnabled.Checked;
            mu.NotifyAll = NotifyAll.Checked;
            DbUtil.Db.SubmitChanges();
            EmailChangedResult.ShowSuccess("Email Profile Changed");
        }
        protected void JoinGroup_Click(object sender, EventArgs e)
        {
            var q = Invitation.LoadBySecretCode(SecretCode.Text);
            foreach (var i in q)
            {
                var g = Group.LoadById(i.GroupId);
                g.SetMember(mu, true);
                DbUtil.Db.SubmitChanges();
                g.NotifyNewUser(User.Identity.Name);
            }
            GroupResult.ShowSuccess("group joined");
            DropDownList1.DataBind();
        }

        protected void DropDownList1_DataBound(object sender, EventArgs e)
        {
            ListItem i = DropDownList1.Items.FindByText(mu.DefaultGroup);
            if (i != null)
                i.Selected = true;
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            mu.DefaultGroup = DropDownList1.SelectedValue;
            DbUtil.Db.SubmitChanges();
            GroupResult.ShowSuccess("Default Group Changed");
        }
        protected void SaveBirthday_Click(object sender, EventArgs e)
        {
            if (BirthdayValid.IsValid)
            {
                mu.BirthDay = DateTime.Parse(txtBirthday.Text);
                DbUtil.Db.SubmitChanges();
                BirthdayResult.ShowSuccess("Birthday Changed");
            }
        }

        protected void BirthdayValid_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime bd;
            args.IsValid = DateTime.TryParse(txtBirthday.Text, out bd);
        }

        protected void SecretCodeValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = false;
            if (this.SecretCode.Text == "")
            {
                SecretCodeValidator.Text = "code required";
                return;
            }
            var q = Invitation.LoadBySecretCode(SecretCode.Text);
            if (q.Count() == 0)
            {
                SecretCodeValidator.Text = "code not found, sorry";
                return;
            }
            foreach (var i in q)
            {
                var g = Group.LoadById(i.GroupId);
                if (g == null)
                    SecretCodeValidator.Text = "Your code was good but group does not exist";
                else if (i.Expires < DateTime.Now)
                    SecretCodeValidator.Text = "invitation expired for group {0}, sorry".Fmt(g.Name);
                else if (Roles.IsUserInRole(User.Identity.Name, g.Name))
                    SecretCodeValidator.Text = "You are already in group {0}".Fmt(g.Name);
                else
                    args.IsValid = true;
                if (!args.IsValid)
                    break;
            }
            return;
        }
    }
}