using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CmsData;
using System.Net.Mail;
using System.Diagnostics;
using UtilityExtensions;

namespace Prayer.Controllers
{

    [HandleError]
    public class AccountController : Controller
    {

        // This constructor is used by the MVC framework to instantiate the controller using
        // the default forms authentication and membership providers.

        public AccountController()
            : this(null, null)
        {
        }

        // This constructor is not used by the MVC framework but is instead provided for ease
        // of unit testing this type. See the comments at the end of this file for more
        // information.
        public AccountController(IFormsAuthentication formsAuth, IMembershipService service)
        {
            FormsAuth = formsAuth ?? new FormsAuthenticationService();
            MembershipService = service ?? new AccountMembershipService();
        }

        public IFormsAuthentication FormsAuth
        {
            get;
            private set;
        }

        public IMembershipService MembershipService
        {
            get;
            private set;
        }

        public ActionResult LogOn()
        {
            return View();
        }
        public ActionResult NewUser(int? id)
        {
            var g = Group.LoadByName(STR_PrayerPartners);
            var u = DbUtil.Db.CurrentUser;
            if (id.HasValue && g.IsAdmin)
            {
                u = DbUtil.Db.Users.SingleOrDefault(uu => uu.UserId == id);
                ViewData["userid"] = u.UserId;
            }
            ViewData["username"] = u.Username;
            ViewData["password"] = u.Password;
            return View();
        }
        public ActionResult Passwords()
        {
            ViewData.Model = FetchPasswords();
            return View("Passwords");
        }
        private IEnumerable<string> FetchPasswords()
        {
            var rnd = new Random();
            for (var i = 0; i < 200; i++)
                yield return FetchPassword(rnd);
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
            DbUtil.Db.CurrentUser = DbUtil.Db.Users.SingleOrDefault(uu => uu.Username == userName);

            if (!String.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("Index", "Home");
        }

        public ActionResult LogOff()
        {
            FormsAuth.SignOut();
            DbUtil.Db.CurrentUser = new User { Username = Request.AnonymousID };
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register(int? id)
        {
            if (id.HasValue)
            {
                var p = CmsData.DbUtil.Db.LoadPersonById(id.Value);
                ViewData["first"] = p.PreferredName;
                ViewData["last"] = p.LastName;
                ViewData["email"] = p.EmailAddress;
                ViewData["birthday"] = p.DOB;
                ViewData["phone"] = p.HomePhone.FmtFone();
            }
            return View();
        }

        public static IQueryable<CmsData.Person> FindMember(string first, string last, string phone, string dob)
        {
            var bdt = DateTime.Parse(dob);
            phone = Util.GetDigits(phone);
            var q = from p in CmsData.DbUtil.Db.People
                    where (p.LastName.StartsWith(last) || p.MaidenName.StartsWith(last))
                            && (p.FirstName.StartsWith(first)
                            || p.NickName.StartsWith(first)
                            || p.MiddleName.StartsWith(first))
                    where p.CellPhone.Contains(phone)
                            || p.Family.HomePhone.Contains(phone)
                            || p.WorkPhone.Contains(phone)
                    where p.BirthDay == bdt.Day && p.BirthMonth == bdt.Month && p.BirthYear == bdt.Year
                    select p;
            return q;
        }

        public static User GetUserByPeopleId(int? PeopleId)
        {
            var q2 = from du in DbUtil.Db.Users
                     where du.PeopleId == PeopleId
                     let lastvisit = du.PageVisits.Max(pv => pv.VisitTime)
                     orderby lastvisit
                     select du;
            var u = q2.FirstOrDefault();
            return u;
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Register(string first, string last, string email, string birthday, string phone)
        {
            if (ValidateRegistration(first, last, email, birthday, phone))
            {
                var q = FindMember(first, last, phone, birthday);
                var count = q.Count();
                if (count > 1)
                    ModelState.AddModelError("_FORM", "More than one match, sorry");
                else if (count == 0)
                    ModelState.AddModelError("_FORM", "Cannot find your church record");
                else
                {
                    var p = q.Single();
						
						if (GetOrCreateUserAccount(p, first, email))
                        return RedirectToAction("Index", "Signup"); // found existing user
                    else
                        return View("NewUser"); // created new user
                }
            }
            return View();
        }
        const string STR_PrayerPartners = "Prayer Partners";
        public bool GetOrCreateUserAccount(CmsData.Person p, string first, string email)
        {
            User u = GetUserByPeopleId(p.PeopleId);
            if (u == null)
            {
                u = new User
                {
                    FirstName = first.HasValue() ? first : p.FirstName,
                    LastName = p.LastName,
                    PeopleId = p.PeopleId,
                    Password = FetchPassword(new Random()),
                    Username = FetchUsername(p),
                    BirthDay = p.GetBirthdate(),
                    EmailAddress = email,
                    IsApproved = true,
                    LastActivityDate = DateTime.Now,
                    CreationDate = DateTime.Now,
                };
                DbUtil.Db.Users.InsertOnSubmit(u);
                DbUtil.Db.SubmitChanges();
                var g = Group.LoadByName(STR_PrayerPartners);
                g.SetMember(u, true);
                DbUtil.Db.SubmitChanges();
                ViewData["username"] = u.Username;
                ViewData["password"] = u.Password;

                WelcomeEmail(u);
                NewUserNotification(p, u, false /* not existing */);
                if (g.IsAdmin)
                    Response.Redirect("/Account/NewUser/" + u.UserId);
                FormsAuth.SignIn(u.Username, true /* remember me */);
                DbUtil.Db.CurrentUser = u;
                return false; // created NewUser
            }
            else
            {
                var g = Group.LoadByName(STR_PrayerPartners);
                if (!g.IsUserMember(u))
                {
                    g.SetMember(u, true);
                    DbUtil.Db.SubmitChanges();
                    NewUserNotification(p, u, true /* existing */);
                }
                if (g.IsAdmin)
                    Response.Redirect("/Signup/Index/" + u.UserId);
                FormsAuth.SignIn(u.Username, true /* remember me */);
                DbUtil.Db.CurrentUser = u;
                return true; // was found
            }
        }
        private void WelcomeEmail(User u)
        {
            Email(u.Name, u.EmailAddress, "Your account on prayer.bellevue.org", @"Hi {0},<br/>
You now have an account setup on http://prayer.bellevue.org.<br/>
We'll send you more info about how you can use the site later.<br/>
In the meantime, you can get back to the Prayer Times page to make changes using the following credentials:
<blockquote>
<table>
<tr><td>Name:</td><td><b>{1}</b></td></tr>
<tr><td>Username:</td><td><b>{2}</b></td></tr>
<tr><td>Password:</td><td><b>{3}</b></td></tr>
</table>
</blockquote>
Thanks for praying!<br/>
Bellevue Prayer ministry".Fmt(u.Person.PreferredName, u.Name, u.Username, u.Password));
        }
        private void NewUserNotification(CmsData.Person p, User u, bool existing)
        {
            var g = Group.LoadByName("Prayer Partners");
            var a = g.GetUsersInRole(GroupType.Admin);
            foreach (var admin in a)
                Email(admin.Name, admin.EmailAddress, "New account on prayer.bellevue.org", @"
<a href='http://cms.bellevue.org/Person.aspx?id={0}'>{1}</a>({4}) registered at {2} on http://prayer.bellevue.org.<br/>
{3}"
                .Fmt(u.PeopleId, u.Name, DateTime.Now, existing ? "(existing user)" : "(welcome sent)",
					p.MemberStatusId == (int)CmsData.Person.MemberStatusCode.Member ? "member" : "non-member"));
        }
        private string FetchPassword(Random rnd)
        {
            var n = DbUtil.Db.Words.Select(w => w.WordX).Count();
            var r1 = rnd.Next(1, n);
            var r2 = rnd.Next(1, n);
            var q = from w in DbUtil.Db.Words
                    where w.N == r1 || w.N == r2
                    select w.WordX;
            var a = q.ToArray();
            return a[0] + "." + a[1];
        }
        private string FetchUsername(CmsData.Person p)
        {
            var username = p.FirstName.ToLower() + p.LastName.ToLower()[0];
            var uname = username;
            var i = 1;
            while (DbUtil.Db.Users.SingleOrDefault(u => u.Username == uname) != null)
                uname = username + i++;
            return uname;
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
                return View();
            try
            {
                if (MembershipService.ChangePassword(User.Identity.Name, currentPassword, newPassword))
                    return RedirectToAction("ChangePasswordSuccess");
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
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity is WindowsIdentity)
                throw new InvalidOperationException("Windows authentication is not supported.");
        }

        private bool ValidateChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            if (String.IsNullOrEmpty(currentPassword))
                ModelState.AddModelError("currentPassword", "You must specify a current password.");
            if (newPassword == null || newPassword.Length < MembershipService.MinPasswordLength)
                ModelState.AddModelError("newPassword",
                        String.Format(CultureInfo.CurrentCulture,
                             "You must specify a new password of {0} or more characters.",
                             MembershipService.MinPasswordLength));
            if (!String.Equals(newPassword, confirmPassword, StringComparison.Ordinal))
                ModelState.AddModelError("_FORM", "The new password and confirmation password do not match.");
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

        private bool ValidateRegistration(string first, string last, string email, string birthday, string phone)
        {
            if (String.IsNullOrEmpty(first))
                ModelState.AddModelError("first", "You must specify a First name.");
            if (String.IsNullOrEmpty(last))
                ModelState.AddModelError("last", "You must specify a Last name.");
            if (!Util.ValidEmail(email))
                ModelState.AddModelError("email", "You must specify a valid email address.");
            var d = phone.GetDigits().Length;
            var g = Group.LoadByName("Prayer Partners");
            if (!g.IsAdmin && (String.IsNullOrEmpty(phone) || (d != 7 && d != 10)))
                ModelState.AddModelError("phone", "You must specify a phone number (7 or 10 digits).");
            DateTime dt;
            if (!DateTime.TryParse(birthday, out dt))
                ModelState.AddModelError("birthday", "You must specifiy a valid birth date.");
            var maxdt = DateTime.Now.AddYears(-16);
            if (!g.IsAdmin && dt > maxdt)
                ModelState.AddModelError("birthday", "You must be at least 16 years old.");
            return ModelState.IsValid;
        }
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
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
        public static void Email(string name, string addr, string subject, string message)
        {
            var InDebug = false;
#if DEBUG
            InDebug = true;
#endif
            if (InDebug)
                return;
            var smtp = new SmtpClient();
            var from = new MailAddress("prayer@bellevue.org");
            var ma = Util.TryGetMailAddress(addr, name);
            if (ma == null)
                return;
            var msg = new MailMessage(from, ma);
            msg.Subject = subject;
            msg.Body = message;
            msg.IsBodyHtml = true;
            smtp.Send(msg);
        }
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

    public interface IMembershipService
    {
        int MinPasswordLength { get; }

        bool ValidateUser(string userName, string password);
        MembershipCreateStatus CreateUser(string userName, string password, string email);
        bool ChangePassword(string userName, string oldPassword, string newPassword);
    }

    public class AccountMembershipService : IMembershipService
    {
        private MembershipProvider _provider;

        public AccountMembershipService()
            : this(null)
        {
        }

        public AccountMembershipService(MembershipProvider provider)
        {
            _provider = provider ?? Membership.Provider;
        }

        public int MinPasswordLength
        {
            get
            {
                return _provider.MinRequiredPasswordLength;
            }
        }

        public bool ValidateUser(string userName, string password)
        {
            return _provider.ValidateUser(userName, password);
        }

        public MembershipCreateStatus CreateUser(string userName, string password, string email)
        {
            MembershipCreateStatus status;
            _provider.CreateUser(userName, password, email, null, null, true, null, out status);
            return status;
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            MembershipUser currentUser = _provider.GetUser(userName, true /* userIsOnline */);
            return currentUser.ChangePassword(oldPassword, newPassword);
        }
    }
}
