using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsWeb.Areas.Main.Models.Report;
using CmsData;
using System.IO;
using CmsWeb.Areas.Main.Models;
using UtilityExtensions;

namespace CmsWeb.Areas.Main.Controllers
{
    public class EmailController : CmsStaffController
    {
        [ValidateInput(false)]
        public ActionResult Index(int id, string body, string subj, bool? ishtml, bool? parents)
        {
            var m = new MassEmailer(id, parents);
            if (body.HasValue())
                m.Body = Server.UrlDecode(body);
            if (subj.HasValue())
                m.Subject = Server.UrlDecode(subj);
            return View(m);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult QueueEmails(string Submit, MassEmailer m)
        {
            if (Submit.StartsWith("Test"))
            {
                m.TestSend(Util.UserPeopleId.Value);
                return View("Index", m);
            }
            m.FromName = m.EmailFroms().Single(ef => ef.Value == m.FromAddress).Text;
            DbUtil.LogActivity("Emailing people");
            var id = m.Queue();
            if (m.Schedule.HasValue)
            {
                ViewData["queued"] = m.Schedule.Value.ToString("M/d/yy h:mm tt");
                ViewData["queueid"] = m.QueueId;
                return View("EmailQueued");
            }
            return RedirectToAction("Progress", new { id = id });
        }
        private bool Authenticate()
        {
            string username, password;
            var auth = Request.Headers["Authorization"];
            if (auth.HasValue())
            {
                var cred = System.Text.ASCIIEncoding.ASCII.GetString(
                    Convert.FromBase64String(auth.Substring(6))).Split(':');
                username = cred[0];
                password = cred[1];
            }
            else
            {
                username = Request.Headers["username"];
                password = Request.Headers["password"];
            }
            return CMSMembershipProvider.provider.ValidateUser(username, password);
        }
        public ActionResult SendEmails(int id)
        {
            if (!Authenticate())
                return Content("not authorized");
            DbUtil.LogActivity("Scheduled Emailing people");
            var m = new MassEmailer();
            m.Send(id);
            return Content(@"<html><body><a href=""/Email/Progress/{0}"">progress</a></body></html>".Fmt(id));
        }
        [Authorize(Roles="Admin")]
        public ActionResult SendNow(int id)
        {
            DbUtil.LogActivity("Immediate Emailing people");
            var m = new MassEmailer();
            m.Send(id);
            return RedirectToAction("Progress", new { id = id });
        }
        public ActionResult Progress(int id)
        {
            var emailqueue = DbUtil.Db.EmailQueues.SingleOrDefault(e => e.Id == id);
            if (emailqueue == null)
                return Content("bad queue");
            var q = from et in DbUtil.Db.EmailQueueTos
                    where et.Id == id
                    select et;
            ViewData["queued"] = emailqueue.Queued.ToString("M/d/yy h:mm tt");
            ViewData["started"] = emailqueue.Started == null ? "not started" : emailqueue.Started.Value.ToString("M/d/yy h:mm tt");
            ViewData["completed"] = emailqueue.Sent == null ? "still running" : emailqueue.Sent.Value.ToString("M/d/yy h:mm tt");
            ViewData["total"] = q.Count();
            ViewData["sent"] = q.Count(e => e.Sent != null);
            if (emailqueue.Started.HasValue)
            {
                var max = q.Max(et => et.Sent);
                max = max ?? DateTime.Now;
                ViewData["elapsed"] = max.Value.Subtract(emailqueue.Started.Value).ToString(@"h\:mm\:ss");
            }
            else
                ViewData["elapsed"] = "not started";
            return View();
        }
    }
}
