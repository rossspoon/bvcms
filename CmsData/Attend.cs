using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Web;
using UtilityExtensions;
using System.IO;
using System.Net.Mail;
using System.Web.Configuration;
using System.Data.SqlClient;
using CmsData.Codes;

namespace CmsData
{
    public partial class Attend
    {
        public static int[] VisitAttendTypes = new int[] 
        { 
            AttendTypeCode.VisitingMember, 
            AttendTypeCode.RecentVisitor, 
            AttendTypeCode.NewVisitor 
        };
        public static string RecordAttendance(int PeopleId, int MeetingId, bool attended)
        {
            return RecordAttendance(DbUtil.Db, PeopleId, MeetingId, attended);
        }
        public static string RecordAttendance(CMSDataContext Db, int PeopleId, int MeetingId, bool attended)
        {
            int ntries = 6;
            while (true)
            {
                try
                {
                    return Db.RecordAttendance(MeetingId, PeopleId, attended);
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 1205)
                        if (--ntries > 0)
                        {
                            System.Threading.Thread.Sleep(500);
                            continue;
                        }
                    throw;
                }
            }
        }
        public static void MarkRegistered(CMSDataContext Db, int OrgId, int PeopleId, DateTime MeetingDate, bool registered)
        {
			if (registered == false)
			{
				var m = Db.Meetings.SingleOrDefault(mm => mm.OrganizationId == OrgId && mm.MeetingDate == MeetingDate);
				if (m == null)
					return;
			}
        	var mid = Db.CreateMeeting(OrgId, MeetingDate);
            MarkRegistered(Db, PeopleId, mid, registered);
        }
        public static void MarkRegistered(CMSDataContext Db, int PeopleId, int MeetingId, bool registered)
        {
            var m = Db.Meetings.Single(mm => mm.MeetingId == MeetingId);
            var om = Db.OrganizationMembers.SingleOrDefault(mm => mm.OrganizationId == m.OrganizationId && mm.PeopleId == PeopleId);
            var a = Db.Attends.SingleOrDefault(aa => aa.PeopleId == PeopleId && aa.MeetingId == MeetingId);
            if (a == null)
            {
                a = new Attend
                {
                    OrganizationId = m.OrganizationId,
                    PeopleId = PeopleId,
                    MeetingDate = m.MeetingDate.Value,
                    AttendanceFlag = false,
                    CreatedDate = Util.Now,
                    CreatedBy = Util.UserId1,
                    AttendanceTypeId = null,
                    BFCAttendance = null,
                    OtherAttends = 0,
                    MemberTypeId = om == null ? CmsData.Codes.MemberTypeCode.Visitor : om.MemberTypeId,
                    OtherOrgId = 0,
                };
                m.Attends.Add(a);
            }
            a.Registered = registered;
            Db.SubmitChanges();
        }
		public static int RecordAttend(CMSDataContext Db, int PeopleId, int OrgId, bool Present, DateTime dt)
		{
			var q = from o in Db.Organizations
					where o.OrganizationId == OrgId
					let p = Db.People.Single(pp => pp.PeopleId == PeopleId)
					select new
					{
						o.Location,
						OrgEntryPoint = o.EntryPointId,
						p.EntryPointId,
					};
			var info = q.Single();
			var meeting = (from m in Db.Meetings
						   where m.OrganizationId == OrgId && m.MeetingDate == dt
						   select m).FirstOrDefault();
			if (info.EntryPointId == null)
			{
				var p = Db.LoadPersonById(PeopleId);
				if (info.OrgEntryPoint > 0)
					p.EntryPointId = info.OrgEntryPoint;
			}
			if (meeting == null)
			{
				var acr = (from s in Db.OrgSchedules
						   where s.OrganizationId == OrgId
						   where s.SchedTime.Value.TimeOfDay == dt.TimeOfDay
						   where s.SchedDay == (int)dt.DayOfWeek
						   select s.AttendCreditId).SingleOrDefault();
				meeting = new Meeting
				{
					OrganizationId = OrgId,
					MeetingDate = dt,
					CreatedDate = Util.Now,
					CreatedBy = Util.UserId1,
					GroupMeetingFlag = false,
					Location = info.Location,
					AttendCreditId = acr
				};
				Db.Meetings.InsertOnSubmit(meeting);
				Db.SubmitChanges();
			}
			RecordAttendance(Db, PeopleId, meeting.MeetingId, Present);
			Db.UpdateMeetingCounters(meeting.MeetingId);
			return meeting.MeetingId;
		}
    }
}
