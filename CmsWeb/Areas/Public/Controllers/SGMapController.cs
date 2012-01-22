using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;
using UtilityExtensions;
using System.Net;
using System.Xml.Linq;
using CmsWeb.Models;

namespace CmsWeb.Areas.Public.Controllers
{
    public class SGMapController : CmsController
    {
        public ActionResult Index(int id) // int id)
        {
            var m = new SGMapModel(id);
            return View(m);
        }
    }
}
