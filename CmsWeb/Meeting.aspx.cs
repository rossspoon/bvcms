/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Linq;
using UtilityExtensions;
using System.Web.UI.WebControls;
using CmsData;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Collections.Generic;
using System.Text;
using CMSPresenter.InfoClasses;
using System.Web;
using System.Web.Security;
using CMSPresenter;
using System.Web.UI;
using System.ComponentModel;
using System.Web.UI.HtmlControls;
using CustomControls;
using System.Data.Linq;
using System.Net.Mail;
//using System.Transactions;

namespace CMSWeb
{
	public partial class Meeting : System.Web.UI.Page
	{
		public CmsData.Meeting meeting;

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

		}
		public int pagesize { get { return Util.GetPageSizeCookie(); } }
		protected void Page_Load(object sender, EventArgs e)
		{
			var site = (CMSWeb.Site)Page.Master;
			site.ScriptManager.EnablePageMethods = true;

			int? id = this.QueryString<int?>("id");
			meeting = DbUtil.Db.Meetings.SingleOrDefault(m => m.MeetingId == id);
			if (meeting == null)
				Response.EndShowMessage("no meeting");

			if (Util.OrgMembersOnly
				&& !DbUtil.Db.OrganizationMembers.Any(om =>
					om.OrganizationId == meeting.OrganizationId
					&& om.PeopleId == Util.UserPeopleId))
				Response.EndShowMessage("You must be a member of this organization to have access to this page");

			if (!IsPostBack)
				DbUtil.LogActivity("Viewing Meeting for {0}".Fmt(meeting.MeetingDate));
			if (!Page.IsPostBack && this.QueryString<int?>("edit").HasValue)
				EditUpdateButton1.Editing = true;
			if (Page.IsPostBack)
				if (Page.Request.Params["__EVENTTARGET"] == "CreateMeeting")
					CreateMeeting(Page.Request.Params["__EVENTARGUMENT"]);

			EditUpdateButton1.DataBind();
			if (EditUpdateButton1.Editing)
				EditUpdateButton1.OnClientClick = "$.blockUI()";

			UpdateFieldVisibility();
			MeetingAttendanceLink.NavigateUrl = "~/Report/MeetingAttendanceRpt.aspx?mtgid={0}".Fmt(meeting.MeetingId);
			MeetingSummaryLink.NavigateUrl = "~/Report/MeetingSummaryRpt.aspx?mtgid={0}".Fmt(meeting.MeetingId);
			TextBox1.Focus();
            rollsheetlink.NavigateUrl = "~/Report/Rollsheet.aspx?meetingid={0}".Fmt(meeting.MeetingId);
		}

		protected void UpdateFieldVisibility()
		{
			AddVisitorLink.Visible = User.IsInRole("Attendance");
			AddVisitorLink.NavigateUrl = "~/Dialog/AddVisitor.aspx?id={0}&TB_iframe=true&height=450&width=600".Fmt(meeting.MeetingId);

			TR_NumMembers.Visible = !meeting.GroupMeetingFlag;
			NumMembers.Visible = !meeting.GroupMeetingFlag;

			NumPresent.BindingMode = meeting.GroupMeetingFlag ? BindingModes.TwoWay : BindingModes.OneWay;
			EditUpdateButton1.Enabled = User.IsInRole("Attendance");
			ShowAttendanceFlag1.Value = EditUpdateButton1.Editing.ToString();
		}

		protected void EditUpdateButton1_Click(object sender, EventArgs e)
		{
			if (EditUpdateButton1.Updating)
			{
				var rollsheet = AttendController.RollSheetAttendees(meeting.MeetingId).ToList();
				var attended = from i in ListView1.Items
							   let ck = i.FindControl("ck") as CheckBox
							   where ck.Checked
							   select ListView1.DataKeys[i.DisplayIndex].Value.ToInt();

				var q = from r in rollsheet
						join pid in attended on r.PeopleId equals pid into j
						from pid in j.DefaultIfEmpty()
						where ((r.AttendFlag == true) != (pid > 0)) || (r.isMember && r.noRecord) // different or needs a record
						select new
						{
							r.PeopleId,
							attended = pid > 0,
						};
				var qlist = q.ToList();

				var errors = new List<string>();
				foreach (var a in qlist)
				{
					var s = Attend.RecordAttendance(a.PeopleId, meeting.MeetingId, a.attended);
					if (s.HasValue())
						errors.Add(s);
				}
				if (errors.Count > 0)
				{
					var sb = new StringBuilder();
					foreach (var s in errors)
						sb.AppendLine(s + "<br/>");
					AlreadyAttendErrors.Text = sb.ToString();
					AlreadyAttendErrors.Visible = true;
				}
        		DbUtil.Db.SubmitChanges();
				DbUtil.Db.UpdateMeetingCounters(meeting.MeetingId);
				DbUtil.Db.Refresh(RefreshMode.OverwriteCurrentValues, meeting);
				if (meeting.NumPresent > 1000)
					DbUtil.LogActivity("MEETING COUNT {0}".Fmt(meeting.NumPresent));
				DbUtil.LogActivity("Updating Meeting for {0}".Fmt(meeting.MeetingDate));
				ListView1.DataBind();
				EditUpdateButton1.DataBind();
			}
			else
			{
				var q2 = from p in DbUtil.Db.People
						 where p.Attends.Any(a => a.OrganizationId == meeting.OrganizationId && a.MeetingId == meeting.MeetingId && a.AttendanceFlag == true)
						 select p;
				DbUtil.Db.PopulateSpecialTag(q2, DbUtil.TagTypeId_AddSelected);
				ListView1.DataBind();
			}
		}

		protected void AttendData_Selected(object sender, ObjectDataSourceStatusEventArgs e)
		{
			if (e.ReturnValue is int)
				GridCount.Text = e.ReturnValue.ToString();
		}

		protected void AddedSelectedVisitors_Click(object sender, EventArgs e)
		{
			ListView1.DataBind();
			DbUtil.Db.UpdateMeetingCounters(meeting.MeetingId);
			DbUtil.Db.Refresh(RefreshMode.OverwriteCurrentValues, meeting);
			EditUpdateButton1.DataBind();
		}

		private void CreateMeeting(string meetingcode)
		{
			var a = meetingcode.SplitStr(".");
			var orgid = a[1].ToInt();
			var organization = DbUtil.Db.LoadOrganizationById(orgid);
			if (organization == null)
				throw new Exception("Could not CreateMeeting with orgid: " + orgid.ToString());
			var dt = new DateTime(
				a[2].Substring(4, 2).ToInt() + 2000,
				a[2].Substring(0, 2).ToInt(),
				a[2].Substring(2, 2).ToInt(),
				a[2].Substring(6, 2).ToInt(),
				a[2].Substring(8, 2).ToInt(),
				0);
			var newMtg = DbUtil.Db.Meetings.SingleOrDefault(m => m.OrganizationId == orgid && m.MeetingDate == dt);
			if (newMtg == null)
			{
                newMtg = new CmsData.Meeting
				{
					CreatedDate = Util.Now,
                    CreatedBy = Util.UserId1,
					OrganizationId = orgid,
					GroupMeetingFlag = false,
					Location = organization.Location,
					MeetingDate = dt,
				};
				DbUtil.Db.Meetings.InsertOnSubmit(newMtg);
				DbUtil.Db.SubmitChanges();
				DbUtil.LogActivity("Created new meeting for {0}".Fmt(dt));
			}
			Response.Redirect("~/Meeting.aspx?edit=1&id=" + newMtg.MeetingId);
		}
        protected void ChangeMeetingDate(object sender, EventArgs e)
        {
            DateTime dt;
            if (!DateTime.TryParse(MeetingDate.Text + " " + MeetingTime.Text, out dt))
                return;

            foreach (var a in meeting.Attends)
                a.MeetingDate = dt;
            meeting.MeetingDate = dt;
            DbUtil.Db.SubmitChanges();
        }
    }
}
