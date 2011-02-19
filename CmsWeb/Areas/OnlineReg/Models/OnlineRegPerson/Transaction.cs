using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using UtilityExtensions;

namespace CmsWeb.Models
{
    public partial class OnlineRegPersonModel
    {
        public decimal AmountToPay()
        {
            if (paydeposit == true && org.Deposit.HasValue && org.Deposit > 0)
                return org.Deposit.Value;
            return TotalAmount();
        }
        public decimal AmountDue()
        {
            return TotalAmount() - AmountToPay();
        }
        public decimal TotalAmount()
        {
            if (org == null)
                return 0;
            decimal amt = 0;
            var countorgs = 0;

            // compute special fees first, in order of precedence, lowest to highest
            if (org.AskTickets == true)
                // fee based on number of tickets
                amt = (org.Fee ?? 0) * (ntickets ?? 0);
            if (org.AgeFee.HasValue())
                // fee based on age
            {
                var q = from o in org.AgeFee.Split(',')
                        let b = o.Split('=')
                        let a = b[0].Split('-')
                        where b.Length > 1
                        where age >= a[0].ToInt()
                        where a.Length > 1 && age <= a[1].ToInt()
                        select decimal.Parse(b[1]);
                if (q.Count() > 0)
                    amt = q.First();
            }
            if (org.OrgMemberFees.HasValue())
                // fee based on being in an organization
            {
                var q = from o in org.OrgMemberFees.Split(',')
                        let b = o.Split('=')
                        where b.Length > 1
                        where person != null && person.OrganizationMembers.Any(om => om.OrganizationId.ToString() == b[0])
                        select decimal.Parse(b[1]);
                countorgs = q.Count();
                if (countorgs > 0)
                    amt = q.First();
            }
            // just use the simple fee if nothing else has been used yet.
            if (amt == 0 && countorgs == 0)
                amt = org.Fee ?? 0;

            amt += TotalOther();
            return amt;
        }
        public decimal TotalOther()
        {
            decimal amt = 0;
            if (org.MenuItems.HasValue())
                amt += MenuItemsChosen().Sum(m => m.number * m.amt);
            if (shirtsize != "lastyear" && org.ShirtFee.HasValue)
                amt += org.ShirtFee.Value;
            if (org.LastDayBeforeExtra.HasValue && org.ExtraFee.HasValue)
                if (Util.Now > org.LastDayBeforeExtra.Value.AddHours(24))
                    amt += org.ExtraFee.Value;
            if (org.AskOptions.HasValue())
            {
                var q = from o in org.AskOptions.Split(',')
                        let a = o.Split('=')
                        where option == a[0].Trim() && a.Length > 1
                        select decimal.Parse(a[1]);
                if (q.Count() > 0)
                    amt += q.First();
            }
            return amt;
        }
    }
}