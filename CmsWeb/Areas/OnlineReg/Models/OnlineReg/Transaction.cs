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

namespace CmsWeb.Models
{
    public partial class OnlineRegModel
    {
        public decimal Amount()
        {
            var amt = List.Sum(p => p.AmountToPay());
            var max = List.Max(p => p.org != null ? p.org.MaximumFee ?? 0 : 0);
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
                if (p.org != null && p.org.AskParents == true)
                    return p.fname.HasValue() ? p.fname : p.mname;
                return p.first + " " + p.last;
            }
        }
    }
}
