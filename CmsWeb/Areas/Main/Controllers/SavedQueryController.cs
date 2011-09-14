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
    public class SavedQueryController : Controller
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
            }
            DbUtil.Db.SubmitChanges();
            return Content(value);
        }
        [HttpPost]
        public ActionResult Results(bool onlyMine)
        {
            var m = new SavedQueryModel { onlyMine = onlyMine };
            UpdateModel(m.Pager);
            return View(m);
        }
    }
}
