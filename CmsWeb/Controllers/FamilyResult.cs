using System;
using System.Collections.Generic;
using System.Xml;
using System.Web.Mvc;
using System.Xml.Linq;

namespace CMSWeb.Models
{
    public class FamilyResult : ActionResult
    {
        private List<Attendee> items;
        public FamilyResult(IEnumerable<Attendee> items)
        {
            this.items = new List<Attendee>(items);
        }
        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "text/xml";
            var settings = new XmlWriterSettings();
            settings.Encoding = new System.Text.UTF8Encoding(false);
            using(var w = XmlWriter.Create(context.HttpContext.Response.OutputStream, settings))
            {
                w.WriteStartElement("Attendees");
                foreach (var c in items)
                {
                    w.WriteStartElement("attendee");
                    w.WriteAttributeString("id", c.Id.ToString());
                    w.WriteAttributeString("name", c.DisplayName);
                    w.WriteAttributeString("bday", c.Birthday);
                    w.WriteAttributeString("org", c.DisplayClass);
                    w.WriteAttributeString("orgid", c.OrgId.ToString());
                    w.WriteAttributeString("loc", c.Location);
                    w.WriteAttributeString("gender", c.Gender);
                    w.WriteAttributeString("age", c.Age.ToString());
                    w.WriteAttributeString("numlabels", c.NumLabels.ToString());
                    w.WriteAttributeString("checkedin", c.CheckedIn.ToString());
                    w.WriteEndElement();
                }
                w.WriteEndElement();
            }
        }
    }
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