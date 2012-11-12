using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsWeb.Models;

namespace CmsWeb.Areas.Manage.Controllers
{
	public class CheckinTimeController : CmsController
	{
		public ActionResult Index(CheckinTimeModel m)
		{
			if (m.Locations().Count == 0)
				return Content("Building Checkin mode not setup, no checkin times available");
			UpdateModel(m.Pager);
			return View(m);
		}
	}
}