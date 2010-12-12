/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Web.Security;
using System.Web.UI.WebControls;
using UtilityExtensions;
using CMSPresenter;
using CmsData;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading;
using System.Web;

namespace CmsWeb.Admin
{
    public partial class Users : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            pager1.PageSize = Util.GetPageSizeCookie();
            pager2.PageSize = Util.GetPageSizeCookie();
            if (!Page.IsPostBack && this.QueryString<int>("create") > 0)
            {
                ListView1.Sort("CreationDate", SortDirection.Descending);
                TextBox1.Text = string.Empty;
                ListView1.DataBind();
                ListView1.SelectedIndex = 0;
                if (Session[UserController.STR_ShowPassword] != null)
                    ListView1.EditIndex = 0;
            }
        }
        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            Session.Remove(UserController.STR_ShowPassword);
        }

        protected void ButtonCreateNewRole_Click(object sender, EventArgs e)
        {
            if (TextBoxCreateNewRole.Text.HasValue())
            {
                Roles.CreateRole(TextBoxCreateNewRole.Text);
                TextBoxCreateNewRole.Text = "";
            }
        }

        protected void ObjectDataSourceMembershipUser_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                CheckNewUser.IsValid = false;
                LabelInsertMessage.Text = e.Exception.InnerException.Message + " Insert Failed";
                LabelInsertMessage.ForeColor = System.Drawing.Color.Red;
                e.ExceptionHandled = true;
            }
            else
            {
                ListView1.Sort("CreationDate", SortDirection.Descending);
                TextBox1.Text = string.Empty;
                ListView1.DataBind();
                ListView1.SelectedIndex = 0;
            }
        }

        public void SearchForUsers(object sender, EventArgs e)
        {
            ListView1.DataBind();
        }

        protected void CheckNewUser_ServerValidate(object source, ServerValidateEventArgs args)
        {

        }

        protected void ListView1_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "Deselect")
                ListView1.SelectedIndex = -1;
        }

        protected void ListView1_Sorting(object sender, ListViewSortEventArgs e)
        {
            ListView1.SelectedIndex = -1;
        }

        protected void ListView1_ItemDeleted(object sender, ListViewDeletedEventArgs e)
        {
			ListView1.SelectedIndex = -1;
        }

        protected void ListView1_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            ListView1.SelectedIndex = -1;
        }

        protected void RolesCheckBoxList_DataBound(object sender, EventArgs e)
        {
            var cbl = sender as CheckBoxList;
            var di = cbl.Parent as ListViewDataItem;
            var u = di.DataItem as User;
            var roles = u.Roles;
            foreach (ListItem li in cbl.Items)
                li.Selected = roles.Contains(li.Text);
        }

        protected void RolesCheckBoxList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cbl = sender as CheckBoxList;
            var di = cbl.Parent as ListViewDataItem;
            var UserId = ListView1.DataKeys[di.DisplayIndex].Value.ToInt();
            var user = DbUtil.Db.Users.Single(u => u.UserId == UserId);
            var checkedRoles = new List<string>();
            foreach (ListItem li in cbl.Items)
                if (li.Selected)
                    checkedRoles.Add(li.Text);
            user.SetRoles(DbUtil.Db, checkedRoles.ToArray());
            DbUtil.Db.SubmitChanges();
        }
        protected void AddSelectedPerson_Click(object sender, EventArgs e)
        {
            var tag = DbUtil.Db.FetchOrCreateTag(Util.SessionId, Util.UserPeopleId, DbUtil.TagTypeId_AddSelected);
            var p = DbUtil.Db.People.Where(pp => pp.Tags.Any(t => t.Id == tag.Id)).First();
            var UserId = ListView1.SelectedValue.ToInt();
            var user = DbUtil.Db.Users.Single(u => u.UserId == UserId);
            var lvi = ListView1.Items[ListView1.SelectedIndex] as ListViewItem;
            user.PeopleId = p.PeopleId;
            DbUtil.Db.SubmitChanges();
        }
    }
}
