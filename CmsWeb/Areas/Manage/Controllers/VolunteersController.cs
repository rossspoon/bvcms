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
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using UtilityExtensions;
using System.Web.Routing;
using CmsWeb;
using CmsWeb.Models;
using CmsData;
using System.Diagnostics;
using System.Net.Mail;
using System.Text;

namespace CmsWeb.Areas.Manage.Controllers
{
	public class VolunteersController : CmsStaffController
	{
		public VolunteersController()
		{
			ViewData["Title"] = "Volunteers";
		}
		public ActionResult Index(int? id)
		{
			var vols = new VolunteersModel { QueryId = id };
			Session["continuelink"] = Request.Url;
			UpdateModel(vols);
			if (!vols.View.HasValue())
				vols.View = "ns";
			DbUtil.LogActivity("Volunteers");
			return View(vols);
		}
		[AcceptVerbs(HttpVerbs.Post)]
		public JsonResult Codes(string id)
		{
			var q = from p in DbUtil.Db.VolInterestInterestCodes
					where p.VolInterestCode.Org == id
					select new
					{
						Key = p.VolInterestCode.Org + p.VolInterestCode.Code,
						PeopleId = "p" + p.PeopleId,
						Name = p.Person.Name
					};
			return Json(q);
		}
		public ActionResult Calendar(int id, string sg1, string sg2, bool? SortByWeek)
		{
			var m = new VolunteerCommitmentsModel(id);
			m.SmallGroup1 = sg1;
			m.SmallGroup2 = sg2;
			m.SortByWeek = SortByWeek ?? false;
			return View(m);
		}
		[HttpPost]
		public ActionResult DragDrop(int id, int week, DateTime time, string pid, string sg1, string sg2, string mid)
		{
			var m = new VolunteerCommitmentsModel(id);

			m.SmallGroup1 = sg1;
			m.SmallGroup2 = sg2;
			List<VolunteerCommitmentsModel.Slot> slots = null;
			if (week > 0)
				slots = (from s in m.FetchSlots(week)
						 where s.Week == week
						 where s.Time.TimeOfDay == time.TimeOfDay
						 select s).ToList();
			List<int> volids = null;
			switch(pid)
			{
				case "nocommits":
					volids = (from p in m.Volunteers()
							  where p.commits == 0
							  select p.PeopleId).ToList();
					break;
				case "commits":
					volids = (from p in m.Volunteers()
							  where p.commits > 0
							  select p.PeopleId).ToList();
					break;
				case "all":
					volids = (from p in m.Volunteers()
							  select p.PeopleId).ToList();
					break;
				case "remove":
				default:
					volids = new List<int>() { pid.ToInt() };
					break;
			}
			foreach (var PeopleId in volids)
				if (mid.ToInt() > 0)
				{
					DbUtil.Db.ExecuteCommand("DELETE FROM dbo.SubRequest WHERE EXISTS(SELECT NULL FROM Attend a WHERE a.AttendId = AttendId AND a.MeetingId = {0} AND a.PeopleId = {1})", mid.ToInt(), PeopleId);
					DbUtil.Db.ExecuteCommand("DELETE dbo.Attend WHERE MeetingId = {0} AND PeopleId = {1}", mid.ToInt(), PeopleId);
				}
				else if (week == 0) // drop all
				{
					DbUtil.Db.ExecuteCommand("DELETE FROM dbo.SubRequest WHERE EXISTS(SELECT NULL FROM Attend a WHERE a.AttendId = AttendId AND a.OrganizationId = {1} AND a.MeetingDate > {1} AND a.PeopleId = {2})", m.OrgId, m.Sunday, PeopleId);
					DbUtil.Db.ExecuteCommand("DELETE dbo.Attend WHERE OrganizationId = {0} AND MeetingDate > {1} AND PeopleId = {2}", m.OrgId, m.Sunday, PeopleId);
				}
				else
					foreach (var s in slots)
						Attend.MarkRegistered(DbUtil.Db, id, PeopleId, s.Time, true);
			return Content("ok");
		}

		public ActionResult CustomReport(string id)
		{
			ViewData["content"] = DbUtil.Content("Volunteer-{0}.report".Fmt(id)).Body;
			return View();
		}
		public ActionResult Query(int id)
		{
			var vols = new VolunteersModel { QueryId = id };
			UpdateModel(vols);
			var qb = DbUtil.Db.QueryBuilderClauses.FirstOrDefault(c => c.QueryId == id).Clone(DbUtil.Db);
			var comp = CompareType.Equal;
			if (vols.Org == "na")
				comp = CompareType.NotEqual;
			var clause = qb.AddNewClause(QueryType.HasVolunteered, comp, "1,T");
			clause.Quarters = vols.View;

			DbUtil.Db.QueryBuilderClauses.InsertOnSubmit(qb);
			DbUtil.Db.SubmitChanges();
			return Redirect("/QueryBuilder/Main/{0}".Fmt(qb.QueryId));
		}
		public ActionResult EmailReminders(int id)
		{
			var qb = DbUtil.Db.QueryBuilderScratchPad();
			qb.CleanSlate(DbUtil.Db);
			var clause = qb.AddNewClause(QueryType.RegisteredForMeetingId, CompareType.Equal, id.ToString());
			DbUtil.Db.SubmitChanges();

			var meeting = DbUtil.Db.Meetings.Single(m => m.MeetingId == id);
			var subject = "{0} Reminder".Fmt(meeting.Organization.OrganizationName);
			var body =
@"<blockquote><table>
<tr><td>Time:</td><td>{0:f}</td></tr>
<tr><td>Location:</td><td>{1}</td></tr>
</table></blockquote><p>{2}</p>".Fmt(
								meeting.MeetingDate,
								meeting.Organization.Location,
								meeting.Organization.LeaderName);

			//return Redirect("/EmailPeople.aspx?id={0}&subj={1}&body={2}&ishtml=true"
			//    .Fmt(qb.QueryId, Server.UrlEncode(subject), Server.UrlEncode(body)));
			TempData["body"] = body;
			return Redirect("/Email/Index/{0}?subj={1}&ishtml=true"
				.Fmt(qb.QueryId, Server.UrlEncode(subject)));
		}
		public ActionResult UpdateAll(string id, int? qid)
		{
			var orgkeys = Person.OrgKeys(id);
			var q = DbUtil.Db.People.AsQueryable();
			if (qid.HasValue)
				q = DbUtil.Db.PeopleQuery(qid.Value);
			q = from p in q
				where p.VolInterestInterestCodes.Count(c => orgkeys.Contains(c.VolInterestCode.Org)) > 0
				select p;
			foreach (var person in q)
			{
				var m = new CmsWeb.Models
					.VolunteerModel2 { View = id, person = person };
				m.person.BuildVolInfoList(id); // gets existing
				m.person.BuildVolInfoList(id); // 2nd time updates existing
				m.person.RefreshCommitments(id);
			}
			return Content("done");
		}
	}
}
