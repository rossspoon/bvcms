using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using CmsData;
using UtilityExtensions;
using CmsWeb.Models.OrganizationPage;
using System.Collections;
using System.Web.Mvc;

namespace CmsWeb.Models
{
	public class VolunteerCommitmentsModel
	{
		public string SmallGroup1 { get; set; }
		public string SmallGroup2 { get; set; }
		public bool SortByWeek { get; set; }

		public class NameId
		{
			public string Name { get; set; }
			public int PeopleId { get; set; }
		}
		public class CellInfo
		{
			public DateTime Sunday { get; set; }
			public DateTime DayHour { get; set; }
			public List<NameId> Persons { get; set; }
			public int Limit { get; set; }
			public int MeetingId { get; set; }
		}
		private class Attendance
		{
			public DateTime Sunday { get; set; }
			public TimeSpan TimeOfWeek { get; set; }
			public DateTime MeetingDate { get; set; }
			public int MeetingId { get; set; }
			public int PeopleId { get; set; }
			public string Name { get; set; }
		}
		private IEnumerable<Attendance> Attends;
		public IEnumerable<DateTime> times;
		public IEnumerable<DateTime> weeks;
		public IEnumerable<CellInfo> details;
		public string OrgName { get; set; }
		public int OrgId { get; set; }
		public bool IsLeader { get; set; }

		public class VolunteerInfo
		{
			public string Name { get; set; }
			public int commits { get; set; }
			public int PeopleId { get; set; }
			public int OrgId { get; set; }
		}
		public SelectList SmallGroups()
		{
			var q = from m in DbUtil.Db.MemberTags
					where m.OrgId == OrgId
					where m.OrgMemMemTags.Any()
					orderby m.Name
					select m.Name;
			var list = q.ToList();
			list.Insert(0, "(not specified)");
			return new SelectList(list);
		}
		public IEnumerable<VolunteerInfo> Volunteers()
		{
			var q = from om in DbUtil.Db.OrganizationMembers
					where om.OrganizationId == OrgId
					where SmallGroup1 == null || SmallGroup1 == "(not specified)"
						|| om.OrgMemMemTags.Any(mm => mm.MemberTag.Name == SmallGroup1)
					where SmallGroup2 == null || SmallGroup2 == "(not specified)"
						|| om.OrgMemMemTags.Any(mm => mm.MemberTag.Name == SmallGroup2)
					let commits = (from a in DbUtil.Db.Attends
								   where a.MeetingDate > Util.Now.Date
								   where a.OrganizationId == om.OrganizationId
								   where a.PeopleId == om.PeopleId
								   where a.Registered == true
								   select a).Count()
					orderby commits descending, om.Person.Name2
					select new VolunteerInfo
					{
						Name = om.Person.Name,
						commits = commits,
						PeopleId = om.PeopleId,
						OrgId = om.OrganizationId
					};
			return q;
		}
		public VolunteerCommitmentsModel(int id)
		{
			OrgName = (from o in DbUtil.Db.Organizations where o.OrganizationId == id select o.OrganizationName).Single();
			OrgId = id;
			var q = from a in DbUtil.Db.Attends
					where a.MeetingDate > Util.Now.Date
					where a.OrganizationId == id
					where a.Registered == true
					orderby a.MeetingDate
					select a;
			var list = q.ToList();
			if (list.Count == 0)
				return;
			var sunday = list.First().MeetingDate.Date;
			sunday = sunday.AddDays(-(int)sunday.DayOfWeek);
			Attends = from a in list
					  let Day = (int)a.MeetingDate.DayOfWeek
					  let Sunday = a.MeetingDate.Date.AddDays(-Day)
					  orderby a.MeetingDate
					  select new Attendance
					  {
						  MeetingId = a.MeetingId,
						  Sunday = Sunday,
						  TimeOfWeek = a.MeetingDate.Subtract(Sunday),
						  MeetingDate = a.MeetingDate,
						  PeopleId = a.PeopleId,
						  Name = a.Person.Name,
					  };
			details = from i in Attends
					  group new NameId
					  {
						  PeopleId = i.PeopleId,
						  Name = i.Name
					  } by new
					  {
						  i.Sunday,
						  i.TimeOfWeek,
						  i.MeetingId,
						  i.MeetingDate
					  } into g
					  let ts = Regsettings.TimeSlots.Single(tt => tt.Time.Value.TimeOfDay == g.Key.MeetingDate.TimeOfDay && tt.DayOfWeek == (int)g.Key.MeetingDate.DayOfWeek)
					  select new CellInfo
					  {
						  Sunday = g.Key.Sunday,
						  DayHour = g.Key.Sunday.Add(g.Key.TimeOfWeek),
						  MeetingId = g.Key.MeetingId,
						  Persons = g.ToList(),
						  Limit = ts.Limit ?? 0
					  };
			times = from ts in Regsettings.TimeSlots
					orderby ts.Time
					select ts.Time.Value;
			weeks = from i in Attends
					group i by i.Sunday into g
					orderby g.Key
					select g.Key;

			IsLeader = OrganizationModel.VolunteerLeaderInOrg(id);
		}
		public class Slot
		{
			public DateTime Time { get; set; }
			public DateTime Sunday { get; set; }
			public int Week { get; set; }
			public bool Disabled { get; set; }
		}

		public IEnumerable<List<Slot>> FetchSlotWeeks()
		{
			if (SortByWeek)
				return from slot in FetchSlots()
					   group slot by slot.Sunday into g
					   where g.Any(gg => gg.Time > DateTime.Today)
					   orderby g.Key.WeekOfMonth(), g.Key
					   select g.OrderBy(gg => gg.Time).ToList();
			else
				return from slot in FetchSlots()
					   group slot by slot.Sunday into g
					   where g.Any(gg => gg.Time > DateTime.Today)
					   orderby g.Key
					   select g.OrderBy(gg => gg.Time).ToList();
		}
		public IEnumerable<Slot> FetchSlots()
		{
			var list = new List<Slot>();
			var sunday = Sunday;
			for (; sunday <= EndDt; sunday = sunday.AddDays(7))
			{
				var dt = sunday;
				{
					var q = from ts in Regsettings.TimeSlots
							orderby ts.Datetime()
							let time = ts.Datetime(dt)
							select new Slot()
									{
										Time = time,
										Sunday = dt,
										Week = dt.WeekOfMonth(),
										Disabled = time < DateTime.Now
									};
					list.AddRange(q);
				}
			}
			return list;
		}
		private Organization _org;
		public Organization Org
		{
			get
			{
				return _org ??
					(_org = DbUtil.Db.Organizations.Single(oo => oo.OrganizationId == OrgId));
			}
		}
		private DateTime? _endDt;
		public DateTime EndDt
		{
			get
			{
				if (!_endDt.HasValue)
				{
					var dt = Org.LastMeetingDate ?? DateTime.MinValue;
					if (dt == DateTime.MinValue)
						dt = DateTime.Today.AddMonths(7);
					_endDt = dt;
				}
				return _endDt.Value;
			}
		}

		private DateTime? _sunday;
		public DateTime Sunday
		{
			get
			{
				if (!_sunday.HasValue)
				{
					var dt = Org.FirstMeetingDate ?? DateTime.MinValue;
					if (dt == DateTime.MinValue || dt < DateTime.Today)
						dt = DateTime.Today;
					_sunday = dt.AddDays(-(int)dt.DayOfWeek);
				}
				return _sunday.Value;
			}
		}
		private RegSettings _regsettings;
		public RegSettings Regsettings
		{
			get
			{
				return _regsettings ??
					(_regsettings = new RegSettings(Org.RegSetting, DbUtil.Db, OrgId));
			}
		}

	}
}
