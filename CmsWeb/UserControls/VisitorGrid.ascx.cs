/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CMSPresenter;
using UtilityExtensions;
using CmsData;

namespace CMSWeb.UserControls
{
    public partial class VisitorGrid : System.Web.UI.UserControl
    {
        public event EventHandler RebindMemberGrids;
        protected void Page_Load(object sender, EventArgs e)
        {
            pager.PageSize = UtilityExtensions.Util.GetPageSizeCookie();
        }
        public override void DataBind()
        {
            ListView1.DataBind();
        }
        public string DataSourceID
        {
            set { ListView1.DataSourceID = value; }
        }
        public object DataSource
        {
            set { ListView1.DataSource = value; }
        }

        protected void ListView1_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var link = e.Item.FindControl("HyperLink1") as HyperLink;
                var r = e.Item as ListViewDataItem;
                var d = r.DataItem as PersonInfo;
                link.NavigateUrl = "javascript:PageMethods.ToggleTag({0},'{1}',ToggleTagCallback)".Fmt(d.PeopleId, link.ClientID);

                var jlink = e.Item.FindControl("JoinLink") as LinkButton;
                jlink.Enabled = Page.User.IsInRole("Attendance");
            }
        }

        protected void ListView1_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "Join")
            {
                var pid = e.CommandArgument.ToInt();
                var orgid = Page.QueryString<int>("id");
                OrganizationMember.InsertOrgMembers(orgid, pid, 
                    (int)OrganizationMember.MemberTypeCode.Member, Util.Now.Date, null, false);
                if (RebindMemberGrids != null)
                    RebindMemberGrids(this, e);
            }
        }
    }
}