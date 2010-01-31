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
	[Table(Name="dbo.OrganizationMembers")]
	public partial class OrganizationMember : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _OrganizationId;
		
		private int _PeopleId;
		
		private int? _CreatedBy;
		
		private DateTime? _CreatedDate;
		
		private int _MemberTypeId;
		
		private DateTime? _EnrollmentDate;
		
		private int? _ModifiedBy;
		
		private DateTime? _ModifiedDate;
		
		private DateTime? _InactiveDate;
		
		private string _AttendStr;
		
		private decimal? _AttendPct;
		
		private DateTime? _LastAttended;
		
		private bool? _Pending;
		
		private string _UserData;
		
   		
   		private EntitySet< OrgMemMemTag> _OrgMemMemTags;
		
    	
		private EntityRef< MemberType> _MemberType;
		
		private EntityRef< Organization> _Organization;
		
		private EntityRef< Person> _Person;
		
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnOrganizationIdChanging(int value);
		partial void OnOrganizationIdChanged();
		
		partial void OnPeopleIdChanging(int value);
		partial void OnPeopleIdChanged();
		
		partial void OnCreatedByChanging(int? value);
		partial void OnCreatedByChanged();
		
		partial void OnCreatedDateChanging(DateTime? value);
		partial void OnCreatedDateChanged();
		
		partial void OnMemberTypeIdChanging(int value);
		partial void OnMemberTypeIdChanged();
		
		partial void OnEnrollmentDateChanging(DateTime? value);
		partial void OnEnrollmentDateChanged();
		
		partial void OnModifiedByChanging(int? value);
		partial void OnModifiedByChanged();
		
		partial void OnModifiedDateChanging(DateTime? value);
		partial void OnModifiedDateChanged();
		
		partial void OnInactiveDateChanging(DateTime? value);
		partial void OnInactiveDateChanged();
		
		partial void OnAttendStrChanging(string value);
		partial void OnAttendStrChanged();
		
		partial void OnAttendPctChanging(decimal? value);
		partial void OnAttendPctChanged();
		
		partial void OnLastAttendedChanging(DateTime? value);
		partial void OnLastAttendedChanged();
		
		partial void OnPendingChanging(bool? value);
		partial void OnPendingChanged();
		
		partial void OnUserDataChanging(string value);
		partial void OnUserDataChanged();
		
    #endregion
		public OrganizationMember()
		{
			
			this._OrgMemMemTags = new EntitySet< OrgMemMemTag>(new Action< OrgMemMemTag>(this.attach_OrgMemMemTags), new Action< OrgMemMemTag>(this.detach_OrgMemMemTags)); 
			
			
			this._MemberType = default(EntityRef< MemberType>); 
			
			this._Organization = default(EntityRef< Organization>); 
			
			this._Person = default(EntityRef< Person>); 
			
			OnCreated();
		}

		
    #region Columns
		
		[Column(Name="OrganizationId", UpdateCheck=UpdateCheck.Never, Storage="_OrganizationId", DbType="int NOT NULL", IsPrimaryKey=true)]
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

		
		[Column(Name="PeopleId", UpdateCheck=UpdateCheck.Never, Storage="_PeopleId", DbType="int NOT NULL", IsPrimaryKey=true)]
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

		
		[Column(Name="ModifiedBy", UpdateCheck=UpdateCheck.Never, Storage="_ModifiedBy", DbType="int")]
		public int? ModifiedBy
		{
			get { return this._ModifiedBy; }

			set
			{
				if (this._ModifiedBy != value)
				{
				
                    this.OnModifiedByChanging(value);
					this.SendPropertyChanging();
					this._ModifiedBy = value;
					this.SendPropertyChanged("ModifiedBy");
					this.OnModifiedByChanged();
				}

			}

		}

		
		[Column(Name="ModifiedDate", UpdateCheck=UpdateCheck.Never, Storage="_ModifiedDate", DbType="datetime")]
		public DateTime? ModifiedDate
		{
			get { return this._ModifiedDate; }

			set
			{
				if (this._ModifiedDate != value)
				{
				
                    this.OnModifiedDateChanging(value);
					this.SendPropertyChanging();
					this._ModifiedDate = value;
					this.SendPropertyChanged("ModifiedDate");
					this.OnModifiedDateChanged();
				}

			}

		}

		
		[Column(Name="InactiveDate", UpdateCheck=UpdateCheck.Never, Storage="_InactiveDate", DbType="datetime")]
		public DateTime? InactiveDate
		{
			get { return this._InactiveDate; }

			set
			{
				if (this._InactiveDate != value)
				{
				
                    this.OnInactiveDateChanging(value);
					this.SendPropertyChanging();
					this._InactiveDate = value;
					this.SendPropertyChanged("InactiveDate");
					this.OnInactiveDateChanged();
				}

			}

		}

		
		[Column(Name="AttendStr", UpdateCheck=UpdateCheck.Never, Storage="_AttendStr", DbType="varchar(200)")]
		public string AttendStr
		{
			get { return this._AttendStr; }

			set
			{
				if (this._AttendStr != value)
				{
				
                    this.OnAttendStrChanging(value);
					this.SendPropertyChanging();
					this._AttendStr = value;
					this.SendPropertyChanged("AttendStr");
					this.OnAttendStrChanged();
				}

			}

		}

		
		[Column(Name="AttendPct", UpdateCheck=UpdateCheck.Never, Storage="_AttendPct", DbType="real")]
		public decimal? AttendPct
		{
			get { return this._AttendPct; }

			set
			{
				if (this._AttendPct != value)
				{
				
                    this.OnAttendPctChanging(value);
					this.SendPropertyChanging();
					this._AttendPct = value;
					this.SendPropertyChanged("AttendPct");
					this.OnAttendPctChanged();
				}

			}

		}

		
		[Column(Name="LastAttended", UpdateCheck=UpdateCheck.Never, Storage="_LastAttended", DbType="datetime")]
		public DateTime? LastAttended
		{
			get { return this._LastAttended; }

			set
			{
				if (this._LastAttended != value)
				{
				
                    this.OnLastAttendedChanging(value);
					this.SendPropertyChanging();
					this._LastAttended = value;
					this.SendPropertyChanged("LastAttended");
					this.OnLastAttendedChanged();
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

		
		[Column(Name="UserData", UpdateCheck=UpdateCheck.Never, Storage="_UserData", DbType="varchar(500)")]
		public string UserData
		{
			get { return this._UserData; }

			set
			{
				if (this._UserData != value)
				{
				
                    this.OnUserDataChanging(value);
					this.SendPropertyChanging();
					this._UserData = value;
					this.SendPropertyChanged("UserData");
					this.OnUserDataChanged();
				}

			}

		}

		
    #endregion
        
    #region Foreign Key Tables
   		
   		[Association(Name="FK_OrgMemMemTags_OrganizationMembers", Storage="_OrgMemMemTags", OtherKey="OrgId,PeopleId")]
   		public EntitySet< OrgMemMemTag> OrgMemMemTags
   		{
   		    get { return this._OrgMemMemTags; }

			set	{ this._OrgMemMemTags.Assign(value); }

   		}

		
	#endregion
	
	#region Foreign Keys
    	
		[Association(Name="FK_ORGANIZATION_MEMBERS_TBL_MemberType", Storage="_MemberType", ThisKey="MemberTypeId", IsForeignKey=true)]
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
						previousValue.OrganizationMembers.Remove(this);
					}

					this._MemberType.Entity = value;
					if (value != null)
					{
						value.OrganizationMembers.Add(this);
						
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

		
		[Association(Name="ORGANIZATION_MEMBERS_ORG_FK", Storage="_Organization", ThisKey="OrganizationId", IsForeignKey=true)]
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
						previousValue.OrganizationMembers.Remove(this);
					}

					this._Organization.Entity = value;
					if (value != null)
					{
						value.OrganizationMembers.Add(this);
						
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

		
		[Association(Name="ORGANIZATION_MEMBERS_PPL_FK", Storage="_Person", ThisKey="PeopleId", IsForeignKey=true)]
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
						previousValue.OrganizationMembers.Remove(this);
					}

					this._Person.Entity = value;
					if (value != null)
					{
						value.OrganizationMembers.Add(this);
						
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

   		
		private void attach_OrgMemMemTags(OrgMemMemTag entity)
		{
			this.SendPropertyChanging();
			entity.OrganizationMember = this;
		}

		private void detach_OrgMemMemTags(OrgMemMemTag entity)
		{
			this.SendPropertyChanging();
			entity.OrganizationMember = null;
		}

		
	}

}

