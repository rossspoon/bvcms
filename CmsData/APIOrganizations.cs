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
    public partial class APIFunctions
    {
        public string Organizations(int divid)
        {
            var q = from o in Db.Organizations
                    where o.DivOrgs.Any(dd => dd.DivId == divid)
                    select new
                    {
                        o,
                        meetings = from m in o.Meetings
                                   where m.MeetingDate > Util.Now
                                   select m
                    };
            var settings = new XmlWriterSettings();
            settings.Encoding = new System.Text.UTF8Encoding(false);
            settings.Indent = true;
            var sb = new StringBuilder();
            var ms = new System.IO.StringWriter(sb);
            //using (var w = XmlWriter.Create(ms, settings))
            //{
            //    w.Start("Organizations");
            //    foreach (var i in q)
            //    {
            //        w.Start("Organization");
            //        w.Attr("Id", i.o.OrganizationId);
            //        w.Attr("Name", i.o.OrganizationName);
            //        w.Attr("Leader", i.o.LeaderName);
            //        w.Attr("NumMembers", i.o.MemberCount ?? 0);
            //        if (i.o.Description.HasValue())
            //            w.Add("Description", i.o.Description);

            //        foreach (var m in i.meetings)
            //        {
            //            w.Start("Meeting");
            //            w.Attr("DateTime", m.MeetingDate.Value);
            //            w.Attr("NumAttendees", m.Attends.Where(aa => aa.AttendanceFlag == true).Count());
            //            w.End();
            //        }
            //        w.End();
            //    }
            //    w.End();
            //}
            return sb.ToString();
        }
        public class OrgMemberInfo
        {
            public OrganizationMember member { get; set; }
            public Person person { get; set; }
            public IEnumerable<string> tags { get; set; }
            public IEnumerable<Attend> meetings { get; set; }
        }
        public List<OrgMemberInfo> OrgMembersData(int orgid)
        {
            // load data, do in memory joins
            var qm = (from m in Db.OrganizationMembers
                      where m.OrganizationId == orgid
                      where m.MemberTypeId != Codes.MemberTypeCode.InActive
                      select new { m, m.Person }).ToList();
            var qa = (from m in Db.Meetings
                      where m.OrganizationId == orgid
                      where m.MeetingDate > DateTime.Now
                      from a in m.Attends
                      where a.AttendanceFlag == true
                      where m.Organization.OrganizationMembers.Any(mm => mm.PeopleId == a.PeopleId)
                      select a).ToList();
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
                         meetings = from a in qa
                                    where a.PeopleId == i.m.PeopleId
                                    select a
                     };
            return q.ToList();
        }
        public string OrgMembers(int orgid)
        {
            var list = OrgMembersData(orgid);
            var script = Db.Content("API-OrgMembers");
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
/*
            w.Start("OrgMembers");
            foreach (var i in q)
            {
                w.Start("Member");
                w.Attr("PeopleId", i.member.PeopleId);
                w.Attr("Name", i.member.Person.Name);
                w.Attr("Email", i.member.Person.EmailAddress);
                w.Attr("Enrolled", i.member.EnrollmentDate);
                w.Attr("MemberType", i.member.MemberType.Description);
                foreach (var t in i.tags)
                    w.Add("Group", t);
                foreach (var a in i.meetings)
                    w.Add("Meeting", a.MeetingDate);
                w.End();
            }
            w.End();
            return w.ToString();
 */       }
    }
}
