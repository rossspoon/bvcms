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
	[Table(Name="GivingCurrentPercentOfFormer")]
	public partial class GivingCurrentPercentOfFormer
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		
		private int _Pid;
		
		private decimal? _C1amt;
		
		private decimal? _C2amt;
		
		private decimal? _Pct;
		
		
		public GivingCurrentPercentOfFormer()
		{
		}

		
		
		[Column(Name="pid", Storage="_Pid", DbType="int NOT NULL")]
		public int Pid
		{
			get
			{
				return this._Pid;
			}

			set
			{
				if (this._Pid != value)
					this._Pid = value;
			}

		}

		
		[Column(Name="c1amt", Storage="_C1amt", DbType="Decimal(38,2)")]
		public decimal? C1amt
		{
			get
			{
				return this._C1amt;
			}

			set
			{
				if (this._C1amt != value)
					this._C1amt = value;
			}

		}

		
		[Column(Name="c2amt", Storage="_C2amt", DbType="Decimal(38,2)")]
		public decimal? C2amt
		{
			get
			{
				return this._C2amt;
			}

			set
			{
				if (this._C2amt != value)
					this._C2amt = value;
			}

		}

		
		[Column(Name="pct", Storage="_Pct", DbType="Decimal(38,6)")]
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
