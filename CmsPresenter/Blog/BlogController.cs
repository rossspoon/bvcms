using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMSModel;
using UtilityExtensions;
using System.ComponentModel;

namespace CMSPresenter
{
    [DataObject]
    public class BlogController
    {
        CMSDataContext Db;
        public BlogController()
        {
            Db = DbUtil.Db;
        }
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public IEnumerable<BlogPost> FetchPageOfPosts(int BlogId, int count, int pagestart)
        {
            var dtnow = Util.Now;
            var q = from p in Db.BlogPosts
                    where p.BlogId == BlogId && p.EntryDate < dtnow
                    orderby p.EntryDate descending
                    select p;
            return q.Skip((pagestart - 1) * count).Take(count);
        }
        public Blog GetBlogByName(string name)
        {
            return Db.Blogs.SingleOrDefault(blog => blog.Name == name);
        }
        public Blog GetBlogById(int id)
        {
            return Db.Blogs.SingleOrDefault(blog => blog.Id == id);
        }
        public BlogPost GetPostById(int id)
        {
            return Db.BlogPosts.SingleOrDefault(p => p.Id == id);
        }
    }
}
