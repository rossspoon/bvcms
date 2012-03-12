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
    }
}
