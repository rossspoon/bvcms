using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CmsData;
using CmsWeb.Models;
using UtilityExtensions;
using System.Configuration;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace CmsWeb.Areas.Public.Controllers
{
    public class OnlineRegController : CmsController
    {
#if DEBUG
        private const int INT_timeout = 1600000;
#else
        private const int INT_timeout = 60000;
#endif

        public ActionResult Index(int? id, int? div, bool? testing)
        {
            if (!id.HasValue && !div.HasValue)
                return Content("no organization");
            var m = new OnlineRegModel
            {
                divid = div,
                orgid = id,
            };
            if (m.org == null && m.div == null)
                return Content("invalid registration");

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

            m.URL = Request.Url.OriginalString;
            ViewData["timeout"] = INT_timeout;
            SetHeaders(m.divid ?? m.orgid ?? 0);

#if DEBUG
            var om = DbUtil.Db.OrganizationMembers.SingleOrDefault(o => o.PeopleId == 828612 && o.Organization.OrganizationName == "Crossroads Camp");
            if (om != null)
            {
                om.Drop();
                DbUtil.Db.SubmitChanges();
            }
            om = DbUtil.Db.OrganizationMembers.SingleOrDefault(o => o.PeopleId == 827651 && o.Organization.OrganizationName == "Crossroads Camp");
            if (om != null)
            {
                om.Drop();
                DbUtil.Db.SubmitChanges();
            }

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
            DbUtil.Db.SetNoLock();
            var p = m.List[id];
            p.ValidateModelForFind(ModelState);
            if (p.org != null)
            {
                p.IsFilled = p.org.OrganizationMembers.Count() >= p.org.Limit;
                if (p.IsFilled)
                    ModelState.AddModelError("dob", "Sorry, that age group is filled");
                if (p.Found == true)
                {
                    FillPriorInfo(p);
                }
                if (!p.AnyOtherInfo())
                    p.OtherOK = true;
            }
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
                    if (p.org.AskTylenolEtc == true)
                    {
                        p.tylenol = rr.Tylenol;
                        p.advil = rr.Advil;
                        p.robitussin = rr.Robitussin;
                        p.maalox = rr.Maalox;
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
                first = "Bethany",
                last = "Carroll",
                dob = "1/29/86",
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
            DbUtil.Db.SetNoLock();
            var d = DbUtil.Db.GetDatum<OnlineRegModel>(m);

            if (m.Amount() == 0 && !m.Terms.HasValue())
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
                Terms = Util.PickFirst(m.Terms, ""),
                _URL = m.URL,
                _timeout = INT_timeout,
                _datumid = d.Id,
                _confirm = "confirm"
            };
            pm.Misc1 = pm.NameOnAccount;

            SetHeaders(m.orgid ?? m.divid ?? 0);
            if (m.Amount() == 0 && m.Terms.HasValue())
                return View("Terms", pm);
            return View("Payment", pm);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PayWithCoupon()
        {
            var pm = new PaymentModel();
            try
            {
                UpdateModel(pm);
            }
            catch(Exception)
            {
                return Json(new { error = "problem coupon" });
            }
            if (!pm._Coupon.HasValue())
                return Json(new { error = "empty coupon" });
            var ed = DbUtil.Db.ExtraDatas.SingleOrDefault(e => e.Id == pm._datumid);
            var m = Util.DeSerialize<OnlineRegModel>(ed.Data.Replace("CMSWeb.Models", "CmsWeb.Models"));
            string coupon = pm._Coupon.ToUpper().Replace(" ", "");
            if (coupon == DbUtil.Settings("AdminCoupon", "ifj4ijweoij"))
                return Json(new
                {
                    confirm = "/onlinereg/{0}/{1}?TransactionID=Coupon(Admin)"
                        .Fmt(pm._confirm, pm._datumid)
                });
            var c = DbUtil.Db.Coupons.SingleOrDefault(cp => cp.Id == coupon);
            if (c == null)
                return Json(new { error = "coupon not found" });
            if (m.divid.HasValue)
            {
                if (c.DivId != m.divid)
                    return Json(new { error = "coupon div not valid" });
            }
            else if (m.orgid != c.OrgId)
                return Json(new { error = "coupon org not valid" });
            if (DateTime.Now.Subtract(c.Created).TotalHours > 24)
                return Json(new { error = "coupon expired" });
            if (c.Used.HasValue)
                return Json(new { error = "coupon already used" });
            if (c.Canceled.HasValue)
                return Json(new { error = "coupon canceled" });
            return Json(new
            {
                confirm = "/onlinereg/{0}/{1}?TransactionID=Coupon({2})"
                    .Fmt(pm._confirm, pm._datumid, coupon.Insert(8, " ").Insert(4, " "))
            });
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

            var s = ed.Data.Replace("CMSWeb.Models", "CmsWeb.Models");
            var m = Util.DeSerialize<OnlineRegModel>(s);

            m.EnrollAndConfirm(TransactionID);
            UseCoupon(TransactionID, m.List[0].PeopleId.Value, m.Amount());

            DbUtil.Db.ExtraDatas.DeleteOnSubmit(ed);
            DbUtil.Db.SubmitChanges();
            ViewData["email"] = m.List[0].email;
            ViewData["orgname"] = m.org == null ? m.div.Name : m.org.OrganizationName;
            ViewData["URL"] = m.URL;
            ViewData["timeout"] = INT_timeout;

            SetHeaders(m.divid ?? m.orgid ?? 0);
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
            PaymentModel pm = null;
            try
            {
                var s = ed.Data.Replace("CMSWeb.Models", "CmsWeb.Models");
                var ti = Util.DeSerialize<TransactionInfo>(s);
                pm = new PaymentModel
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
                    _URL = ti.URL,
                    _timeout = INT_timeout,
                    _datumid = ed.Id,
                    _confirm = "confirm2"
                };

                SetHeaders(ti.orgid);
                return View("Payment2", pm);
            }
            catch (Exception ex)
            {
                throw new Exception("Problem in PayDue: " + ed.Data, ex);
            }
        }
        public ActionResult PayDueTest(string q)
        {
            if (!q.HasValue())
                return Content("unknown");
            var id = Util.Decrypt(q);
            var ed = DbUtil.Db.ExtraDatas.SingleOrDefault(e => e.Id == id.ToInt());
            if (ed == null)
                return Content("no outstanding transaction");
            return Content(ed.Data);
        }
        [ValidateInput(false)]
        public ActionResult Confirm2(int? id, string TransactionID, decimal Amount)
        {
            if (!id.HasValue)
                return View("Unknown");
            if (!TransactionID.HasValue())
                return Content("error no transaction");

            var ed = DbUtil.Db.ExtraDatas.SingleOrDefault(e => e.Id == id);
            if (ed == null)
                return Content("no pending transaction found");

            var ti = Util.DeSerialize<TransactionInfo>(ed.Data.Replace("CMSWeb.Models", "CmsWeb.Models"));
            var org = DbUtil.Db.LoadOrganizationById(ti.orgid);
            if (ti.AmountDue == Amount)
            {
                ti.AmountDue = 0;
                DbUtil.Db.ExtraDatas.DeleteOnSubmit(ed);
            }
            else
            {
                ti.AmountDue -= Amount;
                var s = Util.Serialize<TransactionInfo>(ti);
                ed.Data = s;
                ed.Stamp = Util.Now;
            }
            var amt = Amount;
            var smtp = Util.Smtp();
            foreach (var pi in ti.people)
            {
                var p = DbUtil.Db.LoadPersonById(pi.pid);
                if (p != null)
                {
                    var om = DbUtil.Db.OrganizationMembers.SingleOrDefault(m => m.OrganizationId == ti.orgid && m.PeopleId == pi.pid);

                    var due = (om.Amount - om.AmountPaid) ?? 0;
                    var pay = amt;
                    if (pay > due)
                        pay = due;
                    om.AmountPaid += pay;

                    if (pay > 0)
                    {
                        string tstamp = Util.Now.ToString("MMM d yyyy h:mm tt");
                        AddToMemberData(tstamp, om);
                        var tran = "{0:C} ({1} {2})".Fmt(
                            pay, TransactionID, ti.testing == true ? " test" : "");
                        AddToMemberData(tran, om);

                        var reg = p.RecRegs.Single();
                        AddToRegistrationComments("-------------", reg);
                        AddToRegistrationComments(tran, reg);
                        AddToRegistrationComments(Util.Now.ToString("MMM d yyyy h:mm tt"), reg);
                        AddToRegistrationComments("{0} - {1}".Fmt(org.DivisionName, org.OrganizationName), reg);
                    }
                    amt -= pay;
                }
                else
                    Util.Email2(smtp, org.EmailAddresses, org.EmailAddresses, "missing person on payment due",
                            "Cannot find {0} ({1}), payment due completed of {2:c} but no record".Fmt(pi.name, pi.pid, pi.amt));
            }
            DbUtil.Db.SubmitChanges();
            var names = string.Join(", ", ti.people.Select(i => i.name).ToArray());
            Util.Email2(smtp, org.EmailAddresses, ti.Email, "Payment confirmation",
                "Thank you for paying {0:c} for {1}.<br/>Your balance is {2:c}<br/>{3}".Fmt(Amount, ti.Header, ti.AmountDue, names));
            Util.Email2(smtp, ti.Email, org.EmailAddresses, "payment received for " + ti.Header,
                "{0} paid {1:c} for {2}, balance of {3:c}\n({4})".Fmt(ti.Name, Amount, ti.Header, ti.AmountDue, names));
            ViewData["URL"] = ti.URL;
            ViewData["timeout"] = INT_timeout;
            ViewData["Amount"] = Amount.ToString("c");
            ViewData["AmountDue"] = ti.AmountDue.ToString("c");
            ViewData["Desc"] = ti.Header;
            ViewData["Email"] = ti.Email;

            SetHeaders(ti.orgid);
            return View();
        }
        private static void UseCoupon(string TransactionID, int PeopleId, decimal Amount)
        {
            string matchcoupon = @"Coupon\((?<coupon>.*)\)";
            if (Regex.IsMatch(TransactionID, matchcoupon, RegexOptions.IgnoreCase))
            {
                var coupon = Regex.Match(TransactionID, matchcoupon, RegexOptions.IgnoreCase)
                        .Groups["coupon"].Value.Replace(" ", "");
                if (coupon != "Admin")
                {
                    var c = DbUtil.Db.Coupons.Single(cp => cp.Id == coupon);
                    c.RegAmount = Amount;
                    c.Used = DateTime.Now;
                    c.PeopleId = PeopleId;
                }
            }
        }
        private void SetHeaders(int id)
        {
            ViewData["header"] = DbUtil.Content("OnlineRegHeader-" + id,
                 DbUtil.Content("OnlineRegHeader", ""));
            ViewData["top"] = DbUtil.Content("OnlineRegTop-" + id,
                DbUtil.Content("OnlineRegTop", ""));
            ViewData["bottom"] = DbUtil.Content("OnlineRegBottom-" + id,
                DbUtil.Content("OnlineRegBottom", ""));
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
