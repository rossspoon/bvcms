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
using CmsWeb;
using CmsWeb.Models;
using CmsData;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace CmsWeb.Areas.Main.Controllers
{
    public class QueryBuilderController : CmsStaffController
    {
        public ActionResult NewQuery()
        {
            var qb = DbUtil.Db.QueryBuilderScratchPad();
            qb.CleanSlate(DbUtil.Db);
            return RedirectToAction("Main");
        }
        public ActionResult Main(int? id, int? run)
        {
            ViewData["Title"] = "QueryBuilder";
            ViewData["OnQueryBuilder"] = "true";
            ViewData["TagAction"] = "/QueryBuilder/TagAll/";
            ViewData["UnTagAction"] = "/QueryBuilder/UnTagAll/";
            ViewData["AddContact"] = "/QueryBuilder/AddContact/";
            ViewData["AddTasks"] = "/QueryBuilder/AddTasks/";
            var m = new QueryModel { QueryId = id };
            DbUtil.LogActivity("QueryBuilder");
            if (run.HasValue)
                m.ShowResults = true;
            m.LoadScratchPad();
            ViewData["queryid"] = m.QueryId;
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult SelectCondition(int id, string ConditionName)
        {
            var m = new QueryModel { ConditionName = ConditionName, SelectedId = id };
            m.LoadScratchPad();
            m.SetVisibility();

            m.TextValue = "";
            m.Comparison = "";
            m.IntegerValue = "";
            m.DateValue = "";
            m.CodeValue = "";
            m.CodeValues = new string[0];
            m.Days = "";
            m.Age = "";
            m.Program = 0;
            m.Quarters = "";
            m.StartDate = "";
            m.EndDate = "";

            return Json(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
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
        [AcceptVerbs(HttpVerbs.Post)]
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
            if (Validate(m))
                m.AddConditionToGroup();
            return PartialView("TryConditions", m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Add()
        {
            var m = new QueryModel();
            UpdateModel<IQBUpdateable>(m);
            m.LoadScratchPad();
            if (Validate(m))
                m.AddConditionAfterCurrent();
            return PartialView("TryConditions", m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update()
        {
            var m = new QueryModel();
            UpdateModel<IQBUpdateable>(m);
            m.LoadScratchPad();
            if (Validate(m))
                m.UpdateCondition();
            return PartialView("TryConditions", m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Remove()
        {
            var m = new QueryModel();
            UpdateModel<IQBUpdateable>(m);
            m.LoadScratchPad();
            m.DeleteCondition();
            return Json(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult InsGroupAbove(int id)
        {
            var m = new QueryModel { SelectedId = id };
            m.LoadScratchPad();
            m.InsertGroupAbove();
            var c = new ContentResult();
            c.Content = m.QueryId.ToString();
            return c;
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CopyAsNew(int id)
        {
            var m = new QueryModel { SelectedId = id };
            m.LoadScratchPad();
            m.CopyAsNew();
            var c = new ContentResult();
            c.Content = m.QueryId.ToString();
            return c;
        }
        public ActionResult Conditions()
        {
            var m = new QueryModel();
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetDivisions(int id)
        {
            var m = new QueryModel();
            return Json(new { Divisions = m.Divisions(id), Organizations = m.Organizations(0) });
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetOrganizations(int id)
        {
            var m = new QueryModel();
            return Json(m.Organizations(id));
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult SavedQueries()
        {
            var m = new QueryModel();
            return Json(m.SavedQueries()); ;
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveQuery()
        {
            var m = new QueryModel();
            UpdateModel(m);
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
            var r = Person.ToggleTag(id, Util2.CurrentTagName, Util2.CurrentTagOwnerId, DbUtil.TagTypeId_Personal);
            DbUtil.Db.SubmitChanges();
            return Json(new { HasTag = r });
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult TagAll()
        {
            var m = new QueryModel();
            m.LoadScratchPad();
            m.TagAll();
            var c = new ContentResult();
            c.Content = "Remove";
            return c;
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult UnTagAll()
        {
            var m = new QueryModel();
            m.LoadScratchPad();
            m.UnTagAll();
            var c = new ContentResult();
            c.Content = "Add";
            return c;
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult AddContact()
        {
            var m = new QueryModel();
            m.LoadScratchPad();
            var cid = CmsData.Contact.AddContact(m.QueryId.Value);
            return Content("/Contact.aspx?id=" + cid);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddTasks()
        {
            var m = new QueryModel();
            m.LoadScratchPad();
            var c = new ContentResult();
            c.Content = Task.AddTasks(m.QueryId.Value).ToString();
            return c;
        }

        private bool Validate(QueryModel m)
        {
            m.SetVisibility();
            DateTime dt = DateTime.MinValue;
            if (m.StartDateVisible)
                if (!DateTime.TryParse(m.StartDate, out dt) || dt.Year <= 1900 || dt.Year >= 2200)
                    m.Errors.Add("StartDate", "invalid");
            if (m.EndDateVisible && m.EndDate.HasValue())
                if (!DateTime.TryParse(m.EndDate, out dt) || dt.Year <= 1900 || dt.Year >= 2200)
                    m.Errors.Add("EndDate", "invalid");
            int i = 0;
            if (m.DaysVisible && !int.TryParse(m.Days, out i))
                m.Errors.Add("Days", "must be integer");
            if (i > 10000)
                m.Errors.Add("Days", "days > 1000");
            if (m.AgeVisible && !int.TryParse(m.Age, out i))
                m.Errors.Add("Age", "must be integer");

            //if (m.QuartersVisible && !Regex.IsMatch(m.Quarters, "^\d+([,-](1|2|3|4))*$"))
            //    m.Errors.Add("Quarters", "need integers separated by , or -");
            if (m.ViewVisible && !m.View.HasValue())
                m.Errors.Add("View", "choose View");

            if (m.IntegerVisible && !m.Comparison.EndsWith("Null") && !int.TryParse(m.IntegerValue, out i))
                m.Errors.Add("IntegerValue", "need integer");

            if (m.TagsVisible && string.Join(",", m.Tags).Length > 500)
                m.Errors.Add("tagvalues", "too many tags selected");

            decimal d;
            if (m.NumberVisible && !m.Comparison.EndsWith("Null") && !decimal.TryParse(m.NumberValue, out d))
                m.Errors.Add("NumberValue", "need number");

            if (m.CodesVisible && m.CodeValues.Length == 0)
                m.Errors.Add("CodeValues", "must select item(s)");

            if (m.DateVisible && !m.Comparison.EndsWith("Null"))
                if (!DateTime.TryParse(m.DateValue, out dt) || dt.Year <= 1900 || dt.Year >= 2200)
                    m.Errors.Add("DateValue", "need valid date");

            if (m.Comparison == "Contains")
                if (!m.TextValue.HasValue())
                    m.Errors.Add("TextValue", "cannot be empty");

            return m.Errors.Count == 0;
        }
        public ActionResult Export()
        {
            var m = new QueryModel();
            m.LoadScratchPad();
            return new QBExportResult(m.QueryId.Value);
        }
        [HttpGet]
        public ActionResult Import()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Import(string name, string text)
        {
            var x = XDocument.Parse(text);
            QueryBuilderClause c = null;
            foreach (var xc in x.Root.Elements())
            {
                if (name.HasValue())
                    c = InsertClause(xc, null, name);
                else
                    c = InsertClause(xc, null, Attribute(xc, "Description"));
            }
            return Redirect("/QueryBuilder/Main/" + c.QueryId);
        }
        private QueryBuilderClause InsertClause(XElement r, int? pid, string name=null)
        {
            var c = new QueryBuilderClause
            {
                Field = Attribute(r, "Field"),
                GroupId = pid,
                ClauseOrder = Attribute(r, "ClauseOrder").ToInt(),
                Comparison = Attribute(r, "Comparison"),
                TextValue = Attribute(r, "TextValue"),
                DateValue = AttributeDate(r, "DateValue"),
                CodeIdValue = Attribute(r, "CodeIdValue"),
                StartDate = AttributeDate(r, "StartDate"),
                EndDate = AttributeDate(r, "EndDate"),
                Program = Attribute(r, "Program").ToInt(),
                Division = Attribute(r, "Division").ToInt(),
                Organization = Attribute(r, "Organization").ToInt(),
                Days = Attribute(r, "Days").ToInt(),
                Quarters = Attribute(r, "Quarters"),
                Tags = Attribute(r, "Tags"),
                Schedule = Attribute(r, "Schedule").ToInt(),
                Age = Attribute(r, "Age").ToInt(),
                Description = name,
                SavedBy = Util.UserName
            };
            DbUtil.Db.QueryBuilderClauses.InsertOnSubmit(c);
            DbUtil.Db.SubmitChanges();
            if(c.Field == "Group")
                foreach (var rr in r.Elements())
                    InsertClause(rr, c.QueryId);
            return c;
        }
        private string Attribute(XElement r, string attr)
        {
            return Attribute(r, attr, null);
        }
        private string Attribute(XElement r, string attr, string def)
        {
            var a = r.Attributes(attr).FirstOrDefault();
            if (a == null)
                return def;
            return a.Value;
        }
        private DateTime? AttributeDate(XElement r, string attr)
        {
            var a = r.Attributes(attr).FirstOrDefault();
            if (a == null)
                return null;
            return a.Value.ToDate();
        }
    }
}
