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
using System.Text;
using System.Net.Mail;

namespace CMSRegCustom.Controllers
{
    [HandleError]
    public class LoveRespectController : Controller
    {
        public ActionResult Index()
        {
            var m = new Models.LoveRespectModel();
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
            if (m.person1 == null || m.person2 == null)
                m.AddPeople();
            m.EnrollInClass(m.person1);
            m.EnrollInClass(m.person2);
            var dt = DateTime.Now;
            var q = from s in DbUtil.Db.LoveRespects
                    where s.HerId == m.person2.PeopleId
                    where s.HimId == m.person1.PeopleId
                    where dt <= s.Organization.LastMeetingDate.Value.AddDays(1)
                    select s;
            var lr = q.SingleOrDefault();

            if (lr == null)
            {
                lr = new LoveRespect
                {
                    HerId = m.person2.PeopleId,
                    HerEmail = m.email2,
                    HerEmailPreferred = m.preferredEmail2,
                    HimId = m.person1.PeopleId,
                    HisEmail = m.email1,
                    HisEmailPreferred = m.preferredEmail1,
                    Relationship = m.Relation,
                    OrgId = m.OrgId,
                    Created = DateTime.Now,
                    PreferNight = m.night
                };
                DbUtil.Db.LoveRespects.InsertOnSubmit(lr);
            }
            else
                lr.Relationship = m.Relation;
            DbUtil.Db.SubmitChanges();
            var smtp = new SmtpClient();
            SendStaffEmail(smtp, m.person1, m.email1, m.preferredEmail1, m.organization, 
                LoveRespectModel.NightWord(m.night));
            SendStaffEmail(smtp, m.person2, m.email2, m.preferredEmail2, m.organization, 
                LoveRespectModel.NightWord(m.night));
            SendEmail(smtp, m.person1, m.email1, m.preferredEmail1, 
                m.phone1, m.homecell1, m.MaritalStatus, m.organization, m.night.Value);
            SendEmail(smtp, m.person2, m.email2, m.preferredEmail2, 
                m.phone2, m.homecell2, m.MaritalStatus, m.organization, m.night.Value);
            return RedirectToAction("Confirm");
        }
        public ActionResult Other()
        {
            var m = new Models.LoveRespectModel();
            if (Request.HttpMethod.ToUpper() == "GET")
                return View(m);

            UpdateModel(m);
            m.ValidateModel(ModelState);
            if (!ModelState.IsValid)
                return View(m);
            var smtp = new SmtpClient();
            SendStaffEmailOth(smtp, m);
            SendEmailOth(smtp, m);
            return RedirectToAction("Confirm");
        }
        public JsonResult CityState(string id)
        {
            var z = DbUtil.Db.ZipCodes.SingleOrDefault(zc => zc.Zip == id);
            if (z == null)
                return Json(null);
            return Json( new { city = z.City, state = z.State });
        }

        public ActionResult Confirm()
        {
            return View();
        }

        private static void SendStaffEmail(SmtpClient smtp, Person p, string email, bool preferred, CmsData.Organization org, string night)
        {
            Util.Email2(smtp, email,
                                DbUtil.Settings("LRMail"), "{0} Registration".Fmt(org.OrganizationName),
@"{0}({1}) registered for {2} (night: {3})</p>".Fmt(
            p.Name, p.PeopleId, org.OrganizationName, night));
        }

        private void SendEmail(SmtpClient smtp, Person p, string email, bool preferred, 
            string phone, string homecell, int married, CmsData.Organization org, int night)
        {
            var sb = new StringBuilder();
            var oldaddress = "";
            if (p.EmailAddress != email && preferred)
            {
                sb.AppendFormat("We have updated your email address to be: {0}.<br />\n", email);
                oldaddress = p.EmailAddress;
                p.EmailAddress = email;
            }
            else if (!p.EmailAddress.HasValue())
                p.EmailAddress = email;

            if (homecell == "h" && p.Family.HomePhone.HasValue() && !p.Family.HomePhone.EndsWith(phone.GetDigits()))
            {
                sb.AppendFormat("We have updated your home phone to be: {0}.<br />\n", phone.FmtFone());
                p.Family.HomePhone = phone.GetDigits();
            }
            else if (!p.Family.HomePhone.HasValue())
                p.Family.HomePhone = phone.GetDigits();
            if (homecell == "c" && p.CellPhone.HasValue() && !p.CellPhone.EndsWith(phone.GetDigits()))
            {
                sb.AppendFormat("We have updated your cell phone to be: {0}.<br />\n", phone.FmtFone());
                p.CellPhone = phone.GetDigits();
            }
            else if (!p.CellPhone.HasValue())
                p.CellPhone = phone.GetDigits();
            if (p.MaritalStatusId <= 20 && p.MaritalStatusId != married)
            {
                sb.AppendFormat("We have updated your marital status.<br />\n");
                p.MaritalStatusId = married;
            }
            if (sb.Length > 0)
            {
                ChangeMail(smtp, p.EmailAddress, org, p, sb.ToString());
                ChangeMail(smtp, DbUtil.Settings("LRMail"), org, p, sb.ToString());
                if (oldaddress.HasValue())
                    ChangeMail(smtp, oldaddress, org, p, sb.ToString());
            }
            DbUtil.Db.SubmitChanges();

            if (night == 3)
                Util.Email(smtp, DbUtil.Settings("LRMail"),
                                    p.Name, email, "{0} Registration".Fmt(org.OrganizationName),
    @"Hi {0},<p>Thank you for registering. You are now enrolled for {3} starting the following date:</p>
<p>{1:ddd MMM d}, {2:yyyy h:mm tt} </p>".Fmt(
                    p.PreferredName, org.FirstMeetingDate, org.WeeklySchedule.MeetingTime, org.OrganizationName));
            else
                Util.Email(smtp, DbUtil.Settings("LRMail"),
                                p.Name, email, "{0} Registration".Fmt(org.OrganizationName),
@"Hi {0},<p>Thank you for registering for a Love and Respect small group. 
Someone will be in contact with you as soon as we form groups.</p>".Fmt(
                p.PreferredName));
        }


        private void ChangeMail(SmtpClient smtp, string email, CmsData.Organization org, Person p, string changes)
        {
            Util.Email(smtp, DbUtil.Settings("LRMail"),
                            p.Name, email, "Changes on church record",
@"{0} just registered on Bellevue for {1}.</p>
<p>{2}</p>
		<p>If this was not you, please contact us ASAP.</p>".Fmt(
                p.PreferredName, org.OrganizationName, changes));
            DbUtil.Db.SubmitChanges();
        }

        private static void SendStaffEmailOth(SmtpClient smtp, LoveRespectModel m)
        {
            Util.Email(smtp, DbUtil.Settings("LRMail"),
                                "", DbUtil.Settings("LRMail"), "Love Respect Other Church Registration",
@"We received the following registration:
{0}
".Fmt(m.PrepareSummaryText()));
        }

        private void SendEmailOth(SmtpClient smtp, LoveRespectModel m)
        {
            Util.Email(smtp, DbUtil.Settings("LRMail"),
                                "", m.email1, "Love & Respect Small Group Registration",
@"Hi {0},<p>Thank you for registering. Someone will be in contact with you.</p>".Fmt(
            m.first1));
            Util.Email(smtp, DbUtil.Settings("LRMail"),
                                "", m.email2, "Love & Respect Small Group Registration",
@"Hi {0},<p>Thank you for registering. Someone will be in contact with you.</p>".Fmt(
            m.first2));
        }
    }
}
