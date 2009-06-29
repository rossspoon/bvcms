using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CmsData;
using CMSWeb.Models;
using UtilityExtensions;

namespace CMSWeb.Controllers
{
    public class FundController : Controller
    {
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var m = DbUtil.Db.ContributionFunds.AsEnumerable();
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(int fundid)
        {
            try
            {
                var f = new ContributionFund 
                { 
                    FundName = "new fund", 
                    FundId=fundid,
                    ChurchId = 1,
                    CreatedBy = Util.UserId1,
                    CreatedDate = DateTime.Now,
                    RecordStatus = false,
                    FundStatusId = 1,
                    FundTypeId = 1,
                    FundPledgeFlag = false,
                    FundOpenDate = DateTime.Today,
                    FundIncomeDept = "",
                    FundIncomeAccount = "",
                    FundIncomeFund = "",
                    FundCashDept = "",
                    FundCashAccount = "",
                    FundCashFund = "",
                };
                DbUtil.Db.ContributionFunds.InsertOnSubmit(f);
                DbUtil.Db.SubmitChanges();
                return RedirectToAction("Edit", "Fund", new { id = f.FundId });
            }
            catch
            {
                return Redirect("/Fund/");
            }
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            var fund = DbUtil.Db.ContributionFunds.SingleOrDefault(f => f.FundId == id);
            if (fund == null)
                Redirect("/Fund/");
            return View(fund);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            var f = DbUtil.Db.ContributionFunds.SingleOrDefault(fu => fu.FundId == id);
            if (f != null)
                DbUtil.Db.ContributionFunds.DeleteOnSubmit(f);
            DbUtil.Db.SubmitChanges();
            return Redirect("/Fund/");
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update(int FundId)
        {
            var fund = DbUtil.Db.ContributionFunds.SingleOrDefault(f => f.FundId == FundId);
            if (fund != null)
                UpdateModel(fund);
            DbUtil.Db.SubmitChanges();
            return Redirect("/Fund/");
        }
        public ActionResult FundStatus(ContributionFund fund)
        {
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "Open", Value = "1", Selected = fund.FundStatusId == 1});
            list.Add(new SelectListItem { Text = "Closed", Value = "2", Selected = fund.FundStatusId == 2});
            return View(list);
        }
        public ActionResult FundType(ContributionFund fund)
        {
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "1", Value = "1", Selected = fund.FundTypeId == 1 });
            list.Add(new SelectListItem { Text = "2", Value = "2", Selected = fund.FundTypeId == 2 });
            list.Add(new SelectListItem { Text = "3", Value = "3", Selected = fund.FundTypeId == 3 });
            return View(list);
        }
    }
}
