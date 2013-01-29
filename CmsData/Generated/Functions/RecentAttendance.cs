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
	[Table(Name="RecentAttendance")]
	public partial class RecentAttendance
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		
		private int _PeopleId;
		
		private DateTime? _Lastattend;
		
		private decimal? _Attendpct;
		
		private string _Attendstr;
		
		private string _Attendtype;
		
		private int _Visitor;
		
		
		public RecentAttendance()
		{
		}

		
		
		[Column(Name="PeopleId", Storage="_PeopleId", DbType="int NOT NULL")]
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

		
		[Column(Name="lastattend", Storage="_Lastattend", DbType="datetime")]
		public DateTime? Lastattend
		{
			get
			{
				return this._Lastattend;
			}

			set
			{
				if (this._Lastattend != value)
					this._Lastattend = value;
			}

		}

		
		[Column(Name="attendpct", Storage="_Attendpct", DbType="real")]
		public decimal? Attendpct
		{
			get
			{
				return this._Attendpct;
			}

			set
			{
				if (this._Attendpct != value)
					this._Attendpct = value;
			}

		}

		
		[Column(Name="attendstr", Storage="_Attendstr", DbType="varchar(200)")]
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

		
		[Column(Name="attendtype", Storage="_Attendtype", DbType="varchar(100)")]
		public string Attendtype
		{
			get
			{
				return this._Attendtype;
			}

			set
			{
				if (this._Attendtype != value)
					this._Attendtype = value;
			}

		}

		
		[Column(Name="visitor", Storage="_Visitor", DbType="int NOT NULL")]
		public int Visitor
		{
			get
			{
				return this._Visitor;
			}

			set
			{
				if (this._Visitor != value)
					this._Visitor = value;
			}

		}

		
    }

}
