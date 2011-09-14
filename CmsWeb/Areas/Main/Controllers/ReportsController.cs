using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsWeb.Areas.Main.Models.Report;
using CmsData;
using System.IO;
using UtilityExtensions;
using CmsWeb.Models;

namespace CmsWeb.Areas.Main.Controllers
{
    public class ReportsController : CmsStaffController
    {
        //
        // GET: /Main/Report/

        public ActionResult Index()
        {
            return View();
        }
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
        public ActionResult Contacts(int? id)
        {
            if (!id.HasValue)
                return Content("no query");
            return new ContactsResult(id.Value);
        }
        public ActionResult Rollsheet(int? id, string org, int? pid, int? div, int? schedule, string name, DateTime? dt, int? meetingid, int? bygroup, string sgprefix, bool? altnames)
        {
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
                dt = dt,
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
                    where o.OrganizationId == orgid || orgid == 0 || orgid == null
                    where o.DivOrgs.Any(t => t.DivId == div) || div == 0 || div == null
                    where om.Organization.OrgSchedules.Any(sc => sc.ScheduleId == schedule || schedule == 0 || schedule == null)
                    group om.Person by om.Person.RecRegs.First().ShirtSize into g
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
        public ActionResult AveryAddress(int? id, string format, bool? titles, bool? usephone)
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
            };
        }
        public ActionResult RollLabels(int? id, string format, bool? titles, bool? usephone)
        {
            if (!id.HasValue)
                return Content("no query");
            return new RollLabelsResult {
                qid = id, 
                format = format, 
                titles = titles ?? false,
                usephone = usephone ?? false,
            };
        }
        public ActionResult Prospect(int? id, bool? Form)
        {
            if (!id.HasValue)
                return Content("no query");
            return new ProspectResult(id, Form ?? false);
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
        [Authorize(Roles = "Finance")]
        public ActionResult ContributionYears(int id)
        {
            var m = new ContributionModel(id);
            return View(m);
        }
        [Authorize(Roles = "Finance")]
        public ActionResult ContributionStatement(int id, DateTime FromDate, DateTime ToDate, int typ)
        {
            return new ContributionStatementResult { PeopleId = id, FromDate = FromDate, ToDate = ToDate, typ = typ };
        }
        private string CSE
        {
            get { return "CSE_" + Util.Host; }
        }
        [Authorize(Roles = "Finance")]
        public ActionResult ContributionStatements(string Submit, bool? PDF, DateTime? FromDate, DateTime? ToDate)
        {
            if (Request.HttpMethod.ToUpper() == "GET")
            {
                var m = HttpContext.Cache[CSE] as ContributionStatementsExtract;
#if DEBUG
                ViewData["FromDate"] = DateTime.Parse("7/1/11");
                ViewData["ToDate"] = DateTime.Parse("9/8/11");
#endif
                return View(m);
            }
            if (Submit == "Reset")
            {
                HttpContext.Cache.Remove(CSE);
                var m = null as ContributionStatementsExtract;
                return View(m);
            }
            else
            {
                if (!FromDate.HasValue || !ToDate.HasValue)
                    return Content("<h3>Must have a Startdate and Enddate</h3>");
                var m = new ContributionStatementsExtract(FromDate.Value, ToDate.Value, PDF.Value);
                HttpContext.Cache[CSE] = m;
                m.Run();
                return View(m);
            }
        }
        [Authorize(Roles = "Finance")]
        public ActionResult ContributionStatementsDownload()
        {
            if (HttpContext != null)
            {
                var m = HttpContext.Cache[CSE] as ContributionStatementsExtract;
                HttpContext.Cache.Remove(CSE);
                return new ContributionStatementsResult(m.OutputFile);
            }
            return Content("no session");
        }
        public ActionResult ChurchAttendance(DateTime? id)
        {
            if (!id.HasValue)
                id = ChurchAttendanceModel.MostRecentAttendedSunday();
            var m = new ChurchAttendanceModel(id.Value);
            return View(m);
        }
        public ActionResult ChurchAttendance2(DateTime? Dt1, DateTime? Dt2, string skipweeks)
        {
            if (!Dt1.HasValue)
                Dt1 = ChurchAttendanceModel.MostRecentAttendedSunday();
            if (!Dt2.HasValue)
                Dt2 = DateTime.Today;
            var m = new ChurchAttendance2Model(Dt1, Dt2, skipweeks);
            return View(m);
        }
        public ActionResult AttendanceDetail(DateTime? Dt1, DateTime? Dt2, string name, int? divid, int? schedid, int? campusid)
        {
            if (!Dt1.HasValue)
                Dt1 = ChurchAttendanceModel.MostRecentAttendedSunday();
            if (!Dt2.HasValue)
                Dt2 = Dt1.Value.AddDays(1);
            var m = new AttendanceDetailModel(Dt1.Value, Dt2, name, divid, schedid, campusid);
            return View(m);
        }
        public ActionResult Meetings(MeetingsModel m)
        {
            return View(m);
        }
        public ActionResult Test()
        {
            return new TestResult();
        }
    }
}
