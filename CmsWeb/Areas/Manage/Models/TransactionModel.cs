using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Data.Linq;
using System.Web;
using CmsData;
using UtilityExtensions;
using System.Web.Mvc;
using System.Text;
using System.Net.Mail;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace CmsWeb.Models
{
    public class TransactionModel : CmsData.Transaction
    {
        public string Tip
        {
            get
            {
                return "".Fmt(this.TransactionId, this.Url, this.Message, this.AuthCode, this.Participants, this.Address, this.City, this.State, this.Zip);
            }
        }
    }
}
