using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;
using UtilityExtensions;

namespace CmsWeb.Models
{
	public class TotalsByFundModel
	{
		public DateTime? Dt1 { get; set; }
		public DateTime? Dt2 { get; set; }
		public int CampusId { get; set; }
		public string Sort { get; set; }
		public string Dir { get; set; }
		public bool NonTaxDeductible { get; set; }
		public bool Pledges { get; set; }
		public bool IncUnclosedBundles { get; set; }

		public TotalsByFundModel()
		{
			var today = Util.Now.Date;
			var first = new DateTime(today.Year, today.Month, 1);
			if (today.Day < 8)
				first = first.AddMonths(-1);
			Dt1 = first;
			Dt2 = first.AddMonths(1).AddDays(-1);
		}
        public FundTotalInfo FundTotal;
        public IEnumerable<FundTotalInfo> TotalsByFund()
        {
            var contributionTypes = new int[] 
            { 
                (int)Contribution.TypeCode.ReturnedCheck, 
                (int)Contribution.TypeCode.Reversed, 
            };
            var q = from c in DbUtil.Db.Contributions
                    where !contributionTypes.Contains(c.ContributionTypeId)
					where (c.ContributionFund.NonTaxDeductible ?? false) == NonTaxDeductible
                    where c.ContributionTypeId != (int)Contribution.TypeCode.BrokeredProperty
                    where Pledges || c.ContributionStatusId == (int)Contribution.StatusCode.Recorded
                    where c.ContributionDate >= Dt1 && c.ContributionDate.Value.Date <= Dt2
                    where c.PledgeFlag == Pledges
					let status = c.BundleDetails.First().BundleHeader.BundleStatusId
					where status == 0 || IncUnclosedBundles
                    where CampusId == 0 || c.Person.CampusId == CampusId
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
		public IEnumerable<SelectListItem> Campuses()
		{
			var list = (from c in DbUtil.Db.Campus
				   orderby c.Description
				   select new SelectListItem()
				   {
					   Value = c.Id.ToString(),
					   Text = c.Description,
				   }).ToList();
			list.Insert(0, new SelectListItem {Text = "(not specified)", Value = "0"});
			return list;
		}

		public class FundTotalInfo
		{
			public int FundId { get; set; }
			public string FundName { get; set; }
			public decimal? Total { get; set; }
			public int? Count { get; set; }
		}
	}
}