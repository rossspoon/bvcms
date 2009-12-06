using System;
using System.Web;
using System.Collections.Generic;
using System.IO;
using MetaWebLogAPI;
using System.Web.Security;
using CmsData;
using System.Linq;
using UtilityExtensions;

public class MetaWebLog : CookComputing.XmlRpc.XmlRpcService, IMetaWeblog
{
    private bool Authenticate(int groupid, string username, string password)
    {
        var g = Group.LoadById(groupid);
        var user = DbUtil.Db.Users.Single(uu => uu.Username == username);
        return Membership.ValidateUser(username, password) && g.IsUserBlogger(user);
    }

    public BlogInfo[] getUsersBlogs(string appKey, string username, string password)
    {
        if (!Membership.ValidateUser(username, password))
            throw new CookComputing.XmlRpc.XmlRpcFaultException(0, "Invalid credentials. Access denied");
        var list = from b in Blog.GetBlogsForPoster(username)
                   select new BlogInfo
                   {
                       blogid = b.Id.ToString(),
                       blogName = b.Name,
                       url = Util.ResolveUrl("~/Blog/Default.aspx")
                   };
        return list.ToArray();
    }

    public bool editPost(string postid, string username, string password, Post post, bool publish)
    {
        try
        {
            var p = BlogPost.LoadFromId(postid.ToInt());
            if (Authenticate(p.Blog.GroupId.Value, username, password))
            {
                if (post.dateCreated.Year > 2000)
                    p.EntryDate = post.dateCreated;
                p.Updated = DateTime.Now;
                p.Title = post.title;
                p.Post = post.description;
                p.EnclosureUrl = post.enclosure.url;
                p.EnclosureLength = post.enclosure.length;
                p.EnclosureType = post.enclosure.type;
                DbUtil.Db.BlogCategoryXrefs.DeleteAllOnSubmit(p.BlogCategoryXrefs);
                foreach (var c in post.categories)
                {
                    var cat = DbUtil.Db.BlogCategories.Single(ca => ca.Name == c);
                    var bc = new BlogCategoryXref { CatId = cat.Id };
                    p.BlogCategoryXrefs.Add(bc);
                }
                DbUtil.Db.SubmitChanges();
                //Pinger.NotifyWeb();
                return true;
            }
        }
        catch (Exception ex)
        {
            throw new CookComputing.XmlRpc.XmlRpcFaultException(0, ex.Message);
        }
        throw new CookComputing.XmlRpc.XmlRpcFaultException(0, "Invalid credentials. Access denied");
    }

    public CategoryInfo[] getCategories(object blogid, string username, string password)
    {
        if (Membership.ValidateUser(username, password))
        {
            var q = from v in DbUtil.Db.ViewBlogCategoriesViews
                    let _cat = v.Category.Replace(" ", "_")
                    select new CategoryInfo
                {
                    categoryid = v.N.ToString(),
                    title = v.Category,
                    description = v.Category,
                    htmlUrl = Util.ResolveUrl("~/Blog/" + _cat + ".aspx"),
                    rssUrl = Util.ResolveUrl("~/Blog/Feed/" + _cat + ".aspx")
                };
            return q.ToArray();
        }
        throw new CookComputing.XmlRpc.XmlRpcFaultException(0, "Invalid credentials. Access denied");
    }

    public Post getPost(string postid, string username, string password)
    {
        var p = BlogPost.LoadFromId(int.Parse(postid));
        if (Authenticate(p.Blog.GroupId.Value, username, password))
        {
            Post post = new Post();
            post.categories = p.BlogCategoryXrefs.Select(c => c.BlogCategory.Name).ToArray();
            post.dateCreated = p.EntryDate.Value;
            post.description = p.Post;
            post.link = Util.ResolveUrl("~/Blog/" + p.Id + ".aspx");
            post.permalink = post.link;
            post.postid = p.Id.ToString();
            post.userid = p.User.Username;
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
        throw new CookComputing.XmlRpc.XmlRpcFaultException(0, "Invalid credentials. Access denied");
    }

    public Post[] getRecentPosts(object blogid, string username, string password, int numberOfPosts)
    {
        var b = Blog.LoadById(blogid.ToInt());
        if (Authenticate(b.GroupId.Value, username, password))
            return (from p in b.BlogPosts
                    let lnk = Util.ResolveUrl("~/Blog/{0}.aspx".Fmt(p.Id))
                    orderby p.EntryDate descending
                    select new Post
                    {
                        categories = p.BlogCategoryXrefs.Select(c => c.BlogCategory.Name).ToArray(),
                        userid = p.User.Username,
                        dateCreated = p.EntryDate.Value,
                        description = p.Post,
                        link = lnk,
                        permalink = lnk,
                        postid = p.Id.ToString(),
                        title = p.Title,
                        enclosure = PostEnclosure(p)
                    }).ToArray();
        throw new CookComputing.XmlRpc.XmlRpcFaultException(0, "Invalid credentials. Access denied");
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

    public string newPost(object blogid, string username, string password, Post post, bool publish)
    {
        var b = Blog.LoadById(blogid.ToInt());
        string apppath = HttpContext.Current.Request.ApplicationPath;
        if (Authenticate(b.GroupId.Value, username, password))
        {
            if (post.dateCreated < DateTime.Today.AddYears(-5))
                post.dateCreated = DateTime.Now;
            else
                post.dateCreated = post.dateCreated.ToLocalTime();
            BlogPost p = b.NewPost(post.title, post.description, username, post.dateCreated);
            p.EnclosureUrl = post.enclosure.url;
            p.EnclosureLength = post.enclosure.length;
            p.EnclosureType = post.enclosure.type;
            if (post.categories != null)
                foreach (string s in post.categories)
                {
                    var cat = DbUtil.Db.BlogCategories.Single(ca => ca.Name == s);
                    var bc = new BlogCategoryXref { CatId = cat.Id };
                    p.BlogCategoryXrefs.Add(bc);
                }
            DbUtil.Db.SubmitChanges();
            if (!p.Title.StartsWith("Temporary Post Used For Style Detection"))
                p.NotifyEmail(false);
            return p.Id.ToString();
        }
        throw new CookComputing.XmlRpc.XmlRpcFaultException(0, "Invalid credentials. Access denied");
    }
    public MediaObjectUrl newMediaObject(string blogid, string username, string password, MediaObject mediaobject)
    {
        var b = Blog.LoadById(blogid.ToInt());
        if (Authenticate(b.GroupId.Value, username, password))
        {
            string filename = HttpContext.Current.Server.MapPath("/Pictures/" + mediaobject.name);
            if (!Directory.Exists(Path.GetDirectoryName(filename)))
                Directory.CreateDirectory(Path.GetDirectoryName(filename));
            File.WriteAllBytes(filename, mediaobject.bits);
            MediaObjectUrl ret = new MediaObjectUrl();
            ret.url = "http://" + HttpContext.Current.Request.Url.Authority + VirtualPathUtility.ToAbsolute("~/pictures/" + mediaobject.name);
            return ret;
        }
        throw new CookComputing.XmlRpc.XmlRpcFaultException(0, "Invalid credentials. Access denied");
    }

    public bool deletePost(string appKey, string postid, string username, string password, bool publish)
    {
        var p = BlogPost.LoadFromId(postid.ToInt());
        string apppath = HttpContext.Current.Request.ApplicationPath;
        if (Authenticate(p.Blog.GroupId.Value, username, password))
        {
            DbUtil.Db.BlogPosts.DeleteOnSubmit(p);
            DbUtil.Db.SubmitChanges();
            return true;
        }
        throw new CookComputing.XmlRpc.XmlRpcFaultException(0, "Invalid credentials. Access denied");
    }

    public int newCategory(object blogid, string username, string password, NewCategory newcat)
    {
        var b = Blog.LoadById(blogid.ToInt());
        if (Authenticate(b.GroupId.Value, username, password))
        {
            var bc = new BlogCategory { Name = newcat.name };
            DbUtil.Db.BlogCategories.InsertOnSubmit(bc);
            DbUtil.Db.SubmitChanges();
            return bc.Id;
        }
        throw new CookComputing.XmlRpc.XmlRpcFaultException(0, "Invalid credentials. Access denied");
    }
}
