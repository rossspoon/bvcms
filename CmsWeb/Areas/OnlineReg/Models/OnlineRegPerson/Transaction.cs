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
            if (paydeposit == true && setting.Deposit.HasValue && setting.Deposit > 0)
                return setting.Deposit.Value;
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
            if (setting.AskTickets == true)
                // fee based on number of tickets
                amt = (setting.Fee ?? 0) * (ntickets ?? 0);
            if (setting.SuggestedFee == true)
                amt = suggestedfee ?? 0;
            if (setting.AgeGroups != null) // fee based on age
            {
                var q = from o in setting.AgeGroups
                        where age >= o.StartAge
                        where age <= o.EndAge || o.EndAge == 0
                        select o.Fee ?? 0;
                if (q.Count() > 0)
                    amt = q.First();
            }
            if (setting.OrgFees != null)
                // fee based on being in an organization
            {
                var q = from o in setting.OrgFees
                        where person != null
                        && person.OrganizationMembers.Any(om => om.OrganizationId == o.OrgId)
                        select o.Fee ?? 0;
                countorgs = q.Count();
                if (countorgs > 0)
                    amt = q.First();
            }
            // just use the simple fee if nothing else has been used yet.
            if (amt == 0 && countorgs == 0 && setting.SuggestedFee == false)
                amt = setting.Fee ?? 0;

            amt += TotalOther();
            return amt;
        }
        public decimal TotalOther()
        {
            decimal amt = 0;
            if (setting.MenuItems.Count > 0)
                amt += MenuItemsChosen().Sum(m => m.number * m.amt);
            if (shirtsize != "lastyear" && setting.ShirtFee.HasValue)
                amt += setting.ShirtFee.Value;
            if (org.LastDayBeforeExtra.HasValue && setting.ExtraFee.HasValue)
                if (Util.Now > org.LastDayBeforeExtra.Value.AddHours(24))
                    amt += setting.ExtraFee.Value;
            if (setting.Dropdown1.Count > 0)
            {
                var q = from o in setting.Dropdown1
                        where option == o.SmallGroup
                        select o.Fee ?? 0;
                if (q.Count() > 0)
                    amt += q.First();
            }
            if (setting.Dropdown2.Count > 0)
            {
                var q = from o in setting.Dropdown2
                        where option2 == o.SmallGroup
                        select o.Fee ?? 0;
                if (q.Count() > 0)
                    amt += q.First();
            }
            if (setting.Dropdown3.Count > 0)
            {
                var q = from o in setting.Dropdown3
                        where option3 == o.SmallGroup
                        select o.Fee ?? 0;
                if (q.Count() > 0)
                    amt += q.First();
            }
            if (setting.Checkboxes.Any(vv => vv.Fee > 0))
                amt += CheckboxItemsChosen().Sum(c => c.Fee.Value);
            if (setting.Checkboxes2.Any(vv => vv.Fee > 0))
                amt += Checkbox2ItemsChosen().Sum(c => c.Fee.Value);
            return amt;
        }
    }
}