using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CmsData;
using CMSWeb.Models;

namespace CMSWeb.Controllers
{
    public class MetroZipController : Controller
    {
        //
        // GET: /Funds/

        public ActionResult Index()
        {
            var m = DbUtil.Db.Zips.AsEnumerable();
            return View(m);
        }

        //
        // GET: /Funds/Details/5

        public ActionResult Details(string id)
        {
            var m = new MetroZipModel();
            m.zip = DbUtil.Db.Zips.SingleOrDefault(f => f.ZipCode == id);
            if (m.zip == null)
                RedirectToAction("Index");
            return View(m.zip);
        }

        //
        // POST: /Funds/Create

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create()
        {
            try
            {
                var m = new MetroZipModel();
                var id = m.InsertZip();
                return RedirectToAction("Edit", new { id = id });
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        //
        // GET: /Funds/Edit/5

        public ActionResult Edit(string id)
        {
            var m = new MetroZipModel { ZipCode = id };
            if (m.zip == null)
                RedirectToAction("Index");
            return View(m);
        }

        public ActionResult Delete(string id)
        {
            var m = new MetroZipModel();
            m.DeleteZip(id);
            return RedirectToAction("Index");
        }
        //
        // POST: /Funds/Edit/5

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update(string id)
        {
            var m = new MetroZipModel { ZipCode = id };
            if (m.zip != null)
                UpdateModel(m.zip);
            return RedirectToAction("Index");
        }
    }
}
