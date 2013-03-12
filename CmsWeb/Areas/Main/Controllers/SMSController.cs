using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;
using CmsData.Classes.Twilio;

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

        public ActionResult Send(int iQBID, int iSendGroup, string sTitle, string sMessage)
        {
            TwilioHelper.QueueSMS(iQBID, iSendGroup, sTitle, sMessage);

            ViewBag.QBID = iQBID;
            ViewBag.sTitle = sTitle;
            ViewBag.sMessage = sMessage;
            return View();
        }
    }
}
