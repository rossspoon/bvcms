using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMSWeb.Models;

namespace CMSWeb.Areas.Main.Controllers
{
    public class OrgSearchController : Controller
    {
        public ActionResult Index()
        {
            var m = new OrgSearchModel();
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Results(OrgSearchModel m)
        {
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DivisionIds(int id)
        {
            return View(OrgSearchModel.DivisionIds(id));
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DefaultMeetingDate(int id)
        {
            var dt = OrgSearchModel.DefaultMeetingDate(id);
            return Json(new { date = dt.Date.ToShortDateString(), time = dt.ToShortTimeString() });
        }
    }
}
