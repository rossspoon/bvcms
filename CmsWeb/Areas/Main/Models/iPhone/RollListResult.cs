using System;
using System.Collections.Generic;
using System.Xml;
using System.Web.Mvc;
using System.Xml.Linq;
using UtilityExtensions;
using System.Linq;
using CmsData;
using CmsWeb.Areas.Main.Models.Report;

namespace CmsWeb.Models.iPhone
{
    public class RollListResult : ActionResult
    {
        int? MeetingId;
        int OrgId;
        DateTime MeetingDate;
        public RollListResult(CmsData.Meeting meeting)
        {
            MeetingId = meeting.MeetingId;
            OrgId = meeting.OrganizationId;
            MeetingDate = meeting.MeetingDate.Value;
        }
        public RollListResult(int orgid, DateTime dt)
        {
            var meeting = DbUtil.Db.Meetings.SingleOrDefault(mm => mm.MeetingDate == dt && mm.OrganizationId == orgid);
            if (meeting != null)
                MeetingId = meeting.MeetingId;
            OrgId = orgid;
            MeetingDate = dt;
        }
        private IEnumerable<AttendInfo> RollList()
        {
            var m = new RollsheetModel();
            var q = from a in DbUtil.Db.Attends
                    where a.MeetingId == MeetingId
                    where a.AttendanceFlag == true
                    select a;

            var q1 = from p in m.FetchOrgMembers(OrgId, null)
                     join pa in q on p.PeopleId equals pa.PeopleId into j
                     from pa in j.DefaultIfEmpty()
                     select new
                     {
                         p.PeopleId,
                         p.Name,
                         Attended = pa != null ? pa.AttendanceFlag : false,
                         Member = true
                     };
            var q2 = from p in m.FetchVisitors(OrgId, MeetingDate)
                     join pa in q on p.PeopleId equals pa.PeopleId into j
                     from pa in j.DefaultIfEmpty()
                     select new
                     {
                         p.PeopleId,
                         p.Name,
                         Attended = pa != null ? pa.AttendanceFlag : false,
                         Member = false
                     };
            var q3 = from p in q1.Union(q2)
                     select new AttendInfo
                     {
                         PeopleId = p.PeopleId,
                         Name = p.Name,
                         Attended = p.Attended,
                         Member = p.Member
                     };
            return q3;
        }
        public class AttendInfo
        {
            public int PeopleId { get; set; }
            public string Name { get; set; }
            public bool Attended { get; set; }
            public bool Member { get; set; }
        }
        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "text/xml";
            var settings = new XmlWriterSettings();
            settings.Encoding = new System.Text.UTF8Encoding(false);

            using (var w = XmlWriter.Create(context.HttpContext.Response.OutputStream, settings))
            {
                w.WriteStartElement("RollList");
                w.WriteAttributeString("MeetingId", MeetingId.ToString());

                foreach (var p in RollList())
                {
                    w.WriteStartElement("Person");
                    w.WriteAttributeString("Id", p.PeopleId.ToString());
                    w.WriteAttributeString("Name", p.Name);
                    w.WriteAttributeString("Attended", p.Attended.ToString());
                    w.WriteAttributeString("Member", p.Member.ToString());
                    w.WriteEndElement();
                }
                w.WriteEndElement();
            }
        }
    }
}