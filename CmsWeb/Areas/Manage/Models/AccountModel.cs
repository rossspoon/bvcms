using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using CmsData;
using UtilityExtensions;
using System.IO;
using System.Data.Linq;

namespace CmsWeb.Models
{
    public class AccountModel
    {
        public string GetNewFileName(string path)
        {
            while (System.IO.File.Exists(path))
            {
                var ext = Path.GetExtension(path);
                var fn = Path.GetFileNameWithoutExtension(path) + "a" + ext;
                var dir = Path.GetDirectoryName(path);
                path = Path.Combine(dir, fn);
            }
            return path;
        }
        public string CleanFileName(string fn)
        {
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
            return fn;
        }
        public static string GetValidToken(string otltoken)
        {
            if (!otltoken.HasValue())
                return null;
            var guid = otltoken.ToGuid();
            if (guid == null)
                return null;
            var ot = DbUtil.Db.OneTimeLinks.SingleOrDefault(oo => oo.Id == guid.Value);
            if (ot == null)
                return null;
            if (ot.Used)
                return null;
            if (ot.Expires.HasValue && ot.Expires < DateTime.Now)
                return null;
            ot.Used = true;
            DbUtil.Db.SubmitChanges();
            return ot.Querystring;
        }
        public static object AuthenticateLogon(string userName, string password, string url)
        {
            var q = DbUtil.Db.Users.Where(uu =>
                uu.Username == userName ||
                uu.Person.EmailAddress == userName ||
                uu.Person.EmailAddress2 == userName
                );

            var impersonating = false;
            User user = null;
            int n = q.Count();
			int failedpasswordcount = 0;
            foreach (var u in q.ToList())
                if (u.TempPassword != null && password == u.TempPassword)
                {
                    u.TempPassword = null;
                    if (password == "bvcms") // set this up so Admin/bvcms works until password is changed
                    {
                        u.Password = "";
                        u.MustChangePassword = true;
                    }
                    u.IsLockedOut = false;
                    DbUtil.Db.SubmitChanges();
                    user = u;
                    break;
                }
                else if (password == DbUtil.Db.Setting("ImpersonatePassword", Guid.NewGuid().ToString()))
                {
                    user = u;
                    impersonating = true;
                    break;
                }
				else if (Membership.Provider.ValidateUser(u.Username, password))
				{
					DbUtil.Db.Refresh(RefreshMode.OverwriteCurrentValues, u);	
					user = u;
					break;
				}
				else
				{
					failedpasswordcount = Math.Max(failedpasswordcount, u.FailedPasswordAttemptCount);
				}
			

			var max = CMSMembershipProvider.provider.MaxInvalidPasswordAttempts;
            string problem = "There is a problem with your username and password combination. If you are using your email address, it must match the one we have on record. Try again or use one of the links below.";
            if (user == null && n > 0)
            {
                NotifyAdmins("failed password #{2} by {0} on {1}"
					.Fmt(userName, url, failedpasswordcount),
                        "{0} tried to login at {1} but got the password wrong"
                            .Fmt(userName, Util.Now));
				if (failedpasswordcount == max)
					return "Your account has been locked out for too many failed attempts, use the forgot password link, or notify an Admin";
                return problem;
            }
            else if (user == null)
            {
                NotifyAdmins("attempt to login by non-user {0} on {1}".Fmt(userName, url),
                        "{0} tried to login at {1} but is not a user"
                            .Fmt(userName, Util.Now));
                return problem;
            }
            else if (user.IsLockedOut)
            {
                NotifyAdmins("{0} locked out #{2} on {1}"
					.Fmt(userName, url, user.FailedPasswordAttemptCount),
                        "{0} tried to login at {1} but is locked out"
                            .Fmt(userName, Util.Now));
				return "Your account has been locked out for {0} failed attempts in a short window of time, please use the forgot password link or notify an Admin".Fmt(max);
            }
            else if (!user.IsApproved)
            {
                NotifyAdmins("unapproved user {0} logging in on {1}"
					.Fmt(userName, url),
                        "{0} tried to login at {1} but is not approved"
                            .Fmt(userName, Util.Now));
                return problem;
            }
            if (impersonating == true)
            {
                if (user.Roles.Contains("Finance"))
                {
                    NotifyAdmins("cannot impersonate Finance user {0} on {1}"
						.Fmt(userName, url),
                            "{0} tried to login at {1}".Fmt(userName, Util.Now));
                    return problem;
                }
                DbUtil.Db.EmailRedacted(DbUtil.AdminMail,
                    CMSRoleProvider.provider.GetDevelopers(),
                    "{0} is being impersonated on {1}".Fmt(user.Username, Util.Host), 
					Util.Now.ToString());
            }
			DbUtil.LogActivity("User {0} logged in".Fmt(user.Username));
            return user;
        }
		public static object AuthenticateLogon(string userName, string password, HttpSessionStateBase Session, HttpRequestBase Request)
		{
			var o = AuthenticateLogon(userName, password, Request.Url.OriginalString);
			if (o is User)
			{
				var user = o as User;
				FormsAuthentication.SetAuthCookie(user.Username, false);
				SetUserInfo(user.Username, Session);
				Util.FormsBasedAuthentication = true;
				DbUtil.LogActivity("User {0} logged in".Fmt(user.Username));
				return user;
			}
			return o;
		}
		private static void NotifyAdmins(string subject, string message)
        {
            DbUtil.Db.EmailRedacted(DbUtil.AdminMail,
                CMSRoleProvider.provider.GetAdmins(), subject, message);
        }
        public static void SetUserInfo(string username, System.Web.SessionState.HttpSessionState Session)
        {
            var u = SetUserInfo(username);
            if (u == null)
                return;
            Session["ActivePerson"] = u.Name;
        }
        public static User SetUserInfo(string username, HttpSessionStateBase Session)
        {
            var u = SetUserInfo(username);
            if (u == null)
                return null;
            Session["ActivePerson"] = u.Name;
            return u;
        }
        private static User SetUserInfo(string username)
        {
			var i = (from u in DbUtil.Db.Users
					 where u.Username == username
					 select new { u, u.Person.PreferredName }).Single();
            //var u = DbUtil.Db.Users.SingleOrDefault(us => us.Username == username);
            if (i.u != null)
            {
                Util.UserId = i.u.UserId;
                Util.UserPeopleId = i.u.PeopleId;
                Util2.CurrentPeopleId = i.u.PeopleId.Value;
				Util.UserPreferredName = i.PreferredName;
				Util.UserFullName = i.u.Name;
            }
            return i.u;
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
        public static User AddUser(int id)
        {
            var p = DbUtil.Db.People.Single(pe => pe.PeopleId == id);
            CMSMembershipProvider.provider.AdminOverride = true;
            var user = MembershipService.CreateUser(DbUtil.Db, id);
            CMSMembershipProvider.provider.AdminOverride = false;
            user.MustChangePassword = false;
            DbUtil.Db.SubmitChanges();
            return user;
        }
        public static void SendNewUserEmail(string username)
        {
            var user = DbUtil.Db.Users.Single(u => u.Username == username);
            var body = DbUtil.Content("NewUserWelcome",
                    @"Hi {name},
<p>You have a new account on our Church Management System. 
Click on your username below to set your password and login to the system.</p>
<blockquote>
<h3>Your username is: <b><a href=""{link}"">{username}</a></h3>
</blockquote>");
            body = body.Replace("{name}", user.Name);
            body = body.Replace("{cmshost}", DbUtil.Db.Setting("DefaultHost", DbUtil.Db.Host));
            body = body.Replace("{username}", user.Username);
            user.ResetPasswordCode = Guid.NewGuid();
            user.ResetPasswordExpires = DateTime.Now.AddDays(1);
            var link = Util.ServerLink("/Account/SetPassword/" + user.ResetPasswordCode.ToString());
            body = body.Replace("{link}", link);
            DbUtil.Db.SubmitChanges();
            DbUtil.Db.EmailRedacted(DbUtil.AdminMail, user.Person, "New user welcome", body);
        }
        public static string ForgotPassword(string username)
        {
            var obscuredemail = Util.ObscureEmail("abcdef@gmail.com");
            var user = DbUtil.Db.Users.SingleOrDefault(u => u.Username == username);
            if (user != null)
            {
                user.ResetPasswordCode = Guid.NewGuid();
                user.ResetPasswordExpires = DateTime.Now.AddDays(1);
                var link = Util.ServerLink("/Account/SetPassword/" + user.ResetPasswordCode.ToString());
                DbUtil.Db.SubmitChanges();
                DbUtil.Db.EmailRedacted(DbUtil.AdminMail, user.Person, "bvcms password reset link", @"Hi {0},
<p>You recently requested a new password.  To reset your password, click your username below:</p>
<blockquote><a href=""{1}"">{2}</a></blockquote>
<p>If you did not request a new password, please disregard this message.</p>
<p>Thanks,<br />
The BVCMS Team</p>
".Fmt(user.Name, link, username));
                return Util.ObscureEmail(user.Person.EmailAddress);
            }
            return null;
        }
    }
}
