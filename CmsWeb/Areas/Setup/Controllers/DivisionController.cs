using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CmsData;
using UtilityExtensions;
using System.Drawing;

namespace CMSWeb.Areas.Setup.Controllers
{
    public class DivisionController : CmsController
    {
        public class DivisionInfo
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int? ProgId { get; set; }
            public string Program { get; set; }
            public int OrgCount { get; set; }
            public int DivOrgsCount { get; set; }
            public int RecAgeDivCount { get; set; }
            public int RecRegCount { get; set; }
            public int ToPromotionsCount { get; set; }
            public int FromPromotionsCount { get; set; }
            public string NoZero(int arg)
            {
                if (arg == 0)
                    return "";
                return arg.ToString();
            }
            public bool CanDelete
            {
                get
                {
                    return OrgCount + DivOrgsCount + RecAgeDivCount + RecRegCount 
                        + ToPromotionsCount + FromPromotionsCount == 0;
                }
            }
            
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var m = from d in DbUtil.Db.Divisions
                    orderby d.Program.Name, d.Name
                    select new DivisionInfo
                    {
                        Id = d.Id,
                        Name = d.Name,
                        ProgId = d.ProgId,
                        Program = d.Program.Name,
                        OrgCount = d.Organizations.Count(),
                        DivOrgsCount = d.DivOrgs.Count(),
                        RecAgeDivCount = d.RecAgeDivisions.Count(),
                        RecRegCount = d.RecRegs.Count(),
                        FromPromotionsCount = d.FromPromotions.Count(),
                        ToPromotionsCount = d.ToPromotions.Count(),
                    };
            return View(m);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create()
        {
            var d = new Division { Name = "New Division" };
            DbUtil.Db.Divisions.InsertOnSubmit(d);
            DbUtil.Db.SubmitChanges();
            return Redirect("/Setup/Division/");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(string id, string value)
        {
            var iid = id.Substring(1).ToInt();
            var div = DbUtil.Db.Divisions.SingleOrDefault(p => p.Id == iid);
            if (div != null)
                switch (id.Substring(0, 1))
                {
                    case "n":
                        div.Name = value;
                        DbUtil.Db.SubmitChanges();
                        return Content(value);
                    case "p":
                        div.ProgId = value.ToInt();
                        DbUtil.Db.SubmitChanges();
                        return Content(div.Program.Name);
                }
            return new EmptyResult();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public EmptyResult Delete(string id)
        {
            var iid = id.Substring(1).ToInt();
            var div = DbUtil.Db.Divisions.SingleOrDefault(m => m.Id == iid);
            if (div == null)
                return new EmptyResult();
            var q = from d in DbUtil.Db.Divisions
                    where d.Id == iid
                    where d.Organizations.Count() == 0
                    where d.RecAgeDivisions.Count() == 0
                    where d.RecRegs.Count() == 0
                    where d.ToPromotions.Count() == 0
                    where d.FromPromotions.Count() == 0
                    select d;

            DbUtil.Db.Divisions.DeleteOnSubmit(div);
            DbUtil.Db.SubmitChanges();
            return new EmptyResult();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult ProgramCodes()
        {
            var q = from c in DbUtil.Db.Programs
                    orderby c.BFProgram descending, c.Name
                    select new
                    {
                        Code = c.Id.ToString(),
                        Value = c.Name,
                    };
            return Json(q.ToDictionary(k => k.Code, v => v.Value));
        }
    }
}
