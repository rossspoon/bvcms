using System;
using System.Collections.Generic;
using System.Xml;
using System.Web.Mvc;
using System.Xml.Linq;
using UtilityExtensions;
using System.Linq;
using CmsData;

namespace CMSWeb.Models
{
    public class ClassesResult : ActionResult
    {
        private int thisday;
        private int campusid;
        private int page;
        Person p;
        public bool noagecheck { get; set; }
        public ClassesResult(Person p, int thisday, int campusid, int page, bool noagecheck)
        {
            this.thisday = thisday;
            this.campusid = campusid;
            this.p = p;
            this.page = page;
            this.noagecheck = noagecheck;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "text/xml";
            var settings = new XmlWriterSettings();
            settings.Encoding = new System.Text.UTF8Encoding(false);
            using (var w = XmlWriter.Create(context.HttpContext.Response.OutputStream, settings))
            {
                w.WriteStartElement("Results");
                w.WriteAttributeString("pid", p.PeopleId.ToString());
                w.WriteAttributeString("fid", p.FamilyId.ToString());
                var bd = p.BirthDate;
                var q = from o in DbUtil.Db.Organizations
                        let Hour1 = DbUtil.Db.GetTodaysMeetingHour(o.OrganizationId, thisday)
                        let bdaystart = o.BirthDayStart ?? DateTime.MaxValue
                        where bd <= o.BirthDayEnd || o.BirthDayEnd == null || noagecheck
                        where bd >= o.BirthDayStart || o.BirthDayStart == null || noagecheck
                        where o.CanSelfCheckin == true
                        where (o.ClassFilled ?? false) == false
                        where o.CampusId == campusid || campusid == 0
                        where o.OrganizationStatusId == (int)CmsData.Organization.OrgStatusCode.Active
                        where Hour1 != null
                        orderby o.SchedTime.Value.TimeOfDay, bdaystart, o.OrganizationName
                        select o;

                var count = q.Count();
                const int INT_PageSize = 10;
                var startrow = (page - 1) * INT_PageSize;
                if (count > startrow + INT_PageSize)
                    w.WriteAttributeString("next", (page + 1).ToString());
                else
                    w.WriteAttributeString("next", "");
                if (page > 1)
                    w.WriteAttributeString("prev", (page - 1).ToString());
                else
                    w.WriteAttributeString("prev", "");

                foreach (var o in q.Skip(startrow).Take(INT_PageSize))
                {
                    w.WriteStartElement("class");
                    w.WriteAttributeString("orgid", o.OrganizationId.ToString());
                    var loc = o.Location;
                    if (loc.HasValue())
                        loc = ", " + loc;
                    var leader = o.LeaderName;
                    if (leader.HasValue())
                        leader = ":" + leader;
                    var bdays = " [{0:M/d/yy}-{1:M/d/yy}]".Fmt(o.BirthDayStart, o.BirthDayEnd);
                    if (bdays == " [-]")
                        bdays = null;
                    string display = "{0:hh:mm tt} {1}{2}{3}{4}"
                        .Fmt(o.MeetingTime, o.OrganizationName, leader, loc, bdays); 
                    w.WriteAttributeString("display", display);
                    w.WriteEndElement();
                }
                w.WriteEndElement();
            }
        }
    }
}