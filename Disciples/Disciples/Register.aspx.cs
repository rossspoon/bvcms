using System;
using System.Web.Security;
using System.Web.UI.WebControls;
using CmsData;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Net.Mail;
using UtilityExtensions;

namespace Disciples
{
    public partial class Register : System.Web.UI.Page
    {
        string fromPage = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            fromPage = Request.QueryString<string>("r");
            Label1.Text = Paragraph1.HeaderText;
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            // check for duplicate in CMS
            //using (var cn = new SqlConnection(Util.ConnectionString))
            //{
            //    cn.Open();
            //    var cmd = new SqlCommand("select count(*) from dbo.Users where username = @username", cn);
            //    cmd.Parameters.AddWithValue("@username", txtLogin.Text);
            //    if (cmd.ExecuteScalar().ToInt() > 0)
            //    {
            //        usernameValidator.IsValid = false;
            //        return;
            //    }
            //}

            //register them

            MembershipCreateStatus status;
            Membership.CreateUser(txtLogin.Text, txtPassword.Text, txtEmail.Text,
                null, null, true, out status);
            //add the profile
            if (status == MembershipCreateStatus.Success)
            {
                var u = DbUtil.Db.Users.Single(uu => uu.Username == txtLogin.Text);
                //u.FirstName = txtFirst.Text;
                //u.LastName = txtLast.Text;
                u.DefaultGroup = "";
                //u.NotifyAll = true;
                //u.NotifyEnabled = true;
                //u.BirthDay = DateTime.Parse(txtBirthday.Text);
                DbUtil.Db.SubmitChanges();

                btnRegister.Enabled = false;
                //set the cookie
                FormsAuthentication.SetAuthCookie(txtLogin.Text, true);

                if (SecretCode.Text != "")
                {
                    var q = Invitation.LoadBySecretCode(SecretCode.Text);
                    CmsData.Group ming = null;
                    foreach (var i in q)
                    {
                        var g = CmsData.Group.LoadById(i.GroupId);
                        g.SetMember(u, true);
                        DbUtil.Db.SubmitChanges();
                        g.NotifyNewUser(txtLogin.Text);
                        if (ming == null || ming.Name.Length > g.Name.Length)
                            ming = g;
                    }
                    u.DefaultGroup = ming.Name;
                    DbUtil.Db.SubmitChanges();
                }

                if (fromPage.HasValue() && !fromPage.ToLower().Contains("register.aspx"))
                    Response.Redirect(fromPage);
                else
                    Response.Redirect("~/default.aspx");
            }
            else
            {
                if (status == MembershipCreateStatus.DuplicateUserName)
                    usernameValidator.IsValid = false;
                if (status == MembershipCreateStatus.InvalidEmail)
                    emailValidator.IsValid = false;
                if (status == MembershipCreateStatus.InvalidPassword)
                    validatePassword.IsValid = false;
            }
        }
        protected void SecretCodeValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
            if (this.SecretCode.Text == "")
                return;

            args.IsValid = false;
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
                else if (Roles.IsUserInRole(txtLogin.Text, g.Name))
                    SecretCodeValidator.Text = "You are already in group {0}".Fmt(g.Name);
                else
                    args.IsValid = true;
                if (!args.IsValid)
                    break;
            }
            return;
        }
        protected void BirthdayValid_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime bd;
            args.IsValid = DateTime.TryParse(txtBirthday.Text, out bd);
        }

        protected void emailValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                var a = new MailAddress(txtEmail.Text);
            }
            catch (Exception ex)
            {
                args.IsValid = false;
                emailValidator.ErrorMessage = "email: " + ex.Message;
            }
        }
    }
}