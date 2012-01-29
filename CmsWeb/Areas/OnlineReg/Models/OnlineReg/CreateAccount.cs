using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using System.Text;
using UtilityExtensions;

namespace CmsWeb.Models
{
    public partial class OnlineRegPersonModel
    {
        public User CreateAccount()
        {
            var Db = DbUtil.Db;
            if (!person.EmailAddress.HasValue())
                CannotCreateAccount = true;
            else if (person.Users.Count() > 0) // already have account
            {
                SawExistingAccount = true;
                var user = person.Users.OrderByDescending(uu => uu.LastActivityDate).First();

                var message = DbUtil.Content("ExistingUserConfirmation", 
                        @"Hi {name},
<p>We noticed you already have an account in our church database.</p>
<p>You can login at <a href=""{host}"">{host}</a>. 
And if you can't remember your password or username, click the Forgot Password link when you get there. 
Note: you will need to know your username for this to work. If you do not know your username, then please click forgot username first.
This will send you a link you can use to reset your password.</p>
<p>You can use your account to help us maintain your correct address, email and phone numbers.
Just login to <a href=""{host}"">{host}</a> and you will be taken to your record where you can make corrections if needed.</p>
<p>Thank You</p>
");
                message = message
                    .Replace("{name}", person.Name)
                    .Replace("{host}", DbUtil.Db.CmsHost);

                Db.Email(DbUtil.AdminMail, person, "Account information for " + Db.Host, message);
            }
            else
            {
                CreatedAccount = true;
                var user = MembershipService.CreateUser(Db, person.PeopleId);
                AccountModel.SendNewUserEmail(user.Username);
                return user;
            }
            return null;
        }
        public void SendOneTimeLink(string from, string url, string subject, string body)
        {
            var ot = new OneTimeLink 
            { 
                Id = Guid.NewGuid(),
                Querystring = "{0},{1}".Fmt(divid ?? orgid ?? masterorgid , PeopleId) 
            };
            var Db = DbUtil.Db;
            Db.OneTimeLinks.InsertOnSubmit(ot);
            Db.SubmitChanges();

            var message = body.Replace("{url}", url + ot.Id.ToCode());
            message = message.Replace("{name}", person.Name);
            message = message.Replace("{first}", person.PreferredName);

            Db.Email(from, person, subject, message);
        }
    }
}