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
	[Table(Name="UserList")]
	public partial class UserList
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		
		private string _Username;
		
		private int _UserId;
		
		private string _FirstName;
		
		private string _LastName;
		
		private bool _IsApproved;
		
		private bool _MustChangePassword;
		
		private bool _IsLockedOut;
		
		private string _EmailAddress;
		
		private DateTime? _LastVisit;
		
		private int? _PeopleId;
		
		
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

		
		[Column(Name="UserId", Storage="_UserId", AutoSync=AutoSync.OnInsert, DbType="int NOT NULL IDENTITY", IsDbGenerated=true)]
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

		
		[Column(Name="EmailAddress", Storage="_EmailAddress", DbType="varchar(50)")]
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

		
		[Column(Name="LastVisit", Storage="_LastVisit", DbType="datetime")]
		public DateTime? LastVisit
		{
			get
			{
				return this._LastVisit;
			}

			set
			{
				if (this._LastVisit != value)
					this._LastVisit = value;
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

		
    }

}
