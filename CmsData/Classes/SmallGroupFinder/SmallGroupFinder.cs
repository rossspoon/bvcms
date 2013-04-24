using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CmsData.Classes.SmallGroupFinder
{
    [XmlRoot("Finder")]
    public class SmallGroupFinder
    {
        [XmlAttribute]
        public int divisionid { get; set; }

        [XmlArray]
        public List<Setting> Settings { get; set; }

        [XmlArray]
        public List<Filter> Filters { get; set; }
    }

    [Serializable]
    public class Setting
    {
        [XmlAttribute]
        public string name { get; set; }

        [XmlAttribute]
        public string value { get; set; }

        public override string ToString()
        {
            return value;
        }
    }

    [Serializable]
    public class Filter
    {
        [XmlAttribute]
        public string name { get; set; }

        [XmlAttribute]
        public string title { get; set; }

        [XmlAttribute]
        public string value { get; set; }

        [XmlAttribute]
        public bool locked { get; set; }

        [XmlAttribute]
        public string lockedvalue { get; set; }

        public override string ToString()
        {
            return value;
        }
    }
}
