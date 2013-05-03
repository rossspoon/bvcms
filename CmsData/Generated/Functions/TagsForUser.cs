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
	[Table(Name="TagsForUser")]
	public partial class TagsForUser
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		
		private int _Id;
		
		private string _Name;
		
		private int _TypeId;
		
		private string _Owner;
		
		
		public TagsForUser()
		{
		}

		
		
		[Column(Name="Id", Storage="_Id", DbType="int NOT NULL")]
		public int Id
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

		
		[Column(Name="Name", Storage="_Name", DbType="varchar(50) NOT NULL")]
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

		
		[Column(Name="TypeId", Storage="_TypeId", DbType="int NOT NULL")]
		public int TypeId
		{
			get
			{
				return this._TypeId;
			}

			set
			{
				if (this._TypeId != value)
					this._TypeId = value;
			}

		}

		
		[Column(Name="Owner", Storage="_Owner", DbType="varchar(50)")]
		public string Owner
		{
			get
			{
				return this._Owner;
			}

			set
			{
				if (this._Owner != value)
					this._Owner = value;
			}

		}

		
    }

}
