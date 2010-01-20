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
        public ActionResult Match(string id, int campus, int thisday)
        {
            NoCache();

            var m = new CheckInModel();
            var matches = m.Match(id, campus, thisday);

            if (matches.Count() == 0)
                return new FamilyResult(m.FamilyMembers(0, campus, thisday)); // not found
            if (matches.Count() == 1)
                return new FamilyResult(m.FamilyMembers(matches.Single().FamilyId, campus, thisday));
            return new MultipleResult(matches);
        }
        public FamilyResult Family(int id, int campus, int thisday)
        {
            NoCache();
            var m = new CheckInModel();
            return new FamilyResult(m.FamilyMembers(id, campus, thisday));

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
        [Authorize(Roles="Staff")]
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
                return Content (STR_NotGood);
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
