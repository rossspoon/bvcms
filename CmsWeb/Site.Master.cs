/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Web;
using UtilityExtensions;
using System.Web.UI.HtmlControls;
using System.Web.Security;
using CMSPresenter;
using System.Web.UI.WebControls;
using System.Linq;
using System.Data.Linq;
using CmsData;

namespace CMSWeb
{
    public partial class Site : System.Web.UI.MasterPage
    {
        public System.Web.UI.ScriptManager ScriptManager { get { return ScriptManager1; } }

        public bool NoCache { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ScriptManager.IsInAsyncPostBack)
                return;

            urgentNotice.Visible = ((string)Application["getoff"]).HasValue();
            if (urgentNotice.Visible)
                urgentNotice.Text = (string)Application["getoff"];

            if (NoCache)
                Response.NoCache();

            FooterEntity.ToolTip = HttpContext.Current.Request.UserHostAddress.ToString();
            if (Util.SessionStarting)
            {
                if (Util.OrgMembersOnly)
                    DbUtil.Db.SetOrgMembersOnly();
                Util.SessionStarting = false;
            }
            if (!Util.OrgMembersOnly && Page.User.IsInRole("OrgMembersOnly"))
            {
                Util.OrgMembersOnly = true;
                DbUtil.Db.SetOrgMembersOnly();
            }
            Login.CheckStaffRole(Util.UserName);
            if (CMSMembershipProvider.provider.UserMustChangePassword)
                Response.Redirect(ResolveUrl("~/ChangePassword.aspx"));
            Membership.GetUser(); // record activity

            NewUserItem.Visible = false;
            if (Util.CurrentPeopleId != 0)
            {
                CurrentPersonMenuItem.Visible = true;
                CurrentPersonLink.NavigateUrl = "Person/Index/{0}".Fmt(Util.CurrentPeopleId);
                CurrentPersonLink.Text = Session["ActivePerson"].ToString();
                if (Page.User.IsInRole("Admin"))
                {
                    NewUserItem.Visible = true;
                    NewUser.NavigateUrl = "/Account/AddUser/" + Util.CurrentPeopleId;
                    NewUser.Text = "Add '" + Session["ActivePerson"].ToString() + "' as user";
                }
            }

            if (Util.CurrentOrgId > 0)
            {
                CurrentOrgMenuItem.Visible = true;
                CurrentOrgLink.NavigateUrl = "Organization.aspx?id={0}".Fmt(Util.CurrentOrgId);
                CurrentOrgLink.Text = Session["ActiveOrganization"].ToString();
            }
            //if (DbUtil.Db.UserPreference("neworgsearch").ToBool())
            //    OrgLink.NavigateUrl = "/OrgSearch/";
            //else
                OrgLink.NavigateUrl = "/OrganizationSearch.aspx";
            OrgSearchLink.NavigateUrl = OrgLink.NavigateUrl;
            //HomeDrop.Visible = Page.User.IsInRole("Developer");
            SavedQueriesLink.Enabled = !Util.OrgMembersOnly;
            AdminMenuItem.Visible = Page.User.IsInRole("Admin");
            ContributionsMenuItem.Visible = Page.User.IsInRole("Finance");
            OrgMembersOnly.Text = Util.OrgMembersOnly ? "Turn OrgMembersOnly Off" : "Turn OrgMembersOnly On";
            AdminMenuLink.ToolTip = Util.ConnectionString;
            UserHeader.Text = DbUtil.Header();
        }
        protected void NewQuery_Click(object sender, EventArgs e)
        {
            var qb = DbUtil.Db.QueryBuilderScratchPad();
            qb.CleanSlate();
            Response.Redirect("/QueryBuilder/Main/");
        }

        protected void SearchText_TextChanged(object sender, EventArgs e)
        {
            if (SearchText.Text != "Quick Search")
                Response.Redirect(Page.ResolveUrl("~/Search.aspx?name=" + SearchText.Text));
        }
    }
}