using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMSWeb.Areas.Main.Models.Report;

namespace CMSWeb.Areas.Main.Controllers
{
    public class ReportsController : Controller
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
    }
}
