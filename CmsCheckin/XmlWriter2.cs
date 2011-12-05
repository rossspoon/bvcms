using System;
using System.IO;
using System.Xml;
using System.Text;

namespace CmsCheckin
{
    public class XmlWriter2
    {
        private XmlWriter w;
        private StringBuilder sb;
        public XmlWriter2(MemoryStream ms)
        {
            var settings = new XmlWriterSettings();
            settings.Encoding = new System.Text.UTF8Encoding(false);
            settings.Indent = true;
            w = XmlWriter.Create(ms, settings);
        }
        public XmlWriter2()
        {
            var settings = new XmlWriterSettings();
            settings.Encoding = new System.Text.UTF8Encoding(false);
            settings.Indent = true;
            sb = new StringBuilder();
            var sw = new StringWriter(sb);
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
        public void Attr(string attr, object i)
        {
            var s = tostr(i);
            if (s.HasValue())
                w.WriteAttributeString(attr, s);
        }
        private string tostr(object i)
        {
            string s = null;
            if (i is DateTime)
                s = ((DateTime)i).ToString("M/d/yy h:mm tt");
            else if (i == null)
                s = string.Empty;
            else
                s = i.ToString();
            return s;
        }
        public void Add(string element, object i)
        {
            var s = tostr(i);
            if (s.HasValue())
                w.WriteElementString(element, s);
        }
        public void AddText(string text)
        {
            w.WriteRaw(text);
        }
        public override string ToString()
        {
            w.Flush();
            return sb.ToString();
        }
    }
}
