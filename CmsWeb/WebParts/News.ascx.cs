/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CmsData;
using System.Net;
using System.IO;

namespace CMSWeb.WebParts
{
    public partial class News : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var feedurl = ConfigurationManager.AppSettings["BlogFeedUrl"];
            
            HyperLink1.NavigateUrl = ConfigurationManager.AppSettings["BlogAppUrl"];

            var wr = new WebClient();
            var feed = DbUtil.Db.RssFeeds.FirstOrDefault(r => r.Url == feedurl);

            var req = WebRequest.Create(feedurl) as HttpWebRequest;

            if (feed != null)
            {
                req.IfModifiedSince = feed.LastModified.Value;
                req.Headers.Add("If-None-Match", feed.ETag);
            }
            else
            {
                feed = new RssFeed();
                DbUtil.Db.RssFeeds.InsertOnSubmit(feed);
                feed.Url = feedurl;
            }

            try
            {
                var resp = req.GetResponse() as HttpWebResponse;
                feed.LastModified = resp.LastModified;
                feed.ETag = resp.Headers["ETag"];
                var sr = new StreamReader(resp.GetResponseStream());
                feed.Data = sr.ReadToEnd();
                sr.Close();
                DbUtil.Db.SubmitChanges();
            }
            catch (WebException)
            {
            }

            XDocument rssFeed = XDocument.Parse(feed.Data);

            var posts = from item in rssFeed.Descendants("item")
                        let au = item.Element("author")
                        select new
                        {
                            Title = item.Element("title").Value,
                            Published = DateTime.Parse(item.Element("pubDate").Value),
                            Url = item.Element("link").Value,
                            Author = au != null ? au.Value : "David Carroll",
                        };

            GridView1.DataSource = posts;
            GridView1.DataBind();
        }
    }
}