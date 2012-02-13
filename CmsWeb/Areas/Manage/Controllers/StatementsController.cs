using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsWeb.Areas.Main.Models.Report;
using System.IO;
using CmsData;
using UtilityExtensions;
using CmsWeb.Models;
using System.Text;
using System.Web.UI;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace CmsWeb.Areas.Manage.Controllers
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
			string output = null;
			if (PDF == true)
				output = Server.MapPath("/contributions_{0}.pdf".Fmt(Util.Host));
			else
				output = Server.MapPath("/contributions_{0}.txt").Fmt(Util.Host);
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
	}
}
