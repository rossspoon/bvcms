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
	[Table(Name="UserList")]
	public partial class UserList
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		
		private string _Username;
		
		private int _UserId;
		
		private string _Name;
		
		private string _Name2;
		
		private bool _IsApproved;
		
		private bool _MustChangePassword;
		
		private bool _IsLockedOut;
		
		private string _EmailAddress;
		
		private DateTime? _LastActivityDate;
		
		private int? _PeopleId;
		
		private string _Roles;
		
		
		public UserList()
		{
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

		
		[Column(Name="UserId", Storage="_UserId", DbType="int NOT NULL")]
		public int UserId
		{
			get
			{
				return this._UserId;
			}

			set
			{
				if (this._UserId != value)
					this._UserId = value;
			}

		}

		
		[Column(Name="Name", Storage="_Name", DbType="varchar(56)")]
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

		
		[Column(Name="Name2", Storage="_Name2", DbType="varchar(57)")]
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

		
		[Column(Name="IsApproved", Storage="_IsApproved", DbType="bit NOT NULL")]
		public bool IsApproved
		{
			get
			{
				return this._IsApproved;
			}

			set
			{
				if (this._IsApproved != value)
					this._IsApproved = value;
			}

		}

		
		[Column(Name="MustChangePassword", Storage="_MustChangePassword", DbType="bit NOT NULL")]
		public bool MustChangePassword
		{
			get
			{
				return this._MustChangePassword;
			}

			set
			{
				if (this._MustChangePassword != value)
					this._MustChangePassword = value;
			}

		}

		
		[Column(Name="IsLockedOut", Storage="_IsLockedOut", DbType="bit NOT NULL")]
		public bool IsLockedOut
		{
			get
			{
				return this._IsLockedOut;
			}

			set
			{
				if (this._IsLockedOut != value)
					this._IsLockedOut = value;
			}

		}

		
		[Column(Name="EmailAddress", Storage="_EmailAddress", DbType="varchar(150)")]
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

		
		[Column(Name="LastActivityDate", Storage="_LastActivityDate", DbType="datetime")]
		public DateTime? LastActivityDate
		{
			get
			{
				return this._LastActivityDate;
			}

			set
			{
				if (this._LastActivityDate != value)
					this._LastActivityDate = value;
			}

		}

		
		[Column(Name="PeopleId", Storage="_PeopleId", DbType="int")]
		public int? PeopleId
		{
			get
			{
				return this._PeopleId;
			}

			set
			{
				if (this._PeopleId != value)
					this._PeopleId = value;
			}

		}

		
		[Column(Name="Roles", Storage="_Roles", DbType="varchar(500)")]
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
