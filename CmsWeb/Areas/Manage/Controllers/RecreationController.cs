using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CmsData;
using CMSWeb.Models;
using UtilityExtensions;

namespace CMSWeb.Areas.Manage.Controllers
{
    public class RecreationController : CmsController
    {
        public ActionResult Index()
        {
            var m = new RecreationModel();
            UpdateModel(m);
            return View(m);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AssignToTeam()
        {
            var m = new RecreationModel();
            UpdateModel(m);
            m.AssignToTeam();
            return RedirectToAction("Index");
        }
        public ActionResult List()
        {
            var m = new RecreationModel();
            UpdateModel(m);
            return PartialView("List", m);
        }
        public ActionResult SearchPeople(int? id)
        {
            var m = new Models.SearchPeopleModel();
            UpdateModel(m);
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
            var m = new RecDetailModel(Id);
            m.AssignPerson(PeopleId);
            return Json(new { pid = PeopleId.ToString(), name = m.Name });
        }
        public ActionResult Detail(int id)
        {
            var m = new RecDetailModel(id);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Create(int id, int oid)
        {
            var reg = new RecReg { PeopleId = id, Uploaded = Util.Now, OrgId=oid };
            DbUtil.Db.RecRegs.InsertOnSubmit(reg);
            DbUtil.Db.SubmitChanges();
            return Json(new { id = reg.Id });
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update(int Id)
        {
            var m = new RecDetailModel(Id);
            UpdateModel(m);
            if (!m.recreg.PeopleId.HasValue || m.recreg.Person.GenderId == 0 || !m.recreg.Person.GetBirthdate().HasValue)
            {
                ModelState.AddModelError("person", "Missing data on participant");
                return View("Detail", m);
            }
            if (m.League > 0 && (!m.recreg.OrgId.HasValue || m.recreg.OrgId != m.RecAgeDiv.OrgId) && m.recreg.PeopleId.HasValue)
                m.EnrollInOrg();
            if (m.League == 0)
                m.League = null;
            DbUtil.Db.SubmitChanges();
            return new RedirectResult("/Recreation/Detail/" + Id);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(int rid)
        {
            var m = new Models.RecreationModel();
            m.DeleteRecReg(rid);
            return Redirect("/Recreation/");
        }
        public ActionResult Coaches()
        {
            var m = new RecreationModel();
            UpdateModel(m);
            return View(m);
        }
        public ActionResult All(int? id)
        {
            if (!id.HasValue)
                return Content("no league id");
            var m = new RecreationModel();
            m.LeagueId = id;
            UpdateModel(m);
            return View(m);
        }
    }
}
