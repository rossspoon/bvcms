using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CmsData;
using CMSWeb.Models;
using UtilityExtensions;

namespace CMSWeb.Controllers
{
    public class RecreationSetupController : Controller
    {
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            if (DbUtil.Settings("RecreationProgramId") == string.Empty)
                ModelState.AddModelError("_FORM", "RecreationProgramId required in Settings");
            if (DbUtil.Settings("RecEntry") == string.Empty)
                ModelState.AddModelError("_FORM", "RecEntry required in Settings");
            if (DbUtil.Settings("RecOrigin") == string.Empty)
                ModelState.AddModelError("_FORM", "RecOrigin required in Settings");

            if (!ModelState.IsValid)
                return View("Errors");

            var m = from r in DbUtil.Db.RecAgeDivisions
                    orderby r.OrgId != null ? r.Organization.OrganizationName : ""
                    select r;
            return View(m);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create()
        {
            var m = new RecAgeDivision();
            DbUtil.Db.RecAgeDivisions.InsertOnSubmit(m);
            DbUtil.Db.SubmitChanges();
            return Redirect("/RecreationSetup/");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult Edit(string id, string value)
        {
            var iid = id.Substring(1).ToInt();
            var c = new ContentResult();
            c.Content = value;
            var rec = DbUtil.Db.RecAgeDivisions.SingleOrDefault(p => p.Id == iid);
            if (rec == null)
                return c;
            switch (id.Substring(0, 1))
            {
                case "a":
                    DateTime dt;
                    if (DateTime.TryParse(value, out dt))
                        rec.AgeDate = dt.ToString("M/d");
                    else
                        rec.AgeDate = null;
                    break;
                case "s":
                    rec.StartAge = value.ToInt();
                    break;
                case "e":
                    rec.EndAge = value.ToInt();
                    break;
                case "f":
                    rec.Fee = Decimal.Parse(value);
                    break;
                case "p":
                    rec.ExtraFee = Decimal.Parse(value);
                    break;
                case "z":
                    if (DateTime.TryParse(value, out dt))
                        rec.ExpirationDt = dt.ToString("M/d");
                    else
                        rec.ExpirationDt = null;
                    break;
            }
            DbUtil.Db.SubmitChanges();
            return c;
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult EditDiv(string id, string value)
        {
            var iid = id.Substring(1).ToInt();
            var rec = DbUtil.Db.RecAgeDivisions.SingleOrDefault(m => m.Id == iid);
            rec.DivId = value.ToInt();
            rec.OrgId = null;
            DbUtil.Db.SubmitChanges();
            return Content(rec.Division.Name);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult EditOrg(string id, string value)
        {
            var iid = id.Substring(1).ToInt();
            var rec = DbUtil.Db.RecAgeDivisions.SingleOrDefault(m => m.Id == iid);
            rec.OrgId = value.ToInt();
            DbUtil.Db.SubmitChanges();
            return Content(rec.Organization.OrganizationName);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult EditGender(string id, string value)
        {
            var iid = id.Substring(1).ToInt();
            var rec = DbUtil.Db.RecAgeDivisions.SingleOrDefault(m => m.Id == iid);
            rec.GenderId = value.ToInt();
            DbUtil.Db.SubmitChanges();
            return Content(rec.Gender.Description);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public EmptyResult Delete(string id)
        {
            var iid = id.Substring(1).ToInt();
            var rec = DbUtil.Db.RecAgeDivisions.SingleOrDefault(m => m.Id == iid);
            if (rec == null)
                return new EmptyResult();
            DbUtil.Db.RecAgeDivisions.DeleteOnSubmit(rec);
            DbUtil.Db.SubmitChanges();
            return new EmptyResult();
        }
        public JsonResult Divisions()
        {
            var q = from c in DbUtil.Db.Divisions
                    orderby c.Name
                    where c.ProgId == DbUtil.Settings("RecreationProgramId").ToInt()
                    select new
                    {
                        Code = c.Id.ToString(),
                        Value = c.Name,
                    };
            return Json(q.ToDictionary(k => k.Code, v => v.Value));
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Organizations(string id)
        {
            var iid = id.Substring(1).ToInt();
            var rec = DbUtil.Db.RecAgeDivisions.SingleOrDefault(m => m.Id == iid);
            var q = from c in DbUtil.Db.Organizations
                    orderby c.OrganizationName
                    where c.DivOrgs.Any(od => od.DivId == rec.DivId)
                    select new
                    {
                        Code = c.OrganizationId.ToString(),
                        Value = c.OrganizationName,
                    };
            return Json(q.ToDictionary(k => k.Code, v => v.Value));
        }
        public JsonResult Genders()
        {
            var q = from c in DbUtil.Db.Genders
                    orderby c.Id
                    where c.Id <= 2
                    select new
                    {
                        Code = c.Id.ToString(),
                        Value = c.Description,
                    };
            return Json(q.ToDictionary(k => k.Code, v => v.Value));
        }
    }
}
