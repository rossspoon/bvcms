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
using CmsWeb.Areas.OnlineReg.Models.Payments;
using CmsData.Codes;

namespace CmsWeb.Areas.OnlineReg.Controllers
{
    public partial class OnlineRegController : CmsController
    {
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ProcessPayment(int? id, PaymentForm pf)
        {
            if (pf.ti.Amt < 0)
                pf.ti.Amt = 0;
            if (pf.ti.Donate < 0)
                pf.ti.Donate = 0;
            if ((pf.ti.Amt ?? 0) <= 0 && (pf.ti.Donate ?? 0) <= 0)
            {
                DbUtil.Db.SubmitChanges();
                return RedirectToAction("Confirm", new { id = id, TransactionID = "zero paid", });
            }
            var ed = DbUtil.Db.ExtraDatas.SingleOrDefault(e => e.Id == id);
            if (ed == null)
                return Content("no pending confirmation found");
            var m = Util.DeSerialize<OnlineRegModel>(ed.Data);
            m.ParseSettings();

            var terms = Util.PickFirst(m.Terms, "");
            if (terms.HasValue())
                ViewData["Terms"] = terms;
            ViewData["timeout"] = INT_timeout;

            var t = m.Transaction;
            ITransactionPost tctl = null;
            if (t.TransactionGateway == "Sage")
                tctl = new Sage();
            else if (t.TransactionGateway == "AuthorizeNet")
                tctl = new AuthorizeNet();
            if (tctl == null)
                return Content("no gateway");

            string first, last;
            Person.NameSplit(pf.ti.Name, out first, out last);
            var pid = m.UserPeopleId ?? 0;
            if (pid == 0)
            {
                var pds = DbUtil.Db.FindPerson(first, last, null, pf.ti.Emails, pf.ti.Phone);
                if (pds.Count() == 1)
                    pid = pds.Single().PeopleId.Value;
            }
            var tinfo = tctl.PostTransaction(
                pf.CreditCard,
                pf.CCV,
                pf.Expires,
                pf.ti.Amt ?? 0,
                m.TranId.Value,
                m.Header,
                pid,
                pf.ti.Emails,
                first,
                last,
                pf.ti.Address,
                pf.ti.City,
                pf.ti.State,
                pf.ti.Zip,
                m.Transaction.Testing ?? false);

            if (tinfo.Approved == false)
            {
                ModelState.AddModelError("form", tinfo.Message);
                // fill in things for new transaction that did not come with POST
                pf.ti.TransactionId = tinfo.TransactionId;
                pf.ti.Approved = tinfo.Approved;
                pf.ti.Message = tinfo.Message;
                pf.ti.AuthCode = tinfo.AuthCode;
                pf.ti.TransactionDate = DateTime.Now;
                pf.ti.Description = t.Description;
                pf.ti.OrgId = t.OrgId;
                pf.ti.OriginalId = t.OriginalId;
                pf.ti.Participants = t.Participants;
                DbUtil.Db.Transactions.InsertOnSubmit(pf.ti);
                DbUtil.Db.SubmitChanges();
                SetHeaders(m);
                return View(pf);
            }
            // update information for sucessful transaction from POST
            t.TransactionId = tinfo.TransactionId;
            t.Approved = tinfo.Approved;
            t.Message = tinfo.Message;
            t.AuthCode = tinfo.AuthCode;
            t.TransactionDate = DateTime.Now;
            t.Emails = pf.ti.Emails;
            t.Address = pf.ti.Address;
            t.City = pf.ti.City;
            t.State = pf.ti.State;
            t.Zip = pf.ti.Zip;
            t.Emails = pf.ti.Emails;
            t.Name = pf.ti.Name;
            t.Phone = pf.ti.Phone;
            t.Amt = pf.ti.Amt;  // total, includes donation
            if (pf.ti.Donate > 0)
                t.Donate = pf.ti.Donate;
            DbUtil.Db.SubmitChanges();
            return RedirectToAction("Confirm", new { id = id, TransactionID = tinfo.TransactionId, Amount = pf.ti.Amt });
        }
        public ActionResult Confirm(int? id, string TransactionID, decimal? Amount)
        {
            if (!id.HasValue)
                return View("Unknown");
            if (!TransactionID.HasValue())
                return Content("error no transaction");

            var ed = DbUtil.Db.ExtraDatas.SingleOrDefault(e => e.Id == id);
            if (ed == null)
                return Content("no pending confirmation found");

            var m = Util.DeSerialize<OnlineRegModel>(ed.Data);
            m.ParseSettings();
            string confirm = "Confirm";
            var t = m.Transaction;
            t.Approved = true;

            if (m.org != null && m.org.RegistrationTypeId == RegistrationEnum.CreateAccount)
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
            else if (t.TransactionGateway == "ServiceU")
            {
                t.TransactionId = TransactionID;
                if (m.testing == true)
                    t.TransactionId += "(testing)";
                t.Message = "Transaction Completed";
                t.Approved = true;
                m.EnrollAndConfirm();
                m.UseCoupon(t.TransactionId);
            }
            else
            {
                if (!t.TransactionId.HasValue())
                {
                    t.TransactionId = TransactionID;
                    if (m.testing == true)
                        t.TransactionId += "(testing)";
                }
                m.EnrollAndConfirm();
                m.UseCoupon(t.TransactionId);
            }

            DbUtil.Db.ExtraDatas.DeleteOnSubmit(ed);
            DbUtil.Db.SubmitChanges();
            if (m.IsCreateAccount() || m.ManagingSubscriptions())
                ViewData["email"] = m.List[0].person.EmailAddress;
            else
                ViewData["email"] = m.List[0].email;
            ViewData["orgname"] = m.org == null ? m.div.Name : m.org.OrganizationName;
            ViewData["message"] = t.Message;

            SetHeaders(m);
            return View(confirm, m);
        }
    }
}
