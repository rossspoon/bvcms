using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;
using CmsWeb.Models;
using UtilityExtensions;
using System.Net.Mail;

namespace CmsWeb.Areas.Manage.Controllers
{
    [Authorize(Roles = "Manager, Admin")]
    public class MergeController : Controller
    {
       public ActionResult Index(int PeopleId1, int PeopleId2)
        {
            var m = new MergeModel(PeopleId1, PeopleId2);
            if (m.pi.Count != 2)
                if (m.pi.Count == 1)
                    if (m.pi[0].PeopleId != PeopleId1)
                        return Content("peopleid {0} not found".Fmt(PeopleId1));
                    else
                        return Content("peopleid {0} not found".Fmt(PeopleId2));
                else if (m.pi.Count == 0)
                    return Content("neither peopleid found");
            return View(m);
        }
        [HttpPost]
        public ActionResult Run(string submit, bool? Delete, MergeModel m)
        {
            m.Update();
            if (submit == "Merge Fields and Move Related Records")
                m.Move();
            if (Delete == true)
                m.Delete();
            return Redirect("/Person/Index/" + m.pi[1].PeopleId);
        }
    }
}
