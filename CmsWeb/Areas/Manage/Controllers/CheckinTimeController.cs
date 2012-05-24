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
		public static string SORT_ID = "0";
		public static string SORT_PERSON = "1";
		public static string SORT_DATETIME = "2";
		public static string SORT_LOCATION = "3";
		public static string SORT_ACTIVITY = "4";
		public static string SORT_GUESTOF = "5";

		public static string DIRECTION_ASC = "0";
		public static string DIRECTION_DESC = "1";

		public ActionResult Index()
		{
			var m = new CheckinTimeModel();
			m.Pager.Direction = DIRECTION_DESC;
			m.Pager.Sort = SORT_DATETIME;
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