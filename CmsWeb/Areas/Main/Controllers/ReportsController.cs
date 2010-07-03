using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMSWeb.Areas.Main.Models.Report;

namespace CMSWeb.Areas.Main.Controllers
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
            return new FamilyResult(id);
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
        public ActionResult Rollsheet(int? id, string org, int? div, int? schedule, string name, DateTime? dt, int? meetingid, int? bygroup, string sgprefix)
        {
            return new RollsheetResult
            {
                qid = id,
                orgid = org == "curr" ? (int?)UtilityExtensions.Util.CurrentOrgId : null,
                groupid = org == "curr" ? (int?)UtilityExtensions.Util.CurrentGroupId : null,
                div = div,
                name = name,
                schedule = schedule,
                meetingid = meetingid,
                bygroup = bygroup.HasValue,
                sgprefix = sgprefix,
                dt = dt,
            };
        }
        public ActionResult OrgLeaders(string org, int? div, int? schedule, string name)
        {
            return new OrgLeadersResult
            {
                orgid = org == "curr" ? (int?)UtilityExtensions.Util.CurrentOrgId : null,
                div = div,
                name = name,
                schedule = schedule,
            };
        }
        public ActionResult ClassList(string org, int? div, int? schedule, string name)
        {
            return new ClassListResult
            {
                orgid = org == "curr" ? (int?)UtilityExtensions.Util.CurrentOrgId : null,
                div = div,
                name = name,
                schedule = schedule,
            };
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
        public ActionResult AveryAddress(int? id, string format, bool? titles)
        {
            if (!id.HasValue)
                return Content("no query");
            return new AveryAddressResult { id = id, format = format, titles = titles };
        }
        public ActionResult Registration(int? id)
        {
            if (!id.HasValue)
                return Content("no query");
            return new RegistrationResult(id);
        }
        public ActionResult ChurchAttendance(DateTime? id)
        {
            if (!id.HasValue)
                id = ChurchAttendanceModel.MostRecentAttendedSunday();
            var m = new ChurchAttendanceModel(id.Value);
            return View(m);
        }
    }
}
