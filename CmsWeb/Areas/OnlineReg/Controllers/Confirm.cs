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

namespace CmsWeb.Areas.OnlineReg.Controllers
{
    public partial class OnlineRegController : CmsController
    {
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ProcessPayment(int? id, PaymentForm pf)
        {
            var ed = DbUtil.Db.ExtraDatas.SingleOrDefault(e => e.Id == id);
            if (ed == null)
                return Content("no pending confirmation found");
            var m = Util.DeSerialize<OnlineRegModel>(ed.Data);

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

            Person p = null;
            if (m.UserPeopleId.HasValue)
                p = m.user;
            else
                p = m.List[0].person;

            string first, last;
            Person.NameSplit(pf.Name, out first, out last);
            var tinfo = tctl.PostTransaction(
                pf.CreditCard,
                pf.CCV,
                pf.Expires,
                m.Transaction.Amt.Value,
                m.TranId.Value,
                p.PeopleId,
                first,
                last,
                pf.Address,
                pf.City,
                pf.State,
                pf.Zip,
                m.Transaction.Testing ?? false);
            //Html.ValidationMessage("shirtsize")
            if (tinfo.Approved == false)
            {
                ModelState.AddModelError("form", tinfo.Message);
                SetHeaders(m.orgid ?? m.divid ?? 0);
                return View(pf);
            }

            t.TransactionId = tinfo.TransactionId;
            t.Approved = tinfo.Approved;
            t.Message = tinfo.Message;
            t.AuthCode = tinfo.AuthCode;
            DbUtil.Db.SubmitChanges();

            return RedirectToAction("Confirm", new { id = id, TransactionID = "paid" });
        }
        public ActionResult Confirm(int? id, string TransactionID)
        {
            if (!id.HasValue)
                return View("Unknown");
            if (!TransactionID.HasValue())
                return Content("error no transaction");

            var ed = DbUtil.Db.ExtraDatas.SingleOrDefault(e => e.Id == id);
            if (ed == null)
                return Content("no pending confirmation found");

            var m = Util.DeSerialize<OnlineRegModel>(ed.Data);
            string confirm = "Confirm";
            var t = m.Transaction;
            t.Approved = true;

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
            else if (t.TransactionGateway == "ServiceU")
            {
                t.TransactionId = TransactionID;
                t.Message = "Transaction Completed";
                t.Approved = true;
                m.EnrollAndConfirm();
                m.UseCoupon(t.TransactionId);
            }
            else
            {
                if (!t.TransactionId.HasValue())
                    t.TransactionId = TransactionID;
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
            ViewData["URL"] = m.URL;
            ViewData["timeout"] = INT_timeout;
            ViewData["message"] = t.Message;

            SetHeaders(m.divid ?? m.orgid ?? 0);
            return View(confirm, m);
        }
    }
}
