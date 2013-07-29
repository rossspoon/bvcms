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
	[Table(Name="FirstName2")]
	public partial class FirstName2
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		
		private string _FirstName;
		
		private int _GenderId;
		
		private string _Ca;
		
		private int? _Expr1;
		
		
		public FirstName2()
		{
		}

		
		
		[Column(Name="FirstName", Storage="_FirstName", DbType="nvarchar(25)")]
		public string FirstName
		{
			get
			{
				return this._FirstName;
			}

			set
			{
				if (this._FirstName != value)
					this._FirstName = value;
			}

		}

		
		[Column(Name="GenderId", Storage="_GenderId", DbType="int NOT NULL")]
		public int GenderId
		{
			get
			{
				return this._GenderId;
			}

			set
			{
				if (this._GenderId != value)
					this._GenderId = value;
			}

		}

		
		[Column(Name="CA", Storage="_Ca", DbType="varchar(1) NOT NULL")]
		public string Ca
		{
			get
			{
				return this._Ca;
			}

			set
			{
				if (this._Ca != value)
					this._Ca = value;
			}

		}

		
		[Column(Name="Expr1", Storage="_Expr1", DbType="int")]
		public int? Expr1
		{
			get
			{
				return this._Expr1;
			}

			set
			{
				if (this._Expr1 != value)
					this._Expr1 = value;
			}

		}

		
    }

}
