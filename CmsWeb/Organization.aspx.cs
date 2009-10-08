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
using System.Text;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CMSWeb
{
    public partial class Organization : System.Web.UI.Page
    {
        public CmsData.Organization organization;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            var qb = DbUtil.Db.QueryBuilderInCurrentOrg();
            MemberToolbar.queryId = qb.QueryId;
            MemberToolbar.OrganizationContext = true;
            qb = DbUtil.Db.QueryBuilderVisitedCurrentOrg();
            VisitorToolbar.queryId = qb.QueryId;
            qb = DbUtil.Db.QueryBuilderInactiveCurrentOrg();
            InactiveToolbar.queryId = qb.QueryId;
            InactiveToolbar.OrganizationContext = true;
            qb = DbUtil.Db.QueryBuilderPendingCurrentOrg();
            PendingToolbar.queryId = qb.QueryId;
            PendingToolbar.OrganizationContext = true;
            qb = DbUtil.Db.QueryBuilderPreviousCurrentOrg();
            PriorsToolbar.queryId = qb.QueryId;
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

            MultiDivisions.Visible = EditUpdateButton1.Editing;
            if (MultiDivisions.Visible)
            {
                TagStringOptions.DataSource = DivisionPickList(organization.OrganizationId);
                TagStringOptions.DataBind();
            }

            if (Util.CurrentOrgId != organization.OrganizationId)
                Util.CurrentGroupId = 0;
            Util.CurrentOrgId = organization.OrganizationId;
            Session["ActiveOrganization"] = organization.OrganizationName;
            EditUpdateButton1.DataBind();
            RecentAttendRpt.NavigateUrl = "~/Report/PastAttendeeRpt.aspx?id=" + organization.OrganizationId;
            MemberToolbar.TaggedEvent += new EventHandler(MemberToolbar_TaggedEvent);
            VisitorToolbar.TaggedEvent += new EventHandler(VisitorToolbar_TaggedEvent);
            InactiveToolbar.TaggedEvent += new EventHandler(InactiveToolbar_TaggedEvent);
            PendingToolbar.TaggedEvent += new EventHandler(PendingToolbar_TaggedEvent);
            PriorsToolbar.TaggedEvent += new EventHandler(PriorsToolbar_TaggedEvent);
            Members.RebindMemberGrids += new EventHandler(RebindGrids);
            Inactives.RebindMemberGrids += new EventHandler(RebindGrids);
            Visitors.RebindMemberGrids += new EventHandler(RebindGrids);
            Visitors.RebindMemberGrids += new EventHandler(RebindGrids);
            Members.OrgId = organization.OrganizationId;
            Members.GroupId = Util.CurrentGroupId;
            Inactives.OrgId = organization.OrganizationId;
            Pendings.OrgId = organization.OrganizationId;
            Priors.OrgId = organization.OrganizationId;
            CloneOrg1.Visible = User.IsInRole("Edit");
            NewMeetingLink.Visible = User.IsInRole("Attendance");
            DeleteOrg.Visible = User.IsInRole("OrgTagger");
            ManageGroups.Visible = User.IsInRole("ManageGroups");
        }

        void RebindGrids(object sender, EventArgs e)
        {
            Members.DataBind();
            Inactives.DataBind();
            Pendings.DataBind();
            Priors.DataBind();
            Visitors.DataBind();
        }

        void VisitorToolbar_TaggedEvent(object sender, EventArgs e)
        {
            Visitors.DataBind();
        }
        void PriorsToolbar_TaggedEvent(object sender, EventArgs e)
        {
            Priors.DataBind();
        }

        void InactiveToolbar_TaggedEvent(object sender, EventArgs e)
        {
            Inactives.DataBind();
        }

        void MemberToolbar_TaggedEvent(object sender, EventArgs e)
        {
            Members.DataBind();
        }
        void PendingToolbar_TaggedEvent(object sender, EventArgs e)
        {
            Pendings.DataBind();
        }

        protected void EditUpdateButton1_Click(object sender, EventArgs e)
        {
            if (EditUpdateButton1.Updating)
            {
                organization.TagString = Request.Form["TagString"];
                DbUtil.Db.SubmitChanges();
                var divorg = organization.DivOrgs.SingleOrDefault(d => d.DivId == organization.DivisionId);
                if (divorg == null && organization.DivisionId.HasValue)
                {
                    organization.DivOrgs.Add(new DivOrg { DivId = organization.DivisionId.Value });
                    DbUtil.Db.SubmitChanges();
                }
            }
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
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
            Visitors.DataBind();
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
                CreatedBy = Util.UserId1,
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
                CreatedBy = Util.UserId1,
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
            Util.CurrentGroupId = 0;
            Response.EndShowMessage("Organization Deleted", "/", "click here");
        }

        protected void DeleteGroup_Click(object sender, EventArgs e)
        {
            var group = DbUtil.Db.MemberTags.SingleOrDefault(g => g.Id == Groups.SelectedValue.ToInt());
            if (group == null)
                return;
            DbUtil.Db.OrgMemMemTags.DeleteAllOnSubmit(group.OrgMemMemTags);
            DbUtil.Db.MemberTags.DeleteOnSubmit(group);
            DbUtil.Db.SubmitChanges();
            Groups.DataBind();
            Members.DataBind();
            GroupFilter.DataBind();
        }

        protected void MakeNewGroup_Click(object sender, EventArgs e)
        {
            var Db = DbUtil.Db;
            var group = Db.MemberTags.SingleOrDefault(g =>
                g.Name == GroupName.Text && g.OrgId == organization.OrganizationId);
            if (group == null) // must be a new group
            {
                group = new MemberTag
                {
                    Name = GroupName.Text,
                    OrgId = organization.OrganizationId
                };
                Db.MemberTags.InsertOnSubmit(group);
                Db.SubmitChanges();
                Groups.DataBind();
                GroupFilter.DataBind();
                UpdateGroupsUrl();
            }
        }

        protected void RenameGroup_Click(object sender, EventArgs e)
        {
            var group = DbUtil.Db.MemberTags.Single(d => d.Id == Groups.SelectedValue.ToInt());
            group.Name = GroupName.Text;
            DbUtil.Db.SubmitChanges();
            Groups.DataBind();
            GroupFilter.DataBind();
        }

        [System.Web.Services.WebMethod]
        public static string ToggleGroup(int OrgId, int PeopleId, int groupid, string controlid)
        {
            if (groupid == 0)
                return "";
            var Db = DbUtil.Db;
            var member = Db.OrganizationMembers.SingleOrDefault(m =>
                m.PeopleId == PeopleId && m.OrganizationId == OrgId);
            var r = new ToggleTagReturn { ControlId = controlid };
            r.HasTag = member.ToggleGroup(groupid);
            Db.SubmitChanges();
            var jss = new DataContractJsonSerializer(typeof(ToggleTagReturn));
            var ms = new MemoryStream();
            jss.WriteObject(ms, r);
            return Encoding.Default.GetString(ms.ToArray());
        }

        protected void Group_SelectedIndexChanged(object sender, EventArgs e)
        {
            Util.CurrentGroupId = GroupFilter.SelectedValue.ToInt();
            Members.GroupId = Util.CurrentGroupId;
            Members.DataBind();
        }
        protected void Groups_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateGroupsUrl();
        }
        private void UpdateGroupsUrl()
        {
            UpdateGroup.NavigateUrl = "~/Dialog/EditMembers.aspx?id={0}&group={1}&TB_iframe=true&height=450&width=600"
                            .Fmt(organization.OrganizationId, Groups.SelectedValue);
            if (Groups.SelectedItem != null)
            {
                UpdateGroup.ToolTip = "Update Group Members: " + Groups.SelectedItem.Text;
                UpdateGroup.Enabled = true;
            }
            else
                UpdateGroup.Enabled = false;
        }

        protected void Groups_DataBound(object sender, EventArgs e)
        {
            UpdateGroupsUrl();
        }

        protected void GroupFilter_DataBound(object sender, EventArgs e)
        {
            var v = GroupFilter.Items.FindByValue(Util.CurrentGroupId.ToString());
            if (v == null)
                Util.CurrentGroupId = 0;
            GroupFilter.SelectedValue = Util.CurrentGroupId.ToString();
        }
        public List<SelectListItem> DivisionPickList(int OrganizationId)
        {
            var q1 = from d in DbUtil.Db.DivOrgs
                     where d.OrgId == OrganizationId
                     orderby d.Division.Name
                     select d.Division.Name;
            var q2 = from p in DbUtil.Db.Programs
                     from d in p.Divisions
                     where !q1.Contains(d.Name)
                     orderby d.Name
                     select d.Name;
            var list = q1.Select(name => new SelectListItem { Text = name, Selected = true }).ToList();
            list.AddRange(q2.Select(name => new SelectListItem { Text = name }).ToList());
            return list;
        }
    }
}
