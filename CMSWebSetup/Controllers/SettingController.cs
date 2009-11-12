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
    public class SettingController : CMSWebCommon.Controllers.CmsController
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
            var set = DbUtil.Db.Settings.SingleOrDefault(m => m.Id == id);
            set.SettingX = value;
            DbUtil.Db.SubmitChanges();
            var c = new ContentResult();
            c.Content = set.SettingX;
            return c;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public EmptyResult Delete(string id)
        {
            id = id.Substring(1);
            var set = DbUtil.Db.Settings.SingleOrDefault(m => m.Id == id);
            if (set == null)
                return new EmptyResult();
            DbUtil.Db.Settings.DeleteOnSubmit(set);
            DbUtil.Db.SubmitChanges();
            return new EmptyResult();
        }
        public ActionResult Batch(string text)
        {
            if (Request.HttpMethod.ToUpper() == "GET")
            {
                var q = from s in DbUtil.Db.Settings
                        orderby s.Id
                        select "{0}:\t{1}".Fmt(s.Id, s.SettingX);
                ViewData["text"] = string.Join("\n", q.ToArray());
                return View();
            }
            var q2 = from s in text.Split('\n')
                     where s.HasValue()
                     let a = s.SplitStr(":", 2)
                     select new { name = a[0], value = a[1].Trim() };
            foreach (var i in q2)
            {
                var set = DbUtil.Db.Settings.SingleOrDefault(m => m.Id == i.name);
                if (set == null)
                {
                    set = new Setting { Id = i.name, SettingX = i.value };
                    DbUtil.Db.Settings.InsertOnSubmit(set);
                }
                else
                    set.SettingX = i.value;
                DbUtil.Db.SubmitChanges();
            }
            return RedirectToAction("Index");
        }

    }
}
