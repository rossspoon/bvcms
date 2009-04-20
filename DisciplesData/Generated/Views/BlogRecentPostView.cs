using System; 
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel;

namespace DiscData.View
{
	[Table(Name="BlogRecentPostView")]
	public partial class BlogRecentPostView
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		
		private int _Id;
		
		private string _Title;
		
		private int _BlogId;
		
		private DateTime? _EntryDate;
		
		private string _Poster;
		
		
		public BlogRecentPostView()
		{
		}

		
		
		[Column(Name="Id", Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="int NOT NULL IDENTITY", IsDbGenerated=true)]
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

		
		[Column(Name="Title", Storage="_Title", DbType="nvarchar(250)")]
		public string Title
		{
			get
			{
				return this._Title;
			}

			set
			{
				if (this._Title != value)
					this._Title = value;
			}

		}

		
		[Column(Name="BlogId", Storage="_BlogId", DbType="int NOT NULL")]
		public int BlogId
		{
			get
			{
				return this._BlogId;
			}

			set
			{
				if (this._BlogId != value)
					this._BlogId = value;
			}

		}

		
		[Column(Name="EntryDate", Storage="_EntryDate", DbType="datetime")]
		public DateTime? EntryDate
		{
			get
			{
				return this._EntryDate;
			}

			set
			{
				if (this._EntryDate != value)
					this._EntryDate = value;
			}

		}

		
		[Column(Name="Poster", Storage="_Poster", DbType="varchar(50)")]
		public string Poster
		{
			get
			{
				return this._Poster;
			}

			set
			{
				if (this._Poster != value)
					this._Poster = value;
			}

		}

		
    }

}
