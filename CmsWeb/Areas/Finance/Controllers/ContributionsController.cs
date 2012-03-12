using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsWeb.Models;

namespace CmsWeb.Areas.Finance.Controllers
{
    public class ContributionsController : Controller
    {
        public ActionResult Index(int? id, int? year)
        {
        	var m = new ContributionSearchModel();
			if (id.HasValue)
				m.PeopleId = id;
			if (year.HasValue)
				m.Year = year.Value;
            return View(m);
        }
		public ActionResult Results(ContributionSearchModel m)
		{
			return View(m);
		}
    }
}
