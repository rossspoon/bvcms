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
	[Table(Name="FirstTimeGivers")]
	public partial class FirstTimeGiver
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		
		private int _PeopleId;
		
		private DateTime? _FirstDate;
		
		private decimal? _Amt;
		
		
		public FirstTimeGiver()
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

		
		[Column(Name="FirstDate", Storage="_FirstDate", DbType="datetime")]
		public DateTime? FirstDate
		{
			get
			{
				return this._FirstDate;
			}

			set
			{
				if (this._FirstDate != value)
					this._FirstDate = value;
			}

		}

		
		[Column(Name="Amt", Storage="_Amt", DbType="Decimal(38,2)")]
		public decimal? Amt
		{
			get
			{
				return this._Amt;
			}

			set
			{
				if (this._Amt != value)
					this._Amt = value;
			}

		}

		
    }

}
