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
    public class MinistryController : Controller
    {
        //
        // GET: /Funds/

        public ActionResult Index()
        {
            var m = DbUtil.Db.ContributionFunds.AsEnumerable();
            return View(m);
        }

        //
        // GET: /Funds/Details/5

        public ActionResult Details(int id)
        {
            var m = new MinistryModel();
            m.ministry = DbUtil.Db.Ministries.SingleOrDefault(mi => mi.MinistryId == id);
            if (m.ministry == null)
                RedirectToAction("Index");
            return View(m.ministry);
        }

        //
        // POST: /Funds/Create

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create()
        {
            try
            {
                var m = new MinistryModel();
                var id = m.InsertMinistry();
                return RedirectToAction("Edit", new { id = id });
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        //
        // GET: /Funds/Edit/5


        public ActionResult Edit(int id)
        {
            var m = new MinistryModel { MinistryId = id };
            if (m.ministry == null)
                RedirectToAction("Index");
            return View(m);
        }

        public ActionResult Delete(int id)
        {
            var m = new MinistryModel();
            m.DeleteMinistry(id);
            return RedirectToAction("Index");
        }
        //
        // POST: /Funds/Edit/5

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update(int id)
        {
            var m = new MinistryModel { MinistryId = id };
            if (m.ministry != null)
                UpdateModel(m.ministry);
            return RedirectToAction("Index");
        }
    }
}
