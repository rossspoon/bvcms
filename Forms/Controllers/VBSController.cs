using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;

namespace Forms.Controllers
{
    [HandleError]
    public class VBSController : Controller
    {
        public ActionResult Register()
        {
            var m = new Models.VBSModel();
            if (Request.HttpMethod.ToUpper() == "GET")
                return View(m);

            UpdateModel<Models.IVBSFormBindable>(m);
            m.ValidateModel(ModelState);
            if (ModelState.IsValid)
            {
                m.SaveVBSApp();
                HomeController.Email("vbs@bellevue.org", m.parent, m.email, "VBS Registration", 
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
        public ActionResult Index()
        {
            return RedirectToAction("Register");
        }
    }
}
