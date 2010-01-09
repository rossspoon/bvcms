using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CmsData;

namespace CMSWeb.Areas.Setup.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersCanEmailForController : CmsController
    {
        public ActionResult Index()
        {
            return View(DbUtil.Db.UserCanEmailFors.Select(u => u));
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(int UserId, int CanEmailFor)
        {
            var u = new UserCanEmailFor { UserId = UserId, CanEmailFor = CanEmailFor };
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
