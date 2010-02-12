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

namespace CMSWeb.Areas.Public.Controllers
{
    public class LoveRespectController : CmsController
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
                    if (!m.shownew2)
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
            var dt = Util.Now;
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
                    HimId = m.person1.PeopleId,
                    HisEmail = m.email1,
                    Relationship = m.Relation,
                    OrgId = m.OrgId,
                    Created = Util.Now,
                    PreferNight = m.night
                };
                DbUtil.Db.LoveRespects.InsertOnSubmit(lr);
            }
            else
                lr.Relationship = m.Relation;
            DbUtil.Db.SubmitChanges();
            var smtp = new SmtpClient();
            SendStaffEmail(smtp, 
                m.person1, 
                m.email1, 
                m.organization, 
                Models.LoveRespectModel.NightWord(m.night));
            SendStaffEmail(smtp, 
                m.person2, 
                m.email2, 
                m.organization, 
                Models.LoveRespectModel.NightWord(m.night));
            SendEmail(smtp, 
                m.person1, 
                m.email1, 
                m.phone1, 
                m.homecell1, 
                m.MaritalStatus, 
                m.organization, 
                m.night.Value);
            SendEmail(smtp, 
                m.person2, 
                m.email2, 
                m.phone2, 
                m.homecell2, 
                m.MaritalStatus, 
                m.organization, 
                m.night.Value);
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
        
        public ActionResult Confirm()
        {
            return View();
        }

        private static void SendStaffEmail(SmtpClient smtp, Person p, string email, 
            CmsData.Organization org, string night)
        {
            Util.Email2(smtp, email, DbUtil.Settings("LRMail", DbUtil.SystemEmailAddress), "{0} Registration".Fmt(org.OrganizationName), @"{0}({1}) registered for {2} (night: {3})</p>".Fmt(p.Name, p.PeopleId, org.OrganizationName, night));
        }

        private void SendEmail(SmtpClient smtp, Person p, string email, 
            string phone, string homecell, int married, CmsData.Organization org, int night)
        {
            var sb = new StringBuilder();

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
                p.FixTitle();
            }
            DbUtil.Db.SubmitChanges();

            if (night == 3)
                Util.Email(smtp, DbUtil.Settings("LRMail", DbUtil.SystemEmailAddress),
                                    p.Name, email, "{0} Registration".Fmt(org.OrganizationName),
    @"Hi {0},<p>Thank you for registering. You are now enrolled for {3} starting the following date:</p>
<p>{1:ddd MMM d, yyyy} {2:h:mm tt} </p>".Fmt(
                    p.PreferredName, org.FirstMeetingDate, org.SchedTime, org.OrganizationName));
            else
                Util.Email(smtp, DbUtil.Settings("LRMail", DbUtil.SystemEmailAddress),
                                p.Name, email, "{0} Registration".Fmt(org.OrganizationName),
@"Hi {0},<p>Thank you for registering for a Love and Respect small group. 
Someone will be in contact with you as soon as we form groups.</p>".Fmt(
                p.PreferredName));
        }

        private static void SendStaffEmailOth(SmtpClient smtp, Models.LoveRespectModel m)
        {
            Util.Email(smtp, DbUtil.Settings("LRMail", DbUtil.SystemEmailAddress),
                                "", DbUtil.Settings("LRMail", DbUtil.SystemEmailAddress), "Love Respect Other Church Registration",
@"We received the following registration:
{0}
".Fmt(m.PrepareSummaryText()));
        }

        private void SendEmailOth(SmtpClient smtp, Models.LoveRespectModel m)
        {
            Util.Email(smtp, DbUtil.Settings("LRMail", DbUtil.SystemEmailAddress),
                                "", m.email1, "Love & Respect Small Group Registration",
@"Hi {0},<p>Thank you for registering. Someone will be in contact with you.</p>".Fmt(
            m.first1));
            Util.Email(smtp, DbUtil.Settings("LRMail", DbUtil.SystemEmailAddress),
                                "", m.email2, "Love & Respect Small Group Registration",
@"Hi {0},<p>Thank you for registering. Someone will be in contact with you.</p>".Fmt(
            m.first2));
        }
    }
}
