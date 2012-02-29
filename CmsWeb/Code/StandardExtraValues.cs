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
	public static class StandardExtraValues
	{
		[Serializable]
		public class Field
		{
			[XmlAttribute]
			public string name {get; set;}
			[XmlAttribute]
			public string type {get; set;}
			[XmlAttribute]
			public string location {get; set;}
			public string[] codes {get; set;}
            public int order;
			public int peopleid;
			public bool nonstandard;
			public PeopleExtra extravalue;
			public static Field AddField(Field f, PeopleExtra v)
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
					f.type = v.StrValue.HasValue() ? "code"
						: v.Data.HasValue() ? "data"
						: v.DateValue.HasValue ? "date"
						: v.IntValue.HasValue ? "int"
						: "code";
				}
				f.extravalue = v;
				return f;
			}
			public override string  ToString()
        	{
				if (extravalue == null)
					return null;
				switch (type)
				{
					case "code":
						return extravalue.StrValue;
					case "data":
						return extravalue.Data;
					case "date":
						return extravalue.DateValue.FormatDate();
					case "int":
						if (extravalue.IntValue2.HasValue)
							return "{0} {1}".Fmt(extravalue.IntValue, extravalue.IntValue2);
						else
							return extravalue.IntValue.ToString();
				}
				return null;
        	}
		}
		public static Field[] GetExtraValues(int PeopleId)
		{
			var xml = DbUtil.Db.Content("StandardExtraValues.xml", "<StandardExtraValues />");
			var sr = new StringReader(xml);
			var fields = new XmlSerializer(typeof(Field[])).Deserialize(sr) as Field[];
			var n = 1;
			foreach (var f in fields)
			{
				f.order = n++;
				f.peopleid = PeopleId;
			}
			var exvalues = DbUtil.Db.PeopleExtras.Where(ee => ee.PeopleId == PeopleId);
			var qfields = from f in fields
						  join v in exvalues on f.name equals v.Field into j
						  from v in j.DefaultIfEmpty()
						  orderby f.order
						  select Field.AddField(f, v);
			var qvalues = from v in exvalues
						  join f in fields on v.Field equals f.name into j
						  from f in j.DefaultIfEmpty()
						  where f == null
						  orderby v.Field
						  select Field.AddField(f, v);
			return qfields.Concat(qvalues).ToArray();
		}
	}
}