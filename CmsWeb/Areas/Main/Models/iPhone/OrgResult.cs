using System;
using System.Collections.Generic;
using System.Xml;
using System.Web.Mvc;
using System.Xml.Linq;
using UtilityExtensions;
using System.Linq;
using CmsData;

namespace CmsWeb.Models.iPhone
{
    public class OrgResult : ActionResult
    {
        private int? pid;
        public OrgResult(int? pid)
        {
            this.pid = pid;
        }
        private IEnumerable<OrgInfo> OrgList()
        {
            var q = from o in DbUtil.Db.Organizations
                    where o.OrganizationMembers.Any(om => om.PeopleId == pid)
                    where o.CanSelfCheckin == true
                    select new OrgInfo
                    {
                        OrgId = o.OrganizationId,
                        OrgName = o.OrganizationName,
                        MeetingTime = o.SchedTime.Value + o.SchedTime.Value.TimeOfDay,
                        MeetingDay = o.SchedDay.Value
                    };
            return q;
        }
        public class OrgInfo
        {
            public int OrgId { get; set; }
            public string OrgName { get; set; }
            public DateTime? MeetingTime { get; set; }
            public int? MeetingDay { get; set; }
        }
        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "text/xml";
            var settings = new XmlWriterSettings();
            settings.Encoding = new System.Text.UTF8Encoding(false);

            using (var w = XmlWriter.Create(context.HttpContext.Response.OutputStream, settings))
            {
                w.WriteStartElement("Organizations");

                foreach (var o in OrgList())
                {
                    w.WriteStartElement("Organization");
                    w.WriteAttributeString("Id", o.OrgId.ToString());
                    w.WriteAttributeString("Name", o.OrgName);
                    w.WriteAttributeString("MeetingDate", NewMeetingDate(o.MeetingDay).ToShortDateString());
                    w.WriteAttributeString("MeetingTime", o.MeetingTime.Value.ToShortTimeString());
                    w.WriteEndElement();
                }
                w.WriteEndElement();
            }
        }
        public DateTime NewMeetingDate(int? day)
        {
            var d = Util.Now.Date;
            d = d.AddDays(-(int)d.DayOfWeek); // prev sunday
            d = d.AddDays(day ?? 0);
            if (d > Util.Now.Date)
                d = d.AddDays(-7);
            return d;
        }
    }
}