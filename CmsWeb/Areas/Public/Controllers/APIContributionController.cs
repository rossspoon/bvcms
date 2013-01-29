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
		public ActionResult PostContribution(int PeopleId, decimal Amount, string desc, int FundId, string date, int? contributiontype)
		{
			var ret = AuthenticateDeveloper(addrole: "Finance");
			if (ret.StartsWith("!"))
				return Content(@"<PostContribution status=""error"">" + ret.Substring(1) + "</PostContribution>");
			DbUtil.LogActivity("APIContribution PostContribution");
			return Content(new APIContribution(DbUtil.Db).PostContribution(PeopleId, Amount, FundId, desc, date, contributiontype), "text/xml");
		}
		[HttpGet]
		public ActionResult Contributions(int id, int Year)
		{
			var ret = AuthenticateDeveloper(addrole: "Finance");
			if (ret.StartsWith("!"))
				return Content(@"<Contributions status=""error"">" + ret.Substring(1) + "</Contributions>");
			DbUtil.LogActivity("APIContribution Contributions for ({0})".Fmt(id));
			return Content(new APIContribution(DbUtil.Db).Contributions(id, Year), "text/xml");
		}
    }
}