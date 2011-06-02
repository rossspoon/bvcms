using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;
using UtilityExtensions;
using System.Text;
using CmsWeb.Models.PersonPage;
using CmsWeb.Models;
using System.Diagnostics;
using System.Web.Routing;
using System.Threading;

namespace CmsWeb.Areas.Main.Controllers
{
    [ValidateInput(false)]
    public class FamilyController : CmsStaffController
    {
        public ActionResult Index(int? id)
        {
            if (!id.HasValue)
                return Content("no id");
            var m = new FamilyModel { familyid = id.Value };
            return View(m);
        }
        //public ActionResult btnSplit_Click(object sender, EventArgs e)
        //{
        //    var l = sender as LinkButtonConfirm;
        //    var d = family.People.Single(p => p.PeopleId == l.CommandArgument.ToInt());
        //    var f = new Family
        //    {
        //        CreatedDate = Util.Now,
        //        CreatedBy = Util.UserId1,
        //        AddressLineOne = d.PrimaryAddress,
        //        AddressLineTwo = d.PrimaryAddress2,
        //        CityName = d.PrimaryCity,
        //        StateCode = d.PrimaryState,
        //        ZipCode = d.PrimaryZip,
        //        HomePhone = d.Family.HomePhone
        //    };
        //    f.People.Add(d);
        //    DbUtil.Db.Families.InsertOnSubmit(f);
        //    DbUtil.Db.SubmitChanges();

        //    DbUtil.LogActivity("Splitting Family for {0}".Fmt(person.Name));
        //    Response.Redirect("~/Family.aspx?id=" + f.FamilyId);
        //}

        //public ActionResult btnRemoveRelation_Click(object sender, EventArgs e)
        //{
        //    var l = sender as LinkButtonConfirm;
        //    var d = family.RelatedFamilies1.SingleOrDefault(p => p.RelatedFamilyId == l.CommandArgument.ToInt());
        //    if (d == null)
        //        d = family.RelatedFamilies2.SingleOrDefault(p => p.FamilyId == l.CommandArgument.ToInt());
        //    DbUtil.Db.RelatedFamilies.DeleteOnSubmit(d);
        //    DbUtil.Db.SubmitChanges();
        //    if (person != null)
        //        DbUtil.LogActivity("Removing Related Family for {0}".Fmt(person.Name));
        //    Response.Redirect("~/Family.aspx?id=" + family.FamilyId);

        //}
        public ActionResult QuerySearch(int? id)
        {
            if (!id.HasValue)
                return Content("no id");
            var qb = DbUtil.Db.QueryBuilderScratchPad();
            qb.CleanSlate(DbUtil.Db);
            var comp = CompareType.Equal;
            var clause = qb.AddNewClause(QueryType.FamilyId, comp, id);
            DbUtil.Db.SubmitChanges();
            return Redirect("/QueryBuilder/Main/{0}".Fmt(qb.QueryId));
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult EditPosition(string id, string value)
        {
            var pid = id.Split('.')[1].ToInt();
            var p = DbUtil.Db.LoadPersonById(pid);
            p.PositionInFamilyId = value.ToInt();
            DbUtil.Db.SubmitChanges();
            return Content(p.FamilyPosition.Description);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult EditRelation(string id, string value)
        {
            var a = id.Split('.');
            var r = DbUtil.Db.RelatedFamilies.SingleOrDefault(m => m.FamilyId == a[1].ToInt() && m.RelatedFamilyId == a[2].ToInt());
            if (a[0] == "e")
                r.FamilyRelationshipDesc = value;
            DbUtil.Db.SubmitChanges();
            var c = new ContentResult();
            c.Content = value;
            return c;
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult DeleteRelation(string id)
        {
            var a = id.Split('.');
            var r = DbUtil.Db.RelatedFamilies.SingleOrDefault(m => m.FamilyId == a[1].ToInt() && m.RelatedFamilyId == a[2].ToInt());
            DbUtil.Db.RelatedFamilies.DeleteOnSubmit(r);
            DbUtil.Db.SubmitChanges();
            return Content(a[3]);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult Split(int id)
        {
            var p = DbUtil.Db.LoadPersonById(id);
            var f = new Family
            {
                CreatedDate = Util.Now,
                CreatedBy = Util.UserId1,
                AddressLineOne = p.PrimaryAddress,
                AddressLineTwo = p.PrimaryAddress2,
                CityName = p.PrimaryCity,
                StateCode = p.PrimaryState,
                ZipCode = p.PrimaryZip,
                HomePhone = p.Family.HomePhone
            };
            f.People.Add(p);
            DbUtil.Db.Families.InsertOnSubmit(f);
            DbUtil.Db.SubmitChanges();
            DbUtil.LogActivity("Splitting Family for {0}".Fmt(p.Name));
            if (p == null)
                return Content("/");
            return Content("/Person/Index/" + id);
        }



        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult PositionCodes()
        {
            var q = from c in DbUtil.Db.FamilyPositions
                    select new
                    {
                        Code = c.Id.ToString(),
                        Value = c.Description,
                    };
            return Json(q.ToDictionary(k => k.Code, v => v.Value));
        }

    }
}
