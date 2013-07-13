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
	[Table(Name="UserRoles")]
	public partial class UserRole
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		
		private string _Name;
		
		private string _EmailAddress;
		
		private string _Roles;
		
		
		public UserRole()
		{
		}

		
		
		[Column(Name="Name", Storage="_Name", DbType="nvarchar(50)")]
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

		
		[Column(Name="EmailAddress", Storage="_EmailAddress", DbType="nvarchar(100)")]
		public string EmailAddress
		{
			get
			{
				return this._EmailAddress;
			}

			set
			{
				if (this._EmailAddress != value)
					this._EmailAddress = value;
			}

		}

		
		[Column(Name="Roles", Storage="_Roles", DbType="nvarchar")]
		public string Roles
		{
			get
			{
				return this._Roles;
			}

			set
			{
				if (this._Roles != value)
					this._Roles = value;
			}

		}

		
    }

}
