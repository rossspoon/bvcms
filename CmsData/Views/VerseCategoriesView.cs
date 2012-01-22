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
	[Table(Name="VerseCategoriesView")]
	public partial class VerseCategoriesView
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		
		private int _Id;
		
		private string _DisplayName;
		
		private int? _VerseCount;
		
		private string _Name;
		
		private string _Username;
		
		
		public VerseCategoriesView()
		{
		}

		
		
		[Column(Name="id", Storage="_Id", DbType="int NOT NULL")]
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

		
		[Column(Name="DisplayName", Storage="_DisplayName", DbType="nvarchar(142)")]
		public string DisplayName
		{
			get
			{
				return this._DisplayName;
			}

			set
			{
				if (this._DisplayName != value)
					this._DisplayName = value;
			}

		}

		
		[Column(Name="VerseCount", Storage="_VerseCount", DbType="int")]
		public int? VerseCount
		{
			get
			{
				return this._VerseCount;
			}

			set
			{
				if (this._VerseCount != value)
					this._VerseCount = value;
			}

		}

		
		[Column(Name="Name", Storage="_Name", DbType="nvarchar(100)")]
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

		
		[Column(Name="Username", Storage="_Username", DbType="varchar(50) NOT NULL")]
		public string Username
		{
			get
			{
				return this._Username;
			}

			set
			{
				if (this._Username != value)
					this._Username = value;
			}

		}

		
    }

}
