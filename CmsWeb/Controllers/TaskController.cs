/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using UtilityExtensions;
using System.Web.Routing;
using CMSWeb;
using CMSWeb.Models;
using CmsData;

namespace CMSWeb.Controllers
{
    public class TaskController : CMSWebCommon.Controllers.CmsController
    {
        public TaskController()
        {
            ViewData["Title"] = "Tasks";
        }
        public ActionResult List(int? id)
        {
            var tasks = new TaskModel();
            UpdateModel<ITaskFormBindable>(tasks);
            DbUtil.LogActivity("Tasks");
            if (id.HasValue && id == 0)
                return PartialView("Rows", tasks);
            return View(tasks);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Priority(int id, int priority)
        {
            var tasks = new TaskModel { Id = id };
            tasks.SetPriority(priority);
            return Json(new { Priority = id == 0 ? "" : priority.ToString() });
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SetComplete(int id)
        {
            var tasks = new TaskModel { Id = id };
            tasks.CompleteTask(id, new Emailer());
            return PartialView("Columns", tasks.FetchTask(id));
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Accept(int id)
        {
            var tasks = new TaskModel { Id = id };
            tasks.AcceptTask(id, new Emailer());
            return PartialView("Detail", tasks.FetchTask(id));
        }
        public ActionResult Detail(int id, int? rowid)
        {
            var tasks = new TaskModel();
            if (rowid.HasValue)
            {
                ViewData.Add("detailid", id);
                ViewData.Add("rowid", rowid);
                return PartialView("Detail2", tasks);
            }
            return PartialView("Detail", tasks.FetchTask(id));
        }
        public ActionResult Columns(int id)
        {
            var tasks = new TaskModel();
            return PartialView(tasks.FetchTask(id));
        }
        public ActionResult Row(int id)
        {
            var tasks = new TaskModel();
            return PartialView(tasks.FetchTask(id));
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddTask(string CurTab, string TaskDesc)
        {
            var model = new TaskModel { CurTab = CurTab };
            var listid = model.CurListId;
            if (listid == 0)
            {
                listid = TaskModel.InBoxId(model.PeopleId);
                var c = new HttpCookie("tasktab", model.CurTab);
                c.Expires = DateTime.Now.AddDays(360);
                Response.Cookies.Add(c);
            }
            var tid = model.AddTask(model.PeopleId, listid, TaskDesc);
            return PartialView("Row", model.FetchTask(tid));
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddList(string ListName)
        {
            var model = new TaskModel();
            model.AddList(ListName);
            return View("TabsOptions", model);
        }
        public ActionResult SearchContact(int? id)
        {
            var m = new SearchContactModel();
            UpdateModel<ISearchContactFormBindable>(m);
            if (id.HasValue)
            {
                m.Page = id;
                return PartialView("SearchContactRows", m);
            }
            return PartialView(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddSourceContact(int id, int contactid)
        {
            var tasks = new TaskModel();
            tasks.AddSourceContact(id, contactid);
            return PartialView("Detail", tasks.FetchTask(id));
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult CompleteWithContact(int id)
        {
            var tasks = new TaskModel();
            var contactid = tasks.AddCompletedContact(id, new Emailer());
            return Json(new { ContactId = contactid });
        }
        public ActionResult SearchPeople(int? id)
        {
            var m = new CMSWebCommon.Models.SearchPeopleModel();
            UpdateModel(m);
            if (id.HasValue)
            {
                m.Page = id;
                return PartialView("SearchPeopleRows", m);
            }
            return PartialView(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ChangeOwner(int id, int peopleid)
        {
            var tasks = new TaskModel();
            tasks.ChangeOwner(id, peopleid, new Emailer());
            return PartialView("Detail", tasks.FetchTask(id));
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delegate(int id, int peopleid)
        {
            var tasks = new TaskModel();
            tasks.Delegate(id, peopleid, new Emailer());
            return PartialView("Detail", tasks.FetchTask(id));
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ChangeAbout(int id, int peopleid)
        {
            TaskModel.SetWhoId(id, peopleid);
            var tasks = new TaskModel();
            return PartialView("Detail", tasks.FetchTask(id));
        }
        public ActionResult Edit(int id)
        {
            var m = new TaskModel();
            return PartialView(m.FetchTask(id));
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update(int id)
        {
            var m = new TaskModel();
            var t = m.FetchTask(id);
            UpdateModel(t);
            t.UpdateTask(new Emailer());
            t = m.FetchTask(id);
            return View("Detail", t);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Action(int? id, string option, string items, string curtab)
        {
            var tasks = new TaskModel();
            tasks.CurTab = curtab;
            var a = items.SplitStr(",").Select(i => i.ToInt());

            if (option.StartsWith("M"))
            {
                var ToTab = option.Substring(1).ToInt();
                if (curtab == "t" + ToTab)
                    return new EmptyResult();
                tasks.MoveTasksToList(a, ToTab);
            }
            else if (option == "deletelist")
            {
                tasks.DeleteList(curtab);
                return PartialView("TabsOptionsRows", tasks);
            }
            else if (option == "delete")
                tasks.DeleteTasks(a, new Emailer());
            else if (option.StartsWith("P"))
                tasks.Priortize(a, option);
            else if (option == "archive")
                tasks.ArchiveTasks(a, new Emailer());

            return PartialView("Rows", tasks);
        }
    }
}
