using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CmsWeb.Areas.Public.Controllers
{
    public class ExternalServicesController : Controller
    {
        public ActionResult Index()
        {
            return Content( "Success!" );
        }

        public ActionResult PMMResults()
        {
            StreamReader reader = new StreamReader( Request.InputStream);
            string text = reader.ReadToEnd();

            return Content("OK");
        }
    }
}
