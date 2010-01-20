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
	[Table(Name="Sprocs")]
	public partial class Sproc
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		
		private string _Type;
		
		private string _Name;
		
		private string _Code;
		
		
		public Sproc()
		{
		}

		
		
		[Column(Name="type", Storage="_Type", DbType="nvarchar(20)")]
		public string Type
		{
			get
			{
				return this._Type;
			}

			set
			{
				if (this._Type != value)
					this._Type = value;
			}

		}

		
		[Column(Name="Name", Storage="_Name", DbType="nvarchar(128) NOT NULL")]
		public string Name
		{
			get
			{
				return this._Name;
			}

			set
			{
				if (this._Name != value)
					this._Name = value;
			}

		}

		
		[Column(Name="Code", Storage="_Code", DbType="nvarchar(4000)")]
		public string Code
		{
			get
			{
				return this._Code;
			}

			set
			{
				if (this._Code != value)
					this._Code = value;
			}

		}

		
    }

}
