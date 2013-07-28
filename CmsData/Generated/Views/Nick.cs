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
	[Table(Name="Nick")]
	public partial class Nick
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		
		private string _NickName;
		
		private int? _Count;
		
		
		public Nick()
		{
		}

		
		
		[Column(Name="NickName", Storage="_NickName", DbType="nvarchar(25)")]
		public string NickName
		{
			get
			{
				return this._NickName;
			}

			set
			{
				if (this._NickName != value)
					this._NickName = value;
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
