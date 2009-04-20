using System;
using System.Data;
using System.Web;
using System.Text;
using System.Xml;
using DiscData;
using System.Linq;

namespace BellevueTeachers
{
    public partial class Podcast_Feed : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            string u = Request.QueryString<string>("u");
            var podcaster = Util.GetUser(u);

            Response.ClearContent();
            Response.ContentEncoding = Encoding.UTF8;
            Response.ContentType = "text/xml";

            string sLastMod;
            var dt = (from p in DbUtil.Db.PodCasts
                      where p.PubDate != null && p.UserId == podcaster.UserId
                      orderby p.PubDate descending
                      select p.PubDate).First();
            sLastMod = dt.Value.ToString("r");

            bool Caching = false;
            if (Caching)
            {
                Response.AddHeader("Last-Modified", sLastMod);
                Response.AddHeader("ETag", sLastMod);
                string IfModifiedSince = Request.Headers["If-Modified-Since"];
                string IfNoneMatch = Request.Headers["If-None-Match"];
                if (IfModifiedSince != null && IfModifiedSince == sLastMod && IfNoneMatch == sLastMod)
                {
                    Response.StatusCode = 304;
                    Response.StatusDescription = "Not Modified";
                    return;
                }
            }
            XmlTextWriter w = new XmlTextWriter(Response.OutputStream, Encoding.UTF8);
            w.WriteStartDocument();
            string xslt = ResolveClientUrl("~/rss.xslt");
            w.WriteProcessingInstruction("xml-stylesheet", "href='" + xslt + "' type='text/xsl'");
            w.WriteStartElement("rss");
            w.WriteAttributeString("version", "2.0");
            w.WriteStartElement("channel");
            w.WriteElementString("title", "Bellevue Teacher's Podcast");
            w.WriteElementString("description", "Bellevue Bible Fellowship Teachers Sunday School Lessons");
            w.WriteElementString("link", Request.Url.Authority + ResolveUrl("~"));
            //        w.WriteElementString("copyright", "Copyright 2007 Bellevue Baptist Church.");
            w.WriteElementString("language", "en-us");
            w.WriteElementString("lastBuildDate", dt.Value.ToString("r"));
            w.WriteElementString("pubDate", dt.Value.ToString("r"));

            var q = from p in DbUtil.Db.PodCasts
                    where p.S3Name != null && p.UserId == podcaster.UserId
                    orderby p.PubDate descending, Title
                    select p;

            foreach (var p in q.Take(30))
            {
                //p.UpdateBlogEntry(); // Maintenance
                w.WriteStartElement("item");
                w.WriteElementString("title", p.Title);
                w.WriteElementString("description", p.Description);
                w.WriteElementString("link", "http://{0}/blog/podcast/{1}.aspx"
                    .Fmt(Request.Url.Authority, p.PostId));
                var pc = Util.GetUser(p.UserId);

                w.WriteElementString("author", pc.FirstName + ' ' + pc.LastName);
                w.WriteElementString("pubDate", p.PubDate.Value.ToString("r"));
                w.WriteElementString("category", "Podcast");

                w.WriteStartElement("enclosure");
                w.WriteAttributeString("url", "http://podcast.bellevueteachers.com.s3.amazonaws.com/"
                    + p.S3Name);
                w.WriteAttributeString("length", p.Length.ToString());
                w.WriteAttributeString("type", "audio/mpeg");
                w.WriteEndElement(); //enclosure

                w.WriteEndElement(); // item
            }
            w.WriteEndElement(); // channel
            w.WriteEndElement(); // rss
            w.WriteEndDocument();
            w.Close();
        }
    }

}