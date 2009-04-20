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
	[Table(Name="BadEtsList")]
	public partial class BadEtsList
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		
		private int? _Id;
		
		private int? _Flag;
		
		private int _PeopleId;
		
		private int _OrganizationId;
		
		private int _TransactionId;
		
		private string _OrganizationName;
		
		private string _Name2;
		
		private bool? _Status;
		
		private DateTime _TransactionDate;
		
		private int _TransactionTypeId;
		
		private bool _TransactionStatus;
		
		
		public BadEtsList()
		{
		}

		
		
		[Column(Name="id", Storage="_Id", DbType="int")]
		public int? Id
		{
			get
			{
				return this._Id;
			}

			set
			{
				if (this._Id != value)
					this._Id = value;
			}

		}

		
		[Column(Name="Flag", Storage="_Flag", DbType="int")]
		public int? Flag
		{
			get
			{
				return this._Flag;
			}

			set
			{
				if (this._Flag != value)
					this._Flag = value;
			}

		}

		
		[Column(Name="PeopleId", Storage="_PeopleId", DbType="int NOT NULL")]
		public int PeopleId
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

		
		[Column(Name="TransactionId", Storage="_TransactionId", DbType="int NOT NULL")]
		public int TransactionId
		{
			get
			{
				return this._TransactionId;
			}

			set
			{
				if (this._TransactionId != value)
					this._TransactionId = value;
			}

		}

		
		[Column(Name="OrganizationName", Storage="_OrganizationName", DbType="varchar(60) NOT NULL")]
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

		
		[Column(Name="Name2", Storage="_Name2", DbType="varchar(37)")]
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

		
		[Column(Name="Status", Storage="_Status", DbType="bit")]
		public bool? Status
		{
			get
			{
				return this._Status;
			}

			set
			{
				if (this._Status != value)
					this._Status = value;
			}

		}

		
		[Column(Name="TransactionDate", Storage="_TransactionDate", DbType="datetime NOT NULL")]
		public DateTime TransactionDate
		{
			get
			{
				return this._TransactionDate;
			}

			set
			{
				if (this._TransactionDate != value)
					this._TransactionDate = value;
			}

		}

		
		[Column(Name="TransactionTypeId", Storage="_TransactionTypeId", DbType="int NOT NULL")]
		public int TransactionTypeId
		{
			get
			{
				return this._TransactionTypeId;
			}

			set
			{
				if (this._TransactionTypeId != value)
					this._TransactionTypeId = value;
			}

		}

		
		[Column(Name="TransactionStatus", Storage="_TransactionStatus", DbType="bit NOT NULL")]
		public bool TransactionStatus
		{
			get
			{
				return this._TransactionStatus;
			}

			set
			{
				if (this._TransactionStatus != value)
					this._TransactionStatus = value;
			}

		}

		
    }

}
