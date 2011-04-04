using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UtilityExtensions;
using System.Text;
using System.Net;
using CmsData;
using System.IO;

namespace CmsWeb.Areas.OnlineReg.Models.Payments
{
    public class Sage : ITransactionPost
    {
        //private TransactionResponse SaleTransaction(
        //    string card,
        //    string ccv,
        //    string expdate,
        //    decimal amt,
        //    int tranid,
        //    int PeopleId,
        //    string first,
        //    string last,
        //    string addr,
        //    string city,
        //    string state,
        //    string zip,
        //    bool testing)
        //{
        //    return null;
        //}
        public TransactionResponse PostTransaction(
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