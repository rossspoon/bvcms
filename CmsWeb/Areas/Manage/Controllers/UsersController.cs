using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsWeb.Models;

namespace CmsWeb.Areas.Manage.Controllers
{
	[Authorize(Roles = "Admin")]
    public class UsersController : CmsController
    {
        public ActionResult Index()
        {
	        var m = new UsersModel();
            return View(m);
        }
        [HttpPost]
        public ActionResult Results()
        {
            var m = new UsersModel();
            UpdateModel(m);
            return View(m);
        }
    }
}
