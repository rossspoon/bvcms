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

namespace CMSWeb
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Url.Scheme == "http" && Request.Url.Authority == "cms.bellevue.org")
                Response.Redirect("https://cms.bellevue.org/Login.aspx?" + Request.QueryString);
            var terms = DbUtil.Db.Contents.SingleOrDefault(c => c.Name == "TermsOfUse");
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
            //if (user.LastPasswordChangedDate
            //        .AddDays(WebConfigurationManager.AppSettings["ChangePasswordDays"].ToInt())
            //        < Util.Now)
            //    CMSMembershipProvider.provider.UserMustChangePassword = true;
            var u = DbUtil.Db.Users.Single(us => us.Username == Login1.UserName);
            Util.UserId = u.UserId;
            Util.UserPeopleId = u.PeopleId;
            if (CMSMembershipProvider.provider.UserMustChangePassword)
                Response.Redirect("~/ChangePassword.aspx");
            Util.FormsBasedAuthentication = true;
            CheckStaffRole(Login1.UserName);
            //EventLog.WriteEntry("LoginLog",
            //    "logged in",
            //    EventLogEntryType.Information);
        }

        protected void Login1_LoginError(object sender, EventArgs e)
        {
            var user = Membership.GetUser(Login1.UserName);
            var em = new Emailer();
            foreach (var u in CMSRoleProvider.provider.GetRoleUsers("Admin"))
                em.LoadAddress(u.Person.EmailAddress, u.Name);

            if (user == null)
            {
                //EventLog.WriteEntry("LoginLog",
                //    "non-user",
                //    EventLogEntryType.Information);
                em.NotifyEmail("attempt to login by non-user on " + Request.Url.Authority,
                    "{0} tried to login at {1} but is not a user"
                        .Fmt(Login1.UserName, DateTime.Now));
            }
            else if (user.IsLockedOut)
            {
                //EventLog.WriteEntry("LoginLog",
                //    "locked out",
                //    EventLogEntryType.Information);
                em.NotifyEmail("user locked out on " + Request.Url.Authority,
                    "{0} tried to login at {1} but is locked out"
                        .Fmt(user.UserName, DateTime.Now));
            }
            else if (!user.IsApproved)
            {
                //EventLog.WriteEntry("LoginLog",
                //    "unapproved",
                //    EventLogEntryType.Information);
                em.NotifyEmail("unapproved user logging in on " + Request.Url.Authority,
                    "{0} tried to login at {1} but is not approved"
                        .Fmt(user.UserName, DateTime.Now));
            }
            //else
            //    EventLog.WriteEntry("LoginLog",
            //        "other error",
            //        EventLogEntryType.Information);

        }
        public static void CheckStaffRole(string name)
        {
            var em = new Emailer();
            //if (!name.HasValue())
            //{
            //  foreach (var u in CMSRoleProvider.provider.GetRoleUsers("Admin"))
            //    em.LoadAddress(u.EmailAddress, u.Name);
            //    em.NotifyEmail("unknown user attempted to log in",
            //        string.Format("someone without a name visited site at {0}",
            //            DateTime.Now));
            //    HttpContext.Current.Response.Redirect("AccessDenied.htm");
            //} else
            if (!Roles.IsUserInRole(name, "Staff") && !Roles.IsUserInRole(name, "OrgMembersOnly"))
            {
                foreach (var u in CMSRoleProvider.provider.GetRoleUsers("Admin"))
                    em.LoadAddress(u.Person.EmailAddress, u.Name);
                em.NotifyEmail("user loggedin without a role",
                    string.Format("{0} visited site at {1} but does not have Staff role",
                        name, DateTime.Now));
                HttpContext.Current.Response.Redirect("AccessDenied.htm");
            }
            if (Roles.IsUserInRole(name, "NoRemoteAccess") && DbUtil.CheckRemoteAccessRole)
            {
                foreach (var u in CMSRoleProvider.provider.GetRoleUsers("Admin"))
                    em.LoadAddress(u.Person.EmailAddress, u.Name);
                em.NotifyEmail("NoRemoteAccess", string.Format("{0} tried to login from {1}", name, 
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
                if (Login1.Password == DbUtil.Settings("ImpersonatePassword"))
                {
                    e.Authenticated = true;
                    var em = new Emailer();
                    foreach (var u in CMSRoleProvider.provider.GetRoleUsers("Admin"))
                        em.LoadAddress(u.Person.EmailAddress, u.Name);
                    em.NotifyEmail("{0} is being impersonated".Fmt(Login1.UserName), DateTime.Now.ToString());
                }
                else
                {
                    e.Authenticated = CMSMembershipProvider.provider.ValidateUser(Login1.UserName, Login1.Password);
                    //EventLog.WriteEntry("LoginLog",
                    //    "{0} {1} {2}".Fmt(Login1.UserName, Login1.Password, e.Authenticated),
                    //    EventLogEntryType.Information);
                }
            }
        }
    }
}
