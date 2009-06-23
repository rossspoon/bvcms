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

namespace CMSWeb.Controllers
{
    [HandleError]
    public class SoulMateController : Controller
    {
        public SoulMateController()
        {
            ViewData["header"] = "Soulmate Registration";
            ViewData["logoimg"] = DbUtil.Settings("VolLogo");
        }
        public ActionResult Index()
        {
            var m = new Models.SoulMateModel();
            if (Request.HttpMethod.ToUpper() == "GET")
                return View(m);

            UpdateModel(m);
            m.ValidateModel(ModelState);
            if (ModelState.IsValid)
            {
                var count = m.FindMember1();
                if (count > 1)
                    ModelState.AddModelError("_FORM", "More than one match for him, sorry");
                else if (count == 0)
                    ModelState.AddModelError("_FORM", "Cannot find his church record.");

                count = m.FindMember2();
                if (count > 1)
                    ModelState.AddModelError("_FORM", "More than one match for her, sorry");
                else if (count == 0)
                    ModelState.AddModelError("_FORM", "Cannot find her church record.");
            }
            if (!ModelState.IsValid)
                return View(m);
            m.EnrollInClass(m.person1);
            m.EnrollInClass(m.person2);
            DbUtil.Db.SubmitChanges();
            SendEmail(m.person1, m.email1, m.preferredEmail1, m.meeting);
            SendEmail(m.person2, m.email2, m.preferredEmail2, m.meeting);
            return RedirectToAction("Confirm");
        }
        public ActionResult Step1Confirm()
        {
            return View();
        }
        private void SendEmail(Person p, string email, bool preferred, CmsData.Meeting meeting)
        {
            if (p.EmailAddress != email && preferred)
            {
                HomeController.Email(DbUtil.Settings("RegMail"),
                                p.Name, p.EmailAddress, "Your email has been changed",
@"Hi {0},<p>You have just registered on Bellevue for {2}. We have updated your email address to be: {1}.</p>
<p>If this was not you, please contact us ASAP.</p>".Fmt(p.PreferredName, email, meeting.Organization.OrganizationName));
                p.EmailAddress = email;
                DbUtil.Db.SubmitChanges();
            }
            HomeController.Email(DbUtil.Settings("RegMail"),
                                p.Name, email, "Soulmate Live Registration",
@"Hi {0},<p>Thank you for registering. You are now enrolled for the {2} Soulmate Live Event for the following date:</p>
<p>{1:ddd MMM d, yyyy} 4:30 to 6:00 PM</p>".Fmt(p.PreferredName, meeting.MeetingDate, meeting.Organization.OrganizationName));
        }
    }
}
