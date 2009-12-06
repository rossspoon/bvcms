using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using CmsData;
using System.Web.Services;
using System.Text;
using UtilityExtensions;

namespace Disciples.BlogPost
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class rsd : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/xml";
            var b = CmsData.Blog.LoadById(context.Request.QueryString<int>("blogid"));
            context.Response.ContentEncoding = Encoding.UTF8;
            var homeurl = Util.ResolveServerUrl("~/BlogPost/{0}.aspx".Fmt(b.Name));
            var engineurl = Util.ResolveServerUrl("~");
            var metaurl = Util.ResolveServerUrl("~/BlogPost/metaweblogapi.ashx");
            XDocument document = new XDocument(
                new XDeclaration("1.0", "utf-8", null),
                new XElement("rsd",
                    new XElement("service",
                        new XElement("engineName", "GO Disciple's Blog"),
                        new XElement("engineLink", engineurl),
                        new XElement("homePageLink", homeurl),
                        new XElement("apis",
                            new XElement("api",
                                new XAttribute("name", "MetaWeblog"),
                                new XAttribute("preferred", "true"),
                                new XAttribute("apiLink", metaurl),
                                new XAttribute("BlogID", b.Id.ToString())
                            )
                        )
                    )
                )
            );
            document.Save(context.Response.Output);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
