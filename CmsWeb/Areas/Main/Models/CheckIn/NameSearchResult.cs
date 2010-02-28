﻿using System;
using System.Collections.Generic;
using System.Xml;
using System.Web.Mvc;
using System.Xml.Linq;
using UtilityExtensions;
using System.Linq;
using CmsData;

namespace CMSWeb.Models
{
    public class NameSearchResult : ActionResult
    {
        private List<SearchInfo> items;
        public NameSearchResult(IEnumerable<SearchInfo> items)
        {
            this.items = new List<SearchInfo>(items);
        }
        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "text/xml";
            var settings = new XmlWriterSettings();
            settings.Encoding = new System.Text.UTF8Encoding(false);
            using (var w = XmlWriter.Create(context.HttpContext.Response.OutputStream, settings))
            {
                w.WriteStartElement("Results");
                foreach (var c in items)
                {
                    w.WriteStartElement("person");
                    w.WriteAttributeString("name", c.Name);
                    w.WriteAttributeString("address", c.Address);
                    w.WriteAttributeString("cellphone", c.CellPhone.FmtFone7());
                    w.WriteAttributeString("homephone", c.HomePhone.FmtFone7());
                    w.WriteAttributeString("age", c.Age.ToString());
                    w.WriteAttributeString("display", c.GetDisplay());
                    w.WriteEndElement();
                }
                w.WriteEndElement();
            }
        }
    }
}