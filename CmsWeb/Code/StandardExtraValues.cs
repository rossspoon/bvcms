using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using CmsData;
using System.Web.Mvc;

namespace CmsWeb.Code
{
	public class StandardExtraValues
	{
		List<SelectList> vvv;
		public StandardExtraValues()
		{
			var xml = DbUtil.Db.Content("StandardExtraValues.xml", "<StandardExtraValues />");
			var doc = XDocument.Parse(xml);
		}
	}
}