using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;

namespace CmsWeb.Areas.Main.Controllers
{
    public class ContactController : Controller
    {
        public ActionResult Index(int id)
        {
            var c = new CmsWeb.Models.ContactModel { Id = id };
            return View(c);
        }
    }
}
