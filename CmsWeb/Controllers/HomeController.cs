using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;
using System.Diagnostics;
using UtilityExtensions;
using System.Threading;
using System.Text.RegularExpressions;
using System.IO;
using System.Data.SqlClient;
using System.Net.Mail;

namespace CMSWeb.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return Redirect("/default.aspx");
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult NewQuery()
        {
            var qb = DbUtil.Db.QueryBuilderScratchPad();
            qb.CleanSlate();
            return Redirect("/QueryBuilder/Main");
        }
   //     public ActionResult DoBirthDays()
   //     {
			//var offset = new int[] { 20, 25, 30, 35, 40, 45, 50, 55, 60, 65,
			//						-20, -25, -30, -35, -40, -45, -50, -55, -60, -65 };
   //         var q = from p in DbUtil.Db.People
   //                 where DbUtil.Db.Birthday(p.PeopleId) != null
   //                 select p;
			//var r = new Random();
			//int n = 0;
			//foreach (var p in q)
			//{
			//	var bd = new DateTime(p.BirthYear.Value, p.BirthMonth.Value, p.BirthDay.Value);
			//	bd = bd.AddDays(offset[r.Next(20)]);
			//	p.BirthDay = bd.Day;
			//	p.BirthMonth = bd.Month;
			//	p.BirthYear = bd.Year;
			//	DbUtil.Db.SubmitChanges();
			//	n++;
   //             if (n % 100 == 0)
   //                Debug.WriteLine(n);
			//}
   //         var res = new ContentResult();
   //         res.Content = "<html><body>done</body></html>";
   //         return res;
   //     }
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

