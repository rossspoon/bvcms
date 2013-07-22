using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Data.Linq;
using System.Web;
using CmsData;
using System.Web.Mvc;
using CmsData.Registration;
using CmsWeb.Code;
using CmsWeb.Models;
using CmsWeb.Models.OrganizationPage;
using UtilityExtensions;
using System.Text.RegularExpressions;
using CmsData.Codes;

namespace CmsWeb.Areas.Org.Models
{
    public class OrgMemberMoveModel
    {
        private int? _orgId;
        private int? _peopleId;
        private void Populate()
        {
            Pager = new PagerModel2(Count);
            var i = (from mm in DbUtil.Db.OrganizationMembers
                     where mm.OrganizationId == OrgId && mm.PeopleId == PeopleId
                     select new
                         {
                             mm,
                             mm.Person.Name,
                             mm.Organization.OrganizationName,
                         }).SingleOrDefault();
            Name = i.Name;
            OrgName = i.OrganizationName;
        }
        public PagerModel2 Pager { get; set; }
        public string orgsearch { get; set; }
        public int? OrgId
        {
            get { return _orgId; }
            set
            {
                _orgId = value;
                if(_peopleId.HasValue)
                    Populate();
            }
        }

        public int? PeopleId
        {
            get { return _peopleId; }
            set
            {
                _peopleId = value;
                if(_orgId.HasValue)
                    Populate();
            }
        }
        public string Name { get; set; }
        public string OrgName { get; set; }

        private int _count;
        public int Count()
        {
            if (query != null)
                _count = FetchOrgs().Count();
            return _count;
        }

        private IQueryable<Organization> query = null;
        private IQueryable<Organization> FetchOrgs()
        {
            query = from o in DbUtil.Db.Organizations
                    let org = DbUtil.Db.Organizations.Single(oo => oo.OrganizationId == OrgId)
                    where o.DivOrgs.Any(dd => org.DivOrgs.Any(oo => oo.DivId == dd.DivId))
                    where o.OrganizationId != OrgId
                    where o.OrganizationStatusId == OrgStatusCode.Active
                    where !orgsearch.HasValue() || o.OrganizationName.Contains(orgsearch)
                    select o;
            return query;
        }

        public IEnumerable<OrgMove> OrgMoveList()
        {
            if (query == null)
                query = FetchOrgs();
            var q = from o in query
                    orderby o.OrganizationName
                    select new OrgMove
                    {
                        OrgName = o.OrganizationName,
                        ToOrgId = o.OrganizationId,
                        PeopleId = PeopleId.Value,
                        FromOrgId = OrgId.Value,
                        Program = o.Division.Program.Name,
                        Division = o.Division.Name,
                        orgSchedule = o.OrgSchedules.First()
                    };
            return q.Skip(Pager.StartRow).Take(10);
        }
        public class OrgMove
        {
            public string OrgName { get; set; }
            public int FromOrgId { get; set; }
            public int PeopleId { get; set; }
            public int ToOrgId { get; set; }
            public string Program { get; set; }
            public string Division { get; set; }
            public OrgSchedule orgSchedule { get; set; }
            public string Tip
            {
                get
                {
                    var si = new ScheduleInfo(orgSchedule);
                    return "{0} &bull; {1} &bull; {2}, {3}".Fmt(Program, Division, si.DisplayDay, si.Time);
                }
            }
        }
    }
}
