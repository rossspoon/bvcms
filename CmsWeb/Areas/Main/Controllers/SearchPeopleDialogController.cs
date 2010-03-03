using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;
using UtilityExtensions;
using CMSPresenter;
using CMSWeb.Models;

namespace CMSWeb.Areas.Main.Controllers
{
    public class SearchPeopleDialogController : Controller
    {
        public ActionResult Index(int? id, int? origin, int? entrypoint, bool? pending, string from)
        {
            var m = new SearchPeopleDialogModel();
            if (origin.HasValue)
                m.Origin = origin;
            if (entrypoint.HasValue)
                m.EntryPoint = entrypoint;
            return View(m);
        }
        public ActionResult Rows(SearchPeopleDialogModel m)
        {
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult AddNew()
        {
            var m = new SearchPeopleDialogModel();
            UpdateModel(m);
            var err = m.ValidateAddNew();
            if (err.HasValue())
                return Json(new { err = err });
            var pid = m.AddNewPerson();
            if (!pid.HasValue)
                return Json(new { err = "could not add person" });
            return Json(new { PeopleId = pid });
        }
    }
}
