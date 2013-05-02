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
	[Table(Name="Addr")]
	public partial class Addr
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		
		private string _Street;
		
		private int? _Count;
		
		
		public Addr()
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
