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
    public partial class AddVisitorDialog : System.Web.UI.Page
    {
        public int MeetingId;

        protected void Page_Load(object sender, EventArgs e)
        {
            MeetingId = Request.QueryString["id"].ToInt();
            if (MeetingId == 0)
                throw new Exception("Cannot visit AddVisitorDialog this way");
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

        [System.Web.Services.WebMethod]
        public static string ToggleTag(int PeopleId, string controlid)
        {
            return SearchDialog.ToggleTag(PeopleId, controlid);
        }
        protected void AddSelectedVisitors_Click(object sender, EventArgs e)
        {
            string addSelected = "self.parent.AddSelected();";
            var ctl = new AttendController();
            foreach (var p in SearchDialog.SelectedPeople())
            {
                var r = ctl.RecordAttendance(p.PeopleId, MeetingId, true);
                if (r.HasValue())
                {
                    addSelected = "self.parent.AddSelected('{0}');".Fmt(r);
                    break;
                }
            }
            this.Page.ClientScript.RegisterStartupScript(typeof(AddVisitorDialog),
                "closeThickBox", addSelected, true);
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
            {
                var entrypoint = (from m in DbUtil.Db.Meetings
                                  where m.MeetingId == MeetingId
                                  select m.Organization.EntryPointId).First();
                CustomValidator1.IsValid = PersonSearchDialogController
                    .AddNewPerson(Parameters.Name,
                                   Parameters.DOB,
                                   FamilyOption.SelectedValue,
                                   Parameters.Gender,
                                   (int)Person.OriginCode.Visit,
                                   entrypoint);
            }
            if (!CustomValidator1.IsValid)
                return;
            var ctl = new PersonSearchDialogController();
            ListView1.DataSource = ctl.FetchSearchList(Parameters, false);
            ListView1.DataBind();
        }
    }
}
