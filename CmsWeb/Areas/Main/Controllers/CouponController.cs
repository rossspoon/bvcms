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
    [Authorize(Roles="Coupon")]
    public class CouponController : CmsStaffController
    {
        public ActionResult Index()
        {
            var m = new CouponModel();
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(CouponModel m)
        {
            m.CreateCoupon();
            return View(m);
        }
        public ActionResult Cancel(string id)
        {
            var c = DbUtil.Db.Coupons.SingleOrDefault(cp => cp.Id == id);
            if (!c.Canceled.HasValue)
            {
                c.Canceled = DateTime.Now;
                DbUtil.Db.SubmitChanges();
            }
            var m = new CouponModel();
            return View("List", m);
        }
        public ActionResult List()
        {
            var m = new CouponModel();
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult List(CouponModel m)
        {
            return View(m);
        }
    }
}
