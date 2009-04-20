using System;
using System.ComponentModel;
using System.Web;
using System.Collections.Generic;
using System.Web.Security;
using System.Net.Mail;
using System.Linq;
using BellevueTeachers;

namespace BTeaData
{
    public partial class Blog
    {
        public static IEnumerable<Blog> GetBlogsForPoster(string user)
        {
            return from b in DbUtil.Db.Blogs
                   where b.Group.Roles.Any(r => r.RoleName == "Blogger"
                       && r.UserRoles.Any(ru => ru.User.Username == user))
                   select b;
        }
        public string Owner
        {
            get { return User != null ? User.Username : "na"; }
        }
        private Group _CachedGroup;
        public Group CachedGroup
        {
            get
            {
                if (_CachedGroup == null)
                    _CachedGroup = this.Group;
                return _CachedGroup;
            }
        }
        public bool IsAdmin
        {
            get { return CachedGroup.IsAdmin; }
        }
        public bool IsMember
        {
            get { return CachedGroup.IsMember; }
        }
        public bool IsBlogger
        {
            get { return CachedGroup.IsBlogger; }
        }
        public string GroupName
        {
            get { return CachedGroup.Name; }
        }
        public bool HasPosts
        {
            get
            {
                return 0 < DbUtil.Db.BlogPosts.Count(bp => bp.BlogId == Id);
            }
        }
        public bool CanDelete
        {
            get { return IsAdmin && !HasPosts; }
        }
        public BlogPost NewPost(string title, string entry)
        {
            DateTime dt = DateTime.Now;
            return NewPost(title, entry, Util.CurrentUser.Username, dt);
        }
        public BlogPost NewPost(string title, string entry, string user, DateTime dt)
        {
            var b = new BlogPost();
            var u = Util.GetUser(user);
            b.EntryDate = dt;
            b.Post = entry;
            b.PosterId = u.UserId;
            b.Title = HttpContext.Current.Server.HtmlEncode(title);
            this.BlogPosts.Add(b);
            return b;
        }
        public BlogPost LastPost()
        {
            return DbUtil.Db.BlogPosts
                .Where(bp => bp.BlogId == Id)
                .OrderByDescending(bp => bp.EntryDate)
                .FirstOrDefault();
        }
        public static Blog LoadByName(string name)
        {
            return DbUtil.Db.Blogs.SingleOrDefault(b => b.Name == name);
        }
        public static Blog LoadById(int id)
        {
            return DbUtil.Db.Blogs.SingleOrDefault(b => b.Id == id);
        }
        public class MailTo
        {
            public MailTo(string email, string name, string user)
            {
                Email = email;
                Name = name;
                User = user;
            }
            public string Email { get; set; }
            public string Name { get; set; }
            public string User { get; set; }
        }
        public Dictionary<string, MailTo> GetNotificationList()
        {
            var list = new Dictionary<string, MailTo>();
            foreach (var mu in Group.GetUsersInRole("Administrator"))
            {
                if (mu.NotifyAll ?? true)
                    list.Add(mu.Username, new MailTo(mu.EmailAddress, mu.FirstName + " " + mu.LastName, mu.Username));
            }
            foreach (var mu in Group.GetUsersInGroup(GroupId.Value))
                if (!list.ContainsKey(mu.Username))
                    if (!BlogNotifications.Any(n => n.UserId == mu.UserId))
                    {
                        if (mu.NotifyAll ?? true)
                            list.Add(mu.Username, new MailTo(mu.EmailAddress,
                                mu.FirstName + " " + mu.LastName, mu.Username));
                    }
            foreach (var on in OtherNotifications)
                list.Add("other", new MailTo(on.Email, "", null));
            return list;
        }
    }
    [DataObject]
    public class BlogController
    {
        internal static string user
        {
            get { return Util.CurrentUser.Username; }
        }
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public IEnumerable<Blog> FetchAllForUser()
        {
            var list = Group.FetchIdsForUser();
            if (user == "admin")
                return DbUtil.Db.Blogs;
            else
                return DbUtil.Db.Blogs.Where(b => list.Contains(b.GroupId.Value) || (b.IsPublic && !b.NotOnMenu));
        }
        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public void Update(int Id, string Description, string Name, string Title, bool IsPublic)
        {
            var blog = DbUtil.Db.Blogs.SingleOrDefault(b => b.Id == Id);
            blog.Description = Description;
            blog.Name = Name;
            blog.Title = Title;
            blog.IsPublic = IsPublic;
            DbUtil.Db.SubmitChanges();
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public void Delete(int Id)
        {
            var blog = DbUtil.Db.Blogs.SingleOrDefault(b => b.Id == Id);
            DbUtil.Db.Blogs.DeleteOnSubmit(blog);
            DbUtil.Db.SubmitChanges();
        }
    }
}