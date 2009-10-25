using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel;
using System.Web;
using System.Net.Mail;
using System.Web.Security;
using UtilityExtensions;

namespace DiscData
{
    public partial class BlogComment
    {
        public string CommentHtml
        {
            get { return Util.SafeFormat(Comment); }
        }
        public string PosterName
        {
            get
            {
                var u = BlogPost.getUser(PosterId);
                return u.FirstName + " " + u.LastName;
            }
        }
        public string PosterEmail
        {
            get
            {
                var u = BlogPost.getUser(PosterId);
                return u.EmailAddress;
            }
        }
        public int CommentNumber { get; set; }
        public bool IsAuthor
        {
            get { return PosterId == this.BlogPost.PosterId; }
        }
        public bool IsPoster
        {
            get { return PosterId == DbUtil.Db.CurrentUser.UserId; }
        }
        public bool CanEdit
        {
            get { return BlogPost.Blog.IsBlogger || Roles.IsUserInRole("Administrator"); }
        }
        public void NotifyEmail()
        {
            var returnloc = "http://{0}/Blog/{1}.aspx".Fmt(HttpContext.Current.Request.Url.Authority, BlogPost.Id);
            var smtp = new SmtpClient();
            var n = 0;
            var stopemail = "http://{0}/StopNotifications.aspx?blog={1}&user="
                .Fmt(HttpContext.Current.Request.Url.Authority, this.BlogPost.BlogCached.Id);
            var from = new MailAddress("bbcms01@bellevue.org");
            var subject = "New comment posted regarding: {0}, from {1}".Fmt(BlogPost.Title, User.Username);
            var reply = new MailAddress(PosterEmail);
            foreach (var i in BlogPost.BlogCached.GetNotificationList())
            {
                var ma = Util.TryGetMailAddress(i.Value.Email, i.Value.Name);
                if (ma == null)
                    continue;
                var msg = new MailMessage(from, ma);
                msg.ReplyTo = reply;
                msg.Subject = subject;
                msg.Body = @"<br>--<br>View this comment online at: <a href=""{0}?user={1}#comments"">{0}</a>
<br>--<br>
Click <a href=""{2}"">here</a> to stop receiving notifications"
                        .Fmt(returnloc, i.Key, stopemail + i.Value.User);
                msg.IsBodyHtml = true;
                if (n % 20 == 0)
                    smtp = new SmtpClient();
      
                n++;
                var debug = false;
#if DEBUG
                debug = true;
#endif
                if (!debug)
                    smtp.Send(msg);
            }
        }

        public static BlogComment LoadById(int id)
        {
            return DbUtil.Db.BlogComments.Single(c => c.Id == id);
        }
    }
}
