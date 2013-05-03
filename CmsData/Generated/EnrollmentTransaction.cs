using System; 
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel;

namespace CmsData
{
	[Table(Name="dbo.EnrollmentTransaction")]
	public partial class EnrollmentTransaction : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _TransactionId;
		
		private bool _TransactionStatus;
		
		private int? _CreatedBy;
		
		private DateTime? _CreatedDate;
		
		private DateTime _TransactionDate;
		
		private int _TransactionTypeId;
		
		private int _OrganizationId;
		
		private string _OrganizationName;
		
		private int _PeopleId;
		
		private int _MemberTypeId;
		
		private DateTime? _EnrollmentDate;
		
		private decimal? _AttendancePercentage;
		
		private DateTime? _NextTranChangeDate;
		
		private int? _EnrollmentTransactionId;
		
		private bool? _Pending;
		
   		
   		private EntitySet< EnrollmentTransaction> _DescTransactions;
		
    	
		private EntityRef< EnrollmentTransaction> _FirstTransaction;
		
		private EntityRef< Organization> _Organization;
		
		private EntityRef< Person> _Person;
		
		private EntityRef< MemberType> _MemberType;
		
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnTransactionIdChanging(int value);
		partial void OnTransactionIdChanged();
		
		partial void OnTransactionStatusChanging(bool value);
		partial void OnTransactionStatusChanged();
		
		partial void OnCreatedByChanging(int? value);
		partial void OnCreatedByChanged();
		
		partial void OnCreatedDateChanging(DateTime? value);
		partial void OnCreatedDateChanged();
		
		partial void OnTransactionDateChanging(DateTime value);
		partial void OnTransactionDateChanged();
		
		partial void OnTransactionTypeIdChanging(int value);
		partial void OnTransactionTypeIdChanged();
		
		partial void OnOrganizationIdChanging(int value);
		partial void OnOrganizationIdChanged();
		
		partial void OnOrganizationNameChanging(string value);
		partial void OnOrganizationNameChanged();
		
		partial void OnPeopleIdChanging(int value);
		partial void OnPeopleIdChanged();
		
		partial void OnMemberTypeIdChanging(int value);
		partial void OnMemberTypeIdChanged();
		
		partial void OnEnrollmentDateChanging(DateTime? value);
		partial void OnEnrollmentDateChanged();
		
		partial void OnAttendancePercentageChanging(decimal? value);
		partial void OnAttendancePercentageChanged();
		
		partial void OnNextTranChangeDateChanging(DateTime? value);
		partial void OnNextTranChangeDateChanged();
		
		partial void OnEnrollmentTransactionIdChanging(int? value);
		partial void OnEnrollmentTransactionIdChanged();
		
		partial void OnPendingChanging(bool? value);
		partial void OnPendingChanged();
		
    #endregion
		public EnrollmentTransaction()
		{
			
			this._DescTransactions = new EntitySet< EnrollmentTransaction>(new Action< EnrollmentTransaction>(this.attach_DescTransactions), new Action< EnrollmentTransaction>(this.detach_DescTransactions)); 
			
			
			this._FirstTransaction = default(EntityRef< EnrollmentTransaction>); 
			
			this._Organization = default(EntityRef< Organization>); 
			
			this._Person = default(EntityRef< Person>); 
			
			this._MemberType = default(EntityRef< MemberType>); 
			
			OnCreated();
		}

		
    #region Columns
		
		[Column(Name="TransactionId", UpdateCheck=UpdateCheck.Never, Storage="_TransactionId", AutoSync=AutoSync.OnInsert, DbType="int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int TransactionId
		{
			get { return this._TransactionId; }

			set
			{
				if (this._TransactionId != value)
				{
				
                    this.OnTransactionIdChanging(value);
					this.SendPropertyChanging();
					this._TransactionId = value;
					this.SendPropertyChanged("TransactionId");
					this.OnTransactionIdChanged();
				}

			}

		}

		
		[Column(Name="TransactionStatus", UpdateCheck=UpdateCheck.Never, Storage="_TransactionStatus", DbType="bit NOT NULL")]
		public bool TransactionStatus
		{
			get { return this._TransactionStatus; }

			set
			{
				if (this._TransactionStatus != value)
				{
				
                    this.OnTransactionStatusChanging(value);
					this.SendPropertyChanging();
					this._TransactionStatus = value;
					this.SendPropertyChanged("TransactionStatus");
					this.OnTransactionStatusChanged();
				}

			}

		}

		
		[Column(Name="CreatedBy", UpdateCheck=UpdateCheck.Never, Storage="_CreatedBy", DbType="int")]
		public int? CreatedBy
		{
			get { return this._CreatedBy; }

			set
			{
				if (this._CreatedBy != value)
				{
				
                    this.OnCreatedByChanging(value);
					this.SendPropertyChanging();
					this._CreatedBy = value;
					this.SendPropertyChanged("CreatedBy");
					this.OnCreatedByChanged();
				}

			}

		}

		
		[Column(Name="CreatedDate", UpdateCheck=UpdateCheck.Never, Storage="_CreatedDate", DbType="datetime")]
		public DateTime? CreatedDate
		{
			get { return this._CreatedDate; }

			set
			{
				if (this._CreatedDate != value)
				{
				
                    this.OnCreatedDateChanging(value);
					this.SendPropertyChanging();
					this._CreatedDate = value;
					this.SendPropertyChanged("CreatedDate");
					this.OnCreatedDateChanged();
				}

			}

		}

		
		[Column(Name="TransactionDate", UpdateCheck=UpdateCheck.Never, Storage="_TransactionDate", DbType="datetime NOT NULL")]
		public DateTime TransactionDate
		{
			get { return this._TransactionDate; }

			set
			{
				if (this._TransactionDate != value)
				{
				
                    this.OnTransactionDateChanging(value);
					this.SendPropertyChanging();
					this._TransactionDate = value;
					this.SendPropertyChanged("TransactionDate");
					this.OnTransactionDateChanged();
				}

			}

		}

		
		[Column(Name="TransactionTypeId", UpdateCheck=UpdateCheck.Never, Storage="_TransactionTypeId", DbType="int NOT NULL")]
		public int TransactionTypeId
		{
			get { return this._TransactionTypeId; }

			set
			{
				if (this._TransactionTypeId != value)
				{
				
                    this.OnTransactionTypeIdChanging(value);
					this.SendPropertyChanging();
					this._TransactionTypeId = value;
					this.SendPropertyChanged("TransactionTypeId");
					this.OnTransactionTypeIdChanged();
				}

			}

		}

		
		[Column(Name="OrganizationId", UpdateCheck=UpdateCheck.Never, Storage="_OrganizationId", DbType="int NOT NULL")]
		public int OrganizationId
		{
			get { return this._OrganizationId; }

			set
			{
				if (this._OrganizationId != value)
				{
				
					if (this._Organization.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnOrganizationIdChanging(value);
					this.SendPropertyChanging();
					this._OrganizationId = value;
					this.SendPropertyChanged("OrganizationId");
					this.OnOrganizationIdChanged();
				}

			}

		}

		
		[Column(Name="OrganizationName", UpdateCheck=UpdateCheck.Never, Storage="_OrganizationName", DbType="varchar(100) NOT NULL")]
		public string OrganizationName
		{
			get { return this._OrganizationName; }

			set
			{
				if (this._OrganizationName != value)
				{
				
                    this.OnOrganizationNameChanging(value);
					this.SendPropertyChanging();
					this._OrganizationName = value;
					this.SendPropertyChanged("OrganizationName");
					this.OnOrganizationNameChanged();
				}

			}

		}

		
		[Column(Name="PeopleId", UpdateCheck=UpdateCheck.Never, Storage="_PeopleId", DbType="int NOT NULL")]
		public int PeopleId
		{
			get { return this._PeopleId; }

			set
			{
				if (this._PeopleId != value)
				{
				
					if (this._Person.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnPeopleIdChanging(value);
					this.SendPropertyChanging();
					this._PeopleId = value;
					this.SendPropertyChanged("PeopleId");
					this.OnPeopleIdChanged();
				}

			}

		}

		
		[Column(Name="MemberTypeId", UpdateCheck=UpdateCheck.Never, Storage="_MemberTypeId", DbType="int NOT NULL")]
		public int MemberTypeId
		{
			get { return this._MemberTypeId; }

			set
			{
				if (this._MemberTypeId != value)
				{
				
					if (this._MemberType.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnMemberTypeIdChanging(value);
					this.SendPropertyChanging();
					this._MemberTypeId = value;
					this.SendPropertyChanged("MemberTypeId");
					this.OnMemberTypeIdChanged();
				}

			}

		}

		
		[Column(Name="EnrollmentDate", UpdateCheck=UpdateCheck.Never, Storage="_EnrollmentDate", DbType="datetime")]
		public DateTime? EnrollmentDate
		{
			get { return this._EnrollmentDate; }

			set
			{
				if (this._EnrollmentDate != value)
				{
				
                    this.OnEnrollmentDateChanging(value);
					this.SendPropertyChanging();
					this._EnrollmentDate = value;
					this.SendPropertyChanged("EnrollmentDate");
					this.OnEnrollmentDateChanged();
				}

			}

		}

		
		[Column(Name="AttendancePercentage", UpdateCheck=UpdateCheck.Never, Storage="_AttendancePercentage", DbType="real")]
		public decimal? AttendancePercentage
		{
			get { return this._AttendancePercentage; }

			set
			{
				if (this._AttendancePercentage != value)
				{
				
                    this.OnAttendancePercentageChanging(value);
					this.SendPropertyChanging();
					this._AttendancePercentage = value;
					this.SendPropertyChanged("AttendancePercentage");
					this.OnAttendancePercentageChanged();
				}

			}

		}

		
		[Column(Name="NextTranChangeDate", UpdateCheck=UpdateCheck.Never, Storage="_NextTranChangeDate", DbType="datetime")]
		public DateTime? NextTranChangeDate
		{
			get { return this._NextTranChangeDate; }

			set
			{
				if (this._NextTranChangeDate != value)
				{
				
                    this.OnNextTranChangeDateChanging(value);
					this.SendPropertyChanging();
					this._NextTranChangeDate = value;
					this.SendPropertyChanged("NextTranChangeDate");
					this.OnNextTranChangeDateChanged();
				}

			}

		}

		
		[Column(Name="EnrollmentTransactionId", UpdateCheck=UpdateCheck.Never, Storage="_EnrollmentTransactionId", DbType="int")]
		public int? EnrollmentTransactionId
		{
			get { return this._EnrollmentTransactionId; }

			set
			{
				if (this._EnrollmentTransactionId != value)
				{
				
					if (this._FirstTransaction.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnEnrollmentTransactionIdChanging(value);
					this.SendPropertyChanging();
					this._EnrollmentTransactionId = value;
					this.SendPropertyChanged("EnrollmentTransactionId");
					this.OnEnrollmentTransactionIdChanged();
				}

			}

		}

		
		[Column(Name="Pending", UpdateCheck=UpdateCheck.Never, Storage="_Pending", DbType="bit")]
		public bool? Pending
		{
			get { return this._Pending; }

			set
			{
				if (this._Pending != value)
				{
				
                    this.OnPendingChanging(value);
					this.SendPropertyChanging();
					this._Pending = value;
					this.SendPropertyChanged("Pending");
					this.OnPendingChanged();
				}

			}

		}

		
    #endregion
        
    #region Foreign Key Tables
   		
   		[Association(Name="DescTransactions__FirstTransaction", Storage="_DescTransactions", OtherKey="EnrollmentTransactionId")]
   		public EntitySet< EnrollmentTransaction> DescTransactions
   		{
   		    get { return this._DescTransactions; }

			set	{ this._DescTransactions.Assign(value); }

   		}

		
	#endregion
	
	#region Foreign Keys
    	
		[Association(Name="DescTransactions__FirstTransaction", Storage="_FirstTransaction", ThisKey="EnrollmentTransactionId", IsForeignKey=true)]
		public EnrollmentTransaction FirstTransaction
		{
			get { return this._FirstTransaction.Entity; }

			set
			{
				EnrollmentTransaction previousValue = this._FirstTransaction.Entity;
				if (((previousValue != value) 
							|| (this._FirstTransaction.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._FirstTransaction.Entity = null;
						previousValue.DescTransactions.Remove(this);
					}

					this._FirstTransaction.Entity = value;
					if (value != null)
					{
						value.DescTransactions.Add(this);
						
						this._EnrollmentTransactionId = value.TransactionId;
						
					}

					else
					{
						
						this._EnrollmentTransactionId = default(int?);
						
					}

					this.SendPropertyChanged("FirstTransaction");
				}

			}

		}

		
		[Association(Name="ENROLLMENT_TRANSACTION_ORG_FK", Storage="_Organization", ThisKey="OrganizationId", IsForeignKey=true)]
		public Organization Organization
		{
			get { return this._Organization.Entity; }

			set
			{
				Organization previousValue = this._Organization.Entity;
				if (((previousValue != value) 
							|| (this._Organization.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._Organization.Entity = null;
						previousValue.EnrollmentTransactions.Remove(this);
					}

					this._Organization.Entity = value;
					if (value != null)
					{
						value.EnrollmentTransactions.Add(this);
						
						this._OrganizationId = value.OrganizationId;
						
					}

					else
					{
						
						this._OrganizationId = default(int);
						
					}

					this.SendPropertyChanged("Organization");
				}

			}

		}

		
		[Association(Name="ENROLLMENT_TRANSACTION_PPL_FK", Storage="_Person", ThisKey="PeopleId", IsForeignKey=true)]
		public Person Person
		{
			get { return this._Person.Entity; }

			set
			{
				Person previousValue = this._Person.Entity;
				if (((previousValue != value) 
							|| (this._Person.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._Person.Entity = null;
						previousValue.EnrollmentTransactions.Remove(this);
					}

					this._Person.Entity = value;
					if (value != null)
					{
						value.EnrollmentTransactions.Add(this);
						
						this._PeopleId = value.PeopleId;
						
					}

					else
					{
						
						this._PeopleId = default(int);
						
					}

					this.SendPropertyChanged("Person");
				}

			}

		}

		
		[Association(Name="FK_ENROLLMENT_TRANSACTION_TBL_MemberType", Storage="_MemberType", ThisKey="MemberTypeId", IsForeignKey=true)]
		public MemberType MemberType
		{
			get { return this._MemberType.Entity; }

			set
			{
				MemberType previousValue = this._MemberType.Entity;
				if (((previousValue != value) 
							|| (this._MemberType.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._MemberType.Entity = null;
						previousValue.EnrollmentTransactions.Remove(this);
					}

					this._MemberType.Entity = value;
					if (value != null)
					{
						value.EnrollmentTransactions.Add(this);
						
						this._MemberTypeId = value.Id;
						
					}

					else
					{
						
						this._MemberTypeId = default(int);
						
					}

					this.SendPropertyChanged("MemberType");
				}

			}

		}

		
	#endregion
	
		public event PropertyChangingEventHandler PropertyChanging;
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
				this.PropertyChanging(this, emptyChangingEventArgs);
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

   		
		private void attach_DescTransactions(EnrollmentTransaction entity)
		{
			this.SendPropertyChanging();
			entity.FirstTransaction = this;
		}

		private void detach_DescTransactions(EnrollmentTransaction entity)
		{
			this.SendPropertyChanging();
			entity.FirstTransaction = null;
		}

		
	}

}

