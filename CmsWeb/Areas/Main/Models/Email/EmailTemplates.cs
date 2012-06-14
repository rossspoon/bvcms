using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using CmsWeb.Areas.Manage.Controllers;

namespace CmsWeb.Areas.Main.Models
{
	public class EmailTemplates
	{
		public IQueryable<Content> fetchTemplates()
		{
			return from i in DbUtil.Db.Contents
					 where i.TypeID == DisplayController.TYPE_EMAIL_TEMPLATE
					 orderby i.Name
					 select i;
		}
	}
}