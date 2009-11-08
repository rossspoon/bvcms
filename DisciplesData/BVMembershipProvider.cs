using System.Web.Security;
using System.Configuration.Provider;
using System.Collections.Specialized;
using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Web.Configuration;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.Text.RegularExpressions;
using UtilityExtensions;

namespace DiscData
{

    public sealed class BVMembershipProvider : MembershipProvider
    {
        public static BVMembershipProvider provider
        {
            get { return Membership.Provider as BVMembershipProvider; }
        }

        private int newPasswordLength = 8;

        private MachineKeySection machineKey;

        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            if (name == null || name.Length == 0)
                name = "CMSMembershipProvider";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "CMS Membership provider");
            }
            base.Initialize(name, config);

            pMaxInvalidPasswordAttempts = Convert.ToInt32(GetConfigValue(config["maxInvalidPasswordAttempts"], "5"));
            pPasswordAttemptWindow = Convert.ToInt32(GetConfigValue(config["passwordAttemptWindow"], "10"));
            pMinRequiredNonAlphanumericCharacters = Convert.ToInt32(GetConfigValue(config["minRequiredNonAlphanumericCharacters"], "1"));
            pMinRequiredPasswordLength = Convert.ToInt32(GetConfigValue(config["minRequiredPasswordLength"], "7"));
            pPasswordStrengthRegularExpression = Convert.ToString(GetConfigValue(config["passwordStrengthRegularExpression"], ""));
            pEnablePasswordReset = Convert.ToBoolean(GetConfigValue(config["enablePasswordReset"], "true"));
            pEnablePasswordRetrieval = Convert.ToBoolean(GetConfigValue(config["enablePasswordRetrieval"], "true"));
            pRequiresQuestionAndAnswer = Convert.ToBoolean(GetConfigValue(config["requiresQuestionAndAnswer"], "false"));
            pRequiresUniqueEmail = Convert.ToBoolean(GetConfigValue(config["requiresUniqueEmail"], "true"));

            string temp_format = config["passwordFormat"];
            if (temp_format == null)
                temp_format = "Hashed";

            switch (temp_format)
            {
                case "Hashed":
                    pPasswordFormat = MembershipPasswordFormat.Hashed;
                    break;
                case "Encrypted":
                    pPasswordFormat = MembershipPasswordFormat.Encrypted;
                    break;
                case "Clear":
                    pPasswordFormat = MembershipPasswordFormat.Clear;
                    break;
                default:
                    throw new ProviderException("Password format not supported.");
            }

            // Get encryption and decryption key information from the configuration.
            Configuration cfg =
              WebConfigurationManager.OpenWebConfiguration(System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
            machineKey = (MachineKeySection)cfg.GetSection("system.web/machineKey");

            if (machineKey.ValidationKey.Contains("AutoGenerate"))
                if (PasswordFormat != MembershipPasswordFormat.Clear)
                    throw new ProviderException("Hashed or Encrypted passwords " +
                                                "are not supported with auto-generated keys.");
        }

        private string GetConfigValue(string configValue, string defaultValue)
        {
            if (String.IsNullOrEmpty(configValue))
                return defaultValue;
            return configValue;
        }

        public override string ApplicationName { get { return "disc"; } set { } }

        private bool pEnablePasswordReset;
        public override bool EnablePasswordReset
        {
            get { return pEnablePasswordReset; }
        }
        private bool pEnablePasswordRetrieval;
        public override bool EnablePasswordRetrieval
        {
            get { return pEnablePasswordRetrieval; }
        }
        private bool pRequiresQuestionAndAnswer;
        public override bool RequiresQuestionAndAnswer
        {
            get { return pRequiresQuestionAndAnswer; }
        }
        private bool pRequiresUniqueEmail;
        public override bool RequiresUniqueEmail
        {
            get { return pRequiresUniqueEmail; }
        }
        private int pMaxInvalidPasswordAttempts;
        public override int MaxInvalidPasswordAttempts
        {
            get { return pMaxInvalidPasswordAttempts; }
        }
        private int pPasswordAttemptWindow;
        public override int PasswordAttemptWindow
        {
            get { return pPasswordAttemptWindow; }
        }
        private MembershipPasswordFormat pPasswordFormat;
        public override MembershipPasswordFormat PasswordFormat
        {
            get { return pPasswordFormat; }
        }
        private int pMinRequiredNonAlphanumericCharacters;
        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return pMinRequiredNonAlphanumericCharacters; }
        }
        private int pMinRequiredPasswordLength;
        public override int MinRequiredPasswordLength
        {
            get { return pMinRequiredPasswordLength; }
        }
        private string pPasswordStrengthRegularExpression;
        public override string PasswordStrengthRegularExpression
        {
            get { return pPasswordStrengthRegularExpression; }
        }
        public bool AdminOverride = false;
        public override bool ChangePassword(string username, string oldPwd, string newPwd)
        {
            var Db = new DiscDataContext(Util.ConnectionStringDisc);
            if (!ValidateUser(username, oldPwd))
                return false;
            var args = new ValidatePasswordEventArgs(username, newPwd, true);

            OnValidatingPassword(args);
            if (args.Cancel)
                if (args.FailureInformation != null)
                    throw args.FailureInformation;
                else
                    throw new MembershipPasswordException("Change password canceled due to new password validation failure.");

            if (!AdminOverride)
            {
                if (newPwd.Length < MinRequiredPasswordLength)
                    throw new ArgumentException("Password must contain at least {0} chars".Fmt(MinRequiredPasswordLength));
                int nonalphas = 0;
                for (int i = 0; i < newPwd.Length; i++)
                    if (!char.IsLetterOrDigit(newPwd, i))
                        nonalphas++;
                if (nonalphas < this.MinRequiredNonAlphanumericCharacters)
                    throw new ArgumentException("Password needs at least {0} non-alphanumeric chars".Fmt(MinRequiredNonAlphanumericCharacters));
                if (this.PasswordStrengthRegularExpression.Length > 0 && !Regex.IsMatch(newPwd, this.PasswordStrengthRegularExpression))
                    throw new ArgumentException("Password not strong enough");
            }

            var user = Db.Users.Single(u => u.Username == username);
            user.Password = EncodePassword(newPwd);
            user.LastPasswordChangedDate = DateTime.Now;
            Db.SubmitChanges();
            return true;
        }

        public override bool ChangePasswordQuestionAndAnswer(string username,
                      string password,
                      string newPwdQuestion,
                      string newPwdAnswer)
        {
            var Db = new DiscDataContext(Util.ConnectionStringDisc);
            if (!ValidateUser(username, password))
                return false;
            var user = Db.Users.Single(u => u.Username == username);
            user.PasswordQuestion = newPwdQuestion;
            user.PasswordAnswer = newPwdAnswer;
            Db.SubmitChanges();
            return true;
        }

        public override MembershipUser CreateUser(string username,
                 string password,
                 string email,
                 string passwordQuestion,
                 string passwordAnswer,
                 bool isApproved,
                 object providerUserKey,
                 out MembershipCreateStatus status)
        {
            var Db = new DiscDataContext(Util.ConnectionStringDisc);
            var args = new ValidatePasswordEventArgs(username, password, true);
            OnValidatingPassword(args);
            if (args.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }
            if (RequiresUniqueEmail && GetUserNameByEmail(email) != "")
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }
            var u = GetUser(username, false);
            if (u == null)
            {
                var createDate = DateTime.Now;
                var user = new User
                {
                    Username = username,
                    EmailAddress = email,
                    Password = EncodePassword(password),
                    PasswordQuestion = passwordQuestion,
                    PasswordAnswer = EncodePassword(passwordAnswer),
                    IsApproved = isApproved,
                    Comment = "",
                    CreationDate = createDate,
                    LastPasswordChangedDate = createDate,
                    LastActivityDate = createDate,
                    IsLockedOut = false,
                    LastLockedOutDate = createDate,
                    FailedPasswordAttemptCount = 0,
                    FailedPasswordAttemptWindowStart = createDate,
                    FailedPasswordAnswerAttemptCount = 0,
                    FailedPasswordAnswerAttemptWindowStart = createDate,
                };
                Db.Users.InsertOnSubmit(user);
                Db.SubmitChanges();
                status = MembershipCreateStatus.Success;
                return GetUser(username, false);
            }
            else
                status = MembershipCreateStatus.DuplicateUserName;
            return null;
        }

        public User NewUser(
                string username,
                string password,
                string email,
                bool isApproved,
                int? PeopleId)
        {
            var args = new ValidatePasswordEventArgs(username, password, true);
            OnValidatingPassword(args);
            if (args.Cancel)
                return null;
            var u = GetUser(username, false);
            if (u == null)
                return MakeNewUser(username, EncodePassword(password), email, isApproved, PeopleId);
            return null;
        }

        public static User MakeNewUser(string username, string password, string email, bool isApproved, int? PeopleId)
        {
            var createDate = DateTime.Now;
            var user = new User
            {
                PeopleId = PeopleId,
                EmailAddress = email,
                Username = username,
                Password = password,
                IsApproved = isApproved,
                Comment = "",
                CreationDate = createDate,
                LastPasswordChangedDate = createDate,
                LastActivityDate = createDate,
                IsLockedOut = false,
                LastLockedOutDate = createDate,
                FailedPasswordAttemptCount = 0,
                FailedPasswordAttemptWindowStart = createDate,
                FailedPasswordAnswerAttemptCount = 0,
                FailedPasswordAnswerAttemptWindowStart = createDate,
            };
            var Db = new DiscDataContext(Util.ConnectionStringDisc);
            Db.Users.InsertOnSubmit(user);
            Db.SubmitChanges();
            return user;
        }
        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            var Db = new DiscDataContext(Util.ConnectionStringDisc);
            var user = Db.Users.SingleOrDefault(u => u.Username == username);
            Db.UserRoles.DeleteAllOnSubmit(user.UserRoles);
            Db.Users.DeleteOnSubmit(user);
            Db.SubmitChanges();
            return true;
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            var Db = new DiscDataContext(Util.ConnectionStringDisc);
            var users = new MembershipUserCollection();
            var q = Db.Users.AsQueryable();
            totalRecords = q.Count();
            q = q.OrderBy(u => u.Username).Skip(pageIndex * pageSize).Take(pageSize);
            foreach (var u in q)
                users.Add(GetMu(u));
            return users;
        }

        public override int GetNumberOfUsersOnline()
        {
            var Db = new DiscDataContext(Util.ConnectionStringDisc);
            var onlineSpan = new TimeSpan(0, Membership.UserIsOnlineTimeWindow, 0);
            var compareTime = DateTime.Now.Subtract(onlineSpan);
            return Db.Users.Count(u => u.LastActivityDate > compareTime);
        }

        public override string GetPassword(string username, string answer)
        {
            var Db = new DiscDataContext(Util.ConnectionStringDisc);
            if (!EnablePasswordRetrieval)
                throw new ProviderException("Password Retrieval Not Enabled.");

            if (PasswordFormat == MembershipPasswordFormat.Hashed)
                throw new ProviderException("Cannot retrieve Hashed passwords.");

            var user = Db.Users.SingleOrDefault(u => u.Username == username);
            if (user != null)
            {
                if (user.IsLockedOut)
                    throw new MembershipPasswordException("The supplied user is locked out.");
            }
            else
                throw new MembershipPasswordException("The supplied user name is not found.");

            if (RequiresQuestionAndAnswer && !CheckPassword(answer, user.PasswordAnswer))
            {
                UpdateFailureCount(user, "passwordAnswer");
                throw new MembershipPasswordException("Incorrect password answer.");
            }
            if (PasswordFormat == MembershipPasswordFormat.Encrypted)
                return UnEncodePassword(user.Password);
            return user.Password;
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            var Db = new DiscDataContext(Util.ConnectionStringDisc);
            var u = Db.Users.SingleOrDefault(user => user.Username == username);
            if (u != null)
            {
                MembershipUser mu = GetMu(u);
                if (userIsOnline)
                {
                    u.LastActivityDate = DateTime.Now;
                    string host = HttpContext.Current.Request.Url.Host;
                    if (host.Length > 98)
                        host = host.Substring(0, 98);
                    u.Host = host;
                    Db.SubmitChanges();
                }
                return mu;
            }
            return null;
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            var Db = new DiscDataContext(Util.ConnectionStringDisc);
            var u = Db.Users.SingleOrDefault(user =>
                user.UserId == providerUserKey.ToInt());
            if (u != null)
            {
                MembershipUser mu = GetMu(u);
                if (userIsOnline)
                {
                    u.LastActivityDate = DateTime.Now;
                    string host = HttpContext.Current.Request.Url.Host;
                    if (host.Length > 98)
                        host = host.Substring(0, 98);
                    u.Host = host;
                    Db.SubmitChanges();
                }
                return mu;
            }
            return null;
        }

        private MembershipUser GetMu(User u)
        {
            return new MembershipUser(this.Name,
            u.Username,
            u.UserId,
            u.EmailAddress,
            u.PasswordQuestion,
            u.Comment,
            u.IsApproved,
            u.IsLockedOut,
            u.CreationDate.Value,
            u.LastLoginDate ?? new DateTime(),
            u.LastActivityDate ?? new DateTime(),
            u.LastPasswordChangedDate ?? new DateTime(),
            u.LastLockedOutDate ?? new DateTime());
        }
        public override bool UnlockUser(string username)
        {
            var Db = new DiscDataContext(Util.ConnectionStringDisc);
            var u = Db.Users.SingleOrDefault(user => user.Username == username);
            if (u != null)
            {
                u.LastLockedOutDate = DateTime.Now;
                u.IsLockedOut = false;
                Db.SubmitChanges();
            }
            return u != null;
        }

        public override string GetUserNameByEmail(string email)
        {
            var Db = new DiscDataContext(Util.ConnectionStringDisc);
            return Db.Users.Single(u => u.EmailAddress == email).Username;
        }

        public override string ResetPassword(string username, string answer)
        {
            var Db = new DiscDataContext(Util.ConnectionStringDisc);
            if (!EnablePasswordReset)
                throw new NotSupportedException("Password reset is not enabled.");

            var user = Db.Users.SingleOrDefault(u => u.Username == username);
            if (user == null)
                throw new MembershipPasswordException("The supplied user name is not found.");

            if (answer == null && RequiresQuestionAndAnswer)
            {
                UpdateFailureCount(user, "passwordAnswer");
                throw new ProviderException("Password answer required for password reset.");
            }
            string newPassword =
              Membership.GeneratePassword(newPasswordLength, MinRequiredNonAlphanumericCharacters);

            var args = new ValidatePasswordEventArgs(username, newPassword, true);

            OnValidatingPassword(args);

            if (args.Cancel)
                if (args.FailureInformation != null)
                    throw args.FailureInformation;
                else
                    throw new MembershipPasswordException("Reset password canceled due to password validation failure.");

            if (user.IsLockedOut)
                throw new MembershipPasswordException("The supplied user is locked out.");

            if (RequiresQuestionAndAnswer && !CheckPassword(answer, user.PasswordAnswer))
            {
                UpdateFailureCount(user, "passwordAnswer");
                throw new MembershipPasswordException("Incorrect password answer.");
            }
            user.Password = EncodePassword(newPassword);
            user.LastPasswordChangedDate = DateTime.Now;
            Db.SubmitChanges();
            return newPassword;
        }

        public override void UpdateUser(MembershipUser user)
        {
            var Db = new DiscDataContext(Util.ConnectionStringDisc);
            var u = Db.Users.SingleOrDefault(us => us.Username == user.UserName);
            u.IsApproved = user.IsApproved;
            u.EmailAddress = user.Email;
            u.Comment = user.Comment;
            Db.SubmitChanges();
        }
 
        public override bool ValidateUser(string username, string password)
        {
            var Db = new DiscDataContext(Util.ConnectionStringDisc);
            var user = Db.Users.SingleOrDefault(u => u.Username == username);
            if (user == null)
                return false;
            if (CheckPassword(password, user.Password))
                if (user.IsApproved)
                {
                    user.LastLoginDate = DateTime.Now;
                    Db.SubmitChanges();
                    return true;
                }
            UpdateFailureCount(user, "password");
            return false;
        }

        private void UpdateFailureCount(User user, string failureType)
        {
            var Db = new DiscDataContext(Util.ConnectionStringDisc);
            DateTime windowStart = new DateTime();
            int failureCount = 0;
            if (failureType == "password")
            {
                failureCount = user.FailedPasswordAttemptCount;
                windowStart = user.FailedPasswordAttemptWindowStart ?? DateTime.Now;
            }
            else if (failureType == "passwordAnswer")
            {
                failureCount = user.FailedPasswordAnswerAttemptCount;
                windowStart = user.FailedPasswordAnswerAttemptWindowStart ?? DateTime.Now;
            }
            DateTime windowEnd = windowStart.AddMinutes(PasswordAttemptWindow);
            if (failureCount == 0 || DateTime.Now > windowEnd)
            {
                if (failureType == "password")
                {
                    user.FailedPasswordAttemptCount = 1;
                    user.FailedPasswordAttemptWindowStart = DateTime.Now;
                }
                else if (failureType == "passwordAnswer")
                {
                    user.FailedPasswordAnswerAttemptCount = 1;
                    user.FailedPasswordAnswerAttemptWindowStart = DateTime.Now;
                }
            }
            else if (failureCount++ >= MaxInvalidPasswordAttempts)
            {
                user.IsLockedOut = true;
                user.LastLockedOutDate = DateTime.Now;
            }
            else if (failureType == "password")
                user.FailedPasswordAttemptCount = failureCount;
            else if (failureType == "passwordAnswer")
                user.FailedPasswordAnswerAttemptCount = failureCount;
            Db.SubmitChanges();
        }

        private bool CheckPassword(string password, string dbpassword)
        {
            string pass1 = password;
            string pass2 = dbpassword;
            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Encrypted:
                    pass2 = UnEncodePassword(dbpassword);
                    break;
                case MembershipPasswordFormat.Hashed:
                    pass1 = EncodePassword(password);
                    break;
                default:
                    break;
            }
            return pass1 == pass2;
        }

        private string EncodePassword(string password)
        {
            if (!password.HasValue())
                return "";
            string encodedPassword = password;
            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Clear:
                    break;
                case MembershipPasswordFormat.Encrypted:
                    encodedPassword =
                      Convert.ToBase64String(EncryptPassword(Encoding.Unicode.GetBytes(password)));
                    break;
                case MembershipPasswordFormat.Hashed:
                    var hash = new HMACSHA1();
                    hash.Key = HexToByte(machineKey.ValidationKey);
                    encodedPassword =
                      Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(password)));
                    break;
                default:
                    throw new ProviderException("Unsupported password format.");
            }
            return encodedPassword;
        }

        private string UnEncodePassword(string encodedPassword)
        {
            string password = encodedPassword;
            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Clear:
                    break;
                case MembershipPasswordFormat.Encrypted:
                    password =
                      Encoding.Unicode.GetString(DecryptPassword(Convert.FromBase64String(password)));
                    break;
                case MembershipPasswordFormat.Hashed:
                    throw new ProviderException("Cannot unencode a hashed password.");
                default:
                    throw new ProviderException("Unsupported password format.");
            }
            return password;
        }

        private byte[] HexToByte(string hexString)
        {
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            var Db = new DiscDataContext(Util.ConnectionStringDisc);
            var users = new MembershipUserCollection();
            var q = from u in Db.Users select u;

            bool left = usernameToMatch.StartsWith("%");
            bool right = usernameToMatch.EndsWith("%");
            usernameToMatch = usernameToMatch.Trim('%');
            if (left && right)
                q = q.Where(u => u.Username.Contains(usernameToMatch));
            else if (left)
                q = q.Where(u => u.Username.EndsWith(usernameToMatch));
            else if (right)
                q = q.Where(u => u.Username.StartsWith(usernameToMatch));

            totalRecords = q.Count();
            q = q.OrderBy(u => u.Username).Skip(pageIndex * pageSize).Take(pageSize);
            foreach (var u in q)
                users.Add(GetMu(u));
            return users;
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            var Db = new DiscDataContext(Util.ConnectionStringDisc);
            var users = new MembershipUserCollection();
            var q = from u in Db.Users select u;

            bool left = emailToMatch.StartsWith("%");
            bool right = emailToMatch.EndsWith("%");
            emailToMatch = emailToMatch.Trim('%');
            if (left && right)
                q = q.Where(u => u.EmailAddress.Contains(emailToMatch));
            else if (left)
                q = q.Where(u => u.EmailAddress.EndsWith(emailToMatch));
            else if (right)
                q = q.Where(u => u.EmailAddress.StartsWith(emailToMatch));

            totalRecords = q.Count();
            q = q.OrderBy(u => u.Username).Skip(pageIndex * pageSize).Take(pageSize);
            foreach (var u in q)
                users.Add(GetMu(u));
            return users;
        }
        public void MustChangePassword(User user, bool tf)
        {
            var Db = new DiscDataContext(Util.ConnectionStringDisc);
            user.MustChangePassword = tf;
            Db.SubmitChanges();
        }
        public bool MustChangePassword(User user)
        {
            return user.MustChangePassword;
        }
        public bool UserMustChangePassword
        {
            get { return MustChangePassword(DbUtil.Db.CurrentUser); }
            set { MustChangePassword(DbUtil.Db.CurrentUser, value); }
        }
    }
}