using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;
using CmsWeb.Models;

namespace CmsWeb.Areas.Manage.Controllers
{
    public class SMSController : Controller
    {
        public ActionResult Index(SMSModel m)
        {
            if (m == null) m = new SMSModel();
            else
            {
                UpdateModel(m.Pager);
            }

            return View(m);
        }

        public ActionResult Details( int id )
        {
            ViewBag.ListID = id;
            return View();
        }
    }
}
