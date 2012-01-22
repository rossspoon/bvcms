using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.Linq;
using UtilityExtensions;
using IronPython.Hosting;
using System.Data.Linq;

namespace CmsData.API
{
    public class APIOrganization
    {
        private CMSDataContext Db;

        public APIOrganization()
        {
            Db = new CMSDataContext("Data Source=.;Initial Catalog=CMS_bellevue;Integrated Security=True");
        }
        public APIOrganization(CMSDataContext Db)
        {
            this.Db = Db;
        }
        public string OrganizationsForDiv(int divid)
        {
            var q = from o in Db.Organizations
                    where o.DivOrgs.Any(dd => dd.DivId == divid)
                    where o.OrganizationStatusId == CmsData.Codes.OrgStatusCode.Active
                    let leader = Db.People.SingleOrDefault(ll => ll.PeopleId == o.LeaderId)
                    select new
                    {
                        o.OrganizationId,
                        o.OrganizationName,
                        o.Location,
                        o.Description,
                        o.LeaderName,
                        o.LeaderId,
                        Email = leader != null ? leader.EmailAddress : "",
                        IsParent = o.ChildOrgs.Count() > 0,
                        NumMembers = o.OrganizationMembers.Count(om => om.Pending != true && om.MemberTypeId != CmsData.Codes.MemberTypeCode.InActive)
                    };
            var w = new APIWriter();

            w.Start("Organizations");
            foreach (var o in q)
            {
                w.Start("Organization");
                w.Attr("Id", o.OrganizationId);
                w.Attr("Name", o.OrganizationName);
                w.Attr("NumMembers", o.NumMembers);
                if (o.IsParent)
                    w.Attr("IsParent", o.IsParent);
                w.Attr("Location", o.Location);
                w.Attr("Leader", o.LeaderName);
                w.Attr("LeaderId", o.LeaderId);
                w.Attr("Email", o.Email);
                w.End();
            }
            w.End();
            return w.ToString();
        }
        public class OrgMemberInfo
        {
            public OrganizationMember member { get; set; }
            public Person person { get; set; }
            public IEnumerable<string> tags { get; set; }
        }
        public List<OrgMemberInfo> OrgMembersData(int orgid)
        {
            // load data, do in memory joins
            var qm = (from m in Db.OrganizationMembers
                      where m.OrganizationId == orgid
                      where m.MemberTypeId != Codes.MemberTypeCode.InActive
                      select new { m, m.Person }).ToList();
            var mt = (from m in Db.OrgMemMemTags
                      where m.OrganizationMember.OrganizationId == orgid
                      where m.OrganizationMember.MemberTypeId != Codes.MemberTypeCode.InActive
                      select new { m.OrganizationMember.PeopleId, m.MemberTag.Name }).ToList();
            var q = from i in qm
                    select new OrgMemberInfo
                    {
                        member = i.m,
                        person = i.Person,
                        tags = from t in mt
                               where t.PeopleId == i.m.PeopleId
                               select t.Name,
                    };
            return q.ToList();
        }
        public string OrgMembersPython(int orgid)
        {
            var list = OrgMembersData(orgid);
            var script = Db.Content("API-OrgMembers");
            if (script == null)
            {
                script = new Content();
                script.Body = @"
from System import *
from System.Text import *

class OrgMembers(object):

	def Run(self, m, w, q):
		w.Start('OrgMembers')
		for i in q:
			w.Start('Member')
			w.Attr('PeopleId', i.member.PeopleId)
			w.Attr('Name', i.member.Person.Name)
			w.Attr('PreferredName', i.member.Person.PreferredName)
			w.Attr('LastName', i.member.Person.LastName)
			w.Attr('Email', i.member.Person.EmailAddress)
			w.Attr('Enrolled', i.member.EnrollmentDate)
			w.Attr('MemberType', i.member.MemberType.Description)
			for t in i.tags:
				w.Add('Group', t)
			w.End()
		w.End()
		return w.ToString()
";
            }
            if (script == null)
                return "<login error=\"no API-OrgMembers script\" />";
            var engine = Python.CreateEngine();
            var sc = engine.CreateScriptSourceFromString(script.Body);
            try
            {
                var code = sc.Compile();
                var scope = engine.CreateScope();
                code.Execute(scope);

                dynamic LoginInfo = scope.GetVariable("OrgMembers");
                dynamic m = LoginInfo();
                var w = new APIWriter();
                return m.Run(this, w, list);
            }
            catch (Exception ex)
            {
                return "<login error=\"API-OrgMembers script error: {0}\" />".Fmt(ex.Message);
            }
        }

        public string OrgMembers(int orgid, string search)
        {
            search = search ?? "";
            var nosearch = !search.HasValue();
            var qm = from m in Db.OrganizationMembers
                     where m.OrganizationId == orgid
                     where nosearch || m.Person.Name2.StartsWith(search)
                     select new
                     {
                         m.PeopleId,
                         First = m.Person.PreferredName,
                         Last = m.Person.LastName,
                         m.Person.EmailAddress,
                         m.EnrollmentDate,
                         MemberType = m.MemberType.Description,
                         IsLeaderType = (m.MemberType.AttendanceTypeId ?? 0) == CmsData.Codes.AttendTypeCode.Leader,
                     };
            var mt = from m in Db.OrgMemMemTags
                     where m.OrganizationMember.OrganizationId == orgid
                     where m.OrganizationMember.MemberTypeId != Codes.MemberTypeCode.InActive
                     select new
                     {
                         m.OrganizationMember.PeopleId,
                         m.MemberTag.Name
                     };
            var mtags = mt.ToList();

            var w = new APIWriter();
            w.Start("OrgMembers");
            foreach (var m in qm.ToList())
            {
                w.Start("Member");
                w.Attr("PreferredName", m.First);
                w.Attr("LastName", m.Last);
                w.Attr("Email", m.EmailAddress);
                w.Attr("Enrolled", m.EnrollmentDate);
                w.Attr("MemberType", m.MemberType);
                if (m.IsLeaderType)
                    w.Attr("IsLeader", m.IsLeaderType);
                var qt = from t in mtags
                         where t.PeopleId == m.PeopleId
                         select t.Name;
                foreach (var group in qt)
                    w.Add("Group", group);
                w.End();
            }
            w.End();
            return w.ToString();
        }
        public string UpdateOrgMember(int orgid, int peopleid, int? MemberType, DateTime? EnrollDate, string InactiveDate)
        {
            try
            {
                var om = Db.OrganizationMembers.Single(mm =>
                    mm.OrganizationId == orgid
                    && mm.PeopleId == peopleid);

                if (MemberType.HasValue)
                    om.MemberTypeId = MemberType.Value;
                if (EnrollDate.HasValue)
                    om.EnrollmentDate = EnrollDate;

                var d = InactiveDate.ToDate();
                if (d.HasValue)
                    om.InactiveDate = d;
                else if (InactiveDate == "null")
                    om.InactiveDate = null;

                Db.SubmitChanges();

                return "ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string DeleteExtraValue(int orgid, string field)
        {
            try
            {
                var q = from v in Db.OrganizationExtras
                        where v.Field == field
                        where v.OrganizationId == orgid
                        select v;
                Db.OrganizationExtras.DeleteAllOnSubmit(q);
                Db.SubmitChanges();
                return "ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string ExtraValues(int orgid, string fields)
        {
            try
            {
                var a = (fields ?? "").Split(',');
                var nofields = !fields.HasValue();
                var q = from v in Db.OrganizationExtras
                        where nofields || a.Contains(v.Field)
                        where v.OrganizationId == orgid
                        select v;
                var w = new APIWriter();
                w.Start("ExtraOrgValues");
                w.Attr("Id", orgid);
                foreach (var v in q)
                    w.Add(v.Field, v.Data);
                w.End();
                return w.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string AddEditExtraValue(int orgid, string field, string value)
        {
            try
            {
                var q = from v in Db.OrganizationExtras
                        where v.Field == field
                        where v.OrganizationId == orgid
                        select v;
                var ev = q.SingleOrDefault();
                if (ev == null)
                {
                    ev = new OrganizationExtra
                    {
                        OrganizationId = orgid,
                        Field = field,
                    };
                    Db.OrganizationExtras.InsertOnSubmit(ev);
                }
                ev.Data = value;
                Db.SubmitChanges();
                return "ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
