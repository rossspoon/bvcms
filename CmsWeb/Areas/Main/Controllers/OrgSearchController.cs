using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMSWeb.Models;
using CmsData;
using UtilityExtensions;

namespace CMSWeb.Areas.Main.Controllers
{
    public class OrgSearchController : Controller
    {
        class OrgSearchInfo
        {
            public string Name { get; set; }
            public int? Program { get; set; }
            public int? Division { get; set; }
            public int? Sched { get; set; }
            public int? Status { get; set; }
        }
        private const string STR_OrgSearch = "OrgSearch";
        public ActionResult Index()
        {
            NoCache();
            var m = new OrgSearchModel();
            if (Session[STR_OrgSearch].IsNotNull())
            {
                var os = Session[STR_OrgSearch] as OrgSearchInfo;
                m.Name = os.Name;
                m.ProgramId = os.Program;
                m.DivisionId = os.Division;
                m.ScheduleId = os.Sched;
                m.StatusId = os.Status;
            }
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Results(OrgSearchModel m)
        {
            SaveToSession(m);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DivisionIds(int id)
        {
            return View(OrgSearchModel.DivisionIds(id));
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DefaultMeetingDate(int id)
        {
            var dt = OrgSearchModel.DefaultMeetingDate(id);
            return Json(new { date = dt.Date.ToShortDateString(), time = dt.ToShortTimeString() });
        }
        public ActionResult ExportExcel(int prog, int div, int schedule, int status, int campus, string name)
        {
            var m = new OrgSearchModel { ProgramId = prog, DivisionId = div, ScheduleId = schedule, StatusId = status, CampusId = campus, Name = name };
            return new OrgExcelResult(m);
        }
        private void SaveToSession(OrgSearchModel m)
        {
            Session[STR_OrgSearch] = new OrgSearchInfo
            {
                Name = m.Name,
                Program = m.ProgramId,
                Division = m.DivisionId,
                Sched = m.ScheduleId,
                Status = m.StatusId,
            };
        }
        private void NoCache()
        {
            var seconds = 10;
            Response.Cache.SetExpires(DateTime.Now.AddSeconds(seconds));
            Response.Cache.SetMaxAge(new TimeSpan(0, 0, seconds));
            Response.Cache.SetCacheability(HttpCacheability.Public);
            Response.Cache.SetValidUntilExpires(true);
            Response.Cache.SetSlidingExpiration(true);
            Response.Cache.SetETagFromFileDependencies();
        }
    }
}
