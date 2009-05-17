using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Configuration;

namespace Forms.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewData["header"] = ConfigurationManager.AppSettings["header"];
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
        public static void Email(string from, string name, string addr, string subject, string message)
        {
            var InDebug = false;
#if DEBUG
            InDebug = true;
#endif
            if (InDebug)
                return;
            var smtp = new SmtpClient();
            var fr = new MailAddress(from);
            var ma = Util.TryGetMailAddress(addr, name);
            if (ma == null)
                return;
            var msg = new MailMessage(fr, ma);
            msg.Subject = subject;
            msg.Body = message;
            msg.IsBodyHtml = true;
            smtp.Send(msg);
        }
    }
}
