using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Text;
using System.Linq;
using System.Xml.Serialization;
using CmsData.Codes;
using UtilityExtensions;
using IronPython.Hosting;
using System.Data.Linq;

namespace CmsData.API
{
    public class APIContribution
    {
        private CMSDataContext Db;

        public APIContribution(CMSDataContext Db)
        {
            this.Db = Db;
        }
		public string PostContribution(int PeopleId, decimal Amount, int FundId, string desc, string date, int? type)
		{
			try
			{
				var p = Db.LoadPersonById(PeopleId);
				if (p == null)
					throw new Exception("no person");
				var c = p.PostUnattendedContribution(Db, Amount, FundId, desc);
			    DateTime dt;
			    if (date.DateTryParse(out dt))
			        c.ContributionDate = dt;
                if (type.HasValue)
    			    c.ContributionTypeId = type.Value;
                Db.SubmitChanges();
				return @"<PostContribution status=""ok"" id=""{0}"" />".Fmt(c.ContributionId);
			}
			catch (Exception ex)
			{
				return @"<PostContribution status=""error"">" + ex.Message + "</PostContribution>";
			}
		}
		[Serializable]
		public class FamilyContributions
		{
			[XmlAttribute]
			public string status { get; set; }
			public List<Contributor> Contributors { get; set; }
		}
		[Serializable]
		public class Contributor
		{
			[XmlAttribute]
			public string Type { get; set; }
			public string Name { get; set; }
			public List<Contribution> Contributions { get; set; }
		}
		[Serializable]
		public class Contribution
		{
			public string Name { get; set; }
			public string Date { get; set; }
			public decimal Amount { get; set; }
			public string Fund { get; set; }
			public string Description { get; set; }
			public string CheckNo { get; set; }
		}
	    public string Contributions(int PeopleId, int Year)
		{
		    try
		    {
				var p = Db.LoadPersonById(PeopleId);
				if (p == null)
					throw new Exception("no person");
				if (p.PositionInFamilyId != PositionInFamily.PrimaryAdult)
					throw new Exception("not a primary adult");
			    var frdt = new DateTime(Year, 1, 1);
			    var todt = new DateTime(Year, 12, 31);
			    var f = GetFamilyContributions(frdt, todt, p);
			    return SerializeContributions(f);
		    }
		    catch (Exception ex)
		    {
				return @"<PostContribution status=""error"">" + ex.Message + "</PostContribution>";
		    }
		}

	    private FamilyContributions GetFamilyContributions(DateTime frdt, DateTime todt, Person p)
	    {
		    var f = new FamilyContributions
		    {
			    status = "ok",
			    Contributors = (from ci in contributors(Db, frdt, todt, 0, 0, p.FamilyId, noaddressok: true, useMinAmt: false)
			                    select new Contributor
			                    {
				                    Name = ci.Name,
									Type = ci.Joint ? "Joint" : "Individual",
				                    Contributions = (from c in contributions(Db, ci, frdt, todt)
				                                     select new Contribution
				                                     {
					                                     Amount = c.ContributionAmount.Value,
					                                     Date = c.ContributionDate.FormatDate(),
					                                     Description = c.Description,
														 CheckNo = c.CheckNo,
					                                     Fund = c.Fund,
					                                     Name = c.Name,
				                                     }).ToList()
			                    }).ToList()
		    };
		    return f;
	    }

	    private static string SerializeContributions(FamilyContributions f)
	    {
		    var sw = new StringWriter();
		    var xs = new XmlSerializer(typeof (FamilyContributions));
		    var ns = new XmlSerializerNamespaces();
		    ns.Add("", "");
		    xs.Serialize(sw, f, ns);
		    return sw.ToString();
	    }
        public static int[] ReturnedReversedTypes = new int[] 
        { 
            ContributionTypeCode.ReturnedCheck, 
            ContributionTypeCode.Reversed 
        };

        public static IEnumerable<ContributorInfo> contributors(CMSDataContext Db, DateTime fromDate, DateTime toDate, int PeopleId, int? SpouseId, int FamilyId, bool noaddressok, bool useMinAmt, string startswith = null)
        {
            var MinAmt = Db.Setting("MinContributionAmount", "5").ToDecimal();
            if (!useMinAmt)
                MinAmt = 0;
            var q11 = from p in Db.Contributors(fromDate, toDate, PeopleId, SpouseId, FamilyId, noaddressok)
                      let option = (p.ContributionOptionsId ?? 0) == 0 ? (p.SpouseId > 0 ? 2 : 1) : p.ContributionOptionsId
                      let option2 = (p.SpouseContributionOptionsId ?? 0) == 0 ? (p.SpouseId > 0 ? 2 : 1) : p.SpouseContributionOptionsId
                      let name = (option == 1 ?
                                 (p.Title != null ? p.Title + " " + p.Name : p.Name)
                                 : (p.SpouseId == null ?
                                     (p.Title != null ? p.Title + " " + p.Name : p.Name)
                                     : (p.HohFlag == 1 ?
                                         ((p.Title != null && p.Title != "") ?
                                             p.Title + " and Mrs. " + p.Name
                                             : "Mr. and Mrs. " + p.Name)
                                         : ((p.SpouseTitle != null && p.SpouseTitle != "") ?
                                             p.SpouseTitle + " and Mrs. " + p.SpouseName
                                             : "Mr. and Mrs. " + p.SpouseName))))
                           + ((p.Suffix == null || p.Suffix == "") ? "" : ", " + p.Suffix)
                      where option != 9 || noaddressok
                      where startswith == null || p.LastName.StartsWith(startswith)
#if DEBUG2
					  where p.FamilyId == 17371
#endif
                      where (option == 1 && p.Amount > MinAmt) || (option == 2 && p.HohFlag == 1 && (p.Amount + p.SpouseAmount) > MinAmt)
                      orderby p.FamilyId, p.PositionInFamilyId, p.HohFlag, p.Age
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
                          Joint = option == 2,
                          CampusId = p.CampusId,
                      };

            return q11;
        }

        public static IEnumerable<ContributionInfo> contributions(CMSDataContext Db, ContributorInfo ci, DateTime fromDate, DateTime toDate)
        {
            var q = from c in Db.Contributions
                    where !ReturnedReversedTypes.Contains(c.ContributionTypeId)
                    where c.ContributionTypeId != ContributionTypeCode.BrokeredProperty
                    where c.ContributionStatusId == ContributionStatusCode.Recorded
                    where c.ContributionDate >= fromDate && c.ContributionDate.Value.Date <= toDate
                    where c.PeopleId == ci.PeopleId || (ci.Joint && c.PeopleId == ci.SpouseID)
					where (c.ContributionFund.NonTaxDeductible ?? false) == false
                    where c.PledgeFlag == false
                    orderby c.ContributionDate
                    select new ContributionInfo
                    {
                        ContributionAmount = c.ContributionAmount,
                        ContributionDate = c.ContributionDate,
                        Fund = c.ContributionFund.FundName,
						CheckNo = c.CheckNo,
						Name = c.Person.Name
                    };

            return q;
        }

        public static IEnumerable<PledgeSummaryInfo> pledges(CMSDataContext Db, ContributorInfo ci, DateTime toDate)
        {
            var PledgeExcludes = new int[] 
            { 
                ContributionTypeCode.BrokeredProperty, 
                ContributionTypeCode.GraveSite, 
                ContributionTypeCode.Reversed 
            };

			var showPledgeIfMet = Db.Setting("ShowPledgeIfMet", "true").ToBool();

            var qp = from p in Db.Contributions
                     where p.PeopleId == ci.PeopleId || (ci.Joint && p.PeopleId == ci.SpouseID)
                     where p.PledgeFlag && p.ContributionTypeId == ContributionTypeCode.Pledge
                     where p.ContributionStatusId.Value != ContributionStatusCode.Reversed
                     where p.ContributionFund.FundStatusId == 1 // active
                     where p.ContributionDate <= toDate
                     group p by p.FundId into g
                     select new { FundId = g.Key, Fund = g.First().ContributionFund.FundName, Total = g.Sum(p => p.ContributionAmount) };
            var qc = from c in Db.Contributions
                     where !PledgeExcludes.Contains(c.ContributionTypeId)
                     where c.PeopleId == ci.PeopleId || (ci.Joint && c.PeopleId == ci.SpouseID)
                     where !c.PledgeFlag
                     where c.ContributionStatusId != ContributionStatusCode.Reversed
                     where c.ContributionDate <= toDate
                     group c by c.FundId into g
                     select new { FundId = g.Key, Total = g.Sum(c => c.ContributionAmount) };
            var q = from p in qp
                    join c in qc on p.FundId equals c.FundId into items
                    from c in items.DefaultIfEmpty()
					where (p.Total ?? 0) > (c == null ? 0 : c.Total ?? 0) || showPledgeIfMet
                    orderby p.Fund descending
                    select new PledgeSummaryInfo
                    {
                        Fund = p.Fund,
                        ContributionAmount = c.Total,
                        PledgeAmount = p.Total
                    };
            return q;
        }

        public static IEnumerable<ContributionInfo> quarterlySummary(CMSDataContext Db, ContributorInfo ci, DateTime fromDate, DateTime toDate)
        {
            var q = from c in Db.Contributions
                    where c.ContributionTypeId == ContributionTypeCode.CheckCash
                    where c.ContributionStatusId == ContributionStatusCode.Recorded
                    where c.ContributionDate >= fromDate
                    where c.ContributionDate <= toDate
                    where c.PeopleId == ci.PeopleId || (ci.Joint && c.PeopleId == ci.SpouseID)
                    where c.PledgeFlag == false
					where (c.ContributionFund.NonTaxDeductible ?? false) == false
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
    public class PledgeSummaryInfo
    {
        public string Fund { get; set; }
        public decimal? ContributionAmount { get; set; }
        public decimal? PledgeAmount { get; set; }
        public string Description { get; set; }
    }
    public class ContributorInfo
    {
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public int PeopleId { get; set; }
        public int? SpouseID { get; set; }
        public int FamilyId { get; set; }
        public DateTime? DeacesedDate { get; set; }
        public string CityStateZip { get { return UtilityExtensions.Util.FormatCSZ4(City, State, Zip); } }
        public int hohInd { get; set; }
        public int FamilyPositionId { get; set; }
        public int? Age { get; set; }
        public bool Joint { get; set; }
        public int? CampusId { get; set; }
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
        public string CheckNo { get; set; }
        public string Description { get; set; }
        public bool NotIncluded
        {
            get
            {
                if (!StatusId.HasValue)
                    return true;
                return StatusId.Value != (int)ContributionStatusCode.Recorded
                    || APIContribution.ReturnedReversedTypes.Contains(ContributionTypeId.Value);
            }
        }
    }
}
