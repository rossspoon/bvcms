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
	[Table(Name="GetTodaysMeetingHours3")]
	public partial class GetTodaysMeetingHours3
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		
		private DateTime? _Hour;
		
		private int? _OrganizationId;
		
		
		public GetTodaysMeetingHours3()
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

		
		[Column(Name="OrganizationId", Storage="_OrganizationId", DbType="int")]
		public int? OrganizationId
		{
			get
			{
				return this._OrganizationId;
			}

			set
			{
				if (this._OrganizationId != value)
					this._OrganizationId = value;
			}

		}

		
    }

}
