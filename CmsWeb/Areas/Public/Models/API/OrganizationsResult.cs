using System;
using System.Collections.Generic;
using System.Xml;
using System.Web.Mvc;
using System.Xml.Linq;
using UtilityExtensions;
using System.Linq;
using CmsData;

namespace CmsWeb.Models
{
    public class OrganizationsResult : ActionResult
    {
        private XmlWriter w;
        private int divid;
        public OrganizationsResult(int DivId)
        {
            divid = DivId;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "text/xml";
            var settings = new XmlWriterSettings();
            settings.Encoding = new System.Text.UTF8Encoding(false);
            using (w = XmlWriter.Create(context.HttpContext.Response.OutputStream, settings))
            {
                var q = from o in DbUtil.Db.Organizations
                        where o.DivOrgs.Any(dd => dd.DivId == divid)
                        select new
                        {
                            o,
                            meetings = from m in o.Meetings
                                       where m.MeetingDate > Util.Now
                                       select m
                        };
                w.WriteStartElement("Organizations");
                foreach (var i in q)
                    WriteOrg(i.o, i.meetings);
                w.WriteEndElement();
            }
        }
        private void WriteOrg(Organization o, IEnumerable<Meeting> meetings)
        {
            w.WriteStartElement("Organization");
            Add("Id", o.OrganizationId);
            Add("Name", o.OrganizationName);
            Add("Leader", o.LeaderName);
            Add("NumMembers", o.MemberCount ?? 0);
            w.WriteStartElement("FutureMeetings");
            foreach (var m in meetings)
            {
                w.WriteStartElement("Meeting");
                Add("DateTime", m.MeetingDate.Value);
                Add("NumAttendees", m.Attends.Where(aa => aa.AttendanceFlag == true).Count());
                w.WriteEndElement();
            }
            w.WriteEndElement();
            w.WriteEndElement();
        }
        private void Add(string attr, int i)
        {
            Add(attr, i.ToString());
        }
        private void Add(string attr, DateTime t)
        {
            Add(attr, t.ToString());
        }
        private void Add(string attr, string s)
        {
            w.WriteAttributeString(attr, s);
        }
    }
}