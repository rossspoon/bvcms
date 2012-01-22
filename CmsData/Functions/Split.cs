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
	[Table(Name="Split")]
	public partial class Split
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		
		private int _TokenID;
		
		private string _ValueX;
		
		
		public Split()
		{
		}

		
		
		[Column(Name="TokenID", Storage="_TokenID", AutoSync=AutoSync.OnInsert, DbType="int NOT NULL IDENTITY", IsDbGenerated=true)]
		public int TokenID
		{
			get
			{
				return this._TokenID;
			}

			set
			{
				if (this._TokenID != value)
					this._TokenID = value;
			}

		}

		
		[Column(Name="Value", Storage="_ValueX", DbType="varchar(4000)")]
		public string ValueX
		{
			get
			{
				return this._ValueX;
			}

			set
			{
				if (this._ValueX != value)
					this._ValueX = value;
			}

		}

		
    }

}
