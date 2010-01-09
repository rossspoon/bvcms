using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CmsData;
using UtilityExtensions;
using System.Text;
using CMSWeb.Models.Reports;

namespace CMSWeb.Areas.Manage.Controllers
{
    public class ReportsController : CmsController
    {
        public ActionResult Index()
        {
            return Content("no page");
        }

        public ActionResult Attendance(int id)
        {
            var m = new AttendanceModel(id);
            UpdateModel(m);
            return View(m);
        }
    }
}
