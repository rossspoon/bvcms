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
            if (Session != null)
                if (Session.IsNewSession)
                {
                    string CookieHeader = Request.Headers["Cookie"];
                    if ((null != CookieHeader) && (CookieHeader.IndexOf("ASP.NET_SessionId") >= 0))
                        Response.Redirect("/Errors/SessionTimeout.htm");
                }

            if (ScriptManager.IsInAsyncPostBack)
                return;
            if (NoCache)
                Response.NoCache();
            cssbundle.Text = SquishIt.Framework.Bundle.Css()
                            .Add("/Content/jquery-ui-1.8.13.custom.css")
                            .Add("/Content/site.css")
                            .Add("/Content/Style2.css")
                            .Add("/Content/cmenu.css")
                            .Add("/Content/pager.css")
                            .Render("/Content/AllMVC_#.css");

            if (Util.SessionStarting)
            {
                if (Util2.OrgMembersOnly)
                    DbUtil.Db.SetOrgMembersOnly();
                else if (Util2.OrgLeadersOnly)
                    DbUtil.Db.SetOrgLeadersOnly();
                Util.SessionStarting = false;
            }
            if (!Util2.OrgMembersOnly && Page.User.IsInRole("OrgMembersOnly"))
            {
                Util2.OrgMembersOnly = true;
                DbUtil.Db.SetOrgMembersOnly();
            }
            else if (!Util2.OrgLeadersOnly && Page.User.IsInRole("OrgLeadersOnly"))
            {
                Util2.OrgLeadersOnly = true;
                DbUtil.Db.SetOrgLeadersOnly();
            }

            var r = Models.AccountModel.CheckAccessRole(Util.UserName);
            if (r.HasValue())
                Response.Redirect(r);

            if (CMSMembershipProvider.provider.UserMustChangePassword)
                Response.Redirect(ResolveUrl("/Account/ChangePassword"));
            Membership.GetUser(); // record activity

            string pa = System.IO.Path.ChangeExtension(Request.Url.AbsolutePath, "");
            pa = pa.Substring(0, pa.Length - 1);
        }
        protected void NewQuery_Click(object sender, EventArgs e)
        {
            var qb = DbUtil.Db.QueryBuilderScratchPad();
            qb.CleanSlate(DbUtil.Db);
            Response.Redirect("/QueryBuilder/Main/");
        }
    }
}