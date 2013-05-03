using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using CmsData;
using System.Text;
using UtilityExtensions;
using System.Web.Mvc;
using System.Diagnostics;

namespace CmsWeb.Models
{
	[Serializable]
	public class TestPlan
	{
		[XmlArrayItem("Section")]
		public List<ApiSectionInfo> Sections { get; set; }
	}

	[Serializable]
	public class ApiSectionInfo
	{
		[XmlAttribute]
		public string name { get; set; }

		[XmlArrayItem("test")]
		public List<ApiTestInfo> Tests { get; set; }
	}

	[Serializable]
	public class ApiTestInfo
	{
		[XmlAttribute]
		public string name { get; set; }

		[XmlArrayItemAttribute("name")]
		public List<string> args { get; set; }

		private string _script;

		public XmlCDataSection script
		{
			get
			{
				var doc = new XmlDocument();
				return doc.CreateCDataSection(_script);
			}
			set { _script = value.Value; }
		}

		private string _description;

		public XmlCDataSection description
		{
			get
			{
				var doc = new XmlDocument();
				return doc.CreateCDataSection(_description);
			}
			set { _description = value.Value; }
		}

		public int LineCount
		{
			get
			{
				if (!_script.HasValue())
					return 0;
				return _script.Split('\n').Length;
			}
		}

		public static TestPlan testplan()
		{
			var xs = new XmlSerializer(typeof (TestPlan));
			var doc = (TestPlan) xs.Deserialize(new StringReader(Resource1.APITestPlan));
			return doc;
		}
	}
}