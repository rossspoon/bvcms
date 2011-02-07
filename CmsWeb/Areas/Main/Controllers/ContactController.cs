using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;

namespace CmsWeb.Areas.Main.Controllers
{
    public class ContactController : Controller
    {
        //
        // GET: /Main/Contact/

        public ActionResult Index()
        {
            var m = from c in DbUtil.Db.NewContacts
                    select c;
            return View(m.Take(50));
        }

        //
        // GET: /Main/Contact/Details/5

        public ActionResult Details(int id)
        {
            var m = DbUtil.Db.NewContacts.Single(c => c.ContactId == id);
            return View(m);
        }

        //
        // GET: /Main/Contact/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Main/Contact/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /Main/Contact/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Main/Contact/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Main/Contact/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Main/Contact/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
