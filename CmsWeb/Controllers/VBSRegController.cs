using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;
using System.Configuration;
using UtilityExtensions;

namespace CMSWeb.Controllers
{
    [HandleError]
    public class VBSRegController : Controller
    {
        public ActionResult Index()
        {
            ViewData["header"] = ConfigurationManager.AppSettings["vbsheader"];
            var m = new Models.VBSRegModel();
            if (Request.HttpMethod.ToUpper() == "GET")
                return View(m);

            UpdateModel<Models.IVBSFormBindable>(m);
            m.ValidateModel(ModelState);
            if (ModelState.IsValid)
            {
                m.SaveVBSApp();
                HomeController.Email(ConfigurationManager.AppSettings["vbsmail"], 
                    m.parent, m.email, "VBS Registration", 
@"<p>Thank you for registering your child.
You will receive another email (with the room #) once your child has been assigned to a class.</p>
<p>We have the following information:
<pre>
{0}
</pre>
".Fmt(m.PrepareSummaryText()));

                return RedirectToAction("Confirm");
            }
            return View(m);
        }
        public ActionResult Confirm()
        {
            return View();
        }
    }
}
