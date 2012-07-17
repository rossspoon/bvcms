using System;
using System.Linq;
using System.Text;
using CmsData;
using UtilityExtensions;

namespace CmsWeb.Models
{
	public class PaymentForm
	{
		public decimal? AmtToPay { get; set; }
		public decimal? Donate { get; set; }
		public decimal Amtdue { get; set; }
		public string Coupon { get; set; }
		public string CreditCard { get; set; }
		public string Expires { get; set; }
		public string MaskedCCV { get; set; }
		public string CCV { get; set; }
		public string Routing { get; set; }
		public string Account { get; set; }
		public string Type { get; set; }
		public bool AskDonation { get; set; }
		public bool AllowCoupon { get; set; }
		public string Url { get; set; }
		public int timeout { get; set; }
		public string Terms { get; set; }
		public int DatumId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool PayBalance { get; set; }
		public int? OrgId { get; set; }
		public int? OriginalId { get; set; }
		public bool testing { get; set; }
		public bool? FinanceOnly { get; set; }
		public bool? IsLoggedIn { get; set; }
		public bool? CanSave { get; set; }
		public bool? SavePayInfo { get; set; }
		public bool NoCreditCardsAllowed { get; set; }
		private bool? _noEChecksAllowed;
		public bool NoEChecksAllowed
		{
			get
			{
				if (!_noEChecksAllowed.HasValue)
					_noEChecksAllowed = OnlineRegModel.GetTransactionGateway() != "sage";
				return _noEChecksAllowed.Value;
			}
		}


		public string Email { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Zip { get; set; }
		public string Phone { get; set; }
		public int? TranId { get; set; }

		public Transaction CreateTransaction(CMSDataContext Db)
		{
			var ti = new Transaction
					 {
						 Name = Name,
						 Donate = Donate,
						 Regfees = AmtToPay,
						 Amtdue = Amtdue,
						 Emails = Email,
						 Testing = testing,
						 Description = Description,
						 OrgId = OrgId,
						 Url = Url,
						 TransactionGateway = OnlineRegModel.GetTransactionGateway(),
						 Address = Address.Truncate(50),
						 City = City,
						 State = State,
						 Zip = Zip,
						 DatumId = DatumId,
						 Phone = Phone,
						 OriginalId = OriginalId,
						 Financeonly = FinanceOnly,
						 TransactionDate = DateTime.Now
					 };
			Db.Transactions.InsertOnSubmit(ti);
			Db.SubmitChanges();
			if (OriginalId == null) // first transaction
				ti.OriginalId = ti.Id;
			return ti;
		}
		public static PaymentForm CreatePaymentForm(Transaction ti)
		{
			PaymentInfo pi = null;
			if (ti.Person != null && string.Equals(OnlineRegModel.GetTransactionGateway(),
						"Sage", StringComparison.InvariantCultureIgnoreCase))
				pi = ti.Person.PaymentInfos.FirstOrDefault();
			if (pi == null)
				pi = new PaymentInfo();
			var pf = new PaymentForm
					 {
						 PayBalance = true,
						 AmtToPay = ti.Amtdue ?? 0,
						 Amtdue = ti.Amtdue ?? 0,
						 AllowCoupon = true,
						 AskDonation = false,
						 Description = ti.Description,
						 OrgId = ti.OrgId,
						 OriginalId = ti.OriginalId,
						 Email = Util.FirstAddress(ti.Emails).Address,
						 Name = ti.Name,
						 Phone = ti.Phone,
						 Address = ti.Address,
						 City = ti.City,
						 State = ti.State,
						 Zip = ti.Zip,
						 timeout = 6000000,
						 testing = ti.Testing ?? false,
						 TranId = ti.Id,
#if DEBUG2
						 CreditCard = "4111111111111111",
						 CCV = "123",
						 Expires = "1015",
						 Routing = "056008849",
						 Account = "12345678901234"
#else
						 CreditCard = pi.MaskedCard,
						 MaskedCCV = Util.Mask(new StringBuilder(pi.Ccv), 0),
						 CCV = pi.Ccv,
						 Expires = pi.Expires,
						 Account = pi.MaskedAccount,
						 Routing = pi.Routing,
						 SavePayInfo =
							(pi.MaskedAccount != null && pi.MaskedAccount.StartsWith("X"))
							|| (pi.MaskedCard != null && pi.MaskedCard.StartsWith("X")),
#endif
					 };
			pf.Type = pf.NoEChecksAllowed ? "C" : "";
			return pf;
		}
		public static PaymentForm CreatePaymentForm(OnlineRegModel m)
		{
			var p = m.List[0];
			var pp = p.person;
			PaymentInfo pi = null;
			if (m.user != null && string.Equals(OnlineRegModel.GetTransactionGateway(), 
								"Sage", StringComparison.InvariantCultureIgnoreCase))
			{
				pp = m.user;
				pi = pp.PaymentInfos.FirstOrDefault();
			}
			if (pi == null)
				pi = new PaymentInfo { MaskedAccount = "", MaskedCard = "" };

		var pf = new PaymentForm
					 {
						 AmtToPay = m.Amount() + (m.donation ?? 0),
						 AskDonation = m.AskDonation(),
						 AllowCoupon = !m.OnlineGiving(),
						 PayBalance = false,
						 Amtdue = m.TotalAmount() + (m.donation ?? 0),
						 Donate = m.donation,
						 Description = m.Header,
						 Email = pp != null ? pp.EmailAddress : p.email,
						 Name = m.NameOnAccount,
						 IsLoggedIn = m.UserPeopleId.HasValue,
						 OrgId = p.orgid,
						 Url = m.URL,
						 testing = m.testing ?? false,
						 Terms = m.Terms,
#if DEBUG2
						 CreditCard = "4111111111111111",
						 CCV = "123",
						 Expires = "1015",
						 Routing = "056008849",
						 Account = "12345678901234"
#else
						 CreditCard = pi.MaskedCard,
						 Account = pi.MaskedAccount,
						 Routing = pi.Routing,
						 Expires = pi.Expires,
						 MaskedCCV = Util.Mask(new StringBuilder(pi.Ccv), 0),
						 CCV = pi.Ccv,
						 SavePayInfo =
							(pi.MaskedAccount != null && pi.MaskedAccount.StartsWith("X"))
							|| (pi.MaskedCard != null && pi.MaskedCard.StartsWith("X")),
						 Type = pi.PreferredPaymentType,
#endif
					 };
			if (m.UserPeopleId.HasValue || p.IsNew)
			{
				if (pp == null)
				{
					pf.Address = p.address.Truncate(50);
					pf.City = p.city;
					pf.State = p.state;
					pf.Zip = p.zip;
					pf.Phone = p.phone.FmtFone();
				}
				else
				{
					pf.Address = pp.PrimaryAddress.Truncate(50);
					pf.City = pp.PrimaryCity;
					pf.State = pp.PrimaryState;
					pf.Zip = pp.PrimaryZip;
					pf.Phone = Util.PickFirst(pp.HomePhone, pp.CellPhone).FmtFone();
				}
			}
			if (m.OnlineGiving())
			{
				pf.NoCreditCardsAllowed = DbUtil.Db.Setting("NoCreditCardGiving", "false").ToBool();
				pf.Type = pi.PreferredGivingType;
				if (pf.NoCreditCardsAllowed)
					pf.Type = "B"; // bank account only
				else if (pf.NoEChecksAllowed)
					pf.Type = "C"; // credit card only
			}
			pf.Type = pf.NoEChecksAllowed ? "C" : pf.Type;
			return pf;
		}
		public static Transaction CreateTransaction(CMSDataContext Db, Transaction t)
		{
			var ti = new Transaction
					 {
						 Name = t.Name,
						 Donate = t.Donate,
						 Amtdue = t.Amtdue,
						 Emails = Util.FirstAddress(t.Emails).Address,
						 Testing = t.Testing,
						 Description = t.Description,
						 OrgId = t.OrgId,
						 Url = t.Url,
						 Address = t.Address,
						 TransactionGateway = OnlineRegModel.GetTransactionGateway(),
						 City = t.City,
						 State = t.State,
						 Zip = t.Zip,
						 DatumId = t.DatumId,
						 Phone = t.Phone,
						 OriginalId = t.OriginalId ?? t.Id,
						 Financeonly = t.Financeonly,
						 TransactionDate = DateTime.Now,
					 };
			Db.Transactions.InsertOnSubmit(ti);
			Db.SubmitChanges();
			return ti;
		}
		public object Autocomplete(bool small = false)
		{
			string auto;
#if DEBUG
			auto = "on";
#else
			auto = "off";
#endif
			if (small)
				return new
				{
					AUTOCOMPLETE = auto,
					@class = "short"
				};
			return new
			{
				AUTOCOMPLETE = auto,
			};
		}
	}
}

