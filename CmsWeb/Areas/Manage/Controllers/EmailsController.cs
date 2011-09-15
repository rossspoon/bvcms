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
            if (!DbUtil.Db.CurrentUser.Roles.Contains("Admin"))
            {
                var u = DbUtil.Db.LoadPersonById(Util.UserPeopleId.Value);
                if (m.queue.FromAddr != u.EmailAddress
                        && !m.queue.EmailQueueTos.Any(et => et.PeopleId == u.PeopleId))
                    return Content("not authorized");
            }
            return View(m);
        }

        [Authorize(Roles="Admin")]
        public ActionResult Requeue(int id)
        {
            return Redirect("/Manage/Emails/Details/" + id);
        }
        public ActionResult View(int id)
        {
            var email = DbUtil.Db.EmailQueues.SingleOrDefault(ee => ee.Id == id);
            if ((email.PublicX ?? false) == false)
                return Content("no email available");
            var em = new EmailQueue
            {
                Subject = email.Subject,
                Body = email.Body.Replace("{track}", "").Replace("{first}", "")
            };
            return View(em);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Recipients(int id, string filter)
        {
            var m = new EmailModel { id = id, filter = filter };
            UpdateModel(m.Pager);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult List(EmailsModel m)
        {
            UpdateModel(m.Pager);
            return View(m);
        }
    }
}
