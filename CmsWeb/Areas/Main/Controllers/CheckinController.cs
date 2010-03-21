using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CmsData;
using UtilityExtensions;
using CMSWeb.Models;
using System.Xml;
using System.IO;

namespace CMSWeb.Areas.Main.Controllers
{
    public class CheckinController : CmsController
    {
        public ActionResult Match(string id, int campus, int thisday, int? page)
        {
            NoCache();

            var m = new CheckInModel();
            var matches = m.Match(id, campus, thisday);

            if (page.HasValue)
            {
                if (matches.Count() == 0)
                    return new FamilyResult2(0, campus, thisday, 1); // not found
                if (matches.Count() == 1)
                    return new FamilyResult2(matches.Single().FamilyId, campus, thisday, 1);
                return new MultipleResult(matches);
            }
            else
            {
                if (matches.Count() == 0)
                    return new FamilyResult(m.FamilyMembers(0, campus, thisday)); // not found
                if (matches.Count() == 1)
                    return new FamilyResult(m.FamilyMembers(matches.Single().FamilyId, campus, thisday));
                return new MultipleResult(matches);
            }
        }
        public ActionResult Family(int id, int campus, int thisday, int? page)
        {
            NoCache();
            if (page.HasValue)
                return new FamilyResult2(id, campus, thisday, page.Value);
            else
            {
                var m = new CheckInModel();
                return new FamilyResult(m.FamilyMembers(id, campus, thisday));
            }
        }
        public ActionResult Class(int id, int thisday)
        {
            var org = DbUtil.Db.LoadOrganizationById(id);
            if (org == null)
                return new EmptyResult();
            NoCache();
            return new ClassResult(id, thisday);
        }
        public ActionResult Classes(int id, int campus, int thisday, int page)
        {
            var p = DbUtil.Db.LoadPersonById(id);
            if (p == null)
                return new EmptyResult();
            NoCache();
            return new ClassesResult(p, thisday, campus, page);
        }
        public ActionResult NameSearch(string id, int? page)
        {
            NoCache();
            if (page.HasValue)
                return new NameSearchResult2(id, page.Value);

            string first;
            string last;
            var q = DbUtil.Db.People.Select(p => p);
            Person.NameSplit(id, out first, out last);
            if (first.HasValue())
                q = from p in q
                    where (p.LastName.StartsWith(last) || p.MaidenName.StartsWith(last))
                        && (p.FirstName.StartsWith(first) || p.NickName.StartsWith(first) || p.MiddleName.StartsWith(first))
                    select p;
            else
                q = from p in q
                    where p.LastName.StartsWith(last) || p.MaidenName.StartsWith(last)
                    select p;

            var q2 = from p in q
                     orderby p.Name2
                     select new SearchInfo
                     {
                         CellPhone = p.CellPhone,
                         HomePhone = p.HomePhone,
                         Address = p.PrimaryAddress,
                         Age = p.Age,
                         Name = p.Name
                     };
            return new NameSearchResult(q2.Take(20));
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddPerson(int id,
            string addr,
            string zip,
            string first,
            string last,
            string goesby,
            string dob,
            string email,
            string cell,
            string home,
            int marital,
            int gender,
            int campusid)
        {
            CmsData.Family f;
            if (id > 0)
            {
                f = DbUtil.Db.Families.Single(fam => fam.FamilyId == id);
                var z = DbUtil.Db.ZipCodes.SingleOrDefault(zc => zc.Zip == zip);
                f.HomePhone = home.GetDigits();
                f.AddressLineOne = addr;
                f.CityName = z != null ? z.City : null;
                f.StateCode = z != null ? z.State : null;
                f.ZipCode = zip;
            }
            else
            {
                var z = DbUtil.Db.ZipCodes.SingleOrDefault(zc => zc.Zip == zip);
                f = new CmsData.Family
                {
                    HomePhone = home.GetDigits(),
                    AddressLineOne = addr,
                    CityName = z != null ? z.City : null,
                    StateCode = z != null ? z.State : null,
                    ZipCode = zip,
                };
            }

            if (goesby != null)
                goesby = goesby.Trim();
            var position = (int)CmsData.Family.PositionInFamily.Child;
            if (Util.Age0(dob) >= 18)
                if (f.People.Count(per =>
                     per.PositionInFamilyId == (int)CmsData.Family.PositionInFamily.PrimaryAdult)
                     < 2)
                    position = (int)CmsData.Family.PositionInFamily.PrimaryAdult;
                else
                    position = (int)CmsData.Family.PositionInFamily.SecondaryAdult;

            var p = Person.Add(f, position,
                null, first.Trim(), goesby, last.Trim(), dob, false, gender,
                    (int)Person.OriginCode.Visit, null);
            p.MaritalStatusId = marital;
            p.FixTitle();
            p.EmailAddress = email;
            p.CampusId = campusid;
            p.CellPhone = cell.GetDigits();
            DbUtil.Db.SubmitChanges();
            return Content(f.FamilyId.ToString());
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditPerson(int id,
            string addr,
            string zip,
            string first,
            string last,
            string goesby,
            string dob,
            string email,
            string cell,
            string home,
            int marital,
            int gender,
            int campusid)
        {
            var p = DbUtil.Db.LoadPersonById(id);
            var z = DbUtil.Db.ZipCodes.SingleOrDefault(zc => zc.Zip == zip);
            p.Family.HomePhone = home.GetDigits();
            p.Family.AddressLineOne = addr;
            p.Family.CityName = z != null ? z.City : null;
            p.Family.StateCode = z != null ? z.State : null;
            p.Family.ZipCode = zip;
            p.NickName = goesby;
            p.FirstName = first;
            p.LastName = last;
            p.DOB = dob;
            p.EmailAddress = email;
            p.CellPhone = cell.GetDigits();
            p.MaritalStatusId = marital;
            p.GenderId = gender;
            p.CampusId = campusid;
            DbUtil.Db.SubmitChanges();
            return Content(p.FamilyId.ToString());
        }
        private void NoCache()
        {
            var seconds = 10;
            Response.Cache.SetExpires(DateTime.Now.AddSeconds(seconds));
            Response.Cache.SetMaxAge(new TimeSpan(0, 0, seconds));
            Response.Cache.SetCacheability(HttpCacheability.Public);
            Response.Cache.SetValidUntilExpires(true);
            Response.Cache.SetSlidingExpiration(true);
            Response.Cache.SetETagFromFileDependencies();
        }
        public ActionResult Campuses()
        {
            var q = from c in DbUtil.Db.Campus
                    where c.Organizations.Any(o => o.CanSelfCheckin == true)
                    orderby c.Id
                    select c;
            return View(q);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult RecordAttend(int PeopleId, int OrgId, bool Present, int thisday)
        {
            var m = new CheckInModel();
            m.RecordAttend(PeopleId, OrgId, Present, thisday);
            var r = new ContentResult();
            r.Content = "success";
            return r;
        }
        [Authorize(Roles = "Staff")]
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
        public ContentResult UploadImage()
        {
            var PeopleId = Request.Headers["PeopleId"].ToInt2();
            const string STR_NotGood = "not good";
            if (!PeopleId.HasValue)
                return Content(STR_NotGood);
            var guid = new Guid(Request.Headers["Guid"].ToString());
            var tok = DbUtil.Db.TemporaryTokens.SingleOrDefault(tt => tt.Id == guid);
            if (tok == null)
                return Content(STR_NotGood);
            if (Util.Now.Subtract(tok.CreatedOn).TotalHours > 20 || tok.Expired)
                return Content(STR_NotGood);
            var person = DbUtil.Db.People.Single(pp => pp.PeopleId == PeopleId);
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
        public ActionResult CheckInList()
        {
            var m = from t in DbUtil.Db.CheckInTimes
                    orderby t.CheckInTimeX descending
                    select t;
            return View(m.Take(200));
        }
    }
}
