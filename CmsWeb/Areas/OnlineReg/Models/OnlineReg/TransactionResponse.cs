using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CmsWeb.Models
{
    public class TransactionResponse
    {
        public bool Approved { get; set; }
        public string Message { get; set; }
        public string AuthCode { get; set; }
        public string TransactionId { get; set; }
    }
}