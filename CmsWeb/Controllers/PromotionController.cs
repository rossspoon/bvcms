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
    public class PromotionController : Controller
    {
        public ActionResult Index()
        {
            var m = new PromotionModel();
            UpdateModel(m);
            return View(m);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AssignPending()
        {
            var m = new PromotionModel();
            UpdateModel(m);
            m.AssignPending();
            return RedirectToAction("Index");
        }
        public ActionResult List()
        {
            var m = new PromotionModel();
            UpdateModel(m);
            return PartialView("List", m);
        }
    }
}
