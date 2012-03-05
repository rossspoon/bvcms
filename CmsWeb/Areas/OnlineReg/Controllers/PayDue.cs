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
    public partial class OnlineRegController
    {
        // reached by the paylink in the confirmation email
        // which is produced in EnrollAndConfirm
        public ActionResult PayAmtDue(string q)
        {
            if (!q.HasValue())
                return Content("unknown");
            var id = Util.Decrypt(q).ToInt2();
            var qq = from t in DbUtil.Db.Transactions
                     where t.OriginalId == id || t.Id == id
                     orderby t.Id descending
                     select t;
            var ti = qq.FirstOrDefault();

            if (ti == null || ti.Amtdue == 0)
                return Content("no outstanding transaction");

            try
            {
                var ti2 = new Transaction
                {
                    Amt = ti.Amtdue, // assume they will pay entire amount due
                    Amtdue = ti.Amtdue, // this is what they owe
                    Name = ti.Name,
                    Address = ti.Address,
                    City = ti.City,
                    State = ti.State,
                    Zip = ti.Zip,
                    Testing = ti.Testing,
                    Description = ti.Description,
                    Url = ti.Url,
                    Emails = ti.Emails,
                    TransactionGateway = ti.TransactionGateway,
                    OrgId = ti.OrgId,
                    Participants = ti.Participants,
                    OriginalId = ti.OriginalId ?? ti.Id // links all the transactions together
                };
                foreach (var tp in ti.TransactionPeople)
                    ti2.TransactionPeople.Add(new TransactionPerson { Amt = tp.Amt, OrgId = tp.OrgId, PeopleId = tp.PeopleId });
                DbUtil.Db.Transactions.InsertOnSubmit(ti2);
                DbUtil.Db.SubmitChanges();

                ViewData["Confirm"] = "ConfirmDuePaid";
                ViewData["timeout"] = INT_timeout;

                SetHeaders(ti2.OrgId.Value);

                if (ti.TransactionGateway != "ServiceU")
                {
                    ViewData["PayBalance"] = true;
                    var pf = new PaymentForm { ti = ti2 };
                    return View("ProcessPayment", pf);
                }
                return View(ti2);
            }
            catch (Exception ex)
            {
                throw new Exception("Problem in PayDue: " + ti.Id, ex);
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
        // called from PayAmountDue
        public ActionResult ConfirmDuePaid(int? id, string TransactionID, decimal Amount)
        {
            var Db = DbUtil.Db;
            if (!id.HasValue)
                return View("Unknown");
            if (!TransactionID.HasValue())
                return Content("error no transaction");

            var ti = Db.Transactions.SingleOrDefault(tt => tt.Id == id);
            if (ti == null)
                return Content("no pending transaction found");

            var org = Db.LoadOrganizationById(ti.OrgId);
            ti.Amt = Amount;
            ti.Amtdue -= Amount;
            ti.TransactionId = TransactionID;
            ti.TransactionDate = DateTime.Now;
            var amt = Amount;
            string paylink = null;
            foreach (var pi in ti.TransactionPeople)
            {
                var p = Db.LoadPersonById(pi.PeopleId);
                if (p != null)
                {
                    var om = Db.OrganizationMembers.SingleOrDefault(m => m.OrganizationId == ti.OrgId && m.PeopleId == pi.PeopleId);
                    paylink = om.PayLink;

                    var due = (om.Amount - om.AmountPaid) ?? 0;
                    var pay = amt;
                    if (pay > due)
                        pay = due;
                    om.AmountPaid += pay;

                    string tstamp = Util.Now.ToString("MMM d yyyy h:mm tt");
                    om.AddToMemberData(tstamp);
                    var tran = "{0:C} ({1} {2})".Fmt(
                        pay, TransactionID, ti.Testing == true ? " test" : "");
                    om.AddToMemberData(tran);
                    om.AddToMemberData("(Total due {0:c})".Fmt(ti.Amtdue));

                    var reg = p.RecRegs.Single();
                    reg.AddToComments("-------------");
                    reg.AddToComments(tran);
                    reg.AddToComments("(Total due {0:c})".Fmt(ti.Amtdue));
                    reg.AddToComments(Util.Now.ToString("MMM d yyyy h:mm tt"));
                    reg.AddToComments("{0} - {1}".Fmt(org.DivisionName, org.OrganizationName));
                    amt -= pay;
                }
                else
                    Db.Email(Db.StaffEmailForOrg(org.OrganizationId),
                        Db.PeopleFromPidString(org.NotifyIds),
                        "missing person on payment due",
                        "Cannot find {0} ({1}), payment due completed of {2:c} but no record".Fmt(pi.Person.Name, pi.PeopleId, pi.Amt));
            }
            Db.SubmitChanges();
            var names = string.Join(", ", ti.TransactionPeople.Select(i => i.Person.Name).ToArray());
            var msg = "Thank you for paying {0:c} for {1}.<br/>Your balance is {2:c}<br/>{3}".Fmt(Amount, ti.Description, ti.Amtdue, names);
            if (ti.Amtdue > 0)
                msg += "<br/>\n<a href='{0}'>PayLink</a>".Fmt(paylink);

            var pid = ti.TransactionPeople.Select(t => t.PeopleId).FirstOrDefault();
            var p0 = Db.LoadPersonById(pid);
            if (p0 == null)
                Util.SendMsg(Util.SysFromEmail, Util.Host, Util.TryGetMailAddress(Db.StaffEmailForOrg(org.OrganizationId)),
                    "Payment confirmation", "Thank you for paying {0:c} for {1}.<br/>Your balance is {2:c}<br/>{3}".Fmt(Amount, ti.Description, ti.Amtdue, names), Util.ToMailAddressList(Util.FirstAddress(ti.Emails)), 0, pid);
            else
            {
                Db.Email(Db.StaffEmailForOrg(org.OrganizationId),
                    p0, Util.ToMailAddressList(ti.Emails), "Payment confirmation", "Thank you for paying {0:c} for {1}.<br/>Your balance is {2:c}<br/>{3}".Fmt(Amount, ti.Description, ti.Amtdue, names), false);
                Db.Email(p0.FromEmail,
                    Db.PeopleFromPidString(org.NotifyIds),
                    "payment received for " + ti.Description,
                    "{0} paid {1:c} for {2}, balance of {3:c}\n({4})".Fmt(ti.Name, Amount, ti.Description, ti.Amtdue, names));
            }

            ViewData["timeout"] = INT_timeout;

            SetHeaders(ti.OrgId.Value);
            return View(ti);
        }
    }
}
