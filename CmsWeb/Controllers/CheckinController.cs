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

namespace CMSWeb.Controllers
{
    public class CheckinController : CMSWebCommon.Controllers.CmsController
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
        //http://localhost:58724/CheckIn/CheckIn/88197
        public ActionResult CheckIn(int id, int? pid)
        {
            var m = new CheckInRecModel(id, pid);
            Session["CheckInOrgId"] = id;
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult PostCheckIn(int id, string KeyCode)
        {
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
    }
}
