using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CmsData;
using UtilityExtensions;
using CmsWeb.Models;
using System.Xml;
using System.IO;
using System.Net.Mail;
using CmsData.Codes;
using System.Text;
using System.Net;
using CmsWeb.Areas.Manage.Controllers;

namespace CmsWeb.Areas.Public.Controllers
{
#if DEBUG
#else
   //[RequireHttps]
#endif
    public class APICheckin2Controller : CmsController
    {
        public ActionResult Match(string id, int campus, int thisday, int? page, string kiosk, bool? kioskmode)
        {
            if (!Authenticate())
                return Content("not authorized");
            Response.NoCache();
            DbUtil.Db.SetNoLock();
            DbUtil.LogActivity("checkin " + id, false);

            var m = new CheckInModel();
            var matches = m.Match(id, campus, thisday);

            if (matches.Count() == 0)
                return new FamilyResult(0, campus, thisday, 0, false, false); // not found
            if (matches.Count() == 1)
                return new FamilyResult(matches.Single().FamilyId, campus, thisday, 0, matches[0].Locked, kioskmode ?? false);
            return new MultipleResult(matches, page);
        }
        public ActionResult Family(int id, int campus, int thisday, string kiosk, bool? kioskmode)
        {
            if (!Authenticate())
                return Content("not authorized");
            Response.NoCache();
            DbUtil.Db.SetNoLock();
            return new FamilyResult(id, campus, thisday, 0, false, kioskmode ?? false);
        }
        public ActionResult Class(int id, int thisday)
        {
            if (!Authenticate())
                return Content("not authorized");
            Response.NoCache();
            DbUtil.Db.SetNoLock();
            return new ClassResult(id, thisday);
        }
        public ActionResult Classes(int id, int campus, int thisday, bool? noagecheck, bool? kioskmode)
        {
            if (!Authenticate())
                return Content("not authorized");
            Response.NoCache();
            DbUtil.Db.SetNoLock();
            return new ClassesResult(id, thisday, campus, noagecheck ?? false, kioskmode ?? false);
        }

        public ActionResult NameSearch(string id, int? page)
        {
            if (!Authenticate())
                return Content("not authorized");
            Response.NoCache();
            DbUtil.Db.SetNoLock();
            return new NameSearchResult2(id, page ?? 1);
        }
        public class PersonInfo
        {
            public string addr { get; set; }
            public string zip { get; set; }
            public string first { get; set; }
            public string last { get; set; }
            public string goesby { get; set; }
            public string dob { get; set; } // Date of Birth m/d/yyyy
            public string email { get; set; }
            public string cell { get; set; }
            public string home { get; set; } // home phone
            public string allergies { get; set; } // single line of allergies
            public string grade { get; set; } // grade -1 = preschool, 0 = kindergarten, 1 = first etc. 99 = special
            public string parent { get; set; } // name of parent
            public string emfriend { get; set; } // name of person bringing
            public string emphone { get; set; } // cell phone number of person bringing
            public string churchname { get; set; } // what church they go to
            public int marital { get; set; } // marital status (see lookup table for codes)
            public int gender { get; set; } // see lookup table for codes
            public int campusid { get; set; } // see lookup table for codes
            public string activeother { get; set; } // active in another church (true / false)
            public bool AskChurch { get; set; } // whether they were asked this
            public bool AskChurchName { get; set; } // or this
            public bool AskGrade { get; set; } // or this
            public bool AskEmFriend { get; set; } // or this
        }
        [HttpPost]
        public ActionResult AddPerson(int id, PersonInfo m)
        {
            if (!Authenticate())
                return Content("not authorized");

            CmsData.Family f;
            if (id > 0)
                f = DbUtil.Db.Families.Single(fam => fam.FamilyId == id);
            else
                f = new CmsData.Family();

            var position = PositionInFamily.Child;
            if (Util.Age0(m.dob) >= 18)
                if (f.People.Count(per =>
                     per.PositionInFamilyId == PositionInFamily.PrimaryAdult)
                     < 2)
                    position = PositionInFamily.PrimaryAdult;
                else
                    position = PositionInFamily.SecondaryAdult;

            var p = Person.Add(f, position,
                null, m.first, m.goesby, m.last, m.dob, false, m.gender,
                    OriginCode.Visit, null);

            UpdatePerson(p, m);
            return Content(f.FamilyId.ToString() + "." + p.PeopleId);
        }
        [HttpPost]
        public ActionResult EditPerson(int id, PersonInfo m)
        {
            if (!Authenticate())
                return Content("not authorized");
            var p = DbUtil.Db.LoadPersonById(id);
            UpdatePerson(p, m);
            return Content(p.FamilyId.ToString());
        }
        private bool Authenticate(bool log = false)
        {
            var auth = Request.Headers["Authorization"];
            if (auth.HasValue())
            {
                var cred = System.Text.ASCIIEncoding.ASCII.GetString(
                    Convert.FromBase64String(auth.Substring(6))).Split(':');
                var username = cred[0];
                var password = cred[1];

                var ret = false;
                if (password == DbUtil.Db.Setting("ImpersonatePassword", null))
                    ret = true;
                else
                    ret = CMSMembershipProvider.provider.ValidateUser(username, password);
                if (ret)
                {
                    var roles = CMSRoleProvider.provider;
                    var role = "Access";
                    if (roles.RoleExists("Checkin"))
                        role = "Checkin";
                    AccountController.SetUserInfo(username, Session);
                    if (!roles.IsUserInRole(username, role))
                        ret = false;
                }
                if (log)
                    if (ret)
                        DbUtil.LogActivity("checkin {0} authenticated".Fmt(username));
                    else
                        DbUtil.LogActivity("checkin {0} not authenticated".Fmt(username));
                return ret;
            }
            return false;
        }
        string Trim(string s)
        {
            if (s.HasValue())
                return s.Trim();
            else
                return s;
        }
        private void UpdatePerson(Person p, PersonInfo m)
        {
            var psb = new StringBuilder();
            var fsb = new StringBuilder();
            var z = DbUtil.Db.ZipCodes.SingleOrDefault(zc => zc.Zip == m.zip.Zip5());
            if (!m.home.HasValue() && m.cell.HasValue())
                m.home = m.cell;

            var keys = Request.Form.AllKeys.ToList();

            if (keys.Contains("home"))
                UpdateField(fsb, p.Family, "HomePhone", m.home.GetDigits());
            if (keys.Contains("addr"))
                UpdateField(fsb, p.Family, "AddressLineOne", m.addr);
            if (keys.Contains("zip"))
            {
                UpdateField(fsb, p.Family, "CityName", z != null ? z.City : null);
                UpdateField(fsb, p.Family, "StateCode", z != null ? z.State : null);
                UpdateField(fsb, p.Family, "ZipCode", m.zip);
            }
            if (keys.Contains("goesby"))
                UpdateField(psb, p, "NickName", Trim(m.goesby));
            if (keys.Contains("first"))
                UpdateField(psb, p, "FirstName", Trim(m.first));
            if (keys.Contains("last"))
                UpdateField(psb, p, "LastName", Trim(m.last));
            if (keys.Contains("dob"))
            {
                DateTime dt;
                DateTime.TryParse(m.dob, out dt);
                if (p.BirthDate != dt)
                    UpdateField(psb, p, "DOB", m.dob);
            }
            if (keys.Contains("email"))
                UpdateField(psb, p, "EmailAddress", Trim(m.email));
            if (keys.Contains("cell"))
                UpdateField(psb, p, "CellPhone", m.cell.GetDigits());
            if (keys.Contains("marital"))
                UpdateField(psb, p, "MaritalStatusId", m.marital);
            if (keys.Contains("gender"))
                UpdateField(psb, p, "GenderId", m.gender);
            p.LogChanges(DbUtil.Db, psb, Util.UserPeopleId ?? 0);
            p.Family.LogChanges(DbUtil.Db, fsb, p.PeopleId, Util.UserPeopleId ?? 0);

            var rr = p.GetRecReg();
            if (keys.Contains("allergies"))
                if (m.allergies != rr.MedicalDescription)
                    p.SetRecReg().MedicalDescription = m.allergies;
            if (keys.Contains("grade"))
                if (m.AskGrade)
                    if (m.grade.ToInt2() != p.Grade)
                        p.Grade = m.grade.ToInt2();
            if (m.AskEmFriend)
            {
                if (keys.Contains("parent"))
                    if (m.parent != rr.Mname)
                        p.SetRecReg().Mname = m.parent;
                if (keys.Contains("emfriend"))
                    if (m.emfriend != rr.Emcontact)
                        p.SetRecReg().Emcontact = m.emfriend;
                if (keys.Contains("emphone"))
                    if (m.emphone != rr.Emphone)
                        p.SetRecReg().Emphone = m.emphone;
            }
            if (keys.Contains("campusid"))
                if (m.campusid > 0)
                    p.CampusId = m.campusid;
            if (m.AskChurch)
                if (keys.Contains("activeother"))
                    if (m.activeother.ToBool() != rr.ActiveInAnotherChurch)
                        p.SetRecReg().ActiveInAnotherChurch = m.activeother.ToBool();
            if (m.AskChurchName)
                if (keys.Contains("churchname"))
                    p.OtherPreviousChurch = m.churchname;
            DbUtil.Db.SubmitChanges();
        }
        private void UpdateField(StringBuilder fsb, Family f, string prop, string value)
        {
            f.UpdateValue(fsb, prop, value);
        }
        void UpdateField(StringBuilder psb, Person p, string prop, string value)
        {
            p.UpdateValue(psb, prop, value);
        }
        void UpdateField(StringBuilder psb, Person p, string prop, object value)
        {
            p.UpdateValue(psb, prop, value);
        }
        public class CampusItem
        {
            public CmsData.Campu Campus { get; set; }
            public string password { get; set; }
        }
        public ActionResult Campuses()
        {
            if (!Authenticate(log: true))
                return Content("not authorized");
            var q = from c in DbUtil.Db.Campus
                    where c.Organizations.Any(o => o.CanSelfCheckin == true)
                    orderby c.Id
                    select new CampusItem
                    {
                        Campus = c,
                        password = DbUtil.Db.Setting("kioskpassword" + c.Id, "kio.")
                    };
            return View(q);
        }
        [HttpPost]
        public ContentResult RecordAttend(int PeopleId, int OrgId, bool Present, int thisday, string kiosk)
        {
            if (!Authenticate())
                return Content("not authorized");
            var m = new CheckInModel();
            m.RecordAttend(PeopleId, OrgId, Present, thisday);
            var r = new ContentResult();
            r.Content = "success";
            return r;
        }
        [HttpPost]
        public ContentResult RecordAttend2(int PeopleId, int OrgId, bool Present, DateTime hour, string kiosk)
        {
            if (!Authenticate())
                return Content("not authorized");
            var m = new CheckInModel();
            m.RecordAttend2(PeopleId, OrgId, Present, hour);
            var r = new ContentResult();
            r.Content = "success";
            return r;
        }
        [HttpPost]
        public ContentResult Membership(int PeopleId, int OrgId, bool Member)
        {
            if (!Authenticate())
                return Content("not authorized");
            var m = new CheckInModel();
            m.JoinUnJoinOrg(PeopleId, OrgId, Member);
            var r = new ContentResult();
            r.Content = "success";
            return r;
        }
        [Authorize(Roles = "Access")]
        public ActionResult CheckIn(int? id, int? pid)
        {
            Session.Timeout = 1000;
            Session["CheckInOrgId"] = id ?? 0;
            var m = new CheckInRecModel(id ?? 0, pid);
            return View(m);
        }
        [HttpPost]
        public JsonResult PostCheckIn(int id, string KeyCode)
        {
            Session["CheckInOrgId"] = id;
            var q = from kc in DbUtil.Db.CardIdentifiers
                    where KeyCode == kc.Id
                    select kc.PeopleId;
            var pid = q.SingleOrDefault();
            if (pid > 0)
            {
                var dt = Util.Now;
                var ck = new CheckInTime
                {
                    CheckInDay = dt.Date,
                    CheckInTimeX = dt,
                    OrganizationId = id,
                    PeopleId = pid,
                    //KeyCode = KeyCode
                };
                DbUtil.Db.CheckInTimes.InsertOnSubmit(ck);
                DbUtil.Db.SubmitChanges();
            }
            return Json(new { pid = pid });
        }
        [HttpPost]
        public ContentResult NewKeyCard(int pid, string KeyCode)
        {
            if (!Authenticate())
                return Content("not authorized");
            var p = DbUtil.Db.LoadPersonById(pid);
            if (p == null)
                return Content("No person to associate card with");
            var q = from kc in DbUtil.Db.CardIdentifiers
                    where KeyCode == kc.Id
                    select kc;
            var card = q.SingleOrDefault();
            if (card == null)
            {
                card = new CardIdentifier { Id = KeyCode };
                DbUtil.Db.CardIdentifiers.InsertOnSubmit(card);
            }
            card.PeopleId = pid;
            DbUtil.Db.SubmitChanges();
            return Content("Card Associated");
        }
        [HttpPost]
        public ContentResult Edit(string id, string value)
        {
            if (!Authenticate())
                return Content("not authorized");
            var a = id.Split('.');
            var c = new ContentResult();
            c.Content = value;
            var pid = a[1].ToInt();
            var p = DbUtil.Db.People.Single(pp => pp.PeopleId == pid);
            switch (a[0][0])
            {
                case 's':
                    p.SchoolOther = value;
                    break;
                case 'y':
                    p.Grade = value.ToInt();
                    break;
                case 'n':
                    p.CheckInNotes = value;
                    break;
            }
            DbUtil.Db.SubmitChanges();
            return c;
        }
        [HttpPost]
        public ContentResult UploadImage(int id)
        {
            if (!Authenticate())
                return Content("not authorized");
            var person = DbUtil.Db.People.Single(pp => pp.PeopleId == id);
            if (person.Picture == null)
                person.Picture = new Picture();
            byte[] bits = new byte[Request.InputStream.Length];
            Request.InputStream.Read(bits, 0, bits.Length);

            var p = person.Picture;
            p.CreatedDate = Util.Now;
            p.CreatedBy = Util.UserName;
            p.SmallId = ImageData.Image.NewImageFromBits(bits, 120, 120).Id;
            p.MediumId = ImageData.Image.NewImageFromBits(bits, 320, 400).Id;
            p.LargeId = ImageData.Image.NewImageFromBits(bits).Id;
            DbUtil.Db.SubmitChanges();
            return Content("done");
        }
        public ActionResult FetchImage(int id)
        {
            if (!Authenticate())
                return Content("not authorized");
            var person = DbUtil.Db.People.Single(pp => pp.PeopleId == id);
            if (person.PictureId != null)
                return new ImageResult(person.Picture.LargeId ?? 0);
            return new ImageResult(0);
        }
        public ActionResult CheckInList()
        {
            var m = from t in DbUtil.Db.CheckInTimes
                    orderby t.CheckInTimeX descending
                    select t;
            return View(m.Take(200));
        }
        [HttpPost]
        public ActionResult UnLockFamily(int fid)
        {
            if (!Authenticate())
                return Content("not authorized");
            var lockf = DbUtil.Db.FamilyCheckinLocks.SingleOrDefault(f => f.FamilyId == fid);
            if (lockf != null)
            {
                lockf.Locked = false;
                DbUtil.Db.SubmitChanges();
            }
            return new EmptyResult();
        }
        [HttpPost]
        public ActionResult ReportPrinterProblem(string kiosk, int campusid)
        {
            if (!Authenticate())
                return Content("not authorized");
            var setting = "KioskNotify";
            if (campusid > 0)
                setting += campusid;
            var address = DbUtil.Db.Setting(setting, null);
            if (address.HasValue())
            {
                try
                {
                    var msg = kiosk + " at " + DateTime.Now.ToShortTimeString();
                    Util.SendMsg(Util.SysFromEmail, DbUtil.Db.CmsHost,
                        Util.TryGetMailAddress(Util.SysFromEmail),
                        "Printer Problem", msg, Util.ToMailAddressList(address), 0, null);
                }
                catch (Exception)
                {
                }
            }
            return new EmptyResult();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ContentResult UploadPrintJob(string id)
        {
            if (!Authenticate())
                return Content("not authorized");

            var reader = new StreamReader(Request.InputStream);
            string job = reader.ReadToEnd();

            var m = new CheckInModel();
            m.SavePrintJob(id, job);
            return Content("done");
        }
        public ActionResult FetchPrintJobs(string id)
        {
            if (!Authenticate())
                return Content("not authorized");
            var m = new CheckInModel();
            var b = m.GetNextPrintJobs(id);
            return Content(b, "text/xml");
        }
    }
}
