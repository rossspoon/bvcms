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
	[Table(Name="BlogCategoriesView")]
	public partial class BlogCategoriesView
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		
		private string _Category;
		
		private int? _N;
		
		
		public BlogCategoriesView()
		{
		}

		
		
		[Column(Name="Category", Storage="_Category", DbType="varchar(50)")]
		public string Category
		{
			get
			{
				return this._Category;
			}

			set
			{
				if (this._Category != value)
					this._Category = value;
			}

		}

		
		[Column(Name="n", Storage="_N", DbType="int")]
		public int? N
		{
			get
			{
				return this._N;
			}

			set
			{
				if (this._N != value)
					this._N = value;
			}

		}

		
    }

}
