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

            return Content("ok");
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Tag(int id)
        {
            Person.Tag(DbUtil.Db, id, Util2.CurrentTagName, Util2.CurrentTagOwnerId, DbUtil.TagTypeId_Personal);
            DbUtil.Db.SubmitChanges();
            return new EmptyResult();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UnTag(int id)
        {
            Person.UnTag(id, Util2.CurrentTagName, Util2.CurrentTagOwnerId, DbUtil.TagTypeId_Personal);
            DbUtil.Db.SubmitChanges();
            return new EmptyResult();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult FamilyGrid(int id)
        {
            var m = new PersonFamilyModel(id);
            UpdateModel(m.Pager);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EnrollGrid(int id)
        {
            var m = new PersonEnrollmentsModel(id);
            UpdateModel(m.Pager);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PrevEnrollGrid(int id)
        {
            var m = new PersonPrevEnrollmentsModel(id);
            UpdateModel(m.Pager);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PendingEnrollGrid(int id)
        {
            var m = new PersonPendingEnrollmentsModel(id);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AttendanceGrid(int id, bool? future)
        {
            var m = new PersonAttendHistoryModel(id, future == true);
            UpdateModel(m.Pager);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ContactsMadeGrid(int id)
        {
            var m = new PersonContactsMadeModel(id);
            UpdateModel(m.Pager);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ContactsReceivedGrid(int id)
        {
            var m = new PersonContactsReceivedModel(id);
            UpdateModel(m.Pager);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PendingTasksGrid(int id)
        {
            var m = new TaskModel();
            return View(m.TasksAboutList(id));
        }
        [AcceptVerbs(HttpVerbs.Post)]
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
        [AcceptVerbs(HttpVerbs.Post)]
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
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddAboutTask(int id)
        {
            var p = DbUtil.Db.LoadPersonById(id);

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
        [AcceptVerbs(HttpVerbs.Post)]
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
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult BasicDisplay(int id)
        {
            InitExportToolbar(id);
            var m = BasicPersonInfo.GetBasicPersonInfo(id);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult BasicEdit(int id)
        {
            var m = BasicPersonInfo.GetBasicPersonInfo(id);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult BasicUpdate(int id)
        {
            var m = BasicPersonInfo.GetBasicPersonInfo(id);
            UpdateModel(m);
            m.UpdatePerson();
            m = BasicPersonInfo.GetBasicPersonInfo(id);
            InitExportToolbar(id);
            return View("BasicDisplay", m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddressDisplay(int id, string type)
        {
            var m = AddressInfo.GetAddressInfo(id, type);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddressEdit(int id, string type)
        {
            var m = AddressInfo.GetAddressInfo(id, type);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddressUpdate(int id, string type)
        {
            var m = AddressInfo.GetAddressInfo(id, type);
            UpdateModel(m);
            m.UpdateAddress(ModelState);
            if (!ModelState.IsValid)
                return View("AddressEdit", m);
            m = AddressInfo.GetAddressInfo(id, type);
            return View("AddressDisplay", m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MemberDisplay(int id)
        {
            var m = MemberInfo.GetMemberInfo(id);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MemberEdit(int id)
        {
            var m = MemberInfo.GetMemberInfo(id);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
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
            return View("MemberDisplay", m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GrowthDisplay(int id)
        {
            var m = GrowthInfo.GetGrowthInfo(id);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GrowthEdit(int id)
        {
            var m = GrowthInfo.GetGrowthInfo(id);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GrowthUpdate(int id)
        {
            var m = GrowthInfo.GetGrowthInfo(id);
            UpdateModel(m);
            m.UpdateGrowth();
            return View("GrowthDisplay", m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CommentsDisplay(int id)
        {
            ViewData["Comments"] = Util.SafeFormat(DbUtil.Db.People.Single(p => p.PeopleId == id).Comments);
            ViewData["PeopleId"] = id;
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CommentsEdit(int id)
        {
            ViewData["Comments"] = DbUtil.Db.People.Single(p => p.PeopleId == id).Comments;
            ViewData["PeopleId"] = id;
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CommentsUpdate(int id, string Comments)
        {
            var p = DbUtil.Db.LoadPersonById(id);
            p.Comments = Comments;
            DbUtil.Db.SubmitChanges();
            ViewData["Comments"] = Util.SafeFormat(Comments);
            ViewData["PeopleId"] = id;
            return View("CommentsDisplay");
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MemberNotesDisplay(int id)
        {
            var m = MemberNotesInfo.GetMemberNotesInfo(id);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MemberNotesEdit(int id)
        {
            var m = MemberNotesInfo.GetMemberNotesInfo(id);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MemberNotesUpdate(int id)
        {
            var m = MemberNotesInfo.GetMemberNotesInfo(id);
            UpdateModel(m);
            m.UpdateMemberNotes();
            return View("MemberNotesDisplay", m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RecRegDisplay(int id)
        {
            var m = RecRegInfo.GetRecRegInfo(id);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RecRegEdit(int id)
        {
            var m = RecRegInfo.GetRecRegInfo(id);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RecRegUpdate(int id)
        {
            var m = RecRegInfo.GetRecRegInfo(id);
            UpdateModel(m);
            m.UpdateRecReg();
            return View("RecRegDisplay", m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddContact(int id)
        {
            var c = new ContentResult();
            c.Content = CmsData.Contact.AddContact(id).ToString();
            return c;
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddTasks(int id)
        {
            var c = new ContentResult();
            c.Content = Task.AddTasks(id).ToString();
            return c;
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult VerifyAddress(string Address1, string Address2, string City, string State, string Zip)
        {
            var r = AddressVerify.LookupAddress(Address1, Address2, City, State, Zip);
            var ret = Json(r);
            return ret;
        }
        [Authorize(Roles = "Admin")]
        public ActionResult UserDialog(int id)
        {
            var u = DbUtil.Db.Users.Single(us => us.UserId == id);
            return View(u);
        }
        [Authorize(Roles = "Admin")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UserUpdate(int id, string username, string password2, bool islockedout, string[] role)
        {
            var u = DbUtil.Db.Users.Single(us => us.UserId == id);
            u.Username = username;
            u.IsLockedOut = islockedout;
            u.SetRoles(DbUtil.Db, role, User.IsInRole("Finance"));
            if (password2.HasValue())
                u.ChangePassword(password2);
            DbUtil.Db.SubmitChanges();
            return Content("ok");
        }
        [Authorize(Roles = "Admin")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UserDelete(int id)
        {
            var Db = DbUtil.Db;
            Db.PurgeUser(id);
            return Content("ok");
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UserInfoGrid(int id)
        {
            var p = DbUtil.Db.LoadPersonById(id);
            return View(p);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PeopleExtrasGrid(int id)
        {
            var p = DbUtil.Db.LoadPersonById(id);
            return View(p);
        }
        [AcceptVerbs(HttpVerbs.Post)]
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
        private class TagData
        {
            public int id { get; set; }
            public int? pid { get; set; }
            public string tagname { get; set; }
            public string cachename { get; set; }
        }
        public ActionResult TagDuplicates(int id)
        {
            if (HttpContext.Application["TagDuplicatesStatus"] != null)
                return Content("already running elsewhere, sorry");
            int? pid = Util.UserPeopleId;
            string tagname = Util2.CurrentTagName;
            var td = new TagData { id = id, pid = pid, tagname = tagname, cachename = "TagDuplicatesStatus_" + Util.Host };
            HttpRuntime.Cache[td.cachename] = new TagDuplicatesStatus();

            var t = new Thread(new ParameterizedThreadStart(TagDupsWorker));
            t.Start(td);
            Thread.Sleep(1000);
            return RedirectToAction("TagDuplicatesProgress");
        }
        private void TagDupsWorker(object tagdata)
        {
            var td = tagdata as TagData;
            var st = DateTime.Now;
            int nf = 0, np = 0;

            var status = HttpRuntime.Cache[td.cachename] as TagDuplicatesStatus;
            try
            {
                var db = DbUtil.Db;
                var q = db.PeopleQuery(td.id);
                var tag = db.FetchOrCreateTag(td.tagname, td.pid, DbUtil.TagTypeId_Personal);
                foreach (var p in q)
                {
                    if (p.PossibleDuplicates().Count() > 0)
                    {
                        var tp = db.TagPeople.SingleOrDefault(t => t.Id == tag.Id && t.PeopleId == p.PeopleId);
                        if (tp == null)
                            tag.PersonTags.Add(new TagPerson { PeopleId = p.PeopleId });
                        ++nf;
                        db.SubmitChanges();
                    }
                    ++np;
                    var ts = DateTime.Now.Subtract(st);
                    var dt = new DateTime(ts.Ticks);
                    var tsp = "{0:s.ff}".Fmt(new DateTime(Convert.ToInt64(ts.Ticks / np)));
                    var tt = "{0:mm:ss}".Fmt(dt);
                    status.SetStatus(np, nf, tsp, tt);
                }
            }
            finally
            {
                HttpRuntime.Cache.Remove(td.cachename);
            }
        }
        public ActionResult TagDuplicatesProgress()
        {
            var status = HttpRuntime.Cache["TagDuplicatesStatus_" + Util.Host] as TagDuplicatesStatus;
            if (status == null)
                return Redirect("/Tags");
            return View(status);
        }
    }
    public class TagDuplicatesStatus
    {
        public int found { get; set; }
        public int processed { get; set; }
        public string speed { get; set; }
        public string time { get; set; }
        public bool finished { get; set; }
        public bool isrunning { get; set; }

        public void SetStatus(int np, int nf, string ts, string tt)
        {
            found = nf;
            processed = np;
            speed = ts;
            time = tt;
        }
    }
}
