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
    public class StepClassController : Controller
    {
        public StepClassController()
        {
            ViewData["header"] = DbUtil.Settings("VolHeader");
            ViewData["logoimg"] = DbUtil.Settings("VolLogo");
        }
        public ActionResult Step1()
        {
            var m = new Models.StepClassModel();
            if (Request.HttpMethod.ToUpper() == "GET")
                return View(m);

            UpdateModel(m);
            m.ValidateModel(ModelState);
            if (ModelState.IsValid)
            {
                var count = m.FindMember();
                if (count > 1)
                    ModelState.AddModelError("_FORM", "More than one match, sorry");
                else if (count == 0)
                    ModelState.AddModelError("_FORM", "Cannot find your church record.");
                else
                {
                    if (m.person.Age < 18)
                        ModelState.AddModelError("_FORM", "You must be a at least 18");
                }
            }
            if (!ModelState.IsValid)
                return View(m);
            m.EnrollInClass("Step 1");
            DbUtil.Db.SubmitChanges();
            SendEmail(m);
            return RedirectToAction("Step1Confirm");
        }
        public ActionResult Step1Confirm()
        {
            return View();
        }
        private void SendEmail(StepClassModel m)
        {
            if (m.person.EmailAddress != m.email && m.preferredEmail)
            {
                HomeController.Email(DbUtil.Settings("RegMail"),
                                m.person.Name, m.person.EmailAddress, "Your email has been changed",
@"Hi {0},<p>You have just registered on Bellevue for Step 1. We have updated your email address to be: {1}.</p>
<p>If this was not you, please contact us ASAP.</p>".Fmt(m.person.PreferredName, m.email));
                m.person.EmailAddress = m.email;
                DbUtil.Db.SubmitChanges();
            }
            HomeController.Email(DbUtil.Settings("RegMail"),
                                m.person.Name, m.email, "Church Registration",
@"Hi {0},<p>Thank you for registering. You are now enrolled in the Step 1 Class for the following date:</p>
<p>{1:ddd mmm d, yyyy 4:30 to 6:00 tt}</p>".Fmt(m.person.PreferredName, m.meeting.MeetingDate));
        }
    }
}
