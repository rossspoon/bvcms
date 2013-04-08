using System;
using System.Linq;
using System.Web.Mvc;
using System.Threading;
using CmsWeb.Models;
using UtilityExtensions;
using CmsData;
using Alias = System.Threading.Tasks;

namespace CmsWeb.Areas.Manage.Controllers
{
	[Authorize(Roles = "Admin")]
	public class UploadPeopleController : CmsStaffController
	{
		[HttpGet]
		public ActionResult Index()
		{
			ViewData["text"] = "";
			return View();
		}

		[HttpPost]
		[ValidateInput(false)]
		public ActionResult Upload(string text)
		{
			string host = Util.Host;
			var runningtotals = new UploadPeopleRun
			                    	{
			                    		Started = DateTime.Now,
			                    		Count = 0,
			                    		Processed = 0
			                    	};
			DbUtil.Db.UploadPeopleRuns.InsertOnSubmit(runningtotals);
			DbUtil.Db.SubmitChanges();
			var pid = Util.UserPeopleId;
            

			Alias.Task.Factory.StartNew(() =>
			{
				Thread.CurrentThread.Priority = ThreadPriority.Lowest;
				var Db = new CMSDataContext(Util.GetConnectionString(host));
				try
				{
					var m = new UploadPeopleModel(Db, pid ?? 0);
					m.DoUpload(text, testing: true);
					Db.Dispose();
					Db = new CMSDataContext(Util.GetConnectionString(host));
					m = new UploadPeopleModel(Db, pid ?? 0);
					m.DoUpload(text);
				}
				catch (Exception ex)
				{
					var rt = Db.UploadPeopleRuns.OrderByDescending(mm => mm.Id).First();
					rt.Error = ex.Message.Truncate(200);
					Db.SubmitChanges();
				}
			});
			return Redirect("/UploadPeople/Progress");
		}

		[HttpGet]
		public ActionResult Progress()
		{
			var rt = DbUtil.Db.UploadPeopleRuns.OrderByDescending(mm => mm.Id).First();
			return View(rt);
		}

		[HttpPost]
		public JsonResult Progress2()
		{
			var r = DbUtil.Db.UploadPeopleRuns.OrderByDescending(mm => mm.Id).First();
			return Json(new {r.Count, r.Error, r.Processed, Completed = r.Completed.ToString(), r.Running});
		}

	}
}

