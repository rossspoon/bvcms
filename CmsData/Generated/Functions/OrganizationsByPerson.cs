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
	[Table(Name="OrganizationsByPerson")]
	public partial class OrganizationsByPerson
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		
		private int _OrganizationId;
		
		private string _OrganizationName;
		
		private string _Location;
		
		private int? _LeaderId;
		
		private string _LeaderFirst;
		
		private string _LeaderLast;
		
		private string _ScheduleName;
		
		
		public OrganizationsByPerson()
		{
		}

		
		
		[Column(Name="ORGANIZATION_ID", Storage="_OrganizationId", DbType="int NOT NULL")]
		public int OrganizationId
		{
			get
			{
				return this._OrganizationId;
			}

			set
			{
				if (this._OrganizationId != value)
					this._OrganizationId = value;
			}

		}

		
		[Column(Name="ORGANIZATION_NAME", Storage="_OrganizationName", DbType="varchar(40) NOT NULL")]
		public string OrganizationName
		{
			get
			{
				return this._OrganizationName;
			}

			set
			{
				if (this._OrganizationName != value)
					this._OrganizationName = value;
			}

		}

		
		[Column(Name="LOCATION", Storage="_Location", DbType="varchar(40)")]
		public string Location
		{
			get
			{
				return this._Location;
			}

			set
			{
				if (this._Location != value)
					this._Location = value;
			}

		}

		
		[Column(Name="LeaderId", Storage="_LeaderId", DbType="int")]
		public int? LeaderId
		{
			get
			{
				return this._LeaderId;
			}

			set
			{
				if (this._LeaderId != value)
					this._LeaderId = value;
			}

		}

		
		[Column(Name="LeaderFirst", Storage="_LeaderFirst", DbType="varchar(15)")]
		public string LeaderFirst
		{
			get
			{
				return this._LeaderFirst;
			}

			set
			{
				if (this._LeaderFirst != value)
					this._LeaderFirst = value;
			}

		}

		
		[Column(Name="LeaderLast", Storage="_LeaderLast", DbType="varchar(20)")]
		public string LeaderLast
		{
			get
			{
				return this._LeaderLast;
			}

			set
			{
				if (this._LeaderLast != value)
					this._LeaderLast = value;
			}

		}

		
		[Column(Name="SCHEDULE_NAME", Storage="_ScheduleName", DbType="varchar(40)")]
		public string ScheduleName
		{
			get
			{
				return this._ScheduleName;
			}

			set
			{
				if (this._ScheduleName != value)
					this._ScheduleName = value;
			}

		}

		
    }

}
