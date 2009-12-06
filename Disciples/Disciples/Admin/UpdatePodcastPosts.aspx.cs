using System;
using System.Linq;
using CmsData;

public partial class Admin_UpdatePodcastPosts : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        foreach (var pc in DbUtil.Db.PodCasts)
        {
            pc.BlogPost.Post = pc.GetBlogText();
            pc.BlogPost.Updated = DateTime.Now;
        }
        DbUtil.Db.SubmitChanges();
    }
}
