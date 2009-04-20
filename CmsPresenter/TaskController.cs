/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using UtilityExtensions;
using CMSModel;
using System.ComponentModel;
using System.Collections;
using System.Text;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;

namespace CMSPresenter
{
    [DataObject]
    public class TaskController
    {
        private const string STR_InBox = "InBox";
        private CMSDataContext Db = DbUtil.Db;
        private int pid;

        public TaskController()
        {
            pid = Util.UserPeopleId.Value;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable FetchTaskLists()
        {
            GetRequiredTaskList(STR_InBox, pid);
            GetRequiredTaskList("Personal", pid);
            return from t in Db.TaskLists
                   where t.TaskListOwners.Any(tlo => tlo.PeopleId == pid) || t.CreatedBy == pid
                   orderby t.Name
                   select new
                   {
                       Id = "t" + t.Id,
                       t.Name,
                   };
        }
        class ActionOption
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }
        ActionOption[] actions =
        {
            new ActionOption { Value = "0", Name = "Actions" },
            new ActionOption { Value = "0", Name = "Tasks..." },
            new ActionOption { Value = "delegate", Name = ".. Delegate Task" },
            new ActionOption { Value = "archive", Name = ".. Archive Task" },
            new ActionOption { Value = "delete", Name = ".. Delete Task" },
            new ActionOption { Value = "0", Name = "Set Priority..." },
            new ActionOption { Value = "P1", Name = ".. 1" },
            new ActionOption { Value = "P2", Name = ".. 2" },
            new ActionOption { Value = "P3", Name = ".. 3" },
            new ActionOption { Value = "P0", Name = ".. None" },
            new ActionOption { Value = "0", Name = "List..." },
            new ActionOption { Value = "sharelist", Name = ".. Share List" },
            new ActionOption { Value = "deletelist", Name = ".. Delete List" },
            new ActionOption { Value = "0", Name = "Move To List..." },
        };
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable FetchTaskLists2()
        {
            var q = from t in Db.TaskLists
                    where t.TaskListOwners.Any(tlo => tlo.PeopleId == pid) || t.CreatedBy == pid
                    orderby t.Name
                    select new ActionOption
                    {
                        Name = ".. " + t.Name,
                        Value = "M" + t.Id,
                    };
            return actions.Union(q);
        }
        public static TaskList GetRequiredTaskList(string name, int pid)
        {
            var Db = DbUtil.Db;
            var list = Db.TaskLists.SingleOrDefault(tl => tl.CreatedBy == pid && tl.Name == name);
            if (list == null)
            {
                list = new TaskList { CreatedBy = pid, Name = name };
                Db.TaskLists.InsertOnSubmit(list);
                Db.SubmitChanges();
            }
            return list;
        }
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public IEnumerable<TaskInfo> FetchTasks(string sortExpression, string curtab,
            string project, string location, int? status, bool ownerOnly, bool noCompleted)
        {
            var q = Db.Tasks.Where(t => t.Archive == false);
            if (ownerOnly) // I see only my own tasks or tasks I have been delegated
                q = q.Where(t => t.OwnerId == pid || t.CoOwnerId == pid);
            else // I see my own tasks where I am owner or cowner plus other people's tasks where I share the list the task is in
                q = q.Where(t => t.OwnerId == pid || t.CoOwnerId == pid
                    || t.TaskList.TaskListOwners.Any(tlo => tlo.PeopleId == pid)
                    || t.CoTaskList.TaskListOwners.Any(tlo => tlo.PeopleId == pid)
                    || t.CoTaskList.CreatedBy == pid || t.TaskList.CreatedBy == pid);

            var completedcode = (int)Task.StatusCode.Complete;
            var somedaycode = (int)Task.StatusCode.Someday;
            if (noCompleted)
                q = q.Where(t => t.StatusId != completedcode);

            q = ApplySearch(q, project, location, status);

            var iPhone = HttpContext.Current.Request.UserAgent.Contains("iPhone");
            var listid = curtab.Substring(1).ToInt();
            var q2 = from t in q
                     let tListId = t.CoOwnerId == pid ? t.CoListId.Value : t.ListId
                     select new TaskInfo
                     {
                         Id = t.Id,
                         Owner = t.Owner.Name,
                         OwnerId = t.OwnerId,
                         OwnerEmail = t.Owner.EmailAddress,
                         WhoId = t.WhoId,
                         ListId = tListId,
                         cListId = listid,
                         Description = t.Description,
                         SortDue = t.Due ?? DateTime.MaxValue.Date,
                         SortDueOrCompleted = t.CompletedOn?? (t.Due ?? DateTime.MaxValue.Date),
                         CoOwner = t.CoOwner.Name,
                         CoOwnerId = t.CoOwnerId,
                         CoOwnerEmail = t.CoOwner.EmailAddress,
                         Status = t.TaskStatus.Description,
                         StatusId = t.StatusId.Value,
                         Location = t.Location,
                         Project = t.Project,
                         Completed = t.StatusId == completedcode,
                         PrimarySort = t.StatusId == completedcode ? 3 : (t.StatusId == somedaycode ? 2 : 1),
                         SortPriority = t.Priority ?? 4,
                         IsCoOwner = t.CoOwnerId != null && t.CoOwnerId == pid,
                         IsOwner = t.OwnerId == pid,
                         SourceContactId = t.SourceContactId,
                         SourceContact = t.SourceContact.ContactDate,
                         CompletedContactId = t.CompletedContactId,
                         CompletedContact = t.CompletedContact.ContactDate,
                         Notes = t.Notes,
                         CreatedOn = t.CreatedOn,
                         CompletedOn = t.CompletedOn,
                         NotiPhone = !iPhone,
                     };
            switch (sortExpression)
            {
                case "123":
                case "123 DESC":
                    q2 = from t in q2
                         orderby t.PrimarySort, t.SortPriority, t.SortDue, t.Description
                         select t;
                    break;
                case "Task":
                case "Task DESC":
                    q2 = from t in q2
                         orderby t.Description
                         select t;
                    break;
                case "DueOrCompleted":
                case "Due DESC":
                default:
                    q2 = from t in q2
                         orderby t.PrimarySort, t.SortDueOrCompleted, t.SortPriority, t.Description
                         select t;
                    break;
            }
            return q2;
        }
        public class ContactTaskInfo
        {
            public int Id { get; set; }
            public string Who { get; set; }
            public string Description { get; set; }
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<ContactTaskInfo> FetchContactTasks()
        {
            var completedcode = (int)Task.StatusCode.Complete;
            var q = from t in Db.Tasks
                    // not archived
                    where t.Archive == false // not archived
                    // I am involved in
                    where t.OwnerId == pid || t.CoOwnerId == pid
                    // only contact related and not completed
                    where t.WhoId != null && t.StatusId != completedcode
                    // filter out any I own and have delegated
                    where !(t.OwnerId == pid && t.CoOwnerId != null)
                    orderby t.CreatedOn
                    select new ContactTaskInfo
                    {
                        Id = t.Id,
                        Who = t.AboutWho.Name,
                        Description = t.Description,
                    };
            return q;
        }
        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public void UpdateTask(string Description, string Due, string Location, string Notes, int Priority, string Project, int StatusId, int Id, ITaskNotify notify)
        {
            var sb = new StringBuilder();
            var task = Db.Tasks.Single(t => t.Id == Id);
            ChangeTask(sb, task, "Description", Description);
            ChangeTask(sb, task, "Due", Due);
            ChangeTask(sb, task, "Notes", Notes);
            ChangeTask(sb, task, "StatusId", StatusId);
            if (HttpContext.Current.User.IsInRole("AdvancedTask"))
                ChangeTask(sb, task, "Project", Project);

            task.Location = Location;
            if (Priority == 0)
                task.Priority = null;
            else
                task.Priority = Priority;
            NotifyIfNeeded(notify, sb, task);
            Db.SubmitChanges();
        }

        private void NotifyIfNeeded(ITaskNotify notify, StringBuilder sb, Task task)
        {
            if (sb.Length > 0 && task.CoOwnerId.HasValue)
            {
                var from = pid == task.OwnerId ? task.Owner : task.CoOwner;
                var to = from.PeopleId == task.OwnerId ? task.CoOwner : task.Owner;
                var req = HttpContext.Current.Request;
                notify.EmailNotification(from, to,
                            "Task Updated by " + from.Name,
                            "{0}<br />\n{1}<br />\n{2}".Fmt(
                            TaskLink(task.Description, task.Id), task.AboutName, sb.ToString()));
            }
        }
        private static string TaskLink(string text, int id)
        {
            //var req = HttpContext.Current.Request;
            //var url = Util.DefaultHost + req.ApplicationPath + "/Tasks.aspx";
            //var m = Regex.Match(url, "(https?://)?(.*)", RegexOptions.Singleline);
            //var h = m.Groups[1].Value;
            //var p = Regex.Replace(m.Groups[2].Value, "/+", "/");
            //if (!h.HasValue())
            //    h = "https://";
            return "<a href='https://cms.bellevue.org/Tasks.aspx?id={0}#select'>{1}</a>".Fmt(id, text);
        }

        private void ChangeTask(StringBuilder sb, Task task, string field, object value)
        {
            switch (field)
            {
                case "Due":
                    {
                        DateTime dt;
                        if (DateTime.TryParse((string)value, out  dt))
                        {
                            if ((task.Due.HasValue && task.Due.Value != dt) || !task.Due.HasValue)
                                sb.AppendFormat("Due changed from {0:d} to {1:d}<br />\n", task.Due, dt);
                            task.Due = dt;
                        }
                        else
                        {
                            if (task.Due.HasValue)
                                sb.AppendFormat("Due changed from {0:d} to null<br />\n", task.Due);
                            task.Due = null;
                        }
                    }
                    break;
                case "Notes":
                    if (task.Notes != (string)value)
                        sb.AppendFormat("Notes changed: {{<br />\n{0}<br />}}<br />\n", Util.SafeFormat((string)value));
                    task.Notes = (string)value;
                    break;
                case "StatusId":
                    if (task.StatusId != (int)value)
                    {
                        sb.AppendFormat("Status changed from {0:g} to {1:g}<br />\n", task.StatusEnum, (Task.StatusCode)value);
                        if ((int)value == (int)Task.StatusCode.Complete)
                            task.CompletedOn = Util.Now;
                        else
                            task.CompletedOn = null;
                    }
                    task.StatusId = (int)value;
                    break;
                case "Description":
                    if (task.Description != (string)value)
                        sb.AppendFormat("Description changed from \"{0}\" to \"{1}\"<br />\n", task.Description, value);
                    task.Description = (string)value;
                    break;
                case "Project":
                    if (task.Project != (string)value)
                        sb.AppendFormat("Project changed from \"{0}\" to \"{1}\"<br />\n", task.Project, value);
                    task.Project = (string)value;
                    break;
                default:
                    throw new ArgumentException("Invalid field in ChangeTask", field);
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public void DeleteTask(int TaskId, ITaskNotify notify)
        {
            var task = Db.Tasks.SingleOrDefault(t => t.Id == TaskId);
            if (task == null)
                return;

            if (task.OwnerId == pid)
            {
                if (task.CoOwnerId != null)
                    notify.EmailNotification(task.Owner, task.CoOwner,
                        "Task Deleted by " + task.Owner.Name,
                        task.Description + "<br/>\n" + task.AboutName);
                Db.Tasks.DeleteOnSubmit(task);
                Db.SubmitChanges();
            }
            else // I must be cowner, I can't delete
            {
                notify.EmailNotification(task.CoOwner, task.Owner,
                    task.CoOwner.Name + " tried to delete task",
                    TaskLink(task.Description, task.Id) + "<br/>\n" + task.AboutName);
            }
        }
        private IQueryable<Task> ApplySearch(IQueryable<Task> query, string project, string location, int? status)
        {
            var q = query.Select(t => t);
            if (project != "0")
                q = from t in q
                    where t.Project.Contains(project)
                    select t;
            if (location != "0")
                q = from t in q
                    where t.Location.Contains(location)
                    select t;
            if (status.HasValue && status != 0)
                q = from t in q
                    where t.StatusId == status.Value
                    select t;
            return q;
        }
        public IEnumerable<string> Locations()
        {
            string[] a = { "@work", "@home", "@car", "@computer" };
            var q = from t in Db.Tasks
                    where t.OwnerId == pid
                    where t.Location != ""
                    orderby t.Location
                    select t.Location;
            return a.Union(q.Distinct()).Distinct(StringComparer.OrdinalIgnoreCase);
        }
        private IQueryable<string> projects()
        {
            return from t in Db.Tasks
                   where t.Archive == false
                   where t.TaskList.TaskListOwners.Any(tlo => tlo.PeopleId == pid) || t.TaskList.CreatedBy == pid
                   where t.Project != ""
                   orderby t.Project
                   select t.Project;
        }
        public IEnumerable<string> Projects()
        {
            var q = projects();
            return q.Distinct().ToList();
        }
        public IEnumerable<string> Projects(string startswith)
        {
            var q = projects().Where(p => p.StartsWith(startswith));
            return q.Distinct().ToList();
        }

        public static string JsonStatusCodes()
        {
            var cv = new CodeValueController();
            var sb = new StringBuilder("{");
            foreach (var c in cv.TaskStatusCodes())
            {
                if (sb.Length > 1)
                    sb.Append(",");
                sb.AppendFormat("'{0}':'{1}'", c.Value, c.Value);
            }
            sb.Append("}");
            return sb.ToString();
        }
        public int AddCompletedContact(int id, ITaskNotify notify)
        {
            var task = Db.Tasks.SingleOrDefault(t => t.Id == id);
            var c = new NewContact { ContactDate = Util.Now.Date };
            c.CreatedDate = c.ContactDate;
            c.ContactTypeId = 7;
            c.ContactReasonId = 160;
            var min = Db.Ministries.SingleOrDefault(m => m.MinistryName == task.Project);
            if (min != null)
                c.MinistryId = min.MinistryId;
            c.contactees.Add(new Contactee { PeopleId = task.WhoId.Value });
            c.contactsMakers.Add(new Contactor { PeopleId = pid });
            task.CompletedContact = c;
            task.StatusEnum = Task.StatusCode.Complete;
            if (task.CoOwnerId == pid)
                notify.EmailNotification(task.CoOwner, task.Owner,
                        "Task Completed with a Contact by " + task.CoOwner.Name,
                        TaskLink(task.Description, task.Id) + "<br />" + task.AboutName);
            else if (task.CoOwnerId != null)
                notify.EmailNotification(task.Owner, task.CoOwner,
                    "Task Completed with a Contact by " + task.Owner.Name,
                    TaskLink(task.Description, task.Id) + "<br />" + task.AboutName);
            task.CompletedOn = c.ContactDate;
            Db.SubmitChanges();
            return c.ContactId;
        }

        public void AcceptTask(int id, ITaskNotify notify)
        {
            var task = Db.Tasks.SingleOrDefault(t => t.Id == id);
            task.StatusEnum = Task.StatusCode.Active;
            Db.SubmitChanges();
            notify.EmailNotification(task.CoOwner, task.Owner,
                "Task Accepted from " + task.CoOwner.Name,
                TaskLink(task.Description, task.Id) + "<br />" + task.AboutName);
        }
        public void AddSourceContact(int id, int contactid)
        {
            var task = Db.Tasks.Single(t => t.Id == id);
            task.SourceContact = Db.NewContacts.SingleOrDefault(nc => nc.ContactId == contactid);
            Db.SubmitChanges();
        }
        public static void Delegate(int taskid, int toid, ITaskNotify notify)
        {
            if (toid == Util.UserPeopleId.Value)
                return; // cannot delegate to self
            var task = DbUtil.Db.Tasks.Single(t => t.Id == taskid);
            task.StatusEnum = Task.StatusCode.Pending;
            task.CoOwnerId = toid;

            // if the owner's list is shared by the coowner
            // then put it in owner's list
            // otherwise put it in the coowner's inbox
            if (task.TaskList.TaskListOwners.Any(tlo => tlo.PeopleId == toid) || task.TaskList.CreatedBy == toid)
                task.CoListId = task.ListId;
            else
                task.CoListId = InBoxId(toid);

            DbUtil.Db.SubmitChanges();
            notify.EmailNotification(task.Owner, DbUtil.Db.LoadPersonById(toid),
                "New Task from " + task.Owner.Name,
                TaskLink(task.Description, taskid) + "<br/>" + task.AboutName);
        }
        public static void ChangeOwner(int taskid, int toid, ITaskNotify notify)
        {
            if (toid == Util.UserPeopleId.Value)
                return; // nothing to do
            var task = DbUtil.Db.Tasks.Single(t => t.Id == taskid);

            // if the owner's list is shared by the coowner
            // then put it in owner's list
            // otherwise put it in the coowner's inbox
            var owner = task.Owner;
            var toowner = DbUtil.Db.LoadPersonById(toid);
            if (task.TaskList.TaskListOwners.Any(tlo => tlo.PeopleId == toid) || task.TaskList.CreatedBy == toid)
                task.CoListId = task.ListId;
            else
                task.ListId = InBoxId(toid);

            task.CoOwnerId = task.OwnerId;
            task.Owner = toowner;

            DbUtil.Db.SubmitChanges();
            notify.EmailNotification(owner, toowner,
                "Task transferred from " + owner.Name,
                TaskLink(task.Description, taskid));
        }

        public static void SetWhoId(int id, int pid)
        {
            var task = DbUtil.Db.Tasks.Single(t => t.Id == id);
            task.WhoId = pid;
            DbUtil.Db.SubmitChanges();
        }

        public void SetDescription(int id, string value, ITaskNotify notify)
        {
            var task = Db.Tasks.Single(t => t.Id == id);
            var sb = new StringBuilder();
            ChangeTask(sb, task, "Description", value);
            NotifyIfNeeded(notify, sb, task);
            Db.SubmitChanges();
        }

        public void SetLocation(int id, string value)
        {
            var task = Db.Tasks.Single(t => t.Id == id);
            task.Location = value;
            Db.SubmitChanges();
        }

        public void SetStatus(int id, string value, ITaskNotify notify)
        {
            var task = Db.Tasks.Single(t => t.Id == id);
            var cvc = new CodeValueController();
            var ts = cvc.TaskStatusCodes();
            var statusid = ts.Single(t => t.Value == value).Id;
            var sb = new StringBuilder();
            ChangeTask(sb, task, "StatusId", statusid);
            NotifyIfNeeded(notify, sb, task);
            Db.SubmitChanges();
        }

        public void MoveTasksToList(List<int> tids, int listid)
        {
            var mlist = Db.TaskLists.Single(tl => tl.Id == listid);
            var q = from t in Db.Tasks
                    where tids.Contains(t.Id)
                    // if I am the coowner
                    // and if this task is on a shared list
                    // and that list is the same as my owner's list
                    // then don't move it
                    where !(t.CoOwnerId == pid && t.CoTaskList.TaskListOwners.Count() > 0 && t.CoListId == t.ListId)
                    select t;
            foreach (var t in q)
                if (t.OwnerId == pid && (mlist.TaskListOwners.Any(tlo => tlo.PeopleId == t.CoOwnerId) || mlist.CreatedBy == t.CoOwnerId))
                {
                    mlist.CoTasks.Add(t);
                    mlist.Tasks.Add(t);
                }
                else if (t.CoOwnerId == pid)
                    mlist.CoTasks.Add(t);
                else
                    mlist.Tasks.Add(t);
            Db.SubmitChanges();
        }
        public void DeleteList(string tab)
        {
            var id = tab.Substring(1).ToInt();
            if (id <= 0)
                return;
            var list = Db.TaskLists.Single(tl => tl.Id == id);
            if (list.Name == STR_InBox) // can't delete inbox
                return;
            var inbox = GetRequiredTaskList(STR_InBox, pid);
            var q = Db.Tasks.Where(t => t.ListId == id);
            foreach (var t in q)
            {
                if (t.CoOwnerId.HasValue && t.CoListId == id)
                {
                    var cinbox = GetRequiredTaskList(STR_InBox, t.CoOwnerId.Value);
                    cinbox.CoTasks.Add(t);
                }
                inbox.Tasks.Add(t);
            }
            Db.TaskLists.DeleteOnSubmit(list);
            Db.SubmitChanges();
        }

        public void AddList(string name)
        {
            var txt = name.ToLower();
            if (string.Compare(txt, STR_InBox, true) == 0 || string.Compare(txt, "personal", true) == 0)
                return;
            if (Db.TaskLists.Count(t => t.Name == name && t.CreatedBy == pid) > 0)
                return;
            var list = new TaskList { Name = name, CreatedBy = pid };
            Db.TaskLists.InsertOnSubmit(list);
            Db.SubmitChanges();
        }

        public static int InBoxId(int pid)
        {
            return GetRequiredTaskList(STR_InBox, pid).Id;
        }
        public void AddTask(int pid, int listid, string text)
        {
            if (listid <= 0)
                listid = InBoxId(pid);
            var task = new Task
            {
                ListId = listid,
                Description = text,
                OwnerId = pid,
                StatusEnum = Task.StatusCode.Active,
            };
            Db.Tasks.InsertOnSubmit(task);
            Db.SubmitChanges();
        }

        public void SetPriority(int id, int p)
        {
            var task = Db.Tasks.Single(t => t.Id == id);
            if (p == 0)
                task.Priority = null;
            else
                task.Priority = p;
            Db.SubmitChanges();
        }

        public void SetProject(int id, string value, ITaskNotify notify)
        {
            var task = Db.Tasks.Single(t => t.Id == id);
            var sb = new StringBuilder();
            ChangeTask(sb, task, "Project", value);
            NotifyIfNeeded(notify, sb, task);
            Db.SubmitChanges();
        }

        public void DeleteTasks(List<int> list, ITaskNotify notify)
        {
            foreach (var id in list)
                DeleteTask(id, notify);
        }

        public void Priortize(List<int> list, string p)
        {
            var q = from t in Db.Tasks
                    where list.Contains(t.Id)
                    select t;
            int? priority = p.Substring(1).ToInt();
            if (priority == 0)
                priority = null;
            foreach (var t in q)
                t.Priority = priority;
            Db.SubmitChanges();
        }

        public void CompleteTask(int id, ITaskNotify notify)
        {
            var task = Db.Tasks.Single(t => t.Id == id);
            var sb = new StringBuilder();
            var statusid = (int)Task.StatusCode.Complete;
            ChangeTask(sb, task, "StatusId", statusid);
            NotifyIfNeeded(notify, sb, task);
            Db.SubmitChanges();
        }

        public void ArchiveTask(int TaskId, ITaskNotify notify)
        {
            var task = Db.Tasks.Single(t => t.Id == TaskId);

            if (task.OwnerId == pid)
            {
                if (task.CoOwnerId != null)
                    notify.EmailNotification(task.Owner, task.CoOwner,
                        "Task Archived by " + task.Owner.Name,
                        task.Description + "<br/>\n" + task.AboutName);
                task.Archive = true;
                Db.SubmitChanges();
            }
            else // I must be cowner, I can't archive
            {
                notify.EmailNotification(task.CoOwner, task.Owner,
                    task.CoOwner.Name + " tried to archive task",
                    TaskLink(task.Description, task.Id) + "<br/>\n" + task.AboutName);
            }
        }

        public void ArchiveTasks(List<int> list, ITaskNotify notify)
        {
            foreach (var id in list)
                ArchiveTask(id, notify);
        }
        public static void AddNewPersonTask(int ownerid, int coownerid, int newpersonid, ITaskNotify notify)
        {
            var Db = DbUtil.Db;
            var task = new Task
            {
                ListId = InBoxId(ownerid),
                OwnerId = ownerid,
                Description = "New Person Data Entry",
                CoOwnerId = coownerid,
                CoListId = InBoxId(coownerid),
                WhoId = newpersonid,
                StatusEnum = Task.StatusCode.Active,
            };
            Db.Tasks.InsertOnSubmit(task);
            Db.SubmitChanges();
            //notify.EmailNotification(task.CoOwner, task.Owner,
            //     "New Person Added by " + task.CoOwner.Name,
            //     TaskLink(task.Description, task.Id) + "<br/>\n" + task.AboutName);
        }
        public static int MyListId(Task t)
        {
            var pid = Util.UserPeopleId.Value;
            if (t.CoOwnerId.HasValue && pid == t.CoOwnerId.Value)
                return t.CoListId.Value;
            else if (pid == t.OwnerId)
                return t.ListId;
            else 
                return 0;
        }
    }
}
