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
        public int? peopleid { get; set; }
        public bool testing { get; set; }

        public decimal Amount { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string TransactionId { get; set; }
        public string PostbackURL { get; set; }
        public string OrgID
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
        public string OrgAccountID
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

        public string NameOnAccount { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Misc1 { get; set; }
        public string Misc2 { get; set; }
        public string Misc3 { get; set; }
        public string Misc4 { get; set; }

    }
}
