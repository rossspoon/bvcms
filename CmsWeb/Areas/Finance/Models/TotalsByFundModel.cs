using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;
using CmsData.View;
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
            var q = from c in DbUtil.Db.GetTotalContributions2(Dt1, Dt2, CampusId, NonTaxDeductible, IncUnclosedBundles)
                    group c by c.FundId
                    into g
                    orderby g.Key
                    select new FundTotalInfo
                               {
                                   FundId = g.Key,
                                   FundName = g.First().FundName,
                                   Total = g.Sum(t => t.Amount).Value,
                                   Count = g.Count()
                               };
            FundTotal = new FundTotalInfo
                            {
                                Count = q.Sum(t => t.Count),
                                Total = q.Sum(t => t.Total),
                            };
            return q;
        }

        public GetTotalContributionsRange RangeTotal;

        public IEnumerable<GetTotalContributionsRange> TotalsByRange()
        {
            var list = (from r in DbUtil.Db.GetTotalContributionsRange(Dt1, Dt2, CampusId, NonTaxDeductible, IncUnclosedBundles)
                       orderby r.Range
                       select r).ToList();
            RangeTotal = new GetTotalContributionsRange
                             {
                                 Count = list.Sum(t => t.Count),
                                 Total = list.Sum(t => t.Total),
                             };
            return list;
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