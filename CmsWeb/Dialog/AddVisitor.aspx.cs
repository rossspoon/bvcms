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
    public partial class AddVisitor : System.Web.UI.Page
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
            AddNew1.Enabled = User.IsInRole("Edit");
        }

        [System.Web.Services.WebMethod]
        public static string ToggleTag(int PeopleId, string controlid)
        {
            return Search.ToggleTag(PeopleId, controlid);
        }
        protected void AddSelectedVisitors_Click(object sender, EventArgs e)
        {
            string addSelected = "self.parent.AddSelected();";
            foreach (var p in Search.SelectedPeople())
            {
                var r = Attend.RecordAttendance(p.PeopleId, MeetingId, true);
                if (r.HasValue())
                {
                    addSelected = "self.parent.AddSelected('{0}');".Fmt(r);
                    break;
                }
            }
            this.Page.ClientScript.RegisterStartupScript(typeof(AddVisitor),
                "closeThickBox", addSelected, true);
        }
        protected void AddNew1_Click(object sender, EventArgs e)
        {
            bool AddressOK = FamilyOption.SelectedValue.ToInt() 
                != (int)PersonSearchDialogController.AddFamilyType.ExistingFamily;
            Parameters.ValidateAddNew(ref CustomValidator1, AddressOK, FamilyOption.SelectedValue);
            if (CustomValidator1.IsValid)
            {
                var org = (from m in DbUtil.Db.Meetings
                           where m.MeetingId == MeetingId
                           select m.Organization).First();
                CustomValidator1.IsValid = PersonSearchDialogController
                    .AddNewPerson(Parameters.Name,
                                   Parameters.DOB,
                                   FamilyOption.SelectedValue,
                                   Parameters.Gender,
                                   (int)Person.OriginCode.Visit,
                                   org.EntryPointId, org.CampusId,
                                   Parameters.Comm,
                                   Parameters.Addr, Parameters.Married);
                CustomValidator1.ErrorMessage = "must select family to add to";
            }
            if (!CustomValidator1.IsValid)
                return;
            var ctl = new PersonSearchDialogController();
            ListView1.DataSource = ctl.FetchSearchList(Parameters, false);
            ListView1.DataBind();
        }
    }
}
