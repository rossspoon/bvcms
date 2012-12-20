using System;
using System.Collections;
using System.Web;
using System.Web.Mvc;
using CmsWeb.Areas.Finance.Models;


namespace CmsWeb.Areas.Finance.Controllers
{
    [Authorize(Roles = "Finance")]
    public class BundlesController : CmsStaffController
    {
        public ActionResult Index()
        {
            var m = new BundlesModel();
            return View(m);
        }
        [HttpPost]
        public ActionResult Results(BundlesModel m)
        {
            return View(m);
        }
    }
}
