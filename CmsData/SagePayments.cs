using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using CmsData;
using UtilityExtensions;
using System.Net;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Xml;
using System.Xml.XPath;

namespace CmsData
{
	public class SagePayments
	{
		string login;
		string key;
		CMSDataContext Db;

		public SagePayments(CMSDataContext Db, bool testing)
		{
#if DEBUG2
            testing = true;
#endif
			this.Db = Db;
			if (testing)
			{
				login = "287793447481";
				key = "S4S3N4D2W1D9";
			}
			else
			{
				login = Db.Setting("M_id", "");
				key = Db.Setting("M_key", "");
			}
		}
		private XElement getResponse(string xml)
		{
			var x = XDocument.Parse(xml);
			var t = x.Descendants("Table1").First();
			var success = t.Element("SUCCESS");
			if (success != null && success.Value.ToLower() == "false")
			{
				var message = t.Element("MESSAGE").Value;
				throw new Exception(message);
			}
			return t;
		}
		public void storeVault(int PeopleId,
			string semievery, int? day1, int? day2, int? everyn, string period,
			DateTime? startwhen, DateTime? stopwhen,
			string type, string cardnumber, string expires, string cardcode,
			string routing, string account, bool testing)
		{
			var p = Db.LoadPersonById(PeopleId);
			var rg = p.RecurringGiving();
			var wc = new WebClient();
			wc.BaseAddress = "https://www.sagepayments.net/web_services/wsVault/wsVault.asmx/";
			var coll = new NameValueCollection();
			coll["M_ID"] = login;
			coll["M_KEY"] = key;

			XElement resp = null;
			if (type == "C")
			{
				coll["CARDNUMBER"] = cardnumber;
				coll["EXPIRATION_DATE"] = expires;

				if (rg.SageCardGuid == null)
				{
					var b = wc.UploadValues("INSERT_CREDIT_CARD_DATA", "POST", coll);
					var ret = Encoding.ASCII.GetString(b);
					resp = getResponse(ret);
					rg.SageCardGuid = Guid.Parse(resp.Element("GUID").Value);
				}
				else
				{
					coll["GUID"] = rg.SageBankGuid.ToString().Replace("-", "");
					if (!cardnumber.StartsWith("X"))
					{
						var b = wc.UploadValues("UPDATE_CREDIT_CARD_DATA", "POST", coll);
						var ret = Encoding.ASCII.GetString(b);
						resp = getResponse(ret);
					}
					else
					{
						var b = wc.UploadValues("UPDATE_CREDIT_CARD_EXPIRATION_DATE", "POST", coll);
						var ret = Encoding.ASCII.GetString(b);
						resp = getResponse(ret);
					}
				}
			}
			else
			{
				coll["ROUTING_NUMBER"] = routing; // 064000020
				coll["ACCOUNT_NUMBER"] = account; // my account number
				coll["C_ACCT_TYPE"] = "DDA";

				if (rg.SageBankGuid == null)
				{
					var b = wc.UploadValues("INSERT_VIRTUAL_CHECK_DATA", "POST", coll);
					var ret = Encoding.ASCII.GetString(b);
					resp = getResponse(ret);
					rg.SageBankGuid = Guid.Parse(resp.Element("GUID").Value);
				}
				else
				{
					if (!account.StartsWith("X"))
					{
						coll["GUID"] = rg.SageBankGuid.ToString().Replace("-", "");
						var b = wc.UploadValues("UPDATE_VIRTUAL_CHECK_DATA", "POST", coll);
						var ret = Encoding.ASCII.GetString(b);
						resp = getResponse(ret);
					}
				}
			}
			rg.SemiEvery = semievery;
			rg.Day1 = day1;
			rg.Day2 = day2;
			rg.EveryN = everyn;
			rg.Period = period;
			rg.StartWhen = startwhen;
			rg.StopWhen = stopwhen;
			rg.Type = type;
			rg.MaskedAccount = Util.MaskAccount(account);
			rg.MaskedCard = Util.MaskCC(cardnumber);
			rg.Ccv = cardcode;
			rg.Expires = expires;
			rg.Testing = testing;
			rg.NextDate = rg.FindNextDate(startwhen.Value);
			Db.SubmitChanges();
		}
		public void deleteVaultData(int PeopleId)
		{
			var p = Db.LoadPersonById(PeopleId);
			var rg = p.RecurringGiving();
			var wc = new WebClient();
			wc.BaseAddress = "https://www.sagepayments.net/web_services/wsVault/wsVault.asmx/";
			var coll = new NameValueCollection();
			coll["M_ID"] = login; // 779966396962
			coll["M_KEY"] = key; // T7N8I1M1F7L9

			XElement resp = null;

			if (rg.SageCardGuid.HasValue)
			{
				coll["GUID"] = rg.SageCardGuid.ToString().Replace("-", "");
				var b = wc.UploadValues("DELETE_DATA", "POST", coll);
				var ret = Encoding.ASCII.GetString(b);
			}
			if (rg.SageBankGuid.HasValue)
			{
				coll["GUID"] = rg.SageBankGuid.ToString().Replace("-", "");
				var b = wc.UploadValues("DELETE_DATA", "POST", coll);
				var ret = Encoding.ASCII.GetString(b);
			}

			rg.SageCardGuid = null;
			rg.SageBankGuid = null;
			rg.MaskedCard = null;
			rg.MaskedAccount = null;
			rg.Ccv = null;
			Db.SubmitChanges();
		}

		public TransactionResponse voidTransactionRequest(string reference)
		{
			var wc = new WebClient();
			wc.BaseAddress = "https://www.sagepayments.net/web_services/vterm_extensions/transaction_processing.asmx/";
			var coll = new NameValueCollection();
			coll["M_ID"] = login;
			coll["M_KEY"] = key;
			coll["T_REFERENCE"] = reference;
			var b = wc.UploadValues("BANKCARD_VOID", "POST", coll);
			var ret = Encoding.ASCII.GetString(b);
			var resp = getResponse(ret);
			var tr = new TransactionResponse
			{
				Approved = resp.Element("APPROVAL_INDICATOR").Value == "A",
				AuthCode = resp.Element("CODE").Value,
				Message = resp.Element("MESSAGE").Value,
				TransactionId = resp.Element("REFERENCE").Value
			};
			return tr;
		}

		public TransactionResponse creditTransactionRequest(string reference, Decimal amt)
		{
			var wc = new WebClient();
			wc.BaseAddress = "https://www.sagepayments.net/web_services/vterm_extensions/transaction_processing.asmx/";
			var coll = new NameValueCollection();
			coll["M_ID"] = login;
			coll["M_KEY"] = key;
			coll["T_REFERENCE"] = reference;
			coll["T_AMT"] = amt.ToString("n2");

			var b = wc.UploadValues("BANKCARD_CREDIT", "POST", coll);
			var ret = Encoding.ASCII.GetString(b);
			var resp = getResponse(ret);
			var tr = new TransactionResponse
			{
				Approved = resp.Element("APPROVAL_INDICATOR").Value == "A",
				AuthCode = resp.Element("CODE").Value,
				Message = resp.Element("MESSAGE").Value,
				TransactionId = resp.Element("REFERENCE").Value
			};
			return tr;
		}
		public TransactionResponse creditCheckTransactionRequest(string reference, Decimal amt)
		{
			var wc = new WebClient();
			wc.BaseAddress = "https://www.sagepayments.net/web_services/vterm_extensions/transaction_processing.asmx/";
			var coll = new NameValueCollection();
			coll["M_ID"] = login;
			coll["M_KEY"] = key;
			coll["T_REFERENCE"] = reference;
			coll["T_AMT"] = amt.ToString("n2");

			var b = wc.UploadValues("VIRTUAL_CHECK_CREDIT_BY_REFERENCE", "POST", coll);
			var ret = Encoding.ASCII.GetString(b);
			var resp = getResponse(ret);
			var tr = new TransactionResponse
			{
				Approved = resp.Element("APPROVAL_INDICATOR").Value == "A",
				AuthCode = resp.Element("CODE").Value,
				Message = resp.Element("MESSAGE").Value,
				TransactionId = resp.Element("REFERENCE").Value
			};
			return tr;
		}

		public TransactionResponse createTransactionRequest(int PeopleId, decimal amt,
			string cardnumber, string expires, string description, int tranid, string cardcode,
			string email, string first, string last,
			string addr, string city, string state, string zip, string phone)
		{
			var wc = new WebClient();
			wc.BaseAddress = "https://www.sagepayments.net/web_services/vterm_extensions/transaction_processing.asmx/";
			var coll = new NameValueCollection();
			coll["M_ID"] = login;
			coll["M_KEY"] = key;
			coll["T_AMT"] = amt.ToString("n2");
			coll["C_NAME"] = first + " " + last;
			coll["C_ADDRESS"] = addr;
			coll["C_CITY"] = city;
			coll["C_STATE"] = state;
			coll["C_ZIP"] = zip;
			coll["C_COUNTRY"] = "";
			coll["C_EMAIL"] = email;
			coll["C_CARDNUMBER"] = cardnumber;
			coll["C_EXP"] = expires;
			coll["C_CVV"] = cardcode;
			coll["T_CUSTOMER_NUMBER"] = PeopleId.ToString();
			coll["T_ORDERNUM"] = tranid.ToString();
			coll["C_TELEPHONE"] = phone;
			AddShipping(coll);
			
			var b = wc.UploadValues("BANKCARD_SALE", "POST", coll);
			var ret = Encoding.ASCII.GetString(b);
			var resp = getResponse(ret);
			var tr = new TransactionResponse
			{
				Approved = resp.Element("APPROVAL_INDICATOR").Value == "A",
				AuthCode = resp.Element("CODE").Value,
				Message = resp.Element("MESSAGE").Value,
				TransactionId = resp.Element("REFERENCE").Value
			};
			return tr;
		}
		public TransactionResponse createCheckTransactionRequest(int PeopleId, decimal amt,
			string routing, string acct, string description, int tranid,
			string email, string first, string last,
			string addr, string city, string state, string zip, string phone)
		{
			var wc = new WebClient();
			wc.BaseAddress = "https://www.sagepayments.net/web_services/vterm_extensions/transaction_processing.asmx/";
			var coll = new NameValueCollection();
			coll["M_ID"] = login;
			coll["M_KEY"] = key;
			coll["C_ORIGINATOR_ID"] = ""; // use default
			coll["C_RTE"] = routing;
			coll["C_ACCT"] = acct;
			coll["C_ACCT_TYPE"] = "DDA";
			coll["T_AMT"] = amt.ToString("n2");
			coll["C_NAME"] = first + " " + last;
			coll["C_ADDRESS"] = addr;
			coll["C_CITY"] = city;
			coll["C_STATE"] = state;
			coll["C_ZIP"] = zip;
			coll["C_COUNTRY"] = "";
			coll["C_EMAIL"] = email;
			coll["T_CUSTOMER_NUMBER"] = PeopleId.ToString();
			coll["T_ORDERNUM"] = tranid.ToString();
			coll["C_TELEPHONE"] = phone;
			AddShipping(coll);
			
			var b = wc.UploadValues("VIRTUAL_CHECK_PPD_SALE", "POST", coll);
			var ret = Encoding.ASCII.GetString(b);
			var resp = getResponse(ret);
			var tr = new TransactionResponse
			{
				Approved = resp.Element("APPROVAL_INDICATOR").Value == "A",
				AuthCode = resp.Element("CODE").Value,
				Message = resp.Element("MESSAGE").Value,
				TransactionId = resp.Element("REFERENCE").Value
			};
			return tr;
		}
		public TransactionResponse createVaultTransactionRequest(int PeopleId, decimal amt, string description, int tranid)
		{
			var p = Db.LoadPersonById(PeopleId);
			var rg = p.RecurringGivings.First();

			XElement resp = null;
			if (rg.Type == "C")
			{
				var wc = new WebClient();
				wc.BaseAddress = "https://www.sagepayments.net/web_services/wsVault/wsVaultBankcard.asmx/";
				var coll = new NameValueCollection();

				coll["M_ID"] = login;
				coll["M_KEY"] = key;
				var guid = rg.SageCardGuid.ToString().Replace("-", "");
				coll["GUID"] = guid;
				coll["C_NAME"] = p.FirstName + " " + p.LastName;
				coll["C_ADDRESS"] = p.PrimaryAddress;
				coll["C_CITY"] = p.PrimaryCity;
				coll["C_STATE"] = p.PrimaryState;
				coll["C_ZIP"] = p.PrimaryZip;
				coll["C_COUNTRY"] = p.PrimaryCountry;
				coll["C_EMAIL"] = p.EmailAddress;
				coll["T_AMT"] = amt.ToString("n2");
				coll["T_ORDERNUM"] = tranid.ToString();
				coll["C_TELEPHONE"] = p.HomePhone;
				coll["T_CUSTOMER_NUMBER"] = p.HomePhone;
				coll["C_CVV"] = rg.Ccv;
				AddShipping(coll);

				var b = wc.UploadValues("VAULT_BANKCARD_SALE_CVV", "POST", coll);
				var ret = Encoding.ASCII.GetString(b);
				resp = getResponse(ret);
			}
			else
			{
				var wc = new WebClient();
				wc.BaseAddress = "https://www.sagepayments.net/web_services/wsVault/wsVaultVirtualCheck.asmx/";
				var coll = new NameValueCollection();

				coll["M_ID"] = login;
				coll["M_KEY"] = key;
				var guid = rg.SageBankGuid.ToString().Replace("-", "");
				coll["GUID"] = guid; 
				coll["C_ORIGINATOR_ID"] = Db.Setting("SageOriginatorId", ""); // 1031360711, 1031412710
				coll["C_FIRST_NAME"] = p.FirstName;
				var mi = (p.MiddleName ?? " ").FirstOrDefault().ToString().Trim();
				coll["C_MIDDLE_INITIAL"] = mi;
				coll["C_LAST_NAME"] = p.LastName;
				coll["C_SUFFIX"] = p.SuffixCode;
				coll["C_ADDRESS"] = p.PrimaryAddress;
				coll["C_CITY"] = p.PrimaryCity;
				coll["C_STATE"] = p.PrimaryState;
				coll["C_ZIP"] = p.PrimaryZip;
				coll["C_COUNTRY"] = p.PrimaryCountry;
				coll["C_EMAIL"] = p.EmailAddress;
				coll["T_AMT"] = amt.ToString("n2");
				coll["T_ORDERNUM"] = tranid.ToString();
				coll["C_TELEPHONE"] = p.HomePhone;
				AddShipping(coll);

				var b = wc.UploadValues("VIRTUAL_CHECK_PPD_SALE", "POST", coll);
				var ret = Encoding.ASCII.GetString(b);
				resp = getResponse(ret);
			}
			var tr = new TransactionResponse
			{
				Approved = resp.Element("APPROVAL_INDICATOR").Value == "A",
				AuthCode = resp.Element("CODE").Value,
				Message = resp.Element("MESSAGE").Value,
				TransactionId = resp.Element("REFERENCE").Value
			};
			return tr;
		}
		private void AddShipping(NameValueCollection coll)
		{
			// these not needed
			coll["T_SHIPPING"] = "";
			coll["T_TAX"] = "";
			coll["C_FAX"] = "";
			coll["C_SHIP_NAME"] = "";
			coll["C_SHIP_CITY"] = "";
			coll["C_SHIP_ADDRESS"] = "";
			coll["C_SHIP_ZIP"] = "";
			coll["C_SHIP_STATE"] = "";
			coll["C_SHIP_COUNTRY"] = "";
		}
	}
}