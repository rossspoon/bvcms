using System.Linq;
using System.Web.Mvc;
using CmsWeb.Areas.Search.Models;
using UtilityExtensions;
using CmsData;

namespace CmsWeb.Areas.Search.Controllers
{
    public class SavedQueryController : CmsStaffController
    {
        public ActionResult Index()
        {
            var m = new SavedQueryModel();
            return View(m);
        }
        [HttpPost]
        public ActionResult Edit(string id, string value)
        {
            var a = id.Split('.');
            var iid = a[1].ToInt();
            var c = DbUtil.Db.QueryBuilderClauses.SingleOrDefault(cc => cc.QueryId == iid);
            switch (a[0])
            {
                case "d":
                    c.Description = value;
                    break;
                case "o":
                    c.SavedBy = value;
                    break;
                case "p":
                    c.IsPublic = value == "yes";
                    break;
                case "x":
                    DbUtil.Db.DeleteQueryBuilderClauseOnSubmit(c);
                    break;
            }
            DbUtil.Db.SubmitChanges();
            return Content(value);
        }
        [HttpPost]
        public ActionResult Results()
        {
            var m = new SavedQueryModel();
            UpdateModel(m.Pager);
            UpdateModel(m);
            return View(m);
        }
    }
}
