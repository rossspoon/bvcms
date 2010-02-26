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

namespace CMSWeb.Areas.Manage.Controllers
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
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings",
            Justification = "Needs to take same parameter type as Controller.Redirect()")]
        public ActionResult LogOn(string userName, string password, bool rememberMe, string returnUrl)
        {
            if (!ValidateLogOn(userName, password))
            {
                ViewData["rememberMe"] = rememberMe;
                return View();
            }

            FormsAuth.SignIn(userName, rememberMe);
            if (!String.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            else
                return Redirect("/");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AddUser(int id)
        {
            var p = DbUtil.Db.People.Single(pe => pe.PeopleId == id);
            CMSMembershipProvider.provider.AdminOverride = true;
            var newpassword = MembershipService.FetchPassword();
            var user = CMSMembershipProvider.provider.NewUser(
                MembershipService.FetchUsername(p.FirstName, p.LastName),
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
            Util.Email(smtp, DbUtil.Settings("AdminMail", DbUtil.SystemEmailAddress), user.Name, user.Person.EmailAddress,
                    "New user welcome",
                    @"Hi {0},
<p>You have a new account on our Church Management System which you can access at the following link:<br />
<a href=""{1}"">{1}</a></p>
<table>
<tr><td>Username:</td><td><b>{2}</b></td></tr>
<tr><td>Password:</td><td><b>{3}</b></td></tr>
</table>
<p>Please visit <a href=""{1}/Display/Page/Welcome"">this welcome page</a> for more information</p>
<p>Thanks,<br />
The bvCMS Team</p>
".Fmt(user.Name, DbUtil.Settings("DefaultHost", Util.Host), user.Username, newpassword));
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

        public ActionResult Register()
        {

            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Register(string userName, string email, string password, string confirmPassword)
        {

            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;

            if (ValidateRegistration(userName, email, password, confirmPassword))
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus = MembershipService.CreateUser(userName, password, email);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsAuth.SignIn(userName, false /* createPersistentCookie */);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("_FORM", ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            return View();
        }


        public ActionResult ForgotUsername(string email, string dob)
        {
            if (Request.HttpMethod.ToUpper() == "GET")
                return View();

            if (!Util.ValidEmail(email))
                ModelState.AddModelError("email", "valid email required");
            DateTime bd;
            if (!Util.DateValid(dob, out bd))
                ModelState.AddModelError("dob", "valid birth date required");
            if (!ModelState.IsValid)
                return View();

            var q = from u in DbUtil.Db.Users
                    where u.Person.EmailAddress == email
                    where u.Person.BirthDay == bd.Day
                    where u.Person.BirthMonth == bd.Month
                    where u.Person.BirthYear == bd.Year
                    select u;
            var smtp = Util.Smtp();
            foreach (var user in q)
            {
                Util.Email(smtp, DbUtil.Settings("AdminMail", DbUtil.SystemEmailAddress), user.Name, email,
                    "bvcms forgot username",
                    @"Hi {0},
<p>Your username is: {1}</p>
<p>If you did not request this, please disregard this message.</p>
<p>Thanks,<br />
The bvCMS Team</p>
".Fmt(user.Name, user.Username));
                DbUtil.Db.SubmitChanges();
                Util.Email2(smtp, DbUtil.Settings("AdminMail", DbUtil.SystemEmailAddress), DbUtil.Settings("AdminMail", DbUtil.SystemEmailAddress), "bvcms user: {0} forgot username".Fmt(user.Name), "no content");
            }
            if (q.Count() == 0)
                Util.Email2(smtp, DbUtil.Settings("AdminMail", DbUtil.SystemEmailAddress), DbUtil.Settings("AdminMail", DbUtil.SystemEmailAddress), "bvcms unknown email: {0} forgot username".Fmt(email), "no content");

            return RedirectToAction("RequestUsername");

        }
        public ActionResult ForgotPassword(string username, string dob)
        {
            if (Request.HttpMethod.ToUpper() == "GET")
                return View();

            if (!username.HasValue())
                ModelState.AddModelError("username", "user name required");
            DateTime bd;
            if (!Util.DateValid(dob, out bd))
                ModelState.AddModelError("dob", "valid birth date required");
            if (!ModelState.IsValid)
                return View();

            var user = DbUtil.Db.Users.SingleOrDefault(u =>
                u.Username == username
                && u.Person.BirthDay == bd.Day
                && u.Person.BirthMonth == bd.Month
                && u.Person.BirthYear == bd.Year);
            var smtp = Util.Smtp();
            if (user != null)
            {
                user.ResetPasswordCode = Guid.NewGuid();
                var link = "{0}://{1}/Account/ResetPassword/{2}".Fmt(
                    Request.Url.Scheme, Request.Url.Authority, user.ResetPasswordCode.ToString());
                Util.Email(smtp, DbUtil.Settings("AdminMail", DbUtil.SystemEmailAddress), user.Name, user.Person.EmailAddress,
                    "bvcms password reset link",
                    @"Hi {0},
<p>You recently requested a new password.  To reset your password, follow the link below:<br />
<a href=""{1}"">{1}</a></p>
<p>If you did not request a new password, please disregard this message.</p>
<p>Thanks,<br />
The bvCMS Team</p>
".Fmt(user.Name, link));
                DbUtil.Db.SubmitChanges();
                Util.Email2(smtp, DbUtil.Settings("AdminMail", DbUtil.SystemEmailAddress), DbUtil.Settings("AdminMail", DbUtil.SystemEmailAddress), "bvcms user: {0} forgot password".Fmt(user.Name), "no content");
            }
            else
                Util.Email2(smtp, DbUtil.Settings("AdminMail", DbUtil.SystemEmailAddress), DbUtil.Settings("AdminMail", DbUtil.SystemEmailAddress), "bvcms unknown user: {0} forgot password".Fmt(username), "no content");

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
                return Content("Invalid code");

            CMSMembershipProvider.provider.AdminOverride = true;
            var mu = CMSMembershipProvider.provider.GetUser(user.Username, false);
            mu.UnlockUser();
            var newpassword = MembershipService.FetchPassword();
            mu.ChangePassword(mu.ResetPassword(), newpassword);
            CMSMembershipProvider.provider.AdminOverride = false;

            user.ResetPasswordCode = null;
            
            DbUtil.Db.SubmitChanges();
            var smtp = Util.Smtp();
            Util.Email(smtp, DbUtil.Settings("AdminMail", DbUtil.SystemEmailAddress), user.Name, user.Person.EmailAddress, 
                "bvcms new password",
                @"Hi {0},
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Exceptions result in password not being changed.")]
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
            if (String.IsNullOrEmpty(userName))
                ModelState.AddModelError("username", "You must specify a username.");
            if (String.IsNullOrEmpty(password))
                ModelState.AddModelError("password", "You must specify a password.");
            if (!MembershipService.ValidateUser(userName, password))
                ModelState.AddModelError("_FORM", "The username or password provided is incorrect.");

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
