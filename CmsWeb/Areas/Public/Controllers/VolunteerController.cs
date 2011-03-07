using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;
using System.Configuration;
using CmsWeb.Models;
using UtilityExtensions;
using System.Diagnostics;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace CmsWeb.Areas.Public.Controllers
{
    public class VolunteerController : CmsController
    {
        public ActionResult Start(string id)
        {
            return RedirectToAction("Index", new { id = id });
        }
        private void SetHeader(string view)
        {
#if DEBUG
            ViewData["timeout"] = 600000;
#else
            ViewData["timeout"] = 60000;
#endif
            //ViewData["head"] = HeaderHtml("Volunteer-" + view,
            //                DbUtil.Settings("VolHeader", "change VolHeader setting"),
            //                DbUtil.Settings("VolLogo", "/Content/Crosses.png"));
            SetHeaders(view);
        }
        public ActionResult Index(string id)
        {
            var m = new Models.VolunteerModel { View = id };
            if (!m.formcontent.HasValue())
                return Content("view not found");
            SetHeader(id);
#if DEBUG
            m.first = "David";
            m.last = "Carroll";
            m.dob = "5/30/52";
            m.phone = "4890611";
            m.email = "david@bvcms.com";
#endif
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
            return RedirectToAction("PickList2", new { id = id, pid = m.person.PeopleId, regemail = m.email });
        }

        public ActionResult PickList2(string id, int? pid, string regemail)
        {
            var Db = DbUtil.Db;
            var person = Db.People.SingleOrDefault(p => p.PeopleId == pid);
            if (person == null)
                return Content("person not found");
            var m = new Models.VolunteerModel { View = id, person = person };
            SetHeader(m.View);
            m.person.BuildVolInfoList(m.View); // gets existing
            if (Request.HttpMethod.ToUpper() == "GET")
                return View(m);

            m.person.ReplaceInterestCodes(
                Request.Form.Keys.Cast<string>().Where(k => Request.Form[k] == "on"), id);
            m.person.BuildVolInfoList(m.View); // 2nd time updates existing
            m.person.RefreshCommitments(m.View);

            string email = Db.Setting("VolunteerMail-" + id, Db.Setting("VolunteerMail", ""));
            var staff = Db.UserPersonFromEmail(email);

            if (Request.Form["noemail"] != "noemail")
            {
                var summary = m.PrepareSummaryText2();
                Db.Email(Util.PickFirst(regemail, m.person.FromEmail), staff, "{0} registration".Fmt(id), "{0}({1}) registered for the following areas<br/>\n<blockquote>{2}</blockquote>\n".Fmt(m.person.Name, m.person.PeopleId, summary));

                var c = DbUtil.Content("Volunteer" + id);
                if (c != null)
                {
                    var body = c.Body;
                    var p = m.person;
                    body = body.Replace("{first}", p.PreferredName);
                    body = body.Replace("{serviceareas}", summary);
                    Db.Email(email, m.person, regemail, c.Title, body, false);
                    OnlineRegPersonModel.CheckNotifyDiffEmails(m.person, email, regemail, c.Title, "");

                }
            }
            return RedirectToAction("Confirm", new { id = id });
        }
        public ActionResult Confirm(string id)
        {
            ViewData["url"] = Session["continuelink"];
            SetHeader(id);
            //Response.AppendHeader("Refresh", "30; URL=/Volunteer/" + id);
            var m = new Models.VolunteerModel { View = id };
            return View(m);
        }
        private void SetHeaders(string id)
        {
            var s = DbUtil.Content("Shell-" + id, DbUtil.Content("ShellDefault", ""));
            if (s.HasValue())
            {
                ViewData["hasshell"] = true;
                Regex re = new Regex(@"(.*<!--FORM START-->\s*).*(<!--FORM END-->.*)", RegexOptions.Singleline);

                var t = re.Match(s).Groups[1].Value;
                ViewData["top"] = t;
                var b = re.Match(s).Groups[2].Value;
                ViewData["bottom"] = b;
            }
        }

    }
}
