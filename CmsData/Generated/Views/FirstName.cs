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
	[Table(Name="FirstName")]
	public partial class FirstName
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		
		private string _FirstNameX;
		
		private int? _Count;
		
		
		public FirstName()
		{
		}

		
		
		[Column(Name="FirstName", Storage="_FirstNameX", DbType="varchar(25)")]
		public string FirstNameX
		{
			get
			{
				return this._FirstNameX;
			}

			set
			{
				if (this._FirstNameX != value)
					this._FirstNameX = value;
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
