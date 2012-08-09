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
	[Table(Name="PledgeFulfillment")]
	public partial class PledgeFulfillment
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		
		private int _PeopleId;
		
		private string _HeadName;
		
		private string _SpouseName;
		
		private DateTime? _PledgeDate;
		
		private decimal? _PledgeAmt;
		
		private DateTime _LastDate;
		
		private decimal? _TotalGiven;
		
		private decimal? _Balance;
		
		
		public PledgeFulfillment()
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

		
		[Column(Name="HeadName", Storage="_HeadName", DbType="varchar(127)")]
		public string HeadName
		{
			get
			{
				return this._HeadName;
			}

			set
			{
				if (this._HeadName != value)
					this._HeadName = value;
			}

		}

		
		[Column(Name="SpouseName", Storage="_SpouseName", DbType="varchar(127)")]
		public string SpouseName
		{
			get
			{
				return this._SpouseName;
			}

			set
			{
				if (this._SpouseName != value)
					this._SpouseName = value;
			}

		}

		
		[Column(Name="PledgeDate", Storage="_PledgeDate", DbType="datetime")]
		public DateTime? PledgeDate
		{
			get
			{
				return this._PledgeDate;
			}

			set
			{
				if (this._PledgeDate != value)
					this._PledgeDate = value;
			}

		}

		
		[Column(Name="PledgeAmt", Storage="_PledgeAmt", DbType="Decimal(38,2)")]
		public decimal? PledgeAmt
		{
			get
			{
				return this._PledgeAmt;
			}

			set
			{
				if (this._PledgeAmt != value)
					this._PledgeAmt = value;
			}

		}

		
		[Column(Name="LastDate", Storage="_LastDate", DbType="datetime NOT NULL")]
		public DateTime LastDate
		{
			get
			{
				return this._LastDate;
			}

			set
			{
				if (this._LastDate != value)
					this._LastDate = value;
			}

		}

		
		[Column(Name="TotalGiven", Storage="_TotalGiven", DbType="Decimal(38,2)")]
		public decimal? TotalGiven
		{
			get
			{
				return this._TotalGiven;
			}

			set
			{
				if (this._TotalGiven != value)
					this._TotalGiven = value;
			}

		}

		
		[Column(Name="Balance", Storage="_Balance", DbType="Decimal(38,2)")]
		public decimal? Balance
		{
			get
			{
				return this._Balance;
			}

			set
			{
				if (this._Balance != value)
					this._Balance = value;
			}

		}

		
    }

}
