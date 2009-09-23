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

namespace CMSWeb.Dialog
{
    public partial class AddFamily : System.Web.UI.Page
    {
        public int FamilyId;

        protected void Page_Load(object sender, EventArgs e)
        {
            FamilyId = Request.QueryString["id"].ToInt();
            if (FamilyId == 0)
                throw new Exception("Cannot visit AddFamilyDialog this way");
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
            ListView1.DataSource = ctl.FetchSearchList(Parameters, false);
            ListView1.DataBind();
        }

        protected void ListView1_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "select1")
            {
                FamilyController.AddRelatedFamily(FamilyId, e.CommandArgument.ToInt());
                Page.ClientScript.RegisterStartupScript(typeof(AddTaskAbout), "closeThickBox", "self.parent.AddSelected();", true);
            }
        }
    }
}
