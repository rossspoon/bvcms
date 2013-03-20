using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsWeb.Models;

namespace CmsWeb.Areas.Finance.Controllers
{
    [Authorize(Roles = "Finance")]
    public class ContributionsController : CmsStaffController
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
        [HttpPost]
		public ActionResult Results(ContributionSearchModel m)
		{
			return View(m);
		}
    }
}
