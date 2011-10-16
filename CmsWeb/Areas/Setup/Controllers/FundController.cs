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
    public class FundController : CmsStaffController
    {
        public ActionResult Index(string sort)
        {
            var m = DbUtil.Db.ContributionFunds.AsEnumerable();
            switch (sort)
            {
                case "FundId":
                    m = from f in m
                        orderby f.FundStatusId, f.FundId
                        select f;
                    break;
                case "Created":
                    m = from f in m
                        orderby f.CreatedDate descending
                        select f;
                    break;
                case "Sort":
                    m = from f in m
                        orderby f.OnlineSort != null ? 1 : 2, f.OnlineSort, f.FundId
                        select f;
                    break;
                case "FundName":
                    m = from f in m
                        orderby f.FundName
                        select f;
                    break;
                case "FundDescription":
                    m = from f in m
                        orderby f.FundDescription
                        select f;
                    break;
                case "FundStatusId":
                    m = from f in m
                        orderby f.FundStatusId, f.OnlineSort != null ? 1 : 2, f.OnlineSort, f.FundId
                        select f;
                    break;
                case "FundPledgeFlag":
                    m = from f in m
                        orderby f.FundPledgeFlag, f.FundStatusId, f.OnlineSort != null ? 1 : 2, f.OnlineSort, f.FundId
                        select f;
                    break;
            }
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(string fundid)
        {
            var id = fundid.ToInt();
            if (id == 0)
            {
                ModelState.AddModelError("fundid", "expected an integer (account number)");
                var m = DbUtil.Db.ContributionFunds.AsEnumerable();
                return View("Index", m);
            }
            try
            {
                var f = new ContributionFund 
                { 
                    FundName = "new fund", 
                    FundId=id,
                    CreatedBy = Util.UserId1,
                    CreatedDate = Util.Now,
                    FundStatusId = 1,
                    FundTypeId = 1,
                    FundPledgeFlag = false,
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

            if (!fund.FundIncomeAccount.HasValue())
                ModelState.AddModelError("FundIncomeAccount", "Need an account number");
            if (!fund.FundIncomeDept.HasValue())
                ModelState.AddModelError("FundIncomeDept", "Need an account number");
            if (!fund.FundIncomeFund.HasValue())
                ModelState.AddModelError("FundIncomeFund", "Need an account number");

            if (!fund.FundCashAccount.HasValue())
                ModelState.AddModelError("FundCashAccount", "Need an account number");
            if (!fund.FundCashDept.HasValue())
                ModelState.AddModelError("FundCashDept", "Need an account number");
            if (!fund.FundCashFund.HasValue())
                ModelState.AddModelError("FundCashFund", "Need an account number");

            if (ModelState.IsValid)
            {
                DbUtil.Db.SubmitChanges();
                return RedirectToAction("Index");
            }
            return View("Edit", fund);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult EditOrder(string id, int? value)
        {
            var iid = id.Substring(1).ToInt();
            var fund = DbUtil.Db.ContributionFunds.SingleOrDefault(m => m.FundId == iid);
            fund.OnlineSort = value;
            DbUtil.Db.SubmitChanges();
            return Content(value.ToString());
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult EditStatus(string id, int value)
        {
            var iid = id.Substring(1).ToInt();
            var fund = DbUtil.Db.ContributionFunds.SingleOrDefault(m => m.FundId == iid);
            fund.FundStatusId = value;
            DbUtil.Db.SubmitChanges();
            return Content(value == 1 ? "Open" : "Closed");
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
