using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using CmsWeb.Areas.Manage.Controllers;

namespace CmsWeb.Models
{
	public class ContentModel
	{
		public IQueryable<Content> fetchHTMLFiles()
		{
			return from c in DbUtil.Db.Contents
					 where c.TypeID == DisplayController.TYPE_HTML
					 orderby c.Name
					 select c;
		}

		public IQueryable<Content> fetchTextFiles()
		{
			return from c in DbUtil.Db.Contents
					 where c.TypeID == DisplayController.TYPE_TEXT
					 orderby c.Name
					 select c;
		}

		public IQueryable<Content> fetchEmailTemplates()
		{
			return from c in DbUtil.Db.Contents
					 where c.TypeID == DisplayController.TYPE_EMAIL_TEMPLATE
					 orderby c.Name
					 select c;
		}

		public IQueryable<Content> fetchSavedDrafts()
		{
			return from c in DbUtil.Db.Contents
					 where c.TypeID == DisplayController.TYPE_SAVED_DRAFT
					 orderby c.Name
					 select c;
		}

		public static List<Role> fetchRoles()
		{
			var r = from e in DbUtil.Db.Roles
					  select e;

			var l = r.ToList();
			l.Insert( 0, new Role() { RoleId = 0, RoleName = "Everyone" } );
			return l;
		}
	}
}