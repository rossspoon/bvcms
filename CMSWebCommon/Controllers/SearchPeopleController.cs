/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CmsData;
using UtilityExtensions;

namespace CMSWebCommon.Controllers
{
    public class SearchPeopleController : Controller
    {
        public ActionResult Index(int? origin, int? entrypoint)
        {
            var m = new Models.SearchPeopleModel();
            UpdateModel(m);
            if (origin.HasValue)
                m.Origin = origin;
            if (entrypoint.HasValue)
                m.EntryPoint = entrypoint;
            return View(m);
        }
        public ActionResult Rows(int id)
        {
            var m = new Models.SearchPeopleModel();
            UpdateModel(m);
            m.Page = id;
            return PartialView(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult AddNew()
        {
            var m = new Models.SearchPeopleModel();
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
