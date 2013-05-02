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
	[Table(Name="RandNumber")]
	public partial class RandNumber
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		
		private double? _RandNumberX;
		
		
		public RandNumber()
		{
		}

		
		
		[Column(Name="RandNumber", Storage="_RandNumberX", DbType="float")]
		public double? RandNumberX
		{
			get
			{
				return this._RandNumberX;
			}

			set
			{
				if (this._RandNumberX != value)
					this._RandNumberX = value;
			}

		}

		
    }

}
