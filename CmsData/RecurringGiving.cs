using System;
using System.Web;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using UtilityExtensions;

namespace CmsData
{
    public partial class RecurringGiving
    {
        public DateTime FindNextDate(DateTime ndt)
        {
            if (SemiEvery == "S")
            {
                var dt1 = new DateTime(ndt.Year, ndt.Month, Day1.Value);
                var dt2 = new DateTime(ndt.Year, ndt.Month,
                        Math.Min(DateTime.DaysInMonth(ndt.Year, ndt.Month), Day2.Value));
                if (ndt <= dt1)
                    return dt1;
                if (ndt <= dt2)
                    return dt2;
                return dt1.AddMonths(1);
            }
            else
            {
                var dt = StartWhen.Value;
                var n = 1;
                if (Period == "W")
                    while (ndt > dt)
                        dt = StartWhen.Value.AddDays(EveryN.Value * 7 * n++);
                else if (Period == "M")
                    while (ndt > dt)
                        dt = StartWhen.Value.AddMonths(EveryN.Value * n++);
                return dt;
            }
        }
        public void DoGiving(CMSDataContext Db)
        {
            var gateway = Db.Setting("TransactionGateway", "");
            AuthorizeNet anet = null;
            SagePayments sage = null;
            if (gateway == "AuthorizeNet")
                anet = new AuthorizeNet(Db, Testing ?? true);
            else if (gateway == "SagePayments")
                sage = new SagePayments(Db, Testing ?? true);
            else
                return;

            TransactionResponse ret = null;
            var total = (from a in RecurringAmounts
                         where a.ContributionFund.FundStatusId == 1
                         select a.Amt).Sum();

            if (!total.HasValue || total == 0)
                return;

            if (gateway == "AuthorizeNet")
                ret = anet.createCustomerProfileTransactionRequest(PeopleId, total ?? 0, "Recurring Giving", 0, Ccv);
            else
                ret = sage.createVaultTransactionRequest(PeopleId, total ?? 0, "Recurring Giving", 0);
            var t = new Transaction
            {
                TransactionDate = DateTime.Now,
                TransactionId = ret.TransactionId,
                Approved = ret.Approved,
                Message = ret.Message,
                AuthCode = ret.AuthCode,
                Name = this.Person.Name,
                Amt = total,
                Donate = total,
                Description = "Recurring Giving",
                Testing = Testing,
                TransactionGateway = gateway,
                Financeonly = true
            };
            t.TransactionPeople.Add(new TransactionPerson { PeopleId = PeopleId, Amt = total });
            Db.Transactions.InsertOnSubmit(t);

            if (ret.Approved)
            {
                foreach (var a in RecurringAmounts)
                {
                    if (a.ContributionFund.FundStatusId == 1)
                        Person.PostUnattendedContribution(Db,
                            a.Amt ?? 0,
                            a.FundId,
                            "Recurring Giving", pledge: false);
                }
                NextDate = FindNextDate(DateTime.Today.AddDays(1));
            }
            Db.SubmitChanges();
        }
        public static void DoAllGiving(CMSDataContext Db)
        {
            var gateway = Db.Setting("TransactionGateway", "");
            if (gateway.HasValue())
            {
                var rgq = from rg in Db.RecurringGivings
                          where rg.NextDate == DateTime.Today
                          select new
                          {
                              rg,
                              rg.Person,
                              rg.RecurringAmounts,
                          };
                foreach (var i in rgq)
                    i.rg.DoGiving(Db);
            }
        }
    }
}