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
            var o = HttpContext.Current.Items["attendinfo"] as CMSDataContext.AttendMeetingInfo1;
            if (MemberTypeId == 310 && AttendanceTypeId != 40 && AttendanceTypeId != 50 && AttendanceTypeId != 60 && AttendanceTypeId != 110)
            {
                var tw = new StringWriter();
                ObjectDumper.Write(o, 1, tw);
                var smtp = new SmtpClient();
                var u = DbUtil.Db.CurrentUser;
                var email = u.EmailAddress;
                var from = new MailAddress(u.EmailAddress, u.Name);
                var to = new MailAddress(WebConfigurationManager.AppSettings["senderrorsto"]);
                var msg = new MailMessage(from, to);
                msg.Subject = "Attendance Oddity({0}, {1}, {2})".Fmt(MemberTypeId, AttendanceTypeId, o.path);
                msg.Body = "\n{0} ({1}, {2})\n".Fmt(u.EmailAddress, u.UserId, u.Name) + tw.ToString();
                smtp.Send(msg);
            }
        }
    }
}
