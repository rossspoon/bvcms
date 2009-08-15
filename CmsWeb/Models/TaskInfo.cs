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
using System.ComponentModel;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.Mvc;
using CMSPresenter;
using CmsData;

namespace CMSWeb.Models
{
    public class TaskInfo
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsSelected { get; set; }
        public string DispOwner
        {
            get
            {
                var pid = Util.UserPeopleId.Value;
                if (CoOwnerId == pid) // if task has been delegated to me
                    return ""; // display nothing
                else if (OwnerId == pid) // if I am owner
                    if (CoOwnerId.HasValue) // and task has been delegated
                        return CoOwner; // display delegate
                    else // otherwise
                        return ""; // display nothing
                else if (CoOwnerId.HasValue) // if this task has been delegated
                    return CoOwner; // display the delegate
                return Owner; // otherwise display the owner
            }
        }
        public string Owner { get; set; }
        public int OwnerId { get; set; }
        public string CoOwner { get; set; }
        public int? CoOwnerId { get; set; }
        internal Person who;
        public int? WhoId
        {
            get
            {
                if (who == null)
                    return null;
                return who.PeopleId;
            }
            set
            {
                if (value.HasValue)
                    who = DbUtil.Db.LoadPersonById(value.Value);
            }
        }
        public string Who
        {
            get { return who == null ? "" : who.Name; }
        }
        public int PrimarySort { get; set; }
        //public int ListId { get; set; }
        public DateTime? SortDue { get; set; }
        public DateTime? DueOrCompleted
        {
            get { return SortDueOrCompleted != DateTime.MaxValue.Date ? (DateTime?)SortDueOrCompleted : null; }
        }
        public DateTime SortDueOrCompleted { get; set; }
        public string Status { get; set; }
        public bool Completed { get; set; }
        public DateTime? CompletedOn { get; set; }
        public bool NotiPhone { get; set; }
        public int SortPriority { get; set; }
        public int? Priority
        {
            get { return SortPriority == 4 ? null : (int?)SortPriority; }
            set { SortPriority = value ?? 4; }
        }
        public bool IsAnOwner
        {
            get { return IsOwner || IsCoOwner; }
        }
        public bool IsOwner { get; set; }
        public bool IsCoOwner { get; set; }

        public TaskDetail GetDetail()
        {
            var m = new TaskModel();
            return m.FetchTask(Id);
        }
    }

    public class TaskDetail : TaskInfo
    {
        public string OwnerEmail { get; set; }
        public string TaskEmail
        {
            get
            {
                return "mailto:{0}?subject={1}&body={3}/Tasks.aspx?id={2}#select"
                .Fmt(OwnerEmail, Description, Id, DbUtil.TaskHost);
            }
        }
        public bool ForceCompleteWContact { get; set; }
        public string CoOwnerEmail { get; set; }
        public string WhoEmail
        {
            get { return who == null ? "" : "{0} <{1}>".Fmt(who.Name, who.EmailAddress); }
        }
        public string WhoEmail2
        {
            get
            {
                if (who != null && who.EmailAddress.HasValue())
                    return who.EmailAddress;
                return "no email";
            }
        }
        public string WhoAddress
        {
            get { return who == null ? "" : who.PrimaryAddress; }
        }
        public string WhoAddrCityStateZip
        {
            get { return who == null ? "" : who.AddrCityStateZip; }
        }
        public string WhoPhone
        {
            get { return who == null ? "" : who.HomePhone.FmtFone(); }
        }
        public DateTime CreatedOn { get; set; }
        public string ChangeWho
        {
            get { return AssignChange(WhoId); }
        }

        private string AssignChange(int? id)
        {
            return id == null ? "(assign)" : "(change)";
        }

        public DateTime? Due
        {
            get { return (SortDue.HasValue && SortDue != DateTime.MaxValue.Date) ? SortDue : null; }
            set { SortDue = value; }
        }
        public string ChangeCoOwner
        {
            get { return CoOwnerId == null ? "(delegate)" : "(redelegate)"; }
        }
        public int StatusId { get; set; }
        public Task.StatusCode StatusEnum
        {
            get { return (Task.StatusCode)StatusId; }
            set { StatusId = (int)value; }
        }
        public string Location { get; set; }
        public string Project { get; set; }
        public bool ShowCompleted { get { return CompletedOn.HasValue; } }
        public bool ShowLocation { get { return HttpContext.Current.User.IsInRole("AdvancedTask"); } }
        public bool HasProject
        {
            get { return string.IsNullOrEmpty(Project); }
        }
        public int? SourceContactId { get; set; }
        public DateTime? SourceContact { get; set; }
        public string SourceContactChange
        {
            get { return AssignChange(SourceContactId); }
        }
        public int? CompletedContactId { get; set; }
        public DateTime? CompletedContact { get; set; }
        public string Notes { get; set; }
        public string FmtNotes
        {
            get { return Util.SafeFormat(Notes); }
        }
        public bool HasNotes
        {
            get { return string.IsNullOrEmpty(Notes); }
        }
        public bool CanComplete
        {
            get { return IsAnOwner && this.StatusEnum != Task.StatusCode.Complete && !ForceCompleteWContact; }
        }
        public bool CanCompleteWithContact
        {
            get { return IsAnOwner && this.StatusEnum != Task.StatusCode.Complete && WhoId != null; }
        }
        public bool CanAccept
        {
            get { return IsCoOwner && this.StatusEnum == Task.StatusCode.Pending; }
        }
        public string ProspectReportLink()
        {
            Util.CurrentPeopleId = WhoId.Value;
            HttpContext.Current.Session["ActivePerson"] = Who;
            var qb = DbUtil.Db.QueryBuilderIsCurrentPerson();
            return "/Report/ProspectCardsRpt.aspx?id=" + qb.QueryId;
        }
        public IEnumerable<SelectListItem> PriorityList()
        {
            return new SelectListItem[]
            {
                new SelectListItem { Value = "", Text = "None" },
                new SelectListItem { Value = "1", Text = "1st Priority" },
                new SelectListItem { Value = "2", Text = "2nd Priority" },
                new SelectListItem { Value = "3", Text = "3rd Priority" },
            };
        }
        public IEnumerable<SelectListItem> StatusList()
        {
            return from s in DbUtil.Db.TaskStatuses
                   select new SelectListItem
                   {
                       Text = s.Description,
                       Value = s.Id.ToString()
                   };
        }
        public void UpdateTask(ITaskNotify notify)
        {
            var sb = new StringBuilder();
            var task = DbUtil.Db.Tasks.Single(t => t.Id == Id);
            TaskModel.ChangeTask(sb, task, "Description", Description);
            TaskModel.ChangeTask(sb, task, "Due", Due);
            TaskModel.ChangeTask(sb, task, "Notes", Notes);
            TaskModel.ChangeTask(sb, task, "StatusId", StatusId);
            task.ForceCompleteWContact = ForceCompleteWContact;
            if (HttpContext.Current.User.IsInRole("AdvancedTask"))
                TaskModel.ChangeTask(sb, task, "Project", Project);

            task.Location = Location;
            if (Priority == 0)
                task.Priority = null;
            else
                task.Priority = Priority;
            DbUtil.Db.SubmitChanges();
            TaskModel.NotifyIfNeeded(notify, sb, task);
        }
    }
}
