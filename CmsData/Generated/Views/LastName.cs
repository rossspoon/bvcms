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
	[Table(Name="LastName")]
	public partial class LastName
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		
		private string _LastNameX;
		
		private int? _Count;
		
		
		public LastName()
		{
		}

		
		
		[Column(Name="LastName", Storage="_LastNameX", DbType="varchar(100) NOT NULL")]
		public string LastNameX
		{
			get
			{
				return this._LastNameX;
			}

			set
			{
				if (this._LastNameX != value)
					this._LastNameX = value;
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
