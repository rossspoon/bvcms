using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using DiscData;
using System.Web.Services;
using System.Text;

namespace BellevueTeachers.BlogPost
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class rsd : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/xml";
            var b = DiscData.Blog.LoadById(context.Request.QueryString<int>("blogid"));
            context.Response.ContentEncoding = Encoding.UTF8;

            XDocument document = new XDocument(
                new XDeclaration("1.0", "utf-8", null),
                new XElement("rsd",
                    new XElement("service",
                        new XElement("engineName", "Bellevue Disciple's Blog"),
                        new XElement("engineLink", "http://disciples.bellevue.org"),
                        new XElement("homePageLink", "http://{0}/BlogPost/{1}.aspx".Fmt(context.Request.Url.Authority, b.Name)),
                        new XElement("apis",
                            new XElement("api",
                                new XAttribute("name", "MetaWeblog"),
                                new XAttribute("preferred", "true"),
                                new XAttribute("apiLink", "http://{0}/BlogPost/metaweblogapi.ashx".Fmt(context.Request.Url.Authority)),
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
