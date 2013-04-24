using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsWeb.Areas.Public.Models;

namespace CmsWeb.Areas.Public.Controllers
{
    public class SmallGroupFinderController : Controller
    {
        public SmallGroupFinderModel sgfm;

        public ActionResult Index( string id )
        {
            SmallGroupFinderModel sgfm = new SmallGroupFinderModel();
            sgfm.load(id);

            return View(sgfm);
        }
    }
}