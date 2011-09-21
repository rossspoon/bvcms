using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilityExtensions;
using System.Text.RegularExpressions;
using System.Data.Linq;
using System.Xml.Linq;
using System.Data.Linq.SqlClient;
using IronPython.Hosting;
using System.IO;
using CmsData.Codes;
using System.Web;

namespace CmsData
{
    public class QueryFunctions
    {
        private CMSDataContext Db;

        public QueryFunctions()
        {
            Db = new CMSDataContext("Data Source=.;Initial Catalog=CMS_bellevue;Integrated Security=True");
        }
        public QueryFunctions(CMSDataContext Db)
        {
            this.Db = Db;
        }
        public string VitalStats()
        {
            var script = DbUtil.Content("VitalStats");
            if (script == null)
                return "no VitalStats script";
#if DEBUG2
            var options = new Dictionary<string, object>();
            options["Debug"] = true;
            var engine = Python.CreateEngine(options);
            var paths = engine.GetSearchPaths();
            paths.Add(path);
            engine.SetSearchPaths(paths);
            var sc = engine.CreateScriptSourceFromFile(HttpContext.Current.Server.MapPath("/MembershipAutomation2.py"));
#else
            var engine = Python.CreateEngine();
            var sc = engine.CreateScriptSourceFromString(script.Body);
#endif

            try
            {
                var code = sc.Compile();
                var scope = engine.CreateScope();
                code.Execute(scope);

                dynamic VitalStats = scope.GetVariable("VitalStats");
                dynamic m = VitalStats();
                return m.Run(this);
            }
            catch (Exception ex)
            {
                return "VitalStats script error: " + ex.Message;
            }
        }
        public int MeetingCount(int days, int progid, int divid, int orgid)
        {
            var dt = DateTime.Now.AddDays(-days);
            var q = from m in Db.Meetings
                    where m.MeetingDate >= dt
                    where orgid == 0 || m.OrganizationId == orgid
                    where divid == 0 || m.Organization.DivOrgs.Any(t => t.DivId == divid)
                    where progid == 0 || m.Organization.DivOrgs.Any(t => t.Division.ProgDivs.Any(d => d.ProgId == progid))
                    select m;
            return q.Count();
        }
        public int NumPresent(int days, int progid, int divid, int orgid)
        {
            var dt = DateTime.Now.AddDays(-days);
            var q = from m in Db.Meetings
                    where m.MeetingDate >= dt
                    where orgid == 0 || m.OrganizationId == orgid
                    where divid == 0 || m.Organization.DivOrgs.Any(t => t.DivId == divid)
                    where progid == 0 || m.Organization.DivOrgs.Any(t => t.Division.ProgDivs.Any(d => d.ProgId == progid))
                    select m;
            return q.Sum(mm => mm.NumPresent);
        }

        public int RegistrationCount(int days, int progid, int divid, int orgid)
        {
            var dt = DateTime.Now.AddDays(-days);
            var q = from m in Db.OrganizationMembers
                    where m.EnrollmentDate >= dt
                    where m.Organization.RegistrationTypeId > 0
                    where orgid == 0 || m.OrganizationId == orgid
                    where divid == 0 || m.Organization.DivOrgs.Any(t => t.DivId == divid)
                    where progid == 0 || m.Organization.DivOrgs.Any(t => t.Division.ProgDivs.Any(d => d.ProgId == progid))
                    select m;
            return q.Count();
        }
        public decimal ContributionTotals(int days1, int days2, int fundid)
        {
            var dt1 = DateTime.Now.AddDays(-days1);
            var dt2 = DateTime.Now.AddDays(-days2);
            var typs = new int[] { 6, 7 };
            var q = from c in Db.Contributions
                    where c.ContributionDate >= dt1
                    where days2 == 0 || c.ContributionDate <= dt2
                    where c.PledgeFlag == false
                    where fundid == 0 || c.FundId == fundid
                    where !typs.Contains(c.ContributionTypeId)
                    select c;
            return q.Sum(c => c.ContributionAmount) ?? 0;
        }

        public int ContributionCount(int days1, int days2, int fundid)
        {
            var dt1 = DateTime.Now.AddDays(-days1);
            var dt2 = DateTime.Now.AddDays(-days2);
            var typs = new int[] { 6, 7 };
            var q = from c in Db.Contributions
                    where c.ContributionDate >= dt1
                    where days2 == 0 || c.ContributionDate <= dt2
                    where c.PledgeFlag == false
                    where c.ContributionAmount > 0
                    where fundid == 0 || c.FundId == fundid
                    where !typs.Contains(c.ContributionTypeId)
                    select c;
            return q.Count();
        }

        public int ContributionCount(int days, int fundid)
        {
            var dt = DateTime.Now.AddDays(-days);
            var typs = new int[] { 6, 7 };
            var q = from c in Db.Contributions
                    where c.ContributionDate >= dt
                    where c.PledgeFlag == false
                    where c.ContributionAmount > 0
                    where fundid == 0 || c.FundId == fundid
                    where !typs.Contains(c.ContributionTypeId)
                    select c;
            return q.Count();
        }
        public int QueryCount(string s)
        {
            var qB = Db.QueryBuilderClauses.FirstOrDefault(c => c.Description == s);
            var q = Db.People.Where(qB.Predicate(Db));
            return q.Count();
        }
    }
}
