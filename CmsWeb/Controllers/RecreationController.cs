using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CmsData;
using CMSWeb.Models;
using UtilityExtensions;

namespace CMSWeb.Controllers
{
    public class RecreationController : Controller
    {
        public ActionResult Index()
        {
            var m = new RecreationModel();
            UpdateModel(m);
            return View(m);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AssignToTeam()
        {
            var m = new RecreationModel();
            UpdateModel(m);
            m.AssignToTeam();
            return RedirectToAction("Index");
        }
        public ActionResult List()
        {
            var m = new RecreationModel();
            UpdateModel(m);
            return PartialView("List", m);
        }
        public ActionResult Detail(int id)
        {
            var m = new RecDetailModel(id);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Create(int id, int oid)
        {
            var reg = new RecReg { PeopleId = id, Uploaded = DateTime.Now, OrgId=oid };
            DbUtil.Db.RecRegs.InsertOnSubmit(reg);
            DbUtil.Db.SubmitChanges();
            return Json(new { id = reg.Id });
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update(int Id)
        {
            var m = new RecDetailModel(Id);
            UpdateModel(m);
            DbUtil.Db.SubmitChanges();
            return new RedirectResult("/Recreation/Detail/" + Id);
        }
   }
}
