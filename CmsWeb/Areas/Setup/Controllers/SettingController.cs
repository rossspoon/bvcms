using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CmsData;
using UtilityExtensions;
using CmsWeb.Models;
using LumenWorks.Framework.IO.Csv;
using System.IO;
using System.Text;
using System.Threading;

namespace CmsWeb.Areas.Setup.Controllers
{
    [Authorize(Roles = "Admin")]
    [ValidateInput(false)]
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
            DbUtil.Db.SetSetting(id, value);
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

            foreach (var b in adds)
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
                for (var c = 1; c < a.Length; c++)
                    switch (names[c].Trim())
                    {
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
                        case "AllowOnlyOne":
                            o.AllowOnlyOne = a[c].ToBool2();
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
                        case "EmailAddresses":
                            o.EmailAddresses = a[c];
                            break;
                        case "ExtraFee":
                            o.ExtraFee = a[c].ToDecimal();
                            break;
                        case "Fee":
                            o.Fee = a[c].ToDecimal();
                            break;
                        case "FirstMeeting":
                            o.FirstMeetingDate = a[c].ToDate();
                            break;
                        case "GradeAgeEnd":
                            o.GradeAgeEnd = a[c].ToInt2();
                            break;
                        case "GenderId":
                            o.GenderId = a[c].ToInt2();
                            break;
                        case "GradeAgeStart":
                            o.GradeAgeStart = a[c].ToInt2();
                            break;
                        case "IsBibleFellowshipOrg":
                            o.IsBibleFellowshipOrg = a[c].ToBool2();
                            break;
                        case "LastDayBeforeExtra":
                            o.LastDayBeforeExtra = a[c].ToDate();
                            break;
                        case "LastMeeting":
                            o.LastMeetingDate = a[c].ToDate();
                            break;
                        case "Limit":
                            o.Limit = a[c].ToInt2();
                            break;
                        case "Location":
                            o.Location = a[c];
                            break;
                        case "MaximumFee":
                            o.MaximumFee = a[c].ToDecimal();
                            break;
                        case "MemberOnly":
                            o.MemberOnly = a[c].ToBool2();
                            break;
                        case "Name":
                            o.OrganizationName = a[c];
                            break;
                        case "NoSecurityLabel":
                            o.NoSecurityLabel = a[c].ToBool2();
                            break;
                        case "NumCheckInLabels":
                            o.NumCheckInLabels = a[c].ToInt2();
                            break;
                        case "NumWorkerCheckInLabels":
                            o.NumWorkerCheckInLabels = a[c].ToInt2();
                            break;
                        case "NotReqAddr":
                            o.NotReqAddr = a[c].ToBool2();
                            break;
                        case "NotReqDOB":
                            o.NotReqDOB = a[c].ToBool2();
                            break;
                        case "NotReqGender":
                            o.NotReqGender = a[c].ToBool2();
                            break;
                        case "NotReqMarital":
                            o.NotReqMarital = a[c].ToBool2();
                            break;
                        case "NotReqPhone":
                            o.NotReqPhone = a[c].ToBool2();
                            break;
                        case "NotReqZip":
                            o.NotReqZip = a[c].ToBool2();
                            break;
                        case "OnLineCatalogSort":
                            o.OnLineCatalogSort = a[c] == "0" ? (int?)null : a[c].ToInt2();
                            break;
                        case "PhoneNumber":
                            o.PhoneNumber = a[c];
                            break;
                        case "RegistrationTypeId":
                            o.RegistrationTypeId = a[c].ToInt();
                            break;
                        case "RollSheetVisitorWks":
                            o.RollSheetVisitorWks = a[c] == "0" ? (int?)null : a[c].ToInt2();
                            break;
                        case "ShirtFee":
                            o.ShirtFee = a[c].ToDecimal();
                            break;
                        case "YesNoQuestions":
                            o.YesNoQuestions = a[c];
                            break;
                    }
                DbUtil.Db.SubmitChanges();
            }
            return Redirect("/");
        }
        public ActionResult BatchUploadPeople(string text)
        {
            if (Request.HttpMethod.ToUpper() == "GET")
            {
                ViewData["text"] = "";
                return View();
            }
            var list = text.Split('\n').Select(li => li.Split('\t'));
            var list0 = list.First().ToList();
            var names = list0.ToDictionary(i => i.TrimEnd(),
                i => list0.FindIndex(s => s == i));

            if (names.ContainsKey("Campus"))
            {
                var campuslist = (from li in list.Skip(1)
                                  where li.Length == names.Count
                                  group li by li[names["Campus"]] into campus
                                  select campus.Key).ToList();
                var dbc = from c in campuslist
                          join cp in DbUtil.Db.Campus on c equals cp.Description into j
                          from cp in j.DefaultIfEmpty()
                          select new { cp, c };
                var clist = dbc.ToList();
                var maxcampusid = DbUtil.Db.Campus.Max(c => c.Id);
                foreach (var i in clist)
                    if (i.cp == null)
                    {
                        var cp = new Campu { Description = i.c, Id = ++maxcampusid };
                        DbUtil.Db.Campus.InsertOnSubmit(cp);
                    }
            }
            DbUtil.Db.SubmitChanges();
            var campuses = DbUtil.Db.Campus.ToDictionary(cp => cp.Description, cp => cp.Id);

            var q = from li in list.Skip(1)
                    where li.Length == names.Count
                    group li by li[names["FamilyId"]] into fam
                    select fam;

            foreach (var fam in q)
            {
                var f = new Family();
                DbUtil.Db.Families.InsertOnSubmit(f);
                var line0 = fam.First();
                f.AddressLineOne = line0[names["Address"]];
                f.AddressLineTwo = line0[names["Address2"]];
                f.CityName = line0[names["City"]];
                f.StateCode = line0[names["State"]];
                f.ZipCode = line0[names["Zip"]];
                f.HomePhone = line0[names["HomePhone"]];
                DbUtil.Db.SubmitChanges();

                foreach (var a in fam)
                {
                    var p = Person.Add(f, 10, null, a[names["First"]], a[names["GoesBy"]], a[names["Last"]], a[names["Birthday"]], false, 0, 0, null);
                    p.AltName = a[names["AltName"]];
                    p.CellPhone = a[names["CellPhone"]].GetDigits();
                    p.EmailAddress = a[names["Email"]].GetDigits();

                    switch (a[names["Gender"]])
                    {
                        case "Male":
                            p.GenderId = 1;
                            break;
                        case "Female":
                            p.GenderId = 2;
                            break;
                    }
                    switch (a[names["Married"]])
                    {
                        case "Married":
                            p.MaritalStatusId = 20;
                            break;
                        case "Single":
                            p.MaritalStatusId = 10;
                            break;
                        default:
                            p.MaritalStatusId = 0;
                            break;
                    }
                    p.TitleCode = a[names["Title"]];
                    switch (a[names["Position"]])
                    {
                        case "Primary":
                            p.PositionInFamilyId = 10;
                            break;
                        case "Secondary":
                            p.PositionInFamilyId = 20;
                            break;
                        case "Child":
                            p.PositionInFamilyId = 30;
                            break;
                    }
                    p.AddressLineOne = a[names["Address"]];
                    p.AddressLineTwo = a[names["Address2"]];
                    p.CityName = a[names["City"]];
                    p.StateCode = a[names["State"]];
                    p.ZipCode = a[names["Zip"]];

                    if (names.ContainsKey("Campus"))
                        p.CampusId = campuses[a[names["Campus"]]];

                    if (p.PossibleDuplicates().Count() > 0)
                        p.HasDuplicates = true;
                }
                DbUtil.Db.SubmitChanges();
            }
            return Redirect("/");
        }
        public ActionResult BatchUpdatePeople(string text)
        {
            if (Request.HttpMethod.ToUpper() == "GET")
            {
                ViewData["text"] = "";
                return View();
            }
            var list = text.Split('\n').Select(li => li.Split('\t'));
            var list0 = list.First().ToList();
            var names = list0.ToDictionary(i => i.TrimEnd(),
                i => list0.FindIndex(s => s == i));

            if (names.ContainsKey("Campus"))
            {
                var campuslist = (from li in list.Skip(1)
                                  where li.Length == names.Count
                                  group li by li[names["Campus"]] into campus
                                  select campus.Key).ToList();
                var dbc = from c in campuslist
                          join cp in DbUtil.Db.Campus on c equals cp.Description into j
                          from cp in j.DefaultIfEmpty()
                          select new { cp, c };
                var clist = dbc.ToList();
                var maxcampusid = DbUtil.Db.Campus.Max(c => c.Id);
                foreach (var i in clist)
                    if (i.cp == null)
                    {
                        var cp = new Campu { Description = i.c, Id = ++maxcampusid };
                        DbUtil.Db.Campus.InsertOnSubmit(cp);
                    }
            }
            DbUtil.Db.SubmitChanges();
            var campuses = DbUtil.Db.Campus.ToDictionary(cp => cp.Description, cp => cp.Id);

            var q = from li in list.Skip(1)
                    where li.Length == names.Count
                    select li;

            foreach (var li in q)
            {
                DateTime? dob = null;
                if (names.ContainsKey("Birthday"))
                    dob = li[names["Birthday"]].ToDate();
                string email = "", cell = "", homephone = "";
                if (names.ContainsKey("CellPhone"))
                    cell = li[names["CellPhone"]].GetDigits();
                if (names.ContainsKey("Email"))
                    email = li[names["Email"]].Trim();
                if (names.ContainsKey("HomePhone"))
                    homephone = li[names["HomePhone"]].GetDigits();
                var first = li[names["First"]].Trim();
                var last = li[names["Last"]].Trim();
                var pid = DbUtil.Db.FindPerson3(first, last, dob, email, cell, homephone, null).FirstOrDefault();
                if (pid == null)
                    continue;
                var p = DbUtil.Db.LoadPersonById(pid.PeopleId.Value);

                foreach (var name in names)
                    switch (name.Key)
                    {
                        case "Title":
                            p.TitleCode = li[name.Value];
                            break;
                        case "Campus":
                            p.CampusId = campuses[li[name.Value]];
                            break;
                        case "Gender":
                            switch (li[name.Value])
                            {
                                case "Male":
                                    p.GenderId = 1;
                                    break;
                                case "Female":
                                    p.GenderId = 2;
                                    break;
                            }
                            break;
                        case "Married":
                            switch (li[name.Value])
                            {
                                case "Married":
                                    p.MaritalStatusId = 20;
                                    break;
                                case "Single":
                                    p.MaritalStatusId = 10;
                                    break;
                                default:
                                    p.MaritalStatusId = 0;
                                    break;
                            }
                            break;
                        case "Address":
                            p.AddressLineOne = li[name.Value];
                            break;
                        case "Address2":
                            p.AddressLineTwo = li[name.Value];
                            break;
                        case "City":
                            p.CityName = li[name.Value];
                            break;
                        case "State":
                            p.StateCode = li[name.Value];
                            break;
                        case "Zip":
                            p.ZipCode = li[name.Value];
                            break;
                        case "Position":
                            switch (li[name.Value])
                            {
                                case "Primary":
                                    p.PositionInFamilyId = 10;
                                    break;
                                case "Secondary":
                                    p.PositionInFamilyId = 20;
                                    break;
                                case "Child":
                                    p.PositionInFamilyId = 30;
                                    break;
                            }
                            break;
                        case "Birthday":
                            p.DOB = dob.ToString2("d");
                            break;
                        case "CellPhone":
                            p.CellPhone = cell;
                            break;
                        case "HomePhone":
                            p.HomePhone = homephone;
                            break;
                        case "Email":
                            p.EmailAddress = email;
                            break;
                    }
                DbUtil.Db.SubmitChanges();
            }
            return Redirect("/");
        }
    }
}






