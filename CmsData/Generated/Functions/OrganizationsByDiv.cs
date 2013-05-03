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
	[Table(Name="OrganizationsByDiv")]
	public partial class OrganizationsByDiv
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		
		private int _OrganizationId;
		
		private string _OrganizationName;
		
		private string _Location;
		
		private string _OrganizationCode;
		
		private int? _LeaderId;
		
		private string _FirstName;
		
		private string _LastName;
		
		private int? _MemberCount;
		
		
		public OrganizationsByDiv()
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

		
		[Column(Name="ORGANIZATION_CODE", Storage="_OrganizationCode", DbType="varchar(10) NOT NULL")]
		public string OrganizationCode
		{
			get
			{
				return this._OrganizationCode;
			}

			set
			{
				if (this._OrganizationCode != value)
					this._OrganizationCode = value;
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

		
		[Column(Name="FIRST_NAME", Storage="_FirstName", DbType="varchar(15)")]
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

		
		[Column(Name="LAST_NAME", Storage="_LastName", DbType="varchar(20)")]
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

		
		[Column(Name="MemberCount", Storage="_MemberCount", DbType="int")]
		public int? MemberCount
		{
			get
			{
				return this._MemberCount;
			}

			set
			{
				if (this._MemberCount != value)
					this._MemberCount = value;
			}

		}

		
    }

}
