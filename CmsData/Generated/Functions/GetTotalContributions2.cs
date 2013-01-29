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
	[Table(Name="GetTotalContributions2")]
	public partial class GetTotalContributions2
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		
		private int? _CreditGiverId;
		
		private string _HeadName;
		
		private string _SpouseName;
		
		private int? _Count;
		
		private decimal? _Amount;
		
		private decimal? _PledgeAmount;
		
		private int _FundId;
		
		private string _FundName;
		
		private int _QBSynced;
		
		
		public GetTotalContributions2()
		{
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

		
		[Column(Name="HeadName", Storage="_HeadName", DbType="varchar(139)")]
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

		
		[Column(Name="SpouseName", Storage="_SpouseName", DbType="varchar(139)")]
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

		
		[Column(Name="Count", Storage="_Count", DbType="int")]
		public int? Count
		{
			get
			{
				return this._Count;
			}

			set
			{
				if (this._Count != value)
					this._Count = value;
			}

		}

		
		[Column(Name="Amount", Storage="_Amount", DbType="Decimal(38,2)")]
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

		
		[Column(Name="PledgeAmount", Storage="_PledgeAmount", DbType="Decimal(38,2)")]
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

		
		[Column(Name="QBSynced", Storage="_QBSynced", DbType="int NOT NULL")]
		public int QBSynced
		{
			get
			{
				return this._QBSynced;
			}

			set
			{
				if (this._QBSynced != value)
					this._QBSynced = value;
			}

		}

		
    }

}
