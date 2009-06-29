/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CmsData;
using System.Data.Linq;
using System.ComponentModel;
using UtilityExtensions;
using System.Web;
using System.Collections;
using System.Diagnostics;

namespace CMSPresenter
{
	public class ExtractInfo
	{
		public string Fund { get; set; }
		public int Month { get; set; }
		public int HeaderId { get; set; }
		public DateTime ContributionDate { get; set; }
		public string FundName { get; set; }
		public string FundDept { get; set; }
		public string FundAcct { get; set; }
		public decimal Amount { get; set; }
		public DateTime PostingDate { get; set; }
	}
	public class BundleInfo
	{
		public int BundleId { get; set; }
		public DateTime? PostingDate { get; set; }
		public string HeaderType { get; set; }
		public DateTime? DepositDate { get; set; }
		public decimal TotalBundle { get; set; }
		public int? FundId { get; set; }
		public string Fund { get; set; }
		public string Status { get; set; }
	}
	public class DepositInfo
	{
		public string BundleId { get; set; }
		public decimal Total { get; set; }
		public decimal Checks { get; set; }
		public decimal Cash { get; set; }
		public decimal Coins { get; set; }
	}
	public class ContributionInfo
	{
		public int BundleId { get; set; }
		public int ContributionId { get; set; }
		public string Fund { get; set; }
		public string ContributionType { get; set; }
		public int? ContributionTypeId { get; set; }
		public int? PeopleId { get; set; }
		public string Name { get; set; }
		public DateTime? ContributionDate { get; set; }
		public decimal? ContributionAmount { get; set; }
		public string Status { get; set; }
		public int? StatusId { get; set; }
		public bool Pledge { get; set; }
		public string Description { get; set; }
		public bool NotIncluded
		{
			get
			{
				if (!StatusId.HasValue)
					return true;
				return StatusId.Value != (int)Contribution.StatusCode.Recorded
					|| BundleController.ReturnedReversedTypes.Contains(ContributionTypeId.Value);
			}
		}
	}
	public class FundTotalInfo
	{
		public int FundId { get; set; }
		public string FundName { get; set; }
		public decimal? Total { get; set; }
		public int? Count { get; set; }
	}
	public class RangeInfo
	{
		string[] RangeLabels = new string[] 
        { 
            "0 - 100", 
            "101 - 250",
            "251 - 500",
            "501 - 750",
            "751 - 1,000",
            "1,001 - 2,000",
            "2,001 - 3,000",
            "3,001 - 4,000",
            "4,001 - 5,000",
            "5,001 - 10,000",
            "10,001 - 20,000",
            "20,001 - 30,000",
            "30,001 - 40,000",
            "40,001 - 50,000",
            "50,001 - 100,000",
            "> 100,000",
        };
		public string Range
		{
			get { return RangeLabels[RangeId - 1]; }
		}
		public int RangeId { get; set; }
		public int? Count { get; set; }
		public int? DonorCount { get; set; }
		public decimal? PctCount { get; set; }
		private decimal? total;
		public decimal? Total
		{
			get { return total.HasValue ? total : (decimal?)0.0; }
			set { total = value; }
		}
		public decimal PctTotal { get; set; }
	}
	[DataObject]
	public class BundleController
	{
		public class YearInfo
		{
			public int PeopleId { get; set; }
			public int Year { get; set; }
			public int Count { get; set; }
			public decimal? Amount { get; set; }
		}
		public static int[] ReturnedReversedTypes = new int[] 
        { 
            (int)Contribution.TypeCode.ReturnedCheck, 
            (int)Contribution.TypeCode.Reversed 
        };
		[DataObjectMethod(DataObjectMethodType.Select, true)]
		public IEnumerable<BundleInfo> FetchBundles(int startRowIndex, int maximumRows, string sortExpression)
		{
            var q = from b in DbUtil.Db.BundleHeaders select b;
			var q2 = from b in q
					 where b.RecordStatus == false
					 select new BundleInfo
					 {
						 BundleId = b.BundleHeaderId,
						 HeaderType = b.BundleHeaderType.Description,
						 PostingDate = b.BundleDetails.Max(bd => bd.Contribution.PostingDate),
						 DepositDate = b.DepositDate,
						 TotalBundle = (b.TotalCash ?? 0) + (b.TotalChecks ?? 0) + (b.TotalEnvelopes ?? 0),
						 FundId = b.FundId,
						 Fund = b.Fund.FundDescription,
						 Status = b.BundleStatusType.Description,
					 };
			_count = q2.Count();
			q2 = ApplySort(q2, sortExpression);
			return q2.Skip(startRowIndex).Take(maximumRows);
		}
		int _count;
		public int Count(int startRowIndex, int maximumRows, string sortExpression)
		{
			return _count;
		}
		private IQueryable<BundleInfo> ApplySort(IQueryable<BundleInfo> q, string sortExpression)
		{
			switch (sortExpression)
			{
				case "PostingDate":
					q = q.OrderBy(b => b.PostingDate);
					break;
				case "PostingDate DESC":
					q = q.OrderByDescending(b => b.PostingDate);
					break;
				case "Status":
					q = q.OrderBy(b => b.Status).ThenByDescending(b => b.BundleId);
					break;
				case "Status DESC":
				default:
					q = q.OrderByDescending(b => b.Status).ThenByDescending(b => b.BundleId);
					break;
				case "BundleId":
					q = q.OrderBy(b => b.BundleId);
					break;
				case "BundleId DESC":
					q = q.OrderByDescending(b => b.BundleId);
					break;
				case "HeaderType":
					q = q.OrderBy(b => b.HeaderType);
					break;
				case "HeaderType DESC":
					q = q.OrderByDescending(b => b.HeaderType);
					break;
				case "DepositDate":
					q = q.OrderBy(b => b.DepositDate);
					break;
				case "DepositDate DESC":
					q = q.OrderByDescending(b => b.DepositDate);
					break;
				case "TotalBundle":
					q = q.OrderBy(b => b.TotalBundle);
					break;
				case "TotalCash DESC":
					q = q.OrderByDescending(b => b.TotalBundle);
					break;
				case "Fund":
					q = q.OrderBy(b => b.Fund);
					break;
				case "Fund DESC":
					q = q.OrderByDescending(b => b.Fund);
					break;
			}
			return q;
		}
		[DataObjectMethod(DataObjectMethodType.Select, false)]
		public IEnumerable<ContributionInfo> FetchBundleDetails(int bundleId)
		{
            var q = from d in DbUtil.Db.BundleDetails
					where bundleId == d.BundleHeaderId
					orderby d.BundleDetailId
					select new ContributionInfo
					{
						ContributionAmount = d.Contribution.ContributionAmount,
						ContributionDate = d.Contribution.ContributionDate,
						ContributionId = d.ContributionId,
						ContributionType = d.Contribution.ContributionType.Description,
						ContributionTypeId = d.Contribution.ContributionTypeId,
						Fund = d.Contribution.ContributionFund.FundName,
						PeopleId = d.Contribution.PeopleId,
						Pledge = d.Contribution.PledgeFlag,
						StatusId = d.Contribution.ContributionStatusId.Value,
						Status = d.Contribution.ContributionStatus.Description,
						Name = d.Contribution.Person.Name2,
					};
			return q;
		}
		private int countContributions;
		public int CountContributions(int year, int startRowIndex, int maximumRows, string sortExpression,
			int statusid, int typeid, int fundid)
		{
			return countContributions;
		}
		public int CountContributions(int startRowIndex, int maximumRows, string sortExpression,
		  int peopleId, int year, int statusid, int typeid, int fundid)
		{
			return countContributions;
		}
		[DataObjectMethod(DataObjectMethodType.Select, false)]
		public IEnumerable<ContributionInfo> FetchContributions(int startRowIndex, int maximumRows, string sortExpression,
			int peopleId, int year, int statusid, int typeid, int fundid)
		{
			var q = FetchContributions2(sortExpression, peopleId, year, statusid, typeid, fundid);
			return q.Skip(startRowIndex).Take(maximumRows);
		}
		[DataObjectMethod(DataObjectMethodType.Select, false)]
		public IEnumerable<ContributionInfo> FetchContributions(int year, int startRowIndex, int maximumRows, string sortExpression,
			int statusid, int typeid, int fundid)
		{
			var q = FetchContributions2(sortExpression, 0, year, statusid, typeid, fundid);
			return q.Skip(startRowIndex).Take(maximumRows);
		}
		private IQueryable<ContributionInfo> FetchContributions2(string sortExpression, int peopleId, int year, int statusid, int typeid, int fundid)
		{
            var q = from c in DbUtil.Db.Contributions
					select c;
			q = ApplyWhere(q, peopleId, year, statusid, typeid, fundid);
			countContributions = q.Count();
			q = SortContributions(sortExpression, q);
			var q2 = from c in q
					 let bd = c.BundleDetails.FirstOrDefault()
					 select new ContributionInfo
					 {
						 BundleId = bd == null ? 0 : bd.BundleHeaderId,
						 ContributionAmount = c.ContributionAmount,
						 ContributionDate = c.ContributionDate,
						 ContributionId = c.ContributionId,
						 ContributionType = c.ContributionType.Description,
						 ContributionTypeId = c.ContributionTypeId,
						 Fund = c.ContributionFund.FundName,
						 Pledge = c.PledgeFlag,
						 StatusId = c.ContributionStatusId,
						 Status = c.ContributionStatus.Description,
						 Name = c.Person.Name,
						 PeopleId = c.PeopleId,
					 };
			return q2;
		}
		private IQueryable<Contribution> ApplyWhere(IQueryable<Contribution> q, int peopleId, int year, int statusid, int typeid, int fundid)
		{
			if (peopleId != 0)
				q = q.Where(c => c.PeopleId == peopleId);
			if (year != 0)
				q = q.Where(c => c.ContributionDate.Value.Year == year);
			if (statusid != 99)
				q = q.Where(c => c.ContributionStatusId == statusid);
			if (typeid != 0)
				q = q.Where(c => c.ContributionTypeId == typeid);
			if (fundid != 0)
				q = q.Where(c => c.FundId == fundid);
			return q;
		}
		public decimal Total(int peopleId, int year, int statusid, int typeid, int fundid)
		{
            var q = from c in DbUtil.Db.Contributions
					where c.ContributionStatusId == (int)Contribution.StatusCode.Recorded
					where !ReturnedReversedTypes.Contains(c.ContributionTypeId)
					where c.PledgeFlag == false
					select c;
			q = ApplyWhere(q, peopleId, year, statusid, typeid, fundid);
			decimal? t = q.Sum(c => (decimal?)c.ContributionAmount);
			if (t.HasValue)
				return t.Value;
			return 0;
		}
		public decimal TotalItems(int BundleId)
		{
            var q = from d in DbUtil.Db.BundleDetails
					where d.BundleHeaderId == BundleId
					where d.Contribution.ContributionStatusId == (int)Contribution.StatusCode.Recorded
					where !ReturnedReversedTypes.Contains(d.Contribution.ContributionTypeId)
					select d.Contribution;
			decimal? t = q.Sum(c => (decimal?)c.ContributionAmount);
			if (t.HasValue)
				return t.Value;
			return 0;
		}
		public decimal TotalHeader(int BundleId)
		{
            var b = DbUtil.Db.BundleHeaders.Single(bh => bh.BundleHeaderId == BundleId);
			return (b.TotalEnvelopes.HasValue ? b.TotalEnvelopes.Value : 0)
				+ (b.TotalChecks.HasValue ? b.TotalChecks.Value : 0)
				+ (b.TotalCash.HasValue ? b.TotalCash.Value : 0);
		}

		[DataObjectMethod(DataObjectMethodType.Select, false)]
		public IEnumerable FetchYearlyContributions(int peopleId)
		{
            var q = from c in DbUtil.Db.Contributions
					where peopleId == c.PeopleId || peopleId == 0
					where c.PledgeFlag == false
					//where c.ContributionStatusId != (int)Contribution.StatusCode.Reversed
					where c.ContributionStatusId == (int)Contribution.StatusCode.Recorded
					where !ReturnedReversedTypes.Contains(c.ContributionTypeId)
					group c by c.ContributionDate.Value.Year into g
					orderby g.Key descending
					select new YearInfo
					{
						Year = g.Key,
						Count = g.Count(),
						Amount = g.Sum(c => c.ContributionAmount),
						PeopleId = peopleId,
					};
			return q;
		}
		private static IQueryable<Contribution> SortContributions(string sortExpression, IQueryable<Contribution> q)
		{
			switch (sortExpression)
			{
				case "Date":
					q = q.OrderBy(c => c.ContributionDate);
					break;
				case "Amount":
					q = from c in q
						orderby c.ContributionAmount, c.ContributionDate descending
						select c;
					break;
				case "Type":
					q = from c in q
						orderby c.ContributionTypeId, c.ContributionDate descending
						select c;
					break;
				case "Pledge":
					q = from c in q
						orderby c.PledgeFlag, c.ContributionDate descending
						select c;
					break;
				case "Status":
					q = from c in q
						orderby c.ContributionStatusId, c.ContributionDate descending
						select c;
					break;
				case "Fund":
					q = from c in q
						orderby c.FundId, c.ContributionDate descending
						select c;
					break;
				case "Date DESC":
				default:
					q = q.OrderByDescending(c => c.ContributionDate);
					break;
				case "Amount DESC":
					q = from c in q
						orderby c.ContributionAmount descending, c.ContributionDate descending
						select c;
					break;
				case "Type DESC":
					q = from c in q
						orderby c.ContributionTypeId descending, c.ContributionDate descending
						select c;
					break;
				case "Pledge DESC":
					q = from c in q
						orderby c.PledgeFlag descending, c.ContributionDate descending
						select c;
					break;
				case "Status DESC":
					q = from c in q
						orderby c.ContributionStatusId descending, c.ContributionDate descending
						select c;
					break;
				case "Fund DESC":
					q = from c in q
						orderby c.FundId descending, c.ContributionDate descending
						select c;
					break;
			}
			return q;
		}
		public BundleHeader NewBundle()
		{
			var dt = DateTime.Now.Date;
			var dw = (int)dt.DayOfWeek;
			dt = dt.AddDays(-dw);
			var b = new BundleHeader
			{
				BundleHeaderTypeId = (int)BundleHeader.TypeCode.PreprintedEnvelope,
				BundleStatusId = (int)BundleHeader.StatusCode.Open,
				ChurchId = 1,
				ContributionDate = dt,
                CreatedBy = Util.UserId1,
				CreatedDate = DateTime.Now,
				RecordStatus = false,
				FundId = 1, // Local Church Operations
			};
            DbUtil.Db.BundleHeaders.InsertOnSubmit(b);
            DbUtil.Db.SubmitChanges();
			return b;
		}
		[DataObjectMethod(DataObjectMethodType.Select, false)]
		public List<DepositInfo> FetchDepositBundles(DateTime depositdt)
		{
            var q = from b in DbUtil.Db.BundleHeaders
					where b.DepositDate == depositdt
					orderby b.BundleHeaderId
					select new DepositInfo
					{
						BundleId = b.BundleHeaderId.ToString(),
						Cash = b.TotalCash.Value,
						Checks = b.TotalChecks.Value,
						Coins = b.TotalEnvelopes.Value,
						Total = (b.TotalCash ?? 0) + (b.TotalChecks ?? 0) + (b.TotalEnvelopes ?? 0),
					};

			var list = q.ToList();
			var t = new DepositInfo
			{
				BundleId = "TOTALS",
				Cash = list.Sum(b => b.Cash),
				Checks = list.Sum(b => b.Checks),
				Coins = list.Sum(b => b.Coins),
				Total = list.Sum(b => b.Cash + b.Checks + b.Coins),
			};
			list.Add(t);
			return list;
		}
		[DataObjectMethod(DataObjectMethodType.Select, false)]
		public IEnumerable<CodeValueItem> Funds(int PeopleId)
		{
            var q = from c in DbUtil.Db.Contributions
					where c.PeopleId == PeopleId || PeopleId == 0
					group c by new { c.FundId, c.ContributionFund.FundName } into g
					orderby g.Key.FundName
					select new CodeValueItem
					{
						Id = g.Key.FundId,
						Value = g.Key.FundName
					};
			return q;
		}
		public IEnumerable<CodeValueItem> OpenFunds()
		{
            var q = from c in DbUtil.Db.ContributionFunds
					where c.FundStatusId == 1
					orderby c.FundId
					select new CodeValueItem
					{
						Id = c.FundId,
						Value = c.FundDescription,
					};
			return q;
		}
		[DataObjectMethod(DataObjectMethodType.Select, false)]
		public IEnumerable Years(int PeopleId)
		{
            var q = from c in DbUtil.Db.Contributions
					where c.PeopleId == PeopleId || PeopleId == 0
					group c by c.ContributionDate.Value.Year into g
					orderby g.Key descending
					select g.Key;
			return q;
		}
		public IEnumerable<ExtractInfo> GetGLExtract(DateTime dt1, DateTime dt2)
		{
			var contributionTypes = new int[] 
            { 
                (int)Contribution.TypeCode.ReturnedCheck, 
                (int)Contribution.TypeCode.Reversed, 
            };
			var qIncomeFundNo67 =
				from c in DbUtil.Db.Contributions
				from d in c.BundleDetails
				where dt1 <= c.PostingDate.Value.Date
				where c.PostingDate.Value.Date <= dt2
				where d.BundleHeader.BundleStatusId == 0
				where !contributionTypes.Contains(c.ContributionTypeId)
				where !c.PledgeFlag
				group c by new { d.BundleHeaderId, c.ContributionFund, c.ContributionDate } into g
				select new ExtractInfo
				{
					Fund = g.Key.ContributionFund.FundIncomeFund,
					Month = ((g.Max(c => c.PostingDate.Value.Month) + 8) % 12) + 1,
					HeaderId = g.Key.BundleHeaderId,
					ContributionDate = g.Key.ContributionDate.Value,
					FundName = g.Key.ContributionFund.FundName,
					FundDept = g.Key.ContributionFund.FundIncomeDept, // Income
					FundAcct = g.Key.ContributionFund.FundIncomeAccount, // Income
					Amount = -g.Sum(c => c.ContributionAmount.Value),
					PostingDate = g.Max(c => c.PostingDate.Value),
				};
			var qCashFundNo67 =
				from c in DbUtil.Db.Contributions
				from d in c.BundleDetails
				where dt1 <= c.PostingDate.Value.Date
				where c.PostingDate.Value.Date <= dt2
				where d.BundleHeader.BundleStatusId == 0
				where !contributionTypes.Contains(c.ContributionTypeId) // no 6,7(reversals, returns)
				where !c.PledgeFlag
				group c by new { d.BundleHeaderId, c.ContributionFund, c.ContributionDate } into g
				select new ExtractInfo
				{
					Fund = g.Key.ContributionFund.FundCashFund,
					Month = ((g.Max(c => c.PostingDate.Value.Month) + 8) % 12) + 1,
					HeaderId = g.Key.BundleHeaderId,
					ContributionDate = g.Key.ContributionDate.Value,
					FundName = g.Key.ContributionFund.FundName,
					FundDept = g.Key.ContributionFund.FundCashDept, // Cash
					FundAcct = g.Key.ContributionFund.FundCashAccount, // Cash
					Amount = g.Sum(c => c.ContributionAmount.Value),
					PostingDate = g.Max(c => c.PostingDate.Value),
				};
			var qIncomeFundYes67 =
				from c in DbUtil.Db.Contributions
				where dt1 <= c.PostingDate.Value.Date
				where c.PostingDate.Value.Date <= dt2
				where contributionTypes.Contains(c.ContributionTypeId) // Yes 6,7 (reversals, returns)
				where !c.PledgeFlag
				select new ExtractInfo
				{
					Fund = c.ContributionFund.FundIncomeFund,
					Month = ((c.PostingDate.Value.Month + 8) % 12) + 1,
					HeaderId = 0,
					ContributionDate = c.ContributionDate.Value,
					FundName = c.ContributionFund.FundName,
					FundDept = c.ContributionFund.FundIncomeDept, // Income
					FundAcct = c.ContributionFund.FundIncomeAccount, // Income
					Amount = c.ContributionAmount.Value,
					PostingDate = c.PostingDate.Value,
				};
			var qCashFundYes67 =
				from c in DbUtil.Db.Contributions
				where dt1 <= c.PostingDate.Value.Date
				where c.PostingDate.Value.Date <= dt2
				where contributionTypes.Contains(c.ContributionTypeId) // Yes 6,7 (reversals, returns)
				where !c.PledgeFlag
				select new ExtractInfo
				{
					Fund = c.ContributionFund.FundCashFund,
					Month = (c.PostingDate.Value.Month + 8) % 12 + 1,
					HeaderId = 0,
					ContributionDate = c.ContributionDate.Value,
					FundName = c.ContributionFund.FundName,
					FundDept = c.ContributionFund.FundCashDept, // Cash
					FundAcct = c.ContributionFund.FundCashAccount, // Cash
					Amount = -c.ContributionAmount.Value,
					PostingDate = c.PostingDate.Value,
				};
			var q = qIncomeFundNo67
				.Union(qCashFundNo67)
				.Union(qIncomeFundYes67)
				.Union(qCashFundYes67);
			q = from i in q
				orderby i.Fund, i.Month, i.HeaderId
				select i;
			return q;
		}
		public FundTotalInfo FundTotal;
		[DataObjectMethod(DataObjectMethodType.Select, false)]
		public IEnumerable<FundTotalInfo> TotalsByFund(DateTime dt1, DateTime dt2, bool Pledges)
		{
			var contributionTypes = new int[] 
            { 
                (int)Contribution.TypeCode.ReturnedCheck, 
                (int)Contribution.TypeCode.Reversed, 
            };
            var q = from c in DbUtil.Db.Contributions
					where dt1 <= c.ContributionDate.Value.Date
					where c.ContributionDate.Value.Date <= dt2
					where Pledges || c.ContributionStatusId == (int)Contribution.StatusCode.Recorded
					where !contributionTypes.Contains(c.ContributionTypeId)
					where c.PledgeFlag == Pledges
					where Pledges || c.PostingDate != null
					group c by c.FundId into g
					orderby g.Key
					select new FundTotalInfo
					{
						FundId = g.Key,
						FundName = g.First().ContributionFund.FundName,
						Total = g.Sum(t => t.ContributionAmount).Value,
						Count = g.Count()
					};
			FundTotal = new FundTotalInfo
			{
				Count = q.Sum(t => t.Count),
				Total = q.Sum(t => t.Total),
			};
			return q;
		}
		public RangeInfo RangeTotal;
		[DataObjectMethod(DataObjectMethodType.Select, false)]
		public IEnumerable<RangeInfo> TotalsByFundRange(int fundid, DateTime dt1, DateTime dt2, bool Pledges)
        {
            var contributionTypes = new int[] 
            { 
                (int)Contribution.TypeCode.ReturnedCheck, 
                (int)Contribution.TypeCode.Reversed, 
            };
            var q0 = from c in DbUtil.Db.Contributions
                    where dt1 <= c.ContributionDate.Value.Date
                    where c.ContributionDate.Value.Date <= dt2
                    where Pledges || c.ContributionStatusId == (int)Contribution.StatusCode.Recorded
                    where !contributionTypes.Contains(c.ContributionTypeId)
                    where c.PledgeFlag == Pledges
                    where c.FundId == fundid || fundid == 0
                    where Pledges || c.PostingDate != null
					select c;

            var q = from c in q0
                    group c by DbUtil.Db.DollarRange(c.ContributionAmount) into g
                    orderby g.Key
                    select new RangeInfo
                    {
                        RangeId = g.Key.Value,
                        Total = g.Sum(t => t.ContributionAmount),
                        Count = g.Count(),
						DonorCount = (from d in g
									  group d by d.PeopleId into gd
									  select gd).Count()
                    };
            RangeTotal = new RangeInfo
            {
                Count = q.Sum(t => t.Count),
                Total = q.Sum(t => t.Total),
				DonorCount = (from c in q0 group c by c.PeopleId into g select g).Count()
            };
            var q2 = from r in q
                     select new RangeInfo
                     {
                         RangeId = r.RangeId,
                         Total = r.Total,
                         Count = r.Count,
						 DonorCount = r.DonorCount,
                         PctCount = (decimal)r.Count / RangeTotal.Count * 100,
                         PctTotal = r.Total.Value / RangeTotal.Total.Value * 100,
                     };
            return q2;
        }
		public class JournalInfo
		{
			public int HeaderId { get; set; }
			public string FundName { get; set; }
			public decimal Total { get; set; }
			public DateTime Date { get; set; }
			public int Count { get; set; }
		}
		public JournalInfo JournalTotal;
		[DataObjectMethod(DataObjectMethodType.Select, false)]
		public IEnumerable<JournalInfo> JournalDetails(DateTime dt1, DateTime dt2, int FundId)
		{
			var contributionTypes = new int[] 
            { 
                (int)Contribution.TypeCode.ReturnedCheck, 
                (int)Contribution.TypeCode.Reversed, 
            };
            var q = from c in DbUtil.Db.Contributions
					where dt1 <= c.ContributionDate.Value.Date
					where c.ContributionDate.Value.Date <= dt2
					where c.ContributionStatusId == (int)Contribution.StatusCode.Recorded
					where !contributionTypes.Contains(c.ContributionTypeId)
					where c.PledgeFlag == false
					where c.FundId == FundId
					group c by new { c.BundleDetails.FirstOrDefault().BundleHeader.BundleHeaderId, c.ContributionDate } into g
					select new JournalInfo
					{
						HeaderId = g.Key.BundleHeaderId,
						Total = g.Sum(t => t.ContributionAmount).Value,
						Date = g.Key.ContributionDate.Value,
						Count = g.Count(),
						FundName = g.First().ContributionFund.FundName,
					};
			JournalTotal = new JournalInfo
			{
				Count = q.Sum(t => t.Count),
				Total = q.Sum(t => t.Total),
				FundName = q.First().FundName,
			};
			return q.OrderBy(j => j.HeaderId).ThenBy(j => j.Date);
		}
	}
}
