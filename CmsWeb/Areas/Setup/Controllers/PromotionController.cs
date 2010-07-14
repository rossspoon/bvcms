using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CmsData;
using UtilityExtensions;

namespace CmsWeb.Areas.Setup.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PromotionController : CmsStaffController
    {
        public ActionResult Index()
        {
            var m = new Models.PromotionModel();
            return View(m);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create()
        {
            var m = new Promotion();
            DbUtil.Db.Promotions.InsertOnSubmit(m);
            DbUtil.Db.SubmitChanges();
            return Redirect("/Setup/Promotion/");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult Edit(string id, string value)
        {
            var iid = id.Substring(1).ToInt();
            var c = new ContentResult();
            c.Content = value;
            var pro = DbUtil.Db.Promotions.SingleOrDefault(p => p.Id == iid);
            if (pro == null)
                return c;
            switch (id.Substring(0, 1))
            {
                case "d":
                    pro.Description = value;
                    break;
                case "s":
                    pro.Sort = value;
                    break;
            }
            DbUtil.Db.SubmitChanges();
            return c;
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult EditDiv(string id, string value)
        {
            var iid = id.Substring(1).ToInt();
            var pro = DbUtil.Db.Promotions.SingleOrDefault(m => m.Id == iid);
            var fts = id.Substring(0, 1);
            switch (fts)
            {
                case "f":
                    pro.FromDivId = value.ToInt();
                    break;
                case "t":
                    pro.ToDivId = value.ToInt();
                    break;
            }
            DbUtil.Db.SubmitChanges();
            var c = new ContentResult();
            if (fts == "f")
                c.Content = pro.FromDivision.Name;
            else if (fts == "t")
                c.Content = pro.ToDivision.Name;
            return c;
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public EmptyResult Delete(string id)
        {
            var iid = id.Substring(1).ToInt();
            var pro = DbUtil.Db.Promotions.SingleOrDefault(m => m.Id == iid);
            if (pro == null)
                return new EmptyResult();
            DbUtil.Db.Promotions.DeleteOnSubmit(pro);
            DbUtil.Db.SubmitChanges();
            return new EmptyResult();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult DivisionCodes(int id)
        {
            var q = from c in DbUtil.Db.Divisions
                    orderby c.Name
                    where c.DivOrgs.Any(od => od.Organization.DivOrgs.Any(od2 => od2.Division.ProgId == id))
                    select new
                    {
                        Code = c.Id.ToString(),
                        Value = c.Name,
                    };
            return Json(q.ToDictionary(k => k.Code, v => v.Value));
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Promote(string id)
        {
            var iid = id.Substring(1).ToInt();
            var m = new Models.PromotionModel();
            UpdateModel(m);
            m.Promote(iid);
            return RedirectToAction("Index");
        }

    }
}
