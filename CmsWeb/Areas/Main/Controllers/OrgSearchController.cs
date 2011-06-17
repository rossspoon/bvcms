using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsWeb.Models;
using CmsData;
using UtilityExtensions;
using System.Text.RegularExpressions;

namespace CmsWeb.Areas.Main.Controllers
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
            public int? OnlineReg { get; set; }
        }
        private const string STR_OrgSearch = "OrgSearch2";
        public ActionResult Index(int? div, int? progid)
        {
            Response.NoCache();
            var m = new OrgSearchModel();

            if (div.HasValue)
            {
                m.DivisionId = div;
                if (progid.HasValue)
                    m.ProgramId = progid;
                else
                    m.ProgramId = m.Division().ProgId;
                m.TagProgramId = m.ProgramId;
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
                m.OnlineReg = os.OnlineReg;
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
            var m = new OrgSearchModel { ProgramId = id };
            return View(m);
            //return Json(OrgSearchModel.DivisionIds(id));
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult TagDivIds(int id)
        {
            var m = new OrgSearchModel { ProgramId = id };
            return View("DivisionIds", m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DefaultMeetingDate(int id)
        {
            var dt = OrgSearchModel.DefaultMeetingDate(id);
            return Json(new { date = dt.Date.ToShortDateString(), time = dt.ToShortTimeString() });
        }
        public ActionResult ExportExcel(int prog, int div, int schedule, int status, int campus, string name)
        {
            var m = new OrgSearchModel 
            { 
                ProgramId = prog, 
                DivisionId = div, 
                ScheduleId = schedule, 
                StatusId = status, 
                CampusId = campus, 
                Name = name 
            };
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
                OnlineReg = m.OnlineReg,
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
            public string ChangeMain;
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ToggleTag(int id, int tagdiv)
        {
            var Db = DbUtil.Db;
            var organization = Db.LoadOrganizationById(id);
            if (tagdiv == 0)
                return Json(new { error = "bad tagdiv" });
            bool t = organization.ToggleTag(DbUtil.Db, tagdiv);
            var r = new ToggleTagReturn
            {
                value = t ? "Remove" : "Add",
            };
            if (t)
                r.ChangeMain = "Make Main";
            Db.SubmitChanges();
            return Json(r);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MainDiv(int id, int tagdiv)
        {
            var Db = DbUtil.Db;
            var o = Db.LoadOrganizationById(id);
            o.DivisionId = tagdiv;
            Db.SubmitChanges();
            return Content("ok");
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PasteSettings(OrgSearchModel m)
        {
            var frorg = (int)Session["OrgCopySettings"];
            foreach (var o in m.OrganizationList())
            {
                var toorg = DbUtil.Db.LoadOrganizationById(o.Id);
                toorg.CopySettings(DbUtil.Db, frorg);
            }
            return new EmptyResult();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateMeeting(string name)
        {
            var n = name.ToCharArray().Count(c => c == 'M');
            if (n > 1)
                return RedirectShowError("More than one barcode string found({0})".Fmt(name));
            var a = name.SplitStr(".");
            var orgid = a[1].ToInt();
            var organization = DbUtil.Db.LoadOrganizationById(orgid);
            if (organization == null)
                return RedirectShowError("Cannot interpret barcode orgid({0})".Fmt(name));

            var re = new Regex(@"\A(0[1-9]|1[012])(0[1-9]|[12][0-9]|3[01])([0-9]{2})([01][0-9])([0-5][0-9])\Z");
            if (!re.IsMatch(a[2]))
                return RedirectShowError("Cannot interpret barcode datetime({0})".Fmt(name));
            var g = re.Match(a[2]);
            var dt = new DateTime(
                g.Groups[3].Value.ToInt() + 2000,
                g.Groups[1].Value.ToInt(),
                g.Groups[2].Value.ToInt(),
                g.Groups[4].Value.ToInt(),
                g.Groups[5].Value.ToInt(),
                0);
            var newMtg = DbUtil.Db.Meetings.SingleOrDefault(m => m.OrganizationId == orgid && m.MeetingDate == dt);
            if (newMtg == null)
            {
                newMtg = new CmsData.Meeting
                {
                    CreatedDate = Util.Now,
                    CreatedBy = Util.UserId1,
                    OrganizationId = orgid,
                    GroupMeetingFlag = false,
                    Location = organization.Location,
                    MeetingDate = dt,
                };
                DbUtil.Db.Meetings.InsertOnSubmit(newMtg);
                DbUtil.Db.SubmitChanges();
                DbUtil.LogActivity("Creating new meeting for {0}".Fmt(dt));
            }
            return Redirect("~/Meeting.aspx?edit=1&id=" + newMtg.MeetingId);
        }
    }
}
