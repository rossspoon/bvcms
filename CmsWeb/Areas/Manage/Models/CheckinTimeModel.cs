using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using CmsData;

namespace CmsWeb.Models
{
	public class CheckinTimeModel
	{
		public static string ALL_ACTIVITIES = "- All Activities -";

		public DateTime? dateStart { get; set; }
		public DateTime? dateEnd { get; set; }
		public int Person { get; set; }
		public string Activity { get; set; }

		public PagerModel2 Pager { get; set; }

		public CheckinTimeModel()
		{
			Pager = new PagerModel2();
		}

		public IEnumerable<CheckInActivity> Activities()
		{
			var acResults = from y in DbUtil.Db.CheckInActivities
							  group y by y.Activity into z
							  select z.FirstOrDefault();

			CheckInActivity[] ciaAll = { new CheckInActivity { Id = 0, Activity = ALL_ACTIVITIES } }; 
			IEnumerable<CheckInActivity> ieResults = ciaAll.AsEnumerable().Concat( acResults.AsEnumerable() );

			return ieResults;
		}

		public IQueryable<CheckInTime> Times()
		{
			if( dateEnd != null ) { dateEnd = dateEnd.Value.AddHours( 23 ); dateEnd = dateEnd.Value.AddMinutes( 59 ); dateEnd = dateEnd.Value.AddSeconds( 59 ); }

			var results = from y in DbUtil.Db.CheckInTimes 
							  where y.CheckInTimeX >= dateStart || dateStart == null
							  where y.CheckInTimeX <= dateEnd || dateEnd == null
							  where y.PeopleId == Person || Person == 0
							  select y;

			if( Pager.Direction == null ) Pager.Direction = "0";
			if( Pager.Sort == null ) Pager.Sort = "0";

			switch( Int32.Parse( Pager.Direction ) )
			{
				case 0:
				{
					switch( Int32.Parse( Pager.Sort ) )
					{
						case 0: results = from z in results orderby z.Id ascending select z; break;
						case 1: results = from z in results orderby z.Person.Name ascending select z; break;
						case 2: results = from z in results orderby z.CheckInTimeX ascending select z; break;
						case 3: results = from z in results orderby z.Location ascending select z; break;
						case 4: results = from z in results orderby z.CheckInActivities.FirstOrDefault().Activity ascending select z; break;
						case 5: results = from z in results orderby z.GuestOfId ascending select z; break;
					}
					break;
				}

				case 1:
				{
					switch( Int32.Parse( Pager.Sort ) )
					{
						case 0: results = from z in results orderby z.Id descending select z; break;
						case 1: results = from z in results orderby z.Person.Name descending select z; break;
						case 2: results = from z in results orderby z.CheckInTimeX descending select z; break;
						case 3: results = from z in results orderby z.Location descending select z; break;
						case 4: results = from z in results orderby z.CheckInActivities.FirstOrDefault().Activity descending select z; break;
						case 5: results = from z in results orderby z.GuestOf.Name descending select z; break;
					}
					break;
				}
			}

			if( Activity != null && !Activity.Equals( ALL_ACTIVITIES ) ) results = from x in results where x.CheckInActivities.Any( z => z.Activity == Activity ) select x;

			Pager.setCountDelegate( results.Count );
			return results.Skip( Pager.StartRow ).Take( Pager.PageSize );
		}
	}
}