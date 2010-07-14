using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CmsData;
using UtilityExtensions;

namespace CmsWeb.Areas.Setup.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProgramController : CmsStaffController
    {
        public ActionResult Index()
        {
            var m = from p in DbUtil.Db.Programs
                    orderby p.RptGroup, p.Name
                    select p;
            return View(m);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create()
        {
            var p = new Program { Name = "new program" };
            DbUtil.Db.Programs.InsertOnSubmit(p);
            //p.Divisions.Add(new Division {  Name = "new division" });
            DbUtil.Db.SubmitChanges();
            return Redirect("/Setup/Program/");
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
                case "RptGroup":
                    p.RptGroup = value;
                    break;
                case "StartHours":
                    p.StartHoursOffset = value.ToDecimal();
                    break;
                case "EndHours":
                    p.EndHoursOffset = value.ToDecimal();
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
            DbUtil.Db.Divisions.DeleteAllOnSubmit(p.Divisions);
            DbUtil.Db.Programs.DeleteOnSubmit(p);
            DbUtil.Db.SubmitChanges();
            return new EmptyResult();
        }
    }
}
