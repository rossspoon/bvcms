using System;
using System.Collections;
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
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace CMSWeb.Areas.Public.Controllers
{
    public class VolunteerController : CmsController
    {
        public ActionResult Start(string id)
        {
            return RedirectToAction("Index", new { id = id });
        }
        private void SetHeader(string view)
        {
            ViewData["head"] = HeaderHtml("Volunteer-" + view,
                            DbUtil.Settings("VolHeader", "change VolHeader setting"),
                            DbUtil.Settings("VolLogo", "/Content/Crosses.png"));
        }
        public ActionResult Index(string id)
        {
            var m = new Models.VolunteerModel { View = id };
            if (!m.formcontent.HasValue())
                return Content("view not found");
            SetHeader(id);
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
            return RedirectToAction("PickList2", new { id = id, pid = m.person.PeopleId });
        }

        public ActionResult PickList2(string id, int? pid)
        {
            var person = DbUtil.Db.People.SingleOrDefault(p => p.PeopleId == pid);
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

            string email = DbUtil.Settings("VolunteerMail-" + id, "");
            if (!email.HasValue())
                email = DbUtil.Settings("VolunteerMail", DbUtil.SystemEmailAddress);

            if (Request.Form["noemail"] != "noemail")
            {
                var summary = m.PrepareSummaryText2();
                var smtp = new SmtpClient();
                Util.Email2(smtp,
                    m.person.EmailAddress,
                    email,
                    "{0} volunteer registration".Fmt(id),
                    "{0}({1}) registered for the following areas<br/>\n<blockquote>{2}</blockquote>\n"
                    .Fmt(m.person.Name, m.person.PeopleId, summary));

                var c = DbUtil.Content("Volunteer" + id);
                if (c != null)
                {
                    var body = c.Body;
                    var p = m.person;
                    body = body.Replace("{first}", p.PreferredName);
                    body = body.Replace("{serviceareas}", summary);
                    var em = m.person.EmailAddress;
                    if (!Util.ValidEmail(email))
                    {
                        em = email;
                        body = "<p>NO EMAIL</p>\n" + body;
                    }
                    Util.Email(smtp, email, m.person.Name, em,
                         id, body);
                }
            }
            return RedirectToAction("Confirm", new { id = id });
        }
        public ActionResult Confirm(string id)
        {
            ViewData["url"] = Session["continuelink"];
            SetHeader(id);
            return View();
        }
    }
}
