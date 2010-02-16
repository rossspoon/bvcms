/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Web.UI.WebControls;
using CMSPresenter;
using System.Web.UI;
using UtilityExtensions;
using System.Web;
using CmsData;
using System.Linq;
using CustomControls;

namespace CMSWeb
{
    public partial class MemberGrid : System.Web.UI.UserControl
    {
        public int OrgId { get; set; }
        public int GroupId { get; set; }
        public CMSPresenter.GroupSelect Select { get; set; }
        public event EventHandler RebindMemberGrids;
        protected void Page_Load(object sender, EventArgs e)
        {
            pager.PageSize = Util.GetPageSizeCookie();
            pager2.PageSize = Util.GetPageSizeCookie();
            AddMember.Visible = Page.User.IsInRole("Attendance") && Select!=GroupSelect.Previous;
            UpdateMembers.Visible = AddMember.Visible;
            AddMember.NavigateUrl = "~/Dialog/AddMember.aspx?id={0}&pending={2}&from={1}&TB_iframe=true&height=450&width=600"
                .Fmt(OrgId, MemberPanel.ClientID, Select==GroupSelect.Pending);
            UpdateMembers.NavigateUrl = "~/Dialog/EditMembers.aspx?id={0}&pending={2}&from={1}&TB_iframe=true&height=450&width=600"
                .Fmt(OrgId, MemberPanel.ClientID, Select==GroupSelect.Pending);
            Page.ClientScript.RegisterClientScriptBlock(typeof(MemberGrid), "membergrid", script);
            if (((CMSWeb.Site)Page.Master).ScriptManager.IsInAsyncPostBack)
                if (Page.Request.Params["__EVENTTARGET"] == MemberPanel.ClientID)
                    if (Page.Request.Params["__EVENTARGUMENT"] == "RebindMemberGrids")
                        if (RebindMemberGrids != null)
                            RebindMemberGrids(this, e);
        }
        string script = @"
<script type='text/javascript'>
    function RebindMemberGrids(panel) {
        tb_remove();
        __doPostBack(panel, 'RebindMemberGrids');
    }
    function ToggleTagCallback(e) {
        var result = eval('(' + e + ')');
        $('#' + result.ControlId).text(result.HasTag ? 'Remove' : 'Add');
    }
</script>
";

        protected void MemberGrid1_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var link = e.Item.FindControl("HyperLink1") as HyperLink;
                var r = e.Item as ListViewDataItem;
                var d = r.DataItem as PersonInfo;
                link.NavigateUrl = "javascript:PageMethods.ToggleTag({0},'{1}',ToggleTagCallback)".Fmt(d.PeopleId, link.ClientID);

                var mlink = e.Item.FindControl("MemberType") as HyperLink;
                if (mlink != null && r.DataItem is PersonMemberInfo)
                {
                    var mi = r.DataItem as PersonMemberInfo;
                    if (mi.FromTab == GroupSelect.Previous)
                        mlink.NavigateUrl = "~/AttendStrDetail.aspx?oid={0}&id={1}&TB_iframe=true&height=450&width=700"
                            .Fmt(OrgId, mi.PeopleId, MemberPanel.ClientID);
                    else
                        mlink.NavigateUrl = "/OrgMemberDialog/Index/{0}?pid={1}&from={2}&TB_iframe=true&height=450&width=600"
                            .Fmt(OrgId, mi.PeopleId, MemberPanel.ClientID);
                    //mlink.NavigateUrl = "~/Dialog/EditMember.aspx?oid={0}&pid={1}&from={2}&TB_iframe=true&height=450&width=600"
                    //        .Fmt(OrgId, mi.PeopleId, MemberPanel.ClientID);
                }
            }
        }

        protected void MembersData_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["Select"] = Select;
            e.InputParameters["GroupId"] = GroupId;
        }

        //protected void MemberGrid1_ItemUpdated(object sender, ListViewUpdatedEventArgs e)
        //{
        //    if (e.NewValues.Contains("MemberTypeId"))
        //    {
        //        int inactive = (int)OrganizationMember.MemberTypeCode.InActive;
        //        var o = int.Parse(e.OldValues["MemberTypeId"].ToString());
        //        var n = int.Parse(e.NewValues["MemberTypeId"].ToString());
        //        if (o == inactive || n == inactive)
        //            if (RebindMemberGrids != null)
        //            {
        //                RebindMemberGrids(this, e);
        //                MemberGrid1.SelectedIndex = -1;
        //            }
        //    }
        //}
    }
}
