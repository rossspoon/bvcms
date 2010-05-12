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
    public class GroupMembersController : CmsStaffController
    {
        public ActionResult Index()
        {
            var m = new GroupMembersModel();
            UpdateModel(m);
            return View(m);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AssignToTeam()
        {
            var m = new GroupMembersModel();
            UpdateModel(m);
            m.AssignToTeam();
            return RedirectToAction("Index");
        }
        public ActionResult List()
        {
            var m = new GroupMembersModel();
            UpdateModel(m);
            return PartialView("List", m);
        }
    }
}
