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
		
		private bool _RecordStatus;
		
		private int _MemberTypeId;
		
		private int _RollSheetSectionId;
		
		private bool _FeePaid;
		
		private bool _AllergyInfo;
		
		private int? _RobeNumber;
		
		private int? _GroupNumber;
		
		private int? _SectionId;
		
		private DateTime? _EnrollmentDate;
		
		private string _ClothingSizeInfo;
		
		private string _EmergencyContactInfo;
		
		private string _WhoBringsChildInfo;
		
		private string _AllergyText;
		
		private int? _ModifiedBy;
		
		private DateTime? _ModifiedDate;
		
		private int? _PositionId;
		
		private bool? _VipWeek1;
		
		private bool? _VipWeek2;
		
		private bool? _VipWeek3;
		
		private bool? _VipWeek4;
		
		private bool? _VipWeek5;
		
		private DateTime? _InactiveDate;
		
		private string _AttendStr;
		
		private decimal? _AttendPct;
		
		private DateTime? _LastAttended;
		
   		
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
		
		partial void OnRecordStatusChanging(bool value);
		partial void OnRecordStatusChanged();
		
		partial void OnMemberTypeIdChanging(int value);
		partial void OnMemberTypeIdChanged();
		
		partial void OnRollSheetSectionIdChanging(int value);
		partial void OnRollSheetSectionIdChanged();
		
		partial void OnFeePaidChanging(bool value);
		partial void OnFeePaidChanged();
		
		partial void OnAllergyInfoChanging(bool value);
		partial void OnAllergyInfoChanged();
		
		partial void OnRobeNumberChanging(int? value);
		partial void OnRobeNumberChanged();
		
		partial void OnGroupNumberChanging(int? value);
		partial void OnGroupNumberChanged();
		
		partial void OnSectionIdChanging(int? value);
		partial void OnSectionIdChanged();
		
		partial void OnEnrollmentDateChanging(DateTime? value);
		partial void OnEnrollmentDateChanged();
		
		partial void OnClothingSizeInfoChanging(string value);
		partial void OnClothingSizeInfoChanged();
		
		partial void OnEmergencyContactInfoChanging(string value);
		partial void OnEmergencyContactInfoChanged();
		
		partial void OnWhoBringsChildInfoChanging(string value);
		partial void OnWhoBringsChildInfoChanged();
		
		partial void OnAllergyTextChanging(string value);
		partial void OnAllergyTextChanged();
		
		partial void OnModifiedByChanging(int? value);
		partial void OnModifiedByChanged();
		
		partial void OnModifiedDateChanging(DateTime? value);
		partial void OnModifiedDateChanged();
		
		partial void OnPositionIdChanging(int? value);
		partial void OnPositionIdChanged();
		
		partial void OnVipWeek1Changing(bool? value);
		partial void OnVipWeek1Changed();
		
		partial void OnVipWeek2Changing(bool? value);
		partial void OnVipWeek2Changed();
		
		partial void OnVipWeek3Changing(bool? value);
		partial void OnVipWeek3Changed();
		
		partial void OnVipWeek4Changing(bool? value);
		partial void OnVipWeek4Changed();
		
		partial void OnVipWeek5Changing(bool? value);
		partial void OnVipWeek5Changed();
		
		partial void OnInactiveDateChanging(DateTime? value);
		partial void OnInactiveDateChanged();
		
		partial void OnAttendStrChanging(string value);
		partial void OnAttendStrChanged();
		
		partial void OnAttendPctChanging(decimal? value);
		partial void OnAttendPctChanged();
		
		partial void OnLastAttendedChanging(DateTime? value);
		partial void OnLastAttendedChanged();
		
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

		
		[Column(Name="RecordStatus", UpdateCheck=UpdateCheck.Never, Storage="_RecordStatus", DbType="bit NOT NULL")]
		public bool RecordStatus
		{
			get { return this._RecordStatus; }

			set
			{
				if (this._RecordStatus != value)
				{
				
                    this.OnRecordStatusChanging(value);
					this.SendPropertyChanging();
					this._RecordStatus = value;
					this.SendPropertyChanged("RecordStatus");
					this.OnRecordStatusChanged();
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

		
		[Column(Name="RollSheetSectionId", UpdateCheck=UpdateCheck.Never, Storage="_RollSheetSectionId", DbType="int NOT NULL")]
		public int RollSheetSectionId
		{
			get { return this._RollSheetSectionId; }

			set
			{
				if (this._RollSheetSectionId != value)
				{
				
                    this.OnRollSheetSectionIdChanging(value);
					this.SendPropertyChanging();
					this._RollSheetSectionId = value;
					this.SendPropertyChanged("RollSheetSectionId");
					this.OnRollSheetSectionIdChanged();
				}

			}

		}

		
		[Column(Name="FeePaid", UpdateCheck=UpdateCheck.Never, Storage="_FeePaid", DbType="bit NOT NULL")]
		public bool FeePaid
		{
			get { return this._FeePaid; }

			set
			{
				if (this._FeePaid != value)
				{
				
                    this.OnFeePaidChanging(value);
					this.SendPropertyChanging();
					this._FeePaid = value;
					this.SendPropertyChanged("FeePaid");
					this.OnFeePaidChanged();
				}

			}

		}

		
		[Column(Name="AllergyInfo", UpdateCheck=UpdateCheck.Never, Storage="_AllergyInfo", DbType="bit NOT NULL")]
		public bool AllergyInfo
		{
			get { return this._AllergyInfo; }

			set
			{
				if (this._AllergyInfo != value)
				{
				
                    this.OnAllergyInfoChanging(value);
					this.SendPropertyChanging();
					this._AllergyInfo = value;
					this.SendPropertyChanged("AllergyInfo");
					this.OnAllergyInfoChanged();
				}

			}

		}

		
		[Column(Name="RobeNumber", UpdateCheck=UpdateCheck.Never, Storage="_RobeNumber", DbType="int")]
		public int? RobeNumber
		{
			get { return this._RobeNumber; }

			set
			{
				if (this._RobeNumber != value)
				{
				
                    this.OnRobeNumberChanging(value);
					this.SendPropertyChanging();
					this._RobeNumber = value;
					this.SendPropertyChanged("RobeNumber");
					this.OnRobeNumberChanged();
				}

			}

		}

		
		[Column(Name="GroupNumber", UpdateCheck=UpdateCheck.Never, Storage="_GroupNumber", DbType="int")]
		public int? GroupNumber
		{
			get { return this._GroupNumber; }

			set
			{
				if (this._GroupNumber != value)
				{
				
                    this.OnGroupNumberChanging(value);
					this.SendPropertyChanging();
					this._GroupNumber = value;
					this.SendPropertyChanged("GroupNumber");
					this.OnGroupNumberChanged();
				}

			}

		}

		
		[Column(Name="SectionId", UpdateCheck=UpdateCheck.Never, Storage="_SectionId", DbType="int")]
		public int? SectionId
		{
			get { return this._SectionId; }

			set
			{
				if (this._SectionId != value)
				{
				
                    this.OnSectionIdChanging(value);
					this.SendPropertyChanging();
					this._SectionId = value;
					this.SendPropertyChanged("SectionId");
					this.OnSectionIdChanged();
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

		
		[Column(Name="ClothingSizeInfo", UpdateCheck=UpdateCheck.Never, Storage="_ClothingSizeInfo", DbType="varchar(256)")]
		public string ClothingSizeInfo
		{
			get { return this._ClothingSizeInfo; }

			set
			{
				if (this._ClothingSizeInfo != value)
				{
				
                    this.OnClothingSizeInfoChanging(value);
					this.SendPropertyChanging();
					this._ClothingSizeInfo = value;
					this.SendPropertyChanged("ClothingSizeInfo");
					this.OnClothingSizeInfoChanged();
				}

			}

		}

		
		[Column(Name="EmergencyContactInfo", UpdateCheck=UpdateCheck.Never, Storage="_EmergencyContactInfo", DbType="varchar(256)")]
		public string EmergencyContactInfo
		{
			get { return this._EmergencyContactInfo; }

			set
			{
				if (this._EmergencyContactInfo != value)
				{
				
                    this.OnEmergencyContactInfoChanging(value);
					this.SendPropertyChanging();
					this._EmergencyContactInfo = value;
					this.SendPropertyChanged("EmergencyContactInfo");
					this.OnEmergencyContactInfoChanged();
				}

			}

		}

		
		[Column(Name="WhoBringsChildInfo", UpdateCheck=UpdateCheck.Never, Storage="_WhoBringsChildInfo", DbType="varchar(256)")]
		public string WhoBringsChildInfo
		{
			get { return this._WhoBringsChildInfo; }

			set
			{
				if (this._WhoBringsChildInfo != value)
				{
				
                    this.OnWhoBringsChildInfoChanging(value);
					this.SendPropertyChanging();
					this._WhoBringsChildInfo = value;
					this.SendPropertyChanged("WhoBringsChildInfo");
					this.OnWhoBringsChildInfoChanged();
				}

			}

		}

		
		[Column(Name="AllergyText", UpdateCheck=UpdateCheck.Never, Storage="_AllergyText", DbType="varchar(256)")]
		public string AllergyText
		{
			get { return this._AllergyText; }

			set
			{
				if (this._AllergyText != value)
				{
				
                    this.OnAllergyTextChanging(value);
					this.SendPropertyChanging();
					this._AllergyText = value;
					this.SendPropertyChanged("AllergyText");
					this.OnAllergyTextChanged();
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

		
		[Column(Name="PositionId", UpdateCheck=UpdateCheck.Never, Storage="_PositionId", DbType="int")]
		public int? PositionId
		{
			get { return this._PositionId; }

			set
			{
				if (this._PositionId != value)
				{
				
                    this.OnPositionIdChanging(value);
					this.SendPropertyChanging();
					this._PositionId = value;
					this.SendPropertyChanged("PositionId");
					this.OnPositionIdChanged();
				}

			}

		}

		
		[Column(Name="VipWeek1", UpdateCheck=UpdateCheck.Never, Storage="_VipWeek1", DbType="bit")]
		public bool? VipWeek1
		{
			get { return this._VipWeek1; }

			set
			{
				if (this._VipWeek1 != value)
				{
				
                    this.OnVipWeek1Changing(value);
					this.SendPropertyChanging();
					this._VipWeek1 = value;
					this.SendPropertyChanged("VipWeek1");
					this.OnVipWeek1Changed();
				}

			}

		}

		
		[Column(Name="VipWeek2", UpdateCheck=UpdateCheck.Never, Storage="_VipWeek2", DbType="bit")]
		public bool? VipWeek2
		{
			get { return this._VipWeek2; }

			set
			{
				if (this._VipWeek2 != value)
				{
				
                    this.OnVipWeek2Changing(value);
					this.SendPropertyChanging();
					this._VipWeek2 = value;
					this.SendPropertyChanged("VipWeek2");
					this.OnVipWeek2Changed();
				}

			}

		}

		
		[Column(Name="VipWeek3", UpdateCheck=UpdateCheck.Never, Storage="_VipWeek3", DbType="bit")]
		public bool? VipWeek3
		{
			get { return this._VipWeek3; }

			set
			{
				if (this._VipWeek3 != value)
				{
				
                    this.OnVipWeek3Changing(value);
					this.SendPropertyChanging();
					this._VipWeek3 = value;
					this.SendPropertyChanged("VipWeek3");
					this.OnVipWeek3Changed();
				}

			}

		}

		
		[Column(Name="VipWeek4", UpdateCheck=UpdateCheck.Never, Storage="_VipWeek4", DbType="bit")]
		public bool? VipWeek4
		{
			get { return this._VipWeek4; }

			set
			{
				if (this._VipWeek4 != value)
				{
				
                    this.OnVipWeek4Changing(value);
					this.SendPropertyChanging();
					this._VipWeek4 = value;
					this.SendPropertyChanged("VipWeek4");
					this.OnVipWeek4Changed();
				}

			}

		}

		
		[Column(Name="VipWeek5", UpdateCheck=UpdateCheck.Never, Storage="_VipWeek5", DbType="bit")]
		public bool? VipWeek5
		{
			get { return this._VipWeek5; }

			set
			{
				if (this._VipWeek5 != value)
				{
				
                    this.OnVipWeek5Changing(value);
					this.SendPropertyChanging();
					this._VipWeek5 = value;
					this.SendPropertyChanged("VipWeek5");
					this.OnVipWeek5Changed();
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

