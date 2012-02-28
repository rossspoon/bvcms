using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;
using CmsWeb.Models;
using UtilityExtensions;
using System.Net.Mail;

namespace CmsWeb.Areas.Manage.Controllers
{
    public class EmailsController : Controller
    {
        public ActionResult Index()
        {
            var m = new EmailsModel();
            return View(m);
        }
        public ActionResult SentBy(int? id)
        {
            var m = new EmailsModel { senderid = id };
            return View("Index", m);
        }
        public ActionResult SentTo(int? id)
        {
            var m = new EmailsModel { peopleid = id };
            return View("Index", m);
        }

        public ActionResult Details(int id, string filter)
        {
            var m = new EmailModel { id = id, filter = filter ?? "All" };
            if (User.IsInRole("Admin") || User.IsInRole("ManageEmails"))
                return View(m);
            var u = DbUtil.Db.LoadPersonById(Util.UserPeopleId.Value);
            if (m.queue.FromAddr != u.EmailAddress
                    && !m.queue.EmailQueueTos.Any(et => et.PeopleId == u.PeopleId))
                return Content("not authorized");
            return View(m);
        }

        [Authorize(Roles = "Admin, ManageEmails")]
        public ActionResult Requeue(int id)
        {
            return Redirect("/Manage/Emails/Details/" + id);
        }
        public ActionResult DeleteQueued(int id)
        {
            var email = (from e in DbUtil.Db.EmailQueues
                         where e.Id == id
                         select e).Single();
			var m = new EmailModel { id = id };
			if (!m.CanDelete())
				return Redirect("/");
            DbUtil.Db.EmailQueueTos.DeleteAllOnSubmit(email.EmailQueueTos);
            DbUtil.Db.EmailQueues.DeleteOnSubmit(email);
            DbUtil.Db.SubmitChanges();
            return Redirect("/Manage/Emails");
        }
        public ActionResult MakePublic(int id)
        {
            var email = (from e in DbUtil.Db.EmailQueues
                         where e.Id == id
                         select e).Single();
			var m = new EmailModel { id = id };
			if (!m.CanDelete())
				return Redirect("/");
			email.PublicX = true;
            DbUtil.Db.SubmitChanges();
			return RedirectToAction("View", new { id = id });
        }
        public ActionResult View(int id)
        {
            var email = DbUtil.Db.EmailQueues.SingleOrDefault(ee => ee.Id == id);
			if (email == null)
				return Content("document not found, sorry");
            if ((email.PublicX ?? false) == false)
                return Content("no email available");
            var em = new EmailQueue
            {
                Subject = email.Subject,
                Body = email.Body.Replace("{track}", "").Replace("{first}", "")
            };
            return View(em);
        }
        [HttpPost]
        public ActionResult Recipients(int id, string filter)
        {
            var m = new EmailModel { id = id, filter = filter };
            UpdateModel(m.Pager);
            return View(m);
        }
        [HttpPost]
        public ActionResult List(EmailsModel m)
        {
            UpdateModel(m.Pager);
            return View(m);
        }
    }
}
