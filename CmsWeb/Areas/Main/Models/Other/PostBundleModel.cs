/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using UtilityExtensions;
using System.Text;
using CmsData;
using System.Data.Linq.SqlClient;

namespace CMSWeb.Models
{
    public class PostBundleModel
    {
        public int id { get; set; }
        public int? pid { get; set; }
        public decimal? amt { get; set; }
        public DateTime? dt { get; set; }
        public string fund { get; set; }
        public bool? pledge { get; set; }
        public string notes { get; set; }
        public PostBundleModel()
        {

        }
        public PostBundleModel(int id)
        {
            this.id = id;
        }

        public IEnumerable<ContributionInfo> FetchContributions()
        {
            var q = from d in DbUtil.Db.BundleDetails
                    where d.BundleHeaderId == id
                    orderby d.CreatedDate descending
                    select new ContributionInfo
                    {
                        PeopleId = d.Contribution.PeopleId.Value,
                        Name = d.Contribution.Person.Name2,
                        Amt = d.Contribution.ContributionAmount ?? 0,
                        Fund = d.Contribution.ContributionFund.FundDescription,
                        FundId = d.Contribution.FundId,
                        Notes = d.Contribution.ContributionDesc
                    };
            return q;
        }
        public class ContributionInfo
        {
            public int PeopleId { get; set; }
            public string Name { get; set; }
            public decimal Amt { get; set; }
            public string AmtDisplay
            {
                get
                {
                    return Amt.ToString("c");
                }
            }
            public string Fund { get; set; }
            public int FundId { get; set; }
            public string FundDisplay
            {
                get
                {
                    return "{0} - {1}".Fmt(FundId, Fund);
                }
            }
            public string Notes { get; set; }
        }
    }
}
