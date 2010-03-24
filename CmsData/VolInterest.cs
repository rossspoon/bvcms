using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilityExtensions;
using System.Text.RegularExpressions;
using System.Data.Linq;
using System.Xml.Linq;

namespace CmsData
{
    public partial class Person
    {
        public IEnumerable<VolInterestInterestCode> FetchVolInterestInterestCodes(string view)
        {
            if (orgs == null)
                ReadConfig(this, view);
            var q = from vi in VolInterestInterestCodes
                    where orgs.Keys.Contains(vi.VolInterestCode.Org)
                    select vi;
            return q;
        }
        public static List<string> OrgKeys(string view)
        {
            if (view == "ns")
            {
                var q = from c in DbUtil.Db.VolInterestCodes
                        group c by c.Org into g
                        select g.Key;
                return q.ToList();
            }
            else
            {
                var p = new Person();
                if (p.orgs == null)
                    ReadConfig(p, view);
                return p.orgs.Keys.Cast<string>().ToList();
            }
        }
        public class DayTime
        {
            public string Name { get; set; }
            public int Day { get; set; }
            public DateTime Time { get; set; }
        }
        public class OrgInfo
        {
            public int OrgId { get; set; }
            public string Name { get; set; }
            public Dictionary<string, DayTime> times { get; set; }
            public List<string> groups { get; set; }
            public bool nodrop { get; set; }
            public void CleanAttends(int pid)
            {
                if (times.Count() == 0)
                    return;
                var q = from a in DbUtil.Db.Attends
                        where a.OrganizationId == OrgId
                        where a.MeetingDate > Util.Now
                        where a.AttendanceFlag == false
                        where a.PeopleId == pid
                        select a;
                DbUtil.Db.Attends.DeleteAllOnSubmit(q);
                DbUtil.Db.SubmitChanges();
            }
            public void Drop(int pid)
            {
                if (nodrop)
                    return;
                var memb = DbUtil.Db.OrganizationMembers.SingleOrDefault(om =>
                    om.OrganizationId == OrgId && om.PeopleId == pid);
                if (memb != null)
                    memb.Drop();
                DbUtil.Db.SubmitChanges();
            }
        }
        public class VolInfo
        {
            public string desc { get; set; }
            public string sortdesc { get; set; }
            public int week { get; set; }
            public int month { get; set; }
            public DayTime hour { get; set; }
            public OrgInfo oi { get; set; }
            public string smallgroup { get; set; }
            public void CreateAttends(int pid, int year)
            {
                if (oi.times.Count() == 0)
                    return;

                if (week != 0)
                {
                    for (var m2 = 1; m2 <= 12; m2++)
                    {
                        var sun = Sunday(m2, year);
                        sun = sun.AddDays((week - 1) * 7);
                        if (sun.Month != m2)
                            continue;
                        if (hour != null)
                            CreateMeetingRegister(sun, hour, pid, year);
                        else
                            foreach (var t in oi.times.Values)
                                CreateMeetingRegister(sun, t, pid, year);
                    }
                }
                else if (month != 0 && hour != null)
                {
                    for (var wk = 1; wk <= 5; wk++)
                    {
                        var sun = Sunday(month, year);
                        sun = sun.AddDays((wk - 1) * 7);
                        if (sun.Month != month)
                            break;
                        CreateMeetingRegister(sun, hour, pid, year);
                    }
                }
            }
            private void CreateMeetingRegister(DateTime sun, DayTime hour, int pid, int year)
            {
                var dt = sun.AddDays(hour.Day);
                if (dt.Year > year)
                    return;
                dt = dt.Add(hour.Time.TimeOfDay);
                if (dt <= Util.Now)
                    return;

                var qme = from m in DbUtil.Db.Meetings
                          where m.OrganizationId == oi.OrgId
                          where m.MeetingDate == dt
                          select m;
                var meeting = qme.SingleOrDefault();
                if (meeting == null)
                {
                    meeting = new CmsData.Meeting
                    {
                        OrganizationId = oi.OrgId,
                        MeetingDate = dt,
                        CreatedDate = Util.Now,
                        CreatedBy = Util.UserId1,
                        GroupMeetingFlag = false,
                    };
                    DbUtil.Db.Meetings.InsertOnSubmit(meeting);
                    DbUtil.Db.SubmitChanges();
                }
                var qat = from a in meeting.Attends
                          where a.MeetingDate == dt
                          where a.PeopleId == pid
                          select a;
                var Att = qat.SingleOrDefault();
                if (Att == null)
                {
                    Att = new Attend
                    {
                        OrganizationId = oi.OrgId,
                        PeopleId = pid,
                        MeetingDate = dt,
                        CreatedDate = Util.Now,
                        CreatedBy = Util.UserId1,
                        Registered = true,
                        MemberTypeId = (int)OrganizationMember.MemberTypeCode.Member,
                    };
                    meeting.Attends.Add(Att);
                }
                DbUtil.Db.SubmitChanges();
            }
            public void AddMember(int pid)
            {
                if (oi != null)
                    OrganizationMember.InsertOrgMembers(oi.OrgId,
                        pid,
                        (int)OrganizationMember.MemberTypeCode.Member,
                        Util.Now,
                        null, false);
                if (!smallgroup.HasValue())
                    return;
                var mt = DbUtil.Db.MemberTags.SingleOrDefault(t => t.Name == smallgroup && t.OrgId == oi.OrgId);
                if (mt == null)
                {
                    mt = new MemberTag { Name = smallgroup, OrgId = oi.OrgId };
                    DbUtil.Db.MemberTags.InsertOnSubmit(mt);
                    DbUtil.Db.SubmitChanges();
                }
                var omt = DbUtil.Db.OrgMemMemTags.SingleOrDefault(t => t.PeopleId == pid && t.MemberTagId == mt.Id);
                if (omt == null)
                    mt.OrgMemMemTags.Add(new OrgMemMemTag { PeopleId = pid, OrgId = oi.OrgId });
                DbUtil.Db.SubmitChanges();
            }
            public void RemoveSmallGroups(int pid)
            {
                if (oi == null || !smallgroup.HasValue())
                    return;
                var mt = DbUtil.Db.MemberTags.SingleOrDefault(t => t.Name == smallgroup && t.OrgId == oi.OrgId);
                if (mt == null)
                    return;
                var omt = DbUtil.Db.OrgMemMemTags.SingleOrDefault(t => t.PeopleId == pid && t.MemberTagId == mt.Id);
                if (omt == null)
                    return;
                DbUtil.Db.OrgMemMemTags.DeleteOnSubmit(omt);
                DbUtil.Db.SubmitChanges();
            }

        }
        private static DateTime Sunday(int month, int year)
        {
            var first = new DateTime(year, month, 1);
            return new DateTime(year, month,
                1 + (7 - (int)first.DayOfWeek) % 7);
        }
        public void RefreshCommitments(string view)
        {
            if (orgs == null)
                ReadConfig(this, view);
            foreach (var oi in orgs.Values)
            {
                oi.CleanAttends(PeopleId);
                var q = from v in VolList.Values
                        where v.oi != null && v.oi.OrgId == oi.OrgId
                        select v;
                if (q.Count() == 0)
                    oi.Drop(PeopleId);
                else
                    foreach (var vol in q)
                    {
                        vol.AddMember(PeopleId);
                        vol.CreateAttends(PeopleId, year);
                    }
            }
        }
        public void ReplaceInterestCodes(IEnumerable<string> newcodes, string view)
        {
            // delete all oldcodes first
            DbUtil.Db.VolInterestInterestCodes.DeleteAllOnSubmit(FetchVolInterestInterestCodes(view));
            DbUtil.Db.SubmitChanges();
            if (newcodes == null)
                return;

            var q = from v in DbUtil.Db.VolInterestCodes
                    where orgs.Keys.Contains(v.Org)
                    select v;
            var list = q.ToList();
            var q2 = from v in list
                     where v.Description.Replace(' ', '_').StartsWith(v.Org)
                     select v;
            var dict = q2.ToDictionary(v => v.Org + (v.Code ?? ""));
            foreach (var i in newcodes)
            {
                if (!dict.ContainsKey(i))
                {
                    var desc = i.Replace('_', ' ');
                    var org = orgs.Keys.Where(k => i.StartsWith(k)).SingleOrDefault();
                    if (org == null)
                        org = orgs.First().Key;
                    var vic = new VolInterestCode
                    {
                        Description = desc,
                        Org = org,
                        Code = i.Substring(org.Length),
                    };
                    DbUtil.Db.VolInterestCodes.InsertOnSubmit(vic);
                    dict[i] = vic;
                }
            }
            DbUtil.Db.SubmitChanges();
            DbUtil.Db.Refresh(RefreshMode.OverwriteCurrentValues, this);

            foreach (var i in newcodes)
                VolInterestInterestCodes.Add(
                        new VolInterestInterestCode { InterestCodeId = dict[i].Id });
            DbUtil.Db.SubmitChanges();
        }
        public IEnumerable<string> SummarySorted()
        {
            var q = from vi in VolList
                    orderby vi.Value.sortdesc
                    select vi.Key;
            return q;
        }
        private Dictionary<string, VolInfo> _vollist;
        public Dictionary<string, VolInfo> VolList
        {
            get
            {
                return _vollist;
            }
        }
        public void BuildVolInfoList(string view)
        {
            bool secondtime = _vollist != null;
            if (orgs == null)
                ReadConfig(this, view);
            var keys = orgs.Keys.Cast<string>();
            var q = from vi in FetchVolInterestInterestCodes(view)
                    select vi.VolInterestCode.Org + vi.VolInterestCode.Code;
            if (secondtime)
            {
                var list = q.ToList();
                var dels = from v in _vollist
                           join s in list on v.Key equals s into j
                           from s in j.DefaultIfEmpty()
                           where string.IsNullOrEmpty(s)
                           select v;
                var dlist = dels.ToList();
                foreach (var d in dlist)
                {
                    d.Value.RemoveSmallGroups(PeopleId);
                    _vollist.Remove(d.Key);
                }
            }
            else
                _vollist = new Dictionary<string, VolInfo>();

            foreach (var c in q)
            {
                var vol = _vollist.SingleOrDefault(kp => kp.Key == c).Value;
                if (vol == null)
                {
                    vol = new VolInfo();
                    _vollist[c] = vol;
                }
                vol.oi = orgs.SingleOrDefault(o => c.StartsWith(o.Key)).Value;
                if (vol.oi == null)
                {
                    vol.sortdesc = c;
                    continue;
                }
                if (secondtime)
                    vol.oi.nodrop = true;
                vol.sortdesc = vol.oi.Name;

                vol.hour = (from p in vol.oi.times where c.Contains(p.Key) select p.Value)
                    .SingleOrDefault();
                vol.month = (from p in months where c.Contains(p.Key) select p.Value)
                    .SingleOrDefault();
                vol.week = (from p in weeks where c.Contains(p.Key) select p.Value)
                    .SingleOrDefault();
                var groupend = c.Length;
                if (vol.week != 0)
                {
                    groupend = c.IndexOf("_Week");
                    vol.sortdesc += "_Week" + vol.week;
                    if (vol.hour != null)
                        vol.sortdesc += "_week{0}_{1:d-HHMM}".Fmt(vol.week, vol.hour);
                }
                else if (vol.month != 0 && vol.hour != null)
                {
                    groupend = (from p in months where c.Contains(p.Key) select c.IndexOf(p.Key))
                        .SingleOrDefault();
                    vol.sortdesc += "_Mon{0:00}_{1}_{2:HHMM}".Fmt(vol.month, vol.hour.Day, vol.hour.Time);
                }
                var olen = vol.oi.Name.Length;
                vol.smallgroup = c.Substring(olen, groupend - olen);
                if (vol.smallgroup.StartsWith("_"))
                    vol.smallgroup = vol.smallgroup.Substring(1);
            }
        }
        private int year;
        private Dictionary<string, OrgInfo> orgs;
        Dictionary<string, int> months = new Dictionary<string, int>
        {
           { "_Jan", 1 },
           { "_Feb", 2 },
           { "_Mar", 3 },
           { "_Apr", 4 },
           { "_May", 5 },
           { "_Jun", 6 },
           { "_Jul", 7 },
           { "_Aug", 8 },
           { "_Sep", 9 },
           { "_Oct", 10 },
           { "_Nov", 11 },
           { "_Dec", 12 },
        };
        Dictionary<string, int> weeks = new Dictionary<string, int>
        {
           { "_Week1", 1 },
           { "_Week2", 2 },
           { "_Week3", 3 },
           { "_Week4", 4 },
           { "_Week5", 5 },
        };

        private static void ReadConfig(Person p, string view)
        {
            string config = DbUtil.Content("Volunteer-" + view + ".xml").Body;
            var doc = XDocument.Parse(config);
            var a = doc.Root.Attribute("year");
            if (a != null)
                p.year = doc.Root.Attribute("year").Value.ToInt();
            else
                p.year = Util.Now.Year;
            p.orgs = doc.Root.Descendants("org").ToDictionary(
                o => o.Attribute("name").Value,
                o => new OrgInfo
                {
                    Name = o.Attribute("name").Value,
                    OrgId = o.Attribute("orgid").Value.ToInt(),
                    times = o.Descendants("time").Select(t =>
                        new DayTime
                        {
                            Name = t.Attribute("name").Value,
                            Day = t.Attribute("day").Value.ToInt(),
                            Time = DateTime.Parse(t.Attribute("hour").Value)
                        }).ToDictionary(dt => dt.Name),
                    groups = o.Descendants("group").Select(t => t.Value).ToList()
                });
        }
    }
}
