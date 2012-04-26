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
using CmsData.Codes;

namespace CmsWeb.Areas.OnlineReg.Controllers
{
    public partial class OnlineRegController
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

            string first, last;
            Person.NameSplit(pf.ti.Name, out first, out last);
            var pid = m.UserPeopleId ?? 0;
            if (pid == 0)
            {
                var pds = DbUtil.Db.FindPerson(first, last, null, pf.ti.Emails, pf.ti.Phone);
                if (pds.Count() == 1)
                    pid = pds.Single().PeopleId.Value;
            }
			var tt = DbUtil.Db.Transactions.Single(tr => tr.Id == m.Transaction.Id);
            TransactionResponse tinfo = null;
            if (tt.TransactionGateway.ToLower() == "authorizenet")
                tinfo = OnlineRegModel.PostTransaction(
                    pf.CreditCard, pf.CCV, pf.Expires,
                    pf.ti.Amt ?? 0,
                    m.TranId.Value, m.Header,
                    pid, pf.ti.Emails, first, last,
                    pf.ti.Address, pf.ti.City, pf.ti.State, pf.ti.Zip,
                    m.Transaction.Testing ?? false);
            else if (tt.TransactionGateway.ToLower() == "sage")
                tinfo = OnlineRegModel.PostTransactionSage(
                    pf.CreditCard, pf.CCV, pf.Expires,
                    pf.ti.Amt ?? 0,
                    m.TranId.Value, m.Header,
                    pid, pf.ti.Emails, first, last,
                    pf.ti.Address, pf.ti.City, pf.ti.State, pf.ti.Zip, pf.ti.Phone,
                    m.Transaction.Testing ?? false);
            else
				return Redirect("/Home/ShowError/?error=unknown gateway " + tt.TransactionGateway);

            if (tinfo.Approved == false)
            {
                ModelState.AddModelError("form", tinfo.Message);
                // fill in things for new transaction that did not come with POST
                pf.ti.TransactionId = tinfo.TransactionId;
				pf.ti.TransactionGateway = tt.TransactionGateway;
				pf.ti.Testing = tt.Testing;
                pf.ti.Approved = tinfo.Approved;
                pf.ti.Message = tinfo.Message;
                pf.ti.AuthCode = tinfo.AuthCode;
                pf.ti.TransactionDate = DateTime.Now;
                pf.ti.Description = tt.Description;
                pf.ti.OrgId = tt.OrgId;
                pf.ti.OriginalId = tt.OriginalId;
                pf.ti.Participants = tt.Participants;
				pf.ti.Financeonly = tt.Financeonly;
				pf.ti.Address = (pf.ti.Address ?? "").Truncate(50);
                DbUtil.Db.Transactions.InsertOnSubmit(pf.ti);
                DbUtil.Db.SubmitChanges();
                SetHeaders(m);
                return View(pf);
            }
            // update information for sucessful transaction from POST

            tt.TransactionId = tinfo.TransactionId;
            tt.Approved = tinfo.Approved;
            tt.Message = tinfo.Message;
            tt.AuthCode = tinfo.AuthCode;
            tt.TransactionDate = DateTime.Now;
            tt.Emails = pf.ti.Emails;
            tt.Address = (pf.ti.Address ?? "").Truncate(50);
            tt.City = pf.ti.City;
            tt.State = pf.ti.State;
            tt.Zip = pf.ti.Zip;
            tt.Emails = pf.ti.Emails;
            tt.Name = pf.ti.Name;
            tt.Phone = pf.ti.Phone;
            tt.Amt = pf.ti.Amt;  // total, includes donation
            if (pf.ti.Donate > 0)
                tt.Donate = pf.ti.Donate;
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
            t.TransactionDate = Util.Now;

            if (m.org != null && m.org.RegistrationTypeId == RegistrationTypeCode.CreateAccount)
            {
                m.List[0].CreateAccount();
                ViewData["CreatedAccount"] = true;
                confirm = "ConfirmAccount";
            }
            else if (m.org != null && m.org.RegistrationTypeId == RegistrationTypeCode.OnlineGiving)
            {
                var p = m.List[0];
                if (p.IsNew)
                    p.AddPerson(null, p.org.EntryPointId ?? 0);

                var staff = DbUtil.Db.StaffPeopleForOrg(p.org.OrganizationId)[0];
                var text = p.setting.Body.Replace("{church}", DbUtil.Db.Setting("NameOfChurch", "church"));
                text = text.Replace("{amt}", (t.Amt ?? 0).ToString("N2"));
                text = text.Replace("{date}", DateTime.Today.ToShortDateString());
                text = text.Replace("{tranid}", t.Id.ToString());
                text = text.Replace("{name}", p.person.Name);
                text = text.Replace("{account}", "");
                text = text.Replace("{email}", p.person.EmailAddress);
                text = text.Replace("{phone}", p.person.HomePhone.FmtFone());
                text = text.Replace("{contact}", staff.Name);
                text = text.Replace("{contactemail}", staff.EmailAddress);
                text = text.Replace("{contactphone}", p.org.PhoneNumber.FmtFone());
                var re = new Regex(@"(?<b>.*?)<!--ITEM\sROW\sSTART-->(?<row>.*?)\s*<!--ITEM\sROW\sEND-->(?<e>.*)", RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);
                var match = re.Match(text);
                var b = match.Groups["b"].Value;
                var row = match.Groups["row"].Value.Replace("{funditem}", "{0}").Replace("{itemamt}", "{1:N2}");
                var e = match.Groups["e"].Value;
                var sb = new StringBuilder(b);

                var desc = "{0}; {1}; {2}".Fmt(
                        p.person.Name,
                        p.person.PrimaryAddress,
                        p.person.PrimaryZip);
                foreach (var g in p.FundItemsChosen())
                {
					if (g.amt > 0)
					{
						sb.AppendFormat(row, g.desc, g.amt);
						p.person.PostUnattendedContribution(DbUtil.Db,
						                                    g.amt,
						                                    g.fundid,
						                                    desc);
					}
                }
                t.Financeonly = true;
                if (t.Donate > 0)
                {
                    var fundname = DbUtil.Db.ContributionFunds.Single(ff => ff.FundId == p.setting.DonationFundId).FundName;
                    sb.AppendFormat(row, fundname, t.Donate);
                    t.Fund = p.setting.DonationFund();
                    p.person.PostUnattendedContribution(DbUtil.Db,
                        t.Donate.Value,
                        p.setting.DonationFundId,
                        desc);
                }
                sb.Append(e);
                if (!t.TransactionId.HasValue())
                {
                    t.TransactionId = TransactionID;
                    if (m.testing == true)
                        t.TransactionId += "(testing)";
                }
                Util.SendMsg(Util.SysFromEmail, Util.Host, Util.TryGetMailAddress(DbUtil.Db.StaffEmailForOrg(p.org.OrganizationId)),
                    p.setting.Subject, sb.ToString(),
                    Util.EmailAddressListFromString(p.person.FromEmail), 0, p.PeopleId);
                Util.SendMsg(Util.SysFromEmail, Util.Host, Util.TryGetMailAddress(p.person.FromEmail),
                    "online giving contribution received",
                    "see contribution records for {0} ({1})".Fmt(p.person.Name, p.PeopleId),
                    Util.EmailAddressListFromString(DbUtil.Db.StaffEmailForOrg(p.org.OrganizationId)),
                    0, p.PeopleId);
                if (p.CreatingAccount == true)
                    p.CreateAccount();
            }
            else if (m.ManagingSubscriptions())
            {
                m.ConfirmManageSubscriptions();
                ViewData["ManagingSubscriptions"] = true;
                ViewData["CreatedAccount"] = m.List[0].CreatingAccount;
                confirm = "ConfirmAccount";
            }
            else if (m.ChoosingSlots())
            {
                m.ConfirmPickSlots();
                ViewData["ManagingVolunteer"] = true;
                ViewData["CreatedAccount"] = m.List[0].CreatingAccount;
                confirm = "ConfirmAccount";
            }
            else if (m.OnlinePledge())
            {
                m.ConfirmManagePledge();
                ViewData["ManagingPledge"] = true;
                ViewData["CreatedAccount"] = m.List[0].CreatingAccount;
                confirm = "ConfirmAccount";
            }
            else if (m.ManageGiving())
            {
                m.ConfirmManageGiving();
                ViewData["ManagingGiving"] = true;
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
                if (m.List.Any(pp => pp.PeopleId == null))
                    return Content("no person");
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
                if (m.List.Any(pp => pp.PeopleId == null))
                    return Content("no person");
                m.UseCoupon(t.TransactionId);
            }


            DbUtil.Db.ExtraDatas.DeleteOnSubmit(ed);
            DbUtil.Db.SubmitChanges();
            if (m.IsCreateAccount() || m.ManagingSubscriptions())
                ViewData["email"] = m.List[0].person.EmailAddress;
            else
                ViewData["email"] = m.List[0].email;
            ViewData["orgname"] = m.org != null ? m.org.OrganizationName 
                                : m.masterorgid.HasValue ? m.masterorg.OrganizationName
                                : m.div.Name;
            ViewData["message"] = t.Message;

            SetHeaders(m);
            return View(confirm, m);
        }
    }
}
