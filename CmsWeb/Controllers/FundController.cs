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
    public class FundController : Controller
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
            var m = new FundModel();
            m.fund = DbUtil.Db.ContributionFunds.SingleOrDefault(f => f.FundId == id);
            if (m.fund == null)
                RedirectToAction("Index");
            return View(m.fund);
        }

        //
        // POST: /Funds/Create

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create()
        {
            try
            {
                var m = new FundModel();
                var id = m.InsertFund();
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
            var m = new FundModel { FundId = id };
            if (m.fund == null)
                RedirectToAction("Index");
            return View(m);
        }

        public ActionResult Delete(int id)
        {
            var m = new FundModel();
            m.DeleteFund(id);
            return RedirectToAction("Index");
        }
        //
        // POST: /Funds/Edit/5

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update(int id)
        {
            var m = new FundModel { FundId = id };
            if (m.fund != null)
                UpdateModel(m.fund);
            return RedirectToAction("Index");
        }
    }
}
