/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Linq;
using System.Web.UI.WebControls;
using CMSPresenter;
using UtilityExtensions;
using System.Web.Configuration;
using System.Web;
using System.Collections;
using CmsData;

namespace CMSWeb
{
    public partial class Organization : System.Web.UI.Page
    {
        public CmsData.Organization organization;
        public string displaynone = "display:none";

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            var qb = DbUtil.Db.QueryBuilderInCurrentOrg();
            ExportToolBar1.queryId = qb.QueryId;
            qb = DbUtil.Db.QueryBuilderVisitedCurrentOrg();
            ExportToolBar2.queryId = qb.QueryId;
            qb = DbUtil.Db.QueryBuilderInactiveCurrentOrg();
            ExportToolBar3.queryId = qb.QueryId;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            ((CMSWeb.Site)Page.Master).ScriptManager.EnablePageMethods = true;
            organization = DbUtil.Db.Organizations.Single(o => o.OrganizationId == this.QueryString<int>("id"));

            if (Util.OrgMembersOnly
                && !DbUtil.Db.OrganizationMembers.Any(om =>
                    om.OrganizationId == organization.OrganizationId
                    && om.PeopleId == Util.UserPeopleId))
            {
                DbUtil.LogActivity("Trying to view Organization ({0})".Fmt(organization.FullName));
                Response.EndShowMessage("You must be a member of this organization to have access to this page");
            }
            if (!Page.IsPostBack)
                DbUtil.LogActivity("Viewing Organization ({0})".Fmt(organization.FullName));

            if (TagString.Editing)
            {
                TagString.DataSource = organization.TagPickList();
                TagString.DataBindList();
            }
            Util.CurrentOrgId = organization.OrganizationId;
            Session["ActiveOrganization"] = organization.OrganizationName;
            EditUpdateButton1.DataBind();
            RecentAttendRpt.NavigateUrl = "~/Reports/PastAttendeeRpt.aspx?id=" + organization.OrganizationId;
            ExportToolBar1.TaggedEvent += new EventHandler(ExportToolBar1_TaggedEvent);
            ExportToolBar2.TaggedEvent += new EventHandler(ExportToolBar2_TaggedEvent);
            ExportToolBar3.TaggedEvent += new EventHandler(ExportToolBar3_TaggedEvent);
            MemberGrid1.RebindMemberGrids += new EventHandler(MemberGrid_RebindMemberGrids);
            MemberGrid2.RebindMemberGrids += new EventHandler(MemberGrid_RebindMemberGrids);
            VisitorGrid1.RebindMemberGrids += new EventHandler(MemberGrid_RebindMemberGrids);
            MemberGrid1.OrgId = organization.OrganizationId;
            MemberGrid2.OrgId = organization.OrganizationId;
            CloneOrg1.Visible = User.IsInRole("Edit");
            RollsheetRpt.Visible = User.IsInRole("Attendance");
            NewMeetingLink.Visible = User.IsInRole("Attendance");
            DeleteOrg.Visible = User.IsInRole("OrgTagger");
        }

        void MemberGrid_RebindMemberGrids(object sender, EventArgs e)
        {
            MemberGrid1.DataBind();
            MemberGrid2.DataBind();
            VisitorGrid1.DataBind();
        }

        void ExportToolBar2_TaggedEvent(object sender, EventArgs e)
        {
            VisitorGrid1.DataBind();
        }

        void ExportToolBar3_TaggedEvent(object sender, EventArgs e)
        {
            MemberGrid2.DataBind();
        }

        void ExportToolBar1_TaggedEvent(object sender, EventArgs e)
        {
            MemberGrid1.DataBind();
        }

        protected void EditUpdateButton1_Click(object sender, EventArgs e)
        {
            if (EditUpdateButton1.Updating)
            {
                if (NewTag.Text.HasValue())
                    organization.TagString += ";" + NewTag.Text;
                DbUtil.Db.SubmitChanges();
            }
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            NewTagRow.Visible = EditUpdateButton1.Editing;
            VisitLookbackDays.Text = Util.VisitLookbackDays.ToString();
            if (organization.ScheduleId != null)
            {
                var d = Util.Now.Date;
                d = d.AddDays(-(int)d.DayOfWeek); // prev sunday
                d = d.AddDays((int)organization.WeeklySchedule.Day);
                if (d > Util.Now.Date)
                    d = d.AddDays(-7);
                NewMeetingDate.Text = MeetingDate.Text = d.ToShortDateString();
                NewMeetingTime.Text = MeetingTime.Text = organization.WeeklySchedule.MeetingTime.ToShortTimeString();
            }
            else
            {
                NewMeetingDate.Text = MeetingDate.Text = Util.Now.Date.ToShortDateString();
                NewMeetingTime.Text = MeetingTime.Text = "8:00 AM";
            }
        }

        protected void ShowMeetings_Click(object sender, EventArgs e)
        {
            MeetingGrid1.Visible = true;
            MeetingGrid1.DefaultSort();
            MeetingsPanel.Update();
        }

        [System.Web.Services.WebMethod]
        public static string ToggleTag(int PeopleId, string controlid)
        {
            return MyTags.ToggleTag(PeopleId, controlid);
        }

        protected void SetDays_Click(object sender, EventArgs e)
        {
            Util.VisitLookbackDays = VisitLookbackDays.Text.ToInt();
            VisitorGrid1.DataBind();
            UpdatePanel2.Update();
        }

        protected void CreateMeeting(object sender, EventArgs e)
        {
            DateTime dt;
            if (!DateTime.TryParse(NewMeetingDate.Text + " " + NewMeetingTime.Text, out dt))
                return;
            var mt = DbUtil.Db.Meetings.SingleOrDefault(m => m.MeetingDate == dt
                    && m.OrganizationId == organization.OrganizationId);
            
            if (mt != null)
                return;

            mt = new CmsData.Meeting
            {
                CreatedDate = DateTime.Now,
                CreatedBy = DbUtil.Db.CurrentUser.UserId,
                OrganizationId = organization.OrganizationId,
                GroupMeetingFlag = false,
                Location = organization.Location,
                MeetingDate = dt,
            };
            DbUtil.Db.Meetings.InsertOnSubmit(mt);
            DbUtil.Db.SubmitChanges();
            DbUtil.LogActivity("Creating new meeting for {0}".Fmt(dt));
            Response.Redirect("~/Meeting.aspx?edit=1&id=" + mt.MeetingId);
            return;
        }

        protected void CreateGroupMeeting(object sender, EventArgs e)
        {
            DateTime dt;
            if (!DateTime.TryParse(NewMeetingDate.Text + " " + NewMeetingTime.Text, out dt))
                return;

            var mt = DbUtil.Db.Meetings.SingleOrDefault(m => m.MeetingDate == dt
                                                                  && m.OrganizationId == organization.OrganizationId);
            if (mt != null)
                return;

            mt = new CmsData.Meeting
            {
                CreatedDate = DateTime.Now,
                CreatedBy = DbUtil.Db.CurrentUser.UserId,
                OrganizationId = organization.OrganizationId,
                GroupMeetingFlag = true,
                Location = organization.Location,
                MeetingDate = dt,
            };
            DbUtil.Db.Meetings.InsertOnSubmit(mt);
            DbUtil.Db.SubmitChanges();
            DbUtil.LogActivity("Creating new group meeting for {0}".Fmt(dt));
            Response.Redirect("~/Meeting.aspx?edit=1&id=" + mt.MeetingId);
        }

        protected void CloneOrg_Click(object sender, EventArgs e)
        {
            var oc = new OrganizationController();
            var neworg = oc.CloneOrg(organization.OrganizationId);
            DbUtil.LogActivity("Cloning new org from {0}".Fmt(organization.FullName));
            Response.Redirect("~/Organization.aspx?id=" + neworg.OrganizationId);
        }

        protected void DeleteOrg_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            var oc = new OrganizationController();
            if (!organization.PurgeOrg())
            {
                ValidateDelete.IsValid = false;
                return;
            }
            Util.CurrentOrgId = 0;
            Response.EndShowMessage("Organization Deleted", "/", "click here");
        }
    }
}
