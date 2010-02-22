using System;
using System.Collections.Generic;
using System.Xml;
using System.Web.Mvc;
using System.Xml.Linq;
using UtilityExtensions;
using System.Linq;
using CmsData;

namespace CMSWeb.Models
{
    public class MultipleResult : ActionResult
    {
        private List<FamilyInfo> items;
        public MultipleResult(IEnumerable<FamilyInfo> items)
        {
            this.items = new List<FamilyInfo>(items);
        }
        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "text/xml";
            var settings = new XmlWriterSettings();
            settings.Encoding = new System.Text.UTF8Encoding(false);
            using (var w = XmlWriter.Create(context.HttpContext.Response.OutputStream, settings))
            {
                w.WriteStartElement("Families");
                foreach (var f in items)
                {
                    w.WriteStartElement("family");
                    w.WriteAttributeString("id", f.FamilyId.ToString());
                    w.WriteAttributeString("name", f.Name);
                    w.WriteEndElement();
                }
                w.WriteEndElement();
            }
        }
    }
}