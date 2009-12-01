using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;
using System.Configuration;
using CMSWeb.Models;
using UtilityExtensions;
using System.Diagnostics;

namespace CMSWeb.Controllers
{
    public class VolunteerController : CMSWebCommon.Controllers.CmsController
    {
        public VolunteerController()
        {
            ViewData["header"] = DbUtil.Settings("VolHeader", "change VolHeader setting");
            ViewData["logoimg"] = DbUtil.Settings("VolLogo", "/Content/Crosses.png");
        }
        public ActionResult Start(string id)
        {
            var vol = DbUtil.Db.VolOpportunities.SingleOrDefault(v => v.UrlKey == id);
            if (vol == null)
                return View("Unknown");
            return RedirectToAction("Index", new { id = vol.Id });
        }
        public ActionResult Index(int id)
        {
            var m = new Models.VolunteerModel { OpportunityId = id };
            if (Request.HttpMethod.ToUpper() == "GET")
                return View(m);

            UpdateModel(m);
            m.ValidateModel(ModelState);
            if (ModelState.IsValid)
            {
                var count = m.FindMember();
                if (count > 1)
                    ModelState.AddModelError("find", "More than one match, sorry");
                else if (count == 0)
                    ModelState.AddModelError("find", "Cannot find your church record");
            }
            if (!ModelState.IsValid)
                return View(m);
            var v = DbUtil.Db.VolInterests.SingleOrDefault(vi =>
                vi.PeopleId == m.person.PeopleId && vi.OpportunityCode == m.OpportunityId);
            if (v == null)
            {
                v = new VolInterest
                {
                    Created = DateTime.Now,
                    PeopleId = m.person.PeopleId,
                };
                m.person.EmailAddress = m.email;
                m.Opportunity.VolInterests.Add(v);
                DbUtil.Db.VolInterests.InsertOnSubmit(v);
                DbUtil.Db.SubmitChanges();
            }
            if (m.Opportunity.FormContent.HasValue())
                return RedirectToAction("PickList2", new { id = v.Id });
            return RedirectToAction("PickList", new { id = v.Id });
        }

        public ActionResult PickList(int id)
        {
            var m = new Models.VolunteerModel { VolInterestId = id };
            if (Request.HttpMethod.ToUpper() == "GET")
                return View(m);

            UpdateModel(m);
            m.ValidateModel2(ModelState);
            if (!ModelState.IsValid)
                return View(m);

            m.VolInterest.Question = m.question;

            var qd = from vi in m.VolInterest.VolInterestInterestCodes
                     join i in m.interests on vi.InterestCodeId equals i.ToInt() into j
                     from i in j.DefaultIfEmpty()
                     where string.IsNullOrEmpty(i)
                     select vi;
            DbUtil.Db.VolInterestInterestCodes.DeleteAllOnSubmit(qd);

            var qa = from i in m.interests
                     join vi in m.VolInterest.VolInterestInterestCodes
                        on i.ToInt() equals vi.InterestCodeId into j
                     from vi in j.DefaultIfEmpty()
                     where vi == null
                     select i.ToInt();

            foreach (var i in qa)
                m.VolInterest.VolInterestInterestCodes.Add(new VolInterestInterestCode { InterestCodeId = i });

            var cva = m.person.Volunteers.OrderByDescending(vo => vo.ProcessedDate).FirstOrDefault();
            DbUtil.Db.SubmitChanges();
            string body;
            if ((cva != null && cva.StatusId == 10) || !m.Opportunity.EmailNoCva.HasValue())
                body = m.Opportunity.EmailYesCva; // Yes, have CVA already
            else
                body = m.Opportunity.EmailNoCva;
            var p = m.person;
            body = body.Replace("{first}", p.PreferredName);
            Util.SafeFormat(body);
            body += "<p>You have indicated following interests:\n{0}</p>".Fmt(
                m.PrepareSummaryText());

            Util.Email(m.Opportunity.Email, m.person.Name, m.person.EmailAddress,
                 m.Opportunity.Description, body);
            return RedirectToAction("Confirm");
        }
        public ActionResult PickList2(int id)
        {
            var m = new Models.VolunteerModel { VolInterestId = id };
            if (Request.HttpMethod.ToUpper() == "GET")
                return View(m);

            foreach (var i in Request.Form.Keys)
                Debug.WriteLine("{0}: {1}".Fmt(i, Request.Form[i.ToString()].ToString()));

            DbUtil.Db.VolInterestInterestCodes.DeleteAllOnSubmit(m.VolInterest.VolInterestInterestCodes);
            DbUtil.Db.SubmitChanges();

            var dict = m.Opportunity.VolInterestCodes.ToDictionary(vi => vi.Description);
            foreach (var i in Request.Form.Keys.Cast<string>())
            {
                var val = Request.Form[i];
                if (val == "on")
                {
                    var desc = i.Replace('_', ' ');
                    if (!dict.ContainsKey(desc))
                    {
                        var vic = new VolInterestCode { Description = desc, OpportunityId = m.OpportunityId };
                        DbUtil.Db.VolInterestCodes.InsertOnSubmit(vic);
                        dict[desc] = vic;
                    }
                }
            }
            DbUtil.Db.SubmitChanges();
            DbUtil.Db.Refresh(RefreshMode.OverwriteCurrentValues);

            foreach (var i in Request.Form.Keys.Cast<string>())
            {
                var val = Request.Form[i];
                if (val == "on")
                {
                    var desc = i.Replace('_', ' ');
                    m.VolInterest.VolInterestInterestCodes.Add(new VolInterestInterestCode { InterestCodeId = dict[desc].Id });
                }
            }
            DbUtil.Db.SubmitChanges();
            return RedirectToAction("Confirm");
        }
        public ActionResult Confirm()
        {
            return View();
        }
    }
}
