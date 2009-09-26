using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;
using System.Configuration;
using CMSRegCustom.Models;
using UtilityExtensions;

namespace CMSRegCustom.Controllers
{
    [HandleError]
    public class StepClassController : Controller
    {
        public StepClassController()
        {
            ViewData["header"] = "Step Class Registration";
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
            m.EnrollInClass();
            DbUtil.Db.SubmitChanges();
            SendEmail(m, "Step 1");
            return RedirectToAction("Step1Confirm");
        }
        public ActionResult Step1Confirm()
        {
            return View();
        }
        public ActionResult Step2()
        {
            var m = new Models.StepClassModel();
            if (Request.HttpMethod.ToUpper() == "GET")
                return View(m);

            UpdateModel(m);
            m.ValidateModel(ModelState);
            ValidateStep23(m);
            if (!ModelState.IsValid)
                return View(m);
            m.EnrollInClass();
            DbUtil.Db.SubmitChanges();
            SendEmail(m, "Step 2");
            return RedirectToAction("Step2Confirm");
        }
        public ActionResult Step2Confirm()
        {
            return View();
        }
        public ActionResult Step3()
        {
            var m = new Models.StepClassModel();
            if (Request.HttpMethod.ToUpper() == "GET")
                return View(m);

            UpdateModel(m);
            m.ValidateModel(ModelState);
            ValidateStep23(m);
            if (!ModelState.IsValid)
                return View(m);
            m.EnrollInClass();
            DbUtil.Db.SubmitChanges();
            SendEmail(m, "Step 3");
            return RedirectToAction("Step3Confirm");
        }
        public ActionResult Step3Confirm()
        {
            return View();
        }
        private void ValidateStep23(StepClassModel m)
        {
            if (ModelState.IsValid)
            {
                var count = m.FindMember();
                if (count > 1)
                    ModelState.AddModelError("_FORM", "More than one match, sorry");
                else if (count == 0)
                    ModelState.AddModelError("_FORM", "Cannot find your church record.");
                else
                {
                    switch (m.person.DiscoveryClassStatusId)
                    {
                        case (int)Person.DiscoveryClassStatusCode.Attended:
                            break;
                        case (int)Person.DiscoveryClassStatusCode.GrandFathered:
                            break;
                        case (int)Person.DiscoveryClassStatusCode.AdminApproval:
                            break;
                        case (int)Person.DiscoveryClassStatusCode.ExemptedChild:
                            break;
                        default:
                            ModelState.AddModelError("_FORM", "I'm sorry, but our records indicate that you have not attended Step 1 which is a prerequite for this class");
                            break;
                    }
                    if (m.person.Age < 18)
                        ModelState.AddModelError("_FORM", "You must be 18 years old");
                }
            }
        }
        private void SendEmail(StepClassModel m, string name)
        {
            if (m.person.EmailAddress != m.email && m.preferredEmail)
            {
                Util.Email(DbUtil.Settings("StepMail"),
                                m.person.Name, m.person.EmailAddress, "Your email has been changed",
@"Hi {0},<p>You have just registered on Bellevue for {2}. We have updated your email address to be: {1}.</p>
<p>If this was not you, please contact us ASAP.</p>".Fmt(
            m.person.PreferredName.TrimEnd(), m.email, name));
                m.person.EmailAddress = m.email;
                DbUtil.Db.SubmitChanges();
            }
            Util.Email(DbUtil.Settings("StepMail"),
                                m.person.Name, m.email, "Step Class Registration",
@"Hi {0},<p>Thank you for registering. You are now enrolled in the {2} Class for the following date:</p>
<p>{1:ddd MMM d, yyyy h:mm tt} to {3:h:mm tt}</p>".Fmt(
            m.person.PreferredName.TrimEnd(), m.meeting.MeetingDate, name, m.meeting.MeetingDate.Value.AddMinutes(90)));
            Util.Email2(m.email,
                                DbUtil.Settings("StepMail"), "Step Class Registration",
@"{0}({1}) registered in the {3} Class for the following date:</p>
<p>{2:ddd MMM d, yyyy h:mm tt} to {4:h:mm tt}</p>".Fmt(
            m.person.Name, m.person.PeopleId, m.meeting.MeetingDate, name, m.meeting.MeetingDate.Value.AddMinutes(90)));
        }
    }
}
