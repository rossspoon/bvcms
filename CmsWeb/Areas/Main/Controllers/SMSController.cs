using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;

namespace CmsWeb.Areas.Main.Controllers
{
    public class SMSController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Options( int id )
        {
            ViewBag.QBID = id;
            return View();
        }

        public ActionResult Send(int iQBID, int iSendGroup, string sMessage)
        {
            TwilioHelper.QueueSMS(iQBID, iSendGroup, sMessage);

            ViewBag.QBID = iQBID;
            return View();
        }
    }
}
