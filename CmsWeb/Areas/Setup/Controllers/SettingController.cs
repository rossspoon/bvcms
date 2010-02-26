using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CmsData;
using UtilityExtensions;

namespace CMSWeb.Areas.Setup.Controllers
{
   [Authorize(Roles = "Admin")]
    public class SettingController : CmsController
    {
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
            return Redirect("/Setup/Setting/");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult Edit(string id, string value)
        {
            var set = DbUtil.Db.Settings.SingleOrDefault(m => m.Id == id);
            set.SettingX = value;
            DbUtil.Db.SubmitChanges();
            DbUtil.SetSetting(id, value);
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
            var batch = from s in text.Split('\n')
                        where s.HasValue()
                        let a = s.SplitStr(":", 2)
                        select new { name = a[0], value = a[1].Trim() };

            var settings = DbUtil.Db.Settings.ToList();

            var upds = from s in settings
                       join b in batch on s.Id equals b.name
                       select new { s = s, value = b.value };
            
            foreach (var pair in upds)
                pair.s.SettingX = pair.value;

            var adds = from b in batch
                       join s in settings on b.name equals s.Id into g
                       from s in g.DefaultIfEmpty()
                       where s == null
                       select b;

            foreach(var b in adds)
                DbUtil.Db.Settings.InsertOnSubmit(new Setting { Id = b.name, SettingX = b.value });

            var dels = from s in settings
                       where !batch.Any(b => b.name == s.Id)
                       select s;

            DbUtil.Db.Settings.DeleteAllOnSubmit(dels);

            
            DbUtil.Db.SubmitChanges();            

            return RedirectToAction("Index");
        }
        public ActionResult BatchReportSpecs(string text)
        {
            if (Request.HttpMethod.ToUpper() == "GET")
            {
                var q = from r in DbUtil.Db.ChurchAttReportIds
                        orderby r.Name
                        select "{0}:\t{1}".Fmt(r.Name, r.Id);
                ViewData["text"] = string.Join("\n", q.ToArray());
                return View();
            }
            var q2 = from s in text.Split('\n')
                     where s.HasValue()
                     let a = s.SplitStr(":", 2)
                     select new { name = a[0], value = a[1].ToInt() };
            foreach (var i in q2)
            {
                var set = DbUtil.Db.ChurchAttReportIds.SingleOrDefault(m => m.Name == i.name);
                if (set == null)
                {
                    set = new ChurchAttReportId { Name = i.name, Id = i.value };
                    DbUtil.Db.ChurchAttReportIds.InsertOnSubmit(set);
                }
                else
                    set.Id = i.value;
            }
            DbUtil.Db.SubmitChanges();
            return RedirectToAction("BatchReportSpecs");
        }
        public ActionResult OrphanedImages()
        {
            var m = from i in DbUtil.Db.ViewOrphanedImages
                    select i;
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult DeleteImage(string id)
        {
            var iid = id.Substring(1).ToInt();
            var img = ImageData.DbUtil.Db.Images.SingleOrDefault(m => m.Id == iid);
            if (img == null)
                return Content("#r0");
            ImageData.DbUtil.Db.Images.DeleteOnSubmit(img);
            ImageData.DbUtil.Db.SubmitChanges();
            return Content("#r" + iid);
        }
    }
}
