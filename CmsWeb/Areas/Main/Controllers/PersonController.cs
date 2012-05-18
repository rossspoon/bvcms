using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;
using UtilityExtensions;
using System.Text;
using CmsWeb.Models.PersonPage;
using CmsWeb.Models;
using System.Diagnostics;
using System.Web.Routing;
using System.Threading;
using System.Web.Security;
using CmsData.Codes;

namespace CmsWeb.Areas.Main.Controllers
{
	[ValidateInput(false)]
	[SessionExpire]
	public class PersonController : CmsStaffController
	{
		protected override void Initialize(RequestContext requestContext)
		{
			NoCheckRole = true;
			base.Initialize(requestContext);
		}
		public ActionResult Current()
		{
			return Redirect("/Person/Index/" + Util2.CurrentPeopleId);
		}
		public ActionResult Index(int? id)
		{
			if (!id.HasValue)
				return Content("no id");
			var m = new PersonModel(id);
			if (User.IsInRole("Access"))
			{
				if (m == null)
					return Content("no person");
				if (m.displayperson == null)
					return Content("person not found");
			}
			else
				if (m.Person == null || !m.Person.CanUserSee)
					return Content("no access");
			if (Util2.OrgMembersOnly)
			{
				var omotag = DbUtil.Db.OrgMembersOnlyTag2();
				if (!DbUtil.Db.TagPeople.Any(pt => pt.PeopleId == id && pt.Id == omotag.Id))
				{
					DbUtil.LogActivity("Trying to view person: {0}".Fmt(m.displayperson.Name));
					return Content("<h3 style='color:red'>{0}</h3>\n<a href='{1}'>{2}</a>"
						.Fmt("You must be a member one of this person's organizations to have access to this page",
						"javascript: history.go(-1)", "Go Back"));
				}
			}
			else if (Util2.OrgLeadersOnly)
			{
				var olotag = DbUtil.Db.OrgLeadersOnlyTag2();
				if (!DbUtil.Db.TagPeople.Any(pt => pt.PeopleId == id && pt.Id == olotag.Id))
				{
					DbUtil.LogActivity("Trying to view person: {0}".Fmt(m.displayperson.Name));
					return Content("<h3 style='color:red'>{0}</h3>\n<a href='{1}'>{2}</a>"
						.Fmt("You must be a leader of one of this person's organizations to have access to this page",
						"javascript: history.go(-1)", "Go Back"));
				}
			}
			ViewData["Comments"] = Util.SafeFormat(m.Person.Comments);
			ViewData["PeopleId"] = id.Value;
			Util2.CurrentPeopleId = id.Value;
			Session["ActivePerson"] = m.displayperson.Name;
			DbUtil.LogActivity("Viewing Person: {0}".Fmt(m.displayperson.Name));
			InitExportToolbar(id);
			return View(m);
		}
		[Authorize(Roles = "Admin")]
		public ActionResult Move(int id, int to)
		{
			var p = DbUtil.Db.People.Single(pp => pp.PeopleId == id);
			try
			{
				p.MovePersonStuff(DbUtil.Db, to);
				DbUtil.Db.SubmitChanges();
			}
			catch (Exception ex)
			{
				return Content(ex.Message);
			}
			return Content("ok");
		}
		[Authorize(Roles = "Admin")]
		public ActionResult Impersonate(string id)
		{
			var user = DbUtil.Db.Users.SingleOrDefault(uu => uu.Username == id);
			if (user == null)
				return Content("no user");
			if (user.Roles.Contains("Finance") && !User.IsInRole("Finance"))
				return Content("cannot impersonate finance");
			FormsAuthentication.SetAuthCookie(id, false);
			AccountModel.SetUserInfo(id, Session);
			Util.FormsBasedAuthentication = true;
			Util.UserPeopleId = user.PeopleId;
			Util.UserPreferredName = user.Username;
			return Redirect("/");
		}

		[Authorize(Roles = "Admin")]
		public ActionResult Delete(int id)
		{
			Util.Auditing = false;
			var person = DbUtil.Db.LoadPersonById(id);
			if (person == null)
				return Content("error, bad peopleid");

			var p = person.Family.People.FirstOrDefault(m => m.PeopleId != id);
			if (p != null)
			{
				Util2.CurrentPeopleId = p.PeopleId;
				Session["ActivePerson"] = p.Name;
			}
			else
			{
				Util2.CurrentPeopleId = 0;
				Session.Remove("ActivePerson");
			}

			if (!person.PurgePerson(DbUtil.Db))
				return Content("error, not deleted");

			DbUtil.LogActivity("Deleted Record {0}".Fmt(person.PeopleId));
			return Content("ok");
		}
		[HttpPost]
		public ActionResult Tag(int id)
		{
			Person.Tag(DbUtil.Db, id, Util2.CurrentTagName, Util2.CurrentTagOwnerId, DbUtil.TagTypeId_Personal);
			DbUtil.Db.SubmitChanges();
			return new EmptyResult();
		}
		[HttpPost]
		public ActionResult UnTag(int id)
		{
			Person.UnTag(id, Util2.CurrentTagName, Util2.CurrentTagOwnerId, DbUtil.TagTypeId_Personal);
			DbUtil.Db.SubmitChanges();
			return new EmptyResult();
		}
		[HttpPost]
		public ActionResult FamilyGrid(int id)
		{
			var m = new PersonFamilyModel(id);
			UpdateModel(m.Pager);
			return View(m);
		}
		[HttpPost]
		public ActionResult EnrollGrid(int id)
		{
			var m = new PersonEnrollmentsModel(id);
			DbUtil.LogActivity("Viewing Enrollments for: {0}".Fmt(m.person.Name));
			UpdateModel(m.Pager);
			return View(m);
		}
		[HttpPost]
		public ActionResult PrevEnrollGrid(int id)
		{
			var m = new PersonPrevEnrollmentsModel(id);
			DbUtil.LogActivity("Viewing Prev Enrollments for: {0}".Fmt(m.person.Name));
			UpdateModel(m.Pager);
			return View(m);
		}
		[HttpPost]
		public ActionResult PendingEnrollGrid(int id)
		{
			var m = new PersonPendingEnrollmentsModel(id);
			DbUtil.LogActivity("Viewing Pending Enrollments for: {0}".Fmt(m.person.Name));
			return View(m);
		}
		[HttpPost]
		public ActionResult AttendanceGrid(int id, bool? future)
		{
			var m = new PersonAttendHistoryModel(id, future == true);
			DbUtil.LogActivity("Viewing Attendance History for: {0}".Fmt(Session["ActivePerson"]));
			UpdateModel(m.Pager);
			return View(m);
		}
		[HttpPost]
		public ActionResult ContactsMadeGrid(int id)
		{
			var m = new PersonContactsMadeModel(id);
			DbUtil.LogActivity("Viewing Contacts Tab for: {0}".Fmt(Session["ActivePerson"]));
			UpdateModel(m.Pager);
			return View(m);
		}
		[HttpPost]
		public ActionResult ContactsReceivedGrid(int id)
		{
			var m = new PersonContactsReceivedModel(id);
			UpdateModel(m.Pager);
			return View(m);
		}
		[HttpPost]
		public ActionResult PendingTasksGrid(int id)
		{
			var m = new TaskModel();
			return View(m.TasksAboutList(id));
		}
		[HttpPost]
		public ActionResult AddContactMade(int id)
		{
			var p = DbUtil.Db.LoadPersonById(id);
			DbUtil.LogActivity("Adding contact from: {0}".Fmt(p.Name));
			var c = new CmsData.Contact
			{
				CreatedDate = Util.Now,
				CreatedBy = Util.UserId1,
				ContactDate = Util.Now.Date,
				ContactTypeId = 99,
				ContactReasonId = 99,
			};

			DbUtil.Db.Contacts.InsertOnSubmit(c);
			DbUtil.Db.SubmitChanges();

			var cp = new Contactor
			{
				PeopleId = p.PeopleId,
				ContactId = c.ContactId
			};

			DbUtil.Db.Contactors.InsertOnSubmit(cp);
			DbUtil.Db.SubmitChanges();

			return Content("/Contact.aspx?id=" + c.ContactId);
		}
		[HttpPost]
		public ActionResult AddContactReceived(int id)
		{
			var p = DbUtil.Db.LoadPersonById(id);
			DbUtil.LogActivity("Adding contact to: {0}".Fmt(p.Name));
			var c = new CmsData.Contact
			{
				CreatedDate = Util.Now,
				CreatedBy = Util.UserId1,
				ContactDate = Util.Now.Date,
				ContactTypeId = 99,
				ContactReasonId = 99,
			};

			DbUtil.Db.Contacts.InsertOnSubmit(c);
			DbUtil.Db.SubmitChanges();

			var pc = new Contactee
			{
				PeopleId = p.PeopleId,
				ContactId = c.ContactId
			};

			DbUtil.Db.Contactees.InsertOnSubmit(pc);
			DbUtil.Db.SubmitChanges();

			return Content("/Contact.aspx?id=" + c.ContactId);
		}
		[HttpPost]
		public ActionResult AddAboutTask(int id)
		{
			var p = DbUtil.Db.LoadPersonById(id);
			DbUtil.LogActivity("Adding Task for: {0}".Fmt(Session["ActivePerson"]));

			var pid = Util.UserPeopleId.Value;
			var active = TaskStatusCode.Active;
			var t = new Task
			{
				OwnerId = pid,
				Description = "Please Contact",
				ForceCompleteWContact = true,
				ListId = TaskModel.InBoxId(pid),
				StatusId = active,
			};
			p.TasksAboutPerson.Add(t);
			DbUtil.Db.SubmitChanges();
			return Content("/Task/List/{0}".Fmt(t.Id));
		}
		[HttpPost]
		public ActionResult BusinessCard(int id)
		{
			var m = new PersonModel(id);
			return View(m.displayperson);
		}
		public ContentResult Schools(string q, int limit)
		{
			var qu = from p in DbUtil.Db.People
					 where p.SchoolOther.Contains(q)
					 group p by p.SchoolOther into g
					 select g.Key;
			return Content(string.Join("\n", qu.Take(limit).ToArray()));
		}
		public ContentResult Employers(string q, int limit)
		{
			var qu = from p in DbUtil.Db.People
					 where p.EmployerOther.Contains(q)
					 group p by p.EmployerOther into g
					 select g.Key;
			return Content(string.Join("\n", qu.Take(limit).ToArray()));
		}
		public ContentResult Occupations(string q, int limit)
		{
			var qu = from p in DbUtil.Db.People
					 where p.OccupationOther.Contains(q)
					 group p by p.OccupationOther into g
					 select g.Key;
			return Content(string.Join("\n", qu.Take(limit).ToArray()));
		}
		public ContentResult Churches(string q, int limit)
		{
			var qu = from r in DbUtil.Db.ViewChurches
					 where r.C.Contains(q)
					 select r.C;
			return Content(string.Join("\n", qu.Take(limit).ToArray()));
		}
		[HttpPost]
		public ActionResult BasicDisplay(int id)
		{
			InitExportToolbar(id);
			var m = BasicPersonInfo.GetBasicPersonInfo(id);
			return View(m);
		}
		[HttpPost]
		public ActionResult BasicEdit(int id)
		{
			var m = BasicPersonInfo.GetBasicPersonInfo(id);
			return View(m);
		}
		[HttpPost]
		public ActionResult BasicUpdate(int id)
		{
			var m = BasicPersonInfo.GetBasicPersonInfo(id);
			UpdateModel(m);
			m.UpdatePerson();
			m = BasicPersonInfo.GetBasicPersonInfo(id);
			DbUtil.LogActivity("Update Basic Info for: {0}".Fmt(m.person.Name));
			InitExportToolbar(id);
			return View("BasicDisplay", m);
		}
		[HttpPost]
		public ActionResult Reverse(int id, string field, string value, string pf)
		{
			var m = new PersonModel(id);
			m.Reverse(field, value, pf);
			return View("ChangesGrid", m);
		}
		[HttpPost]
		public ActionResult AddressDisplay(int id, string type)
		{
			var m = AddressInfo.GetAddressInfo(id, type);
			return View(m);
		}
		[HttpPost]
		public ActionResult AddressEdit(int id, string type)
		{
			var m = AddressInfo.GetAddressInfo(id, type);
			return View(m);
		}
		[HttpPost]
		public ActionResult AddressUpdate(int id, string type)
		{
			var m = AddressInfo.GetAddressInfo(id, type);
			UpdateModel(m);
			m.UpdateAddress(ModelState);
			if (!ModelState.IsValid)
				return View("AddressEdit", m);
			DbUtil.LogActivity("Update Address for: {0}".Fmt(m.person.Name));
			return View("AddressDisplay", m);
		}
		[HttpPost]
		public ActionResult MemberDisplay(int id)
		{
			var m = MemberInfo.GetMemberInfo(id);
			return View(m);
		}
		[HttpPost]
		public ActionResult MemberEdit(int id)
		{
			var m = MemberInfo.GetMemberInfo(id);
			return View(m);
		}
		[HttpPost]
		public ActionResult MemberUpdate(int id)
		{
			var m = MemberInfo.GetMemberInfo(id);
			UpdateModel(m);
			var ret = m.UpdateMember();
			if (ret != "ok")
			{
				ModelState.AddModelError("MemberTab", ret);
				return View("MemberEdit", m);
			}
			m = MemberInfo.GetMemberInfo(id);
			DbUtil.LogActivity("Update Member Info for: {0}".Fmt(Session["ActivePerson"]));
			return View("MemberDisplay", m);
		}
		[HttpPost]
		public ActionResult GrowthDisplay(int id)
		{
			var m = GrowthInfo.GetGrowthInfo(id);
			return View(m);
		}
		[HttpPost]
		public ActionResult GrowthEdit(int id)
		{
			var m = GrowthInfo.GetGrowthInfo(id);
			return View(m);
		}
		[HttpPost]
		public ActionResult GrowthUpdate(int id)
		{
			var m = GrowthInfo.GetGrowthInfo(id);
			UpdateModel(m);
			m.UpdateGrowth();
			DbUtil.LogActivity("Update Growth Info for: {0}".Fmt(Session["ActivePerson"]));
			return View("GrowthDisplay", m);
		}
		[HttpPost]
		public ActionResult CommentsDisplay(int id)
		{
			ViewData["Comments"] = Util.SafeFormat(DbUtil.Db.People.Single(p => p.PeopleId == id).Comments);
			ViewData["PeopleId"] = id;
			return View();
		}
		[HttpPost]
		public ActionResult CommentsEdit(int id)
		{
			ViewData["Comments"] = DbUtil.Db.People.Single(p => p.PeopleId == id).Comments;
			ViewData["PeopleId"] = id;
			return View();
		}
		[HttpPost]
		public ActionResult CommentsUpdate(int id, string Comments)
		{
			var p = DbUtil.Db.LoadPersonById(id);
			p.Comments = Comments;
			DbUtil.Db.SubmitChanges();
			ViewData["Comments"] = Util.SafeFormat(Comments);
			ViewData["PeopleId"] = id;
			DbUtil.LogActivity("Update Comments for: {0}".Fmt(Session["ActivePerson"]));
			return View("CommentsDisplay");
		}
		[HttpPost]
		public ActionResult MemberNotesDisplay(int id)
		{
			var m = MemberNotesInfo.GetMemberNotesInfo(id);
			return View(m);
		}
		[HttpPost]
		public ActionResult MemberNotesEdit(int id)
		{
			var m = MemberNotesInfo.GetMemberNotesInfo(id);
			return View(m);
		}
		[HttpPost]
		public ActionResult MemberNotesUpdate(int id)
		{
			var m = MemberNotesInfo.GetMemberNotesInfo(id);
			UpdateModel(m);
			m.UpdateMemberNotes();
			DbUtil.LogActivity("Update Member Notes for: {0}".Fmt(Session["ActivePerson"]));
			return View("MemberNotesDisplay", m);
		}
		[HttpPost]
		public ActionResult RecRegDisplay(int id)
		{
			var m = RecRegInfo.GetRecRegInfo(id);
			return View(m);
		}
		[HttpPost]
		public ActionResult RecRegEdit(int id)
		{
			var m = RecRegInfo.GetRecRegInfo(id);
			return View(m);
		}
		[HttpPost]
		public ActionResult RecRegUpdate(int id)
		{
			var m = RecRegInfo.GetRecRegInfo(id);
			UpdateModel(m);
			m.UpdateRecReg();
			DbUtil.LogActivity("Update Registration Tab for: {0}".Fmt(Session["ActivePerson"]));
			return View("RecRegDisplay", m);
		}
		[HttpPost]
		public ActionResult AddContact(int id)
		{
			var c = new ContentResult();
			c.Content = CmsData.Contact.AddContact(id).ToString();
			return c;
		}
		[HttpPost]
		public ActionResult AddTasks(int id)
		{
			var c = new ContentResult();
			c.Content = Task.AddTasks(id).ToString();
			return c;
		}
		[Authorize(Roles = "Admin")]
		public ActionResult UserDialog(int? id)
		{
			User u = null;
			if (id.HasValue)
				u = DbUtil.Db.Users.Single(us => us.UserId == id);
			else
			{
				u = AccountModel.AddUser(Util2.CurrentPeopleId);
				DbUtil.LogActivity("New User for: {0}".Fmt(Session["ActivePerson"]));
			}
			return View(u);
		}
		[Authorize(Roles = "Admin")]
		[HttpPost]
		public ActionResult UserUpdate(int id, string username, string password2, bool islockedout, string[] role)
		{
			var u = DbUtil.Db.Users.Single(us => us.UserId == id);
			if (u.Username != username)
			{
				var uu = DbUtil.Db.Users.SingleOrDefault(us => us.Username == username);
				if (uu != null)
					return Content("error: username already exists");
			}
			u.Username = username;
			u.IsLockedOut = islockedout;
			u.SetRoles(DbUtil.Db, role, User.IsInRole("Finance"));
			if (password2.HasValue())
				u.ChangePassword(password2);
			DbUtil.Db.SubmitChanges();
			DbUtil.LogActivity("Update User for: {0}".Fmt(Session["ActivePerson"]));
			return Content("ok");
		}
		[Authorize(Roles = "Admin")]
		[HttpPost]
		public ActionResult UserWelcome(int id, string username, string password2, bool islockedout, string[] role)
		{
			var u = DbUtil.Db.Users.Single(us => us.UserId == id);
			if (u.Username != username)
			{
				var uu = DbUtil.Db.Users.SingleOrDefault(us => us.Username == username);
				if (uu != null)
					return Content("error: username already exists");
			}
			u.Username = username;
			u.IsLockedOut = islockedout;
			u.SetRoles(DbUtil.Db, role, User.IsInRole("Finance"));
			if (password2.HasValue())
				u.ChangePassword(password2);
			DbUtil.Db.SubmitChanges();
			AccountModel.SendNewUserEmail(username);
			DbUtil.LogActivity("Welcome Email for: {0}".Fmt(Session["ActivePerson"]));
			return Content("ok");
		}
		[Authorize(Roles = "Admin")]
		[HttpPost]
		public ActionResult UserDelete(int id)
		{
			var Db = DbUtil.Db;
			Db.PurgeUser(id);
			return Content("ok");
		}
		[HttpPost]
		public ActionResult UserInfoGrid(int id)
		{
			var p = DbUtil.Db.LoadPersonById(id);
			return View(p);
		}
		[HttpPost]
		public ActionResult VolunteerDisplay(int id)
		{
			var m = new CmsWeb.Models.PersonPage.VolunteerModel(id);
			return View(m);
		}
		[HttpPost]
		public ContentResult DeleteExtra(int id, string field)
		{
			var e = DbUtil.Db.PeopleExtras.First(ee => ee.PeopleId == id && ee.Field == field);
			DbUtil.Db.PeopleExtras.DeleteOnSubmit(e);
			DbUtil.Db.SubmitChanges();
			return Content("done");
		}
		[HttpPost]
		public ContentResult EditExtra(string id, string value)
		{
			var a = id.SplitStr("-", 2);
			var b = a[1].SplitStr(".", 2);
			var p = DbUtil.Db.LoadPersonById(b[1].ToInt());
			switch (a[0])
			{
				case "s":
					p.AddEditExtraValue(b[0], value);
					break;
				case "t":
					p.AddEditExtraData(b[0], value);
					break;
				case "d":
					{
						DateTime dt;
						if (DateTime.TryParse(value, out dt))
						{
							p.AddEditExtraDate(b[0], dt);
							value = dt.ToShortDateString();
						}
						else
							value = "";
					}
					break;
				case "i":
					p.AddEditExtraInt(b[0], value.ToInt());
					break;
				case "b":
					if (value == "True")
						p.AddEditExtraBool(b[0], true);
					else
						p.RemoveExtraValue(DbUtil.Db, b[0]);
					break;
			}
			DbUtil.Db.SubmitChanges();
			return Content(value);
		}
		[HttpPost]
		public JsonResult ExtraValues(string id)
		{
			var a = id.SplitStr("-", 2);
			var b = a[1].SplitStr(".", 2);
			var f = CmsWeb.Code.StandardExtraValues.GetExtraValues().Single(ee => ee.name == b[0]);
			var j = Json(f.Codes.ToDictionary(ee => ee, ee => ee));
			return j;
		}
		[HttpPost]
		public ActionResult NewExtraValue(int id, string field, string type, string value)
		{
			var v = new PeopleExtra { PeopleId = id, Field = field };
			DbUtil.Db.PeopleExtras.InsertOnSubmit(v);
			switch (type)
			{
				case "string":
					v.StrValue = value;
					break;
				case "text":
					v.Data = value;
					break;
				case "date":
					var dt = DateTime.MinValue;
					DateTime.TryParse(value, out dt);
					v.DateValue = dt;
					break;
				case "int":
					v.IntValue = value.ToInt();
					break;
			}
			try
			{
				DbUtil.Db.SubmitChanges();
			}
			catch (Exception ex)
			{
				return Content("error: " + ex.Message);
			}
			return Content("ok");
		}
		[HttpPost]
		public ActionResult ExtrasGrid(int id)
		{
			var p = DbUtil.Db.LoadPersonById(id);
			return View(p);
		}
		[HttpPost]
		public ActionResult ChangesGrid(int id)
		{
			var m = new PersonModel(id);
			return View(m);
		}
		[HttpPost]
		public ActionResult DuplicatesGrid(int id)
		{
			var m = new DuplicatesModel(id);
			return View(m);
		}
		private void InitExportToolbar(int? id)
		{
			var qb = DbUtil.Db.QueryBuilderIsCurrentPerson();
			ViewData["queryid"] = qb.QueryId;
			ViewData["TagAction"] = "/Person/Tag/" + id;
			ViewData["UnTagAction"] = "/Person/UnTag/" + id;
			ViewData["AddContact"] = "/Person/AddContactReceived/" + id;
			ViewData["AddTasks"] = "/Person/AddAboutTask/" + id;
		}
		public class CurrentRegistration
		{
			public int OrgId { get; set; }
			public string Name { get; set; }
			public string Description { get; set; }
		}
		public ActionResult CurrentRegistrations()
		{
			var types = new[] {1, 2, 10, 11, 5, 6, 9, 14, 15};
			var picklistorgs = DbUtil.Db.ViewPickListOrgs.Select(pp => pp.OrgId).ToArray();
			var dt = DateTime.Today;
			var q = from o in DbUtil.Db.Organizations
					where !picklistorgs.Contains(o.OrganizationId)
					where types.Contains(o.RegistrationTypeId ?? 0)
					where o.RegEnd > dt || o.RegEnd == null
					where (o.RegistrationClosed ?? false) == true
					where o.OrganizationStatusId == OrgStatusCode.Active
					orderby o.OrganizationName
					select new CurrentRegistration()
					{
						OrgId = o.OrganizationId,
						Name = o.OrganizationName,
						Description = o.Description
					};
			return View(q);
		}
	}
}
