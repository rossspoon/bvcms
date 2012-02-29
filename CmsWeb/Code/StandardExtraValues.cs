using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using CmsData;
using System.Web.Mvc;
using System.Xml.Serialization;
using System.IO;
using UtilityExtensions;

namespace CmsWeb.Code
{
	public class StandardExtraValues
	{
		[Serializable]
		public class Fields
		{
			[XmlElement("Field")]
			public Field[] fields { get; set; }
		}
		[Serializable]
		public class Field
		{
			[XmlAttribute]
			public string name {get; set;}
			[XmlAttribute]
			public string type {get; set;}
			[XmlAttribute]
			[System.ComponentModel.DefaultValueAttribute ("default")]
			public string location {get; set;}
			public List<string> Codes { get; set; }
            internal int order;
			public int peopleid;
			public bool nonstandard;
			internal PeopleExtra extravalue;
			internal static Field AddField(Field f, PeopleExtra v)
			{
				if (f == null)
				{
					f = new Field 
					{ 
						name = v.Field, 
						nonstandard = true,
						peopleid = v.PeopleId,
						extravalue = v,
					};
					f.type = v.StrValue.HasValue() ? "Code"
						: v.Data.HasValue() ? "Data"
						: v.DateValue.HasValue ? "Date"
						: v.IntValue.HasValue ? "Int"
						: "Code";
				}
				f.extravalue = v;
				return f;
			}
			public override string  ToString()
        	{
				if (extravalue == null)
					return "Click to edit";
				switch (type)
				{
					case "Code":
						return extravalue.StrValue;
					case "Data":
						return extravalue.Data;
					case "Date":
						return extravalue.DateValue.FormatDate();
					case "Int":
						if (extravalue.IntValue2.HasValue)
							return "{0} {1}".Fmt(extravalue.IntValue, extravalue.IntValue2);
						else
							return extravalue.IntValue.ToString();
				}
				return null;
        	}
		}
		public static IEnumerable<Field> GetExtraValues()
		{
			var xml = DbUtil.StandardExtraValues();
			var sr = new StringReader(xml);
			var fields = (new XmlSerializer(typeof(Fields)).Deserialize(sr) as Fields).fields;
			if (fields == null)
				return new List<Field>();
			return fields;
		}
		public static IEnumerable<Field> GetExtraValues(int PeopleId, string location = "default")
		{
			var fields = GetExtraValues();
			var n = 1;
			foreach (var f in fields)
			{
				f.order = n++;
				f.peopleid = PeopleId;
				if (f.location == null)
					f.location = "default";
			}
			var exvalues = DbUtil.Db.PeopleExtras.Where(ee => ee.PeopleId == PeopleId).ToList();
			var qfields = from f in fields
						  join v in exvalues on f.name equals v.Field into j
						  from v in j.DefaultIfEmpty()
						  where f.location == location
						  orderby f.order
						  select Field.AddField(f, v);
			if (location == "default")
			{
				var qvalues = from v in exvalues
							  join f in fields on v.Field equals f.name into j
							  from f in j.DefaultIfEmpty()
							  where f == null
							  orderby v.Field
							  select Field.AddField(f, v);
				return qfields.Concat(qvalues);
			}
			return qfields;
		}
	}
}