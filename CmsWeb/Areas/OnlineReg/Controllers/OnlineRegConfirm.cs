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
using CmsWeb.Areas.Manage.Controllers;

namespace CmsWeb.Areas.OnlineReg.Controllers
{
    public partial class OnlineRegController : CmsController
    {
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
            string confirm = "Confirm";
            if (m.org != null && m.org.RegistrationTypeId == (int)Organization.RegistrationEnum.CreateAccount)
            {
                m.List[0].CreateAccount();
                ViewData["CreatedAccount"] = true;
                confirm = "ConfirmAccount";
            }
            else if (m.ManagingSubscriptions())
            {
                m.ConfirmManageSubscriptions();
                ViewData["ManagingSubscriptions"] = true;
                ViewData["CreatedAccount"] = m.List[0].CreatingAccount;
                confirm = "ConfirmAccount";
            }
            else
            {
                m.EnrollAndConfirm(TransactionID);
                m.UseCoupon(TransactionID);
            }
            DbUtil.Db.ExtraDatas.DeleteOnSubmit(ed);
            DbUtil.Db.SubmitChanges();
            if (m.IsCreateAccount() || m.ManagingSubscriptions())
                ViewData["email"] = m.List[0].person.EmailAddress;
            else
                ViewData["email"] = m.List[0].email;
            ViewData["orgname"] = m.org == null ? m.div.Name : m.org.OrganizationName;
            ViewData["URL"] = m.URL;
            ViewData["timeout"] = INT_timeout;

            SetHeaders(m.divid ?? m.orgid ?? 0);
            return View(confirm, m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ConfirmSlots(int id, int orgid)
        {
            var m = new SlotModel(id, orgid);
            var slots = string.Join("<br />\n", m.MySlots());
            var smtp = Util.Smtp();
            Util.Email(smtp, m.org.EmailAddresses, m.person.EmailAddress, "Commitment confirmation",
@"Thank you for committing to {0}. You have the following slots:<br/>
{1}".Fmt(m.org.OrganizationName, slots));
            Util.Email(smtp, m.person.EmailAddress, m.org.EmailAddresses, "commitment received for " + m.org.OrganizationName,
                "{0} committed to:<br/>\n{1}".Fmt(m.org.OrganizationName, slots));
            return RedirectToAction("ConfirmSlots", new { id = m.org.OrganizationId });
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ConfirmSlots(int id)
        {
            var org = DbUtil.Db.LoadOrganizationById(id);
            ViewData["Organization"] = org.OrganizationName;
            SetHeaders(org.OrganizationId);
            return View();
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
                        om.AddToMemberData(tstamp);
                        var tran = "{0:C} ({1} {2})".Fmt(
                            pay, TransactionID, ti.testing == true ? " test" : "");
                        om.AddToMemberData(tran);

                        var reg = p.RecRegs.Single();
                        reg.AddToComments("-------------");
                        reg.AddToComments(tran);
                        reg.AddToComments(Util.Now.ToString("MMM d yyyy h:mm tt"));
                        reg.AddToComments("{0} - {1}".Fmt(org.DivisionName, org.OrganizationName));
                    }
                    amt -= pay;
                }
                else
                    Util.Email(smtp, org.EmailAddresses, org.EmailAddresses, "missing person on payment due",
                            "Cannot find {0} ({1}), payment due completed of {2:c} but no record".Fmt(pi.name, pi.pid, pi.amt));
            }
            DbUtil.Db.SubmitChanges();
            var names = string.Join(", ", ti.people.Select(i => i.name).ToArray());
            Util.Email(smtp, org.EmailAddresses, ti.Email, "Payment confirmation",
                "Thank you for paying {0:c} for {1}.<br/>Your balance is {2:c}<br/>{3}".Fmt(Amount, ti.Header, ti.AmountDue, names));
            Util.Email(smtp, ti.Email, org.EmailAddresses, "payment received for " + ti.Header,
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
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ConfirmSubscriptions(ManageSubsModel m)
        {
            m.UpdateSubscriptions();
            var smtp = Util.Smtp();
            var StaffEmail = ManageSubsModel.StaffEmail(m.divid);

            Util.Email(smtp, StaffEmail, m.person.EmailAddress,
                "Subscription Confirmation",
@"Thank you for managing your subscriptions to {0}<br/>
You have the following subscriptions:<br/>
{1}".Fmt(m.Division.Name, m.Summary));

            Util.Email(smtp, m.person.EmailAddress, StaffEmail,
                "Subscriptions managed",
@"{0} managed subscriptions to {1}<br/>
You have the following subscriptions:<br/>
{2}".Fmt(m.person.Name, m.Division.Name, m.Summary));

            SetHeaders(m.divid);
            return View(m);
        }
        public class ConfirmTestInfo
        {
            public ExtraDatum ed;
            public OnlineRegModel m;
        }
        [Authorize(Roles = "Admin")]
        public ActionResult ConfirmTest(int? id, int? count)
        {
            IEnumerable<ExtraDatum> q;
            if (id.HasValue)
                q = DbUtil.Db.ExtraDatas.Where(e => e.Id == id);
            else
                q = from ed in DbUtil.Db.ExtraDatas
                    where ed.Data.StartsWith("<OnlineRegModel ")
                    orderby ed.Stamp descending
                    select ed;
            var list = q.Take(count ?? 20).ToList();
            var q2 = from ed in list
                     let s = ed.Data.Replace("CMSWeb.Models", "CmsWeb.Models")
                     select new ConfirmTestInfo
                     {
                         ed = ed,
                         m = Util.DeSerialize<OnlineRegModel>(s) as OnlineRegModel
                     };
            return View(q2);
        }

    }
}
