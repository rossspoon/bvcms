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
	[Table(Name="City")]
	public partial class City
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		
		private string _CityX;
		
		private string _State;
		
		private string _Zip;
		
		private int? _Count;
		
		
		public City()
		{
		}

		
		
		[Column(Name="City", Storage="_CityX", DbType="varchar(30)")]
		public string CityX
		{
			get
			{
				return this._CityX;
			}

			set
			{
				if (this._CityX != value)
					this._CityX = value;
			}

		}

		
		[Column(Name="State", Storage="_State", DbType="varchar(20)")]
		public string State
		{
			get
			{
				return this._State;
			}

			set
			{
				if (this._State != value)
					this._State = value;
			}

		}

		
		[Column(Name="Zip", Storage="_Zip", DbType="varchar(15)")]
		public string Zip
		{
			get
			{
				return this._Zip;
			}

			set
			{
				if (this._Zip != value)
					this._Zip = value;
			}

		}

		
		[Column(Name="count", Storage="_Count", DbType="int")]
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

		
    }

}
