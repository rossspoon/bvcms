using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CmsData;
using System.Data.Linq;
using System.ComponentModel;
using UtilityExtensions;

namespace CMSWeb.Contributions.Reports
{
    public partial class PledgeReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var today = Util.Now.Date;
                ToDt.Text = today.ToShortDateString();
            }
        }

        protected void Run_Click(object sender, EventArgs e)
        {
            GridView1.Visible = true;
        }
    }
    public class PledgeInfo
    {
        public int FundId { get; set; }
        public string FundName { get; set; }
        public decimal? Pledged { get; set; }
        public decimal? ToPledge { get; set; }
        public decimal? NotToPledge { get; set; }
        public decimal? ToFund { get; set; }
    }
    [DataObject]
    public class PledgeReportODS
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<PledgeInfo> FetchPledgeData(DateTime dt)
        {
            var Db = DbUtil.Db;
            var PledgeExcludes = new int[] 
            { 
                (int)Contribution.TypeCode.BrokeredProperty, 
                (int)Contribution.TypeCode.GraveSite, 
                (int)Contribution.TypeCode.Reversed 
            };
            var qq = from p in Db.Contributions
                     where p.PledgeFlag && p.ContributionTypeId == (int)Contribution.TypeCode.Pledge
                     where p.ContributionStatusId.Value != (int)Contribution.StatusCode.Reversed
                     where p.ContributionFund.FundStatusId == 1 // active
                     where p.ContributionFund.FundPledgeFlag
                     where p.ContributionDate <= dt
                     group p by p.FundId into g
                     select new
                     {
                         FundId = g.Key,
                         FundName = g.First().ContributionFund.FundName,
                     };
            var lookup = qq.ToDictionary(k => k.FundId, k => k.FundName);
            var qp = from p in Db.Contributions
                     where p.PledgeFlag && p.ContributionTypeId == (int)Contribution.TypeCode.Pledge
                     where p.ContributionStatusId.Value != (int)Contribution.StatusCode.Reversed
                     where p.ContributionFund.FundStatusId == 1 // active
                     where p.ContributionFund.FundPledgeFlag
                     where p.ContributionDate <= dt
                     group p by p.FundId into g
                     select new
                     {
                         FundId = g.Key,
                         Total = g.Sum(c => c.ContributionAmount)
                     };
            var qcp = from c in Db.Contributions
                      where !PledgeExcludes.Contains(c.ContributionTypeId)
                      where !c.PledgeFlag
                      where c.Person.Contributions.Any(p => p.PledgeFlag && p.FundId == c.FundId)
                      where c.ContributionStatusId != (int)Contribution.StatusCode.Reversed
                      where c.ContributionFund.FundPledgeFlag
                      where c.ContributionDate <= dt
                      group c by c.FundId into g
                      select new
                      {
                          FundId = g.Key,
                          Total = g.Sum(c => c.ContributionAmount)
                      };
            var qc = from c in Db.Contributions
                     where !PledgeExcludes.Contains(c.ContributionTypeId)
                     where !c.PledgeFlag
                     where !c.Person.Contributions.Any(p => p.PledgeFlag && p.FundId == c.FundId)
                     where c.ContributionFund.FundPledgeFlag
                     where c.ContributionStatusId != (int)Contribution.StatusCode.Reversed
                     where c.ContributionDate <= dt
                     group c by c.FundId into g
                     select new
                     {
                         FundId = g.Key,
                         Total = g.Sum(c => c.ContributionAmount)
                     };
            var q = from tp in qp
                    join tcp in qcp on tp.FundId equals tcp.FundId into items
                    join tc in qc on tp.FundId equals tc.FundId into items2
                    from tcp in items.DefaultIfEmpty()
                    from tc in items2.DefaultIfEmpty()
                    where tp.Total > 0
                    select new PledgeInfo
                    {
                        FundId = tp.FundId,
                        FundName = lookup[tp.FundId],
                        Pledged = tp.Total,
                        ToPledge = tcp.Total,
                        NotToPledge = tc.Total,
                        ToFund = tcp.Total + tc.Total,
                    };
            return q;
        }
    }
}
