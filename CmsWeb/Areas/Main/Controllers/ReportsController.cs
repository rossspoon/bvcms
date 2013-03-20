using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using CmsWeb.Areas.Main.Models.Avery;
using CmsWeb.Areas.Main.Models.Directories;
using CmsWeb.Areas.Main.Models.Report;
using CmsData;
using System.IO;
using CmsWeb.Code;
using UtilityExtensions;
using System.Text;
using System.Web.UI;
using System.Data.SqlClient;

namespace CmsWeb.Areas.Main.Controllers
{
    public class ReportsController : CmsStaffController
    {
        public ActionResult Attendance(int id)
        {

            try
            {
                var m = new AttendanceModel(id);
                UpdateModel(m);
                return View(m);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult WeeklyAttendance(int? id)
        {
            return new WeeklyAttendanceResult(id);
        }
        public ActionResult Family(int? id)
        {
            return new CmsWeb.Areas.Main.Models.Report.FamilyResult(id);
        }
        public ActionResult BarCodeLabels(int? id)
        {
            if (!id.HasValue)
                return Content("no query");
            return new BarCodeLabelsResult(id.Value);
        }
        public ActionResult Contacts(int? id, bool? sortAddress)
        {
            if (!id.HasValue)
                return Content("no query");
            return new ContactsResult(id.Value, sortAddress);
        }
        public ActionResult Rollsheet(int? id, string org, int? pid, int? div, int? schedule, string name, string dt, int? meetingid, int? bygroup, string sgprefix, bool? altnames, string highlight)
        {
            var dt2 = dt.ToDate();

            return new RollsheetResult
            {
                qid = id,
                orgid = org == "curr" ? (int?)Util2.CurrentOrgId : null,
                groups = org == "curr" ? Util2.CurrentGroups : new int[] { 0 },
                pid = pid,
                div = div,
                name = name,
                schedule = schedule,
                meetingid = meetingid,
                bygroup = bygroup.HasValue,
                sgprefix = sgprefix,
                dt = dt2,
                altnames = altnames,
				highlightsg = highlight,
            };
        }
        public ActionResult RallyRollsheet(int? id, string org, int? pid, int? div, int? schedule, string name, string dt, int? meetingid, int? bygroup, string sgprefix, bool? altnames)
        {
            var dt2 = dt.ToDate();

            return new RallyRollsheetResult
            {
                qid = id,
                orgid = org == "curr" ? (int?)Util2.CurrentOrgId : null,
                groups = org == "curr" ? Util2.CurrentGroups : new int[] { 0 },
                pid = pid,
                div = div,
                name = name,
                schedule = schedule,
                meetingid = meetingid,
                bygroup = bygroup.HasValue,
                sgprefix = sgprefix,
                dt = dt2,
                altnames = altnames,
            };
        }
        public ActionResult OrgLeaders(string org, int? div, int? schedule, string name)
        {
            return new OrgLeadersResult
            {
                orgid = org == "curr" ? (int?)Util2.CurrentOrgId : null,
                div = div,
                name = name,
                schedule = schedule,
            };
        }
        public ActionResult ClassList(string org, int? div, int? schedule, string name)
        {
            return new ClassListResult
            {
                orgid = org == "curr" ? (int?)Util2.CurrentOrgId : null,
                div = div,
                name = name,
                schedule = schedule,
            };
        }
        public class ShirtSizeInfo
        {
            public string Size { get; set; }
            public int Count { get; set; }
        }
        public ActionResult ShirtSizes(string org, int? div, int? schedule, string name)
        {
            var orgid = org == "curr" ? (int?)Util2.CurrentOrgId : null;
			var q = from om in DbUtil.Db.OrganizationMembers
					let o = om.Organization
					where o.OrganizationName.Contains(name) || name.Length == 0
					where o.OrganizationId == orgid || orgid == 0 || orgid == null
					where o.DivOrgs.Any(t => t.DivId == div) || div == 0 || div == null
					where om.Organization.OrgSchedules.Any(sc => sc.ScheduleId == schedule) || schedule == 0 || schedule == null
					group 1 by om.ShirtSize into g
					select new ShirtSizeInfo
					{
						Size = g.Key,
						Count = g.Count(),
					}; 
			return View(q);
        }
        public ActionResult Roster1(int? queryid, int? org, int? div, int? schedule, string name, string tm)
        {
            return new RosterResult
            {
                qid = queryid,
                org = org,
                div = div,
                schedule = schedule,
                tm = tm,
            };
        }
        public ActionResult Roster(int? org, int? div, int? schedule, string name)
        {
            return new RosterListResult
            {
                orgid = org,
                div = div,
                schedule = schedule,
                name = name,
            };
        }
        public ActionResult EnrollmentControl(int div, int subdiv, int schedule)
        {
            return new EnrollmentControlResult
            {
                div = div,
                schedule = schedule,
                subdiv = subdiv
            };
        }
        public ActionResult Avery(int? id)
        {
            if (!id.HasValue)
                return Content("no query");
            return new AveryResult { id = id };
        }
        public ActionResult Avery3(int? id)
        {
            if (!id.HasValue)
                return Content("no query");
            return new Avery3Result { id = id };
        }
        //public ActionResult Coupons()
        //{
        //    return new CouponsResult(null, null);
        //}
        public ActionResult AveryAddress(int? id, string format, bool? titles, bool? usephone, bool? sortzip, int skipNum = 0)
        {
            if (!id.HasValue)
                return Content("no query");
            if (!format.HasValue())
                return Content("no format");
            return new AveryAddressResult
            {
                id = id,
                format = format,
                titles = titles,
                usephone = usephone ?? false,
				skip = skipNum,
				sortzip = sortzip
            };
        }
        public ActionResult RollLabels(int? id, string format, bool? titles, bool? usephone, bool? sortzip)
        {
            if (!id.HasValue)
                return Content("no query");
            return new RollLabelsResult
            {
                qid = id,
                format = format,
                titles = titles ?? false,
                usephone = usephone ?? false,
				sortzip = sortzip
            };
        }
        public ActionResult Prospect(int? id, bool? Form, bool? Alpha)
        {
            if (!id.HasValue)
                return Content("no query");
            return new ProspectResult(id, Form ?? false, Alpha ?? false);
        }
        public ActionResult Attendee(int? id)
        {
            if (!id.HasValue)
                return Content("no meetingid");
            return new AttendeeResult(id);
        }
        public ActionResult VisitsAbsents(int? id)
        {
            if (!id.HasValue)
                return Content("no meetingid");
            return new VisitsAbsentsResult(id);
        }
        public ActionResult VisitsAbsents2(int? id)
        {
            //This is basically a Contact Report version of the Visits Absents
            if (!id.HasValue)
                return Content("no meetingid");
            return new VisitsAbsentsResult2(id);
        }
        public ActionResult PastAttendee(int? id)
        {
            if (!id.HasValue)
                return Content("no orgid");
            return new PastAttendeeResult(id);
        }
        public ActionResult Registration(int? id, int? oid)
        {
            if (!id.HasValue)
                return Content("no query");
            return new RegistrationResult(id, oid);
        }
        public ActionResult ChurchAttendance(string id)
        {
            var dt2 = id.ToDate();
            if (!dt2.HasValue)
                dt2 = ChurchAttendanceModel.MostRecentAttendedSunday();
            var m = new ChurchAttendanceModel(dt2.Value);
            return View(m);
        }
        public ActionResult ChurchAttendance2(string Dt1, string Dt2, string skipweeks)
        {
            var dt1 = Dt1.ToDate();
            var dt2 = Dt2.ToDate();
            if (!dt1.HasValue)
                dt1 = ChurchAttendanceModel.MostRecentAttendedSunday();
            if (!dt2.HasValue)
                dt2 = DateTime.Today;
            var m = new ChurchAttendance2Model(dt1, dt2, skipweeks);
            return View(m);
        }
        public ActionResult AttendanceDetail(string Dt1, string Dt2, string name, int? divid, int? schedid, int? campusid)
        {
            var dt1 = Dt1.ToDate();
            if (!dt1.HasValue)
                dt1 = ChurchAttendanceModel.MostRecentAttendedSunday();
            var dt2 = Dt2.ToDate();
            if (!dt2.HasValue)
                dt2 = dt1.Value.AddDays(1);
            var m = new AttendanceDetailModel(dt1.Value, dt2, name, divid, schedid, campusid);
            return View(m);
        }
        public ActionResult RecentAbsents(int? id, int? divid, int? days)
        {
            var q = from p in DbUtil.Db.RecentAbsents(id, divid, days ?? 36)
                    orderby p.OrganizationName, p.OrganizationId, p.Consecutive, p.Name2
                    select p;
            return View(q);
        }
        public ActionResult Meetings(MeetingsModel m)
        {
            return View(m);
        }
        public ActionResult Test()
        {
            return new TestResult();
        }
        public ActionResult QueryStats()
        {
            return new QueryStatsResult();
        }
        public ActionResult VitalStats()
        {
            ViewData["table"] = CmsData.QueryFunctions.VitalStats(DbUtil.Db);
            return View();
        }
        public class ExtraInfo
        {
            public string Field { get; set; }
            public string Value { get; set; }
            public int Count { get; set; }
        }
        public ActionResult ExtraValues()
        {
        	var ev = StandardExtraValues.GetExtraValues();
            var q = from e in DbUtil.Db.PeopleExtras
                    where e.StrValue != null || e.BitValue != null
                    group e by new { e.Field, val = e.StrValue ?? (e.BitValue == true ? "1" : "0") } into g
                    select new ExtraInfo
                    {
                        Field = g.Key.Field,
                        Value = g.Key.val,
                        Count = g.Count(),
                    };

        	var list = from e in q.ToList()
        	           let f = ev.SingleOrDefault(ff => ff.name == e.Field)
					   where f == null || f.UserCanView()
					   orderby e.Field
        	           select e;
            return View(list);
        }
        public ActionResult ExtraValueQuery(string field, string val)
        {
            var qb = DbUtil.Db.QueryBuilderScratchPad();
            qb.CleanSlate(DbUtil.Db);
            qb.AddNewClause(QueryType.PeopleExtra, CompareType.Equal, "{0}:{1}".Fmt(field, val));
            DbUtil.Db.SubmitChanges();
            return Redirect("/QueryBuilder/Main/" + qb.QueryId);
        }
        public ActionResult ExtraValuesGrid(int id, string sort)
        {
            var roles = CMSRoleProvider.provider.GetRolesForUser(Util.UserName);
            var xml = XDocument.Parse(DbUtil.Db.Content("StandardExtraValues.xml", "<Fields/>"));
            var fields = (from ff in xml.Root.Elements("Field")
                          let vroles = ff.Attribute("VisibilityRoles")
                          where vroles != null && (vroles.Value.Split(',').All(rr => !roles.Contains(rr)))
                          select ff.Attribute("name").Value);
            var nodisplaycols = string.Join("|", fields);

            var tag = DbUtil.Db.PopulateSpecialTag(id, DbUtil.TagTypeId_ExtraValues);
            var cmd = new SqlCommand("dbo.ExtraValues @p1, @p2, @p3");
			cmd.Parameters.AddWithValue("@p1", tag.Id);
			cmd.Parameters.AddWithValue("@p2", sort ?? "");
			cmd.Parameters.AddWithValue("@p3", nodisplaycols);
            cmd.Connection = new SqlConnection(Util.ConnectionString);
            cmd.Connection.Open();
            var rdr = cmd.ExecuteReader();
			ViewBag.queryid = id;
            return View(rdr);
        }
		public ActionResult FamilyDirectory(int id)
		{
			return new FamilyDir(id);
		}
		public ActionResult FamilyDirectoryCompact(int id)
		{
			return new CompactDir(id);
		}
		public ActionResult PictureDirectory(int id)
		{
			return new PictureDir(id);
		}
		public ActionResult EmployerAddress(int id)
		{
			return new EmployerAddress(id);
		}

    	public class QueryStatsResult : ActionResult
        {
            StringBuilder sb = new StringBuilder();
            public override void ExecuteResult(ControllerContext context)
            {
                var dt = DateTime.Parse("1/1/1900");
                var firstrunid = DateTime.Now.Date.Subtract(dt).Days - 200;
                var q = from s in DbUtil.Db.QueryStats
                        where s.RunId > firstrunid
                        group s by s.RunId into g
                        orderby g.Key descending
                        select new
                        {
                            g.Key,
                            list = from s in g.OrderBy(ss => ss.StatId)
                                   select new { Count = s.Count as int?, s.StatId }
                        };
                var rows = q.Count();

                var d = new List<Dictionary<string, string>>();

                var q3 = from s in DbUtil.Db.QueryStats
                         where s.RunId > firstrunid
                         group s by s.StatId into g
                         orderby g.Key
                         select new { g.Key, g.OrderByDescending(ss => ss.RunId).First().Description };

                var head = q3.ToDictionary(ss => ss.Key, ss => ss.Description);

                var Response = context.HttpContext.Response;
                foreach (var r in q)
                {
                    var row = new Dictionary<string, string>();
                    row["S00"] = dt.AddDays(r.Key).ToString("d");
                    foreach (var s in r.list)
                        row[s.StatId] = s.Count.ToString2("N0");
                    d.Add(row);
                }
                Response.Write("<table cellpadding=4>\n<tr><td>Date</td>");
                foreach (var c in head)
                    Response.Write("<td align='right'>{0}</td>".Fmt(c.Value));
                Response.Write("</tr>\n");
                foreach (var r in d)
                {
                    Response.Write("<tr><td>{0}</td>".Fmt(r["S00"]));
                    foreach (var c in head)
                    {
                        if (r.ContainsKey(c.Key))
                            Response.Write("<td align='right'>{0}</td>".Fmt(r[c.Key]));
                        else
                            Response.Write("<td></td>");
                    }
                    Response.Write("</tr>\n");
                }
                Response.Write("</table>");
            }
        }
    }
}
