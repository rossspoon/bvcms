using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CmsData;
using UtilityExtensions;

namespace CMSWeb.Areas.Setup.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FundController : CmsController
    {
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
                    CreatedDate = Util.Now,
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
                return RedirectToAction("Index");
            }
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            var fund = DbUtil.Db.ContributionFunds.SingleOrDefault(f => f.FundId == id);
            if (fund == null)
                RedirectToAction("Index");
            return View(fund);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            var f = DbUtil.Db.ContributionFunds.SingleOrDefault(fu => fu.FundId == id);
            if (f != null)
                DbUtil.Db.ContributionFunds.DeleteOnSubmit(f);
            DbUtil.Db.SubmitChanges();
            return RedirectToAction("Index");
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update(int FundId)
        {
            var fund = DbUtil.Db.ContributionFunds.SingleOrDefault(f => f.FundId == FundId);
            if (fund != null)
                UpdateModel(fund);
            DbUtil.Db.SubmitChanges();
            return RedirectToAction("Index");
        }
        public static List<SelectListItem> GetFundStatusList()
        {
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "Open", Value = "1" });
            list.Add(new SelectListItem { Text = "Closed", Value = "2" });
            return list;
        }
        public static List<SelectListItem> GetFundTypeList()
        {
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "1", Value = "1" });
            list.Add(new SelectListItem { Text = "2", Value = "2" });
            list.Add(new SelectListItem { Text = "3", Value = "3" });
            return list;
        }
    }
}
