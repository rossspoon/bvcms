using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CmsData;
using CMSWeb.Models;
using UtilityExtensions;

namespace CMSWeb.Controllers
{
    public class ProgramController : Controller
    {
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var m = DbUtil.Db.Programs.AsEnumerable();
            return View(m);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create()
        {
            var m = new Program { Name = "NEW" };
            DbUtil.Db.Programs.InsertOnSubmit(m);
            DbUtil.Db.SubmitChanges();
            return Redirect("/Program/");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult Edit(string id, string value)
        {
            var a = id.Split('.');
            var c = new ContentResult();
            c.Content = value;
            var p = DbUtil.Db.Programs.SingleOrDefault(m => m.Id == a[1].ToInt());
            if (p == null)
                return c;
            switch (a[0])
            {
                case "ProgramName":
                    p.Name = value;
                    break;
            }
            DbUtil.Db.SubmitChanges();
            return c;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public EmptyResult Delete(string id)
        {
            id = id.Substring(1);
            var p = DbUtil.Db.Programs.SingleOrDefault(m => m.Id == id.ToInt());
            if (p == null)
                return new EmptyResult();
            DbUtil.Db.Programs.DeleteOnSubmit(p);
            DbUtil.Db.SubmitChanges();
            return new EmptyResult();
        }
    }
}
