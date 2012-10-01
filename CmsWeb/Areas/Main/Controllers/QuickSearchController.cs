using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsWeb.Models;
using UtilityExtensions;
using CmsData;

namespace CmsWeb.Areas.Main.Controllers
{
    public class QuickSearchController : CmsController
    {
        public ActionResult Index(string q)
        {
			var m = new QuickSearchModel(q);
            if (q.HasValue())
            {
                if (m.people.Count == 1 && (q.AllDigits() || m.orgs.Count == 0))
                {
                    var pid = m.people.Single().PeopleId;
                    return Redirect("/Person/Index/" + pid);
                }
                if (m.orgs.Count == 1 && m.people.Count == 0)
                {
					var oid = m.orgs.Single().Id;
                    return Redirect("/Organization/Index/" + oid);
                }
            }
            return View(m);
        }
    }
}
