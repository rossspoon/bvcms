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
    public class OrgMembersController : CmsStaffController
    {
        public ActionResult Index()
        {
            var m = new OrgMembersModel();
            UpdateModel(m);
            return View(m);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Move()
        {
            var m = new OrgMembersModel();
            UpdateModel(m);
            m.Move();
            return View("List", m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult List()
        {
            var m = new OrgMembersModel();
            UpdateModel(m);
            return View(m);
        }
    }
}
