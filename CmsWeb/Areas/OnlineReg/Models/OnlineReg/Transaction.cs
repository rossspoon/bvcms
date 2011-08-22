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
using AuthorizeNet;

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
        public static TransactionResponse PostTransactionCheck(
            string routing, string account, 
            decimal amt, int tranid, string description,
            int PeopleId, string first, string last, 
            string addr, string city, string state, string zip, 
            bool testing)
        {
            string x_login, x_tran_key;
            if (testing)
            {
                x_login = "9t8Pqzs4CW3S";
                x_tran_key = "9j33v58nuZB865WR";
            }
            else
            {
                x_login = DbUtil.Db.Setting("x_login", "");
                x_tran_key = DbUtil.Db.Setting("x_tran_key", "");
            }
            var req = new EcheckRequest(amt, routing, account, BankAccountType.Checking, null, null, null);
            req.AddCustomer(PeopleId.ToString(), first, last, addr, state, zip);
            var gate = new Gateway(x_login, x_tran_key, testing);
            var response = gate.Send(req);
            return new TransactionResponse
            {
                Approved = response.Approved,
                Message = response.Message,
                AuthCode = response.AuthorizationCode,
                TransactionId = response.TransactionID
            };
        }
        public static TransactionResponse PostTransaction(
            string card, string ccv, string expdate, 
            decimal amt, int tranid, string description,
            int PeopleId, string email, string first, string last, 
            string addr, string city, string state, string zip, 
            bool testing)
        {
            string x_login, x_tran_key;
            if (testing)
            {
                x_login = "9t8Pqzs4CW3S";
                x_tran_key = "9j33v58nuZB865WR";
            }
            else
            {
                x_login = DbUtil.Db.Setting("x_login", "");
                x_tran_key = DbUtil.Db.Setting("x_tran_key", "");
            }
            var req = new AuthorizationRequest(card, expdate, amt, description);
            req.AddCardCode(ccv);
            req.AddCustomer(PeopleId.ToString(), first, last, addr, state, zip);
            req.AddInvoice(tranid.ToString());
            var gate = new Gateway(x_login, x_tran_key, testing);
            var response = gate.Send(req);
            return new TransactionResponse
            {
                Approved = response.Approved,
                Message = response.Message,
                AuthCode = response.AuthorizationCode,
                TransactionId = response.TransactionID
            };
        }
        public static TransactionResponse PostTransactionSage(
            string card, string ccv, string expdate, 
            decimal amt, int tranid, string description,
            int PeopleId, string email, string first, string last,
            string addr, string city, string state, string zip,
            bool testing)
        {
            string url = "https://www.sagepayments.net/cgi-bin/eftBankcard.dll?transaction";
            var p = new Dictionary<string, string>();

            if (testing)
            {
                p["M_id"] = "434989517813";
                p["M_key"] = "G5W9J2M4B5L7";
            }
            else
            {
                p["M_id"] = DbUtil.Db.Setting("M_id", "");
                p["M_key"] = DbUtil.Db.Setting("M_key", "");
            }

            p["C_cardnumber"] = card;
            p["C_cvv"] = ccv;
            p["C_exp"] = expdate;
            p["T_amt"] = amt.ToString();
            p["T_code"] = "01"; // sale
            p["T_code"] = "05"; // credit
            p["T_customer_number"] = PeopleId.ToString();
            p["T_ordernum"] = tranid.ToString();
            p["C_name"] = first + " " + last;
            p["C_address"] = addr;
            p["C_city"] = city;
            p["C_state"] = state;
            p["C_zip"] = zip;

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
                return new TransactionResponse
                {
                    Approved = resp.Substring(1,1) == "1",
                    Message = resp.Substring(8, 32).Trim(),
                    AuthCode = resp.Substring(2, 6).Trim(),
                    TransactionId = resp.Substring(46, 10).Trim()
                };
            }
        }
    }
}
