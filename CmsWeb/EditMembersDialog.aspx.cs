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
    public partial class EditMembersDialog : System.Web.UI.Page
    {
        public int? OrgId;

        private string from;
        protected void Page_Load(object sender, EventArgs e)
        {
            var dp = ListView1.FindControl("pager") as DataPager;
            if (dp != null)
                dp.PageSize = Util.GetPageSizeCookie();
            OrgId = Page.QueryString<int?>("id");
            from = Page.QueryString<string>("from");
            if (!OrgId.HasValue || !from.HasValue())
                throw new Exception("Cannot visit EditMembersDialog this way");
            if (!IsPostBack)
            {
                MemberType.SelectedValue = "220";
                PersonSearchDialogController.ResetSearchTags();
            }
        }

        [System.Web.Services.WebMethod]
        public static string ToggleTag(int PeopleId, string controlid)
        {
            return SearchDialog.ToggleTag(PeopleId, controlid);
        }
        protected void UpdateSelectedMembers_Click(object sender, EventArgs e)
        {
            if (!User.IsInRole("Attendance"))
                return;
            int membertype = MemberType.SelectedValue.ToInt();
            DateTime? inactivedate = InactiveDate.Text.ToDate();
            var q = from p in SearchDialog.SelectedPeople()
                    from om in p.OrganizationMembers.Where(om => om.OrganizationId == OrgId)
                    select om;
            foreach (var om in q)
            {
                if (membertype == (int)OrganizationMember.MemberTypeCode.Drop)
                    om.Drop();
                else
                {
                    om.MemberTypeId = membertype;
                    om.InactiveDate = inactivedate;
                }
            }
            DbUtil.Db.SubmitChanges();
            this.Page.ClientScript.RegisterStartupScript(typeof(AddMemberDialog),
                "closeThickBox", "self.parent.RebindMemberGrids('{0}');".Fmt(from), true);
        }

        protected void SelectAll_CheckedChanged(object sender, EventArgs e)
        {
            var q = PersonSearchDialogController.SearchMembers(SearchMemberType.Text.ToInt(), 
                        TagSearch.SelectedValue.ToInt(), 
                        SearchInactiveDate.Text.ToDate(), 
                        OrgId.Value);
            var q2 = q.Select(om => om.Person);
            Tag tag = null;
            if (SelectAll.Checked)
                tag = DbUtil.Db.PopulateSpecialTag(q2, DbUtil.TagTypeId_AddSelected);
            else
                DbUtil.Db.DePopulateSpecialTag(q2, DbUtil.TagTypeId_AddSelected);
            ListView1.Visible = true;
            ListView1.DataBind();
        }

        protected void Search_Changed(object sender, EventArgs e)
        {
            ListView1.DataBind();
        }

        protected void ClearSearch_Click(object sender, EventArgs e)
        {
            SearchMemberType.SelectedValue = "0";
            TagSearch.SelectedValue = "0";
            SearchInactiveDate.Text = "";
            ListView1.DataBind();
        }
    }
}
