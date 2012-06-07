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
		
		private int? _GroupId;
		
		private string _SavedBy;
		
		private string _Description;
		
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

		
		[Column(Name="Description", Storage="_Description", DbType="varchar(150)")]
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
