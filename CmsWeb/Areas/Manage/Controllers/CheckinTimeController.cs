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
			if (m.Locations().Count == 0)
				return Content("Building Checkin mode not setup, no checkin times available");
			return View(m);
		}

		[HttpPost]
		public ActionResult List( CheckinTimeModel m )
		{
			UpdateModel( m.Pager );
			return View(m);
		}
	}
}