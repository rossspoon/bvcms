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
    public class ContactSearchController : CmsStaffController
    {
        class ContactSearchInfo
        {
            public string ContacteeName { get; set; }
            public string ContactorName { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public int? ContactTypeId { get; set; }
            public int? ContactReasonId { get; set; }
            public int? StatusId { get; set; }
            public int? MinistryId { get; set; }
        }
        private const string STR_ContactSearch = "ContactSearch";
        public ActionResult Index()
        {
            Response.NoCache();
            var m = new ContactSearchModel();

            if (Session[STR_ContactSearch].IsNotNull())
            {
                var os = Session[STR_ContactSearch] as ContactSearchInfo;
                m.ContactReason = os.ContactReasonId;
                m.ContacteeName = os.ContacteeName;
                m.ContactorName = os.ContactorName;
                m.ContactType = os.ContactTypeId;
                m.Ministry = os.MinistryId;
                m.StartDate = os.StartDate;
                m.EndDate = os.EndDate;
                m.Status = os.StatusId;
            }
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Results(ContactSearchModel m)
        {
            SaveToSession(m);
            return View(m);
        }
        private void SaveToSession(ContactSearchModel m)
        {
            Session[STR_ContactSearch] = new ContactSearchInfo
            {
                EndDate = m.EndDate,
                StartDate = m.StartDate,
                MinistryId = m.Ministry,
                ContactTypeId = m.ContactType,
                ContactorName = m.ContactorName,
                ContacteeName = m.ContacteeName,
                ContactReasonId = m.ContactReason,
                StatusId = m.Status
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
    }
}
