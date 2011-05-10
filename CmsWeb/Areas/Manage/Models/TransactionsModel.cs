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
        public PagerModel2 Pager { get; set; }
        int? _count;
        public int Count()
        {
            if (!_count.HasValue)
                _count = FetchTransactions().Count();
            return _count.Value;
        }
        public TransactionsModel()
        {
            Pager = new PagerModel2(Count);
            Pager.Sort = "Id";
            Pager.Direction = "desc";
        }
        public IEnumerable<Transaction> Transactions()
        {
            var q0 = ApplySort();
            q0 = q0.Skip(Pager.StartRow).Take(Pager.PageSize);
            return q0;
        }

        private IQueryable<Transaction> _transactions;
        private IQueryable<Transaction> FetchTransactions()
        {
            _transactions
               = from t in DbUtil.Db.Transactions
                 where t.Amt > gtamount || gtamount == null
                 where t.Amt <= ltamount || ltamount == null
                 where t.TransactionDate >= startdt || startdt == null
                 where t.TransactionDate <= enddt || enddt == null
                 where description == null || t.Description.Contains(description)
                 where name == null || t.Name.Contains(name)
                 where t.Testing == testtransactions
                 select t;
            if (!enddt.HasValue && startdt.HasValue)
            {
                var edt = startdt.Value.AddHours(24);
                _transactions = _transactions.Where(t => t.TransactionDate < edt);
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
                            orderby t.Id
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
                            orderby t.Id descending
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
                 where t.Amt >= gtamount || gtamount == null
                 where t.Amt <= ltamount || ltamount == null
                 where t.TransactionDate >= startdt || startdt == null
                 where description == null || t.Description.Contains(description)
                 where name == null || t.Name.Contains(name)
                 select t;
            if (!enddt.HasValue && startdt.HasValue)
            {
                var edt = startdt.Value.AddHours(24);
                q = q.Where(t => t.TransactionDate < edt);
            }
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
