using System;
using System.Web;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using UtilityExtensions;

namespace CmsData
{
    public partial class ManagedGiving
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
        public int DoGiving(CMSDataContext Db)
        {
            var gateway = Db.Setting("TransactionGateway", "");
            AuthorizeNet anet = null;
            SagePayments sage = null;
            if (gateway == "AuthorizeNet")
                anet = new AuthorizeNet(Db, testing: false);
            else if (gateway == "Sage")
                sage = new SagePayments(Db, testing: false);
            else
                return 0;

            TransactionResponse ret = null;
            var total = (from a in RecurringAmounts
                         where a.ContributionFund.FundStatusId == 1
                         select a.Amt).Sum();

            if (!total.HasValue || total == 0)
                return 0;

            var t = new Transaction
            {
                TransactionDate = DateTime.Now,
                TransactionId = "started",
                Name = this.Person.Name,
                Amt = total,
                Donate = total,
                Description = "Recurring Giving",
                Testing = false,
                TransactionGateway = gateway,
                Financeonly = true
            };
            Db.Transactions.InsertOnSubmit(t);
			Db.SubmitChanges();

            if (gateway == "AuthorizeNet")
                ret = anet.createCustomerProfileTransactionRequest(PeopleId, total ?? 0, "Recurring Giving", t.Id);
            else
                ret = sage.createVaultTransactionRequest(PeopleId, total ?? 0, "Recurring Giving", t.Id, Type );
            t.TransactionPeople.Add(new TransactionPerson { PeopleId = PeopleId, Amt = total });

			t.Message = ret.Message;
			t.AuthCode = ret.AuthCode;
			t.Approved = ret.Approved;

            if (ret.Approved)
            {
				t.TransactionId = ret.TransactionId;
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
			return 1;
        }
        public static int DoAllGiving(CMSDataContext Db)
        {
            var gateway = Db.Setting("TransactionGateway", "");
			int count = 0;
            if (gateway.HasValue())
            {
                var rgq = from rg in Db.ManagedGivings
                          where rg.NextDate == DateTime.Today
                          select new
                          {
                              rg,
                              rg.Person,
                              rg.Person.RecurringAmounts,
                          };
                foreach (var i in rgq)
                    count += i.rg.DoGiving(Db);
            }
			return count;
        }
    }
}