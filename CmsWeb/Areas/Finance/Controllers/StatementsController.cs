using System;
using System.Linq;
using System.Web.Mvc;
using CmsWeb.Areas.Finance.Models.Report;
using CmsData;
using UtilityExtensions;
using System.Web.Configuration;

namespace CmsWeb.Areas.Finance.Controllers
{
	[Authorize(Roles="Finance")]
	public class StatementsController : CmsController 
	{
		public ActionResult Index()
		{
			return View();
		}
		[HttpPost]
		public ActionResult ContributionStatements(bool? PDF, DateTime? FromDate, DateTime? ToDate)
		{
			if (!FromDate.HasValue || !ToDate.HasValue)
				return Content("<h3>Must have a Startdate and Enddate</h3>");
			else
			{
				var runningtotals = new ContributionsRun
				{
					Started = DateTime.Now,
					Count = 0,
					Processed = 0
				};
				DbUtil.Db.ContributionsRuns.InsertOnSubmit(runningtotals);
				DbUtil.Db.SubmitChanges();
				var host = Util.Host;
				var output = Output(PDF);
				System.Threading.Tasks.Task.Factory.StartNew(() =>
				{
					System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.Lowest;
					var m = new ContributionStatementsExtract(host, FromDate.Value, ToDate.Value, PDF.Value, output);
					m.DoWork();
				});
			}
			return Redirect("/Statements/Progress");
		}
		public ActionResult SomeTaskCompleted(string result)
		{
			return Content(result);
		}
		private string Output(bool? PDF)
		{
			string output = WebConfigurationManager.AppSettings["SharedFolder"];
			if (PDF == true)
				output = output + "/Statements/contributions_{0}.pdf".Fmt(Util.Host);
			else
				output = output + "/Statements/contributions_{0}.txt".Fmt(Util.Host);
			return output;
		}
		[HttpPost]
		public JsonResult Progress2()
		{
			var r = DbUtil.Db.ContributionsRuns.OrderByDescending(mm => mm.Id).First();
			return Json(new { r.Count, r.Error, r.Processed, Completed = r.Completed.ToString(), r.Running } );
		}
		[HttpGet]
		public ActionResult Progress()
		{
			var r = DbUtil.Db.ContributionsRuns.OrderByDescending(mm => mm.Id).First();
			return View(r);
		}
		public ActionResult Download(bool? PDF = true)
		{
			string output = Output(PDF);
			if (!System.IO.File.Exists(output))
				return Content("no pending download");
			return new ContributionStatementsResult(output);
		}
		public ActionResult Download1(bool? PDF = true)
		{
			string output = Output(PDF).Replace(".pdf", "-1.pdf");
			if (!System.IO.File.Exists(output))
				return Content("no pending download");
			return new ContributionStatementsResult(output);
		}
		public ActionResult Download2(bool? PDF = true)
		{
			string output = Output(PDF).Replace(".pdf", "-2.pdf");
			if (!System.IO.File.Exists(output))
				return Content("no pending download");
			return new ContributionStatementsResult(output);
		}
	}
}
