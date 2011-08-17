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
using System.Text.RegularExpressions;
using System.Data.SqlTypes;
using Alias = System.Threading.Tasks;

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
                        case "CanSelfCheckin":
                            o.CanSelfCheckin = a[c].ToBool2();
                            break;
                        case "AllowKioskRegister":
                            o.AllowKioskRegister = a[c].ToBool2();
                            break;
                        case "BirthDayStart":
                            o.BirthDayStart = a[c].ToDate();
                            break;
                        case "BirthDayEnd":
                            o.BirthDayEnd = a[c].ToDate();
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
                        case "OnLineCatalogSort":
                            o.OnLineCatalogSort = a[c] == "0" ? (int?)null : a[c].ToInt2();
                            break;
                        case "PhoneNumber":
                            o.PhoneNumber = a[c];
                            break;
                        case "RollSheetVisitorWks":
                            o.RollSheetVisitorWks = a[c] == "0" ? (int?)null : a[c].ToInt2();
                            break;
                    }
                DbUtil.Db.SubmitChanges();
            }
            return Redirect("/");
        }
        Dictionary<string, int> names;
        StringBuilder psb;
        StringBuilder fsb;
        public ActionResult UploadPeople(string text)
        {
            if (Request.HttpMethod.ToUpper() == "GET")
            {
                ViewData["text"] = "";
                return View();
            }

            var csv = new CsvReader(new StringReader(text), false, '\t');
            var list = csv.ToList();

            var list0 = list.First().ToList();
            names = list0.ToDictionary(i => i.TrimEnd(),
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
                "WorkPhone", 
                "Email", 
                "Email2", 
                "Suffix",
                "Middle",
            };

            foreach (var fam in q)
            {
                Family f = null;

                foreach (var a in fam)
                {
                    var first = a[names["First"]];
                    var last = a[names["Last"]];
                    DateTime dt;
                    DateTime? dob = null;
                    if (names.ContainsKey("Birthday"))
                        if (DateTime.TryParse(a[names["Birthday"]], out dt))
                        {
                            dob = dt;
                            if (dob.Value < SqlDateTime.MinValue)
                                dob = null;
                        }
                    string email = null;
                    string cell = null;
                    string homephone = null;
                    if (names.ContainsKey("Email"))
                        email = a[names["Email"]].Trim();
                    if (names.ContainsKey("CellPhone"))
                        cell = a[names["CellPhone"]].GetDigits();
                    if (names.ContainsKey("HomePhone"))
                        homephone = a[names["HomePhone"]].GetDigits();
                    Person p = null;
                    var pid = DbUtil.Db.FindPerson3(first, last, dob, email, cell, homephone, null).FirstOrDefault();
                    if (pid != null) // found
                    {
                        p = DbUtil.Db.LoadPersonById(pid.PeopleId.Value);
                        psb = new StringBuilder();
                        fsb = new StringBuilder();

                        UpdateField(p, a, "TitleCode", "Title");
                        UpdateField(p, a, "FirstName", "First");
                        UpdateField(p, a, "NickName", "GoesBy");
                        UpdateField(p, a, "LastName", "Last");
                        UpdateField(p, a, "EmailAddress", "Email");
                        UpdateField(p, a, "EmailAddress2", "Email2");
                        UpdateField(p, a, "DOB", "Birthday");
                        UpdateField(p, a, "AltName", "AltName");
                        UpdateField(p, a, "SuffixCode", "Suffix");
                        UpdateField(p, a, "MiddleName", "Middle");

                        UpdateField(p, a, "CellPhone", "CellPhone", GetDigits(a, "CellPhone"));
                        UpdateField(p, a, "WorkPhone", "WorkPhone", GetDigits(a, "WorkPhone"));
                        UpdateField(p, a, "GenderId", "Gender", Gender(a));
                        UpdateField(p, a, "MaritalStatusId", "Married", Marital(a));
                        UpdateField(p, a, "PositionInFamilyId", "Position", Position(a));
                        if (names.ContainsKey("Campus"))
                            UpdateField(p, a, "CampusId", "Campus", campuses[a[names["Campus"]]]);

                        UpdateField(p.Family, a, "AddressLineOne", "Address");
                        UpdateField(p.Family, a, "AddressLineTwo", "Address2");
                        UpdateField(p.Family, a, "CityName", "City");
                        UpdateField(p.Family, a, "StateCode", "State");
                        UpdateField(p.Family, a, "ZipCode", "Zip");

                        //UpdateField(p, a, "AddressLineOne", "Address");
                        //UpdateField(p, a, "AddressLineTwo", "Address2");
                        //UpdateField(p, a, "CityName", "City");
                        //UpdateField(p, a, "StateCode", "State");
                        //UpdateField(p, a, "ZipCode", "Zip");

                        p.LogChanges(DbUtil.Db, psb, Util.UserPeopleId.Value);
                        p.Family.LogChanges(DbUtil.Db, fsb, p.PeopleId, Util.UserPeopleId.Value);

                        DbUtil.Db.SubmitChanges();
                    }
                    else // new person
                    {
                        if (f == null || !a[names["FamilyId"]].HasValue())
                        {
                            f = new Family();
                            if (names.ContainsKey("Address"))
                                f.AddressLineOne = a[names["Address"]];
                            if (names.ContainsKey("Address2"))
                                f.AddressLineTwo = a[names["Address2"]];
                            if (names.ContainsKey("City"))
                                f.CityName = a[names["City"]];
                            if (names.ContainsKey("State"))
                                f.StateCode = a[names["State"]];
                            if (names.ContainsKey("Zip"))
                                f.ZipCode = a[names["Zip"]];
                            if (names.ContainsKey("HomePhone"))
                                f.HomePhone = a[names["HomePhone"]].GetDigits();
                            DbUtil.Db.Families.InsertOnSubmit(f);
                            DbUtil.Db.SubmitChanges();
                        }

                        string goesby = null;
                        if (names.ContainsKey("GoesBy"))
                            goesby = a[names["GoesBy"]];
                        p = Person.Add(DbUtil.Db, false, f, 10, null,
                            a[names["First"]],
                            goesby,
                            a[names["Last"]],
                            dob.FormatDate(),
                            0, 0, 0, null);
                        if (names.ContainsKey("AltName"))
                            p.AltName = a[names["AltName"]];

                        if (names.ContainsKey("Suffix"))
                            p.SuffixCode = a[names["Suffix"]];
                        if (names.ContainsKey("Middle"))
                            p.MiddleName = a[names["Middle"]];

                        if (names.ContainsKey("CellPhone"))
                            p.CellPhone = a[names["CellPhone"]].GetDigits();
                        if (names.ContainsKey("WorkPhone"))
                            p.WorkPhone = a[names["WorkPhone"]].GetDigits();
                        if (names.ContainsKey("Email"))
                            p.EmailAddress = a[names["Email"]].Trim();
                        if (names.ContainsKey("Email2"))
                            p.EmailAddress2 = a[names["Email2"]].Trim();
                        if (names.ContainsKey("Gender"))
                            p.GenderId = Gender(a);
                        if (names.ContainsKey("Married"))
                            p.MaritalStatusId = Marital(a);
                        if (names.ContainsKey("Position"))
                            p.PositionInFamilyId = Position(a);
                        if (names.ContainsKey("Title"))
                            p.TitleCode = a[names["Title"]];
                        if (names.ContainsKey("Campus"))
                            p.CampusId = campuses[a[names["Campus"]]];
                    }

                    var nq = from name in names.Keys
                             where !standardnames.Contains(name)
                             select name;
                    var now = DateTime.Now;
                    foreach (var name in nq)
                        p.SetExtra(name, a[names[name]].Trim());
                }
                DbUtil.Db.SubmitChanges();
            }
            return Redirect("/");
        }
        private void UpdateField(Family f, string[] a, string prop, string s)
        {
            if (names.ContainsKey(s))
                if (a[names[s]].HasValue())
                    f.UpdateValue(fsb, prop, a[names[s]]);
        }
        void UpdateField(Person p, string[] a, string prop, string s)
        {
            if (names.ContainsKey(s))
                if (a[names[s]].HasValue())
                    p.UpdateValue(psb, prop, a[names[s]]);
        }
        void UpdateField(Person p, string[] a, string prop, string s, object value)
        {
            if (names.ContainsKey(s))
                if (a[names[s]].HasValue())
                    p.UpdateValue(psb, prop, value);
        }
        string GetDigits(string[] a, string s)
        {
            if (names.ContainsKey(s))
                if (a[names[s]].HasValue())
                    return a[names[s]].GetDigits();
            return "";
        }
        int Gender(string[] a)
        {
            if (names.ContainsKey("Gender"))
                if (a[names["Gender"]].HasValue())
                {
                    var v = a[names["Gender"]].TrimEnd();
                    switch (v)
                    {
                        case "Male":
                            return 1;
                        case "Female":
                            return 2;
                    }
                }
            return 0;
        }
        int Marital(string[] a)
        {
            if (names.ContainsKey("Married"))
                if (a[names["Married"]].HasValue())
                {
                    var v = a[names["Married"]].TrimEnd();
                    switch (v)
                    {
                        case "Married":
                            return 20;
                        case "Single":
                            return 10;
                        case "Widowed":
                            return 50;
                        case "Divorced":
                            return 40;
                        case "Separated":
                            return 30;
                    }
                }
            return 0;
        }
        int Position(string[] a)
        {
            if (names.ContainsKey("Position"))
                if (a[names["Position"]].HasValue())
                {
                    var v = a[names["Position"]].TrimEnd();
                    switch (v)
                    {
                        case "Primary":
                            return 10;
                        case "Secondary":
                            return 20;
                        case "Child":
                            return 30;
                    }
                }
            return 10;
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
            return Redirect("/");
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
            foreach (var p in q)
            {
                switch (m.Field)
                {
                    case "Member Status":
                        p.MemberStatusId = m.NewValue.ToInt();
                        break;
                    case "Campus":
                        p.CampusId = m.NewValue.ToInt();
                        break;
                    case "Marital Status":
                        p.MaritalStatusId = m.NewValue.ToInt();
                        break;
                    case "Family Position":
                        p.PositionInFamilyId = m.NewValue.ToInt();
                        break;
                    case "Gender":
                        p.GenderId = m.NewValue.ToInt();
                        break;
                    case "Occupation":
                        p.OccupationOther = m.NewValue;
                        break;
                    case "School":
                        p.SchoolOther = m.NewValue;
                        break;
                    case "Employer":
                        p.EmployerOther = m.NewValue;
                        break;
                    case "Grade":
                        p.Grade = m.NewValue.ToInt();
                        break;
                }
                DbUtil.Db.SubmitChanges();
            }
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

        [AsyncTimeout(600000)]
        public void UpdateQueryBitsAsync()
        {
            AsyncManager.OutstandingOperations.Increment();
            string host = Util.Host;
            var uid = Util.UserId;
            ThreadPool.QueueUserWorkItem((e) =>
            {
                var Db = new CMSDataContext(Util.GetConnectionString(host));
                var u = Db.Users.SingleOrDefault(uu => uu.UserId == uid);
                Db.CurrentUser = u;
                Db.DeleteQueryBitTags();
                foreach (var a in QueryBitsFlags(Db))
                {
                    var t = Db.FetchOrCreateSystemTag(a[0]);
                    Db.TagAll(Db.PeopleQuery(a[0] + ":" + a[1]), t);
                    Db.SubmitChanges();
                }
                AsyncManager.OutstandingOperations.Decrement();
            });
        }
        public static IEnumerable<string[]> QueryBitsFlags(CMSDataContext Db)
        {
            var q = from c in Db.QueryBuilderClauses
                    where c.GroupId == null && c.Field == "Group"
                    where c.Description.StartsWith("F") && c.Description.Contains(":")
                    select c.Description;

            const string FindPrefix = @"^F\d+:.*";
            var re = new Regex(FindPrefix, RegexOptions.Singleline | RegexOptions.Multiline);
            var q2 = from s in q.ToList()
                     where re.Match(s).Success
                     let a = s.SplitStr(":", 2)
                     select new string[] { a[0], a[1] };
            return q2;
        }
        public ActionResult UpdateQueryBitsCompleted()
        {
            return Redirect("/");
        }

        public ActionResult DoVisits()
        {
            var q = from p in DbUtil.Db.People
                    where p.PeopleExtras.Any(pp => pp.FieldValue == "VisitEaster:1")
                    select p.PeopleId;

            foreach (var pid in q)
                Attend.RecordAttendance(pid, 4272905, true);

            DbUtil.Db.UpdateMeetingCounters(4272905);

            var j = from p in DbUtil.Db.People
                    where p.PeopleExtras.Any(pp => pp.FieldValue == "VisitEaster:2")
                    select p.PeopleId;

            foreach (var pid in j)
                Attend.RecordAttendance(pid, 4272907, true);

            DbUtil.Db.UpdateMeetingCounters(4272907);

            return Content("done");
        }
        public class FindInfo
        {
            public int? PeopleId { get; set; }
            public int Found { get; set; }
            public string First { get; set; }
            public string Last { get; set; }
            public string Email { get; set; }
            public string CellPhone { get; set; }
            public string HomePhone { get; set; }
            public DateTime? Birthday { get; set; }
        }
        [HttpGet]
        public ActionResult FindTagPeople()
        {
            return View("FindTagPeople0");
        }
        [HttpPost]
        string FindColumn(Dictionary<string, int> names, string[] a, string col)
        {
            if (names.ContainsKey(col))
                return a[names[col]];
            return null;
        }
        string FindColumnDigits(Dictionary<string, int> names, string[] a, string col)
        {
            var s = FindColumn(names, a, col);
            if (s.HasValue())
                return s.GetDigits();
            return s;
        }
        DateTime? FindColumnDate(Dictionary<string, int> names, string[] a, string col)
        {
            var s = FindColumn(names, a, col);
            DateTime dt;
            if (names.ContainsKey(col))
                if (DateTime.TryParse(a[names[col]], out dt))
                    return dt;
            return null;
        }
        public ActionResult FindTagPeople(string text, string tagname)
        {
            if (!tagname.HasValue())
                return Content("no tag");
            var csv = new CsvReader(new StringReader(text), false, '\t').ToList();

            var line0 = csv.First().ToList();
            names = line0.ToDictionary(i => i.TrimEnd(),
                i => line0.FindIndex(s => s == i));
            var ActiveNames = new List<string>
            {
                "First",
                "Last",
                "Birthday",
                "Email",
                "CellPhone",
                "HomePhone",
            };
            var hasvalidcolumn = false;
            foreach (var name in names.Keys)
                if (ActiveNames.Contains(name))
                {
                    hasvalidcolumn = true;
                    break;
                }
            if (!hasvalidcolumn)
                return Content("no valid column");


            var list = new List<FindInfo>();
            foreach (var a in csv.Skip(1))
            {
                var row = new FindInfo();
                row.First = FindColumn(names, a, "First");
                row.Last = FindColumn(names, a, "Last");
                row.Birthday = FindColumnDate(names, a, "Birthday");
                row.Email = FindColumn(names, a, "Email");
                row.CellPhone = FindColumnDigits(names, a, "CellPhone");
                row.HomePhone = FindColumnDigits(names, a, "HomePhone");

                var pids = DbUtil.Db.FindPerson3(row.First, row.Last, row.Birthday, row.Email, row.CellPhone, row.HomePhone, null);
                row.Found = pids.Count();
                if (row.Found == 1)
                    row.PeopleId = pids.Single().PeopleId.Value;
                list.Add(row);
            }
            var q = from pi in list
                    where pi.PeopleId.HasValue
                    select pi.PeopleId;
            foreach (var pid in q.Distinct())
                Person.Tag(DbUtil.Db, pid.Value, tagname, Util.UserPeopleId, DbUtil.TagTypeId_Personal);
            DbUtil.Db.SubmitChanges();

            return View(list);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult TagPeopleIds()
        {
            return View();
        }
        public ActionResult TagUploadPeopleIds(string name, HttpPostedFileBase file, string text)
        {
            string s;
            if (file != null)
            {
                byte[] buffer = new byte[file.ContentLength];
                file.InputStream.Read(buffer, 0, file.ContentLength);
                var enc = new System.Text.ASCIIEncoding();
                s = enc.GetString(buffer);
            }
            else
                s = text;

            var q = from line in s.Split('\n')
                    select line.ToInt();
            foreach (var pid in q)
                Person.Tag(DbUtil.Db, pid, name, DbUtil.Db.CurrentUser.PeopleId, (int)DbUtil.TagTypeId_Personal);
            DbUtil.Db.SubmitChanges();
            return Redirect("/Tags?tag=" + name);
        }
        [HttpGet]
        public ActionResult Test()
        {
            ViewData["guid"] = Guid.NewGuid().ToString();
            return View();
        }
        class LongRunningStatus
        {
            private static object lockobject = new object();
            private static Dictionary<string, int> Status { get; set; }
            public LongRunningStatus()
            {
                if (Status == null)
                    Status = new Dictionary<string, int>();
            }
            public void SetStatus(string id, int i)
            {
                lock (lockobject)
                    Status[id] = i;
            }
            public int GetStatus(string id)
            {
                lock (lockobject)
                    if (Status.Keys.Count(i => i == id) == 1)
                        return Status[id];
                return 0;
            }
            public void RemoveStatus(string id)
            {
                lock (lockobject)
                    Status.Remove(id);
            }
        }
        [HttpPost]
        public ActionResult TestStart(string id)
        {
            var i = 100;
            string host = Util.Host;
            Alias.Task.Factory.StartNew(() =>
            {
                var e = new LongRunningStatus();
                for (; i > 0; i--)
                {
                    var Db = new CMSDataContext(Util.GetConnectionString(host));
                    e.SetStatus(id, i);
                    Thread.Sleep(150);
                    Db.Dispose();
                }
                e.RemoveStatus(id);
            });
            return Content(i.ToString());
        }
        [HttpPost]
        public ActionResult TestProgress(string id)
        {
            var e = new LongRunningStatus();
            return Content(e.GetStatus(id).ToString());
        }
                        //case "AgeFee":
                        //    o.AgeFee = a[c];
                        //    break;
                        //case "AgeGroups":
                        //    o.AgeGroups = a[c];
                        //    break;
                        //case "AllowLastYearShirt":
                        //    o.AllowLastYearShirt = a[c].ToBool2();
                        //    break;
                        //case "AllowOnlyOne":
                        //    o.AllowOnlyOne = a[c].ToBool2();
                        //    break;
                        //case "AskAllergies":
                        //    o.AskAllergies = a[c].ToBool2();
                        //    break;
                        //case "AskChurch":
                        //    o.AskChurch = a[c].ToBool2();
                        //    break;
                        //case "AskCoaching":
                        //    o.AskCoaching = a[c].ToBool2();
                        //    break;
                        //case "AskDoctor":
                        //    o.AskDoctor = a[c].ToBool2();
                        //    break;
                        //case "AskEmContact":
                        //    o.AskEmContact = a[c].ToBool2();
                        //    break;
                        //case "AskGrade":
                        //    o.AskGrade = a[c].ToBool2();
                        //    break;
                        //case "AskInsurance":
                        //    o.AskInsurance = a[c].ToBool2();
                        //    break;
                        //case "AskOptions":
                        //    o.AskOptions = a[c];
                        //    break;
                        //case "AskParents":
                        //    o.AskParents = a[c].ToBool2();
                        //    break;
                        //case "AskRequest":
                        //    o.AskRequest = a[c].ToBool2();
                        //    break;
                        //case "AskShirtSize":
                        //    o.AskShirtSize = a[c].ToBool2();
                        //    break;
                        //case "AskTickets":
                        //    o.AskTickets = a[c].ToBool2();
                        //    break;
                        //case "AskTylenolEtc":
                        //    o.AskTylenolEtc = a[c].ToBool2();
                        //    break;
                        //case "Deposit":
                        //    o.Deposit = a[c].ToDecimal();
                        //    break;
                        //case "ExtraFee":
                        //    o.ExtraFee = a[c].ToDecimal();
                        //    break;
                        //case "Fee":
                        //    o.Fee = a[c].ToDecimal();
                        //    break;
                        //case "MaximumFee":
                        //    o.MaximumFee = a[c].ToDecimal();
                        //    break;
                        //case "MemberOnly":
                        //    o.MemberOnly = a[c].ToBool2();
                        //    break;
                        //case "NotReqAddr":
                        //    o.NotReqAddr = a[c].ToBool2();
                        //    break;
                        //case "NotReqDOB":
                        //    o.NotReqDOB = a[c].ToBool2();
                        //    break;
                        //case "NotReqGender":
                        //    o.NotReqGender = a[c].ToBool2();
                        //    break;
                        //case "NotReqMarital":
                        //    o.NotReqMarital = a[c].ToBool2();
                        //    break;
                        //case "NotReqPhone":
                        //    o.NotReqPhone = a[c].ToBool2();
                        //    break;
                        //case "NotReqZip":
                        //    o.NotReqZip = a[c].ToBool2();
                        //    break;
                        //case "RegistrationTypeId":
                        //    o.RegistrationTypeId = a[c].ToInt();
                        //    break;
                        //case "ShirtFee":
                        //    o.ShirtFee = a[c].ToDecimal();
                        //    break;
                        //case "YesNoQuestions":
                        //    o.YesNoQuestions = a[c];
                        //    break;
    }
}
