using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CmsWeb.Models
{
    public class PaymentForm
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string CreditCard { get; set; }
        public string Expires { get; set; }
        public string CCV { get; set; }
        public int DatumId { get; set; }
        public decimal Amt { get; set; }
        public string Url { get; set; }
    }
}