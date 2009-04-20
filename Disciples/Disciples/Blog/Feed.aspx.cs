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
using DiscData;

public partial class Feed : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var blog = Blog.LoadByName(Request.QueryString<string>("name"));
        var q = from p in blog.BlogPosts
                 where p.EntryDate != null
                 orderby p.EntryDate descending
                 select p.EntryDate;
        var dt = DateTime.MinValue;
        if(q.Count() > 0)
            dt = q.First().Value;
        var abbreviated = Request.QueryString<string>("abbr");
        Response.Clear();
        Response.ContentType = "text/xml";

        var sLastMod = dt.ToString("r");

        Context.Response.AddHeader("Last-Modified", sLastMod);
        Context.Response.AddHeader("ETag", sLastMod);
        string IfModifiedSince = Context.Request.Headers["If-Modified-Since"];
        string IfNoneMatch = Context.Request.Headers["If-None-Match"];
        if (IfModifiedSince != null && IfModifiedSince == sLastMod && IfNoneMatch == sLastMod)
        {
            Context.Response.StatusCode = 304;
            Context.Response.StatusDescription = "Not Modified";
            return;
        }

        var document = new XDocument(
            new XDeclaration("1.0", "utf-8", null),
            new XElement("rss",
                new XElement("channel",
                    new XElement("title", blog.Title),
                    new XElement("link", "http://{0}/Blog/{1}.aspx".Fmt(Request.Url.Authority, blog.Name)),
                    new XElement("description", blog.Description),
                    new XElement("lastBuildDate", dt.ToString("r")),
                    new XElement("pubDate", dt.ToString("r")),
                    from p in blog.BlogPosts.OrderByDescending(bp => bp.EntryDate).Take(20)
                    select new XElement("item",
                        new XElement("title", p.Title),
                        new XElement("description", abbreviated.HasValue() ? trunc25(p.Post) : p.Post),
                        new XElement("link", "http://{0}/blog/{1}.aspx".Fmt(Request.Url.Authority, p.Id)),
                        new XElement("author", p.PosterName),
                        new XElement("pubDate", p.EntryDate),
                        from c in p.GetBlogCategories()
                        select new XElement("category", c.Key)
                        )
                ),
                new XAttribute("version", "2.0")
            )
        );
        document.Save(Response.Output);
        Response.End();
    }
    string trunc25(string s)
    {
        var t25 = 25;
        if (s.Length < 25)
            t25 = s.Length;
        return s.Substring(0, t25);
    }
}
