using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;
using UtilityExtensions;
using System.Text;
using CmsWeb.Models.OrganizationPage;
using System.Diagnostics;
using CmsData.Codes;

namespace CmsWeb.Areas.Main.Controllers
{
    [ValidateInput(false)]
	[SessionExpire]
    public class OrganizationController : CmsStaffController
    {
		const string needNotify = "WARNING: please add the notify persons on messages tab.";
        public ActionResult Index(int? id)
        {
            if (!id.HasValue)
                return Content("no org");
            if (Util2.CurrentOrgId != id)
                Util2.CurrentGroups = null;

            var m = new OrganizationModel(id.Value, Util2.CurrentGroups);

            if (m.org == null)
                return Content("organization not found");

            if (Util2.OrgMembersOnly)
            {
                if (m.org.SecurityTypeId == 3)
                    return NotAllowed("You do not have access to this page", m.org.OrganizationName);
                else if (!m.org.OrganizationMembers.Any(om => om.PeopleId == Util.UserPeopleId))
                    return NotAllowed("You must be a member of this organization", m.org.OrganizationName);
            }
            else if (Util2.OrgLeadersOnly)
            {
                var oids = DbUtil.Db.GetLeaderOrgIds(Util.UserPeopleId);
                if (!oids.Contains(m.org.OrganizationId))
                    return NotAllowed("You must be a leader of this organization", m.org.OrganizationName);
            }
			if (m.org.LimitToRole.HasValue())
				if (!User.IsInRole(m.org.LimitToRole))
                    return NotAllowed("no privledge to view ", m.org.OrganizationName);

            DbUtil.LogActivity("Viewing Organization ({0})".Fmt(m.org.OrganizationName));

            Util2.CurrentOrgId = m.org.OrganizationId;
            ViewData["OrganizationContext"] = true;
            var qb = DbUtil.Db.QueryBuilderInCurrentOrg();
            InitExportToolbar(id.Value, qb.QueryId);
            Session["ActiveOrganization"] = m.org.OrganizationName;
            return View(m);
        }
        private ActionResult NotAllowed(string error, string name)
        {
            DbUtil.LogActivity("Trying to view Organization ({0})".Fmt(name));
            return Content("<h3 style='color:red'>{0}</h3>\n<a href='{1}'>{2}</a>"
                                    .Fmt(error, "javascript: history.go(-1)", "Go Back"));
        }
        [Authorize(Roles = "Admin")]

        public ActionResult Delete(int id)
        {
            var org = DbUtil.Db.LoadOrganizationById(id);
            if (org == null)
                return Content("error, bad orgid");
            if (id == 1)
                return Content("Cannot delete first org");
            if (!org.PurgeOrg(DbUtil.Db))
                return Content("error, not deleted");
            Util2.CurrentOrgId = 0;
            Util2.CurrentGroups = null;
			DbUtil.LogActivity("Delete Org {0}".Fmt(Session["ActiveOrganization"]));
            Session.Remove("ActiveOrganization");
            return Content("ok");
        }

        [HttpPost]
        public ActionResult NewMeeting(string d, string t, int AttendCredit, bool group)
        {
            var organization = DbUtil.Db.LoadOrganizationById(Util2.CurrentOrgId);
            if (organization == null)
                return Content("error: no org");
            DateTime dt;
            if (!DateTime.TryParse(d + " " + t, out dt))
                return Content("error: bad date");
            var mt = DbUtil.Db.Meetings.SingleOrDefault(m => m.MeetingDate == dt
                    && m.OrganizationId == organization.OrganizationId);

            if (mt != null)
                return Content("/Meeting/Index/" + mt.MeetingId);

            mt = new CmsData.Meeting
            {
                CreatedDate = Util.Now,
                CreatedBy = Util.UserId1,
                OrganizationId = organization.OrganizationId,
                GroupMeetingFlag = group,
                Location = organization.Location,
                MeetingDate = dt,
                AttendCreditId = AttendCredit
            };
            DbUtil.Db.Meetings.InsertOnSubmit(mt);
            DbUtil.Db.SubmitChanges();
            DbUtil.LogActivity("Creating new meeting for {0}".Fmt(organization.OrganizationName));
            return Content("/Meeting/Index/" + mt.MeetingId);
        }
        private void InitExportToolbar(int oid, int qid)
        {
            Util2.CurrentOrgId = oid;
            ViewData["queryid"] = qid;
            ViewData["TagAction"] = "/Organization/TagAll/{0}?m=tag".Fmt(qid);
            ViewData["UnTagAction"] = "/Organization/TagAll/{0}?m=untag".Fmt(qid);
            ViewData["AddContact"] = "/Organization/AddContact/" + qid;
            ViewData["AddTasks"] = "/Organization/AddTasks/" + qid;
            ViewData["OrganizationContext"] = true;
        }

        public ActionResult CurrMemberGrid(int id, int[] smallgrouplist, string namefilter)
        {
            ViewData["OrgMemberContext"] = true;
            Util2.CurrentGroups = smallgrouplist;
            var qb = DbUtil.Db.QueryBuilderInCurrentOrg();
            InitExportToolbar(id, qb.QueryId);
            var m = new MemberModel(id, Util2.CurrentGroups, MemberModel.GroupSelect.Active, namefilter);
            UpdateModel(m.Pager);
            return View(m);
        }
        [HttpPost]
        public ActionResult PrevMemberGrid(int id, string namefilter)
        {
            var qb = DbUtil.Db.QueryBuilderPreviousCurrentOrg();
            InitExportToolbar(id, qb.QueryId);
            var m = new PrevMemberModel(id, namefilter);
            UpdateModel(m.Pager);
			DbUtil.LogActivity("Viewing Prev Members for {0}".Fmt(Session["ActiveOrganization"]));
            return View(m);
        }
        [HttpPost]
        public ActionResult VisitorGrid(int id, string namefilter)
        {
            var qb = DbUtil.Db.QueryBuilderVisitedCurrentOrg();
            InitExportToolbar(id, qb.QueryId);
            var m = new VisitorModel(id, qb.QueryId, namefilter);
            UpdateModel(m.Pager);
			DbUtil.LogActivity("Viewing Visitors for {0}".Fmt(Session["ActiveOrganization"]));
            return View("VisitorGrid", m);
        }
        [HttpPost]
        public ActionResult PendingMemberGrid(int id, string namefilter)
        {
            var qb = DbUtil.Db.QueryBuilderPendingCurrentOrg();
            InitExportToolbar(id, qb.QueryId);
            var m = new MemberModel(id, null, MemberModel.GroupSelect.Pending, namefilter);
            UpdateModel(m.Pager);
            return View(m);
        }
        [HttpPost]
        public ActionResult InactiveMemberGrid(int id, string namefilter)
        {
            var qb = DbUtil.Db.QueryBuilderInactiveCurrentOrg();
            InitExportToolbar(id, qb.QueryId);
            var m = new MemberModel(id, null, MemberModel.GroupSelect.Inactive, namefilter);
            UpdateModel(m.Pager);
			DbUtil.LogActivity("Viewing Inactive for {0}".Fmt(Session["ActiveOrganization"]));
            return View(m);
        }
        [HttpPost]
        public ActionResult MeetingGrid(int id, bool future)
        {
            var m = new MeetingModel(id, future);
            UpdateModel(m.Pager);
			DbUtil.LogActivity("Viewing Meetings for {0}".Fmt(Session["ActiveOrganization"]));
            return View(m);
        }
        
        [HttpPost]
        public ActionResult SettingsOrg(int id)
        {
            var m = new OrganizationModel(id, Util2.CurrentGroups);
            return View(m);
        }
        [HttpPost]
        public ActionResult SettingsOrgEdit(int id)
        {
            var m = new OrganizationModel(id, Util2.CurrentGroups);
            return View(m);
        }
        [HttpPost]
        public ActionResult SettingsOrgUpdate(int id)
        {
            var m = new OrganizationModel(id, Util2.CurrentGroups);
            UpdateModel(m);
			if (!m.org.LimitToRole.HasValue())
				m.org.LimitToRole = null;
			DbUtil.LogActivity("Update SettingsOrg {0}".Fmt(m.org.OrganizationName));
            if (ModelState.IsValid)
            {
                m.UpdateSchedules();
                DbUtil.Db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, m.org.OrgSchedules);
                return View("SettingsOrg", m);
            }
            return View("SettingsOrgEdit", m);
        }
        
        [HttpPost]
        public ActionResult SettingsMeetings(int id)
        {
            var m = new OrganizationModel(id, Util2.CurrentGroups);
            return View(m);
        }
        [HttpPost]
        public ActionResult SettingsMeetingsEdit(int id)
        {
            var m = new OrganizationModel(id, Util2.CurrentGroups);
            return View(m);
        }
        [HttpPost]
        public ActionResult SettingsMeetingsUpdate(int id)
        {
            var m = new OrganizationModel(id, Util2.CurrentGroups);
            m.schedules.Clear();

            UpdateModel(m);
            m.UpdateSchedules();
            DbUtil.Db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, m.org.OrgSchedules);
			DbUtil.LogActivity("Update SettingsMeetings {0}".Fmt(m.org.OrganizationName));
            return View("SettingsMeetings", m);
            //return View("SettingsMeetingsEdit", m);
        }

        [HttpPost]
        public ActionResult NewSchedule()
        {
            var s = new ScheduleInfo(
                new OrgSchedule
                {
                    SchedDay = 0,
                    SchedTime = DateTime.Parse("8:00 AM"),
                    AttendCreditId = 1
                });
            return View("EditorTemplates/ScheduleInfo", s);
        }

        [HttpPost]
        public ActionResult OrgInfo(int id)
        {
            var m = new OrganizationModel(id, Util2.CurrentGroups);
            return View(m);
        }
        [HttpPost]
        public ActionResult OrgInfoEdit(int id)
        {
            var m = new OrganizationModel(id, Util2.CurrentGroups);
            return View(m);
        }
        [HttpPost]
        public ActionResult OrgInfoUpdate(int id)
        {
            var m = new OrganizationModel(id, Util2.CurrentGroups);
            UpdateModel(m);
            if (m.org.CampusId == 0)
                m.org.CampusId = null;
            if (m.org.OrganizationTypeId == 0)
                m.org.OrganizationTypeId = null;
            DbUtil.Db.SubmitChanges();
			DbUtil.LogActivity("Update OrgInfo {0}".Fmt(m.org.OrganizationName));
            return View("OrgInfo", m);
        }

        private static RegSettings GetRegSettings(int id)
        {
            var org = DbUtil.Db.LoadOrganizationById(id);
            var m = new RegSettings(org.RegSetting, DbUtil.Db, id);
            m.org = org;
            return m;
        }
        [HttpPost]
        public ActionResult OnlineRegAdmin(int id)
        {
            return View(GetRegSettings(id));
        }
        [HttpPost]
        [Authorize(Roles="Edit")]
        public ActionResult OnlineRegAdminEdit(int id)
        {
            return View(GetRegSettings(id));
        }
        [HttpPost]
        public ActionResult OnlineRegAdminUpdate(int id)
        {
            var m = GetRegSettings(id);
            m.AgeGroups.Clear();
            m.GradeOptions.Clear();
			DbUtil.LogActivity("Update OnlineRegAdmin {0}".Fmt(m.org.OrganizationName));
            try
            {
                UpdateModel(m);
				if (m.org.OrgPickList.HasValue() && m.org.RegistrationTypeId == RegistrationTypeCode.JoinOrganization)
					m.org.OrgPickList = null;

                var os = new RegSettings(m.ToString(), DbUtil.Db, id, check: true);
                m.org.RegSetting = os.ToString();
                DbUtil.Db.SubmitChanges();
				if (!m.org.NotifyIds.HasValue())
					ModelState.AddModelError("Form", needNotify);
                return View("OnlineRegAdmin", m);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Form", ex.Message);
                return View("OnlineRegAdminEdit", m);
            }
        }

        [HttpPost]
        public ActionResult OnlineRegOptions(int id)
        {
            return View(GetRegSettings(id));
        }
        [HttpPost]
        [Authorize(Roles="Edit")]
        public ActionResult OnlineRegOptionsEdit(int id)
        {
            return View(GetRegSettings(id));
        }
        [HttpPost]
        public ActionResult OnlineRegOptionsUpdate(int id)
        {
            var m = GetRegSettings(id);
            try
            {
				DbUtil.LogActivity("Update OnlineRegOptions {0}".Fmt(m.org.OrganizationName));
                UpdateModel(m);
                var os = new RegSettings(m.ToString(), DbUtil.Db, id);
                m.org.RegSetting = os.ToString();
                DbUtil.Db.SubmitChanges();
				if (!m.org.NotifyIds.HasValue())
					ModelState.AddModelError("Form", needNotify);
                return View("OnlineRegOptions", m);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Form", ex.Message);
                return View("OnlineRegOptionsEdit", m);
            }
        }

        [HttpPost]
        public ActionResult OnlineRegQuestions(int id)
        {
            return View(GetRegSettings(id));
        }
        [HttpPost]
        [Authorize(Roles = "Edit")]
        public ActionResult OnlineRegQuestionsEdit(int id)
        {
            return View(GetRegSettings(id));
        }
        [HttpPost]
        public ActionResult OnlineRegQuestionsUpdate(int id)
        {
            var m = GetRegSettings(id);
			DbUtil.LogActivity("Update OnlineRegQuestions {0}".Fmt(m.org.OrganizationName));
            m.YesNoQuestions.Clear();
            m.ExtraQuestions.Clear();
            m.ShirtSizes.Clear();
            m.TimeSlots.Clear();
            m.MenuItems.Clear();
            m.Dropdown1.Clear();
            m.Dropdown2.Clear();
            m.Dropdown3.Clear();
            m.Checkboxes.Clear();
            m.Checkboxes2.Clear();
            try
            {
                UpdateModel(m);
                var os = new RegSettings(m.ToString(), DbUtil.Db, id, check: true);
                m.org.RegSetting = os.ToString();
                DbUtil.Db.SubmitChanges();
				if (!m.org.NotifyIds.HasValue())
					ModelState.AddModelError("Form", needNotify);
                return View("OnlineRegQuestions", m);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Form", ex.Message);
                return View("OnlineRegQuestionsEdit", m);
            }
        }

        [HttpPost]
        public ActionResult OnlineRegFees(int id)
        {
            return View(GetRegSettings(id));
        }
        [HttpPost]
        [Authorize(Roles="Edit")]
        public ActionResult OnlineRegFeesEdit(int id)
        {
            return View(GetRegSettings(id));
        }
        [HttpPost]
        public ActionResult OnlineRegFeesUpdate(int id)
        {
            var m = GetRegSettings(id);
            m.OrgFees.Clear();
            try
            {
				DbUtil.LogActivity("Update OnlineRegFees {0}".Fmt(m.org.OrganizationName));
                UpdateModel(m);
                var os = new RegSettings(m.ToString(), DbUtil.Db, id);
                m.org.RegSetting = os.ToString();
                DbUtil.Db.SubmitChanges();
				if (!m.org.NotifyIds.HasValue())
					ModelState.AddModelError("Form", needNotify);
                return View("OnlineRegFees", m);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Form", ex.Message);
                return View("OnlineRegFeesEdit", m);
            }
        }

        [HttpPost]
        public ActionResult OnlineRegMessages(int id)
        {
            return View(GetRegSettings(id));
        }
        [HttpPost]
        [Authorize(Roles="Edit")]
        public ActionResult OnlineRegMessagesEdit(int id)
        {
            return View(GetRegSettings(id));
        }
        [HttpPost]
        public ActionResult OnlineRegMessagesUpdate(int id)
        {
            var m = GetRegSettings(id);
			DbUtil.LogActivity("Update OnlineRegMessages {0}".Fmt(m.org.OrganizationName));
            m.VoteTags.Clear();
            try
            {
                UpdateModel(m);
                var os = new RegSettings(m.ToString(), DbUtil.Db, id);
                m.org.RegSetting = os.ToString();
                DbUtil.Db.SubmitChanges();
				if (!m.org.NotifyIds.HasValue())
					ModelState.AddModelError("Form", needNotify);
                return View("OnlineRegMessages", m);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Form", ex.Message);
                return View("OnlineRegMessagesEdit", m);
            }
        }

        [HttpPost]
        public ActionResult NewTimeSlot()
        {
            return View("EditorTemplates/TimeSlot", new RegSettings.TimeSlot());
        }

        [HttpPost]
        public ActionResult NewMenuItem()
        {
            return View("EditorTemplates/MenuItem", new RegSettings.MenuItem());
        }
        [HttpPost]
        public ActionResult NewDropdown1Item()
        {
            return View("EditorTemplates/Dropdown1", new RegSettings.MenuItem());
        }
        [HttpPost]
        public ActionResult NewDropdown2Item()
        {
            return View("EditorTemplates/Dropdown2", new RegSettings.MenuItem());
        }
        [HttpPost]
        public ActionResult NewDropdown3Item()
        {
            return View("EditorTemplates/Dropdown3", new RegSettings.MenuItem());
        }
        [HttpPost]
        public ActionResult NewCheckbox()
        {
            return View("EditorTemplates/Checkbox", new RegSettings.MenuItem());
        }
        [HttpPost]
        public ActionResult NewCheckbox2()
        {
            return View("EditorTemplates/Checkbox2", new RegSettings.MenuItem());
        }
        [HttpPost]
        public ActionResult NewOrgFee()
        {
            return View("EditorTemplates/OrgFee", new RegSettings.OrgFee());
        }
        [HttpPost]
        public ActionResult NewAgeGroup()
        {
            return View("EditorTemplates/AgeGroup", new RegSettings.AgeGroup());
        }
        [HttpPost]
        public ActionResult NewGradeOption()
        {
            return View("EditorTemplates/GradeOption", new RegSettings.GradeOption());
        }
        [HttpPost]
        public ActionResult NewYesNoQuestion()
        {
            return View("EditorTemplates/YesNoQuestion", new RegSettings.YesNoQuestion());
        }
        [HttpPost]
        public ActionResult NewShirtSize()
        {
            return View("EditorTemplates/ShirtSize", new RegSettings.ShirtSize());
        }
        [HttpPost]
        public ActionResult NewExtraQuestion()
        {
            return View("EditorTemplates/ExtraQuestion", new RegSettings.ExtraQuestion());
        }
        [HttpPost]
        public ActionResult NewVoteTag()
        {
            return View("EditorTemplates/VoteTag", new RegSettings.VoteTag());
        }
        public ActionResult VoteTag(int id)
        {
            var org = DbUtil.Db.LoadOrganizationById(id);
            RegSettings m = new RegSettings(org.RegSetting, DbUtil.Db, id);
            Response.ContentType = "text/plain";
            return Content(@"Copy and paste these directly into your email text, 
no need to put these into the ""Source"" view of the editor anymore.

" + m.VoteTagsLinks());
        }
        
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SmallGroups()
        {
            var m = new OrganizationModel(Util2.CurrentOrgId, Util2.CurrentGroups);
            return View(m);
        }
        [Authorize(Roles = "Edit")]
        public ActionResult CopySettings()
        {
            if (Util.SessionTimedOut() || Util2.CurrentOrgId == 0)
                return Redirect("/");
            Session["OrgCopySettings"] = Util2.CurrentOrgId;
            return Redirect("/OrgSearch/");
        }
        [HttpPost]
        public ActionResult Join(string id)
        {
            var aa = id.Split('.');
            if (aa.Length != 3)
                return Content("error: bad info");
            var pid = aa[1].ToInt();
            var oid = aa[2].ToInt();
            var org = DbUtil.Db.LoadOrganizationById(oid);
            if (org.AllowAttendOverlap != true)
            {
                var om = DbUtil.Db.OrganizationMembers.FirstOrDefault(mm => 
                    mm.OrganizationId != oid
                    && mm.MemberTypeId != 230 // inactive
                    && mm.MemberTypeId != 500 // inservice
                    && mm.Organization.AllowAttendOverlap != true
                    && mm.PeopleId == pid
                    && mm.Organization.OrgSchedules.Any(ss => 
                        DbUtil.Db.OrgSchedules.Any(os => 
                            os.OrganizationId == oid 
                            && os.ScheduleId == ss.ScheduleId)));
				if (om != null)
				{
					DbUtil.LogActivity("Same Hour Joining Org {0}({1})".Fmt(org.OrganizationName, pid));
					return Content("Already a member of {0} at this hour".Fmt(om.OrganizationId));
				}
            }
            OrganizationMember.InsertOrgMembers(DbUtil.Db,
                oid, pid, MemberTypeCode.Member,
                DateTime.Now, null, false);
			DbUtil.Db.UpdateMainFellowship(oid);
			DbUtil.LogActivity("Joining Org {0}({1})".Fmt(org.OrganizationName, pid));
            return Content("ok");
        }

        [HttpPost]
        public ActionResult ToggleTag(int id)
        {
            var t = Person.ToggleTag(id, Util2.CurrentTagName, Util2.CurrentTagOwnerId, DbUtil.TagTypeId_Personal);
            DbUtil.Db.SubmitChanges();
            return Content(t ? "Remove" : "Add");
        }
        [HttpPost]
        public ContentResult TagAll(int id, string m)
        {
            DbUtil.Db.SetNoLock();
            var q = DbUtil.Db.PeopleQuery(id);
            switch (m)
            {
                case "tag":
                    DbUtil.Db.TagAll(q);
                    return Content("Remove");
                case "untag":
                    DbUtil.Db.UnTagAll(q);
                    return Content("Add");
            }
            return Content("?");
        }
        [HttpPost]
        public ActionResult AddContact(int id)
        {
            var cid = CmsData.Contact.AddContact(id);
            return Content("/Contact.aspx?id=" + cid);
        }
        [HttpPost]
        public ActionResult AddTasks(int id)
        {
            var c = new ContentResult();
            c.Content = Task.AddTasks(id).ToString();
            return c;
        }
        public ActionResult NotifyIds(int id)
        {
            if (Util.SessionTimedOut())
                return Content("<script type='text/javascript'>window.onload = function() { parent.location = '/'; }</script>");
            Response.NoCache();
            var t = DbUtil.Db.FetchOrCreateTag(Util.SessionId, Util.UserPeopleId, DbUtil.TagTypeId_AddSelected);
            DbUtil.Db.TagPeople.DeleteAllOnSubmit(t.PersonTags);
            Util2.CurrentOrgId = id;
            DbUtil.Db.SubmitChanges();
            var o = DbUtil.Db.LoadOrganizationById(id);
            var q = DbUtil.Db.PeopleFromPidString(o.NotifyIds).Select(p => p.PeopleId);
            foreach (var pid in q)
                t.PersonTags.Add(new TagPerson { PeopleId = pid });
            DbUtil.Db.SubmitChanges();
            return Redirect("/SearchUsers?ordered=true&topid=" + q.FirstOrDefault());
        }
        public ActionResult OrgPickList(int id)
        {
            if (Util.SessionTimedOut())
                return Content("<script type='text/javascript'>window.onload = function() { parent.location = '/'; }</script>");
            Response.NoCache();
            Util2.CurrentOrgId = id;
            var o = DbUtil.Db.LoadOrganizationById(id);
            Session["orgPickList"] = (o.OrgPickList ?? "").Split(',').Select(oo => oo.ToInt()).ToList();
            return Redirect("/SearchOrgs/Index/" + id);
        }
        [HttpPost]
        public ActionResult UpdateNotifyIds(int id, int topid)
        {
            var t = DbUtil.Db.FetchOrCreateTag(Util.SessionId, Util.UserPeopleId, DbUtil.TagTypeId_AddSelected);
            var selected_pids = (from p in t.People(DbUtil.Db)
                                 orderby p.PeopleId == topid ? "0" : "1"
                                 select p.PeopleId).ToArray();
            var o = DbUtil.Db.LoadOrganizationById(Util2.CurrentOrgId);
            o.NotifyIds = string.Join(",", selected_pids);
            DbUtil.Db.TagPeople.DeleteAllOnSubmit(t.PersonTags);
            DbUtil.Db.Tags.DeleteOnSubmit(t);
            DbUtil.Db.SubmitChanges();
            var rs = new RegSettings(o.RegSetting, DbUtil.Db, id);
            rs.org = o;
            return View("NotifyList2", rs);
        }
        [HttpPost]
        public ActionResult UpdateOrgIds(int id, string list)
        {
            var o = DbUtil.Db.LoadOrganizationById(id);
            Util2.CurrentOrgId = id;
            var m = new RegSettings(o.RegSetting, DbUtil.Db, id);
            m.org = o;
            o.OrgPickList = list;
            DbUtil.Db.SubmitChanges();
            return View("OrgPickList2", m);
        }
        [HttpPost]
        [Authorize(Roles="Edit")]
        public ActionResult NewExtraValue(int id, string field, string value, bool multiline)
        {
            var m = new OrganizationModel(id, null);
            try
            {
				m.org.AddEditExtra(DbUtil.Db, field, value, multiline);
				DbUtil.Db.SubmitChanges();
            }
            catch (Exception ex)
            {
                return Content("error: " + ex.Message);
            }
            return View("ExtrasGrid", m.org);
        }
        [HttpPost]
        [Authorize(Roles="Edit")]
        public ViewResult DeleteExtra(int id, string field)
        {
            var e = DbUtil.Db.OrganizationExtras.Single(ee => ee.OrganizationId == id && ee.Field == field);
            DbUtil.Db.OrganizationExtras.DeleteOnSubmit(e);
            DbUtil.Db.SubmitChanges();
            var m = new OrganizationModel(id, null);
            return View("ExtrasGrid", m.org);
        }
        [HttpPost]
        [Authorize(Roles="Edit")]
        public ContentResult EditExtra(string id, string value)
        {
            var a = id.SplitStr("-", 2);
            var b = a[1].SplitStr(".", 2);
            var e = DbUtil.Db.OrganizationExtras.Single(ee => ee.OrganizationId == b[1].ToInt() && ee.Field == b[0]);
            e.Data = value;
            DbUtil.Db.SubmitChanges();
            return Content(value);
        }
		[Authorize(Roles = "Edit")]
		[HttpPost]
		public ActionResult Reminders(int id)
		{
            var m = new OrganizationModel(id, null);
			try
			{
				m.SendReminders();
			}
			catch (Exception ex)
			{
				return Content(ex.Message);
			}
			return Content("ok");
		}
    }
}