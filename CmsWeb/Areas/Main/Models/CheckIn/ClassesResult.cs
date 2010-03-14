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
        public ClassesResult(Person p, int thisday, int campusid, int page)
        {
            this.thisday = thisday;
            this.campusid = campusid;
            this.p = p;
            this.page = page;
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
                var bd = DateTime.Parse(p.DOB);
                var q = from o in DbUtil.Db.Organizations
                        let Hour1 = DbUtil.Db.GetTodaysMeetingHour(o.OrganizationId, thisday)
                        where bd <= o.BirthDayEnd || o.BirthDayEnd == null
                        where bd >= o.BirthDayStart || o.BirthDayStart == null
                        where o.CanSelfCheckin == true
                        where o.CampusId == campusid
                        where o.OrganizationStatusId == (int)CmsData.Organization.OrgStatusCode.Active
                        where Hour1 != null
                        orderby o.SchedTime.Value.TimeOfDay, o.BirthDayStart, o.Division.Name, o.OrganizationName
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
                    w.WriteAttributeString("display",
                        o.MeetingTime.Value.ToShortTimeString() + " - " + o.Division.Name + " - " + o.FullName);
                    w.WriteEndElement();
                }
                w.WriteEndElement();
            }
        }
    }
}