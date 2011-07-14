using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilityExtensions;

namespace CmsData
{
    public partial class Task
    {
        public enum StatusCode
        {
            Active = 10,
            Waiting = 20,
            Someday = 30,
            Complete = 40,
            Pending = 50,
            Redelegated = 60,
        }
        public StatusCode StatusEnum
        {
            get { return (StatusCode)StatusId; }
        }
        public string AboutName { get { return AboutWho == null ? "" : AboutWho.Name; } }
        partial void OnCreated()
        {
            CreatedOn = Util.Now;
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
        public static void AddNewPerson(int newpersonid)
        {
            var Db = DbUtil.Db;
            var NewPeopleManagerId = Db.NewPeopleManagerId;
            var task = new Task
            {
                ListId = Task.GetRequiredTaskList("InBox", NewPeopleManagerId).Id,
                OwnerId = NewPeopleManagerId,
                Description = "New Person Data Entry",
                WhoId = newpersonid,
                StatusId = (int)Task.StatusCode.Active,
            };
            if (Util.UserPeopleId.HasValue && Util.UserPeopleId.Value != NewPeopleManagerId)
            {
                task.CoOwnerId = Util.UserPeopleId.Value;
                task.CoListId = Task.GetRequiredTaskList("InBox", Util.UserPeopleId.Value).Id;
            }
            Db.Tasks.InsertOnSubmit(task);
            Db.SubmitChanges();
        }
        public static int AddTasks(int qid)
        {
            var q = DbUtil.Db.PeopleQuery(qid);
            int qCount = q.Count();
            if (qCount > 100)
                return qCount;
            foreach (var p in q)
            {
                var t = new Task
                {
                    ListId = Task.GetRequiredTaskList("InBox", Util.UserPeopleId.Value).Id,
                    OwnerId = Util.UserPeopleId.Value,
                    Description = "Please Contact",
                    ForceCompleteWContact = true,
                    StatusId = (int)Task.StatusCode.Active,
                };
                p.TasksAboutPerson.Add(t);
            }
            DbUtil.Db.SubmitChanges();
            return qCount;
        }

    }
}
