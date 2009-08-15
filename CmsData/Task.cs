using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            CreatedOn = DateTime.Now;
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
        public static void AddNewPerson(int coownerid, int newpersonid)
        {
            var Db = DbUtil.Db;
            var task = new Task
            {
                ListId = Task.GetRequiredTaskList("InBox", DbUtil.NewPeopleManagerId).Id,
                OwnerId = DbUtil.NewPeopleManagerId,
                Description = "New Person Data Entry",
                CoOwnerId = coownerid,
                CoListId = Task.GetRequiredTaskList("InBox", coownerid).Id,
                WhoId = newpersonid,
                StatusId = (int)Task.StatusCode.Active,
            };
            Db.Tasks.InsertOnSubmit(task);
            Db.SubmitChanges();
        }
    }
}
