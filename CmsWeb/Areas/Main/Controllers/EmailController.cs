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
            if (parents == true)
                ViewData["parentsof"] = "with ParentsOf option";
            return View(m);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult QueueEmails(MassEmailer m)
        {
            if (!m.Subject.HasValue() || !m.Body.HasValue())
                return Json(new { id = 0, content = "<h2>Both Subject and Body need some text</h2>" });

            DbUtil.LogActivity("Emailing people");
            m.FromName = m.EmailFroms().First(ef => ef.Value == m.FromAddress).Text;

            var emailqueue = m.CreateQueue();

            var Db = DbUtil.Db;
            if (emailqueue.SendWhen.HasValue)
                return Json(new { id = 0, content = "<h2>Emails Queued</h2>" });
            if (Db.UseMassEmailer)
                Db.QueueEmail(emailqueue.Id, Util.CmsHost, Util.Host);
            else
                Db.SendPeopleEmail(Util.CmsHost, emailqueue);

            return Json(new { id = emailqueue.Id });
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult TestEmail(MassEmailer m)
        {
            var From = Util.FirstAddress(m.FromAddress, m.FromName);
            var p = DbUtil.Db.LoadPersonById(Util.UserPeopleId.Value);
            DbUtil.Db.Email(From.ToString(), p, m.Subject, m.Body);
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
                ViewData["total"] = q.Count();
                ViewData["sent"] = q.Count(e => e.Sent != null);
                ViewData["finished"] = false;
                if (emailqueue.Started == null)
                {
                    ViewData["started"] = "not started";
                    ViewData["completed"] = "not started";
                    ViewData["elapsed"] = "not started";
                }
                else
                {
                    ViewData["started"] = emailqueue.Started.Value.ToString("M/d/yy h:mm tt");
                    var max = q.Max(et => et.Sent);
                    max = max ?? DateTime.Now;

                    if (emailqueue.Sent == null)
                        ViewData["completed"] = "running";
                    else
                    {
                        ViewData["completed"] = max;
                        emailqueue.Sent.Value.ToString("M/d/yy h:mm tt");
                        ViewData["finished"] = true;
                    }
                    ViewData["elapsed"] = max.Value.Subtract(emailqueue.Started.Value).ToString(@"h\:mm\:ss");
                }
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
        public ActionResult List()
        {
            return View();
        }
    }
    
}
