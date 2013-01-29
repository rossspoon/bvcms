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
	[Table(Name="ContributionsView")]
	public partial class ContributionsView
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		
		private int _FundId;
		
		private int _TypeId;
		
		private DateTime? _CDate;
		
		private decimal? _Amount;
		
		private int? _StatusId;
		
		private bool? _Pledged;
		
		private int? _Age;
		
		private int? _AgeRange;
		
		private string _Fund;
		
		private string _Status;
		
		private string _Type;
		
		private string _Name;
		
		
		public ContributionsView()
		{
		}

		
		
		[Column(Name="FundId", Storage="_FundId", DbType="int NOT NULL")]
		public int FundId
		{
			get
			{
				return this._FundId;
			}

			set
			{
				if (this._FundId != value)
					this._FundId = value;
			}

		}

		
		[Column(Name="TypeId", Storage="_TypeId", DbType="int NOT NULL")]
		public int TypeId
		{
			get
			{
				return this._TypeId;
			}

			set
			{
				if (this._TypeId != value)
					this._TypeId = value;
			}

		}

		
		[Column(Name="CDate", Storage="_CDate", DbType="datetime")]
		public DateTime? CDate
		{
			get
			{
				return this._CDate;
			}

			set
			{
				if (this._CDate != value)
					this._CDate = value;
			}

		}

		
		[Column(Name="Amount", Storage="_Amount", DbType="Decimal(11,2)")]
		public decimal? Amount
		{
			get
			{
				return this._Amount;
			}

			set
			{
				if (this._Amount != value)
					this._Amount = value;
			}

		}

		
		[Column(Name="StatusId", Storage="_StatusId", DbType="int")]
		public int? StatusId
		{
			get
			{
				return this._StatusId;
			}

			set
			{
				if (this._StatusId != value)
					this._StatusId = value;
			}

		}

		
		[Column(Name="Pledged", Storage="_Pledged", DbType="bit")]
		public bool? Pledged
		{
			get
			{
				return this._Pledged;
			}

			set
			{
				if (this._Pledged != value)
					this._Pledged = value;
			}

		}

		
		[Column(Name="Age", Storage="_Age", DbType="int")]
		public int? Age
		{
			get
			{
				return this._Age;
			}

			set
			{
				if (this._Age != value)
					this._Age = value;
			}

		}

		
		[Column(Name="AgeRange", Storage="_AgeRange", DbType="int")]
		public int? AgeRange
		{
			get
			{
				return this._AgeRange;
			}

			set
			{
				if (this._AgeRange != value)
					this._AgeRange = value;
			}

		}

		
		[Column(Name="Fund", Storage="_Fund", DbType="varchar(256)")]
		public string Fund
		{
			get
			{
				return this._Fund;
			}

			set
			{
				if (this._Fund != value)
					this._Fund = value;
			}

		}

		
		[Column(Name="Status", Storage="_Status", DbType="varchar(50)")]
		public string Status
		{
			get
			{
				return this._Status;
			}

			set
			{
				if (this._Status != value)
					this._Status = value;
			}

		}

		
		[Column(Name="Type", Storage="_Type", DbType="varchar(50)")]
		public string Type
		{
			get
			{
				return this._Type;
			}

			set
			{
				if (this._Type != value)
					this._Type = value;
			}

		}

		
		[Column(Name="Name", Storage="_Name", DbType="varchar(126)")]
		public string Name
		{
			get
			{
				return this._Name;
			}

			set
			{
				if (this._Name != value)
					this._Name = value;
			}

		}

		
    }

}
