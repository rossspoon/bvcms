using System;
using System.Collections.Generic;
using System.Linq;
using CmsData;
using CmsWeb.Areas.Finance.Controllers;
using CmsWeb.Models;
using UtilityExtensions;
using CmsData.Codes;

namespace CmsWeb.Areas.Finance.Models
{
    public class BundlesModel : PagerModel2
    {
        int? _count;
        public int Count()
        {
            if (!_count.HasValue)
                _count = FetchBundles().Count();
            return _count.Value;
        }

        public BundlesModel()
        {
            GetCount = Count;
            Sort = "Status";
        }
        private IQueryable<BundleHeader> _bundles;

        private IQueryable<BundleHeader> FetchBundles()
        {
            if (_bundles == null)
                _bundles = from b in DbUtil.Db.BundleHeaders select b;
            return _bundles;
        }

        public IEnumerable<BundleInfo> Bundles()
        {
            var q = ApplySort();
            var q2 = q.Skip(StartRow).Take(PageSize);
            var q3 = from b in q2
                     where b.RecordStatus == false
                     select new BundleInfo
                                {
                                    BundleId = b.BundleHeaderId,
                                    HeaderType = b.BundleHeaderType.Description,
                                    PostingDate = b.BundleDetails.Max(bd => bd.Contribution.PostingDate),
                                    DepositDate = b.DepositDate,
                                    TotalBundle = (b.TotalCash ?? 0) + (b.TotalChecks ?? 0) + (b.TotalEnvelopes ?? 0),
                                    TotalItems = b.BundleDetails.Sum(bd => bd.Contribution.ContributionAmount ?? 0),
                                    TotalNonTaxDed = b.BundleDetails.Where(bd => bd.Contribution.ContributionFund.NonTaxDeductible == true || bd.Contribution.ContributionTypeId == ContributionTypeCode.NonTaxDed).Sum(bd => bd.Contribution.ContributionAmount ?? 0),
                                    FundId = b.FundId,
                                    Fund = b.Fund.FundName,
                                    Status = b.BundleStatusType.Description,
                                    open = b.BundleStatusId == 1
                                };
            return q3;
        }
        public IQueryable<BundleHeader> ApplySort()
        {
            var q = FetchBundles();
            if (Direction == "asc")
                switch (Sort)
                {
                    case "Type":
                        q = from b in q
                            orderby b.BundleHeaderType.Description
                            select b;
                        break;
                    case "Deposit Date":
                        q = from b in q
                            orderby b.DepositDate
                            select b;
                        break;
                    case "Total Bundle":
                        q = from b in q
                            orderby b.TotalCash ?? 0 + b.TotalChecks ?? 0 + b.TotalEnvelopes ?? 0
                            select b;
                        break;
                    case "Posting Date":
                        q = from b in q
                            orderby b.BundleDetails.Max(bd => bd.Contribution.PostingDate)
                            select b;
                        break;
                    case "Id":
                    case "Status":
                        q = from b in q
                            orderby b.BundleStatusType.Description descending, b.BundleHeaderId descending 
                            select b;
                        break;
                }
            else
                switch (Sort)
                {
                    case "Type":
                        q = from b in q
                            orderby b.BundleHeaderType.Description descending 
                            select b;
                        break;
                    case "Deposit Date":
                        q = from b in q
                            orderby b.DepositDate descending 
                            select b;
                        break;
                    case "Total Bundle":
                        q = from b in q
                            orderby b.TotalCash ?? 0 + b.TotalChecks ?? 0 + b.TotalEnvelopes ?? 0 descending 
                            select b;
                        break;
                    case "Posting Date":
                        q = from b in q
                            orderby b.BundleDetails.Max(bd => bd.Contribution.PostingDate) descending 
                            select b;
                        break;
                    case "Id":
                    case "Status":
                        q = from b in q
                            orderby b.BundleStatusType.Description descending, b.BundleHeaderId descending 
                            select b;
                        break;
                }
            return q;
        }
    }
    public class BundleInfo
    {
        public int BundleId { get; set; }
        public DateTime? PostingDate { get; set; }
        public string HeaderType { get; set; }
        public DateTime? DepositDate { get; set; }
        public decimal? TotalBundle { get; set; }
        public decimal? TotalItems { get; set; }
        public decimal? TotalNonTaxDed { get; set; }
        public int? FundId { get; set; }
        public string Fund { get; set; }
        public string Status { get; set; }
        public bool open { get; set; }
    }
}