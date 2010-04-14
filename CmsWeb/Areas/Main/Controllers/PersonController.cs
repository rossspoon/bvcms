using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;
using UtilityExtensions;
using System.Text;
using CMSWeb.Models.PersonPage;
using CMSWeb.Models;
using System.Diagnostics;

namespace CMSWeb.Areas.Main.Controllers
{
    public class PersonController : CmsStaffController
    {
        public ActionResult Index(int? id)
        {
            if (!id.HasValue)
                return Content("no person");
            var m = new PersonModel(id);
            if (m == null)
                return Content("no person");
            if (m.displayperson == null)
                return Content("person not found");
            if (Util.OrgMembersOnly && !DbUtil.Db.OrgMembersOnlyTag.People().Any(p => p.PeopleId == id.Value))
            {
                DbUtil.LogActivity("Trying to view person: {0}".Fmt(m.displayperson.Name));
                return Content("<h3 style='color:red'>{0}</h3>\n<a href='{1}'>{2}</a>"
                    .Fmt("You must be a member one of this person's organizations to have access to this page",
                    "javascript: history.go(-1)", "Go Back"));
            }
            Util.CurrentPeopleId = id.Value;
            Session["ActivePerson"] = m.displayperson.Name;
            DbUtil.LogActivity("Viewing Person: {0}".Fmt(m.displayperson.Name));
            InitExportToolbar(id);
            return View(m);
        }
        [Authorize(Roles="Admin")]
        public ActionResult Move(int id, int to)
        {
            var p = DbUtil.Db.People.Single(pp => pp.PeopleId == id);
            try
            {
                p.MovePersonStuff(to);
                DbUtil.Db.SubmitChanges();
            }
            catch
            {
                return Content("error");
            }
            return new EmptyResult();
        }
        [Authorize(Roles="Admin")]
        public ActionResult Delete(int id)
        {
            Util.Auditing = false;
            var person = DbUtil.Db.LoadPersonById(id);
            if (person == null)
                return Content("error, bad peopleid");
            if (!person.PurgePerson())
                return Content("error, not deleted");
            Util.CurrentPeopleId = 0;
            Session.Remove("ActivePerson");
            return new EmptyResult();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Tag(int id)
        {
            Person.Tag(id, Util.CurrentTagName, Util.CurrentTagOwnerId, DbUtil.TagTypeId_Personal);
            DbUtil.Db.SubmitChanges();
            return new EmptyResult();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UnTag(int id)
        {
            Person.UnTag(id, Util.CurrentTagName, Util.CurrentTagOwnerId, DbUtil.TagTypeId_Personal);
            DbUtil.Db.SubmitChanges();
            return new EmptyResult();
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
            var c = new NewContact
            {
                CreatedDate = Util.Now,
                CreatedBy = Util.UserId1,
                ContactDate = Util.Now.Date,
                ContactTypeId = 99,
                ContactReasonId = 99,
            };

            DbUtil.Db.NewContacts.InsertOnSubmit(c);
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
            var c = new NewContact
            {
                CreatedDate = Util.Now,
                CreatedBy = Util.UserId1,
                ContactDate = Util.Now.Date,
                ContactTypeId = 99,
                ContactReasonId = 99,
            };

            DbUtil.Db.NewContacts.InsertOnSubmit(c);
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
            var active = (int)Task.StatusCode.Active;
            var t = new Task
            {
                OwnerId = pid,
                Description = "NewTask",
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
            m.UpdateAddress();
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
            m.UpdateMember();
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
        public JsonResult VerifyAddress(string Address1, string Address2, string City, string State, string Zip)
        {
            var r = CMSPresenter.PersonController.LookupAddress(Address1, Address2, City, State, Zip);
            return Json(r);
        }
        private void InitExportToolbar(int? id)
        {
            var qb = DbUtil.Db.QueryBuilderIsCurrentPerson();
            ViewData["queryid"] = qb.QueryId;
            ViewData["TagAction"] = "/Person/Tag/" + id;
            ViewData["UnTagAction"] = "/Person/UnTag/" + id;
        }
    }
}
