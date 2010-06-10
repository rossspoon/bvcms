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
    [Authorize(Roles = "Testing")]
    public class PostBundleController : Controller
    {
        public ActionResult Index(int id)
        {
            var m = new PostBundleModel(id);
            if (m.bundle == null)
                return Content("no bundle");
            if (m.bundle.BundleStatusId == (int)BundleHeader.StatusCode.Closed)
                return Content("bundle closed");
            m.fund = m.bundle.FundId.Value;
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetName(PostBundleModel m)
        {
            var s = DbUtil.Db.People.Where(i => i.PeopleId == m.pid.ToInt()).Select(i => i.Name2).SingleOrDefault();
            if (!s.HasValue())
                return Content("not found");
            return Content(s);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PostRow(PostBundleModel m)
        {
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
                CreatedDate = bd.CreatedDate,
                FundId = m.fund.ToInt(),
                PeopleId = m.pid,
                ContributionDate = m.bundle.ContributionDate,
                ContributionAmount = m.amt,
                ContributionStatusId = 0,
                PledgeFlag = m.pledge,
                ContributionTypeId = type,
            };
            m.bundle.BundleDetails.Add(bd);
            DbUtil.Db.SubmitChanges();
            return Json(new
            {
                amt = m.amt.ToString2("c"),
                totalitems = m.bundle.BundleDetails.Sum(d => d.Contribution.ContributionAmount).ToString2("c"),
                itemcount = m.bundle.BundleDetails.Count(),
                fund = "{0} - {1}".Fmt(bd.Contribution.FundId, bd.Contribution.ContributionFund.FundName),
                cid = bd.Contribution.ContributionId
            });
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateRow(PostBundleModel m)
        {
            int type;
            if (m.pledge == true)
                type = (int)Contribution.TypeCode.Pledge;
            else
                type = (int)Contribution.TypeCode.CheckCash;
            var c = DbUtil.Db.Contributions.Single(cc => cc.ContributionId == m.editid);
            c.FundId = m.fund;
            c.PeopleId = m.pid;
            c.ContributionAmount = m.amt;
            c.PledgeFlag = m.pledge;
            c.ContributionTypeId = type;
            DbUtil.Db.SubmitChanges();
            return Json(new
            {
                amt = m.amt.ToString2("c"),
                totalitems = m.bundle.BundleDetails.Sum(d => d.Contribution.ContributionAmount).ToString2("c"),
                itemcount = m.bundle.BundleDetails.Count(),
                fund = "{0} - {1}".Fmt(c.FundId, c.ContributionFund.FundDescription),
                cid = c.ContributionId
            });
        }
        public ActionResult Names(string q, int limit)
        {
            var qu = from p in DbUtil.Db.People
                     where p.Name2.StartsWith(q)
                     orderby p.Name2
                     select p.Name2 + "|" + p.PeopleId;
            return Content(string.Join("\n", qu.Take(limit).ToArray()));
        }
        public ActionResult Batch(string text)
        {
            if (Request.HttpMethod.ToUpper() == "GET")
            {
                ViewData["text"] = "";
                return View();
            }


            var lines = text.Replace("\r\n", "\n").Split('\n');
            var names = lines[0].Trim().Split(',');
            var now = DateTime.Now;
            var dt = Util.Now.Date;
            dt = Util.Now.Date.AddDays(-(int)dt.DayOfWeek);
            var bh = new BundleHeader
            {
                BundleHeaderTypeId = (int)BundleHeader.TypeCode.PreprintedEnvelope,
                BundleStatusId = (int)BundleHeader.StatusCode.Open,
                ContributionDate = dt,
                CreatedBy = Util.UserId,
                CreatedDate = now,
                FundId = 1
            };
            DbUtil.Db.BundleHeaders.InsertOnSubmit(bh);
            bh.BundleStatusId = (int)BundleHeader.StatusCode.Open;
            bh.BundleHeaderTypeId = (int)BundleHeader.TypeCode.ChecksAndCash;
            for (var i = 1; i < lines.Length; i++)
            {
                var a = lines[i].Trim().SplitCSV();
                var bd = new CmsData.BundleDetail
                {
                    CreatedBy = Util.UserId,
                    CreatedDate = now,
                };
                bh.BundleDetails.Add(bd);
                var qf = from f in DbUtil.Db.ContributionFunds
                         where f.FundStatusId == 1
                         orderby f.FundId
                         select f.FundId;

                bd.Contribution = new Contribution
                {
                    CreatedBy = Util.UserId,
                    CreatedDate = now,
                    FundId = qf.First(),
                    ContributionStatusId = 0,
                    ContributionTypeId = (int)Contribution.TypeCode.CheckCash,
                };
                string ac = null, rt = null;
                for (var c = 1; c < a.Length; c++)
                {
                    switch (names[c].Trim())
                    {
                        case "Submit Date":
                            bd.Contribution.ContributionDate = a[c].ToDate();
                            break;
                        case "Post Amount":
                            bd.Contribution.ContributionAmount = a[c].ToDecimal();
                            break;
                        case "Check Number":
                            bd.Contribution.ContributionDesc = a[c];
                            break;
                        case "R/T":
                            rt = a[c];
                            break;
                        case "Account Number":
                            ac = a[c];
                            break;
                    }
                    if (c == a.Length - 1)
                    {
                        var eac = Util.Encrypt(rt + "|" + ac);
                        var q = from kc in DbUtil.Db.CardIdentifiers
                                where kc.Id == eac
                                select kc.PeopleId;
                        var pid = q.SingleOrDefault();
                        if (pid != null)
                            bd.Contribution.PeopleId = pid;
                        bd.Contribution.BankAccount = eac;
                    }
                }
            }
            bh.TotalChecks = bh.BundleDetails.Sum(d => d.Contribution.ContributionAmount);
            bh.TotalCash = 0;
            bh.TotalEnvelopes = 0;
            DbUtil.Db.SubmitChanges();
            return Redirect("/PostBundle/Index/" + bh.BundleHeaderId);
        }
    }
}
