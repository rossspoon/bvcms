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
	[Table(Name="ViewPageVisit")]
	public partial class ViewPageVisit
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		
		private string _FirstName;
		
		private string _LastName;
		
		private string _PageTitle;
		
		private DateTime _CreatedOn;
		
		private string _PageUrl;
		
		private DateTime? _VisitTime;
		
		
		public ViewPageVisit()
		{
		}

		
		
		[Column(Name="FirstName", Storage="_FirstName", DbType="varchar(50)")]
		public string FirstName
		{
			get
			{
				return this._FirstName;
			}

			set
			{
				if (this._FirstName != value)
					this._FirstName = value;
			}

		}

		
		[Column(Name="LastName", Storage="_LastName", DbType="varchar(50)")]
		public string LastName
		{
			get
			{
				return this._LastName;
			}

			set
			{
				if (this._LastName != value)
					this._LastName = value;
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

		
		[Column(Name="CreatedOn", Storage="_CreatedOn", DbType="datetime NOT NULL")]
		public DateTime CreatedOn
		{
			get
			{
				return this._CreatedOn;
			}

			set
			{
				if (this._CreatedOn != value)
					this._CreatedOn = value;
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
