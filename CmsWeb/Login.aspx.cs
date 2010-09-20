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

namespace CmsWeb
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Url.Scheme == "http" && Util.CmsHost.StartsWith("https://"))
                if (Request.QueryString.Count > 0)
                    Response.Redirect(Util.CmsHost + "Login.aspx?" + Request.QueryString);
                else
                    Response.Redirect(Util.CmsHost + "Login.aspx");

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
            SetUserInfo(Login1.UserName);
            if (CMSMembershipProvider.provider.UserMustChangePassword)
                Response.Redirect("~/ChangePassword.aspx");
            Util.FormsBasedAuthentication = true;
            if (user != null && !Login1.DestinationPageUrl.HasValue())
                if (!CMSRoleProvider.provider.IsUserInRole(Login1.UserName, "Access"))
                    Response.Redirect("/Person/Index/" + Util.UserPeopleId);
         
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
            if (!Roles.IsUserInRole(name, "Access") && !Roles.IsUserInRole(name, "OrgMembersOnly"))
            {
                if (name.HasValue())
                    NotifyAdmins("user loggedin without a role on " + Util.Host,
                        string.Format("{0} visited site at {1} but does not have Staff role",
                            name, Util.Now));
                FormsAuthentication.SignOut();
                HttpContext.Current.Response.Redirect("/Errors/AccessDenied.htm");
            }
            if (Roles.IsUserInRole(name, "NoRemoteAccess") && DbUtil.CheckRemoteAccessRole)
            {
                NotifyAdmins("NoRemoteAccess", string.Format("{0} tried to login from {1}", name,
                    HttpContext.Current.Request.UserHostAddress));
                HttpContext.Current.Response.Redirect("NoRemoteAccess.htm");
            }
        }
        public static void SetUserInfo(string username)
        {
            var u = DbUtil.Db.Users.Single(us => us.Username == username);
            Util.UserId = u.UserId;
            Util.UserPeopleId = u.PeopleId;
            Util.CurrentPeopleId = Util.UserPeopleId.Value;
            HttpContext.Current.Session["ActivePerson"] = u.Name;
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
                if (user != null && Login1.Password == DbUtil.Settings("ImpersonatePassword", null))
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
            var smtp = Util.Smtp();
            DbUtil.Email2(smtp, DbUtil.Settings("AdminMail", DbUtil.SystemEmailAddress), to, subject, message);
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
                sb.Append(u.EmailAddress);
            }
            Notify(sb.ToString(), subject, message);
        }
    }
}
