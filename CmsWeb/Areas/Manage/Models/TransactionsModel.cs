using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using UtilityExtensions;

namespace CmsWeb.Models
{
    public class TransactionsModel
    {
        public string description { get; set; }
        public string name { get; set; }
        public string Submit { get; set; }
        public decimal? gtamount { get; set; }
        public decimal? ltamount { get; set; }
        public DateTime? startdt { get; set; }
        public DateTime? enddt { get; set; }
        public bool testtransactions { get; set; }
        public bool apprtransactions { get; set; }
        public bool nocoupons { get; set; }
        public PagerModel2 Pager { get; set; }
        int? _count;
        public int Count()
        {
            if (!_count.HasValue)
                _count = FetchTransactions().Count();
            return _count.Value;
        }
        public bool finance { get; set; }
        public bool admin { get; set; }
        public TransactionsModel()
        {
            Pager = new PagerModel2(Count);
            Pager.Sort = "Id";
            Pager.Direction = "desc";
            finance = HttpContext.Current.User.IsInRole("Finance");
            admin = HttpContext.Current.User.IsInRole("Admin");
        }
        public IEnumerable<Transaction> Transactions()
        {
            var q0 = ApplySort();
            q0 = q0.Skip(Pager.StartRow).Take(Pager.PageSize);
            return q0;
        }

    	public class TotalTransaction
    	{
    		public decimal Amt { get; set; }
    		public decimal Amtdue { get; set; }
    		public decimal Donate { get; set; }
    	}

    	public TotalTransaction TotalTransactions()
        {
            var q0 = ApplySort();
			var q = from t in q0
					group t by 1 into g
					select new TotalTransaction()
					{
						Amt = g.Sum(tt => tt.Amt ?? 0),
						Amtdue = g.Sum(tt => tt.Amtdue ?? 0),
						Donate = g.Sum(tt => tt.Donate ?? 0),
					};
			return q.FirstOrDefault();
        }

        private IQueryable<Transaction> _transactions;
        private IQueryable<Transaction> FetchTransactions()
        {
            _transactions
               = from t in DbUtil.Db.Transactions
                 let donate = t.Donate ?? 0
                 where t.Amt > gtamount || gtamount == null
                 where t.Amt <= ltamount || ltamount == null
                 where t.TransactionDate >= startdt || startdt == null
                 where description == null || t.Description.Contains(description)
                 where name == null || t.Name.Contains(name)
                 where (t.Testing ?? false) == testtransactions
				 where apprtransactions == (t.Moneytran == true) || !apprtransactions
				 where (nocoupons && !t.TransactionId.Contains("Coupon")) || !nocoupons
                 where (t.Financeonly ?? false) == false || finance
                 select t;
            if (!HttpContext.Current.User.IsInRole("Finance"))
                _transactions = _transactions.Where(tt => (tt.Financeonly ?? false) == false);
            var edt = enddt;
            if (!edt.HasValue && startdt.HasValue)
                 edt = startdt.Value;
            if (edt.HasValue)
            {
                edt = edt.Value.AddHours(24);
                _transactions = _transactions.Where(t => t.TransactionDate <= edt);
            }
            return _transactions;
        }
        public IQueryable<Transaction> ApplySort()
        {
            var q = FetchTransactions();
            if (Pager.Direction == "asc")
                switch (Pager.Sort)
                {
                    case "Id":
                        q = from t in q
                            orderby (t.OriginalId ?? t.Id), t.TransactionDate 
                            select t;
                        break;
                    case "Tran Id":
                        q = from t in q
                            orderby t.TransactionId
                            select t;
                        break;
                    case "Appr":
                        q = from t in q
                            orderby t.Approved, t.TransactionDate descending
                            select t;
                        break;
                    case "Date":
                        q = from t in q
                            orderby t.TransactionDate
                            select t;
                        break;
                    case "Description":
                        q = from t in q
                            orderby t.Description, t.TransactionDate descending
                            select t;
                        break;
                    case "Name":
                        q = from t in q
                            orderby t.Name, t.TransactionDate descending
                            select t;
                        break;
                    case "Amount":
                        q = from t in q
                            orderby t.Amt, t.TransactionDate descending
                            select t;
                        break;
                    case "Due":
                        q = from t in q
                            orderby t.Amtdue, t.TransactionDate descending
                            select t;
                        break;
                }
            else
                switch (Pager.Sort)
                {
                    case "Id":
                        q = from t in q
                            orderby (t.OriginalId ?? t.Id) descending, t.TransactionDate descending 
                            select t;
                        break;
                    case "Tran Id":
                        q = from t in q
                            orderby t.TransactionId descending
                            select t;
                        break;
                    case "Appr":
                        q = from t in q
                            orderby t.Approved descending, t.TransactionDate
                            select t;
                        break;
                    case "Date":
                        q = from t in q
                            orderby t.TransactionDate descending
                            select t;
                        break;
                    case "Description":
                        q = from t in q
                            orderby t.Description descending, t.TransactionDate
                            select t;
                        break;
                    case "Name":
                        q = from t in q
                            orderby t.Name descending, t.TransactionDate
                            select t;
                        break;
                    case "Amount":
                        q = from t in q
                            orderby t.Amt descending, t.TransactionDate
                            select t;
                        break;
                    case "Due":
                        q = from t in q
                            orderby t.Amtdue descending, t.TransactionDate
                            select t;
                        break;
                }

            return q;
        }
        public IQueryable ExportTransactions()
        {
            var q
               = from t in DbUtil.Db.Transactions
                 where t.Amt > gtamount || gtamount == null
                 where t.Amt <= ltamount || ltamount == null
                 where t.TransactionDate >= startdt || startdt == null
                 where description == null || t.Description.Contains(description)
                 where name == null || t.Name.Contains(name)
                 where (t.Testing ?? false) == testtransactions
				 where apprtransactions == (t.Moneytran == true) || !apprtransactions
				 where (nocoupons && !t.TransactionId.Contains("Coupon")) || !nocoupons
                 where (t.Financeonly ?? false) == false || finance
                 select t;

            var edt = enddt;
            if (!edt.HasValue && startdt.HasValue)
                 edt = startdt.Value.AddHours(24);
            if (edt.HasValue)
                q = q.Where(t => t.TransactionDate < edt);

            var q2 = from t in q
                     select new
                 {
                     t.Id,
                     t.TransactionId,
                     t.Approved,
                     TranDate = t.TransactionDate.FormatDate(),
                     RegAmt = (t.Amt ?? 0) - (t.Donate ?? 0),
                     Donate = t.Donate ?? 0,
                     TotalAmt = t.Amt ?? 0,
                     Amtdue = t.Amtdue ?? 0,
                     t.Description,
                     t.Message,
                     t.Name,
                     t.Address,
                     t.City,
                     t.State,
                     t.Zip,
                     t.Fund
                 };
            return q2;
        }
    }
}
