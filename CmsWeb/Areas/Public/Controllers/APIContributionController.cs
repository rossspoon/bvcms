using System;
using System.IO;
using System.Web.Mvc;
using CmsData;
using UtilityExtensions;
using CmsData.API;

namespace CmsWeb.Areas.Public.Controllers
{
    public class APIContributionController : CmsController
    {
		[HttpPost]
		public ActionResult PostContribution(int PeopleId, decimal Amount, int FundId, string desc)
		{
			var ret = AuthenticateDeveloper();
			if (ret.StartsWith("!"))
				return Content(@"<PostContribution status=""error"">" + ret.Substring(1) + "</PostContribution>");
			if (!User.IsInRole("Finance"))
				return Content(@"<PostContribution status=""error"">No Finance Role</PostContribution>");
			DbUtil.LogActivity("APIContribution PostContribution");
			return Content(new APIContribution(DbUtil.Db).PostContribution(PeopleId, Amount, FundId, desc), "text/xml");
		}
		[HttpGet]
		public ActionResult Contributions(int id, int Year)
		{
			var ret = AuthenticateDeveloper();
			if (ret.StartsWith("!"))
				return Content(@"<Contributions status=""error"">" + ret.Substring(1) + "</Contributions>");
			if (!User.IsInRole("Finance"))
				return Content(@"<Contributions status=""error"">No Finance Role</Contributions>");
			DbUtil.LogActivity("APIContribution Contributions for ({0})".Fmt(id));
			return Content(new APIContribution(DbUtil.Db).Contributions(id, Year), "text/xml");
		}
    }
}