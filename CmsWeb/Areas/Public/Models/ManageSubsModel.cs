using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using System.Text;
using UtilityExtensions;
using System.Web.Mvc;
using System.Xml.Linq;
using CmsData.Codes;

namespace CmsWeb.Models
{
    public class ManageSubsModel
    {
        public int pid { get; set; }
        public int divid { get; set; }
        private Person _Person;
        public Person person
        {
            get
            {
                if (_Person == null)
                    _Person = DbUtil.Db.LoadPersonById(pid);
                return _Person;
            }
        }
        private Division _division;
        public Division Division
        {
            get
            {
                if (_division == null)
                    _division = DbUtil.Db.Divisions.Single(d => d.Id == divid);
                return _division;
            }
        }
        public ManageSubsModel()
        {

        }
        public ManageSubsModel(int pid, int divid)
        {
            this.pid = pid;
            this.divid = divid;
        }
        public int[] Subscribe { get; set; }
        public class OrgSub
        {
            public int OrgId { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public bool Checked { get; set; }
            public string CHECKED
            {
                get
                {
                    return Checked ? "checked=\"checked\"" : "";
                }
            }
        }
        public IEnumerable<OrgSub> FetchSubs()
        {
            var q = from o in DbUtil.Db.Organizations
                    where o.DivOrgs.Any(dd => dd.DivId == divid)
                    where o.RegistrationTypeId == RegistrationEnum.ManageSubscriptions
                    select new OrgSub
                    {
                        OrgId = o.OrganizationId,
                        Name = o.OrganizationName,
                        Description = o.Description,
                        Checked = o.OrganizationMembers.Any(om => om.PeopleId == pid)
                    };
            return q;
        }
        private string _summary;
        public string Summary
        {
            get
            {
                if (!_summary.HasValue())
                {
                    var q = from om in DbUtil.Db.OrganizationMembers
                            where om.Organization.DivOrgs.Any(dd => dd.DivId == divid)
                            where om.Organization.RegistrationTypeId == RegistrationEnum.ManageSubscriptions
                            where om.PeopleId == pid
                            select new { om.Organization.OrganizationName, om.Organization.Description };
                    var sb = new StringBuilder();
                    foreach (var s in q)
                        sb.AppendFormat("<p><b>{0}</b><br/>{1}</p>\n",
                            s.OrganizationName, s.Description);
                    _summary = Util.PickFirst(sb.ToString(), "<p>no subscriptions</p>");
                }
                return _summary;
            }
        }
        public void UpdateSubscriptions()
        {
            var q = from om in DbUtil.Db.OrganizationMembers
                    where om.Organization.DivOrgs.Any(dd => dd.DivId == divid)
                    where om.Organization.RegistrationTypeId == RegistrationEnum.ManageSubscriptions
                    where om.PeopleId == pid
                    select om;
            var current = q.ToList();

            if (Subscribe == null)
                Subscribe = new int[] { };

            var drops = from om in current
                        join id in Subscribe on om.OrganizationId equals id into j
                        from id in j.DefaultIfEmpty()
                        where id == 0
                        select om;

            var joins = from id in Subscribe
                        join om in current on id equals om.OrganizationId into j
                        from om in j.DefaultIfEmpty()
                        where om == null
                        select id;

            foreach (var om in drops)
            {
                om.Drop(DbUtil.Db, addToHistory:true);
                DbUtil.Db.SubmitChanges();
            }
            foreach (var id in joins)
            {
                OrganizationMember.InsertOrgMembers(DbUtil.Db, 
                    id, pid, 220, DateTime.Now, null, false);
                DbUtil.Db.SubmitChanges();
            }
        }
    }
}
