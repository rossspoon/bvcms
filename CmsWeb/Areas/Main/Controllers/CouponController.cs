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
            var c = new CouponModel();
            return View(c);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(CouponModel m)
        {
            m.CreateCoupon();
            return View(m);
        }
        public ActionResult List()
        {
            var c = new CouponModel();
            return View(c);
        }

    }
}
