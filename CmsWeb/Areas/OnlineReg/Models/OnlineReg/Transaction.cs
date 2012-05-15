using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using CmsData;
using System.Web.Mvc;
using System.Text;
using System.Configuration;
using UtilityExtensions;
using System.Data.Linq.SqlClient;
using CMSPresenter;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Collections;
using System.Runtime.Serialization;
using System.Net;
using System.IO;

namespace CmsWeb.Models
{
    public partial class OnlineRegModel
    {
        public static string GetTransactionGateway()
        {
            return DbUtil.Db.Setting("TransactionGateway", "ServiceU");
        }
        public decimal Amount()
        {
            var amt = List.Sum(p => p.AmountToPay());
            var max = List.Max(p => p.org != null ? p.setting.MaximumFee ?? 0 : 0);
            if (max == 0)
                return amt;
            var totalother = List.Sum(p => p.TotalOther());
            if ((amt - totalother) > max)
                amt = max + totalother;
            return amt;
        }
        public decimal TotalAmountDue()
        {
            // there is a bug here I need to figure out
            // max will not work correctly when there is a deposit.
            return List.Sum(p => p.AmountDue());
        }
        public string NameOnAccount
        {
            get
            {
                var p = List[0];
                if (user != null)
                    return user.Name;
                if (p.org != null && p.setting.AskParents == true)
                    return p.fname.HasValue() ? p.fname : p.mname;
                return p.first + " " + p.last;
            }
        }
        public static TransactionResponse PostTransaction(
            string card, 
            string ccv, 
            string expdate, 
            decimal amt, 
            int tranid, 
            string description,
            int PeopleId, 
            string email,
            string first, 
            string last, 
            string addr, 
            string city, 
            string state, 
            string zip, 
            bool testing)
        {
            string url = "https://secure.authorize.net/gateway/transact.dll";
            if (testing)
                url = "https://test.authorize.net/gateway/transact.dll";

            
            var p = new Dictionary<string, string>();
            p["x_delim_data"] = "TRUE";
            p["x_delim_char"] = "|";
            p["x_relay_response"] = "FALSE";
            p["x_type"] = "AUTH_CAPTURE";
            p["x_method"] = "CC";

            if (testing)
            {
                p["x_login"] = "9t8Pqzs4CW3S";
                p["x_tran_key"] = "9j33v58nuZB865WR";
            }
            else
            {
                p["x_login"] = DbUtil.Db.Setting("x_login", "");
                p["x_tran_key"] = DbUtil.Db.Setting("x_tran_key", "");
            }

            p["x_card_num"] = card;
            p["x_card_code"] = ccv;
            p["x_exp_date"] = expdate;
            p["x_amount"] = amt.ToString();
            p["x_description"] = description;
            p["x_invoice_num"] = tranid.ToString();
            p["x_cust_id"] = PeopleId.ToString();
            p["x_first_name"] = first;
            p["x_last_name"] = last;
            p["x_address"] = addr;
            p["x_city"] = city;
            p["x_state"] = state;
            p["x_zip"] = zip;
            p["x_email"] = email;

            var sb = new StringBuilder();
            foreach (var kv in p)
                sb.AppendFormat("{0}={1}&", kv.Key, HttpUtility.UrlEncode(kv.Value));
            sb.Length = sb.Length - 1;

            var wc = new WebClient();
            var req = WebRequest.Create(url);
            req.Method = "POST";
            req.ContentLength = sb.Length;
            req.ContentType = "application/x-www-form-urlencoded";

            var sw = new StreamWriter(req.GetRequestStream());
            sw.Write(sb.ToString());
            sw.Close();

            var r = req.GetResponse();
            using (var rs = new StreamReader(r.GetResponseStream()))
            {
                var resp = rs.ReadToEnd();
                rs.Close();
                var a = resp.Split('|');
                return new TransactionResponse
                {
                    Approved = a[0] == "1",
                    Message = a[3],
                    AuthCode = a[4],
                    TransactionId = a[6]
                };
            }
        }
        public static TransactionResponse PostECheck(
            string routing, string account,
            decimal amt, int tranid, string description,
            int PeopleId, string first, string last,
            string addr, string city, string state, string zip,
            bool testing)
        {
            string url = "https://secure.authorize.net/gateway/transact.dll";
            if (testing)
                url = "https://test.authorize.net/gateway/transact.dll";

            var p = new Dictionary<string, string>();
            p["x_delim_data"] = "TRUE";
            p["x_delim_char"] = "|";
            p["x_relay_response"] = "FALSE";
            p["x_method"] = "ECHECK";

            if (testing)
            {
                p["x_login"] = "9t8Pqzs4CW3S";
                p["x_tran_key"] = "9j33v58nuZB865WR";
            }
            else
            {
                p["x_login"] = DbUtil.Db.Setting("x_login", "");
                p["x_tran_key"] = DbUtil.Db.Setting("x_tran_key", "");
            }

            p["x_bank_aba_code"] = routing;
            p["x_bank_acct_num"] = account;
            p["x_bank_acct_type"] = "CHECKING";
            p["x_bank_acct_name"] = first + " " + last;
            p["x_echeck_type"] = "WEB";
            p["x_recurring_billing"] = "FALSE";
            p["x_amount"] = amt.ToString();

            p["x_description"] = description;
            p["x_invoice_num"] = tranid.ToString();
            p["x_cust_id"] = PeopleId.ToString();
            p["x_last_name"] = last;
            p["x_address"] = addr;
            p["x_city"] = city;
            p["x_state"] = state;
            p["x_zip"] = zip;

            var sb = new StringBuilder();
            foreach (var kv in p)
                sb.AppendFormat("{0}={1}&", kv.Key, HttpUtility.UrlEncode(kv.Value));
            sb.Length = sb.Length - 1;

            var wc = new WebClient();
            var req = WebRequest.Create(url);
            req.Method = "POST";
            req.ContentLength = sb.Length;
            req.ContentType = "application/x-www-form-urlencoded";

            var sw = new StreamWriter(req.GetRequestStream());
            sw.Write(sb.ToString());
            sw.Close();

            var r = req.GetResponse();
            using (var rs = new StreamReader(r.GetResponseStream()))
            {
                var resp = rs.ReadToEnd();
                rs.Close();
                var a = resp.Split('|');
                return new TransactionResponse
                {
                    Approved = a[0] == "1",
                    Message = a[3],
                    AuthCode = a[4],
                    TransactionId = a[6]
                };
            }
        }
        public static TransactionResponse PostTransactionSage(
            string card, string ccv, string expdate, 
            decimal amt, int tranid, string description,
            int PeopleId, string email, string first, string last,
            string addr, string city, string state, string zip, string phone,
            bool testing)
        {
        	var t = new SagePayments(DbUtil.Db, testing);
			var resp = t.createTransactionRequest(PeopleId, amt, card, expdate, description, tranid, ccv,
				email, first, last, addr, city, state, zip, phone);
        	return resp;
        }
        public static TransactionResponse PostVirtualCheckTransactionSage(
            string routing, string acct,
            decimal amt, int tranid, string description,
            int PeopleId, string email, string first, string last,
            string addr, string city, string state, string zip, string phone,
            bool testing)
        {
        	var t = new SagePayments(DbUtil.Db, testing);
			var resp = t.createCheckTransactionRequest(PeopleId, amt, routing, acct, description, tranid,
				email, first, last, addr, city, state, zip, phone);
        	return resp;
        }
    }
}
