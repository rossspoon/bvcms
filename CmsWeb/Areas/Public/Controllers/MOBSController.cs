using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CmsData;
using UtilityExtensions;
using System.Configuration;
using System.Net.Mail;

namespace CMSWeb.Areas.Public.Controllers
{
    public class MOBSController : CmsController
    {
        public ActionResult Index(int? test)
        {
            if (test.HasValue && test > 0)
                Session["test"] = "1";
            var m = new Models.MOBSModel();
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
                    if (!m.shownew)
                    {
                        ModelState.AddModelError("find", "Cannot find church record.");
                        m.shownew = true;
                    }
            }
            if (!ModelState.IsValid)
                return View(m);

            if (m.person == null)
                m.AddPerson();
            m.AttendEvent();

            var reg = DbUtil.Db.MOBSRegs.SingleOrDefault(r =>
                r.PeopleId == m.person.PeopleId
                && r.MeetingId == m.meeting.MeetingId
                && r.TransactionId == null);

            if (reg == null)
            {
                reg = new MOBSReg
                {
                    PeopleId = m.person.PeopleId,
                    MeetingId = m.meeting.MeetingId,
                };
                DbUtil.Db.MOBSRegs.InsertOnSubmit(reg);
            }
            reg.Created = Util.Now;
            reg.Email = m.email;
            reg.NumTickets = m.tickets;

            DbUtil.Db.SubmitChanges();

            Util.Email2(new SmtpClient(), m.email, 
                DbUtil.Settings("MOBSMail", DbUtil.SystemEmailAddress), 
                "MOBS Registration", 
                "{0}({1}) has registered for MOBS event on {2}\r\n(check cms to confirm feepaid)"
                .Fmt(m.person.Name, m.peopleid, m.MeetingTime));

            TempData["regid"] = reg.Id;
            return RedirectToAction("Payment");
        }
        public ActionResult Payment()
        {
            if (!TempData.ContainsKey("regid"))
                return View("Unknown");
            var m = new Models.MOBSModel { regid = (int)TempData["regid"] };
            return View(m);
        }
        public ActionResult Confirm(int? id, string TransactionID)
        {
            if (!id.HasValue)
                return View("Unknown");
            var m = new Models.MOBSModel { regid = id };
            if (!TransactionID.HasValue())
                return Content("error no transaction");

            m.registration.FeePaid = true;
            m.registration.TransactionId = TransactionID;
            DbUtil.Db.SubmitChanges();

            var c = DbUtil.Content("MOBSMessage");
            if (c == null)
            {
                c = new Content();
                c.Body = 
@"<p>Hi {first},</p><p>Thank you for registering for a {description} event on {date} at {time}.</p>
<p>You purchased {tickets} tickets for a total cost of {amount}</p>";
                c.Title = "Event Registration";
            }
            var p = m.person;
            c.Body = c.Body.Replace("{first}", p.PreferredName);
            c.Body = c.Body.Replace("{tickets}", m.tickets.ToString());
            c.Body = c.Body.Replace("{amount}", m.Amount.ToString("C"));
            c.Body = c.Body.Replace("{date}", m.meeting.MeetingDate.Value.ToShortDateString());
            c.Body = c.Body.Replace("{time}", m.meeting.MeetingDate.Value.ToShortTimeString());
            c.Body = c.Body.Replace("{when}", m.MeetingTime);
            c.Body = c.Body.Replace("{description}", m.meeting.Organization.OrganizationName);

            Util.Email(DbUtil.Settings("MOBSMail", DbUtil.SystemEmailAddress),
                 m.person.Name, m.registration.Email, c.Title, c.Body);
            return View(m);
        }
        [Authorize]
        public ActionResult Registrations()
        {
            var m = new Models.MOBSModel();
            return View(m.Attendees());
        }
    }
}
