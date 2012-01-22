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
using System.Web.UI;
using System.Web.UI.WebControls;
using UtilityExtensions;
using CMSPresenter;
using CmsData;
using System.Drawing;
using AjaxControlToolkit;
using CustomControls;
using System.Configuration;
using System.Data;
using System.Web.Security;
using CmsData.Codes;

namespace CmsWeb
{
    public partial class Contact : System.Web.UI.Page
    {
        public CmsData.Contact contact;
        private ContactController ctrl = new ContactController();

        public bool CanEdit()
        {
            return true;
        }
        public Task task;

        protected void Page_Load(object sender, EventArgs e)
        {
            int? id = this.QueryString<int?>("id");
            contact = DbUtil.Db.Contacts.SingleOrDefault(c => c.ContactId == id);
            if (contact == null)
                Response.EndShowMessage("no contact", "/", "home");

            if (!Page.IsPostBack)
            {
                GridPager.SetPageSize(ContacteeGrid);
                GridPager.SetPageSize(ContactorGrid);
            }

            if (!Page.IsPostBack && this.QueryString<int?>("edit").HasValue)
                EditUpdateButton1.Editing = true;
            EditUpdateButton1.Enabled = CanEdit();
            DeleteButton.Enabled = CanEdit();
            EditUpdateButton1.DataBind();

            task = contact.TasksCompleted.FirstOrDefault();
            TaskRow.Visible = task != null;
            if (TaskRow.Visible)
            {
                TaskLink.Text = task.Description;
                TaskLink.NavigateUrl = "~/Task/List/{0}".Fmt(task.Id);
            }
            CommentsSection.Visible = ctrl.CanViewComments((int)id);
        }

        protected void RefreshGrids_Click(object sender, EventArgs e)
        {
            ContactorGrid.DataBind();
            ContacteeGrid.DataBind();
        }

        protected void EditUpdateButton1_Click(object sender, EventArgs e)
        {
            if (EditUpdateButton1.Updating)
            {
                DbUtil.Db.SubmitChanges();
                EditUpdateButton1.DataBind();
            }
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            ctrl.DeleteContact(contact.ContactId);
            Response.EndShowMessage("Contact Deleted", "/ContactSearch.aspx", "click here");
        }

        protected void AddTeamContact_Click(object sender, EventArgs e)
        {
            var c = new CmsData.Contact
            {
                ContactDate = contact.ContactDate,
                ContactTypeId = contact.ContactTypeId,
                ContactReasonId = contact.ContactReasonId,
                MinistryId = contact.MinistryId,
                CreatedDate = Util.Now,
                CreatedBy = Util.UserId1,
            };
            DbUtil.Db.Contacts.InsertOnSubmit(c);
            foreach (var cp in contact.contactsMakers)
                c.contactsMakers.Add(new Contactor { PeopleId = cp.PeopleId });
            DbUtil.Db.SubmitChanges();
            Response.Redirect("~/contact.aspx?id=" + c.ContactId);
        }

        protected void AddTask_Command(object sender, CommandEventArgs e)
        {
            int index = e.CommandArgument.ToInt();
            int pid = (int)ContacteeGrid.DataKeys[index]["PeopleId"];
            var uid = Util.UserPeopleId.Value;
            var task = new Task
            {
                OwnerId = uid,
                WhoId = pid,
                SourceContactId = contact.ContactId,
                Description = "Follow up",
                ListId = Models.TaskModel.InBoxId(uid),
                StatusId = TaskStatusCode.Active,
                Project = contact.MinistryId == null ? null : contact.Ministry.MinistryName,
            };
            DbUtil.Db.Tasks.InsertOnSubmit(task);
            DbUtil.Db.SubmitChanges();
            Response.Redirect("~/Task/List/{0}".Fmt(task.Id));
        }

        protected void ContacteeGrid_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var b = e.Row.FindControl("AddTask") as LinkButton;
                b.CommandArgument = e.Row.RowIndex.ToString();
            }
        }

    }
}
