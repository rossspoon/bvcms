using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using UtilityExtensions;
using System.Text;
using CmsData;
using LumenWorks.Framework.IO.Csv;
using System.IO;
using CmsWeb.Models;
using CMSPresenter;

namespace CmsWeb.Areas.Manage.Controllers
{
    [Authorize(Roles = "Admin")]
    [ValidateInput(false)]
    public class BatchController : AsyncController
    {
        public ActionResult MoveAndDelete()
        {
            return View();
        }
        [HttpPost]
        [AsyncTimeout(600000)]
        public void MoveAndDeleteAsync(string text)
        {
            AsyncManager.OutstandingOperations.Increment();
            string host = Util.Host;
            ThreadPool.QueueUserWorkItem((e) =>
            {
                var sb = new StringBuilder();
                sb.Append("<h2>done</h2>\n<p><a href='/'>home</a></p>\n");
                using (var csv = new CsvReader(new StringReader(text), false, '\t'))
                {
                    while (csv.ReadNextRecord())
                    {
                        if (csv.FieldCount != 2)
                        {
                            sb.AppendFormat("expected two ids, row {0}<br/>\n", csv[0]);
                            continue;
                        }

                        var fromid = csv[0].ToInt();
                        var toid = csv[1].ToInt();
                        var Db = new CMSDataContext(Util.GetConnectionString(host));
                        var p = Db.LoadPersonById(fromid);
                        if (p == null)
                        {
                            sb.AppendFormat("fromid {0} not found<br/>\n", fromid);
                            Db.Dispose();
                            continue;
                        }
                        var tp = Db.LoadPersonById(toid);
                        if (tp == null)
                        {
                            sb.AppendFormat("toid {0} not found<br/>\n", toid);
                            Db.Dispose();
                            continue;
                        }
                        try
                        {
                            p.MovePersonStuff(Db, toid);
                            Db.SubmitChanges();
                        }
                        catch (Exception ex)
                        {
                            sb.AppendFormat("error on move ({0}, {1}): {2}<br/>\n", fromid, toid, ex.Message);
                            Db.Dispose();
                            continue;
                        }
                        try
                        {
                            Db.PurgePerson(fromid);
                            sb.AppendFormat("moved ({0}, {1}) successful<br/>\n", fromid, toid);
                        }
                        catch (Exception ex)
                        {
                            sb.AppendFormat("error on delete ({0}): {1}<br/>\n", fromid, ex.Message);
                        }
                        finally
                        {
                            Db.Dispose();
                        }
                    }
                }
                AsyncManager.Parameters["results"] = sb.ToString();
                AsyncManager.OutstandingOperations.Decrement();
            });
        }
        public ActionResult MoveAndDeleteCompleted(string results)
        {
            return Content(results);
        }
        public ActionResult Grade(string text)
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
        public ActionResult RegistrationMail(string text)
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
        public ActionResult UpdateOrg(string text)
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
            return Redirect("/Home");
        }
        public ActionResult UploadPeople(string text)
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

            var standardnames = new List<string>
            {
                "FamilyId", 
                "Title",	
                "First",	
                "Last",	
                "GoesBy",	
                "AltName",	
                "Gender",	
                "Married",	
                "Address",	
                "Address2",	
                "City",	
                "State",	
                "Zip",	
                "Position",	
                "Birthday",	
                "CellPhone",	
                "HomePhone",	
                "Email"
            };

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
                    var p = Person.Add(DbUtil.Db, false, f, 10, null, a[names["First"]], a[names["GoesBy"]], a[names["Last"]], a[names["Birthday"]], 0, 0, 0, null);
                    p.AltName = a[names["AltName"]];
                    p.CellPhone = a[names["CellPhone"]].GetDigits();
                    p.EmailAddress = a[names["Email"]].Trim();

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
                    var nq = from name in names.Keys
                             where !standardnames.Contains(name)
                             select name;
                    var dt = DateTime.Now;
                    foreach (var name in nq)
                        p.PeopleExtras.Add(new PeopleExtra 
                        {  
                            Field = name, 
                            StrValue = a[names[name]],
                            TransactionTime = dt
                        });
                }
                DbUtil.Db.SubmitChanges();
            }
            return Redirect("/Home");
        }
        public ActionResult UpdatePeople0(string text)
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
            return Redirect("/Home");
        }
        public ActionResult UpdateFields()
        {
            var m = new UpdateFieldsModel();
            if (Request.HttpMethod.ToUpper() == "GET")
            {
                var success = (string)TempData["success"];
                if (success.HasValue())
                    ViewData["success"] = success;
                ViewData["text"] = "";
                return View(m);
            }
            UpdateModel(m);
            var tag = DbUtil.Db.TagById(m.Tag.ToInt());
            var q = tag.People(DbUtil.Db);
            switch (m.Field)
            {
                case "Member Status":
                    foreach (var p in q)
                        p.MemberStatusId = m.NewValue.ToInt();
                    break;
                case "Campus":
                    foreach (var p in q)
                        p.CampusId = m.NewValue.ToInt();
                    break;
                case "Marital Status":
                    foreach (var p in q)
                        p.MaritalStatusId = m.NewValue.ToInt();
                    break;
                case "Family Position":
                    foreach (var p in q)
                        p.PositionInFamilyId = m.NewValue.ToInt();
                    break;
                case "Gender":
                    foreach (var p in q)
                        p.GenderId = m.NewValue.ToInt();
                    break;
                case "Occupation":
                    foreach (var p in q)
                        p.OccupationOther = m.NewValue;
                    break;
                case "School":
                    foreach (var p in q)
                        p.SchoolOther = m.NewValue;
                    break;
                case "Employer":
                    foreach (var p in q)
                        p.EmployerOther = m.NewValue;
                    break;
            }
            DbUtil.Db.SubmitChanges();
            TempData["success"] = m.Field + " Updated";
            return RedirectToAction("UpdateFields");
        }
        public ActionResult UpdatePeople()
        {
            if (Request.HttpMethod.ToUpper() == "GET")
            {
                ViewData["text"] = "";
                return View();
            }
            var file = Request.Files[0];
            if (file.ContentLength == 0)
                return Content("empty file");
            var path = Server.MapPath("/Upload/" + Guid.NewGuid().ToCode() + ".xls");
            file.SaveAs(path);
            try
            {
                UpdatePeopleModel.UpdatePeople(path, Util.Host, Util.UserPeopleId.Value);
            }
            finally
            {
                System.IO.File.Delete(path);
            }
            return Content("<div>done <a href='/'>go home</a><div>");
        }
        public ActionResult LookupDataPage()
        {
            return View(new UpdateFieldsModel().TitleItems());
        }
    }
}
