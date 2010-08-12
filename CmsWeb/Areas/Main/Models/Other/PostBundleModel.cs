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
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Collections;
using System.IO;
using System.Globalization;
using System.Threading;
using LumenWorks.Framework.IO.Csv;

namespace CmsWeb.Models
{
    public class PostBundleModel
    {
        public class FundTotal
        {
            public string Name { get; set; }
            public decimal? Total { get; set; }
        }
        public int id { get; set; }
        public int? editid { get; set; }
        public int? pid { get; set; }
        public decimal? amt { get; set; }
        public int fund { get; set; }
        public bool pledge { get; set; }
        public string notes { get; set; }
        private BundleHeader _bundle;
        public string FundDescription { get; set; }
        public BundleHeader bundle
        {
            get
            {
                if (_bundle == null)
                {
                    _bundle = DbUtil.Db.BundleHeaders.SingleOrDefault(bh => bh.BundleHeaderId == id);
                    if (_bundle != null && _bundle.FundId.HasValue)
                        FundDescription = _bundle.Fund.FundDescription;
                }
                return _bundle;
            }
        }
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
                        ContributionId = d.ContributionId,
                        PeopleId = d.Contribution.PeopleId,
                        Name = d.Contribution.Person.Name2,
                        Amt = d.Contribution.ContributionAmount,
                        Fund = d.Contribution.ContributionFund.FundName,
                        FundId = d.Contribution.FundId,
                        Notes = d.Contribution.ContributionDesc,
                        eac = d.Contribution.BankAccount,
                        Address = d.Contribution.Person.PrimaryAddress,
                        City = d.Contribution.Person.PrimaryCity,
                        State = d.Contribution.Person.PrimaryState,
                        Zip = d.Contribution.Person.PrimaryZip,
                        Age = d.Contribution.Person.Age,
                        extra = d.Contribution.ExtraDatum.Data,
                        pledge = d.Contribution.PledgeFlag,
                    };
            var list = q.ToList();
            foreach (var c in list)
            {
                string s = null;
                if (!c.PeopleId.HasValue)
                {
                    s = c.extra ?? "";
                    if (c.eac.HasValue())
                        s += " (" + Util.Decrypt(c.eac) + ")";
                    if (s.HasValue())
                        c.Name = s;
                }
            }
            return list;
        }
        public IEnumerable<FundTotal> TotalsByFund()
        {
            var q = from d in DbUtil.Db.BundleDetails
                    where d.BundleHeaderId == id
                    group d by d.Contribution.ContributionFund.FundName into g
                    orderby g.Key
                    select new FundTotal
                    {
                        Name = g.Key,
                        Total = g.Sum(d => d.Contribution.ContributionAmount)
                    };
            return q;
        }
        public IEnumerable<SelectListItem> Funds()
        {
            var q = from f in DbUtil.Db.ContributionFunds
                    where f.FundStatusId == 1
                    orderby f.FundId
                    select new SelectListItem
                    {
                        Text = "{0} - {1}".Fmt(f.FundId, f.FundName),
                        Value = f.FundId.ToString()
                    };
            return q;
        }
        public IEnumerable Funds2()
        {
            var q = from f in DbUtil.Db.ContributionFunds
                    where f.FundStatusId == 1
                    orderby f.FundId
                    select new
                    {
                        Code = f.FundId.ToString(),
                        Value = "{0} - {1}".Fmt(f.FundId, f.FundName),
                    };
            return q.ToDictionary(k => k.Code, v => v.Value);
        }
        public string GetNameFromPid()
        {
            var q = from i in DbUtil.Db.People
                    where i.PeopleId == pid.ToInt()
                    select i.Name2;
            var s = q.SingleOrDefault();
            if (!s.HasValue())
                return "not found";
            return s;
        }
        public static string Names(string q, int limit)
        {
            var qu = from p in DbUtil.Db.People
                     where p.Name2.StartsWith(q)
                     orderby p.Name2
                     select p.Name2 +
                     "|" + p.PeopleId + "|" + (p.Age ?? 0) + "|" + p.PrimaryAddress;
            return string.Join("\n", qu.Take(limit).ToArray());
        }
        private object ContributionRowData(int id)
        {
            var q = from c in DbUtil.Db.Contributions
                    let bh = c.BundleDetails.First().BundleHeader
                    where c.ContributionId == id
                    select new
                    {
                        amt = amt.ToString2("c"),
                        totalitems = bh.BundleDetails.Sum(d =>
                            d.Contribution.ContributionAmount).ToString2("c"),
                        itemcount = bh.BundleDetails.Count(),
                        pledge = c.PledgeFlag,
                        fund = "{0} - {1}".Fmt(
                            c.FundId, c.ContributionFund.FundName),
                        cid = c.ContributionId,
                        tip = Tip(c.PeopleId, c.Person.Age, c.Person.PrimaryAddress, c.Person.PrimaryCity, c.Person.PrimaryState, c.Person.PrimaryZip)
                    };
            return q.First();
        }
        public object PostContribution()
        {
            var bd = new CmsData.BundleDetail
            {
                BundleHeaderId = id,
                CreatedBy = Util.UserId,
                CreatedDate = DateTime.Now,
            };
            int type;
            if (pledge == true)
                type = (int)Contribution.TypeCode.Pledge;
            else
                type = (int)Contribution.TypeCode.CheckCash;
            bd.Contribution = new Contribution
            {
                CreatedBy = Util.UserId,
                CreatedDate = bd.CreatedDate,
                FundId = fund,
                PeopleId = pid,
                ContributionDate = bundle.ContributionDate,
                ContributionAmount = amt,
                ContributionStatusId = 0,
                PledgeFlag = pledge,
                ContributionTypeId = type,
                ContributionDesc = notes
            };
            bundle.BundleDetails.Add(bd);
            DbUtil.Db.SubmitChanges();
            return ContributionRowData(bd.ContributionId);
        }
        public object UpdateContribution()
        {
            int type;
            if (pledge == true)
                type = (int)Contribution.TypeCode.Pledge;
            else
                type = (int)Contribution.TypeCode.CheckCash;
            var c = DbUtil.Db.Contributions.Single(cc => cc.ContributionId == editid);
            c.FundId = fund;
            c.PeopleId = pid;
            c.ContributionAmount = amt;
            c.PledgeFlag = pledge;
            c.ContributionTypeId = type;
            c.ContributionDesc = notes;
            DbUtil.Db.SubmitChanges();
            return ContributionRowData(c.ContributionId);
        }
        public object DeleteContribution()
        {
            var bd = bundle.BundleDetails.SingleOrDefault(d => d.ContributionId == editid);
            if (bd != null)
            {
                var c = bd.Contribution;
                DbUtil.Db.BundleDetails.DeleteOnSubmit(bd);
                bundle.BundleDetails.Remove(bd);
                DbUtil.Db.Contributions.DeleteOnSubmit(c);
                DbUtil.Db.SubmitChanges();
            }
            return new
            {
                totalitems = bundle.BundleDetails.Sum(d =>
                    d.Contribution.ContributionAmount).ToString2("c"),
                itemcount = bundle.BundleDetails.Count(),
            };
        }
        private static string[] columns = 
        { 
            "Submit Date=Date,Post Amount=Amount,Check Number=Check,R/T=Route,Account Number=Account,Deposit Number=Bundle",
            "Date Entered=Date,Total=Amount,ProfileID=Account"
        };
        private static string[] _MagTek = { "From MICR :" };

        private static bool CheckNames(string[] names, IEnumerable<string> lookfor)
        {
            var q = from n in names
                    join r in lookfor on n equals r
                    select n;
            return q.Count() == lookfor.Count();
        }

        private static Dictionary<string, string> GetNames(string[] names)
        {
            foreach (var s in columns)
            {
                var rq = from c in s.Split(',')
                         let a = c.Split('=')
                         select new { col = a[0], name = a[1] };
                var rd = rq.ToDictionary(d => d.col, d => d.name);
                if (CheckNames(names, rd.Keys))
                    return rd;
            }
            return null;
        }
        public static int? BatchProcess(string text, DateTime date)
        {
            if (text.StartsWith("From MICR :"))
                return BatchProcessMagTek(text, date);
            using (var csv = new CsvReader(new StringReader(text), true))
            {
                var names = csv.GetFieldHeaders();
                var rd = GetNames(names);
                if (rd == null)
                    return null;
                if (rd.ContainsKey("ProfileID"))
                    return BatchProcessServiceU(csv, date);
                return BatchProcess(csv, rd, date);
            }
        }
        private static int? BatchProcessMagTek(string lines, DateTime date)
        {
            var now = DateTime.Now;
            var bh = new BundleHeader
            {
                BundleHeaderTypeId = (int)BundleHeader.TypeCode.ChecksAndCash,
                BundleStatusId = (int)BundleHeader.StatusCode.Open,
                ContributionDate = date,
                CreatedBy = Util.UserId,
                CreatedDate = now,
                FundId = 1
            };
            DbUtil.Db.BundleHeaders.InsertOnSubmit(bh);

            var re = new Regex(
@"(T(?<rt>[\d?]+)T(?<ac>[\d ?]*)U\s*(?<ck>[\d?]+))|
(CT(?<rt>[\d?]+)A(?<ac>[\d ?]*)C(?<ck>[\d?]+)M)",
                RegexOptions.IgnoreCase);
            var m = re.Match(lines);
            while (m.Success)
            {
                var rt = m.Groups["rt"].Value;
                var ac = m.Groups["ac"].Value;
                var ck = m.Groups["ck"].Value;
                var bd = new CmsData.BundleDetail
                {
                    CreatedBy = Util.UserId,
                    CreatedDate = now,
                };
                bh.BundleDetails.Add(bd);
                var qf = from f in DbUtil.Db.ContributionFunds
                         where f.FundStatusId == 1
                         orderby f.FundId
                         select f.FundId;

                bd.Contribution = new Contribution
                {
                    CreatedBy = Util.UserId,
                    CreatedDate = now,
                    ContributionDate = date,
                    FundId = qf.First(),
                    ContributionStatusId = 0,
                    ContributionTypeId = (int)Contribution.TypeCode.CheckCash,
                };
                bd.Contribution.ContributionDesc = ck;
                var eac = Util.Encrypt(rt + "," + ac);
                var q = from kc in DbUtil.Db.CardIdentifiers
                        where kc.Id == eac
                        select kc.PeopleId;
                var pid = q.SingleOrDefault();
                if (pid != null)
                    bd.Contribution.PeopleId = pid;
                else
                {
                    bd.Contribution.BankAccount = eac;
                    bd.Contribution.ContributionDesc = ck;
                }

                m = m.NextMatch();
            }
            bh.TotalChecks = 0;
            bh.TotalCash = 0;
            bh.TotalEnvelopes = 0;
            DbUtil.Db.SubmitChanges();
            return bh.BundleHeaderId;
        }

        private static BundleHeader GetBundleHeader(DateTime date, DateTime now)
        {
            var bh = new BundleHeader
                        {
                            BundleHeaderTypeId = (int)BundleHeader.TypeCode.PreprintedEnvelope,
                            BundleStatusId = (int)BundleHeader.StatusCode.Open,
                            ContributionDate = date,
                            CreatedBy = Util.UserId,
                            CreatedDate = now,
                            FundId = 1
                        };
            DbUtil.Db.BundleHeaders.InsertOnSubmit(bh);
            bh.BundleStatusId = (int)BundleHeader.StatusCode.Open;
            bh.BundleHeaderTypeId = (int)BundleHeader.TypeCode.ChecksAndCash;
            return bh;
        }
        private static void FinishBundle(BundleHeader bh)
        {
            bh.TotalChecks = bh.BundleDetails.Sum(d => d.Contribution.ContributionAmount);
            bh.TotalCash = 0;
            bh.TotalEnvelopes = 0;
            DbUtil.Db.SubmitChanges();
        }
        public static int? BatchProcess(CsvReader csv, Dictionary<string, string> Names, DateTime date)
        {
            var now = DateTime.Now;
            var prevbundle = -1;
            var curbundle = 0;

            var bh = GetBundleHeader(date, now);

            int fieldCount = csv.FieldCount;
            var cols = csv.GetFieldHeaders();

            while (csv.ReadNextRecord())
            {
                var bd = new CmsData.BundleDetail
                {
                    CreatedBy = Util.UserId,
                    CreatedDate = now,
                };
                var qf = from f in DbUtil.Db.ContributionFunds
                         where f.FundStatusId == 1
                         orderby f.FundId
                         select f.FundId;

                bd.Contribution = new Contribution
                {
                    CreatedBy = Util.UserId,
                    CreatedDate = now,
                    ContributionDate = date,
                    FundId = qf.First(),
                    ContributionStatusId = 0,
                    ContributionTypeId = (int)Contribution.TypeCode.CheckCash,
                };
                string ac = null, rt = null;
                for (var c = 1; c < fieldCount; c++)
                {
                    var col = cols[c].Trim();
                    if (!Names.ContainsKey(col))
                        continue;
                    switch (Names[col])
                    {
                        case "Bundle":
                            curbundle = csv[c].ToInt();
                            if (curbundle != prevbundle)
                            {
                                FinishBundle(bh);
                                bh = GetBundleHeader(date, now);
                                prevbundle = curbundle;
                            }
                            break;
                        //case "Date": // does not parse TZ correctly
                        //    bd.Contribution.ContributionDate = a[c].ToDate();
                        //    break;
                        case "Amount":
                            bd.Contribution.ContributionAmount = csv[c].GetAmount();
                            break;
                        case "Check":
                            bd.Contribution.ContributionDesc = csv[c];
                            break;
                        case "Route":
                            rt = csv[c];
                            break;
                        case "Account":
                            ac = csv[c];
                            break;
                    }
                    var eac = Util.Encrypt(rt + "|" + ac);
                    var q = from kc in DbUtil.Db.CardIdentifiers
                            where kc.Id == eac
                            select kc.PeopleId;
                    var pid = q.SingleOrDefault();
                    if (pid != null)
                        bd.Contribution.PeopleId = pid;
                    else
                        bd.Contribution.BankAccount = eac;
                }
                bh.BundleDetails.Add(bd);
            }
            FinishBundle(bh);
            return bh.BundleHeaderId;

        }
        private static int? FindFund(string s)
        {
            var qf = from f in DbUtil.Db.ContributionFunds
                     where f.FundName == s
                     select f;
            var fund = qf.FirstOrDefault();
            if (fund == null)
                return null;
            return fund.FundId;
        }
        private static CmsData.BundleDetail CreateContribution(DateTime date, int fundid)
        {
            var bd = new CmsData.BundleDetail
            {
                CreatedBy = Util.UserId,
                CreatedDate = Util.Now,
            };
            bd.Contribution = new Contribution
            {
                CreatedBy = Util.UserId,
                CreatedDate = Util.Now,
                ContributionDate = date,
                FundId = fundid,
                ContributionStatusId = 0,
                ContributionTypeId = (int)Contribution.TypeCode.CheckCash,
            };
            return bd;
        }
        public static int? BatchProcessServiceU(CsvReader csv, DateTime date)
        {
            var cols = csv.GetFieldHeaders();
            var now = DateTime.Now;

            var bh = GetBundleHeader(date, now);

            while (csv.ReadNextRecord())
            {
                string ac = null, oth = null, first = null, last = null, addr = null, name = null;
                var dt = date;
                for (var c = 1; c < csv.FieldCount; c++)
                {
                    var col = cols[c].Trim();
                    switch (col)
                    {
                        case "Date Entered":
                            dt = csv[c].ToDate() ?? date;
                            break;
                        case "ProfileID":
                            ac = csv[c];
                            break;
                        case "First Name":
                            first = csv[c];
                            break;
                        case "Last Name":
                            last = csv[c];
                            break;
                        case "Full Name":
                            name = csv[c];
                            break;
                        case "Address":
                            addr = csv[c];
                            break;
                        case "Designation for &quot;Other&quot;":
                            oth = csv[c];
                            break;
                    }
                }
                var eac = Util.Encrypt(ac);
                var q = from kc in DbUtil.Db.CardIdentifiers
                        where kc.Id == eac
                        select kc.PeopleId;
                var pid = q.SingleOrDefault();
                string bankac = null;
                ExtraDatum ed = null;
                if (pid == null)
                {
                    bankac = eac;
                    string person;
                    if (last.HasValue())
                        person = "{1}, {0}; {2}".Fmt(first, last, addr);
                    else
                        person = "{0}; {1}".Fmt(name, addr);
                    ed = new ExtraDatum { Data = person, Stamp = Util.Now };
                }
                CmsData.BundleDetail bd = null;
                for (var c = 1; c < csv.FieldCount; c++)
                {
                    var col = cols[c].Trim();
                    if (col != "Total" && csv[c].StartsWith("$") && csv[c].GetAmount() > 0)
                    {
                        var fundid = FindFund(col);
                        bd = CreateContribution(date, fundid ?? 1);
                        bd.Contribution.ContributionAmount = csv[c].GetAmount();
                        if (col == "Other")
                            col = oth;
                        if (!fundid.HasValue)
                            bd.Contribution.ContributionDesc = col;
                        bd.Contribution.BankAccount = bankac;
                        bd.Contribution.PeopleId = pid;
                        bh.BundleDetails.Add(bd);
                        if (ed != null)
                            bd.Contribution.ExtraDatum = ed;
                    }
                }
            }
            FinishBundle(bh);
            return bh.BundleHeaderId;
        }
        public static string Tip(int? pid, int? age, string address, string city, string state, string zip)
        {
            return "PeopleId: {0}|Age: {1}|{2}|{3}".Fmt(pid, age, address, Util.FormatCSZ(city, state, zip));
        }
        public class ContributionInfo
        {
            public int ContributionId { get; set; }
            public string eac { get; set; }
            public string extra { get; set; }
            public int? PeopleId { get; set; }
            public int? Age { get; set; }
            public string Address { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string Zip { get; set; }
            public bool pledge { get; set; }
            public string CityStateZip
            {
                get
                {
                    return Util.FormatCSZ(City, State, Zip);
                }
            }
            public string Name { get; set; }
            public decimal? Amt { get; set; }
            public string AmtDisplay
            {
                get
                {
                    return Amt.ToString2("c");
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
            public string tip
            {
                get
                {
                    return Tip(PeopleId, Age, Address, City, State, Zip);
                }
            }
        }
    }
}
