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
using System.Text;

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
            SendNotices(DivOrg.SelectedValue.ToInt(),
                SubDivOrg.SelectedValue.ToInt(),
                Organization.SelectedValue.ToInt(),
                EndDate.Text.ToDate().Value,
                new WebEmailer());
            Label1.Visible = true;
            Button1.Enabled = false;
        }
        private void SendNotices(
            int progid,
            int divid,
            int orgid,
            DateTime date,
            ITaskNotify notify)
        {
            var q = from m in DbUtil.Db.OrganizationMembers
                    where m.OrganizationId == orgid || orgid == 0
                    where m.Organization.DivOrgs.Any(t => t.DivId == divid) || divid == 0
                    where m.Organization.DivOrgs.Any(t => t.Division.ProgId == progid) || progid == 0
                    where m.Organization.Meetings.Any(meeting => meeting.MeetingDate.Value.Date == date.Date)
                    where m.MemberTypeId != (int)OrganizationMember.MemberTypeCode.InActive
                    let u = m.Person.Users.FirstOrDefault(uu => uu.UserRoles.Any(r => r.Role.RoleName == "Access"))
                    where u != null
                    group m by m.PeopleId into g
                    select g;
            var sb2 = new StringBuilder("Notices sent to:</br>\n<table>\n");
            foreach (var g in q)
            {
                var person = g.First().Person;
                var sb = new StringBuilder("The following meetings are ready to be viewed:<br/>\n");
                foreach (var om in g)
                {
                    var q2 = from mt in DbUtil.Db.Meetings
                             where mt.OrganizationId == om.OrganizationId
                             where mt.MeetingDate.Value.Date == date.Date
                             select new
                             {
                                 mt.MeetingId,
                                 mt.Organization.OrganizationName,
                                 mt.MeetingDate,
                                 mt.Organization.LeaderName,
                                 mt.Organization.Location
                             };
                    foreach (var mt in q2)
                    {
                        string orgname = CmsData.Organization.FormatOrgName(mt.OrganizationName, mt.LeaderName, mt.Location);
                        sb.AppendFormat("<a href='{0}/Meeting.aspx?id={1}'>{2} - {3}</a><br/>\n",
                            DbUtil.Db.CmsHost, mt.MeetingId, orgname, mt.MeetingDate);
                        sb2.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2:M/d/yy h:mmtt}</td></tr>\n",
                            person.Name, orgname, mt.MeetingDate);
                    }
                }
                notify.EmailNotification(DbUtil.Db.CurrentUser.Person, person,
                    "Attendance reports ready for viewing on CMS", sb.ToString());
            }
            sb2.Append("</table>\n");
            notify.EmailNotification(DbUtil.Db.CurrentUser.Person, DbUtil.Db.CurrentUser.Person,
                "Attendance emails sent", sb2.ToString());
        }


        protected void CustomValidator2_ServerValidate(object source, ServerValidateEventArgs args)
        {
            CustomValidator1.IsValid = DivOrg.SelectedValue.ToInt() > 0;
            CustomValidator2.IsValid = SubDivOrg.SelectedValue.ToInt() > 0;
            args.IsValid = CustomValidator1.IsValid && CustomValidator2.IsValid;
        }
    }
}
