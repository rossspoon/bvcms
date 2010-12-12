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
using CmsWeb.Areas.Manage.Controllers;

namespace CmsWeb
{
    public partial class Site : System.Web.UI.MasterPage
    {
        public System.Web.UI.ScriptManager ScriptManager { get { return ScriptManager1; } }

        public bool NoCache { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ScriptManager.IsInAsyncPostBack)
                return;

            cssbundle.Text = ViewExtensions2.StandardCss();
            jsbundle.Text = SquishIt.Framework.Bundle.JavaScript()
                .Add("/Content/js/jquery-1.4.2.js")
                .Add("/Content/js/jquery-ui-1.8.2.custom.js")
                .Add("/Content/js/jquery.bgiframe-2.1.1.js")
                .Add("/Content/js/hoverIntent.js")
                .Add("/Content/js/jquery-cookie.js")
                .Add("/Content/js/jquery.blockUI.js")
                .Add("/Content/js/superfish.js")
                .Add("/Content/js/supersubs.js")
                .Add("/Scripts/ExportToolBar.js")
                .Render("/Content/combined_#.js");

            urgentNotice.Visible = ((string)Application["getoff"]).HasValue();
            if (urgentNotice.Visible)
                urgentNotice.Text = (string)Application["getoff"];

            if (NoCache)
                Response.NoCache();

            FooterEntity.ToolTip = HttpContext.Current.Request.UserHostAddress.ToString();
            if (Util.SessionStarting)
            {
                if (Util2.OrgMembersOnly)
                    DbUtil.Db.SetOrgMembersOnly();
                Util.SessionStarting = false;
            }
            if (!Util2.OrgMembersOnly && Page.User.IsInRole("OrgMembersOnly"))
            {
                Util2.OrgMembersOnly = true;
                DbUtil.Db.SetOrgMembersOnly();
            }

            var r = AccountController.CheckAccessRole(Util.UserName);
            if (r.HasValue())
                Response.Redirect(r);

            if (CMSMembershipProvider.provider.UserMustChangePassword)
                Response.Redirect(ResolveUrl("/ChangePassword.aspx"));
            Membership.GetUser(); // record activity

            NewUserItem.Visible = false;
            if (Util2.CurrentPeopleId != 0)
            {
                CurrentPersonMenuItem.Visible = true;
                CurrentPersonLink.NavigateUrl = "Person/Index/{0}".Fmt(Util2.CurrentPeopleId);
                CurrentPersonLink.Text = Session["ActivePerson"].ToString();
                if (Page.User.IsInRole("Admin"))
                {
                    NewUserItem.Visible = true;
                    NewUser.NavigateUrl = "/Account/AddUser/" + Util2.CurrentPeopleId;
                    NewUser.Text = "Add '" + Session["ActivePerson"].ToString() + "' as user";
                }
            }

            if (Util2.CurrentOrgId > 0)
            {
                CurrentOrgMenuItem.Visible = true;
                CurrentOrgLink.NavigateUrl = "Organization/Index/{0}".Fmt(Util2.CurrentOrgId);
                if (Session["ActiveOrganization"] != null)
                    CurrentOrgLink.Text = Session["ActiveOrganization"].ToString();
                else
                    CurrentOrgLink.Text = "Current Org";
            }
            //HomeDrop.Visible = Page.User.IsInRole("Developer");
            SavedQueriesLink.Enabled = !Util2.OrgMembersOnly;
            AdminMenuItem.Visible = Page.User.IsInRole("Admin");
            OrgMembersMenuItem.Visible = Page.User.IsInRole("Edit");
            ContributionsMenuItem.Visible = Page.User.IsInRole("Finance");
            OrgMembersOnly.Text = Util2.OrgMembersOnly ? "Turn OrgMembersOnly Off" : "Turn OrgMembersOnly On";
            AdminMenuLink.ToolTip = Util.ConnectionString;
            UserHeader.Text = DbUtil.Header();
            string pa = System.IO.Path.ChangeExtension(Request.Url.AbsolutePath, "");
            pa = pa.Substring(0, pa.Length - 1);
            HelpLink.NavigateUrl = Util.HelpLink(pa.Replace('/', '_'));
            HelpLink2.NavigateUrl = HelpLink.NavigateUrl;
        }
        protected void NewQuery_Click(object sender, EventArgs e)
        {
            var qb = DbUtil.Db.QueryBuilderScratchPad();
            qb.CleanSlate(DbUtil.Db);
            Response.Redirect("/QueryBuilder/Main/");
        }

        protected void SearchText_TextChanged(object sender, EventArgs e)
        {
            if (SearchText.Text != "Quick Search")
                Response.Redirect(Page.ResolveUrl("~/Search.aspx?name=" + SearchText.Text));
        }
    }
}