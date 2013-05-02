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
	[Table(Name="MasterOrgs")]
	public partial class MasterOrg
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		
		private int _OrganizationId;
		
		private int? _PickListOrgId;
		
		private string _OrganizationName;
		
		
		public MasterOrg()
		{
		}

		
		
		[Column(Name="OrganizationId", Storage="_OrganizationId", DbType="int NOT NULL")]
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

		
		[Column(Name="PickListOrgId", Storage="_PickListOrgId", DbType="int")]
		public int? PickListOrgId
		{
			get
			{
				return this._PickListOrgId;
			}

			set
			{
				if (this._PickListOrgId != value)
					this._PickListOrgId = value;
			}

		}

		
		[Column(Name="OrganizationName", Storage="_OrganizationName", DbType="varchar(100) NOT NULL")]
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

		
    }

}
