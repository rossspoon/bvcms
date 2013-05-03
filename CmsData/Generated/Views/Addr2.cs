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
	[Table(Name="Addr2")]
	public partial class Addr2
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		
		private string _Street;
		
		private string _City;
		
		private string _State;
		
		private string _Zip;
		
		private int? _Count;
		
		
		public Addr2()
		{
		}

		
		
		[Column(Name="Street", Storage="_Street", DbType="varchar(50)")]
		public string Street
		{
			get
			{
				return this._Street;
			}

			set
			{
				if (this._Street != value)
					this._Street = value;
			}

		}

		
		[Column(Name="City", Storage="_City", DbType="varchar(50)")]
		public string City
		{
			get
			{
				return this._City;
			}

			set
			{
				if (this._City != value)
					this._City = value;
			}

		}

		
		[Column(Name="State", Storage="_State", DbType="varchar(5)")]
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

		
		[Column(Name="Zip", Storage="_Zip", DbType="varchar(50)")]
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
