using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsWeb.Areas.Finance.Models.Report;
using CmsData;
using System.IO;
using Intuit.Ipp.Data.Qbo;
using UtilityExtensions;
using CmsWeb.Models;
using System.Text;
using System.Web.UI;
using System.Data.SqlClient;
using Intuit.Ipp.Data.Qbo;

namespace CmsWeb.Areas.Finance.Controllers
{
    [Authorize(Roles = "Finance")]
    public class FinanceReportsController : CmsStaffController
    {
        public ActionResult ContributionYears(int id)
        {
            var m = new ContributionModel(id);
            return View(m);
        }
        public ActionResult ContributionStatement(int id, DateTime fromDate, DateTime toDate, int typ)
        {
            DbUtil.LogActivity("Contribution Statement for ({0})".Fmt(id));
            return new ContributionStatementResult
                       {
                           PeopleId = id, 
                           FromDate = fromDate,
                           ToDate = toDate,
                           typ = typ
                       };
        }
		[HttpGet]
        public ActionResult DonorTotalsByRange()
		{
			var m = new TotalsByFundModel();
            return View(m);
        }
		[HttpPost]
        public ActionResult DonorTotalsByRangeResults(TotalsByFundModel m)
        {
            return View(m);
        }
		[HttpGet]
        public ActionResult TotalsByFund()
		{
			var m = new TotalsByFundModel();
            return View(m);
        }
		[HttpPost]
        public ActionResult TotalsByFundResults(TotalsByFundModel m)
        {
            return View(m);
        }
        public ActionResult PledgeReport()
        {
        	var fd = DateTime.Parse("1/1/1900");
        	var td = DateTime.Parse("1/1/2099");
        	var q = from r in DbUtil.Db.PledgeReport(fd, td, 0)
        	        select r;
		    return View(q);
        }
        public ActionResult ManagedGiving()
        {
			var q = from rg in DbUtil.Db.ManagedGivings.ToList()
					orderby rg.NextDate
					select rg;
			return View(q);
        }
		[HttpGet]
		public ActionResult ManageGiving2(int id)
		{ 
			var m = new ManageGivingModel(id);
			m.testing = true;
			var body = ViewExtensions2.RenderPartialViewToString(this, "ManageGiving2", m);
			return Content(body);
		}

		[HttpPost]
		public ActionResult ToQuickBooks(TotalsByFundModel m)
		{
            List<int> lFunds = new List<int>();
			var entries = m.TotalsByFund();

            QuickBooksHelper qbh = new QuickBooksHelper();
            qbh.InitJournalEntires();

			foreach (var item in entries)
			{
                if (item.QBSynced > 0) continue;

				var accts = (from e in DbUtil.Db.ContributionFunds
							where e.FundId == item.FundId
							select e).Single();

                if (accts.QBAssetAccount > 0 && accts.QBIncomeAccount > 0)
                {
                    JournalEntryLine jelFrom = new JournalEntryLine();
                    jelFrom.Desc = item.FundName;
                    jelFrom.Amount = item.Total ?? 0;
                    jelFrom.AmountSpecified = true;
                    jelFrom.AccountId = new IdType() { Value = accts.QBIncomeAccount.ToString() };
                    jelFrom.PostingType = PostingTypeEnum.Credit;
                    jelFrom.PostingTypeSpecified = true;

                    JournalEntryLine jelTo = new JournalEntryLine();
                    jelTo.Desc = item.FundName;
                    jelTo.Amount = item.Total ?? 0;
                    jelTo.AmountSpecified = true;
                    jelTo.AccountId = new IdType() { Value = accts.QBAssetAccount.ToString() };
                    jelTo.PostingType = PostingTypeEnum.Debit;
                    jelTo.PostingTypeSpecified = true;

                    qbh.AddJournalEntry(jelFrom);
                    qbh.AddJournalEntry(jelTo);
                }

                lFunds.Add(item.FundId);
			}

            int iJournalID = qbh.CommitJournalEntries( "Bundle from BVCMS" );

            if (iJournalID > 0)
            {
                string sFundList = string.Join( ",", lFunds.ToArray() );
                DbUtil.Db.ExecuteCommand("UPDATE dbo.Contribution SET QBSyncID = " + iJournalID + " WHERE FundId IN (" + sFundList + ")");
            }

            return View("TotalsByFund", m);
		}
		public ActionResult PledgeFulfillments(int id)
		{
			var list = DbUtil.Db.PledgeFulfillment(id).OrderBy(vv => vv.Last).ThenBy(vv => vv.First);
			return new DataGridResult(list);
		}
    }
}
