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
using System.Net.Mail;

namespace CMSWeb.Areas.Public.Controllers
{
    public class RecRegController : CmsController
    {
        public ActionResult Index(int? id)
        {
            if (!id.HasValue)
                return View("NoLeague");
            var m = new Models.RecRegModel { divid = id };
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

            TempData["model"] = m;
            return RedirectToAction("OtherInfo");

        }
        public ActionResult OtherInfo()
        {
            if (Request.HttpMethod.ToUpper() == "GET")
            {
                if (!TempData.ContainsKey("model"))
                    return View("Unknown");
                var tm = TempData["model"] as Models.RecRegModel;
                return View(tm);
            }

            var m = new Models.RecRegModel();
            UpdateModel(m);

            m.ValidateModel2(ModelState);
            if (!ModelState.IsValid)
                return View(m);

            // At this point, we have good data from the first two pages in m

            if (m.participant == null)
                m.AddPerson();
            m.EnrollInOrg(m.participant);

            var reg = DbUtil.Db.RecRegs.SingleOrDefault(r =>
                r.PeopleId == m.participant.PeopleId
                && (r.Expired ?? false) == false
                && r.OrgId == m.orgid);

            if (reg == null)
            {
                reg = new RecReg
                {
                    PeopleId = m.participant.PeopleId,
                    UserInfo = "online",
                    OrgId = m.orgid,
                    DivId = m.divid,
                    Uploaded = Util.Now,
                    Email = m.email,
                    FeePaid = false,
                };
                DbUtil.Db.RecRegs.InsertOnSubmit(reg);
            }

            reg.Request = m.request;
            reg.ShirtSize = m.shirtsize;
            reg.ActiveInAnotherChurch = m.otherchurch;
            reg.MedAllergy = m.medical.HasValue();
            reg.Member = m.member;
            reg.Mname = m.mname;
            reg.Fname = m.fname;
            reg.Emcontact = m.emcontact;
            reg.Emphone = m.emphone;
            reg.Docphone = m.docphone;
            reg.Doctor = m.doctor;
            reg.Coaching = m.coaching > 0;
            reg.Insurance = m.insurance;
            reg.Policy = m.policy;
            reg.MedicalDescription = m.medical;
            m.testing = (reg.Doctor == "test");

            var t = m.PrepareSummaryText();
            var bits = System.Text.ASCIIEncoding.ASCII.GetBytes(t);
            var i = ImageData.Image.NewTextFromBits(bits);
            reg.ImgId = i.Id;
            reg.IsDocument = true;
            DbUtil.Db.SubmitChanges();
            m.regid = reg.Id;

            Util.Email2(new SmtpClient(), m.email, 
                DbUtil.Settings("RecMail", DbUtil.SystemEmailAddress), 
                "{0} Registration".Fmt(m.division.Name), 
                "{0}({1}) has registered for {2}: {3}"
                .Fmt(m.participant.Name, m.participant.PeopleId, 
                m.division.Name, m.organization.OrganizationName));

            if ((reg.FeePaid ?? false) == true)
                return RedirectToAction("Confirm", new { id = reg.Id });
            else
            {
                TempData["model"] = m;
                return RedirectToAction("Payment");
            }
        }
        public ActionResult Payment()
        {
            if (!TempData.ContainsKey("model"))
                return View("Unknown");
            var tm = TempData["model"] as Models.RecRegModel;
            return View(tm);
        }
        public ActionResult Confirm(int? id, string TransactionID, string Misc3)
        {
            if (!id.HasValue)
                return View("Unknown");
            var m = new Models.RecRegModel { regid = id };
            if (TransactionID.HasValue())
            {
                m.registration.FeePaid = true;
                m.registration.TransactionId = TransactionID;
                DbUtil.Db.SubmitChanges();
            }
            decimal amt = 0;
            if (Misc3.HasValue())
                amt = decimal.Parse(Misc3);

            Util.Email(DbUtil.Settings("RecMail", DbUtil.SystemEmailAddress),
    "", m.registration.Email, "Recreation Registration",
@"<p>Thank you for registering for {0}: {1} 
You will receive another email with team information once they have been established.</p>
<p>You will need to download the <a href=""{3}/Upload/MedicalRelease.pdf"">Medical Release Form</a>, 
print, sign, and return it to the Recreation Ministry in order to complete your registration.</p>
<p>We have the following information:
{2}
".Fmt(m.division.Name, m.organization.OrganizationName,
    ImageData.Image.Content(m.registration.ImgId.Value), Util.CmsHost));

            Util.Email2(new SmtpClient(), m.email,
                DbUtil.Settings("RecMail", DbUtil.SystemEmailAddress),
                "{0} Registration".Fmt(m.division.Name),
@"{0}({1}) has registered for {2}: {3}\r\n
Feepaid: {4:C}, TransactionID: {5}"
                .Fmt(m.participant.Name, m.participant.PeopleId,
                m.division.Name, m.organization.OrganizationName, 
                amt, TransactionID));

            return View(m);
        }
        public JsonResult CityState(string id)
        {
            var z = DbUtil.Db.ZipCodes.SingleOrDefault(zc => zc.Zip == id);
            if (z == null)
                return Json(null);
            return Json(new { city = z.City.Trim(), state = z.State });
        }
    }
}
