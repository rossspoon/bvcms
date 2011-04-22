using System; 
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel;

namespace CmsData.View
{
	[Table(Name="GetTodaysMeetingHours")]
	public partial class GetTodaysMeetingHour
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		
		private DateTime? _Hour;
		
		
		public GetTodaysMeetingHour()
		{
		}

		
		
		[Column(Name="hour", Storage="_Hour", DbType="datetime")]
		public DateTime? Hour
		{
			get
			{
				return this._Hour;
			}

			set
			{
				if (this._Hour != value)
					this._Hour = value;
			}

		}

		
    }

}
