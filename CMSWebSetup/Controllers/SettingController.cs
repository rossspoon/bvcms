using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CmsData;
using UtilityExtensions;

namespace CMSWebSetup.Controllers
{
    public class SettingController : Controller
    {
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var m = DbUtil.Db.Settings.AsEnumerable();
            return View(m);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(string id)
        {
            var m = new Setting { Id = id };
            DbUtil.Db.Settings.InsertOnSubmit(m);
            DbUtil.Db.SubmitChanges();
            return Redirect("/Setting/");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult Edit(string id, string value)
        {
            var zip = DbUtil.Db.Settings.SingleOrDefault(m => m.Id == id);
            zip.SettingX = value;
            DbUtil.Db.SubmitChanges();
            var c = new ContentResult();
            c.Content = zip.SettingX;
            return c;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public EmptyResult Delete(string id)
        {
            id = id.Substring(1);
            var zip = DbUtil.Db.Settings.SingleOrDefault(m => m.Id == id);
            if (zip == null)
                return new EmptyResult();
            DbUtil.Db.Settings.DeleteOnSubmit(zip);
            DbUtil.Db.SubmitChanges();
            return new EmptyResult();
        }
    }
}
