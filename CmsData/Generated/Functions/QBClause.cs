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
	[Table(Name="QBClauses")]
	public partial class QBClause
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		
		private int? _QueryId;
		
		private int? _TopId;
		
		private int? _GroupId;
		
		private string _SavedBy;
		
		private string _Description;
		
		private string _Field;
		
		private string _Comparison;
		
		private int? _Level;
		
		
		public QBClause()
		{
		}

		
		
		[Column(Name="QueryId", Storage="_QueryId", DbType="int")]
		public int? QueryId
		{
			get
			{
				return this._QueryId;
			}

			set
			{
				if (this._QueryId != value)
					this._QueryId = value;
			}

		}

		
		[Column(Name="TopId", Storage="_TopId", DbType="int")]
		public int? TopId
		{
			get
			{
				return this._TopId;
			}

			set
			{
				if (this._TopId != value)
					this._TopId = value;
			}

		}

		
		[Column(Name="GroupId", Storage="_GroupId", DbType="int")]
		public int? GroupId
		{
			get
			{
				return this._GroupId;
			}

			set
			{
				if (this._GroupId != value)
					this._GroupId = value;
			}

		}

		
		[Column(Name="SavedBy", Storage="_SavedBy", DbType="varchar(50)")]
		public string SavedBy
		{
			get
			{
				return this._SavedBy;
			}

			set
			{
				if (this._SavedBy != value)
					this._SavedBy = value;
			}

		}

		
		[Column(Name="DESCRIPTION", Storage="_Description", DbType="varchar(150)")]
		public string Description
		{
			get
			{
				return this._Description;
			}

			set
			{
				if (this._Description != value)
					this._Description = value;
			}

		}

		
		[Column(Name="Field", Storage="_Field", DbType="varchar(32)")]
		public string Field
		{
			get
			{
				return this._Field;
			}

			set
			{
				if (this._Field != value)
					this._Field = value;
			}

		}

		
		[Column(Name="Comparison", Storage="_Comparison", DbType="varchar(20)")]
		public string Comparison
		{
			get
			{
				return this._Comparison;
			}

			set
			{
				if (this._Comparison != value)
					this._Comparison = value;
			}

		}

		
		[Column(Name="Level", Storage="_Level", DbType="int")]
		public int? Level
		{
			get
			{
				return this._Level;
			}

			set
			{
				if (this._Level != value)
					this._Level = value;
			}

		}

		
    }

}
