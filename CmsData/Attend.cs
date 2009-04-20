using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public AttendTypeCode AttendTypeEnum
        {
            get { return (AttendTypeCode)AttendanceTypeId; }
            set { AttendanceTypeId = (int)value; }
        }
        public enum MemberTypeCode
        {
            VisitingMember = 300,
            Visitor = 310,
            InServiceMember = 500,
        }
        public MemberTypeCode MemberTypeEnum
        {
            get { return (MemberTypeCode)MemberTypeId; }
            set { MemberTypeId = (int)value; }
        }
    }
}
