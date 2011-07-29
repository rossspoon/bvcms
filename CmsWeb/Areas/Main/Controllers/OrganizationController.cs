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
    public class OrganizationController : CmsStaffController
    {
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
                if (m.org.SecurityTypeId == 3)
                    return NotAllowed("You do not have access to this page", m.org.OrganizationName);
                else if (!m.org.OrganizationMembers.Any(om => om.PeopleId == Util.UserPeopleId))
                    return NotAllowed("You must be a member of this organization", m.org.OrganizationName);

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
            if (!org.PurgeOrg(DbUtil.Db))
                return Content("error, not deleted");
            Util2.CurrentOrgId = 0;
            Util2.CurrentGroups = null;
            Session.Remove("ActiveOrganization");
            return Content("ok");
        }
        public ActionResult Clone(int id)
        {
            var org = DbUtil.Db.LoadOrganizationById(id);
            var neworg = org.CloneOrg(DbUtil.Db);
            DbUtil.LogActivity("Cloning new org from {0}".Fmt(org.FullName));
            return Content("/Organization/Index/" + neworg.OrganizationId);
        }

        [AcceptVerbs(HttpVerbs.Post)]
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
            DbUtil.LogActivity("Creating new meeting for {0}".Fmt(dt));
            return Content("/Meeting/Index/" + mt.MeetingId);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteMeeting(string id, bool future)
        {
            var aa = id.Split('.');
            var mid = aa[1].ToInt2();
            if (!mid.HasValue)
                return Content("error: no meetingid");
            var meeting = DbUtil.Db.Meetings.SingleOrDefault(m => m.MeetingId == mid);
            if (meeting == null)
                return Content("error: no meeting");
            var orgid = meeting.OrganizationId;
            var q = from a in DbUtil.Db.Attends
                    where a.MeetingId == mid
                    select a.PeopleId;
            var list = q.ToList();
            var attendees = DbUtil.Db.Attends.Where(a => a.MeetingId == mid);
            foreach (var a in attendees)
                if (a.AttendanceFlag == true)
                    Attend.RecordAttendance(a.PeopleId, mid.Value, false);
            DbUtil.Db.Attends.DeleteAllOnSubmit(attendees);
            DbUtil.Db.Meetings.DeleteOnSubmit(meeting);
            DbUtil.Db.SubmitChanges();
            return Content("ok");
        }
        [AcceptVerbs(HttpVerbs.Post)]

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
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PrevMemberGrid(int id, string namefilter)
        {
            var qb = DbUtil.Db.QueryBuilderPreviousCurrentOrg();
            InitExportToolbar(id, qb.QueryId);
            var m = new PrevMemberModel(id, namefilter);
            UpdateModel(m.Pager);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult VisitorGrid(int id, string namefilter)
        {
            var qb = DbUtil.Db.QueryBuilderVisitedCurrentOrg();
            InitExportToolbar(id, qb.QueryId);
            var m = new VisitorModel(id, qb.QueryId, namefilter);
            UpdateModel(m.Pager);
            return View("VisitorGrid", m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PendingMemberGrid(int id, string namefilter)
        {
            var qb = DbUtil.Db.QueryBuilderPendingCurrentOrg();
            InitExportToolbar(id, qb.QueryId);
            var m = new MemberModel(id, null, MemberModel.GroupSelect.Pending, namefilter);
            UpdateModel(m.Pager);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult InactiveMemberGrid(int id, string namefilter)
        {
            var qb = DbUtil.Db.QueryBuilderInactiveCurrentOrg();
            InitExportToolbar(id, qb.QueryId);
            var m = new MemberModel(id, null, MemberModel.GroupSelect.Inactive, namefilter);
            UpdateModel(m.Pager);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MeetingGrid(int id, bool future)
        {
            var m = new MeetingModel(id, future);
            UpdateModel(m.Pager);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]

        public ActionResult Settings(int id)
        {
            var m = new OrganizationModel(id, Util2.CurrentGroups);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SettingsEdit(int id)
        {
            var m = new OrganizationModel(id, Util2.CurrentGroups);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SettingsUpdate(int id)
        {
            var m = new OrganizationModel(id, Util2.CurrentGroups);
            UpdateModel(m);
            m.ValidateSettings(ModelState);
            if (ModelState.IsValid)
            {
                m.UpdateSchedules();
                DbUtil.Db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, m.org.OrgSchedules);
                return View("Settings", m);
            }
            return View("SettingsEdit", m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult NewSchedule()
        {
            var s = new ScheduleInfo(
                new OrgSchedule 
                { 
                    SchedDay = 0, 
                    SchedTime = DateTime.Parse("8:00 AM"), 
                    AttendCreditId = 1 
                });
            return View("ScheduleEditor", s);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult OrgInfo(int id)
        {
            var m = new OrganizationModel(id, Util2.CurrentGroups);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult OrgInfoEdit(int id)
        {
            var m = new OrganizationModel(id, Util2.CurrentGroups);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult OrgInfoUpdate(int id)
        {
            var m = new OrganizationModel(id, Util2.CurrentGroups);
            UpdateModel(m);
            m.DivisionsList = Request.Form["DivisionsList"];
            m.UpdateOrganization();
            m = new OrganizationModel(id, Util2.CurrentGroups);
            return View("OrgInfo", m);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SmallGroups()
        {
            var m = new OrganizationModel(Util2.CurrentOrgId, Util2.CurrentGroups);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddFromTag(int id, int tagid, bool? pending)
        {
            var o = DbUtil.Db.LoadOrganizationById(id);
            var q = from t in DbUtil.Db.TagPeople
                    where t.Id == tagid
                    select t.PeopleId;
            foreach (var pid in q)
                OrganizationMember.InsertOrgMembers(DbUtil.Db,
                    id, pid, MemberTypeCode.Member, DateTime.Now, null, pending ?? false);
            return Content("ok");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult CopySettings()
        {
            if (Util.SessionTimedOut() || Util2.CurrentOrgId == 0)
                return Redirect("/");
            Session["OrgCopySettings"] = Util2.CurrentOrgId;
            return Redirect("/OrgSearch/");
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Join(string id)
        {
            var aa = id.Split('.');
            if (aa.Length != 3)
                return Content("error: bad info");
            var pid = aa[1].ToInt();
            var oid = aa[2].ToInt();
            OrganizationMember.InsertOrgMembers(DbUtil.Db,
                oid, pid, MemberTypeCode.Member,
                DateTime.Now, null, false);
            return Content("ok");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ToggleTag(int id)
        {
            var t = Person.ToggleTag(id, Util2.CurrentTagName, Util2.CurrentTagOwnerId, DbUtil.TagTypeId_Personal);
            DbUtil.Db.SubmitChanges();
            return Content(t ? "Remove" : "Add");
        }
        [AcceptVerbs(HttpVerbs.Post)]
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
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddContact(int id)
        {
            var cid = CmsData.Contact.AddContact(id);
            return Content("/Contact.aspx?id=" + cid);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddTasks(int id)
        {
            var c = new ContentResult();
            c.Content = Task.AddTasks(id).ToString();
            return c;
        }
        public ActionResult NotifyIds()
        {
            if (Util.SessionTimedOut() || Util2.CurrentOrgId == 0)
                return Content("<script type='text/javascript'>window.onload = function() { parent.location = '/'; }</script>");
            Response.NoCache();
            var t = DbUtil.Db.FetchOrCreateTag(Util.SessionId, Util.UserPeopleId, DbUtil.TagTypeId_AddSelected);
            DbUtil.Db.TagPeople.DeleteAllOnSubmit(t.PersonTags);
            DbUtil.Db.SubmitChanges();
            var o = DbUtil.Db.LoadOrganizationById(Util2.CurrentOrgId);
            var q = DbUtil.Db.PeopleFromPidString(o.NotifyIds).Select(p => p.PeopleId);
            foreach (var pid in q)
                t.PersonTags.Add(new TagPerson { PeopleId = pid });
            DbUtil.Db.SubmitChanges();
            return Redirect("/SearchUsers?ordered=true&topid=" + q.FirstOrDefault());
        }
        [HttpPost]
        public ActionResult UpdateNotifyIds(int topid)
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
            return View("NotifyList", DbUtil.Db.PeopleFromPidString(o.NotifyIds));
        }
        [HttpPost]
        public ActionResult ScheduleList(int id)
        {
            var m = new OrganizationModel(id, null);
            var q = new SelectList(m.schedules.OrderBy(cc => cc.Id), "Value", "Display");
            return View(q);
        }
    }
}
