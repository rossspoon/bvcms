using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMSWebCommon.Controllers
{
    public class CommonHomeController : CMSWebCommon.Controllers.CmsController
    {
        public ActionResult Index()
        {
            return Redirect("/default.aspx");
        }
    }
}
