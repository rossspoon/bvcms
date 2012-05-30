using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.Linq;
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
		public bool usebatchdates { get; set; }
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
			admin = HttpContext.Current.User.IsInRole("Admin") || HttpContext.Current.User.IsInRole("ManageTransactions");
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
			if (_transactions != null)
				return _transactions;
			_transactions
			   = from t in DbUtil.Db.Transactions
				 let donate = t.Donate ?? 0
				 where t.Amt > gtamount || gtamount == null
				 where t.Amt <= ltamount || ltamount == null
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
				edt = startdt;
			if (edt.HasValue)
				edt = edt.Value.AddHours(24);
			if (usebatchdates && startdt.HasValue)
			{
				CheckBatchDates(startdt.Value, edt.Value);
				_transactions = from t in _transactions
								where t.Batch >= startdt || startdt == null
								where t.Batch <= edt || edt == null
								where t.Moneytran == true
								select t;
			}
			else
				_transactions = from t in _transactions
								where t.TransactionDate >= startdt || startdt == null
								where t.TransactionDate <= edt || edt == null
								select t;
			return _transactions;
		}

		private void CheckBatchDates(DateTime start, DateTime end)
		{
			if (OnlineRegModel.GetTransactionGateway() != "sage")
				return;
			var sage = new SagePayments(DbUtil.Db, false);
			var bds = sage.SettledBatchSummary(start, end, true, true);
			var	batches = from batch in bds.Tables[0].AsEnumerable()
						  select new
						  {
							  date = batch["date"].ToDate().Value,
							  reference = batch["reference"].ToString(),
							  type = batch["type"].ToString()
						  };
			foreach (var batch in batches)
			{
				if (DbUtil.Db.CheckedBatches.Any(tt => tt.BatchRef == batch.reference))
					continue;
				var ds = sage.SettledBatchListing(batch.reference, batch.type);

				var items = from r in ds.Tables[0].AsEnumerable()
							select new
							{
								settled = r["settle_date"].ToDate().Value,
								tranid = r["order_number"].ToInt(),
								reference = r["reference"].ToString()
							};
				var settlelist = items.ToDictionary(ii => ii.reference, ii => ii.settled);
				var q = from t in DbUtil.Db.Transactions
						where settlelist.Keys.Contains(t.TransactionId)
						where t.Approved == true
						select t;

				foreach (var t in q)
				{
					t.Batch = batch.date;
					t.Batchref = batch.reference;
					t.Batchtyp = batch.type;
					t.Settled = settlelist[t.TransactionId];
				}
				DbUtil.Db.CheckedBatches.InsertOnSubmit(
					new CheckedBatch() 
					{ 
						BatchRef = batch.reference, 
						CheckedX = DateTime.Now 
					});
				DbUtil.Db.SubmitChanges();
			}
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
