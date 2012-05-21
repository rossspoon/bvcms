using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsWeb.Models;

namespace CmsWeb.Areas.Manage.Controllers
{
	public class CheckinTimeController : Controller
	{
		public ActionResult Index()
		{
			var m = new CheckinTimeModel();
			return View(m);
		}

		[HttpPost]
		public ActionResult List(CheckinTimeModel m)
		{
			return View(m);
		}
	}
}