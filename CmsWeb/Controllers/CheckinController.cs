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
                return View("Family", m.FamilyMembers(0, campus, thisday)); // not found
            if (matches.Count() == 1)
                return View("Family", m.FamilyMembers(matches.Single().FamilyId, campus, thisday));

            return View("Multiple", matches);
        }
        public ActionResult Family(int id, int campus, int thisday)
        {
            NoCache();
            var m = new CheckInModel();
            return View(m.FamilyMembers(id, campus, thisday));
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
    }
}
