using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CmsData;
using UtilityExtensions;

namespace CmsWeb.Areas.Setup.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : CmsStaffController
    {
        public ActionResult Index()
        {
            var r = DbUtil.Db.Roles.AsEnumerable();
            return View(r);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create()
        {
            var r = new Role { RoleName = "NEW" };
            DbUtil.Db.Roles.InsertOnSubmit(r);
            DbUtil.Db.SubmitChanges();
            return Redirect("/Setup/Roles/");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult Edit(string id, string value)
        {
            var a = id.Split('.');
            var c = new ContentResult();
            c.Content = value;
            var role = DbUtil.Db.Roles.SingleOrDefault(m => m.RoleId == a[1].ToInt());
            if (role == null)
                return c;
            switch (a[0])
            {
                case "RoleName":
                    role.RoleName = value;
                    break;
            }
            DbUtil.Db.SubmitChanges();
            return c;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string id)
        {
            id = id.Substring(1);
            var role = DbUtil.Db.Roles.SingleOrDefault(m => m.RoleId == id.ToInt());
            if (role == null)
                return new EmptyResult();
			if (role.UserRoles.Any())
				return Content("users have that role, not deleted");
            DbUtil.Db.Roles.DeleteOnSubmit(role);
            DbUtil.Db.SubmitChanges();
            return new EmptyResult();
        }
    }
}
