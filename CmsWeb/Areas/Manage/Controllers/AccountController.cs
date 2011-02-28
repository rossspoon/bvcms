using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using CmsData;
using UtilityExtensions;
using System.Net.Mail;
using CMSPresenter;
using System.IO;
using System.Web.Configuration;
using System.Text;

namespace CmsWeb.Areas.Manage.Controllers
{
    public class AccountController : CmsController
    {
        // This constructor is used by the MVC framework to instantiate the controller using
        // the default forms authentication and membership providers.

        // This constructor is not used by the MVC framework but is instead provided for ease
        // of unit testing this type. See the comments at the end of this file for more
        // information.
        public AccountController()
        {
            FormsAuth = new FormsAuthenticationService();
            MembershipService = new MembershipService();
        }
        [HttpPost]
        public ActionResult KeepAlive()
        {
            return Content("alive");
        }
        [HttpPost]
        public ActionResult CKEditorUpload(string CKEditorFuncNum)
        {
            string baseurl = WebConfigurationManager.AppSettings["UploadUrl"];
            if (!baseurl.HasValue())
                baseurl = "{0}://{1}/Upload/".Fmt(Request.Url.Scheme, Request.Url.Authority);
            var error = string.Empty;
            var fn = string.Empty;
            try
            {
                var file = Request.Files[0];
                fn = Path.GetFileName(file.FileName);
                fn = fn.Replace(' ', '_');
                fn = fn.Replace('(', '-');
                fn = fn.Replace(')', '-');
                fn = fn.Replace(',', '_');
                fn = fn.Replace("#", "");
                fn = fn.Replace("!", "");
                fn = fn.Replace("$", "");
                fn = fn.Replace("%", "");
                fn = fn.Replace("&", "_");
                fn = fn.Replace("'", "");
                fn = fn.Replace("+", "-");
                fn = fn.Replace("=", "-");

                string path = WebConfigurationManager.AppSettings["UploadPath"];
                if (!path.HasValue())
                    path = Server.MapPath("/Upload/");
                path += fn;

                while (System.IO.File.Exists(path))
                {
                    var ext = Path.GetExtension(path);
                    fn = Path.GetFileNameWithoutExtension(path) + "a" + ext;
                    var dir = Path.GetDirectoryName(path);
                    path = Path.Combine(dir, fn);
                }
                file.SaveAs(path);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                baseurl = string.Empty;
            }
            return Content(string.Format(
"<script type='text/javascript'>window.parent.CKEDITOR.tools.callFunction( {0}, '{1}', '{2}' );</script>",
CKEditorFuncNum, baseurl + fn, error));
        }

        public IFormsAuthentication FormsAuth
        {
            get;
            private set;
        }

        public MembershipService MembershipService
        {
            get;
            private set;
        }

        public ActionResult LogOn()
        {
            if (Request.Url.Scheme == "http" && Util.CmsHost.StartsWith("https://"))
                if (Request.QueryString.Count > 0)
                    return Redirect(Util.CmsHost + "Logon?" + Request.QueryString);
                else
                    return Redirect(Util.CmsHost + "Logon");

            if (User.Identity.IsAuthenticated)
                return Redirect("/");

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LogOn(string userName, string password, string returnUrl)
        {
            if (returnUrl.HasValue())
            {
                var lc = returnUrl.ToLower();
                if (lc.StartsWith("/default.aspx") || lc.StartsWith("/login.aspx"))
                    returnUrl = null;
            }

            if (!userName.HasValue())
                return View();

            var ret = AuthenticateLogon(userName, password, Session, Request);
            if (ret is string)
            {
                ModelState.AddModelError("login", ret.ToString());
                return View();
            }
            var user = ret as User;

            if (CMSMembershipProvider.provider.UserMustChangePassword)
                Redirect("/ChangePassword.aspx");
            if (!returnUrl.HasValue())
                if (!CMSRoleProvider.provider.IsUserInRole(user.Username, "Access"))
                    return Redirect("/Person/Index/" + Util.UserPeopleId);
            if (returnUrl.HasValue())
                return Redirect(returnUrl);
            return Redirect("/");
        }
        public static object AuthenticateLogon(string userName, string password, HttpSessionStateBase Session, HttpRequestBase Request)
        {
            var q = DbUtil.Db.Users.Where(uu => 
                uu.Username == userName || 
                uu.Person.EmailAddress == userName ||
                uu.Person.EmailAddress2 == userName
                );

            var impersonating = false;
            User user = null;
            int n = q.Count();
            foreach (var u in q)
                if (u.TempPassword != null && password == u.TempPassword)
                {
                    u.TempPassword = null;
                    u.IsLockedOut = false;
                    DbUtil.Db.SubmitChanges();
                    user = u;
                    break;
                }
                else if (password == DbUtil.Db.Setting("ImpersonatePassword", null))
                {
                    user = u;
                    impersonating = true;
                    break;
                }
                else if (Membership.Provider.ValidateUser(u.Username, password))
                {
                    user = u;
                    break;
                }

            string problem = "There is a problem with your username and password combination. If you are using your email address, it must match the one we have on record. Try again or use one of the links below.";
            if (user == null && n > 0)
            {
                NotifyAdmins("failed password by user on " + Request.Url.OriginalString,
                        "{0} tried to login at {1} but got the password wrong"
                            .Fmt(userName, Util.Now));
                return problem;
            }
            else if (user == null)
            {
                NotifyAdmins("attempt to login by non-user on " + Request.Url.OriginalString,
                        "{0} tried to login at {1} but is not a user"
                            .Fmt(userName, Util.Now));
                return problem;
            }
            else if (user.IsLockedOut)
            {
                NotifyAdmins("user locked out on " + Request.Url.OriginalString,
                        "{0} tried to login at {1} but is locked out"
                            .Fmt(userName, Util.Now));
                return problem;
            }
            else if (!user.IsApproved)
            {
                NotifyAdmins("unapproved user logging in on " + Request.Url.OriginalString,
                        "{0} tried to login at {1} but is not approved"
                            .Fmt(userName, Util.Now));
                return problem;
            }
            if (impersonating == true)
            {
                if (user.Roles.Contains("Finance"))
                {
                    NotifyAdmins("cannot impersonate Finance user on " + Request.Url.OriginalString,
                            "{0} tried to login at {1}".Fmt(userName, Util.Now));
                    return problem;
                }
                Util.Email(Util.Smtp(), DbUtil.AdminMail, 
                    WebConfigurationManager.AppSettings["senderrorsto"], 
                    "{0} is being impersonated on {1}".Fmt(user.Username, Util.Host),
                    Util.Now.ToString());
            }

            FormsAuthentication.SetAuthCookie(user.Username, false);
            SetUserInfo(user.Username, Session);
            Util.FormsBasedAuthentication = true;

            return user;
        }
        private static void NotifyAdmins(string subject, string message)
        {
            var list = CMSRoleProvider.provider.GetRoleUsers("Admin")
                .Select(rr => rr.Person).ToList();
            Emailer.Email(Util.Smtp(), DbUtil.AdminMail,
                list, subject, message);
        }
        public static string CheckAccessRole(string name)
        {
            if (!Roles.IsUserInRole(name, "Access") && !Roles.IsUserInRole(name, "OrgMembersOnly"))
            {
                if (Util.UserPeopleId > 0)
                    return "/Person/Index/" + Util.UserPeopleId;

                if (name.HasValue())
                    NotifyAdmins("user loggedin without a role on " + DbUtil.Db.Host,
                        string.Format("{0} visited site at {1} but does not have Access role",
                            name, Util.Now));
                FormsAuthentication.SignOut();
                return "/Errors/AccessDenied.htm";
            }
            if (Roles.IsUserInRole(name, "NoRemoteAccess") && DbUtil.CheckRemoteAccessRole)
            {
                NotifyAdmins("NoRemoteAccess", string.Format("{0} tried to login from {1}", name, DbUtil.Db.Host));
                return "NoRemoteAccess.htm";
            }
            return null;
        }
        public static void SetUserInfo(string username, System.Web.SessionState.HttpSessionState Session)
        {
            var u = SetUserInfo(username);
            if (u == null)
                return;
            Session["ActivePerson"] = u.Name;
        }
        public static void SetUserInfo(string username, HttpSessionStateBase Session)
        {
            var u = SetUserInfo(username);
            if (u == null)
                return;
            Session["ActivePerson"] = u.Name;
        }
        private static User SetUserInfo(string username)
        {
            var u = DbUtil.Db.Users.SingleOrDefault(us => us.Username == username);
            if (u != null)
            {
                Util.UserId = u.UserId;
                Util.UserPeopleId = u.PeopleId;
                Util2.CurrentPeopleId = Util.UserPeopleId.Value;
            }
            return u;
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AddUser(int id)
        {
            var p = DbUtil.Db.People.Single(pe => pe.PeopleId == id);
            CMSMembershipProvider.provider.AdminOverride = true;
            var newpassword = MembershipService.FetchPassword(DbUtil.Db);
            var user = CMSMembershipProvider.provider.NewUser(
                MembershipService.FetchUsername(DbUtil.Db, p.FirstName, p.LastName),
                newpassword,
                null,
                true,
                id);
            CMSMembershipProvider.provider.AdminOverride = false;
            user.MustChangePassword = false;
            DbUtil.Db.SubmitChanges();
            ViewData["newpassword"] = newpassword;
            return View(user);
        }
        [Authorize(Roles = "Admin")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SendNewUserEmail(int userid, string newpassword)
        {
            var user = DbUtil.Db.Users.Single(u => u.UserId == userid);
            var smtp = Util.Smtp();
            var body = DbUtil.Content("NewUserEmail", 
                    @"Hi {name},
<p>You have a new account on our Church Management System which you can access at the following link:<br />
<a href=""{cmshost}"">{cmshost}</a></p>
<table>
<tr><td>Username:</td><td><b>{username}</b></td></tr>
<tr><td>Password:</td><td><b>{password}</b></td></tr>
</table>
<p>Please visit <a href=""{cmshost}/Display/Page/Welcome"">this welcome page</a> for more information</p>
<p>Thanks,<br />
The bvCMS Team</p>
");
            body = body.Replace("{name}", user.Name);
            body = body.Replace("{cmshost}", DbUtil.Db.Setting("DefaultHost", DbUtil.Db.Host));
            body = body.Replace("{username}", user.Username);
            body = body.Replace("{password}", newpassword);
            Emailer.Email(smtp, DbUtil.AdminMail, user.Person, 
                "New user welcome", body);
            return Redirect("/Admin/Users.aspx?create=1");
        }
        [Authorize(Roles = "Admin")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UsersPage(string newpassword)
        {
            if (!User.IsInRole("Admin"))
                return Content("unauthorized");

            Session[UserController.STR_ShowPassword] = newpassword;
            return Redirect("/Admin/Users.aspx?create=1");
        }

        public ActionResult LogOff()
        {
            FormsAuth.SignOut();
            return Redirect("/");
        }

        public ActionResult ForgotUsername(string email)
        {
            if (Request.HttpMethod.ToUpper() == "GET")
                return View();

            if (!Util.ValidEmail(email))
                ModelState.AddModelError("email", "valid email required");
            if (!ModelState.IsValid)
                return View();

            if (email != null)
                email = email.Trim();
            var q = from u in DbUtil.Db.Users
                    where u.Person.EmailAddress == email || u.Person.EmailAddress2 == email
                    where email != "" && email != null
                    select u;
            var smtp = Util.Smtp();
            foreach (var user in q)
            {
                Emailer.Email(smtp, DbUtil.AdminMail, user.Person, "bvcms forgot username", @"Hi {0},
<p>Your username is: {1}</p>
<p>If you did not request this, please disregard this message.</p>
<p>Thanks,<br />
The bvCMS Team</p>
".Fmt(user.Name, user.Username));
                DbUtil.Db.SubmitChanges();
                Util.Email(smtp, DbUtil.AdminMail, DbUtil.AdminMail, "bvcms user: {0} forgot username".Fmt(user.Name), "no content");
            }
            if (q.Count() == 0)
                Util.Email(smtp, DbUtil.AdminMail, DbUtil.AdminMail, "bvcms unknown email: {0} forgot username".Fmt(email), "no content");

            return RedirectToAction("RequestUsername");

        }
        public ActionResult ForgotPassword(string username)
        {
            if (Request.HttpMethod.ToUpper() == "GET")
                return View();

            if (!username.HasValue())
                ModelState.AddModelError("username", "user name required");
            if (!ModelState.IsValid)
                return View();

            var user = DbUtil.Db.Users.SingleOrDefault(u =>
                u.Username == username);
            var smtp = Util.Smtp();
            if (user != null)
            {
                user.ResetPasswordCode = Guid.NewGuid();
                var link = "{0}://{1}/Account/ResetPassword/{2}".Fmt(
                    Request.Url.Scheme, Request.Url.Authority, user.ResetPasswordCode.ToString());
                Emailer.Email(smtp, DbUtil.AdminMail, user.Person, "bvcms password reset link", @"Hi {0},
<p>You recently requested a new password.  To reset your password, follow the link below:<br />
<a href=""{1}"">{1}</a></p>
<p>If you did not request a new password, please disregard this message.</p>
<p>Thanks,<br />
The bvCMS Team</p>
".Fmt(user.Name, link));
                DbUtil.Db.SubmitChanges();
                Util.Email(smtp, DbUtil.AdminMail, DbUtil.AdminMail, "{0} user: {1} forgot password".Fmt(DbUtil.Db.Host, user.Name), "no content");
            }
            else
                Util.Email(smtp, DbUtil.AdminMail, DbUtil.AdminMail, "{0} unknown user: {1} forgot password".Fmt(DbUtil.Db.Host, username), "no content");

            return RedirectToAction("RequestPassword");

        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult RequestPassword()
        {
            return View();
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult RequestUsername()
        {
            return View();
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ResetPassword(Guid id)
        {
            var user = DbUtil.Db.Users.SingleOrDefault(u => u.ResetPasswordCode == id);
            if (user == null)
                return Content("Password has been reset already, if you did not get the email, contact the church");

            CMSMembershipProvider.provider.AdminOverride = true;
            var mu = CMSMembershipProvider.provider.GetUser(user.Username, false);
            mu.UnlockUser();
            var newpassword = MembershipService.FetchPassword(DbUtil.Db);
            mu.ChangePassword(mu.ResetPassword(), newpassword);
            CMSMembershipProvider.provider.AdminOverride = false;

            user.ResetPasswordCode = null;

            DbUtil.Db.SubmitChanges();
            var smtp = Util.Smtp();
            Emailer.Email(smtp, DbUtil.AdminMail, user.Person, "bvcms new password", @"Hi {0},
<p>Your new password is {1}</p>
<p>If you did not request a new password, please notify us ASAP.</p>
<p>Thanks,<br />
The bvCMS Team</p>
".Fmt(user.Name, newpassword));

            return View();
        }
        [Authorize]
        public ActionResult ChangePassword()
        {

            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;

            return View();
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {

            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;

            if (!ValidateChangePassword(currentPassword, newPassword, confirmPassword))
            {
                return View();
            }

            try
            {
                if (MembershipService.ChangePassword(User.Identity.Name, currentPassword, newPassword))
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("_FORM", "The current password is incorrect or the new password is invalid.");
                    return View();
                }
            }
            catch
            {
                ModelState.AddModelError("_FORM", "The current password is incorrect or the new password is invalid.");
                return View();
            }
        }

        public ActionResult ChangePasswordSuccess()
        {

            return View();
        }

        //protected override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    if (filterContext.HttpContext.User.Identity is WindowsIdentity)
        //    {
        //        throw new InvalidOperationException("Windows authentication is not supported.");
        //    }
        //}

        #region Validation Methods

        private bool ValidateChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            if (String.IsNullOrEmpty(currentPassword))
            {
                ModelState.AddModelError("currentPassword", "You must specify a current password.");
            }
            if (newPassword == null || newPassword.Length < MembershipService.MinPasswordLength)
            {
                ModelState.AddModelError("newPassword",
                    String.Format(CultureInfo.CurrentCulture,
                         "You must specify a new password of {0} or more characters.",
                         MembershipService.MinPasswordLength));
            }

            if (!String.Equals(newPassword, confirmPassword, StringComparison.Ordinal))
            {
                ModelState.AddModelError("_FORM", "The new password and confirmation password do not match.");
            }

            return ModelState.IsValid;
        }

        private bool ValidateLogOn(string userName, string password)
        {
            if (!MembershipService.ValidateUser(userName, password))
                ModelState.AddModelError("login", "Username or password not recognized.");

            return ModelState.IsValid;
        }

        private bool ValidateRegistration(string userName, string email, string password, string confirmPassword)
        {
            if (String.IsNullOrEmpty(userName))
            {
                ModelState.AddModelError("username", "You must specify a username.");
            }
            if (String.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("email", "You must specify an email address.");
            }
            if (password == null || password.Length < MembershipService.MinPasswordLength)
            {
                ModelState.AddModelError("password",
                    String.Format(CultureInfo.CurrentCulture,
                         "You must specify a password of {0} or more characters.",
                         MembershipService.MinPasswordLength));
            }
            if (!String.Equals(password, confirmPassword, StringComparison.Ordinal))
            {
                ModelState.AddModelError("_FORM", "The new password and confirmation password do not match.");
            }
            return ModelState.IsValid;
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://msdn.microsoft.com/en-us/library/system.web.security.membershipcreatestatus.aspx for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Username already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A username for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }

    // The FormsAuthentication type is sealed and contains static members, so it is difficult to
    // unit test code that calls its members. The interface and helper class below demonstrate
    // how to create an abstract wrapper around such a type in order to make the AccountController
    // code unit testable.

    public interface IFormsAuthentication
    {
        void SignIn(string userName, bool createPersistentCookie);
        void SignOut();
    }

    public class FormsAuthenticationService : IFormsAuthentication
    {
        public void SignIn(string userName, bool createPersistentCookie)
        {
            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }
        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}
