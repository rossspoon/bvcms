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
using UtilityExtensions;
using System.Web.Routing;
using CMSWeb;
using CMSWeb.Models;
using CmsData;

namespace CMSWeb.Controllers
{
    public class QueryBuilderController : Controller
    {
        public QueryBuilderController()
        {
            ViewData["Title"] = "QueryBuilder";
            ViewData["OnQueryBuilder"] = "true";
        }
        public ActionResult Main(int? id, int? run)
        {
            var m = new QueryModel { QueryId = id };
            DbUtil.LogActivity("QueryBuilder");
            if (run.HasValue)
                m.ShowResults = true;
            m.LoadScratchPad();
            ViewData["queryid"] = m.QueryId;
            return View(m);
        }
        public JsonResult SelectCondition(int id, string ConditionName)
        {
            var m = new QueryModel { ConditionName = ConditionName, SelectedId = id };
            m.LoadScratchPad();
            m.SetState();
            return Json(m);
        }
        public JsonResult GetCodes(string Comparison, string ConditionName)
        {
            var m = new QueryModel { Comparison = Comparison, ConditionName = ConditionName };
            m.SetCodes();
            return Json(new
            { 
                CodesVisible = m.CodesVisible, 
                CodeVisible = m.CodeVisible, 
                CodeData = m.CodeData, 
                SelectMultiple = m.SelectMultiple 
            });
        }
        public JsonResult EditCondition(int id)
        {
            var m = new QueryModel { SelectedId = id };
            m.LoadScratchPad();
            m.EditCondition();
            return Json(m);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddToGroup()
        {
            var m = new QueryModel();
            UpdateModel<IQBUpdateable>(m);
            m.LoadScratchPad();
            m.AddConditionToGroup();
            return PartialView("Conditions", m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Add()
        {
            var m = new QueryModel();
            UpdateModel<IQBUpdateable>(m);
            m.LoadScratchPad();
            m.AddConditionAfterCurrent();
            return PartialView("Conditions", m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update()
        {
            var m = new QueryModel();
            UpdateModel<IQBUpdateable>(m);
            m.LoadScratchPad();
            m.UpdateCondition();
            return PartialView("Conditions", m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Remove()
        {
            var m = new QueryModel();
            UpdateModel<IQBUpdateable>(m);
            m.LoadScratchPad();
            m.DeleteCondition();
            return PartialView("Conditions", m);
        }
        public JsonResult GetDivisions(int id)
        {
            var m = new QueryModel();
            return Json(new { Divisions = m.Divisions(id), Organizations = m.Organizations(0) });
        }
        public JsonResult GetOrganizations(int id)
        {
            var m = new QueryModel();
            return Json(m.Organizations(id));
        }
        public JsonResult SavedQueries()
        {
            var m = new QueryModel();
            return Json(m.SavedQueries());
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveQuery(string SavedQueryDesc, bool IsPublic)
        {
            var m = new QueryModel { SavedQueryDesc = SavedQueryDesc, IsPublic = IsPublic };
            m.LoadScratchPad();
            m.SaveQuery();
            var c = new ContentResult();
            c.Content = m.Description;
            return c;
        }
        public ActionResult Results()
        {
            var m = new QueryModel();
            UpdateModel<IQBUpdateable>(m);
            m.LoadScratchPad();
            return PartialView(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult ToggleTag(int id)
        {
            var r = Person.ToggleTag(id, Util.CurrentTagName, Util.CurrentTagOwnerId, DbUtil.TagTypeId_Personal);
            DbUtil.Db.SubmitChanges();
            return Json(new { HasTag = r });
        }
    }
}
