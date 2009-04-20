using System;
using System.Web;
using System.Collections.Generic;
using System.IO;
using System.Web.Security;
using System.Linq;
using ccx = CookComputing.XmlRpc;
using CMSModel;
using UtilityExtensions;

namespace CMSPresenter
{
    public class MetaWebLog : ccx.XmlRpcService, IMetaWeblog
    {
        private CMSDataContext Db;
        public MetaWebLog()
        {
            Db = DbUtil.Db;
        }
        private bool Authenticate(string username, string password)
        {
            return Membership.ValidateUser(username, password) && Roles.IsUserInRole(username, "Blogger");
        }

        #region IMetaWeblog Members
        public BlogInfo[] getUsersBlogs(string appKey, string username, string password)
        {
            if (!Membership.ValidateUser(username, password))
                throw new ccx.XmlRpcFaultException(0, "Invalid credentials. Access denied");
            var list = from blogrole in Roles.GetRolesForUser(username)
                       where blogrole.StartsWith("Blog-")
                       let blog = Db.Blogs.Single(b => b.Name == blogrole.Substring(5))
                       select new BlogInfo
                       {
                           blogid = blog.Id.ToString(),
                           blogName = blog.Name,
                           url = HttpContext.Current.Request.ApplicationPath + "/Default.aspx"
                       };
            return list.ToArray();
        }

        public bool editPost(string postid, string username, string password, Post post, bool publish)
        {
            try
            {
                var p = Db.BlogPosts.Single(bp => bp.Id == postid.ToInt());
                if (Authenticate(username, password))
                {
                    if (post.dateCreated.Year > 2000)
                        p.EntryDate = post.dateCreated;
                    p.Updated = UtilityExtensions.Util.Now;
                    p.Title = post.title;
                    p.Post = post.description;
                    p.EnclosureUrl = post.enclosure.url;
                    p.EnclosureLength = post.enclosure.length;
                    p.EnclosureType = post.enclosure.type;
                    Db.BlogPostBlogCategories.DeleteAllOnSubmit(p.BlogPostBlogCategories);
                    AddCategories(post, p);
                    Db.SubmitChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new ccx.XmlRpcFaultException(0, ex.Message);
            }
            throw new ccx.XmlRpcFaultException(0, "Invalid credentials. Access denied");
        }

        public CategoryInfo[] getCategories(object blogid, string username, string password)
        {
            if (Membership.ValidateUser(username, password))
            {
                string apppath = HttpContext.Current.Request.ApplicationPath;
                var q = from v in Db.BlogCategories
                        let _cat = v.Name.Replace(" ", "_")
                        select new CategoryInfo
                        {
                            categoryid = v.Id.ToString(),
                            title = v.Name,
                            description = v.Name,
                            htmlUrl = apppath + "/Posts.aspx?cat=" + _cat,
                            rssUrl = apppath + "/Feed.aspx?cat=" + _cat
                        };
                return q.ToArray();
            }
            throw new ccx.XmlRpcFaultException(0, "Invalid credentials. Access denied");
        }

        public Post getPost(string postid, string username, string password)
        {
            var p = Db.BlogPosts.Single(bp => bp.Id == postid.ToInt());
            string apppath = HttpContext.Current.Request.ApplicationPath;
            if (Authenticate(username, password))
            {
                Post post = new Post();
                post.categories = p.BlogPostBlogCategories.Select(c => c.BlogCategory.Name).ToArray();
                post.dateCreated = p.EntryDate.Value;
                post.description = p.Post;
                post.link = apppath + "/Post.aspx?id=" + p.Id;
                post.permalink = post.link;
                post.postid = p.Id.ToString();
                post.userid = p.UserId.ToString();
                post.title = p.Title;
                if (p.EnclosureUrl.HasValue())
                {
                    post.enclosure = new Enclosure();
                    post.enclosure.url = p.EnclosureUrl;
                    post.enclosure.length = p.EnclosureLength.Value;
                    post.enclosure.type = p.EnclosureType;
                }
                return post;
            }
            throw new ccx.XmlRpcFaultException(0, "Invalid credentials. Access denied");
        }

        public Post[] getRecentPosts(object blogid, string username, string password, int numberOfPosts)
        {
            var b = Db.Blogs.SingleOrDefault(blog => blog.Id == blogid.ToInt());
            string apppath = HttpContext.Current.Request.ApplicationPath;
            if (Authenticate(username, password))
                return (from p in b.BlogPosts
                        let lnk = "{0}/Post.aspx?id={1}".Fmt(apppath, p.Id)
                        orderby p.EntryDate descending
                        select new Post
                        {
                            categories = p.BlogPostBlogCategories.Select(c => c.BlogCategory.Name).ToArray(),
                            userid = p.UserId.ToString(),
                            dateCreated = p.EntryDate.Value,
                            description = p.Post,
                            link = lnk,
                            permalink = lnk,
                            postid = p.Id.ToString(),
                            title = p.Title,
                            enclosure = PostEnclosure(p)
                        }).ToArray();
            throw new ccx.XmlRpcFaultException(0, "Invalid credentials. Access denied");
        }
        private Enclosure PostEnclosure(BlogPost p)
        {
            var enc = new Enclosure();
            if (p.EnclosureUrl.HasValue())
            {
                enc.url = p.EnclosureUrl;
                enc.type = p.EnclosureType;
                enc.length = p.EnclosureLength.Value;
            }
            return enc;
        }

        private void AddCategories(Post post, BlogPost p)
        {
            if (post.categories != null)
                foreach (var c in post.categories)
                {
                    var cat = Db.BlogCategories.SingleOrDefault(cc => cc.Name == c);
                    if (cat == null)
                    {
                        cat = new BlogCategory { Name = c };
                        Db.BlogCategories.InsertOnSubmit(cat);
                    }
                    var bc = new BlogPostBlogCategory { BlogCategory = cat };
                    p.BlogPostBlogCategories.Add(bc);
                }
        }
        public string newPost(object blogid, string username, string password, Post post, bool publish)
        {
            var b = Db.Blogs.SingleOrDefault(blog => blog.Id == blogid.ToInt());
            string apppath = HttpContext.Current.Request.ApplicationPath;
            if (Authenticate(username, password))
            {
                if (post.dateCreated < DateTime.Today.AddYears(-5))
                    post.dateCreated = Util.Now;
                else
                    post.dateCreated = post.dateCreated.ToLocalTime();
                var user = Db.Users.Single(u => u.Username == username);
                var p = new BlogPost
                {
                    Blog = b,
                    Title = post.title,
                    Post = post.description,
                    UserId = user.UserId,
                    Username = user.Person.Name,
                    EntryDate = post.dateCreated,
                    EnclosureUrl = post.enclosure.url,
                    EnclosureLength = post.enclosure.length,
                    EnclosureType = post.enclosure.type
                };
                Db.BlogPosts.InsertOnSubmit(p);
                AddCategories(post, p);
                Db.SubmitChanges();
                return p.Id.ToString();
            }
            throw new ccx.XmlRpcFaultException(0, "Invalid credentials. Access denied");
        }
        public MediaObjectUrl newMediaObject(string blogid, string username, string password, MediaObject mediaobject)
        {
            var b = Db.Blogs.SingleOrDefault(blog => blog.Id == blogid.ToInt());
            if (Authenticate(username, password))
            {
                string filename = HttpContext.Current.Server.MapPath("pictures/" + mediaobject.name);
                if (!Directory.Exists(Path.GetDirectoryName(filename)))
                    Directory.CreateDirectory(Path.GetDirectoryName(filename));
                File.WriteAllBytes(filename, mediaobject.bits);
                var ret = new MediaObjectUrl();
                ret.url = "http://" + HttpContext.Current.Request.Url.Authority
                    + VirtualPathUtility.ToAbsolute("~/pictures/" + mediaobject.name);
                return ret;
            }
            throw new ccx.XmlRpcFaultException(0, "Invalid credentials. Access denied");
        }

        public bool deletePost(string appKey, string postid, string username, string password, bool publish)
        {
            var p = Db.BlogPosts.Single(bp => bp.Id == postid.ToInt());
            string apppath = HttpContext.Current.Request.ApplicationPath;
            if (Authenticate(username, password))
            {
                Db.BlogPostBlogCategories.DeleteAllOnSubmit(p.BlogPostBlogCategories);
                Db.BlogPosts.DeleteOnSubmit(p);
                Db.SubmitChanges();
                return true;
            }
            throw new ccx.XmlRpcFaultException(0, "Invalid credentials. Access denied");
        }

        #endregion
    }
}