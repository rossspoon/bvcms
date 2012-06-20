using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using CmsWeb.Areas.Manage.Controllers;

namespace CmsWeb.Areas.Main.Models
{
	public class EmailTemplatesModel
	{
		public IQueryable<Content> fetchTemplates()
		{
			var currentRoleIds = DbUtil.Db.CurrentRoleIds();
			var isadmin = HttpContext.Current.User.IsInRole("Admin");

			return from i in DbUtil.Db.Contents
					 where i.TypeID == DisplayController.TYPE_EMAIL_TEMPLATE
					 where isadmin || i.RoleID == 0 || currentRoleIds.Contains(i.RoleID)
					 orderby i.Name
					 select i;
		}

		public IQueryable<Content> fetchDrafts()
		{
			var currentRoleIds = DbUtil.Db.CurrentRoleIds();
			var isadmin = HttpContext.Current.User.IsInRole("Admin");

			return from i in DbUtil.Db.Contents
					 where i.TypeID == DisplayController.TYPE_SAVED_DRAFT
					 where isadmin || i.RoleID == 0 || currentRoleIds.Contains(i.RoleID)
					 orderby i.Name
					 select i;
		}
	}
}