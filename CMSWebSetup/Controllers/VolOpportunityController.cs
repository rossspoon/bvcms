using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;
using System.Configuration;
using UtilityExtensions;

namespace CMSWebSetup.Controllers
{
    [HandleError]
    public class VolOpportunityController : Controller
    {
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var m = DbUtil.Db.VolOpportunities.AsEnumerable();
            return View(m);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Codes(int id)
        {
            var o = DbUtil.Db.VolOpportunities.Single(op => op.Id == id);
            ViewData["Opportunity"] = o.Description;
            ViewData["OpportunityId"] = id;
            var m = DbUtil.Db.VolInterestCodes.Where(vc => vc.OpportunityId == id);
            return View(m);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create()
        {
            var o = new VolOpportunity();
            DbUtil.Db.VolOpportunities.InsertOnSubmit(o);
            DbUtil.Db.SubmitChanges();
            return Redirect("/VolOpportunity/");
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateCode(int OpportunityId)
        {
            var c = new VolInterestCode{ OpportunityId = OpportunityId };
            DbUtil.Db.VolInterestCodes.InsertOnSubmit(c);
            DbUtil.Db.SubmitChanges();
            return Redirect("/VolOpportunity/Codes/" + OpportunityId.ToString());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult Edit(string id, string value)
        {
            var a = id.Split('.');
            var c = new ContentResult();
            c.Content = value;
            var o = DbUtil.Db.VolOpportunities.SingleOrDefault(vo => vo.Id == a[1].ToInt());
            if (o == null)
                return c;
            switch (a[0])
            {
                case "Description":
                    o.Description = value;
                    break;
                case "UrlKey":
                    o.UrlKey = value;
                    break;
                case "EmailNoCva":
                    o.EmailNoCva = value;
                    break;
                case "EmailYesCva":
                    o.EmailYesCva = value;
                    break;
                case "Question":
                    o.ExtraQuestion = value;
                    break;
                case "Instructions":
                    o.ExtraInstructions = value;
                    break;
                case "Email":
                    o.Email = value;
                    break;
                case "MaxChecks":
                    if (value.HasValue())
                        o.MaxChecks = value.ToInt();
                    else
                        o.MaxChecks = null;
                    break;
            }
            DbUtil.Db.SubmitChanges();
            return c;
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult EditCode(int id, string value)
        {
            var c = new ContentResult();
            c.Content = value;
            var intcode = DbUtil.Db.VolInterestCodes.SingleOrDefault(vo => vo.Id == id);
            if (intcode == null)
                return c;
            intcode.Description = value;
            DbUtil.Db.SubmitChanges();
            return c;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public EmptyResult Delete(string id)
        {
            id = id.Substring(1);
            var o = DbUtil.Db.VolOpportunities.SingleOrDefault(vo => vo.Id == id.ToInt());
            if (o == null)
                return new EmptyResult();
            DbUtil.Db.VolOpportunities.DeleteOnSubmit(o);
            DbUtil.Db.SubmitChanges();
            return new EmptyResult();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public EmptyResult DeleteCode(string id)
        {
            int code = id.Substring(1).ToInt();
            var intcode = DbUtil.Db.VolInterestCodes.SingleOrDefault(vo => vo.Id == code);
            if (intcode == null)
                return new EmptyResult();
            DbUtil.Db.VolInterestCodes.DeleteOnSubmit(intcode);
            DbUtil.Db.SubmitChanges();
            return new EmptyResult();
        }
    }
}
