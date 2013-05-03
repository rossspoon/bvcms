using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CmsData;
using CmsWeb.Models;
using UtilityExtensions;

namespace CmsWeb.Areas.Manage.Controllers
{
    [Authorize(Roles="Edit")]
    public class OrgMembersController : CmsStaffController
    {
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Index()
        {
            var m = new OrgMembersModel();
            m.FetchSavedIds();
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
        public ActionResult EmailNotices()
        {
            var m = new OrgMembersModel();
            UpdateModel(m);
            m.SendMovedNotices();
            return View("List", m);
        }
        public ActionResult GradeList(int id)
        {
            var m = new OrgMembersModel();
            UpdateModel(m);
            return new OrgMembersModel.OrgExcelResult(id);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult List()
        {
            var m = new OrgMembersModel();
            UpdateModel(m);
            m.ValidateIds();
            DbUtil.Db.SetUserPreference("OrgMembersModelIds", "{0}.{1}.{2}".Fmt(m.ProgId,m.DivId,m.SourceId));
            DbUtil.DbDispose();
            DbUtil.Db.SetNoLock();
            return View(m);
        }
    }
}
