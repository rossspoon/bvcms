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
    public class UsersCanEmailForController : CmsStaffController
    {
        public ActionResult Index()
        {
            return View(DbUtil.Db.UserCanEmailFors.Select(u => u));
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(int? UserId, int? CanEmailFor)
        {
			if (!UserId.HasValue || !CanEmailFor.HasValue)
				return RedirectShowError("missing id");
            var user1 = DbUtil.Db.Users.SingleOrDefault(uu => uu.UserId == UserId);
            if (user1 == null)
                return RedirectShowError("no such user " + UserId);
            var user2 = DbUtil.Db.Users.SingleOrDefault(uu => uu.UserId == CanEmailFor);
            if (user2 == null)
                return RedirectShowError("no such user " + CanEmailFor);
            var u = DbUtil.Db.UserCanEmailFors.SingleOrDefault(uu => uu.UserId == UserId && uu.CanEmailFor == CanEmailFor);
            if (u != null)
                return RedirectShowError("already exists");

            u = new UserCanEmailFor { UserId = UserId.Value, CanEmailFor = CanEmailFor.Value };
            DbUtil.Db.UserCanEmailFors.InsertOnSubmit(u);
            DbUtil.Db.SubmitChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id, int CanEmailFor)
        {
            var um = DbUtil.Db.UserCanEmailFors.Single(u => u.UserId == id && u.CanEmailFor == CanEmailFor);
            DbUtil.Db.UserCanEmailFors.DeleteOnSubmit(um);
            DbUtil.Db.SubmitChanges();
            return RedirectToAction("Index");
        }
    }
}
