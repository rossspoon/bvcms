using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Net;
using CmsData;

namespace CmsWeb.Areas.OnlineReg.Models.Payments
{
    public class AuthorizeNet : ITransactionPost
    {
        public TransactionResponse PostTransaction(
            string card, 
            string ccv, 
            string expdate, 
            decimal amt, 
            int tranid, 
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
            p["x_description"] = tranid.ToString(); // should goto invoice number, this s/b description
            p["x_cust_id"] = PeopleId.ToString(); // not passing?
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
    }
}