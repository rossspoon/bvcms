using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;
using UtilityExtensions;
using System.Text;

namespace CMSWeb.Areas.Main.Controllers
{
    public class PersonController : Controller
    {
        public ActionResult Index(int? id)
        {
            if (!id.HasValue)
                return Content("no person");
            var m = new Models.PersonModel(id);
            if (m.displayperson == null)
                return Content("person not found");
            Util.CurrentPeopleId = id.Value;
            Session["ActivePerson"] = m.displayperson.Name;
            var qb = DbUtil.Db.QueryBuilderIsCurrentPerson();
            ViewData["queryid"] = qb.QueryId;
            ViewData["TagAction"] = "/Person/Tag/" + id;
            ViewData["UnTagAction"] = "/Person/UnTag/" + id;
            return View(m);
        }
        public ActionResult Move(int? id, int to)
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
            var m = new Models.PersonEnrollmentsModel(id);
            UpdateModel(m.Pager);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PrevEnrollGrid(int id)
        {
            var m = new Models.PersonPrevEnrollmentsModel(id);
            UpdateModel(m.Pager);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PendingEnrollGrid(int id)
        {
            var m = new Models.PersonPendingEnrollmentsModel(id);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AttendanceGrid(int id)
        {
            var m = new Models.PersonAttendHistoryModel(id);
            UpdateModel(m.Pager);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ContactsMadeGrid(int id)
        {
            var m = new Models.PersonContactsMadeModel(id);
            UpdateModel(m.Pager);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ContactsReceivedGrid(int id)
        {
            var m = new Models.PersonContactsReceivedModel(id);
            UpdateModel(m.Pager);
            return View(m);
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
                ListId = Models.TaskModel.InBoxId(pid),
                StatusId = active,
            };
            p.TasksAboutPerson.Add(t);
            DbUtil.Db.SubmitChanges();
            return Content("/Task/List/{0}".Fmt(t.Id));
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult BasicDisplay(int id)
        {
            var m = Models.BasicPersonInfo.GetBasicPersonInfo(id);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult BasicEdit(int id)
        {
            var m = Models.BasicPersonInfo.GetBasicPersonInfo(id);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult BasicUpdate(int id)
        {
            var m = Models.BasicPersonInfo.GetBasicPersonInfo(id);
            UpdateModel(m);
            m.UpdatePerson();
            m = Models.BasicPersonInfo.GetBasicPersonInfo(id);
            return View("BasicDisplay", m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult BusinessCard(int id)
        {
            var m = new Models.PersonModel(id);
            return View(m.displayperson);
        }
        public ContentResult Schools(string q, int limit)
        {
            var qu = from p in DbUtil.Db.People
                     where p.SchoolOther.Contains(q)
                     group p by p.SchoolOther into g
                     select g.Key;
            var sb = new StringBuilder();
            foreach (var li in qu.Take(limit))
                sb.AppendLine(li);
            return Content(sb.ToString());
        }
        public ContentResult Employers(string q, int limit)
        {
            var qu = from p in DbUtil.Db.People
                     where p.EmployerOther.Contains(q)
                     group p by p.EmployerOther into g
                     select g.Key;
            var sb = new StringBuilder();
            foreach (var li in qu.Take(limit))
                sb.AppendLine(li);
            return Content(sb.ToString());
        }
        public ContentResult Occupations(string q, int limit)
        {
            var qu = from p in DbUtil.Db.People
                     where p.OccupationOther.Contains(q)
                     group p by p.OccupationOther into g
                     select g.Key;
            var sb = new StringBuilder();
            foreach (var li in qu.Take(limit))
                sb.AppendLine(li);
            return Content(sb.ToString());
        }
    }
}
