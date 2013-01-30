using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;

namespace CmsWeb.Areas.Setup.Controllers
{
    public class TwilioController : Controller
    {
        public ActionResult Index( int activeTab = 0 )
        {
            ViewBag.Tab = activeTab;
            return View();
        }

        // Group Actions
        public ActionResult GroupList()
        {
            return View();
        }

        public ActionResult GroupCreate( string name, string description )
        {
            var n = new SMSGroup();

            n.Name = name;
            n.Description = description;

            DbUtil.Db.SMSGroups.InsertOnSubmit(n);
            DbUtil.Db.SubmitChanges();

            return RedirectToAction( "Index", new { activeTab = 1 } );
        }

        public ActionResult NumberList()
        {
            return View();
        }

        public ActionResult GroupManagement()
        {
            return View();
        }
    }
}
