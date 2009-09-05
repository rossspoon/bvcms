/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Text;
using UtilityExtensions;
using CMSPresenter;
using CmsData;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Collections.Generic;
using System.Web;

namespace CMSWeb
{
    public partial class AddTaskAboutDialog : System.Web.UI.Page
    {
        public int TaskId;

        protected void Page_Load(object sender, EventArgs e)
        {
            TaskId = Request.QueryString["id"].ToInt();
            if (TaskId == 0)
                throw new Exception("Cannot visit AddTaskAboutDialog this way");
            if (!IsPostBack)
                PersonSearchDialogController.ResetSearchTags();
            Parameters.SearchButtonClicked += new EventHandler(SearchButton_Click);
            Parameters.ClearButtonClicked += new EventHandler(Parameters_ClearButtonClicked);
            AddNew1.Enabled = User.IsInRole("Edit");
        }

        void Parameters_ClearButtonClicked(object sender, EventArgs e)
        {
            ListView1.Visible = false;
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            ListView1.Visible = true;
            var ctl = new PersonSearchDialogController();
            ListView1.DataSource = ctl.FetchSearchList(Parameters, false);
            ListView1.DataBind();
        }

        protected void AddNew1_Click(object sender, EventArgs e)
        {
            CustomValidator1.Text = "Must Select a Family";
            if (!Parameters.Name.HasValue())
            {
                CustomValidator1.IsValid = false;
                CustomValidator1.Text = "Must have a name when adding";
            }
            else
                CustomValidator1.IsValid = PersonSearchDialogController
                    .AddNewPerson(Parameters.Name, 
                        Parameters.DOB, 
                        FamilyOption.SelectedValue, 
                        Parameters.Gender, 
                        (int)Person.OriginCode.Request,
                        null, DbUtil.Settings("DefaultCampusId").ToInt2());
            if (!CustomValidator1.IsValid)
                return;
            var p = SearchDialog.SelectedPeople().First();
            p.OriginId = (int)Person.OriginCode.Request;
            Models.TaskModel.SetWhoId(TaskId, p.PeopleId);
            Page.ClientScript.RegisterStartupScript(typeof(AddTaskAboutDialog), "closeThickBox", "self.parent.AddSelected();", true);
        }

        protected void ListView1_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "select1")
            {
                Models.TaskModel.SetWhoId(TaskId, e.CommandArgument.ToInt());
                Page.ClientScript.RegisterStartupScript(typeof(AddTaskAboutDialog), "closeThickBox", "self.parent.AddSelected();", true);
            }
        }
    }
}
