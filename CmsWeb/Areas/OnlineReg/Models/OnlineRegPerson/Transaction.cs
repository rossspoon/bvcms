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
            if (amt == 0 && org.AskTickets == true)
                amt = (org.Fee ?? 0) * (ntickets ?? 0);
            if (amt == 0 && org.AskOptions.HasValue())
            {
                var q = from o in org.AskOptions.Split(',')
                        let a = o.Split('=')
                        where option == a[0].Trim() && a.Length > 1
                        select decimal.Parse(a[1]);
                if (q.Count() > 0)
                    amt = q.First();
            }
            if (amt == 0 && org.MenuItems.HasValue())
                amt = MenuItemsChosen().Sum(m => m.number * m.amt);
            if (amt == 0 && org.AgeFee.HasValue())
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
            var countorgs = 0;
            if (amt == 0 && org.OrgMemberFees.HasValue())
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
            if (amt == 0 && countorgs == 0)
                amt = org.Fee ?? 0;
            if (org.LastDayBeforeExtra.HasValue && org.ExtraFee.HasValue)
                if (Util.Now > org.LastDayBeforeExtra.Value.AddHours(24))
                    amt += org.ExtraFee.Value;
            amt += TotalOther();
            return amt;
        }
        public decimal TotalOther()
        {
            if (shirtsize != "lastyear" && org.ShirtFee.HasValue)
                return org.ShirtFee.Value;
            return 0;
        }
    }
}