using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;
using UtilityExtensions;
using CMSWeb.Models;

namespace CMSWeb.Areas.Main.Controllers
{
    [Authorize(Roles="Testing")]
    public class PostBundleController : Controller
    {
        public ActionResult Index(int id)
        {
            var m = new PostBundleModel(id);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetName(PostBundleModel m)
        {
            var s = DbUtil.Db.People.Where(i => i.PeopleId == m.pid.ToInt()).Select(i => i.Name).SingleOrDefault();
            if (!s.HasValue())
                return Content("not found");
            return Content(s);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PostRow(PostBundleModel m)
        {
            var bundle = DbUtil.Db.BundleHeaders.Single(b => b.BundleHeaderId == m.id);
            var bd = new CmsData.BundleDetail
            {
                BundleHeaderId = m.id,
                CreatedBy = Util.UserId,
                CreatedDate = DateTime.Now,
            };
            int type;
            if (m.pledge == true)
                type = (int)Contribution.TypeCode.Pledge;
            else
                type = (int)Contribution.TypeCode.CheckCash;
            bd.Contribution = new Contribution
            {
                CreatedBy = Util.UserId,
                CreatedDate = DateTime.Now,
                FundId = m.fund.ToInt(),
                PeopleId = m.pid,
                ContributionDate = m.dt,
                ContributionAmount = m.amt,
                ContributionStatusId = 0,
                PledgeFlag = m.pledge ?? false,
                ContributionTypeId = type,
            };
            bundle.BundleDetails.Add(bd);
            DbUtil.Db.SubmitChanges();
            return Content("");
        }
        public ActionResult Names(string q, int limit)
        {
            var qu = from p in DbUtil.Db.People
                     where p.Name2.StartsWith(q)
                     orderby p.Name2
                     select p.Name2 + "|" + p.PeopleId;
            return Content(string.Join("\n", qu.Take(limit).ToArray()));
        }

    }
}
