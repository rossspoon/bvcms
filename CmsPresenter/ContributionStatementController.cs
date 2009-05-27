/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using CmsData;
using CmsData.View;
using System.Collections;
using UtilityExtensions;
using CMSPresenter.InfoClasses;

namespace CMSPresenter
{
    public class PledgeSummaryInfo
    {
        public string Fund { get; set; }
        public decimal? ContributionAmount { get; set; }
        public decimal? PledgeAmount { get; set; }
        public string Description { get; set; }
    }

    public class ContributionStatementController
    {
        int[] Gifts = new int[] { 2, 3, 4 };

        public IEnumerable contributions(int pid, DateTime fromDate, DateTime toDate)
        {
            var q = from c in DbUtil.Db.Contributions
                    where c.PeopleId == pid
                    where c.ContributionDate >= fromDate.Date && c.ContributionDate <= toDate.Date
                    where !BundleController.ReturnedReversedTypes.Contains(c.ContributionTypeId)
                    where c.ContributionStatusId == (int)Contribution.StatusCode.Recorded
                    select new
                    {
                        amount = c.ContributionAmount,
                        date = c.ContributionDate,
                        fundname = c.ContributionFund.FundName
                    };
            return q;
        }

        private IEnumerable<ContributorInfo> contributors(DateTime fromDate, DateTime toDate, int PeopleId, int SpouseId, int FamilyId)
        {
            //var pids = new int[] { 817023, 865610, 828611, 828612 };

            var q11 = from p in DbUtil.Db.Contributors(fromDate, toDate, PeopleId, SpouseId, FamilyId)
                      let option = (p.ContributionOptionsId ?? 0) == 0 ? 1 : p.ContributionOptionsId
                      let name = (p.ContributionOptionsId == 1 ?
                                 (p.Title != null ? p.Title + " " + p.Name : p.Name)
                                 : (p.SpouseId == null ?
                                     (p.Title != null ? p.Title + " " + p.Name : p.Name)
                                     : (p.HohFlag == 1 ?
                                         (p.Title != null ?
                                             p.Title + " and Mrs. " + p.Name
                                             : "Mr. and Mrs. " + p.Name)
                                         : (p.SpouseTitle != null ?
                                             p.SpouseTitle + " and Mrs. " + p.SpouseName
                                             : "Mr. and Mrs. " + p.SpouseName))))
                           + ((p.Suffix == null || p.Suffix == "") ? "" : ", " + p.Suffix)
                      //where pids.Contains(p.PeopleId)
                      where option == 1 || (option == 2 && p.HeadOfHouseholdId == p.PeopleId)
                      orderby p.HohFlag, p.PositionInFamilyId, p.Age
                      select new ContributorInfo
                      {
                          Name = name,
                          Address1 = p.PrimaryAddress,
                          Address2 = p.PrimaryAddress2,
                          City = p.PrimaryCity,
                          State = p.PrimaryState,
                          Zip = p.PrimaryZip,
                          PeopleId = p.PeopleId,
                          SpouseID = p.SpouseId,
                          DeacesedDate = p.DeceasedDate,
                          FamilyId = p.FamilyId,
                          Age = p.Age,
                          FamilyPositionId = p.PositionInFamilyId,
                          hohInd = p.HohFlag,
                      };
            return q11;
        }

        public IEnumerable<ContributorInfo> contributors(DateTime fromDate, DateTime toDate)
        {
            return contributors(fromDate, toDate, 0, 0, 0);
        }

        public IEnumerable<ContributorInfo> contributor(int pid, DateTime fromDate, DateTime toDate)
        {
            int sid = DbUtil.Db.People.Where(p => p.PeopleId == pid).Single().SpouseId.ToInt();
            return contributors(fromDate, toDate, pid, sid, 0);
        }

        public IEnumerable<ContributorInfo> family(int fid, DateTime fromDate, DateTime toDate)
        {
            return contributors(fromDate, toDate, 0, 0, fid);
        }

        public IEnumerable<ContributionInfo> contributions(int pid, int? spid, DateTime fromDate, DateTime toDate)
        {
            var q = from c in DbUtil.Db.Contributions
                    where !BundleController.ReturnedReversedTypes.Contains(c.ContributionTypeId)
                    where c.ContributionTypeId != (int)Contribution.TypeCode.BrokeredProperty
                    where c.ContributionStatusId == (int)Contribution.StatusCode.Recorded
                    where c.ContributionDate >= fromDate.Date && c.ContributionDate <= toDate.Date
                    where c.PeopleId == pid || (c.Person.ContributionOptionsId == 2 && c.PeopleId == spid)
                    where !c.PledgeFlag
                    orderby c.ContributionDate
                    select new ContributionInfo
                    {
                        ContributionAmount = c.ContributionAmount,
                        ContributionDate = c.ContributionDate,
                        Fund = c.ContributionFund.FundName,
                    };

            return q;
        }

        public IEnumerable<ContributionInfo> gifts(int pid, int? spid, DateTime fromDate, DateTime toDate)
        {
            var q = from c in DbUtil.Db.Contributions
                    where c.ContributionTypeId == (int)Contribution.TypeCode.BrokeredProperty
                    where c.ContributionDate >= fromDate
                    where c.ContributionDate <= toDate
                    where c.PeopleId == pid || (c.Person.ContributionOptionsId == 2 && c.PeopleId == spid)
                    where c.PledgeFlag == false
                    orderby c.ContributionDate
                    select new ContributionInfo
                    {
                        ContributionAmount = c.ContributionAmount,
                        ContributionDate = c.ContributionDate,
                        Fund = c.ContributionFund.FundName,
                        Description = c.ContributionDesc,
                    };
            return q;
        }

        public IEnumerable<PledgeSummaryInfo> pledges(int pid, int? spid, DateTime toDate)
        {
            var PledgeExcludes = new int[] 
            { 
                (int)Contribution.TypeCode.BrokeredProperty, 
                (int)Contribution.TypeCode.GraveSite, 
                (int)Contribution.TypeCode.Reversed 
            };

            var qp = from p in DbUtil.Db.Contributions
                     where p.PeopleId == pid || (p.Person.ContributionOptionsId == 2 && p.PeopleId == spid)
                     where p.PledgeFlag && p.ContributionTypeId == (int)Contribution.TypeCode.Pledge
                     where p.ContributionStatusId.Value != (int)Contribution.StatusCode.Reversed
                     where p.ContributionFund.FundStatusId == 1 // active
                     where p.ContributionDate <= toDate
                     select p;
            var qc = from c in DbUtil.Db.Contributions
                     where !PledgeExcludes.Contains(c.ContributionTypeId)
                     where c.PeopleId == pid || (c.Person.ContributionOptionsId == 2 && c.PeopleId == spid)
                     where !c.PledgeFlag
                     where c.ContributionStatusId != (int)Contribution.StatusCode.Reversed
                     where c.ContributionDate <= toDate
                     group c by c.FundId into g
                     select new { FundId = g.Key, Total = g.Sum(c => c.ContributionAmount) };
            var q = from p in qp
                    join c in qc on p.FundId equals c.FundId into items
                    from c in items.DefaultIfEmpty()
                    orderby p.ContributionFund.FundName descending
                    select new PledgeSummaryInfo
                    {
                        Fund = p.ContributionFund.FundName,
                        ContributionAmount = c.Total,
                        PledgeAmount = p.ContributionAmount,
                    };
            return q;
        }

        public IEnumerable<ContributionInfo> quarterlySummary(int pid, int? spid, DateTime fromDate, DateTime toDate)
        {
            var q = from c in DbUtil.Db.Contributions
                    where c.ContributionTypeId == (int)Contribution.TypeCode.CheckCash
                    where c.ContributionStatusId == (int)Contribution.StatusCode.Recorded
                    where c.ContributionDate >= fromDate
                    where c.ContributionDate <= toDate
                    where c.PeopleId == pid || (c.Person.ContributionOptionsId == 2 && c.PeopleId == spid)
                    where c.PledgeFlag == false
                    group c by c.ContributionFund.FundName into g
                    orderby g.Key
                    select new ContributionInfo
                    {
                        ContributionAmount = g.Sum(z => z.ContributionAmount.Value),
                        Fund = g.Key,
                    };

            return q;
        }

    }
}