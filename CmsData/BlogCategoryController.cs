using System;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using System.Web;

namespace CmsData
{
    [DataObject]
    public class BlogCategoryController
    {
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public IEnumerable<View.BlogCategoriesView> GetCategorySummary()
        {
            return DbUtil.Db.ViewBlogCategoriesViews;
        }
        public static IEnumerable<string> GetCategoryListFromCache(int BlogPostId)
        {
            if (!HttpContext.Current.Items.Contains("categories"))
                HttpContext.Current.Items["categories"] = DbUtil.Db.BlogCategoryXrefs.ToList();
            return from bc in (List<BlogCategoryXref>)(HttpContext.Current.Items["categories"])
                   where bc.BlogPostId == BlogPostId
                   select bc.BlogCategory.Name;
        }
    }
}