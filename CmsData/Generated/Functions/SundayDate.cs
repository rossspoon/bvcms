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
	[Table(Name="SundayDates")]
	public partial class SundayDate
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		
		private DateTime? _Dt;
		
		
		public SundayDate()
		{
		}

		
		
		[Column(Name="dt", Storage="_Dt", DbType="datetime")]
		public DateTime? Dt
		{
			get
			{
				return this._Dt;
			}

			set
			{
				if (this._Dt != value)
					this._Dt = value;
			}

		}

		
    }

}
