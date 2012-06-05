using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using System.Web.Mvc;
using UtilityExtensions;
using CmsWeb.Areas.Main.Models.Report;

namespace CmsWeb.Models
{
    public class MeetingModel
    {
        public CmsData.Meeting meeting;

        public bool showall { get; set; }
        public bool showregister { get; set; }
        public bool showregistered { get; set; }
        public bool sortbyname { get; set; }

        public MeetingModel(int id)
        {
            meeting = DbUtil.Db.Meetings.SingleOrDefault(m => m.MeetingId == id);
        }
        public IEnumerable<RollsheetModel.AttendInfo> Attends(bool sorted = false)
        {
            return RollsheetModel.RollList(meeting.MeetingId, meeting.OrganizationId, meeting.MeetingDate.Value, sorted);
        }
        public IEnumerable<RollsheetModel.AttendInfo> VisitAttends(bool sorted = false)
        {
            var q =  RollsheetModel.RollList(meeting.MeetingId, meeting.OrganizationId, meeting.MeetingDate.Value, sorted);
			return q.Where(vv => !vv.Member);
        }
        public string AttendCreditType()
        {
            if (meeting.AttendCredit == null)
                return "Every Meeting";
            return meeting.AttendCredit.Description;
        }
        public bool HasRegistered()
        {
            return meeting.Attends.Any(aa => aa.Registered == true);
        }
    }
}
