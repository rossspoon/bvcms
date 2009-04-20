using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DiscData;
using System.Text;
using System.Net;

namespace Prayer.Models
{
    public static class PsUtil
    {
        public static string Id(this PrayerSlot ps)
        {
            return IdStr(ps.Time, ps.Day);
        }
        public static string IdStr(DateTime time, int day)
        {
            var dt = new DateTime(2005, 1, day + 1, time.Hour, time.Minute, 0);
            return dt.ToString("ddHHmm");
        }
        public static void ParseIdStr(string id, out DateTime time, out int day)
        {
            var dt = DateTime.ParseExact(id, "ddHHmm", null);
            time = new DateTime(2005, 1, 1, dt.Hour, dt.Minute, 0);
            day = dt.Day - 1;
        }
        public static void SendNotifications()
        {
            var q = from n in DbUtil.Db.PendingNotifications
                    where n.NotifyType == "PrayerTime"
                    select n;
            var q2 = from n in q
                     select new
                     {
                         n.User.FirstName,
                         n.User.Name,
                         n.User.PrayerSlots,
                         n.User.EmailAddress,
                     };
            foreach (var n in q2)
            {
                var sb = new StringBuilder();
                foreach (var ps in n.PrayerSlots)
                    sb.AppendFormat("<tr><td>{0}</td><td>{1:hh:mm tt}</td></tr>\n", ((DayOfWeek)ps.Day), ps.Time);
                Prayer.Controllers.AccountController.Email(n.Name, n.EmailAddress, "Your Prayer Commitment",
@"Hi {0},<br/>
You have reserved the following prayer times:
<table>{1}</table>
Thank you for praying!<br/>
Prayer Ministry".Fmt(n.FirstName, sb.ToString()));
            }
            DbUtil.Db.PendingNotifications.DeleteAllOnSubmit(q);
            DbUtil.Db.SubmitChanges();
        }
    }
}
