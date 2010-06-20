using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMSWeb.Models;
using CmsData;
using UtilityExtensions;
using System.Text.RegularExpressions;

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
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PasteSettings(OrgSearchModel m)
        {
            var frorg = DbUtil.Db.LoadOrganizationById((int)Session["OrgCopySettings"]);
            foreach (var o in m.OrganizationList())
            {
                var toorg = DbUtil.Db.LoadOrganizationById(o.Id);
                toorg.AgeFee = frorg.AgeFee;
                toorg.AgeGroups = frorg.AgeGroups;
                toorg.AllowLastYearShirt = frorg.AllowLastYearShirt;
                toorg.AllowOnlyOne = frorg.AllowOnlyOne;
                toorg.AskAllergies = frorg.AskAllergies;
                toorg.AskChurch = frorg.AskChurch;
                toorg.AskCoaching = frorg.AskCoaching;
                toorg.AskDoctor = frorg.AskDoctor;
                toorg.AskEmContact = frorg.AskEmContact;
                toorg.AskGrade = frorg.AskGrade;
                toorg.AskInsurance = frorg.AskInsurance;
                toorg.AskOptions = frorg.AskOptions;
                toorg.AskParents = frorg.AskParents;
                toorg.AskRequest = frorg.AskRequest;
                toorg.AskShirtSize = frorg.AskShirtSize;
                toorg.AskTickets = frorg.AskTickets;
                toorg.AskTylenolEtc = frorg.AskTylenolEtc;
                toorg.BirthDayEnd = frorg.BirthDayEnd;
                toorg.BirthDayStart = frorg.BirthDayStart;
                toorg.CanSelfCheckin = frorg.CanSelfCheckin;
                toorg.EmailAddresses = frorg.EmailAddresses;
                toorg.EmailMessage = frorg.EmailMessage;
                toorg.EmailSubject = frorg.EmailSubject;
                toorg.ExtraFee = frorg.ExtraFee;
                toorg.Fee = frorg.Fee;
                toorg.GenderId = frorg.GenderId;
                toorg.GradeAgeStart = frorg.GradeAgeStart;
                toorg.GradeAgeEnd = frorg.GradeAgeEnd;
                toorg.Instructions = frorg.Instructions;
                toorg.LastDayBeforeExtra = frorg.LastDayBeforeExtra;
                toorg.MaximumFee = frorg.MaximumFee;
                toorg.MemberOnly = frorg.MemberOnly;
                toorg.NumCheckInLabels = frorg.NumCheckInLabels;
                toorg.NumWorkerCheckInLabels = frorg.NumWorkerCheckInLabels;
                toorg.RegistrationTypeId = frorg.RegistrationTypeId;
                toorg.ShirtFee = frorg.ShirtFee;
                toorg.Terms = frorg.Terms;
                toorg.ValidateOrgs = frorg.ValidateOrgs;
                toorg.YesNoQuestions = frorg.YesNoQuestions;
            }
            DbUtil.Db.SubmitChanges();
            return new EmptyResult();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateMeeting(string name)
        {
            var a = name.SplitStr(".");
            var orgid = a[1].ToInt();
            var organization = DbUtil.Db.LoadOrganizationById(orgid);
            if (organization == null)
                Util.ShowError("Bad Orgid ({0})".Fmt(name));

            var re = new Regex(@"\A(0[1-9]|1[012])(0[1-9]|[12][0-9]|3[01])([0-9]{2})([01][0-9])([0-5][0-9])\Z");
            if (!re.IsMatch(a[2]))
                Util.ShowError("Bad Date and Time ({0})".Fmt(name));
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
