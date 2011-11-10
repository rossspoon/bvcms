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
            int ntries = 3;
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
                            System.Threading.Thread.Sleep(300);
                            continue;
                        }
                    throw;
                }
            }
        }
        public static void MarkRegistered(int PeopleId, int MeetingId, bool registered)
        {
            MarkRegistered(DbUtil.Db, PeopleId, MeetingId, registered);
        }
        public static void MarkRegistered(CMSDataContext Db, int OrgId, int PeopleId, DateTime MeetingDate, bool registered)
        {
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
    }
}
