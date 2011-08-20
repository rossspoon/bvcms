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
        public void CreateAccount()
        {
            var Db = DbUtil.Db;
            if (!person.EmailAddress.HasValue())
                CannotCreateAccount = true;
            else if (person.Users.Count() > 0) // already have account
            {
                SawExistingAccount = true;
                var user = person.Users.OrderByDescending(uu => uu.LastActivityDate).First();

                var message = 
                        @"Hi {name},
<p>We noticed you already have an account in our church database.</p>
<p>You can login at <a href=""{host}"">{host}</a>. 
And if you can't remember your password or username, click the forgot password link when you get there. 
Note: you will need to know your username for this to work. If you do not know your username, then please click forgot username first.
This will send you a link you can use to reset your password.</p>
<p>You can use your account to help us maintain your correct address, email and phone numbers.
Just login to <a href=""{host}"">{host}</a> and you will be taken to your record where you can make corrections if needed.</p>
<p>Thank You</p>
";
                message = message
                    .Replace("{name}", person.Name)
                    .Replace("{host}", DbUtil.Db.CmsHost);

                Db.Email(DbUtil.AdminMail, person, "Account information for " + Db.Host, message);
            }
            else
            {
                CreatedAccount = true;
                var uname = MembershipService.FetchUsername(Db, person.PreferredName, person.LastName);
                var pword = MembershipService.FetchPassword(Db);
                var user = MembershipService.CreateUser(person.PeopleId, uname, pword);

                var gobackurl = HttpContext.Current.Session["gobackurl"] as string;
                Content c = null;
                if (gobackurl.HasValue())
                    c = DbUtil.Content("CreateAccountRegistration");
                else
                    c = DbUtil.Content("CreateAccountConfirmation");
                if (c == null)
                    c = new Content();

                var message = Util.PickFirst(c.Body,
                        @"Hi {name},
<p>We have created an account you for in our church database.</p>
<p>This will make it easier for you to do online registrations.
Just use this account next time you register online.</p>
<p>And this will allow you to help us maintain your correct address, email and phone numbers.
Just login to {host} and you will be taken to your record where you can make corrections if needed.</p>
<p>The following are the credentials you can use. Both the username and password are system generated.
</p>
<blockquote><table>
<tr><td>Username:</td><td><strong><span style=""font-family: courier new, courier, monospace"">{username}</span></strong></td></tr>
<tr><td>Password:</td><td><strong><span style=""font-family: courier new, courier, monospace"">{password}</span></strong></td></tr>
</table></blockquote>
<p>Thank You</p>
");
                message = message
                    .Replace("{name}", person.Name)
                    .Replace("{username}", uname)
                    .Replace("{password}", pword)
                    .Replace("{gobackurl}", gobackurl)
                    .Replace("{host}", DbUtil.Db.CmsHost);

                Db.EmailRedacted(DbUtil.AdminMail, person, "New account for " + Db.Host, message);
            }
        }
        public void SendOneTimeLink(string from, string url)
        {
            var ot = new OneTimeLink 
            { 
                Id = Guid.NewGuid(),
                Querystring = "{0},{1}".Fmt(divid, PeopleId) 
            };
            var Db = DbUtil.Db;
            Db.OneTimeLinks.InsertOnSubmit(ot);
            Db.SubmitChanges();
            var c = DbUtil.Content("OneTimeConfirmation");
            if (c == null)
                c = new Content();

            var message = Util.PickFirst(c.Body,
                    @"Hi {name},
<p>Here is your <a href=""{url}"">link</a> to manage your subscriptions. (note: it will only work once for security reasons)</p> ");
            message = message.Replace("{url}", url + ot.Id.ToCode());
            message = message.Replace("{name}", person.Name);
            message = message.Replace("{first}", person.PreferredName);

            Db.Email(from, person, "Manage Your Subscriptions", message);
        }
    }
}