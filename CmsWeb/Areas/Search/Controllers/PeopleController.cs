using System.Linq;
using System.Web.Mvc;
using CmsWeb.Areas.Search.Models;
using UtilityExtensions;

namespace CmsWeb.Areas.Search.Controllers
{
    public class PeopleController : CmsController
    {
        [HttpGet]
        public ActionResult Index(string name)
        {
            var m = new PeopleModel();
            if (name.HasValue())
            {
                m.m.name = name;
                if (m.Count() == 1)
                {
                    var pid = m.FetchPeople().Single().PeopleId;
                    return Redirect("/Person/Index/" + pid);
                }
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
            var m = new PeopleModel();
            UpdateModel(m);
            UpdateModel(m.m);
            Session["FindPeopleInfo"] = m.m;
            return View(m);
        }
        [HttpPost]
        public ActionResult ConvertToQuery()
        {
            var m = new PeopleModel();
            UpdateModel(m);
            UpdateModel(m.m);
            Session["FindPeopleInfo"] = m.m;
            return Content("/QueryBuilder/Main/" + m.ConvertToQuery());
        }
    }
}
