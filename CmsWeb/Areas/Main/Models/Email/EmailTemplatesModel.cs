using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using CmsData.Codes;
using CmsWeb.Areas.Manage.Controllers;
using UtilityExtensions;

namespace CmsWeb.Areas.Main.Models
{
	public class EmailTemplatesModel
	{
	    public bool wantparents { get; set; }
	    public int queryid { get; set; }
		public IQueryable<Content> fetchTemplates()
		{
			var currentRoleIds = DbUtil.Db.CurrentRoleIds();
			var isadmin = HttpContext.Current.User.IsInRole("Admin");

			return from i in DbUtil.Db.Contents
					 where i.TypeID == ContentTypeCode.TypeEmailTemplate
					 where isadmin || i.RoleID == 0 || currentRoleIds.Contains(i.RoleID)
					 orderby i.Name
					 select i;
		}

        public Content fetchTemplateByName( string name )
        {
            var currentRoleIds = DbUtil.Db.CurrentRoleIds();
            var isadmin = HttpContext.Current.User.IsInRole("Admin");

            return (from i in DbUtil.Db.Contents
                    where i.Name == name
                    select i).SingleOrDefault();
        }

		public IQueryable<Content> fetchDrafts()
		{
			var currentRoleIds = DbUtil.Db.CurrentRoleIds();
			var isadmin = HttpContext.Current.User.IsInRole("Admin");

			return from i in DbUtil.Db.Contents
					 where i.TypeID == ContentTypeCode.TypeSavedDraft
					 where isadmin || i.RoleID == 0 || i.OwnerID == Util.UserId || currentRoleIds.Contains(i.RoleID)
					 orderby (i.OwnerID == Util.UserId ? 1 : 0) descending, i.Name
					 select i;
		}
	}
}