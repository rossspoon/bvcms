using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UtilityExtensions;
using CmsData;
using CMSWeb.Models;

namespace CMSWeb.Areas.Main.Controllers
{
    public class CouponController : CmsStaffController
    {
        public ActionResult Index()
        {
            //string c = Util.RandomPassword(12);
            //c = c.Insert(8, " ").Insert(4, " ");
            //return Content("<span style='font-family:courier new'>{0}</span>".Fmt(c));
            var c = new CouponModel();
            return View(c);
        }
    }
}
