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
		public IEnumerable<DateTime> times;
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
			times = from ts in Regsettings.TimeSlots
					orderby ts.Time
					select ts.Time.Value;

			IsLeader = OrganizationModel.VolunteerLeaderInOrg(id);
		}
		public class Slot
		{
			public DateTime Time { get; set; }
			public DateTime Sunday { get; set; }
			public int Week { get; set; }
			public bool Disabled { get; set; }
			public DateTime DayHour { get; set; }
			public List<NameId> Persons { get; set; }
			public int Limit { get; set; }
			public int MeetingId { get; set; }
		}

		public IEnumerable<List<Slot>> FetchSlotWeeks(int week = 0)
		{
			if (SortByWeek)
				return from slot in FetchSlots(week)
					   group slot by slot.Sunday into g
					   where g.Any(gg => gg.Time > DateTime.Today)
					   orderby g.Key.WeekOfMonth(), g.Key
					   select g.OrderBy(gg => gg.Time).ToList();
			else
				return from slot in FetchSlots(week)
					   group slot by slot.Sunday into g
					   where g.Any(gg => gg.Time > DateTime.Today)
					   orderby g.Key
					   select g.OrderBy(gg => gg.Time).ToList();
		}
		public IEnumerable<Slot> FetchSlots(int week)
		{
			var q = from a in DbUtil.Db.Attends
					where a.MeetingDate > Util.Now.Date
					where a.OrganizationId == OrgId
					where a.Registered == true
					orderby a.MeetingDate
					select a;
			var alist = q.ToList();
			var Attends = from a in alist
					  let Day = (int)a.MeetingDate.DayOfWeek
					  let sday = a.MeetingDate.Date.AddDays(-Day)
					  where week == 0 || Day == week
					  orderby a.MeetingDate
					  select new 
					  {
						  a.MeetingId,
						  a.MeetingDate,
						  a.PeopleId,
						  a.Person.Name,
					  };

			var list = new List<Slot>();
			for (var sunday = Sunday; sunday <= EndDt; sunday = sunday.AddDays(7))
			{
				var dt = sunday;
				{
					var u = from ts in Regsettings.TimeSlots
							orderby ts.Datetime()
							let time = ts.Datetime(dt)
							select new Slot()
									{
										Time = time,
										Sunday = dt,
										Week = dt.WeekOfMonth(),
										Disabled = time < DateTime.Now,
										Limit = ts.Limit ?? 0,
										Persons = (from a in Attends
												   where a.MeetingDate == time
												   select new NameId { Name = a.Name, PeopleId = a.PeopleId }).ToList(),
									    MeetingId = Attends.Where(aa => aa.MeetingDate == time).Select(aa => aa.MeetingId).FirstOrDefault()
									};
					list.AddRange(u);
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
