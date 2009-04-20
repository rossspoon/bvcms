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
    public partial class AddTaskDelegateDialog : System.Web.UI.Page
    {
        public int TaskId;

        protected void Page_Load(object sender, EventArgs e)
        {
            TaskId = Request.QueryString["id"].ToInt();
            if (TaskId == 0)
                throw new Exception("Cannot visit AddTaskDelegateDialog this way");
            if (!IsPostBack)
                PersonSearchDialogController.ResetSearchTags();
            Parameters.SearchButtonClicked += new EventHandler(SearchButton_Click);
            Parameters.ClearButtonClicked += new EventHandler(Parameters_ClearButtonClicked);
        }

        void Parameters_ClearButtonClicked(object sender, EventArgs e)
        {
            ListView1.Visible = false;
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            ListView1.Visible = true;
            var ctl = new PersonSearchDialogController();
            ListView1.DataSource = ctl.FetchSearchList(Parameters, true);
            ListView1.DataBind();
        }

        protected void ListView1_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "select1")
            {
                var tmodel = new Models.TaskModel();
                if (this.QueryString<int?>("chown").HasValue)
                    tmodel.ChangeOwner(TaskId, e.CommandArgument.ToInt(), new Emailer());
                else
                    tmodel.Delegate(TaskId, e.CommandArgument.ToInt(), new Emailer());
                Page.ClientScript.RegisterStartupScript(typeof(AddTaskDelegateDialog), "closeThickBox", "self.parent.AddSelected();", true);
            }
        }
    }
}
