using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CmsData;
using CMSWeb.Models;
using UtilityExtensions;
using System.Configuration;

namespace CMSWeb.Controllers
{
    public class RecRegController : Controller
    {
        public ActionResult Index(int id)
        {
            var m = new RecRegModel { divid = id };
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
            if (m.participant == null)
                m.AddPerson();
            if (!m.EnrollInOrg(m.participant))
            {
                ModelState.AddModelError("find", "Sorry, cannot find an appropriate age division");
                return View(m);
            }

            var reg = DbUtil.Db.RecRegs.SingleOrDefault(r =>
                r.PeopleId == m.participant.PeopleId
                && (r.Expired ?? false) == false
                && r.OrgId == m.OrgId);
            if (reg == null)
            {
                reg = new RecReg
                {
                    PeopleId = m.participant.PeopleId,
                    UserInfo = "online",
                    OrgId = m.OrgId,
                    DivId = m.divid,
                    Uploaded = DateTime.Now,
                    Email = m.email,
                    FeePaid = false,
                };
                DbUtil.Db.RecRegs.InsertOnSubmit(reg);
                DbUtil.Db.SubmitChanges();
            }

            if(!m.last.HasValue())
                return RedirectToAction("Confirm", new { id = reg.Id });
            return RedirectToAction("OtherInfo", new { id = reg.Id });
        }
        public ActionResult OtherInfo(int id)
        {
            var m = new RecRegModel { regid = id };
            if (Request.HttpMethod.ToUpper() == "GET")
                return View(m);

            UpdateModel(m);
            m.ValidateModel2(ModelState);
            if (!ModelState.IsValid)
                return View(m);

            m.registration.Request = m.request;
            m.registration.ShirtSize = m.shirtsize;
            m.registration.ActiveInAnotherChurch = m.otherchurch;
            m.registration.MedAllergy = m.medical.HasValue();
            m.registration.Member = m.member;
            m.registration.Mname = m.mname;
            m.registration.Fname = m.fname;
            m.registration.Emcontact = m.emcontact;
            m.registration.Emphone = m.emphone;
            m.registration.Docphone = m.docphone;
            m.registration.Doctor = m.doctor;
            m.registration.Coaching = m.coaching > 0;
            m.registration.Insurance = m.insurance;
            m.registration.Policy = m.policy;
            m.registration.MedicalDescription = m.medical;

            var t = m.PrepareSummaryText();
            var bits = System.Text.ASCIIEncoding.ASCII.GetBytes(t);
            var i = ImageData.Image.NewTextFromBits(bits);
            m.registration.ImgId = i.Id;
            m.registration.IsDocument = true;
            DbUtil.Db.SubmitChanges();

            if (m.registration.FeePaid ?? false)
                return RedirectToAction("Confirm", new { id = m.regid });
            else
                return RedirectToAction("Payment", new { id = m.regid });
        }
        public ActionResult Payment(int id)
        {
            var m = new RecRegModel { regid = id };

            return View(m);
        }
        public ActionResult Confirm(int id, int? TransactionID)
        {
            var m = new RecRegModel { regid = id };
            if (TransactionID.HasValue)
            {
                m.registration.FeePaid = true;
                m.registration.TransactionId = TransactionID.ToString();
                DbUtil.Db.SubmitChanges();
            }

            HomeController.Email(DbUtil.Settings("RecMail"),
    "", m.registration.Email, "Recreation Registration",
@"<p>Thank you for registering for {0}: {1}
You will receive another email with team information once they have been established.</p>
<p>You will need to download the <a href=""https://cms.bellevue.org/Upload/MedicalRelease.pdf"">Medical Relase Form</a>, 
print, sign, and return it to the Recreation Ministry in order to complete your registration.</p>
<p>We have the following information:
{2}
".Fmt(m.division.Name, m.organization.OrganizationName, 
    ImageData.Image.Content(m.registration.ImgId.Value)));

            HomeController.Email(m.registration.Email,
                    "", DbUtil.Settings("RecMail"), "{0} Registration".Fmt(m.division.Name),
@"{0}({1}) has registered for {2}: {3}</p>".Fmt(
m.participant.Name, m.participant.PeopleId, m.division.Name, m.organization.OrganizationName));

            return View(m);
        }
    }
}
