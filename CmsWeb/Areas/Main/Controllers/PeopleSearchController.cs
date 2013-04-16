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
    public class PeopleSearchController : CmsController
    {
        [HttpGet]
        public ActionResult Index(string name)
        {
            var m = new PeopleSearchModel();
            if (name.HasValue())
            {
                m.m.name = name;
//                if (m.Count() == 1)
//                {
//                    var pid = m.FetchPeople().Single().PeopleId;
//                    return Redirect("/Person/Index/" + pid);
//                }
            }
            else
            {
                var i = Session["FindPeopleInfo"] as PeopleSearchInfo;
                if (i != null)
                    m.m = i;
            }
                
            return View(m);
        }
        [HttpPost]
        public ActionResult Results()
        {
            var m = new PeopleSearchModel();
            UpdateModel(m);
            UpdateModel(m.m);
            Session["FindPeopleInfo"] = m.m;
            return View(m);
        }
        [HttpPost]
        public ActionResult ConvertToQuery()
        {
            var m = new PeopleSearchModel();
            UpdateModel(m);
            UpdateModel(m.m);
            Session["FindPeopleInfo"] = m.m;
            return Content("/QueryBuilder/Main/" + m.ConvertToQuery());
        }
    }
}
