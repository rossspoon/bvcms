using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;
using CmsWeb.Models;
using UtilityExtensions;

namespace CmsWeb.Areas.Manage.Controllers
{
    public class EmailsController : Controller
    {
        public ActionResult Index()
        {
            var m = new EmailsModel();
            UpdateModel(m);
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

        public ActionResult Details(int id)
        {
            var m = new EmailModel { id = id };
            if (!DbUtil.Db.CurrentUser.Roles.Contains("Admin"))
            {
                var u = DbUtil.Db.LoadPersonById(Util.UserPeopleId.Value);
                if (m.queue.FromAddr != u.EmailAddress
                        && !m.queue.EmailQueueTos.Any(et => et.PeopleId == u.PeopleId))
                    return Content("not authorized");
            }
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Recipients(int id)
        {
            var m = new EmailModel { id = id };
            UpdateModel(m.Pager);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult List(EmailsModel m)
        {
            return View(m);
        }
    }
}
