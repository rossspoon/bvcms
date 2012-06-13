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
	[Table(Name="Contributions2")]
	public partial class Contributions2
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		
		private int _FamilyId;
		
		private int _PeopleId;
		
		private DateTime? _DateX;
		
		private int? _CreditGiverId;
		
		private string _HeadName;
		
		private string _SpouseName;
		
		private decimal? _Amount;
		
		private decimal? _PledgeAmount;
		
		private int _BundleHeaderId;
		
		private string _ContributionDesc;
		
		private int _FundId;
		
		private string _FundName;
		
		private string _BundleType;
		
		private string _BundleStatus;
		
		
		public Contributions2()
		{
		}

		
		
		[Column(Name="FamilyId", Storage="_FamilyId", DbType="int NOT NULL")]
		public int FamilyId
		{
			get
			{
				return this._FamilyId;
			}

			set
			{
				if (this._FamilyId != value)
					this._FamilyId = value;
			}

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

		
		[Column(Name="Date", Storage="_DateX", DbType="datetime")]
		public DateTime? DateX
		{
			get
			{
				return this._DateX;
			}

			set
			{
				if (this._DateX != value)
					this._DateX = value;
			}

		}

		
		[Column(Name="CreditGiverId", Storage="_CreditGiverId", DbType="int")]
		public int? CreditGiverId
		{
			get
			{
				return this._CreditGiverId;
			}

			set
			{
				if (this._CreditGiverId != value)
					this._CreditGiverId = value;
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

		
		[Column(Name="PledgeAmount", Storage="_PledgeAmount", DbType="Decimal(11,2)")]
		public decimal? PledgeAmount
		{
			get
			{
				return this._PledgeAmount;
			}

			set
			{
				if (this._PledgeAmount != value)
					this._PledgeAmount = value;
			}

		}

		
		[Column(Name="BundleHeaderId", Storage="_BundleHeaderId", DbType="int NOT NULL")]
		public int BundleHeaderId
		{
			get
			{
				return this._BundleHeaderId;
			}

			set
			{
				if (this._BundleHeaderId != value)
					this._BundleHeaderId = value;
			}

		}

		
		[Column(Name="ContributionDesc", Storage="_ContributionDesc", DbType="varchar(256)")]
		public string ContributionDesc
		{
			get
			{
				return this._ContributionDesc;
			}

			set
			{
				if (this._ContributionDesc != value)
					this._ContributionDesc = value;
			}

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

		
		[Column(Name="FundName", Storage="_FundName", DbType="varchar(256) NOT NULL")]
		public string FundName
		{
			get
			{
				return this._FundName;
			}

			set
			{
				if (this._FundName != value)
					this._FundName = value;
			}

		}

		
		[Column(Name="BundleType", Storage="_BundleType", DbType="varchar(50)")]
		public string BundleType
		{
			get
			{
				return this._BundleType;
			}

			set
			{
				if (this._BundleType != value)
					this._BundleType = value;
			}

		}

		
		[Column(Name="BundleStatus", Storage="_BundleStatus", DbType="varchar(50)")]
		public string BundleStatus
		{
			get
			{
				return this._BundleStatus;
			}

			set
			{
				if (this._BundleStatus != value)
					this._BundleStatus = value;
			}

		}

		
    }

}
