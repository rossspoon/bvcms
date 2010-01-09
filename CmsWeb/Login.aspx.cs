/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Web;
using System.Web.Security;
using UtilityExtensions;
using System.Web.Configuration;
using System.Text.RegularExpressions;
using System.Linq;
using CMSPresenter;
using CmsData;
using System.Text;
using System.Net.Mail;

namespace CMSWeb
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // just something special for Bellevue
            if (Request.Url.Scheme == "http" && Request.Url.Authority == "cms.bellevue.org")
                if (Request.QueryString.Count > 0)
                    Response.Redirect("https://cms.bellevue.org/Login.aspx?" + Request.QueryString);
                else
                    Response.Redirect("https://cms.bellevue.org/Login.aspx");

            var terms = DbUtil.Content("TermsOfUse");
            if (terms != null)
                TermsLabel.Text = terms.Body;
            else
                TermsLabel.Text =
@"This web site is for the use of staff, teachers, and leadership to access church
membership information online. All content on this site should only be used for
the purpose of ministry to and edification of the body of Christ at Bellevue Baptist Church.
By logging in below, you agree that you understand this purpose and will abide by it.";

            Login1.Focus();
            if (User.Identity.IsAuthenticated)
                Response.Redirect("~/");
        }

        protected void Login1_LoggedIn(object sender, EventArgs e)
        {
            var user = Membership.GetUser(Login1.UserName);
            var u = DbUtil.Db.Users.Single(us => us.Username == Login1.UserName);
            Util.UserId = u.UserId;
            Util.UserPeopleId = u.PeopleId;
            if (CMSMembershipProvider.provider.UserMustChangePassword)
                Response.Redirect("~/ChangePassword.aspx");
            Util.FormsBasedAuthentication = true;
            CheckStaffRole(Login1.UserName);
        }

        protected void Login1_LoginError(object sender, EventArgs e)
        {
            var user = Membership.GetUser(Login1.UserName);
            if (user == null)
                NotifyAdmins("attempt to login by non-user on " + Request.Url.Authority,
                        "{0} tried to login at {1} but is not a user"
                            .Fmt(Login1.UserName, Util.Now));
            else if (user.IsLockedOut)
                NotifyAdmins("user locked out on " + Request.Url.Authority,
                        "{0} tried to login at {1} but is locked out"
                            .Fmt(user.UserName, Util.Now));
            else if (!user.IsApproved)
                NotifyAdmins("unapproved user logging in on " + Request.Url.Authority,
                        "{0} tried to login at {1} but is not approved"
                            .Fmt(user.UserName, Util.Now));
        }
        public static void CheckStaffRole(string name)
        {
            if (!Roles.IsUserInRole(name, "Staff") && !Roles.IsUserInRole(name, "OrgMembersOnly"))
            {
                NotifyAdmins("user loggedin without a role",
                    string.Format("{0} visited site at {1} but does not have Staff role",
                        name, Util.Now));
                FormsAuthentication.SignOut();
                HttpContext.Current.Response.Redirect("Errors/AccessDenied.htm");
            }
            if (Roles.IsUserInRole(name, "NoRemoteAccess") && DbUtil.CheckRemoteAccessRole)
            {
                NotifyAdmins("NoRemoteAccess", string.Format("{0} tried to login from {1}", name, 
                    HttpContext.Current.Request.UserHostAddress));
                HttpContext.Current.Response.Redirect("NoRemoteAccess.htm");
            }
        }

        protected void Login1_Authenticate(object sender, System.Web.UI.WebControls.AuthenticateEventArgs e)
        {
            var user = DbUtil.Db.Users.SingleOrDefault(u => u.Username == Login1.UserName);
            if (user != null && Login1.Password == user.TempPassword)
            {
                user.TempPassword = null;
                user.IsLockedOut = false;
                DbUtil.Db.SubmitChanges();
                e.Authenticated = true;
            }
            else
            {
                if (Login1.Password == DbUtil.Settings("ImpersonatePassword", null))
                {
                    e.Authenticated = true;
                    Notify(WebConfigurationManager.AppSettings["senderrorsto"], 
                        "{0} is being impersonated".Fmt(Login1.UserName), 
                        Util.Now.ToString());
                }
                else
                    e.Authenticated = CMSMembershipProvider.provider.ValidateUser(Login1.UserName, Login1.Password);
            }
        }
        private static void Notify(string to, string subject, string message)
        {
            Util.Email2(new SmtpClient(), Util.FirstAddress(DbUtil.Settings("AdminMail", DbUtil.SystemEmailAddress)).Address, to, subject, message);
        }
        private static void NotifyAdmins(string subject, string message)
        {
            var sb = new StringBuilder();
            foreach (var u in CMSRoleProvider.provider.GetRoleUsers("Admin"))
            {
                if (!Util.ValidEmail(u.Person.EmailAddress))
                    continue;
                if (sb.Length > 0)
                    sb.Append(",");
                sb.AppendFormat("{0} <{1}>", u.Person.Name, u.Person.EmailAddress);
            }
            Notify(sb.ToString(), subject, message);
        }
    }
}
