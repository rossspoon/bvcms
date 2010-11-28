using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsWeb.Areas.Main.Models;
using UtilityExtensions;
using CmsData;

namespace CmsWeb.Areas.Main.Controllers
{
    public class EmailController : CmsStaffController
    {
        [ValidateInput(false)]
        public ActionResult Index(int id, string body, string subj, bool? ishtml, bool? parents)
        {
            var m = new MassEmailer(id, parents);
            m.CmsHost = Util.CmsHost;
            m.Host = Util.Host;
            if (body.HasValue())
                m.Body = Server.UrlDecode(body);
            if (subj.HasValue())
                m.Subject = Server.UrlDecode(subj);
            ViewData["oldemailer"] = "/EmailPeople.aspx?id=" + id
                + "&subj=" + subj + "&body=" + body + "&ishtml=" + ishtml
                + (parents == true ? "&parents=true" : "");
            return View(m);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult QueueEmails(MassEmailer m)
        {
            DbUtil.LogActivity("Emailing people");
            m.FromName = m.EmailFroms().Single(ef => ef.Value == m.FromAddress).Text;

            var emailqueue = m.CreateQueue();

            if (emailqueue.SendWhen.HasValue)
                return Json(new { id = 0, content = "<h2>Emails Queued</h2>" });

            if (DbUtil.Db.EmailQueueTos.Count(et => et.Id == emailqueue.Id) < 10)
                // Send Immediately, bypass Service Broker
                Emailer.SendPeopleEmail(DbUtil.Db, Util.CmsHost, emailqueue);
            else
                // Send to Service Broker, Send ASAP (at end of queue)
                DbUtil.Db.QueueEmail(emailqueue.Id, Util.CmsHost, Util.Host);

            return Json(new { id = emailqueue.Id });
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult TestEmail(MassEmailer m)
        {
            var From = Util.FirstAddress(m.FromAddress, m.FromName);
            var qp = DbUtil.Db.People.Where(pp => pp.PeopleId == Util.UserPeopleId);
            Emailer.SendPeopleEmail(DbUtil.Db, Util.CmsHost, From, qp, m.Subject, m.Body);
            return Content("<h2>Test Email Sent</h2>");
        }
        [HttpPost]
        public ActionResult TaskProgress(int id)
        {
            var queue = SetProgressInfo(id);
            if (queue == null)
                return Content("no queue");
            return View();
        }
        private EmailQueue SetProgressInfo(int id)
        {
            var emailqueue = DbUtil.Db.EmailQueues.SingleOrDefault(e => e.Id == id);
            if (emailqueue != null)
            {
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
            }
            return emailqueue;
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
        //public ActionResult SendEmails(int id)
        //{
        //    //if (Submit.StartsWith("Test"))
        //    //{
        //    //    m.TestSend(Util.UserPeopleId.Value);
        //    //    return View("Index", m);
        //    //}
        //    if (!Authenticate())
        //        return Content("not authorized");
        //    DbUtil.LogActivity("Scheduled Emailing people");
        //    var m = new MassEmailer();
        //    m.Send(id);
        //    return Content(@"<html><body><a href=""/Email/Progress/{0}"">progress</a></body></html>".Fmt(id));
        //}
        //[Authorize(Roles = "Admin")]
        //public ActionResult SendNow(int id)
        //{
        //    DbUtil.LogActivity("Immediate Emailing people");
        //    var m = new MassEmailer();
        //    m.Send(id);
        //    return RedirectToAction("Progress", new { id = id });
        //}
    }
}
