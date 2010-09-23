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

namespace CmsWeb.Areas.Main.Controllers
{
#if DEBUG
#else
   [RequireHttps]
#endif
    [RequireBasicAuthentication]
    public class Checkin2Controller : CmsController
    {
        public ActionResult Match(string id, int campus, int thisday, int? page, bool? kioskmode)
        {
            if (!Authenticate())
                return Content("not authorized");
            Response.NoCache();

            var m = new CheckInModel();
            var matches = m.Match(id, campus, thisday);

            if (matches.Count() == 0)
                return new FamilyResult(kioskmode, 0, campus, thisday, 0, false); // not found
            if (matches.Count() == 1)
                return new FamilyResult(kioskmode, matches.Single().FamilyId, campus, thisday, 0, matches[0].Locked);
            return new MultipleResult(matches, page);
        }
        public ActionResult Family(int id, int campus, int thisday, bool? kioskmode)
        {
            if (!Authenticate())
                return Content("not authorized");
            Response.NoCache();
            return new FamilyResult(kioskmode, id, campus, thisday, 0, false);
        }
        public ActionResult Class(int id, int thisday)
        {
            if (!Authenticate())
                return Content("not authorized");
            var org = DbUtil.Db.LoadOrganizationById(id);
            if (org == null)
                return new EmptyResult();
            Response.NoCache();
            return new ClassResult(id, thisday);
        }
        public ActionResult Classes(int id, int campus, int thisday, int page, bool? noagecheck, bool? kioskmode)
        {
            if (!Authenticate())
                return Content("not authorized");
            var p = DbUtil.Db.LoadPersonById(id);
            if (p == null)
                return new EmptyResult();
            Response.NoCache();
            return new ClassesResult(kioskmode, p, thisday, campus, page, noagecheck ?? false);
        }
        public ActionResult NameSearch(string id, int? page)
        {
            if (!Authenticate())
                return Content("not authorized");
            Response.NoCache();
            return new NameSearchResult2(id, page.Value);
        }
        public class PersonInfo
        {
            public string addr { get; set; }
            public string zip { get; set; }
            public string first { get; set; }
            public string last { get; set; }
            public string goesby { get; set; }
            public string dob { get; set; }
            public string email { get; set; }
            public string cell { get; set; }
            public string home { get; set; }
            public string allergies { get; set; }
            public string grade { get; set; }
            public string parent { get; set; }
            public string emfriend { get; set; }
            public string emphone { get; set; }
            public string churchname { get; set; }
            public int marital { get; set; }
            public int gender { get; set; }
            public int campusid { get; set; }
            public string activeother { get; set; }
            public bool AskChurch { get; set; }
            public bool AskChurchName { get; set; }
            public bool AskGrade { get; set; }
            public bool AskEmFriend { get; set; }
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddPerson(int id, PersonInfo m)
        {
            if (!Authenticate())
                return Content("not authorized");

            CmsData.Family f;
            if (id > 0)
                f = DbUtil.Db.Families.Single(fam => fam.FamilyId == id);
            else
                f = new CmsData.Family();

            var position = (int)CmsData.Family.PositionInFamily.Child;
            if (Util.Age0(m.dob) >= 18)
                if (f.People.Count(per =>
                     per.PositionInFamilyId == (int)CmsData.Family.PositionInFamily.PrimaryAdult)
                     < 2)
                    position = (int)CmsData.Family.PositionInFamily.PrimaryAdult;
                else
                    position = (int)CmsData.Family.PositionInFamily.SecondaryAdult;

            var p = Person.Add(f, position,
                null, m.first, m.goesby, m.last, m.dob, false, m.gender,
                    (int)Person.OriginCode.Visit, null);

            UpdatePerson(p, m);
            return Content(f.FamilyId.ToString() + "." + p.PeopleId);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditPerson(int id, PersonInfo m)
        {
            if (!Authenticate())
                return Content("not authorized");
            var p = DbUtil.Db.LoadPersonById(id);
            UpdatePerson(p, m);
            return Content(p.FamilyId.ToString());
        }
        string Trim(string s)
        {
            if (s.HasValue())
                return s.Trim();
            else
                return s;
        }
        private bool Authenticate()
        {
            var auth = Request.Headers["Authorization"];
            if (auth.HasValue())
            {
                var cred = System.Text.ASCIIEncoding.ASCII.GetString(
                    Convert.FromBase64String(auth.Substring(6))).Split(':');
                var username = cred[0];
                var password = cred[1];
                return CMSMembershipProvider.provider.ValidateUser(username, password);
            }
            return false;
        }
        private void UpdatePerson(Person p, PersonInfo m)
        {
            var z = DbUtil.Db.ZipCodes.SingleOrDefault(zc => zc.Zip == m.zip.Zip5());
            if (!m.home.HasValue() && m.cell.HasValue())
                m.home = m.cell;
            p.Family.HomePhone = m.home.GetDigits();
            p.Family.AddressLineOne = m.addr;
            p.Family.CityName = z != null ? z.City : null;
            p.Family.StateCode = z != null ? z.State : null;
            p.Family.ZipCode = m.zip;

            p.NickName = Trim(m.goesby);
            p.FirstName = Trim(m.first);
            p.LastName = Trim(m.last);
            p.DOB = m.dob;
            p.EmailAddress = Trim(m.email);
            p.CellPhone = m.cell.GetDigits();
            p.MaritalStatusId = m.marital;
            p.GenderId = m.gender;
            var rr = GetRecReg(p);
            if (m.allergies != rr.MedicalDescription)
                SetRecReg(p).MedicalDescription = m.allergies;
            if (m.AskGrade)
                if (m.grade.ToInt2() != p.Grade)
                    p.Grade = m.grade.ToInt2();
            if (m.AskEmFriend)
            {
                if (m.parent != rr.Mname)
                    SetRecReg(p).Mname = m.parent;
                if (m.emfriend != rr.Emcontact)
                    SetRecReg(p).Emcontact = m.emfriend;
                if (m.emphone != rr.Emphone)
                    SetRecReg(p).Emphone = m.emphone;
            }
            if (m.campusid > 0)
                p.CampusId = m.campusid;
            if (m.AskChurch)
                if (m.activeother.ToBool() != rr.ActiveInAnotherChurch)
                    SetRecReg(p).ActiveInAnotherChurch = m.activeother.ToBool();
            if (m.AskChurchName)
                p.OtherNewChurch = m.churchname;
            DbUtil.Db.SubmitChanges();
        }
        private RecReg GetRecReg(Person p)
        {
            var rr = p.RecRegs.SingleOrDefault();
            if (rr == null)
                return new RecReg();
            return rr;
        }
        private RecReg SetRecReg(Person p)
        {
            var rr = p.RecRegs.SingleOrDefault();
            if (rr == null)
            {
                rr = new RecReg();
                p.RecRegs.Add(rr);
            }
            return rr;
        }
        public ActionResult Campuses()
        {
            if (!Authenticate())
                return Content("not authorized");
            var q = from c in DbUtil.Db.Campus
                    where c.Organizations.Any(o => o.CanSelfCheckin == true)
                    orderby c.Id
                    select c;
            return View(q);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult RecordAttend(int PeopleId, int OrgId, bool Present, int thisday)
        {
            if (!Authenticate())
                return Content("not authorized");
            var m = new CheckInModel();
            m.RecordAttend(PeopleId, OrgId, Present, thisday);
            var r = new ContentResult();
            r.Content = "success";
            return r;
        }
        [AcceptVerbs(HttpVerbs.Post)]
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
        [AcceptVerbs(HttpVerbs.Post)]
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
        [AcceptVerbs(HttpVerbs.Post)]
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
        [AcceptVerbs(HttpVerbs.Post)]
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
        [AcceptVerbs(HttpVerbs.Post)]
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
            p.LargeId = ImageData.Image.NewImageFromBits(bits, 570, 800).Id;
            DbUtil.Db.SubmitChanges();
            return Content("done");
        }
        public ActionResult FetchImage(int id)
        {
            if (!Authenticate())
                return Content("not authorized");
            var person = DbUtil.Db.People.Single(pp => pp.PeopleId == id);
            if (person.PictureId != null)
                return new ImageResult(person.Picture.MediumId ?? 0);
            return new ImageResult(0);
        }
        public ActionResult CheckInList()
        {
            var m = from t in DbUtil.Db.CheckInTimes
                    orderby t.CheckInTimeX descending
                    select t;
            return View(m.Take(200));
        }
        [AcceptVerbs(HttpVerbs.Post)]
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
    }
}
