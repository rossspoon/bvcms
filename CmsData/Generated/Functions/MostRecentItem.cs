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
	[Table(Name="MostRecentItems")]
	public partial class MostRecentItem
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		
		private int? _Id;
		
		private string _Name;
		
		private string _Type;
		
		
		public MostRecentItem()
		{
		}

		
		
		[Column(Name="Id", Storage="_Id", DbType="int")]
		public int? Id
		{
			get
			{
				return this._Id;
			}

			set
			{
				if (this._Id != value)
					this._Id = value;
			}

		}

		
		[Column(Name="Name", Storage="_Name", DbType="varchar(150)")]
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

		
		[Column(Name="Type", Storage="_Type", DbType="varchar(3)")]
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

		
    }

}
