using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CmsData;
using CmsWeb.Models;
using UtilityExtensions;
using System.Net.Mail;

namespace CmsWeb.Areas.Manage.Controllers
{
	public class EmailsController : CmsController
	{
		public ActionResult Index()
		{
			var m = new EmailsModel();
			return View(m);
		}
		public ActionResult SentBy(int? id)
		{
			var m = new EmailsModel { senderid = id };
			return View("Index", m);
		}
		public ActionResult SentTo(int? id)
		{
			var m = new EmailsModel { peopleid = id };
			return View("Index", m);
		}

		public ActionResult Details(int id, string filter)
		{
			var m = new EmailModel { id = id, filter = filter ?? "All" };
		    if (m.queue == null)
		        return Content("no email found");
			if (User.IsInRole("Admin") || User.IsInRole("ManageEmails"))
				return View(m);
			var u = DbUtil.Db.LoadPersonById(Util.UserPeopleId.Value);
			if (m.queue.FromAddr != u.EmailAddress
					&& !m.queue.EmailQueueTos.Any(et => et.PeopleId == u.PeopleId))
				return Content("not authorized");
			return View(m);
		}

		[Authorize(Roles = "Admin, ManageEmails")]
		public ActionResult Requeue(int id)
		{
			return Redirect("/Manage/Emails/Details/" + id);
		}
		public ActionResult DeleteQueued(int id)
		{
			var email = (from e in DbUtil.Db.EmailQueues
						 where e.Id == id
						 select e).Single();
			var m = new EmailModel { id = id };
			if (!m.CanDelete())
				return Redirect("/");
			DbUtil.Db.EmailQueueTos.DeleteAllOnSubmit(email.EmailQueueTos);
			DbUtil.Db.EmailQueues.DeleteOnSubmit(email);
			DbUtil.Db.SubmitChanges();
			return Redirect("/Manage/Emails");
		}
		public ActionResult MakePublic(int id)
		{
			var email = (from e in DbUtil.Db.EmailQueues
						 where e.Id == id
						 select e).Single();
			var m = new EmailModel { id = id };
			if (!User.IsInRole("Admin") && m.queue.QueuedBy != Util.UserPeopleId)
				return Redirect("/");
			email.PublicX = true;
			DbUtil.Db.SubmitChanges();
			return RedirectToAction("View", new { id = id });
		}
		[HttpPost]
		public ActionResult Recipients(int id, string filter)
		{
			var m = new EmailModel { id = id, filter = filter };
			UpdateModel(m.Pager);
			return View(m);
		}
		[HttpPost]
		public ActionResult List(EmailsModel m)
		{
			UpdateModel(m.Pager);
			return View(m);
		}
		public ActionResult Failed(int? id, string email)
		{
			var isadmin = User.IsInRole("Admin");
			var isdevel = User.IsInRole("Developer");
			var q = from e in DbUtil.Db.EmailQueueToFails
					where id == null || id == e.PeopleId
					where email == null || email == e.Email
					let et = DbUtil.Db.EmailQueueTos.SingleOrDefault(ef => ef.Id == e.Id && ef.PeopleId == e.PeopleId)
					orderby e.Time descending
					select new MailFail
						   {
							   time = e.Time,
							   eventx = e.EventX,
							   type = e.Bouncetype,
							   reason = e.Reason,
							   emailid = e.Id,
							   name = et != null ? et.Person.Name : "unknown",
							   subject = et != null ? et.EmailQueue.Subject : "unknown",
							   peopleid = e.PeopleId,
							   email = e.Email,
							   devel = isdevel,
							   admin = isadmin
						   };
			return View(q.Take(300));
		}
		[Authorize(Roles = "Admin")]
		[HttpPost]
		public ActionResult Unblock(string email)
		{
			var deletebounce = ConfigurationManager.AppSettings["DeleteBounce"] + email;
			var wc = new WebClient();
			var ret = wc.DownloadString(deletebounce);
			return Content(ret);
		}
		[Authorize(Roles = "Developer")]
		[HttpPost]
		public ActionResult Unspam(string email)
		{
			var deletespam = ConfigurationManager.AppSettings["DeleteSpamReport"] + email;
			var wc = new WebClient();
			var ret = wc.DownloadString(deletespam);
			return Content(ret);
		}

		public class MailFail
		{
			public DateTime? time { get; set; }
			public string eventx { get; set; }
			public string type { get; set; }
			public string reason { get; set; }
			public string name { get; set; }
			public int? peopleid { get; set; }
			public int? emailid { get; set; }
			public string subject { get; set; }
			public string email { get; set; }
			public bool admin { get; set; }
			public bool devel { get; set; }
			public bool canunblock
			{
				get
				{
					if (!admin || !email.HasValue())
						return false;
					if ((eventx != "bounce" || type != "blocked") && eventx != "dropped")
						return false;
					if (eventx == "dropped" && reason.Contains("spam", ignoreCase: true))
						return false;
					var deletebounce = ConfigurationManager.AppSettings["DeleteBounce"];
					if (!deletebounce.HasValue())
						return false;
					return true;
				}
			}
			public bool canunspam
			{
				get
				{
					if (!devel || !email.HasValue())
						return false;
					if ((eventx != "bounce" || type != "blocked") && eventx != "dropped")
						return false;
					if (eventx == "dropped" && !reason.Contains("spam", ignoreCase: true))
						return false;
					var deletespam = ConfigurationManager.AppSettings["DeleteSpamReport"];
					if (!deletespam.HasValue())
						return false;
					return true;
				}
			}
		}
	}

	public class EmailsViewController : CmsControllerNoHttps
	{
		public ActionResult View(int id)
		{
			var email = DbUtil.Db.EmailQueues.SingleOrDefault(ee => ee.Id == id);
			if (email == null)
				return Content("document not found, sorry");
			if ((email.PublicX ?? false) == false)
				return Content("no email available");
			var em = new EmailQueue
			{
				Subject = email.Subject,
				Body = email.Body.Replace("{track}", "").Replace("{first}", "")
			};
			return View(em);
		}
	}
}
