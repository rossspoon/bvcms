using System;
using System.Collections.Generic;
using System.Xml;
using System.Web.Mvc;
using System.Xml.Linq;
using UtilityExtensions;
using System.Linq;
using CmsData;

namespace CmsWeb.Models.iPhone
{
    public class SearchResult : ActionResult
    {
        private List<PeopleInfo> items;
        private int count;
        public SearchResult(IEnumerable<PeopleInfo> items, int count)
        {
            this.items = items.ToList();
            this.count = count;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "text/xml";
            var settings = new XmlWriterSettings();
            settings.Encoding = new System.Text.UTF8Encoding(false);
            settings.Indent = true;

            using (var w = XmlWriter.Create(context.HttpContext.Response.OutputStream, settings))
            {
                w.WriteStartElement("SearchResults");
                w.WriteAttributeString("count", count.ToString());

                foreach (var p in items)
                {
                    w.WriteStartElement("Person");
                    w.WriteAttributeString("peopleid", p.PeopleId.ToString());
                    w.WriteAttributeString("name", p.Name);
                    w.WriteAttributeString("address", p.Address);
                    w.WriteAttributeString("citystatezip", p.CityStateZip);
                    w.WriteAttributeString("zip", p.Zip);
                    w.WriteAttributeString("homephone", p.HomePhone);
                    w.WriteAttributeString("age", p.Age.ToString());
                    w.WriteEndElement();
                }
                w.WriteEndElement();
            }
        }
    }
}