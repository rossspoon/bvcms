using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;
using System.Configuration;
using UtilityExtensions;
using System.Text;
using System.Net.Mail;

namespace CmsWeb.Areas.Public.Controllers
{
    public class SoulMateController : CmsController
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
                {
                    var shownew1 = m.shownew1.ToInt();
                    if (shownew1 < 3)
                    {
                        if (shownew1 == 0)
                            ModelState.AddModelError("findhim", "Cannot find his record. Is everything correct? (correct if necessary and press submit again)");
                        m.shownew1 = (shownew1 + 1).ToString();
                    }
                }

                count = m.FindMember2();
                if (count > 1)
                    ModelState.AddModelError("findher", "More than one match for her, sorry");
                else if (count == 0)
                {
                    var shownew2 = m.shownew2.ToInt();
                    if (shownew2 < 3)
                    {
                        if (shownew2 == 0)
                            ModelState.AddModelError("findher", "Cannot find her record. Is everything correct? (correct if necessary and press submit again)");
                        m.shownew2 = (shownew2 + 1).ToString();
                    }
                }
            }
            if (!ModelState.IsValid || m.shownew1 == "2" || m.shownew2 == "2")
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
                    HimId = m.person1.PeopleId,
                    HisEmail = m.email1,
                    Relationship = m.Relation,
                    ChildcareId = m.childcaremeetingid
                };
                DbUtil.Db.SoulMates.InsertOnSubmit(sm);
            }
            else
                sm.Relationship = m.Relation;
            DbUtil.Db.SubmitChanges();
            var smtp = Util.Smtp();
            SendStaffEmail(smtp, m.person1, m.email1, m.meeting);
            SendStaffEmail(smtp, m.person2, m.email2, m.meeting);
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
                return View("ChildCare", m);
            int count = 0;
            Person c = null;
            Person p = null;
            if (m.ChildParent.Value == m.person1.PeopleId)
            {
                p = m.person1;
                count = m.FindMember(p.HomePhone, m.first1, m.lastname1, m.BDay1, out c);
            }
            else
            {
                p = m.person2;
                count = m.FindMember(p.HomePhone, m.first1, m.lastname1, m.BDay1, out c);
            }
            if (count > 1)
                ModelState.AddModelError("findkid", "More than one match for child, sorry");
            if (c == null)
                c = m.AddChild(p);
            m.EnrollInChildcare(c);
            return RedirectToAction("ChildCare", new { id = id });
        }
        public ActionResult Confirm(int id)
        {
            var m = new Models.SoulMateModel(id);
            var smtp = Util.Smtp();
            SendEmail(smtp, m.person1, m.email1, m.meeting, m.Children(m.person1));
            SendEmail(smtp, m.person2, m.email2, m.meeting, m.Children(m.person2));
            return View(m);
        }

        private static void SendStaffEmail(SmtpClient smtp, Person p, string email, CmsData.Meeting meeting)
        {
            DbUtil.Email2(smtp, email,
                DbUtil.Settings("SmlMail", DbUtil.SystemEmailAddress),
                "{0} Registration".Fmt(meeting.Organization.OrganizationName),
@"{0}({1}) registered for {3} for the following date:
{2:ddd MMM d, yyyy h:mm tt}"
.Fmt(p.Name, p.PeopleId, meeting.MeetingDate, meeting.Organization.OrganizationName));
        }


        private void SendEmail(SmtpClient smtp, Person p, string email,
            CmsData.Meeting meeting, IEnumerable<Models.ChildItem> children)
        {
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
            DbUtil.Email(smtp, DbUtil.Settings("SmlMail", DbUtil.SystemEmailAddress),
                                p.Name, email, "{0} Registration".Fmt(meeting.Organization.OrganizationName),
@"Hi {0},<p>Thank you for registering. You are now enrolled for the {2} Event for the following date:</p>
<p>{1:ddd MMM d, yyyy h:mm tt} </p><p>{3}</p>".Fmt(
            p.PreferredName, meeting.MeetingDate, meeting.Organization.OrganizationName,
            sb.ToString()));
        }
    }
}
