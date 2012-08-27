using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using CmsData;
using UtilityExtensions;

namespace CmsWeb.Models
{
	public class CheckinTimeModel
	{
		public static string ALL_ACTIVITIES = "- All Activities -";

		public DateTime? dateStart { get; set; }
		public DateTime? dateEnd { get; set; }
		public int peopleid { get; set; }
		public string location { get; set; }
		public string activity { get; set; }
		public bool withGuest { get; set; }

		public PagerModel2 Pager { get; set; }

		public CheckinTimeModel()
		{
			Pager = new PagerModel2();
			Pager.setCountDelegate(Count);
			Pager.Direction = "desc";
			Pager.Sort = "Host";
			var locs = Locations();
			location = DbUtil.Db.UserPreference("checkintimes-location", locs.FirstOrDefault());
		}

		public List<string> Activities()
		{
			var q = from a in DbUtil.Db.CheckInActivities
					group a.Activity by a.Activity into g
					select g.Key;
			var list = q.ToList();
			list.Insert(0, ALL_ACTIVITIES);
			return list;
		}

		private List<string> locations;
		public List<string> Locations()
		{
			if (locations == null)
			{
				var q = from t in DbUtil.Db.CheckInTimes
						where t.Location != null
						group t.Location by t.Location
							into g
							select g.Key;
				locations = q.ToList();
			}
			return locations;
		}

		public class CheckinTimeEx
		{
			public CheckInTime ctime { get; set; }
			public string name { get; set; }
			public string activities { get; set; }
			public int guests { get; set; }
			internal PagerModel2 pager;
			public string rowclass()
			{
				if (pager.Sort != "Host")
					return null;
				if (ctime.GuestOfId == null)
					return "host";
				return "guest";
			}
			public string indent()
			{
				if (pager.Sort != "Host")
					return null;
				if (ctime.GuestOfId != null)
					return "indent";
				return null;
			}
		}

		public class CountInfo
		{
			public int members { get; set; }
			public int guests { get; set; }
			private string _name;
			public string name
			{
				get { return _name.HasValue() ? _name : "Not Specified"; }
				set { _name = value; }
			}
		}

		private CountInfo _counts;
		public CountInfo counts
		{
			get
			{
				if (_counts == null)
					FetchTimes();
				return _counts;
			}
		}

		public int Count()
		{
			return counts.guests = counts.members;
		}

		public class ActivityCount
		{
			public string name;
			public int count;
		}

		public List<ActivityCount> FetchActivityCount()
		{
			var q1 = from e in DbUtil.Db.CheckInTimes
					 join c in DbUtil.Db.CheckInActivities on e.Id equals c.Id
					 where e.Location == location
					 where e.CheckInTimeX >= dateStart || dateStart == null
					 where e.CheckInTimeX < dateEnd || dateEnd == null
					 group c by c.Activity into grouped
					 select new ActivityCount() { name = grouped.Key, count = grouped.Count() };

			var q2 = from e in DbUtil.Db.CheckInTimes
					 where e.Location == location
					 where e.CheckInTimeX >= dateStart || dateStart == null
					 where e.CheckInTimeX < dateEnd || dateEnd == null
					 group e by e.PeopleId into grouped
					 select new { key = grouped.Key };

			var q3 = from e in DbUtil.Db.CheckInTimes
					 where e.Location == location
					 where e.CheckInTimeX >= dateStart || dateStart == null
					 where e.CheckInTimeX < dateEnd || dateEnd == null
					 select e;

			var p = new ActivityCount() { name = "Total Visits", count = q3.Count() };
			var q = new ActivityCount() { name = "Unique People", count = q2.Count() };
			

			var lq = q1.ToList();
			lq.Insert(0, q);
			lq.Insert(0, p);

			return lq;
		}

		private IEnumerable<CheckinTimeEx> _times;
		public IEnumerable<CheckinTimeEx> FetchTimes()
		{
			if (_times != null)
				return _times;

			// filter
			if (dateEnd != null)
				dateEnd = dateEnd.Value.AddHours(24);

			var q = from t in DbUtil.Db.CheckInTimes
					where t.Location == location
					where t.CheckInTimeX >= dateStart || dateStart == null
					where t.CheckInTimeX < dateEnd || dateEnd == null
					where peopleid == 0 || t.PeopleId == peopleid || t.Guests.Any(g => g.PeopleId == peopleid)
					where withGuest == false || (t.Guests.Any() || t.GuestOfId != null)
					select t;

			if (activity != null && !activity.Equals(ALL_ACTIVITIES))
				q = from t in q
					where t.CheckInActivities.Any(z => z.Activity == activity)
					select t;

			// count
			var q2 = from t in q
					 group t by 1 into g
					 select new CountInfo()
					 {
						 members = g.Count(tt => tt.GuestOfId == null),
						 guests = g.Count(tt => tt.GuestOfId != null),
						 name = (from p in DbUtil.Db.People
								 where p.PeopleId == peopleid
								 select p.Name).SingleOrDefault()
					 };
			_counts = q2.Single();

			// sort
			q = SortItems(q);

			// transform to view model
			_times = from t in q.Skip(Pager.StartRow).Take(Pager.PageSize)
					 select new CheckinTimeEx
					 {
						 ctime = t,
						 activities = string.Join(",",
							 t.CheckInActivities.Select(a => a.Activity)),
						 name = t.Person.Name,
						 guests = t.Guests.Count(),
						 pager = Pager,
					 };
			return _times;
		}
		public IQueryable<CheckInTime> SortItems(IQueryable<CheckInTime> results)
		{
			if (Pager.Sort == "Host")
				return from z in results
					   orderby z.CheckInTimeX
					   select z;
			switch (Pager.Direction)
			{
				case "asc":
					switch (Pager.Sort)
					{
						case "Person":
							results = results.OrderBy(z => z.Person.Name2);
							break;
						case "Date/Time":
							results = results.OrderBy(z => z.CheckInTimeX);
							break;
						case "Activity":
							results = from z in results
									  orderby z.CheckInActivities.FirstOrDefault().Activity,
										z.CheckInTimeX
									  select z;
							break;
					}
					break;
				case "desc":
					switch (Pager.Sort)
					{
						case "Person":
							results = results.OrderByDescending(z => z.Person.Name2);
							break;
						case "Date/Time":
							results = results.OrderByDescending(z => z.CheckInTimeX);
							break;
						case "Activity":
							results = from z in results
									  orderby z.CheckInActivities.FirstOrDefault().Activity descending,
										z.CheckInTimeX
									  select z;
							break;
					}
					break;
			}

			return results;
		}
	}
}