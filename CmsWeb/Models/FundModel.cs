/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using UtilityExtensions;
using CmsData;
using System.Web.Mvc;

namespace CMSWeb.Models
{
    public class FundModel
    {
        public int? FundId
        {
            get
            {
                return fund.FundId;
            }
            set
            {
                fund = DbUtil.Db.ContributionFunds.SingleOrDefault(f => f.FundId == value);
            }
        }
        public ContributionFund fund { get; set; }

        public List<SelectListItem> FundStatuses()
        {
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "Open", Value = "1"});
            list.Add(new SelectListItem { Text = "Closed", Value = "2"});
            return list;
        }
        public int InsertFund()
        {
            var f = new ContributionFund { FundName = "new fund" };
            f.FundId = DbUtil.Db.ContributionFunds.Max(fu => fu.FundId) + 1;
            DbUtil.Db.ContributionFunds.InsertOnSubmit(f);
            DbUtil.Db.SubmitChanges();
            return f.FundId;
        }
        public void DeleteFund(int id)
        {
            var f = DbUtil.Db.ContributionFunds.SingleOrDefault(fu => fu.FundId == id);
            if (f != null)
                DbUtil.Db.ContributionFunds.DeleteOnSubmit(f);
            DbUtil.Db.SubmitChanges();
        }
    }
}
