using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CmsData;
using UtilityExtensions;

namespace CMSWeb.Areas.Setup.Controllers
{
   [Authorize(Roles = "Admin")]
    public class SettingController : CmsStaffController
    {
        public ActionResult Index()
        {
            var m = DbUtil.Db.Settings.AsEnumerable();
            return View(m);
        }

        [AcceptVerbs(HttpVerbs.Post)] 
        public ActionResult Create(string id)
        {
            var m = new Setting { Id = id };
            DbUtil.Db.Settings.InsertOnSubmit(m);
            DbUtil.Db.SubmitChanges();
            return Redirect("/Setup/Setting/");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult Edit(string id, string value)
        {
            var set = DbUtil.Db.Settings.SingleOrDefault(m => m.Id == id);
            set.SettingX = value;
            DbUtil.Db.SubmitChanges();
            DbUtil.SetSetting(id, value);
            var c = new ContentResult();
            c.Content = set.SettingX;
            return c;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public EmptyResult Delete(string id)
        {
            id = id.Substring(1);
            var set = DbUtil.Db.Settings.SingleOrDefault(m => m.Id == id);
            if (set == null)
                return new EmptyResult();
            DbUtil.Db.Settings.DeleteOnSubmit(set);
            DbUtil.Db.SubmitChanges();
            return new EmptyResult();
        }
        public ActionResult Batch(string text)
        {
            if (Request.HttpMethod.ToUpper() == "GET")
            {
                var q = from s in DbUtil.Db.Settings
                        orderby s.Id
                        select "{0}:\t{1}".Fmt(s.Id, s.SettingX);
                ViewData["text"] = string.Join("\n", q.ToArray());
                return View();
            }
            var batch = from s in text.Split('\n')
                        where s.HasValue()
                        let a = s.SplitStr(":", 2)
                        select new { name = a[0], value = a[1].Trim() };

            var settings = DbUtil.Db.Settings.ToList();

            var upds = from s in settings
                       join b in batch on s.Id equals b.name
                       select new { s = s, value = b.value };
            
            foreach (var pair in upds)
                pair.s.SettingX = pair.value;

            var adds = from b in batch
                       join s in settings on b.name equals s.Id into g
                       from s in g.DefaultIfEmpty()
                       where s == null
                       select b;

            foreach(var b in adds)
                DbUtil.Db.Settings.InsertOnSubmit(new Setting { Id = b.name, SettingX = b.value });

            var dels = from s in settings
                       where !batch.Any(b => b.name == s.Id)
                       select s;

            DbUtil.Db.Settings.DeleteAllOnSubmit(dels);
            DbUtil.Db.SubmitChanges();            

            return RedirectToAction("Index");
        }
        public ActionResult BatchReportSpecs(string text)
        {
            if (Request.HttpMethod.ToUpper() == "GET")
            {
                var q = from r in DbUtil.Db.ChurchAttReportIds
                        orderby r.Name
                        select "{0}:\t{1}".Fmt(r.Name, r.Id);
                ViewData["text"] = string.Join("\n", q.ToArray());
                return View();
            }
            var q2 = from s in text.Split('\n')
                     where s.HasValue()
                     let a = s.SplitStr(":", 2)
                     select new { name = a[0], value = a[1].ToInt() };
            foreach (var i in q2)
            {
                var set = DbUtil.Db.ChurchAttReportIds.SingleOrDefault(m => m.Name == i.name);
                if (set == null)
                {
                    set = new ChurchAttReportId { Name = i.name, Id = i.value };
                    DbUtil.Db.ChurchAttReportIds.InsertOnSubmit(set);
                }
                else
                    set.Id = i.value;
            }
            DbUtil.Db.SubmitChanges();
            return RedirectToAction("BatchReportSpecs");
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult DeleteImage(string id)
        {
            var iid = id.Substring(1).ToInt();
            var img = ImageData.DbUtil.Db.Images.SingleOrDefault(m => m.Id == iid);
            if (img == null)
                return Content("#r0");
            ImageData.DbUtil.Db.Images.DeleteOnSubmit(img);
            ImageData.DbUtil.Db.SubmitChanges();
            return Content("#r" + iid);
        }
        public ActionResult BatchGrade(string text)
        {
            if (Request.HttpMethod.ToUpper() == "GET")
            {
                ViewData["text"] = "";
                return View();
            }
            var batch = from s in text.Split('\n')
                        where s.HasValue()
                        let a = s.SplitStr("\t", 3)
                        select new { pid = a[0].ToInt(), oid = a[1].ToInt(), grade = a[2].ToInt() };
            foreach (var i in batch)
            {
                var m = DbUtil.Db.OrganizationMembers.Single(om => om.OrganizationId == i.oid && om.PeopleId == i.pid);
                m.Grade = i.grade;
            }
            DbUtil.Db.SubmitChanges();

            return Content("done");
        }
        public ActionResult BatchRegMail(string text)
        {
            if (Request.HttpMethod.ToUpper() == "GET")
            {
                ViewData["text"] = "";
                return View();
            }
            var batch = from s in text.Split('\n')
                        where s.HasValue()
                        let a = s.SplitStr("\t", 2)
                        select new { pid = a[0].ToInt(), em = a[1] };
            foreach (var i in batch)
            {
                var m = DbUtil.Db.OrganizationMembers.SingleOrDefault(om => om.OrganizationId == 88485 && om.PeopleId == i.pid);
                if (m == null)
                    continue;
                m.RegisterEmail = i.em;
            }
            DbUtil.Db.SubmitChanges();

            return Content("done");
        }
        public ActionResult BatchUpdateOrg(string text)
        {
            if (Request.HttpMethod.ToUpper() == "GET")
            {
                ViewData["text"] = "";
                return View();
            }
            var lines = text.Split('\n');
            var names = lines[0].Split('\t');

            for (var i = 1; i < lines.Length; i++)
            {
                var a = lines[i].Split('\t');
                var oid = a[0].ToInt();
                var o = DbUtil.Db.LoadOrganizationById(oid);
                for(var c = 1; c < a.Length; c++)
                    switch (names[c].Trim())
                    {
                        case "Name":
                            o.OrganizationName = a[c];
                            break;
                        case "FirstMeeting":
                            o.FirstMeetingDate = a[c].ToDate();
                            break;
                        case "Location":
                            o.Location = a[c];
                            break;
                        case "RollSheetVisitorWks":
                            o.RollSheetVisitorWks = a[c] == "0" ? (int?)null : a[c].ToInt2();
                            break;
                        case "AgeFee":
                            o.AgeFee = a[c];
                            break;
                        case "AgeGroups":
                            o.AgeGroups = a[c];
                            break;
                        case "CanSelfCheckin":
                            o.CanSelfCheckin = a[c].ToBool2();
                            break;
                        case "AllowKioskRegister":
                            o.AllowKioskRegister = a[c].ToBool2();
                            break;
                        case "AllowLastYearShirt":
                            o.AllowLastYearShirt = a[c].ToBool2();
                            break;
                        case "AskAllergies":
                            o.AskAllergies = a[c].ToBool2();
                            break;
                        case "AskChurch":
                            o.AskChurch = a[c].ToBool2();
                            break;
                        case "AskCoaching":
                            o.AskCoaching = a[c].ToBool2();
                            break;
                        case "AskDoctor":
                            o.AskDoctor = a[c].ToBool2();
                            break;
                        case "AskEmContact":
                            o.AskEmContact = a[c].ToBool2();
                            break;
                        case "AskGrade":
                            o.AskGrade = a[c].ToBool2();
                            break;
                        case "AskInsurance":
                            o.AskInsurance = a[c].ToBool2();
                            break;
                        case "AskOptions":
                            o.AskOptions = a[c];
                            break;
                        case "AskParents":
                            o.AskParents = a[c].ToBool2();
                            break;
                        case "AskRequest":
                            o.AskRequest = a[c].ToBool2();
                            break;
                        case "AskShirtSize":
                            o.AskShirtSize = a[c].ToBool2();
                            break;
                        case "AskTickets":
                            o.AskTickets = a[c].ToBool2();
                            break;
                        case "AskTylenolEtc":
                            o.AskTylenolEtc = a[c].ToBool2();
                            break;
                        case "BirthDayStart":
                            o.BirthDayStart = a[c].ToDate();
                            break;
                        case "BirthDayEnd":
                            o.BirthDayEnd = a[c].ToDate();
                            break;
                        case "Deposit":
                            o.Deposit = a[c].ToDecimal();
                            break;
                        case "GradeAgeEnd":
                            o.GradeAgeEnd = a[c].ToInt2();
                            break;
                        case "Limit":
                            o.Limit = a[c].ToInt2();
                            break;
                        case "Fee":
                            o.Fee = a[c].ToDecimal();
                            break;
                        case "Gender":
                            o.GenderId = a[c].ToInt2();
                            break;
                        case "GradeAgeStart":
                            o.GradeAgeStart = a[c].ToInt2();
                            break;
                        case "EmailAddresses":
                            o.EmailAddresses = a[c];
                            break;
                        case "MaximumFee":
                            o.MaximumFee = a[c].ToDecimal();
                            break;
                        case "MemberOnly":
                            o.MemberOnly = a[c].ToBool2();
                            break;
                        case "NumCheckInLabels":
                            o.NumCheckInLabels = a[c].ToInt2();
                            break;
                        case "NumWorkerCheckInLabels":
                            o.NumWorkerCheckInLabels = a[c].ToInt2();
                            break;
                        case "ShirtFee":
                            o.ShirtFee = a[c].ToDecimal();
                            break;
                        case "YesNoQuestions":
                            o.YesNoQuestions = a[c];
                            break;
                        case "LastDayBeforeExtra":
                            o.LastDayBeforeExtra = a[c].ToDate();
                            break;
                    }
                DbUtil.Db.SubmitChanges();
            }
            return Redirect("/");
        }
    }
}






