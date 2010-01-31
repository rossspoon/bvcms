using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using CmsData;
using System.Web.Mvc;
using UtilityExtensions;
using System.Net.Mail;

namespace CMSWeb.Models
{
    public class PaymentModel
    {
        public bool testing { get; set; }
        public decimal amount { get; set; }
        public int? peopleid { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string TransactionId { get; set; }
        public string postbackurl { get; set; }
        public string ServiceUOrgID
        {
            get
            {
#if DEBUG
                return DbUtil.Settings("ServiceUOrgIDTest", "0");
#else
                if (testing)
                    return DbUtil.Settings("ServiceUOrgIDTest", "0");
                return DbUtil.Settings("ServiceUOrgID", "0");
#endif
            }
        }
        public string ServiceUOrgAccountID
        {
            get
            {
#if DEBUG
                return DbUtil.Settings("ServiceUOrgAccountIDTest", "0");
#else
                if (testing)
                    return DbUtil.Settings("ServiceUOrgAccountIDTest", "0");
                return DbUtil.Settings("ServiceUOrgAccountID", "0");
#endif
            }
        }

        public string address { get; set; }
        public string zip { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int orgid { get; set; }

    }
}
