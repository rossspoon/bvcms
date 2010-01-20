using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;

namespace CMSWeb.Areas.Main.Controllers
{
    public class PersonController : Controller
    {
        //
        // GET: /Main/Person/

        public ActionResult Index(int? id)
        {
            if (!id.HasValue)
                return Content("no person");
            var m = new Models.PersonModel(id);
            if (m.person == null)
                return Content("person not found");
            return View(m);
        }
        public ActionResult Move(int? id, int to)
        {
            var p = DbUtil.Db.People.Single(pp => pp.PeopleId == id);
            try
            {
                p.MovePersonStuff(to);
                DbUtil.Db.SubmitChanges();
            }
            catch
            {
                return Content("error");
            }
            return new EmptyResult();
        }

    }
}
