using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsWeb.Areas.Finance.Models.Report;
using CmsData;
using System.IO;
using UtilityExtensions;
using CmsWeb.Models;
using System.Text;
using System.Web.UI;
using System.Data.SqlClient;

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
        public ActionResult ContributionStatement(int id, DateTime FromDate, DateTime ToDate, int typ)
        {
            DbUtil.LogActivity("Contribution Statement for ({0})".Fmt(id));
            return new ContributionStatementResult { PeopleId = id, FromDate = FromDate, ToDate = ToDate, typ = typ };
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
			var body = CmsController.RenderPartialViewToString(this, "ManageGiving2", m);
			return Content(body);
		}

		[HttpPost]
		public ActionResult ToQuickBooks(TotalsByFundModel m)
		{
			var entries = m.TotalsByFund();

			foreach (var item in entries)
			{
				var accts = (from e in DbUtil.Db.ContributionFunds
							where e.FundId == item.FundId
							select e).Single();

				QuickBooksModel.CreateJournalEntry(item.FundName, item.Total ?? 0, accts.FundCashAccount.ToInt(), accts.FundAccountCode.ToInt());
			}

			return Content("");
		}
    }
}
