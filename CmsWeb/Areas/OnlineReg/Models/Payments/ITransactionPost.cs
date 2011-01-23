using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CmsWeb.Areas.OnlineReg.Models.Payments
{
    interface ITransactionPost
    {
        TransactionResponse PostTransaction(
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
            bool testing);
    }
}
