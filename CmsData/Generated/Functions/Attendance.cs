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
	[Table(Name="Attendance")]
	public partial class Attendance
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		
		private int _PeopleId;
		
		private string _Attendstr;
		
		private decimal? _Pct;
		
		
		public Attendance()
		{
		}

		
		
		[Column(Name="People_id", Storage="_PeopleId", DbType="int NOT NULL")]
		public int PeopleId
		{
			get
			{
				return this._PeopleId;
			}

			set
			{
				if (this._PeopleId != value)
					this._PeopleId = value;
			}

		}

		
		[Column(Name="attendstr", Storage="_Attendstr", DbType="varchar(52)")]
		public string Attendstr
		{
			get
			{
				return this._Attendstr;
			}

			set
			{
				if (this._Attendstr != value)
					this._Attendstr = value;
			}

		}

		
		[Column(Name="pct", Storage="_Pct", DbType="real")]
		public decimal? Pct
		{
			get
			{
				return this._Pct;
			}

			set
			{
				if (this._Pct != value)
					this._Pct = value;
			}

		}

		
    }

}
