using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;
using UtilityExtensions;

namespace CMSWeb.Areas.Main.Controllers
{
    public class PersonController : Controller
    {
        //
        // GET: /Main/Person/

        public ActionResult Index(int? id)
        {
            if (!id.HasValue)
                return Content("no person");
            var m = new Models.PersonModel(id);
            if (m.person == null)
                return Content("person not found");
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
    }
}
