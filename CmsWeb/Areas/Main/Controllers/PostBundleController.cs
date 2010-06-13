using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;
using UtilityExtensions;
using CMSWeb.Models;
using System.Web.Script.Serialization;

namespace CMSWeb.Areas.Main.Controllers
{
    [Authorize(Roles = "Testing")]
    [Authorize(Roles = "Finance")]
    public class PostBundleController : Controller
    {
        public ActionResult Index(int id)
        {
            var m = new PostBundleModel(id);
            if (m.bundle == null)
                return Content("no bundle");
            if (m.bundle.BundleStatusId == (int)BundleHeader.StatusCode.Closed)
                return Content("bundle closed");
            m.fund = m.bundle.FundId.Value;
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetName(PostBundleModel m)
        {
            var s = m.GetNameFromPid();
            return Content(s);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PostRow(PostBundleModel m)
        {
            return Json(m.PostContribution());
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateRow(PostBundleModel m)
        {
            return Json(m.UpdateContribution());
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteRow(PostBundleModel m)
        {
            return Json(m.DeleteContribution());
        }
        public ActionResult Names(string q, int limit)
        {
            return Content(PostBundleModel.Names(q, limit));
        }
        public ActionResult FundTotals(int id)
        {
            var m = new PostBundleModel(id);
            return View(m);
        }
        public ActionResult Batch(string text)
        {
            if (Request.HttpMethod.ToUpper() == "GET")
            {
                ViewData["text"] = "";
                return View();
            }
            var id = PostBundleModel.BatchProcess(text);
            return Redirect("/PostBundle/Index/" + id);
        }
    }
}
