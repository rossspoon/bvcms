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
using System.Runtime.Serialization;
using System.IO;
using System.Text;

namespace CMSWeb.Areas.Public.Controllers
{
    public class RecRegController : CmsController
    {
        public ActionResult Index(int? id, bool? testing)
        {
            if (!id.HasValue)
                return View("NoLeague");
            var league = DbUtil.Db.RecLeagues.SingleOrDefault(l => l.DivId == id);
            if (league == null)
                return Content("invalid league");
            ViewData["divid"] = id;
            ViewData["LeagueName"] = league.Division.Name + " Registration";

#if DEBUG
            testing = true;
            var m = new RecRegModel
            {
                first = "Davy",
                last = "Carroll",
                dob = "5/30/02",
                email = "david@davidcarroll.name",
                phone = "9017581862",
                homecell = "h",
                divid = id
            };
#else
            var m = new RecRegModel();
            m.divid = id;
#endif
            if (testing == true)
                ViewData["testing"] = "?testing=true";
            return View(m);

        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ShowMoreInfo(RecRegModel m)
        {
#if DEBUG
            m.address = "235 Riveredge Cv.";
            m.city = "Cordova";
            m.state = "TN";
            m.zip = "38018";
            m.gender = 1;
            m.married = 10;
#endif
            m.ShowAddress = true;
            return View("list", m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PersonFind(RecRegModel m)
        {
            m.ValidateModelForFind(ModelState);
            if (m.RecAgeDiv == null)
                ModelState.AddModelError("dob", "Sorry, cannot find an appropriate age division");
            else
            {
                m.IsFilled = m.RecAgeDiv.OrganizationMembers.Count() >= m.RecAgeDiv.Limit;
                if (m.IsFilled)
                    ModelState.AddModelError("dob", "Sorry, that age division is filled");
                ViewData["fee"] = m.Amount.ToString("C");
            }
            if (m.Found == true)
                FillPriorInfo(m);
            return View("list", m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SubmitNew(RecRegModel m)
        {
            m.ValidateModelForNew(ModelState);
            if (ModelState.IsValid)
            {
                if (m.RecAgeDiv == null)
                    ModelState.AddModelError("find", "Sorry, cannot find an appropriate age division");
                else
                {
                    m.IsFilled = m.RecAgeDiv.OrganizationMembers.Count() >= m.RecAgeDiv.Limit;
                    if (m.IsFilled)
                        ModelState.AddModelError("dob", "Sorry, that age division is filled");
                }
                m.IsNew = true;
            }
            if (m.IsNew)
                FillPriorInfo(m);
            return View("list", m);
        }
        private static void FillPriorInfo(RecRegModel m)
        {
#if DEBUG
            m.shirtsize = "YT-L";
            m.request = "tommy";
            m.emcontact = "test";
            m.emphone = "test";
            m.docphone = "test";
            m.doctor = "test";
            m.insurance = "test";
            m.policy = "test";
            m.mname = "test";
            m.fname = "test";
#endif
            if (!m.IsNew)
            {
                var rr = m.person.RecRegs.SingleOrDefault();
                if (rr != null)
                {
                    var om = m.GetOrgMember();
                    if (om != null)
                        m.request = om.Request;
                    m.shirtsize = rr.ShirtSize;
                    m.emcontact = rr.Emcontact;
                    m.emphone = rr.Emphone;
                    m.docphone = rr.Docphone;
                    m.doctor = rr.Doctor;
                    m.insurance = rr.Insurance;
                    m.policy = rr.Policy;
                    m.mname = rr.Mname;
                    m.fname = rr.Fname;
                    m.medical = rr.MedicalDescription;
                    m.coaching = rr.Coaching == true ? 1 : 0;
                    m.otherchurch = rr.ActiveInAnotherChurch ?? false;
                    m.member = rr.Member ?? false;
                }
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SubmitOtherInfo(RecRegModel m)
        {
            m.ValidateModelForOther(ModelState);
            m.OtherOK = ModelState.IsValid;
            return View("list", m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CompleteRegistration(int id, bool? testing, RecRegModel m)
        {
            var org = m.RecAgeDiv;

            var s = Util.Serialize<RecRegModel>(m);
            var d = new ExtraDatum { Data = s, Stamp = Util.Now };
            DbUtil.Db.ExtraDatas.InsertOnSubmit(d);
            DbUtil.Db.SubmitChanges();

            var om = m.GetOrgMember();
            if (om != null && om.Amount > 0)
                return RedirectToAction("Confirm",
                    new
                    {
                        id = d.Id,
                        TransactionID = "alreadypaid",
                        Misc3 = org.OrganizationId.ToString(),
                        Misc4 = ""
                    });

            var pm = new PaymentModel
            {
                NameOnAccount = m.first + " " + m.last,
                Address = m.address,
                Amount = m.Amount,
                City = m.city,
                Email = m.email,
                Phone = m.phone.FmtFone(),
                State = m.state,
                PostalCode = m.zip,
                testing = testing ?? false,
                PostbackURL = Request.Url.Scheme + "://" + Request.Url.Authority + "/RecReg/Confirm/" + d.Id,
                Misc1 = m.first + " " + m.last,
                Misc2 = org.OrganizationName,
                Misc3 = org.OrganizationId.ToString(),
                Misc4 = m.Amount.ToString()
            };
            return View("Payment", pm);
        }
        public ActionResult Confirm(int? id, string TransactionID, string Misc3, string Misc4)
        {
            if (!id.HasValue)
                return View("Unknown");
            if (!TransactionID.HasValue())
                return Content("error no transaction");

            var s = DbUtil.Db.ExtraDatas.Single(e => e.Id == id).Data;
            var m = Util.DeSerialize<RecRegModel>(s);

            m.Confirm(TransactionID, Misc4, Misc3.ToInt());
            return View(m);
        }
    }
}
