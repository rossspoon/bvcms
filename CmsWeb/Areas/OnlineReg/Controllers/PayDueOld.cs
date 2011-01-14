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
                };

                SetHeaders(ti.orgid);
                return View("Payment2", pm);
            }
            catch (Exception ex)
            {
                throw new Exception("Problem in PayDue: " + ed.Data, ex);
            }
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

                    if (om != null)
                    {
                        var due = (om.Amount - om.AmountPaid) ?? 0;
                        var pay = amt;
                        if (pay > due)
                            pay = due;
                        om.AmountPaid += pay;

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

                        amt -= pay;
                    }
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
            return View("Confirm2");
        }
    }
}
