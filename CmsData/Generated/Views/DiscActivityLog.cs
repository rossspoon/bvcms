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
	[Table(Name="DiscActivityLog")]
	public partial class DiscActivityLog
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		
		private string _Name2;
		
		private string _PageUrl;
		
		private string _PageTitle;
		
		private DateTime? _VisitTime;
		
		
		public DiscActivityLog()
		{
		}

		
		
		[Column(Name="Name2", Storage="_Name2", DbType="varchar(50)")]
		public string Name2
		{
			get
			{
				return this._Name2;
			}

			set
			{
				if (this._Name2 != value)
					this._Name2 = value;
			}

		}

		
		[Column(Name="PageUrl", Storage="_PageUrl", DbType="varchar(150)")]
		public string PageUrl
		{
			get
			{
				return this._PageUrl;
			}

			set
			{
				if (this._PageUrl != value)
					this._PageUrl = value;
			}

		}

		
		[Column(Name="PageTitle", Storage="_PageTitle", DbType="varchar(100) NOT NULL")]
		public string PageTitle
		{
			get
			{
				return this._PageTitle;
			}

			set
			{
				if (this._PageTitle != value)
					this._PageTitle = value;
			}

		}

		
		[Column(Name="VisitTime", Storage="_VisitTime", DbType="datetime")]
		public DateTime? VisitTime
		{
			get
			{
				return this._VisitTime;
			}

			set
			{
				if (this._VisitTime != value)
					this._VisitTime = value;
			}

		}

		
    }

}
