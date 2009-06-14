using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DiscData;
using System.Text;

namespace Prayer.Models
{
    public class SignupModel
    {
        public SignupModel(User who)
        {
            Group = Group.LoadByName("Prayer Partners");
            User = who;
        }
        public Group Group;
        public User User { get; set; }
        public static string DayName(int day)
        {
            return ((DayOfWeek)day).ToString().Substring(0, 3);
        }
        public int TotalCount()
        {
            return DbUtil.Db.PrayerSlots.Count();
        }
        private IEnumerable<MemSlotInfo> MemSlots()
        {
            foreach (var time in FetchTimeSlots())
                for (var day = 0; day < 7; day++)
                    yield return new MemSlotInfo
                    {
                        Day = day,
                        Time = time,
                    };
        }
        private IEnumerable<MemSlotInfo> MemSlots(int day)
        {
            foreach (var time in FetchTimeSlots())
                yield return new MemSlotInfo
                {
                    Day = day,
                    Time = time,
                };
        }
        public IEnumerable<SlotInfo> FetchSlots(int day)
        {
            var list = from ps in DbUtil.Db.PrayerSlots
                       where ps.Day == day
                       select new
                       {
                           Slot = ps,
                           Name = ps.User.FirstName + " " + ps.User.LastName,
                           UserId = ps.UserId
                       };
            return from ms in MemSlots(day)
                   join ps in list on ms.Id equals ps.Slot.Id() into j
                   select new SlotInfo(User)
                   {
                       Owners = j.ToDictionary(k => k.UserId, s => s.Name),
                       Day = ms.Day,
                       Time = ms.Time,
                   };
        }
        private IEnumerable<SlotInfo> FetchSlots()
        {
            var list = from ps in DbUtil.Db.PrayerSlots
                       select new
                       {
                           Slot = ps,
                           Name = ps.User.FirstName + " " + ps.User.LastName,
                           UserId = ps.UserId
                       };
            return from ms in MemSlots()
                   join ps in list on ms.Id equals ps.Slot.Id() into j
                   select new SlotInfo(User)
                   {
                       Owners = j.ToDictionary(k => k.UserId, s => s.Name),
                       Day = ms.Day,
                       Time = ms.Time,
                   };
        }
        public SlotInfo FetchSlot(int day, DateTime time)
        {
            var list = from ps in DbUtil.Db.PrayerSlots
                       where ps.Time == time && ps.Day == day
                       select new
                       {
                           UserId = ps.UserId,
                           Name = ps.User.FirstName + " " + ps.User.LastName,
                       };
            return new SlotInfo(User)
            {
                Day = day,
                Time = time,
                Owners = list.ToDictionary(k => k.UserId, s => s.Name),
            };
        }
        public DateTime TimeStart = new DateTime(2000, 1, 1, 7, 00, 0); // 7:00 AM;
        public DateTime TimeEnd = new DateTime(2000, 1, 1, 22, 30, 0); // 10:30 PM;

        private IEnumerable<SlotInfo> _Slots;
        private IEnumerable<SlotInfo> AllSlots
        {
            get
            {
                if (_Slots == null)
                    _Slots = FetchSlots().ToList();
                return _Slots;
            }
        }
        public IEnumerable<SlotInfo> FetchWeekForTime(DateTime time)
        {
            var q = from ts in AllSlots
                    where ts.Time == time
                    select ts;
            return q;
        }
        public IEnumerable<DateTime> FetchTimeSlots()
        {
            var ThirtyMinutes = new TimeSpan(0, 30, 0);
            for (var time = TimeStart; time <= TimeEnd; time += ThirtyMinutes)
                yield return time;
        }
        internal void PrayerSlotChangeNotify(User u, SlotInfo si)
        {
            var g = Group.LoadByName("Prayer Partners");
            var a = g.GetUsersInRole(GroupType.Admin);
            foreach (var admin in a)
                Prayer.Controllers.AccountController.Email(admin.Name, admin.EmailAddress,
                "time {0} for {1}".Fmt(si.Mine ? "claimed" : "released", u.Name),
                "{0} {1:hh:mm tt} changed at {2} ({3} total)".Fmt(
                DayName(si.Day), si.Time, DateTime.Now, si.Owners.Count));
        }
        public SlotCellInfo ToggleSlot(string id, bool ck)
        {
            DateTime time;
            int day;
            PsUtil.ParseIdStr(id, out time, out day);

            var q = from s in DbUtil.Db.PrayerSlots
                    where s.UserId == User.UserId
                    select s;
            if (ck && q.Count() >= 5)
                return new SlotCellInfo(this, ToggleResult.Limit, day, time);
            int userid = User.UserId;
            var ps = DbUtil.Db.PrayerSlots.SingleOrDefault(s =>
                s.Time == time && s.Day == day && s.UserId == userid);
            if (ps == null && ck)
            {
                ps = new PrayerSlot
                {
                    Time = time,
                    Day = day,
                    UserId = userid
                };
                DbUtil.Db.PrayerSlots.InsertOnSubmit(ps);
                ScheduleNotify(userid);
                DbUtil.Db.SubmitChanges();
                return new SlotCellInfo(this, ToggleResult.Yours, day, time);
            }
            else if (ps.UserId == userid && !ck)
            {
                DbUtil.Db.PrayerSlots.DeleteOnSubmit(ps);
                ScheduleNotify(userid);
                DbUtil.Db.SubmitChanges();
                return new SlotCellInfo(this, ToggleResult.Open, day, time);
            }
            else if (ps.UserId != userid)
                return new SlotCellInfo(this, ToggleResult.Taken, day, time);
            else
                return new SlotCellInfo(this, ToggleResult.NoChange, day, time);
        }
        private static void ScheduleNotify(int userid)
        {
            const string STR_PrayerTime = "PrayerTime";
            var notify = DbUtil.Db.PendingNotifications.SingleOrDefault(n =>
                n.UserId == userid && n.NotifyType == STR_PrayerTime);
            if (notify == null)
            {
                notify = new PendingNotification { NotifyType = STR_PrayerTime, UserId = userid };
                DbUtil.Db.PendingNotifications.InsertOnSubmit(notify);
            }
        }
    }
    public enum ToggleResult { Open, Yours, Taken, NoChange, Limit }
    public class SlotCellInfo
    {
        public SlotCellInfo(SignupModel m, ToggleResult r, int day, DateTime time)
        {
            Status = r.ToString();
            var si = m.FetchSlot(day, time);
            Html = si.SlotCell();
            if (r == ToggleResult.Open || r == ToggleResult.Yours)
                m.PrayerSlotChangeNotify(m.User, si);
        }
        public string Status { get; set; }
        public string Html { get; set; }
    }
    public class MemSlotInfo
    {
        public DateTime Time { get; set; }
        public int Day { get; set; }
        public string Id
        {
            get { return PsUtil.IdStr(Time, Day); }
        }
    }
    public class SlotInfo : MemSlotInfo
    {
        public SlotInfo(User u)
        {
            user = u;
        }
        private User user;
        private const int MaxCubes = 8;
        public Dictionary<int, string> Owners { get; set; }
        public bool Checked
        {
            get { return Mine || Owners.Count == MaxCubes; }
        }
        public bool CanClick
        {
            get { return (Owners.Count < MaxCubes || Mine) && user.UserId > 0 && user.PeopleId.HasValue; }
        }
        public bool Mine
        {
            get { return Owners.ContainsKey(user.UserId); }
        }
        private string classAttr
        {
            get
            {
                var r = (double)Owners.Count / MaxCubes;
                var c = "slot";
                if (r > 0)
                {
                    if (Mine)
                        c += " m";
                    else
                        c += " o";
                    if (r < .35)
                        c += 1;
                    else if (r > .8)
                        c += 3;
                    else
                        c += 2;
                }
                return "class=\"{0}\"".Fmt(c);
            }
        }
        private string onclickAttr
        {
            get
            {
                if (CanClick)
                    return " onclick='ToggleSlot(this)'";
                return "";
            }
        }
        private string disabledAttr
        {
            get
            {
                if (!CanClick)
                    return " disabled=\"disabled\"";
                return "";
            }
        }
        private string checkedAttr
        {
            get
            {
                if (Checked)
                    return " checked=\"checked\"";
                return "";
            }
        }
        private string titleAttr
        {
            get
            {
                var sb = new StringBuilder(((DayOfWeek)Day).ToString() + ", {0:hh:mm tt}".Fmt(Time));
                foreach (var o in Owners)
                {
                    sb.Append(" - ");
                    sb.Append(o.Value);
                }
                if (sb.Length > 0 && Util.CurrentUser.UserId > 0)
                    return " title='{0}'".Fmt(sb.ToString());
                return "";
            }
        }
        public string SlotCell()
        {
            var s = "<td {1}{2}><input id='{0}' type='checkbox'{3}{4}{5}/>"
                .Fmt(Id, classAttr, titleAttr, checkedAttr, onclickAttr, disabledAttr);
            var r = (double)Owners.Count / MaxCubes;
            var h = Convert.ToInt32(Math.Round(r * 20));
            if (h > 0)
                s += "<img align='bottom' border='0' src='/content/dot.gif' width='2' height='" + h + "'/>";
            return s += "</td>";
        }
    }
}
