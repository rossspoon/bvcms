using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsWeb.Models;
using CmsData;
using UtilityExtensions;

namespace CmsWeb.Areas.Setup.Controllers
{
    public class UpdateController : Controller
    {
        public ActionResult Index()
        {
            var success = (string)TempData["success"];
            if (success.HasValue())
                ViewData["success"] = success;
            var m = new UpdateModel();
            return View(m);
        }
        public ActionResult Run(UpdateModel m)
        {
            var tag = DbUtil.Db.TagById(m.Tag.ToInt());
            var q = tag.People(DbUtil.Db);
            switch (m.Field)
            {
                case "Member Status":
                    foreach (var p in q)
                        p.MemberStatusId = m.NewValue.ToInt();
                    break;
                case "Campus":
                    foreach (var p in q)
                        p.CampusId = m.NewValue.ToInt();
                    break;
                case "Marital Status":
                    foreach (var p in q)
                        p.MaritalStatusId = m.NewValue.ToInt();
                    break;
                case "Family Position":
                    foreach (var p in q)
                        p.PositionInFamilyId = m.NewValue.ToInt();
                    break;
                case "Gender":
                    foreach (var p in q)
                        p.GenderId = m.NewValue.ToInt();
                    break;
                case "Occupation":
                    foreach (var p in q)
                        p.OccupationOther = m.NewValue;
                    break;
                case "School":
                    foreach (var p in q)
                        p.SchoolOther = m.NewValue;
                    break;
                case "Employer":
                    foreach (var p in q)
                        p.EmployerOther = m.NewValue;
                    break;
            }
            DbUtil.Db.SubmitChanges();
            TempData["success"] = m.Field + " Updated";
            return RedirectToAction("Index");
        }
    }
}
