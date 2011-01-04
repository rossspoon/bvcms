using System;
using System.Linq;
using System.Web.Mvc;
using CmsData;
using CmsWeb.Models;
using UtilityExtensions;
using CmsWeb.Areas.Manage.Controllers;
using CmsWeb.Areas.OnlineReg.Models.Payments;
using System.Text;

namespace CmsWeb.Areas.OnlineReg.Controllers
{
    public partial class OnlineRegController : CmsController
    {
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PayWithCoupon(int id, string Coupon)
        {
            if (!Coupon.HasValue())
                return Json(new { error = "empty coupon" });
            var ed = DbUtil.Db.ExtraDatas.SingleOrDefault(e => e.Id == id);
            var m = Util.DeSerialize<OnlineRegModel>(ed.Data);
            string coupon = Coupon.ToUpper().Replace(" ", "");
            string admincoupon = DbUtil.Db.Setting("AdminCoupon", "ifj4ijweoij").ToUpper().Replace(" ", "");
            if (coupon == admincoupon)
                return Json(new { confirm = "/onlinereg/Confirm/{0}?TransactionID=Coupon(Admin)".Fmt(id) });
            var c = DbUtil.Db.Coupons.SingleOrDefault(cp => cp.Id == coupon);
            if (c == null)
                return Json(new { error = "coupon not found" });
            if (m.divid.HasValue)
            {
                if (c.DivId != m.divid)
                    return Json(new { error = "coupon division not match" });
            }
            else if (m.orgid != c.OrgId)
                return Json(new { error = "coupon org not match" });
            if (DateTime.Now.Subtract(c.Created).TotalHours > 24)
                return Json(new { error = "coupon expired" });
            if (c.Used.HasValue)
                return Json(new { error = "coupon already used" });
            if (c.Canceled.HasValue)
                return Json(new { error = "coupon canceled" });
            return Json(new
            {
                confirm = "/onlinereg/confirm/{0}?TransactionID=Coupon({1})"
                    .Fmt(id, coupon.Insert(8, " ").Insert(4, " "))
            });
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PayWithCoupon2(int id, string Coupon, decimal Amount)
        {
            if (!Coupon.HasValue())
                return Json(new { error = "empty coupon" });
            var ti = DbUtil.Db.Transactions.SingleOrDefault(tt => tt.Id == id);
            string coupon = Coupon.ToUpper().Replace(" ", "");
            string admincoupon = DbUtil.Db.Setting("AdminCoupon", "ifj4ijweoij").ToUpper().Replace(" ", "");
            if (coupon == admincoupon)
                return Json(new { confirm = "/onlinereg/ConfirmDuePaid/{0}?TransactionID=Coupon(Admin)&Amount={1}".Fmt(id, Amount) });
            var c = DbUtil.Db.Coupons.SingleOrDefault(cp => cp.Id == coupon);
            if (c == null)
                return Json(new { error = "coupon not found" });
            if (ti.OrgId != c.OrgId)
                return Json(new { error = "coupon org not match" });
            if (DateTime.Now.Subtract(c.Created).TotalHours > 24)
                return Json(new { error = "coupon expired" });
            if (c.Used.HasValue)
                return Json(new { error = "coupon already used" });
            if (c.Canceled.HasValue)
                return Json(new { error = "coupon canceled" });
            return Json(new
            {
                confirm = "/onlinereg/ConfirmDuePaid/{0}?TransactionID=Coupon({1})&Amount={2}"
                    .Fmt(id, coupon.Insert(8, " ").Insert(4, " "), Amount)
            });
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PayWithCouponOld(int id, string Coupon, decimal Amount)
        {
            if (!Coupon.HasValue())
                return Json(new { error = "empty coupon" });
            var ed = DbUtil.Db.ExtraDatas.SingleOrDefault(e => e.Id == id);
            var ti = Util.DeSerialize<TransactionInfo>(ed.Data.Replace("CMSWeb.Models", "CmsWeb.Models"));          
            string coupon = Coupon.ToUpper().Replace(" ", "");
            string admincoupon = DbUtil.Db.Setting("AdminCoupon", "ifj4ijweoij").ToUpper().Replace(" ", "");
            if (coupon == admincoupon)
                return Json(new { confirm = "/onlinereg/Confirm2/{0}?TransactionID=Coupon(Admin)&Amount={1}".Fmt(id, Amount) });
            var c = DbUtil.Db.Coupons.SingleOrDefault(cp => cp.Id == coupon);
            if (c == null)
                return Json(new { error = "coupon not found" });
            if (ti.orgid != c.OrgId)
                return Json(new { error = "coupon org not match" });
            if (DateTime.Now.Subtract(c.Created).TotalHours > 24)
                return Json(new { error = "coupon expired" });
            if (c.Used.HasValue)
                return Json(new { error = "coupon already used" });
            if (c.Canceled.HasValue)
                return Json(new { error = "coupon canceled" });
            return Json(new
            {
                confirm = "/onlinereg/Confirm2/{0}?TransactionID=Coupon({1})&Amount={2}"
                    .Fmt(id, coupon.Insert(8, " ").Insert(4, " "), Amount)
            });
        }
    }
}
