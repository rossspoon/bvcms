using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;
using UtilityExtensions;
using System.Text;
using CMSWeb.Models.OrganizationPage;
using CMSWeb.Models;
using System.Diagnostics;

namespace CMSWeb.Areas.Main.Controllers
{
    public class OrganizationController : CmsStaffController
    {
        public ActionResult Index(int? id)
        {
            if (!id.HasValue)
                return Content("no org");
            var m = new OrganizationModel(id.Value);
            if (m.org == null)
                return Content("organization not found");
            if (Util.OrgMembersOnly
                && !DbUtil.Db.OrganizationMembers.Any(om =>
                    om.OrganizationId == m.org.OrganizationId
                    && om.PeopleId == Util.UserPeopleId))
            {
                DbUtil.LogActivity("Trying to view Organization ({0})".Fmt(m.org.OrganizationName));
                return Content("<h3 style='color:red'>{0}</h3>\n<a href='{1}'>{2}</a>"
                    .Fmt("You must be a member of this organization to have access to this page",
                    "javascript: history.go(-1)", "Go Back"));
            }
            DbUtil.LogActivity("Viewing Organization ({0})".Fmt(m.org.OrganizationName));

            if (Util.CurrentOrgId != m.org.OrganizationId)
                Util.CurrentGroupId = 0;
            Util.CurrentOrgId = m.org.OrganizationId;
            Session["ActiveOrganization"] = m.org.OrganizationName;

            return View(m);
        }
        [Authorize(Roles="Admin")]
        public ActionResult Delete(int id)
        {
            var org = DbUtil.Db.LoadOrganizationById(id);
            if (org == null)
                return Content("error, bad orgid");
            if (!org.PurgeOrg())
                return Content("error, not deleted");
            Util.CurrentOrgId = 0;
            Util.CurrentGroupId = 0;
            Session.Remove("ActiveOrganization");
            return Content("<h3 style='color:red'>Organization Deleted</h3>\n<a href='/'>Go Home</a>");
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PrevEnrollGrid(int id)
        {
            var m = new PersonPrevEnrollmentsModel(id);
            UpdateModel(m.Pager);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PendingEnrollGrid(int id)
        {
            var m = new PersonPendingEnrollmentsModel(id);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AttendanceGrid(int id, bool? future)
        {
            var m = new MeetingsModel(id, future == true);
            UpdateModel(m.Pager);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ContactsMadeGrid(int id)
        {
            var m = new PersonContactsMadeModel(id);
            UpdateModel(m.Pager);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ContactsReceivedGrid(int id)
        {
            var m = new PersonContactsReceivedModel(id);
            UpdateModel(m.Pager);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PendingTasksGrid(int id)
        {
            var m = new TaskModel();
            return View(m.TasksAboutList(id));
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Settings(int id)
        {
            var m = new OrganizationModel(id);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SettingsEdit(int id)
        {
            var m = new OrganizationModel(id);
            return View(m.org);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SettingsUpdate(int id)
        {
            var m = new OrganizationModel(id);
            UpdateModel(m.org);
            m.UpdateOrganization();
            m = new OrganizationModel(id);
            return View("Settings", m);
        }
    }
}
