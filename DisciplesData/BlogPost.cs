using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Net.Mail;
using System.Web;
using System.Threading;
using System.Linq;
using System.Web.Security;
using System.Text;
using UtilityExtensions;

namespace DiscData
{
    public partial class BlogPost
    {
        public Dictionary<string, BlogCategoryXref> GetBlogCategories()
        {
            var d = new Dictionary<string, BlogCategoryXref>();
            foreach (var bc in this.BlogCategoryXrefs)
                d.Add(bc.Category.Name, bc);
            return d;
        }
        public static BlogPost LoadFromId(int id)
        {
            return DbUtil.Db.BlogPosts.SingleOrDefault(b => b.Id == id);
        }
        public Blog BlogCached
        {
            get
            {
                string BLOG = "BLOG" + BlogId;
                if (!HttpContext.Current.Items.Contains(BLOG))
                    HttpContext.Current.Items[BLOG] = this.Blog;
                return (Blog)HttpContext.Current.Items[BLOG];
            }
        }
        public void AddCategory(string Category)
        {
            var cat = DbUtil.Db.Categories.SingleOrDefault(ca => ca.Name == Category);
            if (cat == null)
            {
                cat = new Category { Name = Category };
                DbUtil.Db.Categories.InsertOnSubmit(cat);
                DbUtil.Db.SubmitChanges();
            }
            var bc = new BlogCategoryXref { CatId = cat.Id };
            BlogCategoryXrefs.Add(bc);
        }
        partial void OnEntryDateChanging(DateTime? value)
        {
            DateTime dtnow = DateTime.Now;
            if (value > dtnow)
                NotifyLater = true;
        }
        internal bool PostedRecently
        {
            get
            {
                TimeSpan ts = DateTime.Now
                                - (Updated.HasValue ? Updated.Value : EntryDate.Value);
                return ts.TotalSeconds > 10 && ts.TotalHours < 24;
            }
        }
        public void NotifyEmail(bool notifyAnyway)
        {
            //if (notifyAnyway || (!PostedRecently && !NotifyLater))
            var returnloc = Util.ResolveServerUrl("~/Blog/{0}.aspx".Fmt(Id));
            var stopemail = Util.ResolveServerUrl("~/StopNotifications.aspx") + "?blog={0}&user="
                .Fmt(BlogCached.Id);
            var smtp = new SmtpClient();
            var n = 0;
            var from = new MailAddress("bbcms01@bellevue.org");
            var subject = "New Blog Post: " + Title + ", From: " + PosterName;
            var reply = new MailAddress(PosterEmail);
            var sb = new StringBuilder();
            foreach (var i in BlogCached.GetNotificationList())
            {
                var ma = Util.TryGetMailAddress(i.Value.Email, i.Value.Name);
                if (ma == null)
                    sb.AppendFormat("{0}&lt;{1}&gt; (not sent)<br/>\n", i.Value.Name, i.Value.Email);
                else
                {
                    sb.AppendFormat("{0}&lt;{1}&gt;<br/>\n", i.Value.Name, i.Value.Email);
                    var msg = new MailMessage(from, ma);
                    msg.ReplyTo = reply;
                    msg.Subject = subject;
                    msg.Body = @"<br>--<br>View this post online at: <a href=""{0}?user={1}"">{0}</a>
<br>--<br>
Click <a href=""{2}"">here</a> to stop receiving notifications"
                        .Fmt(returnloc, i.Key, stopemail + i.Value.User);
                    msg.IsBodyHtml = true;
                    if (n % 20 == 0)
                        smtp = new SmtpClient();
                    n++;
                    smtp.Send(msg);
                }
            }
            var f = new MailAddress("bbcms01@bellevue.org");
            var msg2 = new MailMessage(f, reply);
            msg2.Subject = "Notifications sent to:";
            msg2.Body = sb.ToString();
            msg2.IsBodyHtml = true;
            var smtp2 = new SmtpClient();
            smtp.Send(msg2);
        }
        public bool IsTempPost
        {
            get
            {
                return Title.Contains("Temporary Post Used For Style Detection") &&
                    Post.Contains("This is a temporary post that was not deleted.");
            }
        }
        public int CommentCount
        {
            get { return BlogComments.Count(); }
        }
        public static User getUser(int? posterid)
        {
            string loc = "User_" + posterid;
            var u = HttpContext.Current.Items[loc] as User;
            if (u == null)
            {
                u = DbUtil.Db.GetUser(posterid);
                HttpContext.Current.Items[loc] = u;
            }
            return u;
        }
        public string PosterName
        {
            get
            {
                var u = getUser(PosterId);
                return u.FirstName + " " + u.LastName;
            }
        }
        public string PosterEmail
        {
            get
            {
                var u = getUser(PosterId);
                return u.EmailAddress;
            }
        }
        public BlogPost PrevPost()
        {
            var q = from bp in DbUtil.Db.BlogPosts
                    where bp.BlogId == BlogId
                    where bp.EntryDate < EntryDate
                    orderby bp.EntryDate descending
                    select bp;
            return q.FirstOrDefault();
        }
        public BlogPost NextPost()
        {
            var q = from bp in DbUtil.Db.BlogPosts
                    where bp.BlogId == BlogId
                    where bp.EntryDate > EntryDate
                    orderby bp.EntryDate
                    select bp;
            return q.FirstOrDefault();
        }
        public BlogComment AddComment(string text)
        {
            var bc = new BlogComment();
            bc.Comment = text;
            bc.PosterId = DbUtil.Db.CurrentUser.UserId;
            bc.DatePosted = DateTime.Now;
            BlogComments.Add(bc);
            return bc;
        }
    }

    public class ArchiveInfo
    {
        public int Count { get; set; }
        public DateTime Month { get; set; }
    }
    [DataObject]
    public class BlogPostController
    {
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static IEnumerable<BlogPost> FetchTopNPosts(int BlogId, int count)
        {
            DateTime dtnow = DateTime.Now;
            var q = from p in DbUtil.Db.BlogPosts
                    where p.BlogId == BlogId && p.EntryDate < dtnow
                    orderby p.EntryDate descending
                    select p;
            return q.Take(count);
        }
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static IEnumerable<BlogPost> FetchNotifyLaters()
        {
            return from p in DbUtil.Db.BlogPosts
                   where p.NotifyLater
                   select p;
        }
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static IEnumerable<BlogPost> FetchPageOfPosts(int BlogId, string category, int count, int Page, out int tcount)
        {
            DateTime dtnow = DateTime.Now;
            var q = from p in DbUtil.Db.BlogPosts
                    where p.BlogId == BlogId && p.EntryDate < dtnow
                    where (category == "" || p.BlogCategoryXrefs.Any(bc => bc.Category.Name == category))
                    orderby p.EntryDate descending
                    select p;
            var c = q.Count();
            tcount = c / count;
            if (c % count > 0)
                tcount++;
            return q.Skip((Page - 1) * count).Take(count);
        }
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static IEnumerable<BlogPost> FetchMonthOfPosts(int BlogId, DateTime month)
        {
            var nextmon = month.AddMonths(1);
            var dtnow = DateTime.Now;
            var q = from p in DbUtil.Db.BlogPosts
                    where p.BlogId == BlogId && p.EntryDate < dtnow
                    where p.EntryDate >= month && p.EntryDate < nextmon
                    orderby p.EntryDate descending
                    select p;
            return q;
        }
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static IEnumerable<View.BlogRecentPostView> FetchTopNTitles(int BlogId, int count)
        {
            DateTime dtnow = DateTime.Now;
            var q = from p in DbUtil.Db.ViewBlogRecentPostViews
                    where p.BlogId == BlogId && p.EntryDate < dtnow
                    orderby p.EntryDate descending
                    select p;
            return q.Take(count);
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<BlogComment> FetchPostComments(int BlogPostId)
        {
            var q = from c in DbUtil.Db.BlogComments
                    where c.BlogPostId == BlogPostId
                    orderby c.DatePosted
                    select c;
            var list = q.ToList();
            int i = 1;
            foreach (var c in list)
                c.CommentNumber = i++;
            return list;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static IEnumerable<ArchiveInfo> FetchArchive(int BlogId)
        {
            var q = from p in DbUtil.Db.BlogPosts
                    where p.BlogId == BlogId
                    orderby p.EntryDate descending
                    group p by new { p.EntryDate.Value.Year, p.EntryDate.Value.Month } into g
                    orderby g.Key.Year descending, g.Key.Month descending
                    select new ArchiveInfo
                    {
                        Month = new DateTime(g.Key.Year, g.Key.Month, 1),
                        Count = g.Count(),
                    };
            return q;
        }
    }
}