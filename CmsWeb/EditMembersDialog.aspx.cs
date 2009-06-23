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
        public int? GroupId;
        private bool GroupMode;
        private string from;
        private List<int> members;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            OrgId = Page.QueryString<int?>("id");
            GroupId = Page.QueryString<int?>("group");
            GroupMode = GroupId.HasValue;
            SetMembers();
            GroupId = GroupId ?? 0;
            MemberData.SelectParameters["noinactive"].DefaultValue = (GroupId > 0).ToString();
        }
        private void SetMembers()
        {
            if (GroupMode)
            {
                var q = from m in DbUtil.Db.OrgMemMemTags
                        where m.OrgId == OrgId && m.MemberTagId == GroupId
                        select m.PeopleId;
                members = q.ToList();
            }
            else
            {
                var tag = DbUtil.Db.FetchOrCreateTag(Util.SessionId, Util.UserPeopleId, DbUtil.TagTypeId_AddSelected);
                members = DbUtil.Db.TaggedPeople(tag.Id).Select(p => p.PeopleId.Value).ToList();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            var dp = ListView1.FindControl("pager") as DataPager;
            if (dp != null)
                dp.PageSize = Util.GetPageSizeCookie();
            from = Page.QueryString<string>("from");
            EditSection.Visible = !GroupId.HasValue;
            if (!OrgId.HasValue || (!from.HasValue() && EditSection.Visible))
                throw new Exception("Cannot visit EditMembersDialog this way");
            if (!IsPostBack)
            {
                MemberType.SelectedValue = "220";
                PersonSearchDialogController.ResetSearchTags();
            }
        }

        [System.Web.Services.WebMethod]
        public static string ToggleTag(int PeopleId, int OrgId, int GroupId, string controlid)
        {
            if (GroupId > 0)
            {
                var Db = DbUtil.Db;
                var m = Db.OrganizationMembers.Single(om => om.OrganizationId == OrgId && om.PeopleId == PeopleId);
                var r = new ToggleTagReturn { ControlId = controlid };
                r.HasTag = m.ToggleGroup(GroupId);
                Db.SubmitChanges();
                return SearchDialog.JsonReturnStr(r);
            }
            else
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
            var list = new List<int>();
            foreach (var om in q)
            {
                if (membertype == (int)OrganizationMember.MemberTypeCode.Drop)
                {
                    list.Add(om.PeopleId);
                    om.Drop();
                }
                else
                {
                    om.MemberTypeId = membertype;
                    om.InactiveDate = inactivedate;
                }
            }
            DbUtil.Db.SubmitChanges();
            foreach (var pid in list)
                DbUtil.Db.UpdateSchoolGrade(pid);
            this.Page.ClientScript.RegisterStartupScript(typeof(AddMemberDialog),
                "closeThickBox", "self.parent.RebindMemberGrids('{0}');".Fmt(from), true);
        }

        protected void SelectAll_CheckedChanged(object sender, EventArgs e)
        {
            var q = PersonSearchDialogController.SearchMembers(SearchMemberType.Text.ToInt(),
                        TagSearch.SelectedValue.ToInt(),
                        SearchInactiveDate.Text.ToDate(),
                        OrgId.Value, GroupId > 0);
            if (GroupMode)
                if (SelectAll.Checked)
                {
                    var q2 = from om in q
                             where !om.OrgMemMemTags.Any(mt => mt.MemberTagId == GroupId)
                             select om;
                    foreach (var om in q2)
                        om.ToggleGroup(GroupId.Value);
                    DbUtil.Db.SubmitChanges();
                    SetMembers();
                }
                else
                {
                    var q2 = from om in q
                             where om.OrgMemMemTags.Any(mt => mt.MemberTagId == GroupId)
                             select om;
                    foreach (var om in q2)
                        om.ToggleGroup(GroupId.Value);
                    DbUtil.Db.SubmitChanges();
                    SetMembers();
                }
            else
            {
                var q2 = q.Select(om => om.Person);
                Tag tag = null;
                if (SelectAll.Checked)
                    tag = DbUtil.Db.PopulateSpecialTag(q2, DbUtil.TagTypeId_AddSelected);
                else
                    DbUtil.Db.DePopulateSpecialTag(q2, DbUtil.TagTypeId_AddSelected);
                SetMembers();
            }
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

        protected void ListView1_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType != ListViewItemType.DataItem)
                return;
            var cb = e.Item.FindControl("ck") as CheckBox;
            var r = e.Item as ListViewDataItem;
            var d = r.DataItem as PersonDialogSearchInfo;
            cb.Checked = members.Contains(d.PeopleId);
        }
    }
}
