/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMSPresenter;
using UtilityExtensions;
using CmsData;

namespace CmsWeb
{
    public partial class EmailAttendanceNotices : System.Web.UI.Page
    {
        private CodeValueController CVController = new CodeValueController();
        protected void Page_Load(object sender, EventArgs e)
        {
            Button1.Enabled = User.IsInRole("Attendance");
            if (!IsPostBack)
            {
                DivOrg.Items.Clear();
                DivOrg.Items.Add(new ListItem("(not specified)", "0"));
                DivOrg.SelectedIndex = -1;
                DivOrg.DataSource = CVController.OrgDivTags();
                DivOrg.DataBind();
            }
            Button1.Enabled = DbUtil.Settings("emailer", "on") != "off";
        }
        private void ReBindDivOrg()
        {
            if (SubDivOrg.Visible)
            {
                SubDivOrg.DataSource = CVController
                    .OrgSubDivTags(DivOrg.SelectedValue.ToInt());
                SubDivOrg.DataBind();
            }
            ReBindOrg();
        }
        protected void DivOrg_SelectedIndexChanged(object o, EventArgs e)
        {
            DivOrg.Items.FindByValue("0").Enabled = false;
            Organization.SelectedIndex = -1;
            SubDivOrg.SelectedIndex = -1;
            ReBindDivOrg();
        }

        private void ReBindOrg()
        {
            if (Organization.Visible)
            {
                Organization.DataSource = CVController
                    .Organizations(SubDivOrg.SelectedValue.ToInt());
                Organization.DataBind();
            }
        }
        protected void SubDivOrg_SelectedIndexChanged(object o, EventArgs e)
        {
            Organization.SelectedIndex = -1;
            ReBindOrg();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;
            var ctl = new OrganizationController();
            ctl.SendNotices(DivOrg.SelectedValue.ToInt(),
                SubDivOrg.SelectedValue.ToInt(),
                Organization.SelectedValue.ToInt(),
                EndDate.Text.ToDate().Value,
                new Emailer());
            Label1.Visible = true;
            Button1.Enabled = false;
        }

        protected void CustomValidator2_ServerValidate(object source, ServerValidateEventArgs args)
        {
            CustomValidator1.IsValid = DivOrg.SelectedValue.ToInt() > 0;
            CustomValidator2.IsValid = SubDivOrg.SelectedValue.ToInt() > 0;
            args.IsValid = CustomValidator1.IsValid && CustomValidator2.IsValid;
        }
    }
}
