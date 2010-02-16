using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CmsData;
using UtilityExtensions;

namespace CMSWeb.Areas.Setup.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RecreationController : CmsController
    {
        public ActionResult Index()
        {
            if (DbUtil.Settings("RecreationProgramId", "") == string.Empty)
                ModelState.AddModelError("_FORM", "RecreationProgramId required in Settings");
            if (DbUtil.Settings("RecEntry", "") == string.Empty)
                ModelState.AddModelError("_FORM", "RecEntry required in Settings");
            if (DbUtil.Settings("RecOrigin", "") == string.Empty)
                ModelState.AddModelError("_FORM", "RecOrigin required in Settings");

            if (!ModelState.IsValid)
                return View("Errors");

            var q = from c in DbUtil.Db.Divisions
                    where c.ProgId == DbUtil.Settings("RecreationProgramId", "0").ToInt()
                    where c.RecLeagues.Count() == 0
                    orderby c.Name
                    select new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name,
                    };
            ViewData["leagues"] = q;
            var m = from league in DbUtil.Db.RecLeagues
                    orderby league.Division.Name
                    select league;
            return View(m);
        }
        public ActionResult AgeDivisions(int? id)
        {
            if (!id.HasValue)
                return Content("no league");
            var league = DbUtil.Db.RecLeagues.SingleOrDefault(l => l.DivId == id);

            var m = from o in DbUtil.Db.Organizations
                    where o.DivisionId == id
                    orderby o.OrganizationName
                    select o;
            ViewData["League"] = league.Division.Name;
            ViewData["id"] = league.DivId;
            return View(m);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateLeague(int? id)
        {
            if (!id.HasValue)
                ModelState.AddModelError("league", "Must select league");
            var m = new RecLeague { DivId = id.Value };
            DbUtil.Db.RecLeagues.InsertOnSubmit(m);
            DbUtil.Db.SubmitChanges();
            return RedirectToAction("Index");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult Edit(string id, string value)
        {
            var iid = id.Substring(1).ToInt();
            var c = Content(value);
            var org = DbUtil.Db.Organizations.SingleOrDefault(p => p.OrganizationId == iid);
            if (org == null)
                return c;
            switch (id.Substring(0, 1))
            {
                case "s":
                    org.GradeAgeStart = value.ToInt();
                    break;
                case "e":
                    org.GradeAgeEnd = value.ToInt();
                    break;
                case "f":
                    org.Fee = Decimal.Parse(value);
                    break;
                case "g":
                    org.GenderId = value.ToInt();
                    c = Content(org.Gender.Description);
                    break;
            }
            DbUtil.Db.SubmitChanges();
            return c;
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult EditLeague(string id, string value)
        {
            var iid = id.Substring(1).ToInt();
            var c = new ContentResult();
            c.Content = value;
            var league = DbUtil.Db.RecLeagues.SingleOrDefault(l => l.DivId == iid);
            if (league == null)
                return c;
            switch (id.Substring(0, 1))
            {
                case "a":
                    DateTime dt;
                    if (!DateTime.TryParse(value, out dt))
                        league.AgeDate = null;
                    else
                        league.AgeDate = value;
                    break;
                case "e":
                    league.ExtraFee = Decimal.Parse(value);
                    break;
                case "t":
                    league.ShirtFee = Decimal.Parse(value);
                    break;
                case "z":
                    if (DateTime.TryParse(value, out dt))
                        league.ExpirationDt = dt.ToString("M/d");
                    else
                        league.ExpirationDt = null;
                    break;
            }
            DbUtil.Db.SubmitChanges();
            return c;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public EmptyResult DeleteLeague(string id)
        {
            var iid = id.Substring(1).ToInt();
            var league = DbUtil.Db.RecLeagues.SingleOrDefault(m => m.DivId == iid);
            if (league == null)
                return new EmptyResult();
            DbUtil.Db.RecLeagues.DeleteOnSubmit(league);
            DbUtil.Db.SubmitChanges();
            return new EmptyResult();
        }
        [AcceptVerbs(HttpVerbs.Get)]
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
            return Json(q.ToDictionary(k => k.Code, v => v.Value), JsonRequestBehavior.AllowGet);
        }
    }
}
