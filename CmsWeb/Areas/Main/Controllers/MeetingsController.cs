using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsWeb.Models;

namespace CmsWeb.Areas.Main.Controllers
{
    public class MeetingsController : CmsStaffController
    {
        public ActionResult Calendar()
        {
            var m = new MeetingsModel();
            return View();
        }
    }
}
