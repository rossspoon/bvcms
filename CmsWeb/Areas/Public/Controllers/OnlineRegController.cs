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
    public class OnlineRegController : CmsController
    {
        public ActionResult Index(int? id, int? div, bool? testing)
        {
            if (!id.HasValue && !div.HasValue)
                return Content("no organization");
            var m = new OnlineRegModel
            {
                divid = div,
                orgid = id,
            };
            if (m.org != null)
            {
                if ((m.org.RegistrationTypeId ?? 0) == (int)CmsData.Organization.RegistrationEnum.None)
                    return Content("no registration allowed on this org");
            }
            else if (m.div != null)
            {
                var a = new int?[]
                {
                    (int)CmsData.Organization.RegistrationEnum.ComputeOrganizationByAge,
                    (int)CmsData.Organization.RegistrationEnum.UserSelectsOrganization
                };
                if (!m.div.Organizations.Any(o => a.Contains(o.RegistrationTypeId)))
                    return Content("no registration allowed on this div");
            }
            ViewData["url"] = Request.Path + Request.QueryString;
#if DEBUG
            ViewData["timeout"] = 106000;
#else
            ViewData["timeout"] = 60000;
#endif

#if DEBUG
            m.testing = true;
            m.List = new List<OnlineRegPersonModel>
            {
                new OnlineRegPersonModel
                {
                    divid = div,
                    orgid = id,
                    first = "David",
                    last = "Carroll",
                    dob = "5/30/52",
                    email = "david@davidcarroll.name",
                    phone = "9017581862",
                    homecell = "h",
                }
            };
#else
            m.testing = testing;
            m.List.Add(
                new OnlineRegPersonModel
                {
                    divid = div,
                    orgid = id,
                });
#endif
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ShowMoreInfo(int id, OnlineRegModel m)
        {
            var p = m.List[id];
#if DEBUG
            p.address = "235 Riveredge Cv.";
            p.city = "Cordova";
            p.state = "TN";
            p.zip = "38018";
            p.gender = 1;
            p.married = 10;
#endif
            if (id > 0)
            {
                var p0 = m.List[0];
                p.address = p0.address;
                p.city = p0.city;
                p.state = p0.state;
                p.zip = p0.zip;
            }
            p.ShowAddress = true;
            return View("list", m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PersonFind(int id, OnlineRegModel m)
        {
            var p = m.List[id];
            p.ValidateModelForFind(ModelState);
            if (p.org == null)
            {
                if (p.ComputesOrganizationByAge())
                    ModelState.AddModelError("dob", "Sorry, cannot find an appropriate age group");
            }
            else
            {
                p.IsFilled = p.org.OrganizationMembers.Count() >= p.org.Limit;
                if (p.IsFilled)
                    ModelState.AddModelError("dob", "Sorry, that age group is filled");
            }
            if (p.Found == true)
                FillPriorInfo(p);
            if (!p.AnyOtherInfo())
                p.OtherOK = true;
            return View("list", m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SubmitNew(int id, OnlineRegModel m)
        {
            var p = m.List[id];
            p.ValidateModelForNew(ModelState);
            if (ModelState.IsValid)
            {
                if (p.org == null)
                    ModelState.AddModelError("find", "Sorry, cannot find an appropriate age group");
                else
                {
                    p.IsFilled = p.org.OrganizationMembers.Count() >= p.org.Limit;
                    if (p.IsFilled)
                        ModelState.AddModelError("dob", "Sorry, that age group is filled");
                }
                p.IsNew = true;
            }
            p.IsValidForExisting = ModelState.IsValid == false;
            if (p.IsNew)
                FillPriorInfo(p);
            return View("list", m);
        }
        private static void FillPriorInfo(OnlineRegPersonModel p)
        {
#if DEBUG
            p.shirtsize = "YT-L";
            p.request = "tommy";
            p.emcontact = "test";
            p.emphone = "test";
            p.docphone = "test";
            p.doctor = "test";
            p.insurance = "test";
            p.policy = "test";
            p.mname = "";
            p.fname = "test t";
            p.tylenol = true;
            p.advil = true;
            p.robitussin = false;
            p.maalox = false;
            p.paydeposit = true;
#endif
            if (!p.IsNew)
            {
                var rr = p.person.RecRegs.SingleOrDefault();
                if (rr != null)
                {
                    if (p.org.AskRequest == true)
                    {
                        var om = p.GetOrgMember();
                        if (om != null)
                            p.request = om.Request;
                    }
                    if (p.org.AskShirtSize == true)
                        p.shirtsize = rr.ShirtSize;
                    if (p.org.AskEmContact == true)
                    {
                        p.emcontact = rr.Emcontact;
                        p.emphone = rr.Emphone;
                    }
                    if (p.org.AskInsurance == true)
                    {
                        p.insurance = rr.Insurance;
                        p.policy = rr.Policy;
                    }
                    if (p.org.AskDoctor == true)
                    {
                        p.docphone = rr.Docphone;
                        p.doctor = rr.Doctor;
                    }
                    if (p.org.AskParents == true)
                    {
                        p.mname = rr.Mname;
                        p.fname = rr.Fname;
                    }
                    if (p.org.AskAllergies == true)
                        p.medical = rr.MedicalDescription;
                    if (p.org.AskCoaching == true)
                        p.coaching = rr.Coaching;
                    if (p.org.AskChurch == true)
                    {
                        p.otherchurch = rr.ActiveInAnotherChurch ?? false;
                        p.memberus = rr.Member ?? false;
                    }
                }
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SubmitOtherInfo(int id, OnlineRegModel m)
        {
            var p = m.List[id];
            p.ValidateModelForOther(ModelState);
            p.OtherOK = ModelState.IsValid;
            return View("list", m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddAnotherPerson(OnlineRegModel m)
        {
            if (!ModelState.IsValid)
                return View("list", m.List);
#if DEBUG
            m.List.Add(new OnlineRegPersonModel
            {
                divid = m.divid,
                orgid = m.orgid,
                first = "Karney",
                last = "Carro",
                dob = "8/16/02",
                email = "davcar@pobox.com",
                phone = "9017581862".FmtFone(),
                homecell = "h"
            });
#else
            m.List.Add(new OnlineRegPersonModel
            {
                divid = m.divid,
                orgid = m.orgid,
            });
#endif
            return View("list", m);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CompleteRegistration(OnlineRegModel m)
        {
            var d = DbUtil.Db.GetDatum<OnlineRegModel>(m);

            if (m.Amount() == 0)
                return RedirectToAction("Confirm",
                    new
                    {
                        id = d.Id,
                        TransactionID = "zero due",
                    });

            var p = m.List[0];
            var pm = new PaymentModel
            {
                NameOnAccount = m.NameOnAccount,
                Address = p.address,
                Amount = m.Amount(),
                City = p.city,
                Email = p.email,
                Phone = p.phone.FmtFone(),
                State = p.state,
                PostalCode = p.zip,
                testing = m.testing ?? false,
                PostbackURL = Util.ServerLink("/OnlineReg/Confirm/" + d.Id),
                Misc2 = m.Header,
                Terms = Util.PickFirst(m.org.Terms, m.org.Division.Terms, "")
            };
            pm.Misc1 = pm.NameOnAccount;

            return View("Payment", pm);
        }
        [ValidateInput(false)]
        public ActionResult Confirm(int? id, string TransactionID)
        {
            if (!id.HasValue)
                return View("Unknown");
            if (!TransactionID.HasValue())
                return Content("error no transaction");

            var ed = DbUtil.Db.ExtraDatas.SingleOrDefault(e => e.Id == id);
            if (ed == null)
                return Content("no pending confirmation found");

            var s = ed.Data;
            var m = Util.DeSerialize<OnlineRegModel>(s);


            m.EnrollAndConfirm(TransactionID);

            DbUtil.Db.ExtraDatas.DeleteOnSubmit(ed);
            DbUtil.Db.SubmitChanges();
            ViewData["email"] = m.List[0].email;
            ViewData["orgname"] = m.org.OrganizationName;
            return View(m);
        }
        public ActionResult PayDue(string q)
        {
            if (!q.HasValue())
                return Content("unknown");
            var id = Util.Decrypt(q);
            var ed = DbUtil.Db.ExtraDatas.SingleOrDefault(e => e.Id == id.ToInt());
            if (ed == null)
                return Content("no outstanding transaction");
            var ti = Util.DeSerialize<TransactionInfo>(ed.Data);

            var pm = new PaymentModel
            {
                NameOnAccount = ti.Name,
                Address = ti.Address,
                Amount = ti.AmountDue,
                City = ti.City,
                Email = ti.Email,
                Phone = ti.Phone.FmtFone(),
                State = ti.State,
                PostalCode = ti.Zip,
                testing = ti.testing,
                PostbackURL = Util.ServerLink("/OnlineReg/Confirm2/" + id),
                Misc2 = ti.Header,
                Misc1 = ti.Name,
            };
            return View("Payment2", pm);
        }
        [ValidateInput(false)]
        public ActionResult Confirm2(int? id, string TransactionID)
        {
            if (!id.HasValue)
                return View("Unknown");
            if (!TransactionID.HasValue())
                return Content("error no transaction");

            var ed = DbUtil.Db.ExtraDatas.SingleOrDefault(e => e.Id == id);
            if (ed == null)
                return Content("no pending transaction found");

            var ti = Util.DeSerialize<TransactionInfo>(ed.Data);
            var org = DbUtil.Db.LoadOrganizationById(ti.orgid);
            DbUtil.Db.ExtraDatas.DeleteOnSubmit(ed);
            var smtp = Util.Smtp();
            foreach (var pi in ti.people)
            {
                var p = DbUtil.Db.LoadPersonById(pi.pid);
                if (p != null)
                {
                    var om = DbUtil.Db.OrganizationMembers.SingleOrDefault(m => m.OrganizationId == ti.orgid && m.PeopleId == pi.pid);
                    om.Amount = pi.amt;

                    string tstamp = Util.Now.ToString("MMM d yyyy h:mm tt");
                    AddToMemberData(tstamp, om);
                    AddToMemberData("{0:C} ({1})".Fmt(om.Amount.ToString2("C"), TransactionID), om);
                    if (ti.testing == true)
                        AddToMemberData("(test transaction)", om);

                    var reg = p.RecRegs.Single();
                    AddToRegistrationComments("-------------", reg);
                    AddToRegistrationComments("{0:C} ({1})".Fmt(om.Amount.ToString2("C"), TransactionID), reg);
                    AddToRegistrationComments(Util.Now.ToString("MMM d yyyy h:mm tt"), reg);
                    AddToRegistrationComments("{0} - {1}".Fmt(org.Division.Name, org.OrganizationName), reg);
                }
                else
                    Util.Email2(smtp, org.EmailAddresses, org.EmailAddresses, "missing person on payment due",
                            "Cannot find {0} ({1}), payment due completed of {2:c} but no record".Fmt(pi.name, pi.pid, pi.amt));
            }
            DbUtil.Db.SubmitChanges();
            Util.Email2(smtp, org.EmailAddresses, ti.Email, "Payment confirmation",
                "Thank you for paying the balance of {0:c} for {1}.".Fmt(ti.AmountDue, ti.Header));
            Util.Email2(smtp, ti.Email, org.EmailAddresses, "payment received for " + ti.Header,
                "{0} paid a balance of {1:c} for {2}.".Fmt(ti.Name, ti.AmountDue, ti.Header));
            return View(ti);
        }
        private static void AddToMemberData(string s, OrganizationMember om)
        {
            if (om.UserData.HasValue())
                om.UserData += "\n";
            om.UserData += s;
        }
        private static void AddToRegistrationComments(string s, RecReg rr)
        {
            rr.Comments = s + "\n" + rr.Comments;
        }

    }
}
