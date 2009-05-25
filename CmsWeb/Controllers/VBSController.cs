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

namespace CMSWeb.Controllers
{
    public class VBSController : Controller
    {
        public ActionResult Index()
        {
            var m = new Models.VBSModel();
            UpdateModel<Models.IVBSBindable>(m);
            return View(m);
        }
        public ActionResult SearchPeople(int? id)
        {
            var m = new Models.SearchPeopleModel();
            UpdateModel<Models.ISearchPeopleFormBindable>(m);
            if (id.HasValue)
            {
                m.Page = id;
                return PartialView("SearchPeopleRows", m);
            }
            return PartialView(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Assign(int Id, int PeopleId)
        {
            var m = new Models.VBSDetailModel(Id);
            m.AssignPerson(PeopleId);
            return Json(new { pid = PeopleId.ToString(), name = m.Name });
        }
        public ActionResult Detail(int Id)
        {
            var m = new Models.VBSDetailModel(Id);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update(int Id)
        {
            var m = new Models.VBSDetailModel(Id);
            UpdateModel<Models.IVBSDetailBindable>(m);
            DbUtil.Db.SubmitChanges();
            return new RedirectResult("/VBS/Detail/" + Id);
        }
        public JsonResult OrgOptions(int Id, int? DivId, int? OrgId)
        {
            var m = new Models.VBSModel();
            if ((DivId ?? 0) == 0)
                return Json(m.FetchOrganizations(Id, 0));
            return Json(m.FetchOrganizations(DivId.Value, OrgId.Value));
        }
		[AcceptVerbs(HttpVerbs.Post)]
		public JsonResult SelectOrg(int Id, int OrgId)
		{
            var m = new Models.VBSModel();
			var v = m.UpdateVBSApp(Id, OrgId);
			return Json(new { OrgName = v.OrgName, DivId = v.DivId, OrgId = v.OrgId });
		}
		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Delete(int vid)
		{
            var m = new Models.VBSModel();
			m.DeleteVBSApp(vid);
			return Redirect("/VBS/");
		}
	}
}
