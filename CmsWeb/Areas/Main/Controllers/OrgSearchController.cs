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
    public class OrgSearchController : CmsStaffController
    {
        class OrgSearchInfo
        {
            public string Name { get; set; }
            public int? Program { get; set; }
            public int? Division { get; set; }
            public int? Sched { get; set; }
            public int? Status { get; set; }
        }
        private const string STR_OrgSearch = "OrgSearch2";
        public ActionResult Index(int? div, int? progid)
        {
            Response.NoCache();
            var m = new OrgSearchModel();

            if (div.HasValue && progid.HasValue)
            {
                m.ProgramId = progid;
                m.TagProgramId = progid;
                m.DivisionId = div;
                m.TagDiv = div;
            }
            else if (Session[STR_OrgSearch].IsNotNull())
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
        public ActionResult TagDivIds(int id)
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
        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult Edit(string id, string value)
        {
            var a = id.Split('-');
            var c = new ContentResult();
            c.Content = value;
            var org = DbUtil.Db.LoadOrganizationById(a[1].ToInt());
            if (org == null)
                return c;
            switch (a[0])
            {
                case "bs":
                    org.BirthDayStart = value.ToDate();
                    break;
                case "be":
                    org.BirthDayEnd = value.ToDate();
                    break;
                case "ck":
                    org.CanSelfCheckin = value == "yes";
                    break;
            }
            DbUtil.Db.SubmitChanges();
            return c;
        }
        [Serializable]
        public class ToggleTagReturn
        {
            public string value;
            public string element;
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult ToggleTag(int id, int tagdiv, string element, bool main)
        {
            var Db = DbUtil.Db;
            var organization = Db.LoadOrganizationById(id);
            var r = new ToggleTagReturn 
            { 
                element = element,
                value = organization.ToggleTag(tagdiv, main) ? "Remove" : "Add"
            };
            Db.SubmitChanges();
            return Json(r);
        }
        public ActionResult UseOldOrgSearch()
        {
            DbUtil.Db.SetUserPreference("neworgsearch", "false");
            return Redirect("/OrganizationSearch.aspx");
        }
    }
}
