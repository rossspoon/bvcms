using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using BitFactory.Logging;
using System.Web;
using UtilityExtensions;

namespace CmsData
{
    public partial class Attend
    {
        public enum AttendTypeCode
        {
            Absent = 0,
            Leader = 10,
            Volunteer = 20,
            Member = 30,
            VisitingMember = 40,
            RecentVisitor = 50,
            NewVisitor = 60,
            InService = 70,
            Offsite = 80,
            Group = 90,
            Homebound = 100,
            OtherClass = 110,
        };
        public enum MemberTypeCode
        {
            VisitingMember = 300,
            Visitor = 310,
            InServiceMember = 500,
        }
        partial void OnValidate(System.Data.Linq.ChangeAction action)
        {
            var path = HttpContext.Current.Items["atpath"].ToInt();
                if (MemberTypeId == 310 && AttendanceTypeId != 50 && AttendanceTypeId != 60)
                    Util.Logger.LogStatus("Attendance Oddity({0}, {1}, {2})\n\n--Stacktrace\n{3}".Fmt(
                        MemberTypeId, AttendanceTypeId, path, System.Environment.StackTrace));
        }
    }
}
