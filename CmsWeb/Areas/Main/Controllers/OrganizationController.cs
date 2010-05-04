using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;
using UtilityExtensions;
using System.Text;
using CMSWeb.Models.OrganizationPage;
using CMSWeb.Models;
using System.Diagnostics;

namespace CMSWeb.Areas.Main.Controllers
{
    public class OrganizationController : CmsStaffController
    {
        public ActionResult Index(int? id)
        {
            if (!id.HasValue)
                return Content("no org");
            var m = new OrganizationModel(id.Value);
            if (m.org == null)
                return Content("organization not found");
            if (Util.OrgMembersOnly
                && !DbUtil.Db.OrganizationMembers.Any(om =>
                    om.OrganizationId == m.org.OrganizationId
                    && om.PeopleId == Util.UserPeopleId))
            {
                DbUtil.LogActivity("Trying to view Organization ({0})".Fmt(m.org.OrganizationName));
                return Content("<h3 style='color:red'>{0}</h3>\n<a href='{1}'>{2}</a>"
                    .Fmt("You must be a member of this organization to have access to this page",
                    "javascript: history.go(-1)", "Go Back"));
            }
            DbUtil.LogActivity("Viewing Organization ({0})".Fmt(m.org.OrganizationName));

            if (Util.CurrentOrgId != m.org.OrganizationId)
                Util.CurrentGroupId = 0;
            Util.CurrentOrgId = m.org.OrganizationId;
            ViewData["OrganizationContext"] = true;
            var qb = DbUtil.Db.QueryBuilderInCurrentOrg();
            ViewData["queryid"] = qb.QueryId;
            Session["ActiveOrganization"] = m.org.OrganizationName;
            return View(m);
        }
        [Authorize(Roles = "Admin")]

        public ActionResult Delete(int id)
        {
            var org = DbUtil.Db.LoadOrganizationById(id);
            if (org == null)
                return Content("error, bad orgid");
            if (!org.PurgeOrg())
                return Content("error, not deleted");
            Util.CurrentOrgId = 0;
            Util.CurrentGroupId = 0;
            Session.Remove("ActiveOrganization");
            return new EmptyResult();
        }
        public ActionResult Clone(int id)
        {
            var org = DbUtil.Db.LoadOrganizationById(id);
            var neworg = org.CloneOrg();
            DbUtil.LogActivity("Cloning new org from {0}".Fmt(org.FullName));
            return Content("/Organization/Index/" + neworg.OrganizationId);
        }
        [AcceptVerbs(HttpVerbs.Post)]

        public ActionResult NewMeeting(string d, string t, bool group)
        {
            var organization = DbUtil.Db.LoadOrganizationById(Util.CurrentOrgId);
            DateTime dt;
            if (!DateTime.TryParse(d + " " + t, out dt))
                return new EmptyResult();
            var mt = DbUtil.Db.Meetings.SingleOrDefault(m => m.MeetingDate == dt
                    && m.OrganizationId == organization.OrganizationId);

            if (mt != null)
                return new EmptyResult();

            mt = new CmsData.Meeting
            {
                CreatedDate = Util.Now,
                CreatedBy = Util.UserId1,
                OrganizationId = organization.OrganizationId,
                GroupMeetingFlag = group,
                Location = organization.Location,
                MeetingDate = dt,
            };
            DbUtil.Db.Meetings.InsertOnSubmit(mt);
            DbUtil.Db.SubmitChanges();
            DbUtil.LogActivity("Creating new meeting for {0}".Fmt(dt));
            return Content("/Meeting.aspx?edit=1&id=" + mt.MeetingId);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteMeeting(string id, bool future)
        {
            var aa = id.Split('.');
            var mid = aa[1].ToInt2();
            if (!mid.HasValue)
                return new EmptyResult();
            var meeting = DbUtil.Db.Meetings.SingleOrDefault(m => m.MeetingId == mid);
            if (meeting == null)
                return new EmptyResult();
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
            DbUtil.Db.SoulMates.DeleteAllOnSubmit(meeting.SoulMates);
            DbUtil.Db.Meetings.DeleteOnSubmit(meeting);
            DbUtil.Db.SubmitChanges();
            return Content("true");
        }
        [AcceptVerbs(HttpVerbs.Post)]

        public ActionResult CurrMemberGrid(int id, int smallgroupid)
        {
            var m = new MemberModel(id, smallgroupid, MemberModel.GroupSelect.Active);
            Util.CurrentOrgId = id;
            Util.CurrentGroupId = smallgroupid;
            var qb = DbUtil.Db.QueryBuilderInCurrentOrg();
            ViewData["queryid"] = qb.QueryId;
            ViewData["OrganizationContext"] = true;
            UpdateModel(m.Pager);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PrevMemberGrid(int id)
        {
            var m = new PrevMemberModel(id);
            Util.CurrentOrgId = id;
            var qb = DbUtil.Db.QueryBuilderPreviousCurrentOrg();
            ViewData["queryid"] = qb.QueryId;
            UpdateModel(m.Pager);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult VisitorGrid(int id)
        {
            Util.CurrentOrgId = id;
            var qb = DbUtil.Db.QueryBuilderVisitedCurrentOrg();
            ViewData["queryid"] = qb.QueryId;
            var m = new VisitorModel(id, qb.QueryId);
            UpdateModel(m.Pager);
            return View("VisitorGrid", m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PendingMemberGrid(int id)
        {
            var m = new MemberModel(id, 0, MemberModel.GroupSelect.Pending);
            UpdateModel(m.Pager);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult InactiveMemberGrid(int id)
        {
            var m = new MemberModel(id, 0, MemberModel.GroupSelect.Inactive);
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
            var m = new OrganizationModel(id);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SettingsEdit(int id)
        {
            var m = new OrganizationModel(id);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SettingsUpdate(int id)
        {
            var m = new OrganizationModel(id);
            UpdateModel(m);
            m.UpdateOrganization();
            m = new OrganizationModel(id);
            return View("Settings", m);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult OrgInfo(int id)
        {
            var m = new OrganizationModel(id);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult OrgInfoEdit(int id)
        {
            var m = new OrganizationModel(id);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult OrgInfoUpdate(int id)
        {
            var m = new OrganizationModel(id);
            UpdateModel(m);
            m.DivisionsList = Request.Form["DivisionsList"];
            m.UpdateOrganization();
            m = new OrganizationModel(id);
            return View("OrgInfo", m);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MakeNewGroup(int id, string GroupName)
        {
            if (!GroupName.HasValue())
                return new EmptyResult();
            var Db = DbUtil.Db;
            var group = Db.MemberTags.SingleOrDefault(g =>
                g.Name == GroupName && g.OrgId == id);
            if (group == null)
            {
                group = new MemberTag
                {
                    Name = GroupName,
                    OrgId = id
                };
                Db.MemberTags.InsertOnSubmit(group);
                Db.SubmitChanges();
            }
            var m = new OrganizationModel(id);
            return View("ManageGroups", m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RenameGroup(int id, string GroupName, int? groupid)
        {
            if (!GroupName.HasValue() || !groupid.HasValue)
                return new EmptyResult();
            var group = DbUtil.Db.MemberTags.Single(d => d.Id == groupid);
            group.Name = GroupName;
            DbUtil.Db.SubmitChanges();
            var m = new OrganizationModel(id);
            return View("ManageGroups", m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteGroup(int id, string GroupName, int groupid)
        {
            var group = DbUtil.Db.MemberTags.SingleOrDefault(g => g.Id == groupid);
            if (group != null)
            {
                DbUtil.Db.OrgMemMemTags.DeleteAllOnSubmit(group.OrgMemMemTags);
                DbUtil.Db.MemberTags.DeleteOnSubmit(group);
                DbUtil.Db.SubmitChanges();
            }
            var m = new OrganizationModel(id);
            return View("ManageGroups", m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SmallGroups()
        {
            var m = new OrganizationModel(Util.CurrentOrgId);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]

        public EmptyResult AddFromTag(int id, int tagid, bool? pending)
        {
            var o = DbUtil.Db.LoadOrganizationById(id);
            var q = from t in DbUtil.Db.TagPeople
                    where t.Id == tagid
                    select t.PeopleId;
            foreach (var pid in q)
                OrganizationMember.InsertOrgMembers(id, pid, (int)OrganizationMember.MemberTypeCode.Member, DateTime.Now, null, pending ?? false);
            return new EmptyResult();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult CopySettings()
        {
            Session["OrgCopySettings"] = Util.CurrentOrgId;
            return Redirect("/OrgSearch/");
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Join(string id)
        {
            var aa = id.Split('.');
            if (aa.Length != 3)
                return new EmptyResult();
            var pid = aa[1].ToInt();
            var oid = aa[2].ToInt();
            OrganizationMember.InsertOrgMembers(oid, pid,
                (int)OrganizationMember.MemberTypeCode.Member,
                DateTime.Now, null, false);
            return Content("true");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ToggleTag(int id)
        {
            var t = Person.ToggleTag(id, Util.CurrentTagName, Util.CurrentTagOwnerId, DbUtil.TagTypeId_Personal);
            DbUtil.Db.SubmitChanges();
            return Content(t ? "Remove" : "Add");
        }
    }
}
