using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CmsData;
using CMSWeb.Models;
using UtilityExtensions;

namespace CMSWeb.Areas.Manage.Controllers
{
    public class GroupsController : CmsStaffController
    {
        public ActionResult Index()
        {
            var m = new GroupsModel();
            UpdateModel(m);
            return View(m);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AssignToGroup()
        {
            var m = new GroupsModel();
            UpdateModel(m);
            m.AssignToGroup();
            return RedirectToAction("Index");
        }
        public ActionResult List()
        {
            var m = new GroupsModel();
            UpdateModel(m);
            return PartialView("List", m);
        }
        public ActionResult Coaches()
        {
            var m = new GroupsModel();
            UpdateModel(m);
            return View(m);
        }
        public ActionResult All(int? id)
        {
            if (!id.HasValue)
                return Content("no division id");
            var m = new GroupsModel();
            m.DivId = id;
            UpdateModel(m);
            return View(m);
        }
    }
}
