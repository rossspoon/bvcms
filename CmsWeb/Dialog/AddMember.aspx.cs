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
    public partial class AddMember : System.Web.UI.Page
    {
        public int? OrgId;
        private bool? PendingMembers;
        private string from;
        protected void Page_Load(object sender, EventArgs e)
        {
            OrgId = Page.QueryString<int?>("id");
            from = Page.QueryString<string>("from");
            PendingMembers = Page.QueryString<bool?>("pending");
            if (!PendingMembers.HasValue)
                PendingMembers = false;
            if (!OrgId.HasValue || !from.HasValue())
                throw new Exception("Cannot visit AddMemberDialog this way");
            if (!IsPostBack)
            {
                PersonSearchDialogController.ResetSearchTags();
                EnrollmentDate.Text = Util.Now.Date.ToShortDateString();
                MemberType.SelectedValue = "220";
            }
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
        protected void AddSelectedMembers_Click(object sender, EventArgs e)
        {
            var q = from p in Search.SelectedPeople()
                    select p.PeopleId;
            int membertype = MemberType.SelectedValue.ToInt();
            DateTime? enrollmentdate = EnrollmentDate.Text.ToDate();
            DateTime? inactivedate = InactiveDate.Text.ToDate();
            foreach (var pid in q)
                OrganizationMember.InsertOrgMembers(OrgId.Value, pid, membertype, enrollmentdate.Value, inactivedate, PendingMembers.Value);
            this.Page.ClientScript.RegisterStartupScript(typeof(AddMember),
                "closeThickBox", "self.parent.RebindMemberGrids('{0}');".Fmt(from), true);
        }
        protected void AddNew1_Click(object sender, EventArgs e)
        {
            bool AddressOK = FamilyOption.SelectedValue.ToInt()
                != (int)PersonSearchDialogController.AddFamilyType.ExistingFamily;
            Parameters.ValidateAddNew(ref CustomValidator1, AddressOK, FamilyOption.SelectedValue);
            if (CustomValidator1.IsValid)
            {
                var org = DbUtil.Db.Organizations.Single(o => o.OrganizationId == OrgId);
                CustomValidator1.IsValid = PersonSearchDialogController
                    .AddNewPerson(Parameters.Name,
                                   Parameters.DOB,
                                   FamilyOption.SelectedValue,
                                   Parameters.Gender,
                                   (int)Person.OriginCode.Enrollment,
                                   org.EntryPointId, org.CampusId, 
                                   Parameters.Comm, Parameters.Addr, 
                                   Parameters.Married);
                CustomValidator1.ErrorMessage = "must select family to add to";
            }
            if (!CustomValidator1.IsValid)
                return;
            var ctl = new PersonSearchDialogController();
            ListView1.DataSource = ctl.FetchSearchList(Parameters, false);
            ListView1.DataBind();
        }

        protected void SelectAll_CheckedChanged(object sender, EventArgs e)
        {
            var q = PersonSearchController.ApplySearch(
                        Parameters.Name,
                        Parameters.Addr,
                        Parameters.Comm,
                        Parameters.Member,
                        Parameters.Tag,
                        Parameters.DOB,
                        Parameters.Gender,
                        Parameters.OrgId,
                        Parameters.Campus,
                        false, Parameters.Married);
            Tag tag = null;
            if (SelectAll.Checked)
                tag = DbUtil.Db.PopulateSpecialTag(q, DbUtil.TagTypeId_AddSelected);
            else
                DbUtil.Db.DePopulateSpecialTag(q, DbUtil.TagTypeId_AddSelected);
            ListView1.Visible = true;
            var ctl = new PersonSearchDialogController();
            ListView1.DataSource = ctl.FetchSearchList(Parameters, false);
            ListView1.DataBind();
        }
    }
}
