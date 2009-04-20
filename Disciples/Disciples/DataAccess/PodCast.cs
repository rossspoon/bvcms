using System;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using BellevueTeachers;

namespace BTeaData
{
    public partial class PodCast
    {
        public static PodCast Post(User user, string Title, string Description, DateTime pubDate, string S3Name, int length)
        {
            var podcast = DbUtil.Db.PodCasts.SingleOrDefault(pc => pc.S3Name == S3Name);
            if (podcast == null)
            {
                podcast = new PodCast();
                podcast.S3Name = S3Name;
            }
            podcast.UserId = user.UserId;
            podcast.Title = Title;
            podcast.Description = Description;
            podcast.PubDate = pubDate;
            podcast.Length = length;
            var blog = Blog.LoadByName("Podcast");
            podcast.BlogPost = blog.NewPost(Title, podcast.GetBlogText());
            podcast.BlogPost.PosterId = user.UserId;
            podcast.BlogPost.AddCategory("Podcast");
            return podcast;
        }
        public string GetBlogText()
        {
            var u = Util.GetUser(UserId);
            return string.Format(@"<strong>{0} {1} on {2}</strong>
<div>{3}</div>
<div>Listen:<object id=""audioplayer{5}"" type=""application/x-shockwave-flash"" height=""24"" width=""290"" data=""player.swf"">
<param value=""player.swf"" name=""movie"" />
<param value=""playerID={5}&amp;soundFile=http://podcast.bellevueteachers.com.s3.amazonaws.com/{4}"" name=""FlashVars"" />
<param value=""high"" name=""quality"" />
<param value=""false"" name=""menu"" />
<param value=""transparent"" name=""wmode"" /></object></div>
<div><a href=""http://podcast.bellevueteachers.com.s3.amazonaws.com/{4}"">download mp3</a></div>",
                            u.FirstName, u.LastName, PubDate, Description, S3Name, Id);
        }
    }
    public partial class PodCastController
    {
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public IEnumerable<View.PodcastSummary> FetchSummary()
        {
            return DbUtil.Db.ViewPodcastSummaries;
        }
    }
}