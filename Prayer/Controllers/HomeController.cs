using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Prayer.Models;
using CmsData;

namespace Prayer.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewData["Message"] = "Welcome to Bellevue Prays!";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
        public ActionResult Signup()
        {
            return View(new SignupModel(DbUtil.Db.CurrentUser));
        }
        public ActionResult CacheExpire()
        {
            return new EmptyResult();
        }
    }
}
