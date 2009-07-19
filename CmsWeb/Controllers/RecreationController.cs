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
    public class RecreationController : Controller
    {
        public ActionResult Index(int id)
        {
            var m = new RecreationModel { divid = id };
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
            m.EnrollInOrg(m.participant);

            var reg = DbUtil.Db.Participants.SingleOrDefault(r =>
                r.PeopleId == m.participant.PeopleId
                && r.DivId == m.divid
                && r.OrgId == m.OrgId);
            if (reg == null)
            {
                reg = new Participant
                {
                    PeopleId = m.participant.PeopleId,
                    UserInfo = "online",
                    OrgId = m.OrgId,
                    DivId = m.divid,
                    Uploaded = DateTime.Now,
                };
                DbUtil.Db.Participants.InsertOnSubmit(reg);
                DbUtil.Db.SubmitChanges();
            }

            return RedirectToAction("OtherInfo", new { id = reg.Id });
        }
        public ActionResult OtherInfo(int id)
        {
            var m = new RecreationModel { regid = id };
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

            var t = m.PrepareSummaryText();
            var bits = System.Text.ASCIIEncoding.ASCII.GetBytes(t);
            var i = ImageData.Image.NewTextFromBits(bits);
            m.registration.ImgId = i.Id;
            m.registration.IsDocument = true;

            DbUtil.Db.SubmitChanges();

            HomeController.Email(ConfigurationManager.AppSettings["recmail"],
    "", m.email, "Recreation Registration",
@"<p>Thank you for registering for {0}: {1}
You will receive another email with team information once they have been established.</p>
<p>We have the following information:
<pre>
{2}
</pre>
".Fmt(m.division.Name, m.organization.OrganizationName, m.PrepareSummaryText()));

            return RedirectToAction("Confirm", new { id = m.regid });
        }
        public ActionResult Confirm(int id)
        {
            var m = new RecreationModel { regid = id };
            return View(m);
        }
    }
}
