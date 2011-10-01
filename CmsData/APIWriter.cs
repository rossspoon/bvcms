using System;
using System.Linq;
using System.Xml;
using UtilityExtensions;
using System.Text;
using System.IO;

namespace CmsData.API
{
    public class APIWriter
    {
        private XmlWriter w;
        private StringBuilder sb;
        private StringWriter sw;
        public APIWriter()
        {
            var settings = new XmlWriterSettings();
            settings.Encoding = new System.Text.UTF8Encoding(false);
            settings.Indent = true;
            sb = new StringBuilder();
            sw = new StringWriter(sb);
            w = XmlWriter.Create(sw, settings);
        }
        public void Start(string element)
        {
            w.WriteStartElement(element);
        }
        public void End()
        {
            w.WriteEndElement();
        }
        public void Attr(string attr, int i)
        {
            Attr(attr, i.ToString());
        }
        //public void Attr(string attr, DateTime? d)
        //{
        //    Attr(attr, d.ToString2("M/d/yy h:mm tt"));
        //}
        public void Attr(string attr, DateTime d)
        {
            Attr(attr, d.ToString("M/d/yy h:mm tt"));
        }
        public void Attr(string attr, string s)
        {
            if (s.HasValue())
                w.WriteAttributeString(attr, s);
        }
        public void Add(string element, string s)
        {
            if (s.HasValue())
                w.WriteElementString(element, s);
        }
        public void Add(string element, bool b)
        {
            w.WriteElementString(element, b.ToString());
        }
        public void Add(string element, DateTime d)
        {
            w.WriteElementString(element, d.ToString("M/d/yy h:mm tt"));
        }
        public override string ToString()
        {
            w.Close();
            return sb.ToString(); 
        }
    }
}
