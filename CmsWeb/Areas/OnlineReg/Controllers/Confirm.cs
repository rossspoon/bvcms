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
		public ActionResult ProcessPayment(PaymentForm pf)
		{
			OnlineRegModel m = null;
			var ed = DbUtil.Db.ExtraDatas.SingleOrDefault(e => e.Id == pf.DatumId);
			if (ed != null)
				m = Util.DeSerialize<OnlineRegModel>(ed.Data);

			if (pf.AmtToPay < 0) pf.AmtToPay = 0;
			if (pf.Donate < 0) pf.Donate = 0;

			if ((pf.AmtToPay ?? 0) <= 0 && (pf.Donate ?? 0) <= 0)
			{
				DbUtil.Db.SubmitChanges();
				ModelState.AddModelError("form", "amount zero");
				return View("ProcessPayment", pf);
			}

			try
			{
				var ti = ProcessPaymentTransaction(m, pf);

				SetHeaders(pf.OrgId ?? 0);
				ViewBag.Url = pf.Url;
				ViewBag.timeout = INT_timeout;
				if (ti.Approved == false)
				{
					ModelState.AddModelError("form", ti.Message);
					return View("ProcessPayment", pf);
				}
				if (pf.DatumId > 0)
					return View(ConfirmTransaction(m, ti.TransactionId, pf.AmtToPay));
				ConfirmDuePaidTransaction(ti, ti.TransactionId, pf.AmtToPay ?? 0);
				return View("ConfirmDuePaid", ti);
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("form", ex.Message);
				return View("ProcessPayment", pf);
			}
		}
		public Transaction ProcessPaymentTransaction(OnlineRegModel m, PaymentForm pf)
		{
			var ti = pf.CreateTransaction(DbUtil.Db);

			int? pid = null;
			if (m != null)
			{
				m.ParseSettings();
				var terms = Util.PickFirst(m.Terms, "");
				if (terms.HasValue())
					ViewData["Terms"] = terms;
				pid = m.UserPeopleId;
				m.TranId = ti.Id;
			}

			string first, last;
			Person.NameSplit(pf.Name, out first, out last);
			if (!pid.HasValue)
			{
				var pds = DbUtil.Db.FindPerson(first, last, null, pf.Email, pf.Phone);
				if (pds.Count() == 1)
					pid = pds.Single().PeopleId.Value;
			}
			TransactionResponse tinfo;
			var gateway = OnlineRegModel.GetTransactionGateway();
			if (gateway == "authorizenet")
				if (pf.Type == "B")
					tinfo = OnlineRegModel.PostECheck(
						pf.Routing, pf.Account,
						pf.AmtToPay ?? 0,
						ti.Id, pf.Description,
						pid ?? 0, first, last,
						pf.Address, pf.City, pf.State, pf.Zip,
						pf.testing);
				else
					tinfo = OnlineRegModel.PostTransaction(
						pf.CreditCard, pf.CCV, pf.Expires,
						pf.AmtToPay ?? 0,
						ti.Id, pf.Description,
						pid ?? 0, pf.Email, first, last,
						pf.Address, pf.City, pf.State, pf.Zip,
						pf.testing);
			else if (gateway == "sage")
				if (pf.Type == "B")
					tinfo = OnlineRegModel.PostVirtualCheckTransactionSage(
						pf.Routing, pf.Account,
						pf.AmtToPay ?? 0,
						ti.Id, pf.Description,
						pid ?? 0, pf.Email, first, last, "",
						pf.Address, pf.City, pf.State, pf.Zip, pf.Phone,
						pf.testing);
				else
					tinfo = OnlineRegModel.PostTransactionSage(
						pf.CreditCard, pf.CCV, pf.Expires,
						pf.AmtToPay ?? 0,
						ti.Id, pf.Description,
						pid ?? 0, pf.Email, first, last,
						pf.Address, pf.City, pf.State, pf.Zip, pf.Phone,
						pf.testing);

			else
				throw new Exception("unknown gateway " + gateway);

			ti.TransactionId = tinfo.TransactionId;
			if (ti.Testing == true)
				ti.TransactionId += "(testing)";
			ti.Approved = tinfo.Approved;
			ti.Message = tinfo.Message;
			ti.AuthCode = tinfo.AuthCode;
			ti.TransactionDate = DateTime.Now;
			DbUtil.Db.SubmitChanges();
			return ti;
		}

		private string ConfirmTransaction(OnlineRegModel m, string TransactionID, decimal? Amount)
		{
			m.ParseSettings();
			string confirm = "Confirm";
			var t = m.Transaction;
			if (t == null) // serviceu
			{
				var pf = PaymentForm.CreatePaymentForm(m);
				t = pf.CreateTransaction(DbUtil.Db);
				m.TranId = t.Id;
			}
			t.Amt = Amount;
			t.Amtdue -= t.Amt;
			ViewData["message"] = t.Message;
			t.Approved = true;
			t.TransactionDate = Util.Now;
			DbUtil.Db.SubmitChanges();

			if (m.org != null && m.org.RegistrationTypeId == RegistrationTypeCode.CreateAccount)
			{
				m.List[0].CreateAccount();
				ViewData["CreatedAccount"] = true;
				confirm = "ConfirmAccount";
			}
			else if (m.OnlineGiving())
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
					return "error: no person";
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
					return "error: no person";
				m.UseCoupon(t.TransactionId);
			}
			if (m.IsCreateAccount() || m.ManagingSubscriptions())
				ViewData["email"] = m.List[0].person.EmailAddress;
			else
				ViewData["email"] = m.List[0].email;
			ViewData["orgname"] = m.org != null ? m.org.OrganizationName
								: m.masterorgid.HasValue ? m.masterorg.OrganizationName
								: m.div.Name;
			return confirm;
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
			var confirm = ConfirmTransaction(m, TransactionID, Amount);
			if (confirm.StartsWith("error:"))
				return Content(confirm);
			ViewBag.Url = m.URL;

			DbUtil.Db.ExtraDatas.DeleteOnSubmit(ed);
			DbUtil.Db.SubmitChanges();

			SetHeaders(m);
			return View(confirm, m);
		}

	}
}
