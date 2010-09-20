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
    public class DetailsResult : ActionResult
    {
        private List<FamilyInfo> items;
        private int page;
        public DetailsResult(IEnumerable<FamilyInfo> items, int? page)
        {
            this.items = new List<FamilyInfo>(items);
            this.page = page ?? 1;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "text/xml";
            var settings = new XmlWriterSettings();
            settings.Encoding = new System.Text.UTF8Encoding(false);

            using (var w = XmlWriter.Create(context.HttpContext.Response.OutputStream, settings))
            {
                w.WriteStartElement("Families");
                w.WriteAttributeString("phone", items[0].Phone);

                var count = items.Count;
                const int INT_PageSize = 10;
                var startrow = (page - 1) * INT_PageSize;
                if (count > startrow + INT_PageSize)
                    w.WriteAttributeString("next", (page + 1).ToString());
                else
                    w.WriteAttributeString("next", "");
                if (page > 1)
                    w.WriteAttributeString("prev", (page - 1).ToString());
                else
                    w.WriteAttributeString("prev", "");

                foreach (var f in items.Skip(startrow).Take(INT_PageSize))
                {
                    w.WriteStartElement("family");
                    w.WriteAttributeString("id", f.FamilyId.ToString());
                    w.WriteAttributeString("name", f.Name);
                    w.WriteAttributeString("waslocked", f.Locked.ToString());
                    w.WriteEndElement();
                }
                w.WriteEndElement();
            }
        }
    }
}