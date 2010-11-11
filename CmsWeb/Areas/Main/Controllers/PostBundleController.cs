using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;
using UtilityExtensions;
using CmsWeb.Models;
using System.Web.Script.Serialization;
using System.IO;

namespace CmsWeb.Areas.Main.Controllers
{
    [Authorize(Roles = "Finance")]
    [ValidateInput(false)]
    public class PostBundleController : Controller
    {
        public ActionResult Index(int id)
        {
            var m = new PostBundleModel(id);
            if (m.bundle == null)
                return Content("no bundle");
            if (m.bundle.BundleStatusId == (int)BundleHeader.StatusCode.Closed)
                return Content("bundle closed");
            m.fund = m.bundle.FundId ?? 1;
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetName(PostBundleModel m)
        {
            var s = m.GetNameFromPid();
            return Content(s);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PostRow(PostBundleModel m)
        {
            return Json(m.PostContribution());
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateRow(PostBundleModel m)
        {
            return Json(m.UpdateContribution());
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteRow(PostBundleModel m)
        {
            return Json(m.DeleteContribution());
        }
        public ActionResult Names(string q, int limit)
        {
            return Content(PostBundleModel.Names(q, limit));
        }
        public ActionResult FundTotals(int id)
        {
            var m = new PostBundleModel(id);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Batch()
        {
            var dt = Util.Now.Date;
            dt = Util.Now.Date.AddDays(-(int)dt.DayOfWeek);
            ViewData["date"] = dt;
            return View();
        }
        public ActionResult BatchUpload(DateTime date, HttpPostedFileBase file, string text)
        {
            string s;
            if (file != null)
            {
                byte[] buffer = new byte[file.ContentLength];
                file.InputStream.Read(buffer, 0, file.ContentLength);
                System.Text.Encoding enc = null;
                if (buffer[0] == 0xFF && buffer[1] == 0xFE)
                {
                    enc = new System.Text.UnicodeEncoding();
                    s = enc.GetString(buffer, 2, buffer.Length - 2);
                }
                else
                {
                    enc = new System.Text.ASCIIEncoding();
                    s = enc.GetString(buffer);
                }
            }
            else
                s = text;
            var id = PostBundleModel.BatchProcess(s, date);
            if (id.HasValue)
                return Redirect("/PostBundle/Index/" + id);
            return RedirectToAction("Batch");
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Funds()
        {
            var m = new PostBundleModel();
            return Json(m.Funds2());
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(string id, string value)
        {
            var iid = id.Substring(1).ToInt();
            var c = DbUtil.Db.Contributions.SingleOrDefault(co => co.ContributionId == iid);
            if (c != null)
                switch (id.Substring(0, 1))
                {
                    case "a":
                        c.ContributionAmount = value.ToDecimal();
                        DbUtil.Db.SubmitChanges();
                        return Content(c.ContributionAmount.ToString2("N2"));
                    case "f":
                        c.FundId = value.ToInt();
                        DbUtil.Db.SubmitChanges();
                        return Content("{0} - {1}".Fmt(c.ContributionFund.FundId, c.ContributionFund.FundName));
                }
            return new EmptyResult();
        }

    }
}
