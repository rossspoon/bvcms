using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;
using UtilityExtensions;
using System.Text;
using CmsWeb.Models.PersonPage;
using CmsWeb.Models;
using System.Diagnostics;
using System.Web.Routing;
using System.Threading;
using System.Data.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace CmsWeb.Areas.Main.Controllers
{
    public class MeetingController : CmsStaffController
    {
        public ActionResult Index(int? id, bool? showall, bool? sortbyname)
        {
            if (!id.HasValue)
                return RedirectShowError("no id");
            var m = new MeetingModel(id.Value);
            m.showall = showall == true;
            m.sortbyname = sortbyname == true;
            if (m.meeting == null)
                return RedirectShowError("no meeting");

			if (Util2.OrgMembersOnly
				&& !DbUtil.Db.OrganizationMembers.Any(om =>
					om.OrganizationId == m.meeting.OrganizationId
					&& om.PeopleId == Util.UserPeopleId))
				return RedirectShowError("You must be a member of this organization to have access to this page");
			else if (Util2.OrgLeadersOnly
				&& !DbUtil.Db.OrganizationMembers.Any(om =>
					om.OrganizationId == m.meeting.OrganizationId
					&& om.PeopleId == Util.UserPeopleId
                    && om.MemberType.AttendanceTypeId == CmsData.Codes.AttendTypeCode.Leader))
				return RedirectShowError("You must be a leader of this organization to have access to this page");
			DbUtil.LogActivity("Viewing Meeting for {0}".Fmt(m.meeting.Organization.OrganizationName));
            return View(m);
        }
        public ActionResult iPad(int? id)
        {
            if (!id.HasValue)
                return RedirectShowError("no id");
            var m = new MeetingModel(id.Value);
            m.showall = true;
            if (m.meeting == null)
                return RedirectShowError("no meeting");

			if (Util2.OrgMembersOnly
				&& !DbUtil.Db.OrganizationMembers.Any(om =>
					om.OrganizationId == m.meeting.OrganizationId
					&& om.PeopleId == Util.UserPeopleId))
				return RedirectShowError("You must be a member of this organization to have access to this page");
			else if (Util2.OrgLeadersOnly
				&& !DbUtil.Db.OrganizationMembers.Any(om =>
					om.OrganizationId == m.meeting.OrganizationId
					&& om.PeopleId == Util.UserPeopleId
                    && om.MemberType.AttendanceTypeId == CmsData.Codes.AttendTypeCode.Leader))
				return RedirectShowError("You must be a leader of this organization to have access to this page");
			DbUtil.LogActivity("iPad Meeting for {0}({1:d})".Fmt(m.meeting.OrganizationId, m.meeting.MeetingDate));
            return View(m);
        }
        public ActionResult Tickets(int? id)
        {
            if (!id.HasValue)
                return RedirectShowError("no id");
            var m = new MeetingModel(id.Value);
            m.showall = true;
            if (m.meeting == null)
                return RedirectShowError("no meeting");

			if (Util2.OrgMembersOnly
				&& !DbUtil.Db.OrganizationMembers.Any(om =>
					om.OrganizationId == m.meeting.OrganizationId
					&& om.PeopleId == Util.UserPeopleId))
				return RedirectShowError("You must be a member of this organization to have access to this page");
			else if (Util2.OrgLeadersOnly
				&& !DbUtil.Db.OrganizationMembers.Any(om =>
					om.OrganizationId == m.meeting.OrganizationId
					&& om.PeopleId == Util.UserPeopleId
                    && om.MemberType.AttendanceTypeId == CmsData.Codes.AttendTypeCode.Leader))
				return RedirectShowError("You must be a leader of this organization to have access to this page");
			DbUtil.LogActivity("Tickets Meeting for {0}({1:d})".Fmt(m.meeting.OrganizationId, m.meeting.MeetingDate));
            return View(m);
        }
        [HttpPost]
        public ContentResult EditGroup(string id, string value)
        {
            var i = id.Substring(2).ToInt();
            var m = new MeetingModel(i);
            m.meeting.GroupMeetingFlag = value == "true";
            DbUtil.Db.SubmitChanges();
            if (m.meeting.GroupMeetingFlag)
                return Content("Group (headcount)");
            return Content("Regular");
        }
        [HttpPost]
        public ContentResult EditAttendCredit(string id, string value)
        {
            var i = id.Substring(2).ToInt();
            var m = new MeetingModel(i);
            m.meeting.AttendCreditId = value.ToInt();
            DbUtil.Db.SubmitChanges();
            return Content(m.AttendCreditType());
        }
        [HttpPost]
        public JsonResult AttendCredits()
        {
            var q = from c in DbUtil.Db.AttendCredits
                    select new
                    {
                        Code = c.Id.ToString(),
                        Value = c.Description,
                    };
            return Json(q.ToDictionary(k => k.Code, v => v.Value));
        }
		[HttpPost]
        public JsonResult MeetingTypes()
        {
			var d = new Dictionary<string, string>
			{{"true","Group (headcount)"},{"false","Regular"}};
            return Json(d);
        }
        [HttpPost]
        public ContentResult Edit(string id, string value)
        {
            var i = id.Substring(2).ToInt();
            var m = new MeetingModel(i);
            switch (id[0])
            {
                case 'd':
                    m.meeting.Description = value;
                    break;
                case 'n':
                    m.meeting.NumPresent = value.ToInt();
                    break;
            }
            DbUtil.Db.SubmitChanges();
            return Content(value);
        }

    	public class ScanTicketInfo
    	{
			public enum Error
			{
				none,
				alreadymarked,
				notmember,
				notregistered,
				noperson
			}
    		public Error error { get; set; }
    		public Person person { get; set; }
    		public Meeting meeting { get; set; }
    		public Attend attended { get; set; }
    		public OrganizationMember orgmember { get; set; }
			public string CssClass()
			{
				if (error == Error.none)
					return "bgyellow";
				return "bgred";
			}
    	}

    	[Authorize(Roles = "Attendance")]
        [HttpPost]
        public ActionResult ScanTicket(int wandtarget, int MeetingId, bool? requireMember, bool? requireRegistered)
        {
			var q = from person in DbUtil.Db.People
					where person.PeopleId == wandtarget
					let meeting = DbUtil.Db.Meetings.SingleOrDefault(mm => mm.MeetingId == MeetingId)
					let attended = DbUtil.Db.Attends.SingleOrDefault(aa => aa.MeetingId == MeetingId && aa.PeopleId == wandtarget)
					let orgmember = DbUtil.Db.OrganizationMembers.SingleOrDefault(om => om.OrganizationId == meeting.OrganizationId && om.PeopleId == wandtarget)
					select new ScanTicketInfo
					{
						person = person,
						meeting = meeting,
						attended = attended,
						orgmember = orgmember
					};
			var d = q.SingleOrDefault();

			if (d == null)
				return View(new ScanTicketInfo 
				{ 
					error = ScanTicketInfo.Error.noperson,
					person = new Person()
				});

			d.error = ScanTicketInfo.Error.none;
    		if (d.attended != null && d.attended.AttendanceFlag == true)
				d.error = ScanTicketInfo.Error.alreadymarked;
			else if (requireMember == true && d.orgmember == null)
				d.error = ScanTicketInfo.Error.notmember;
			else if (requireRegistered == true && (d.attended == null || d.attended.Registered != true))
				d.error = ScanTicketInfo.Error.notregistered;

			if (d.error == ScanTicketInfo.Error.none)
	    		Attend.RecordAttendance(DbUtil.Db, wandtarget, MeetingId, true);

			return View(d);
        }
        [Authorize(Roles = "Attendance")]
        [HttpPost]
        public ActionResult MarkAttendance(int PeopleId, int MeetingId, bool Present)
        {
            var ret = Attend.RecordAttendance(DbUtil.Db, PeopleId, MeetingId, Present);
            if (ret != "ok")
                return Json(new { error = ret });
            DbUtil.Db.UpdateMeetingCounters(MeetingId);
            var m = DbUtil.Db.Meetings.Single(mm => mm.MeetingId == MeetingId);
            DbUtil.Db.Refresh(RefreshMode.OverwriteCurrentValues, m);
            var v = Json(new 
            { 
                m.NumPresent, 
                m.NumMembers, 
                m.NumVstMembers, 
                m.NumRepeatVst, 
                m.NumNewVisit, 
                m.NumOtherAttends 
            });
            return v;
        }
        [Authorize(Roles = "Attendance")]
        [HttpPost]
        public ActionResult MarkRegistered(int PeopleId, int MeetingId, bool Registered)
        {
            Attend.MarkRegistered(DbUtil.Db, PeopleId, MeetingId, Registered);
            return Content("ok");
        }
        [Authorize(Roles = "Attendance")]
        [HttpPost]
        public ActionResult CreateMeeting(string id)
        {
            var a = id.SplitStr(".");
            var orgid = a[1].ToInt();
            var organization = DbUtil.Db.LoadOrganizationById(orgid);
            if (organization == null)
                return Content("error:Bad Orgid ({0})".Fmt(id));

            var re = new Regex(@"\A(0[1-9]|1[012])(0[1-9]|[12][0-9]|3[01])([0-9]{2})([01][0-9])([0-5][0-9])\Z");
            if (!re.IsMatch(a[2]))
                return Content("error:Bad Date and Time ({0})".Fmt(id));
            var g = re.Match(a[2]);
            var dt = new DateTime(
                g.Groups[3].Value.ToInt() + 2000,
                g.Groups[1].Value.ToInt(),
                g.Groups[2].Value.ToInt(),
                g.Groups[4].Value.ToInt(),
                g.Groups[5].Value.ToInt(),
                0);
            var newMtg = DbUtil.Db.Meetings.FirstOrDefault(m => m.OrganizationId == orgid && m.MeetingDate == dt);
			if (newMtg == null)
			{
                var attsch = organization.OrgSchedules.SingleOrDefault(ss => ss.MeetingTime.Value.TimeOfDay == dt.TimeOfDay && ss.MeetingTime.Value.DayOfWeek == dt.DayOfWeek);
                int? attcred = null;
                if (attsch != null)
                    attcred = attsch.AttendCreditId;
                newMtg = new CmsData.Meeting
				{
					CreatedDate = Util.Now,
                    CreatedBy = Util.UserId1,
					OrganizationId = orgid,
					GroupMeetingFlag = false,
					Location = organization.Location,
					MeetingDate = dt,
                    AttendCreditId = attcred,
				};
				DbUtil.Db.Meetings.InsertOnSubmit(newMtg);
				DbUtil.Db.SubmitChanges();
				DbUtil.LogActivity("Created new meeting for {0}".Fmt(organization.OrganizationName));
			}
			return Content("/Meeting/Index/{0}?showall=true".Fmt(newMtg.MeetingId));
        }
        public ActionResult QueryAttendees(int Id)
        {
            var m = DbUtil.Db.Meetings.Single(mm => mm.MeetingId == Id);
            var qb = DbUtil.Db.QueryBuilderScratchPad();
            qb.CleanSlate(DbUtil.Db);
            qb.AddNewClause(QueryType.MeetingId, CompareType.Equal, m.MeetingId);
            DbUtil.Db.SubmitChanges();
            return Redirect("/QueryBuilder/Main/{0}".Fmt(qb.QueryId));
        }
        public ActionResult QueryVisitors(int Id)
        {
            var m = DbUtil.Db.Meetings.Single(mm => mm.MeetingId == Id);
            var qb = DbUtil.Db.QueryBuilderScratchPad();
            qb.CleanSlate(DbUtil.Db);
            qb.AddNewClause(QueryType.MeetingId, CompareType.Equal, m.MeetingId);
            var c = qb.AddNewClause(QueryType.AttendTypeAsOf, CompareType.OneOf, "40,VM;50,RV;60,NV");
            c.StartDate = m.MeetingDate;
            c.Program = m.Organization.Division.Program.Id;
            c.Division = m.Organization.DivisionId.Value;
            c.Organization = m.OrganizationId;

            DbUtil.Db.SubmitChanges();
            return Redirect("/QueryBuilder/Main/{0}".Fmt(qb.QueryId));
        }
        public ActionResult QueryAbsents(int Id)
        {
            var m = DbUtil.Db.Meetings.Single(mm => mm.MeetingId == Id);
            var qb = DbUtil.Db.QueryBuilderScratchPad();
            qb.CleanSlate(DbUtil.Db);
            qb.AddNewClause(QueryType.MeetingId, CompareType.NotEqual, m.MeetingId);
            var c = qb.AddNewClause(QueryType.WasMemberAsOf, CompareType.Equal, "1,T");
            c.StartDate = m.MeetingDate;
            c.Program = m.Organization.Division.Program.Id;
            c.Division = m.Organization.DivisionId.Value;
            c.Organization = m.OrganizationId;

            DbUtil.Db.SubmitChanges();
            return Redirect("/QueryBuilder/Main/{0}".Fmt(qb.QueryId));
        }
        public ActionResult QueryRegistered(int Id, string type)
        {
            var m = DbUtil.Db.Meetings.Single(mm => mm.MeetingId == Id);
            var qb = DbUtil.Db.QueryBuilderScratchPad();
            qb.CleanSlate(DbUtil.Db);
            switch (type)
            {
                case "Registered":
                    qb.AddNewClause(QueryType.RegisteredForMeetingId, CompareType.Equal, m.MeetingId);
                    break;
                case "Attends":
                    qb.AddNewClause(QueryType.RegisteredForMeetingId, CompareType.Equal, m.MeetingId);
                    qb.AddNewClause(QueryType.MeetingId, CompareType.Equal, m.MeetingId);
                    break;
                case "Absents":
                    qb.AddNewClause(QueryType.RegisteredForMeetingId, CompareType.Equal, m.MeetingId);
                    qb.AddNewClause(QueryType.MeetingId, CompareType.NotEqual, m.MeetingId);
                    break;
                case "UnregisteredAttends":
                    qb.AddNewClause(QueryType.RegisteredForMeetingId, CompareType.NotEqual, m.MeetingId);
                    qb.AddNewClause(QueryType.MeetingId, CompareType.Equal, m.MeetingId);
                    break;
            }
            DbUtil.Db.SubmitChanges();
            return Redirect("/QueryBuilder/Main/{0}".Fmt(qb.QueryId));
        }
        class ttt
        {
            public string label { get; set; }
            public string name { get; set; }
        }
        public DataGridResult AttendanceByGroups(int id)
        {
            var q = from a in DbUtil.Db.Attends
                    where a.MeetingId == id
                    where a.AttendanceFlag == true
                    join om in DbUtil.Db.OrgMemMemTags on new { a.OrganizationId, a.PeopleId } equals new { OrganizationId = om.OrgId, om.PeopleId }
                    select new { a.Person.Name, SmallGroup = om.MemberTag.Name };
            var j = from i in q
                    group i.Name by i.SmallGroup into g
                    select new { g.Key, g };
            var list = new List<ttt>();
            foreach (var i in j)
            {
                list.Add(new ttt { label = "SmallGroup", name = "{0} ({1})".Fmt(i.Key, i.g.Count()) });
                foreach (var name in i.g)
                    list.Add(new ttt { label = "", name = name });
            }
            return new DataGridResult(list);
        }
        public ActionResult NewExtraValue(int id, string field, string value, bool multiline)
        {
            var m = new MeetingModel(id);
            try
            {
                var mev = new MeetingExtra { MeetingId = id, Field = field, Data = value, DataType = multiline ? "text" : null };
                DbUtil.Db.MeetingExtras.InsertOnSubmit(mev);
                DbUtil.Db.SubmitChanges();
            }
            catch (Exception ex)
            {
                return Content("error: " + ex.Message);
            }
            return View("ExtrasGrid", m.meeting);
        }
        [HttpPost]
        public ViewResult DeleteExtra(int id, string field)
        {
            var e = DbUtil.Db.MeetingExtras.Single(ee => ee.MeetingId == id && ee.Field == field);
            DbUtil.Db.MeetingExtras.DeleteOnSubmit(e);
            DbUtil.Db.SubmitChanges();
            var m = new MeetingModel(id);
            return View("ExtrasGrid", m.meeting);
        }
        [HttpPost]
        public ContentResult EditExtra(string id, string value)
        {
            var a = id.SplitStr("-", 2);
            var b = a[1].SplitStr(".", 2);
            var e = DbUtil.Db.MeetingExtras.Single(ee => ee.MeetingId == b[1].ToInt() && ee.Field == b[0]);
            e.Data = value;
            DbUtil.Db.SubmitChanges();
            return Content(value);
        }
    }
}
