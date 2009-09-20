using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace CMSWebSetup.Controllers
{
    public class Home2Controller : Controller
    {
        public ActionResult Index()
        {
            return Redirect("/default.aspx");
        }

    }
}
