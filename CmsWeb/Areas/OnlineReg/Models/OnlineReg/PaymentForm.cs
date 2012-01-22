using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CmsWeb.Models
{
    public class PaymentForm
    {
        public CmsData.Transaction ti { get; set; } 
        public string CreditCard { get; set; }
        public string Expires { get; set; }
        public string CCV { get; set; }
        public bool AskDonation { get; set; }
    }
}