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
using System.Text;
using System.Net.Mail;

namespace CMSWeb.Controllers
{
    [HandleError]
    public class SoulMateController : Controller
    {
        public ActionResult Index()
        {
            var m = new Models.SoulMateModel();
            if (m.meeting == null)
                return View("CheckBack");
            if (Request.HttpMethod.ToUpper() == "GET")
                return View(m);

            UpdateModel(m);
            m.ValidateModel(ModelState);
            if (ModelState.IsValid)
            {
                var count = m.FindMember1();
                if (count > 1)
                    ModelState.AddModelError("findhim", "More than one match for him, sorry");
                else if (count == 0)
                    if (!m.shownew1)
                    {
                        ModelState.AddModelError("findhim", "Cannot find his record.");
                        m.shownew1 = true;
                    }

                count = m.FindMember2();
                if (count > 1)
                    ModelState.AddModelError("findher", "More than one match for her, sorry");
                else if (count == 0)
                    if(!m.shownew2)
                    {
                        ModelState.AddModelError("findher", "Cannot find her record.");
                        m.shownew2 = true;
                    }
            }
            if (!ModelState.IsValid)
                return View(m);
            m.AddPeople();
            m.EnrollInClass(m.person1);
            m.EnrollInClass(m.person2);
            var sm = DbUtil.Db.SoulMates.SingleOrDefault(s => 
                s.EventId == m.meeting.MeetingId 
                && s.HerId == m.person2.PeopleId 
                && s.HimId == m.person1.PeopleId);
            if (sm == null)
            {
                sm = new SoulMate
                {
                    EventId = m.meeting.MeetingId,
                    HerId = m.person2.PeopleId,
                    HerEmail = m.email2,
                    HerEmailPreferred = m.preferredEmail2,
                    HimId = m.person1.PeopleId,
                    HisEmail = m.email1,
                    HisEmailPreferred = m.preferredEmail1,
                    Relationship = m.Relation,
                    ChildcareId = m.childcaremeetingid
                };
                DbUtil.Db.SoulMates.InsertOnSubmit(sm);
            }
            else
                sm.Relationship = m.Relation;
            DbUtil.Db.SubmitChanges();
            var smtp = new SmtpClient();
            SendStaffEmail(smtp, m.person1, m.email1, m.preferredEmail1, m.meeting);
            SendStaffEmail(smtp, m.person2, m.email2, m.preferredEmail2, m.meeting);
            if (m.childcaremeeting != null)
                return RedirectToAction("ChildCare", new { id = sm.Id });
            return RedirectToAction("Confirm", new { id = sm.Id });
        }
        public ActionResult ChildCare(int id)
        {
            var m = new Models.SoulMateModel(id);
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddChild(int id)
        {
            var m = new Models.SoulMateModel(id);

            UpdateModel(m);
            m.ValidateChild(ModelState);
            if (!ModelState.IsValid)
                return View(m);
            int count = 0;
            Person c = null;
            Person p = null;
            if (m.ChildParent.Value == m.person1.PeopleId)
            {
                p = m.person1;
                count = m.FindMember(m.person1.HomePhone, m.lastname1, m.first1, m.DOB1, out c);
            }
            else
            {
                p = m.person2;
                count = m.FindMember(m.person2.HomePhone, m.lastname1, m.first1, m.DOB1, out c);
            }
            if (count > 1)
                ModelState.AddModelError("findkid", "More than one match for child, sorry");
            if (c == null)
                c = m.AddChild(p);
            m.EnrollInChildcare(c);
            return RedirectToAction("ChildCare", new { id = id });
        }
        public JsonResult CityState(string id)
        {
            var z = DbUtil.Db.ZipCodes.SingleOrDefault(zc => zc.Zip == id);
            if (z == null)
                return Json(null);
            return Json( new { city = z.City, state = z.State });
        }

        public ActionResult Confirm(int id)
        {
            var m = new Models.SoulMateModel(id);
            var smtp = new SmtpClient();
            SendEmail(smtp, m.person1, m.email1, m.preferredEmail1, m.meeting, m.Children(m.person1));
            SendEmail(smtp, m.person2, m.email2, m.preferredEmail2, m.meeting, m.Children(m.person2));
            return View(m);
        }

        private static void SendStaffEmail(SmtpClient smtp, Person p, string email, bool preferred, CmsData.Meeting meeting)
        {
            HomeController.Email(smtp, email,
                                "", DbUtil.Settings("SmlMail"), "{0} Registration".Fmt(meeting.Organization.OrganizationName),
@"{0}({1}) registered for {3} for the following date:</p>
<p>{2:ddd MMM d, yyyy h:mm tt}</p>".Fmt(
            p.Name, p.PeopleId, meeting.MeetingDate, meeting.Organization.OrganizationName));
        }


        private void SendEmail(SmtpClient smtp, Person p, string email, bool preferred, 
            CmsData.Meeting meeting, IEnumerable<ChildItem> children)
        {
            if (p.EmailAddress != email && preferred)
            {
                HomeController.Email(smtp, DbUtil.Settings("SmlMail"),
                                p.Name, p.EmailAddress, "Your email has been changed",
@"Hi {0},<p>You have just registered on Bellevue for {2}. We have updated your email address to be: {1}.</p>
		<p>If this was not you, please contact us ASAP.</p>".Fmt(p.PreferredName, email, meeting.Organization.OrganizationName));
                p.EmailAddress = email;
                DbUtil.Db.SubmitChanges();
            }
            var sb = new StringBuilder();
            if (children.Count() > 0)
            {
                sb.AppendLine("<table><tr><th colspan=\"3\">Children in Childcare</th></tr><tr><th>Name</th><th>Date of Birth</th><th>Age</th><th>Gender</th></tr>");
                foreach (var c in children)
                {
                    sb.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>\n",
                        c.Name, c.Birthday, c.Age, c.Gender);
                }
                sb.AppendLine("</table>");
            }
            HomeController.Email(smtp, DbUtil.Settings("SmlMail"),
                                p.Name, email, "{0} Registration".Fmt(meeting.Organization.OrganizationName),
@"Hi {0},<p>Thank you for registering. You are now enrolled for the {2} Event for the following date:</p>
<p>{1:ddd MMM d, yyyy h:mm tt} </p><p>{3}</p>".Fmt(
            p.PreferredName, meeting.MeetingDate, meeting.Organization.OrganizationName,
            sb.ToString()));
        }
    }
}
