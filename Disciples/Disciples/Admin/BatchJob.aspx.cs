using System;
using System.Linq;
using DiscData;
using System.Text.RegularExpressions;

public partial class BatchJob : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //var q = from p in BlogPostController.FetchNotifyLaters()
        //        where p.NotifyLater && p.EntryDate < SiteUtility.PSTNow
        //        select p;
        //foreach (var p in q)
        //{
        //    p.NotifyLater = false;
        //    p.NotifyEmail(true);
        //}
        //LinqUtil.Db.SubmitChanges();
        var q = from p in DbUtil.Db.BlogPosts
                where p.Post.Contains("http://podcast.bellevueteachers.com/")
                select p;
        foreach(var p in q)
            p.Post = Regex.Replace(p.Post,
                         "http://podcast.bellevueteachers.com/",
                         "http://podcast.bellevueteachers.com.s3.amazonaws.com/", 
                         RegexOptions.IgnoreCase);
        DbUtil.Db.SubmitChanges();
    }
}
