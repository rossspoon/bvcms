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
	[Table(Name="dbo.People")]
	public partial class Person : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _PeopleId;
		
		private int _CreatedBy;
		
		private DateTime? _CreatedDate;
		
		private int _DropCodeId;
		
		private int _GenderId;
		
		private bool _DoNotMailFlag;
		
		private bool _DoNotCallFlag;
		
		private bool _DoNotVisitFlag;
		
		private int _AddressTypeId;
		
		private int _PhonePrefId;
		
		private int _MaritalStatusId;
		
		private int _PositionInFamilyId;
		
		private int _MemberStatusId;
		
		private int _FamilyId;
		
		private int? _BirthMonth;
		
		private int? _BirthDay;
		
		private int? _BirthYear;
		
		private int? _OriginId;
		
		private int? _EntryPointId;
		
		private int? _InterestPointId;
		
		private int? _BaptismTypeId;
		
		private int? _BaptismStatusId;
		
		private int? _DecisionTypeId;
		
		private int? _DiscoveryClassStatusId;
		
		private int? _NewMbrClassStatusId;
		
		private int? _LetterStatusId;
		
		private int _JoinCodeId;
		
		private int? _EnvelopeOptionsId;
		
		private bool? _BadAddressFlag;
		
		private bool? _AltBadAddressFlag;
		
		private int? _ResCodeId;
		
		private int? _AltResCodeId;
		
		private DateTime? _AddressFromDate;
		
		private DateTime? _AddressToDate;
		
		private DateTime? _AltAddressFromDate;
		
		private DateTime? _AltAddressToDate;
		
		private DateTime? _WeddingDate;
		
		private DateTime? _OriginDate;
		
		private DateTime? _BaptismSchedDate;
		
		private DateTime? _BaptismDate;
		
		private DateTime? _DecisionDate;
		
		private DateTime? _DiscoveryClassDate;
		
		private DateTime? _NewMbrClassDateCompleted;
		
		private DateTime? _LetterDateRequested;
		
		private DateTime? _LetterDateReceived;
		
		private DateTime? _JoinDate;
		
		private DateTime? _DropDate;
		
		private DateTime? _DeceasedDate;
		
		private string _TitleCode;
		
		private string _FirstName;
		
		private string _MiddleName;
		
		private string _MaidenName;
		
		private string _LastName;
		
		private string _SuffixCode;
		
		private string _NickName;
		
		private string _AddressLineOne;
		
		private string _AddressLineTwo;
		
		private string _CityName;
		
		private string _StateCode;
		
		private string _ZipCode;
		
		private string _CountryName;
		
		private string _StreetName;
		
		private string _AltAddressLineOne;
		
		private string _AltAddressLineTwo;
		
		private string _AltCityName;
		
		private string _AltStateCode;
		
		private string _AltZipCode;
		
		private string _AltCountryName;
		
		private string _AltStreetName;
		
		private string _CellPhone;
		
		private string _WorkPhone;
		
		private string _EmailAddress;
		
		private string _OtherPreviousChurch;
		
		private string _OtherNewChurch;
		
		private string _SchoolOther;
		
		private string _EmployerOther;
		
		private string _OccupationOther;
		
		private string _HobbyOther;
		
		private string _SkillOther;
		
		private string _InterestOther;
		
		private string _LetterStatusNotes;
		
		private string _Comments;
		
		private bool _ChristAsSavior;
		
		private bool? _MemberAnyChurch;
		
		private bool _InterestedInJoining;
		
		private bool _PleaseVisit;
		
		private bool _InfoBecomeAChristian;
		
		private bool _ContributionsStatement;
		
		private int? _ModifiedBy;
		
		private DateTime? _ModifiedDate;
		
		private int? _PictureId;
		
		private int? _ContributionOptionsId;
		
		private string _PrimaryCity;
		
		private string _PrimaryZip;
		
		private string _PrimaryAddress;
		
		private string _PrimaryState;
		
		private string _HomePhone;
		
		private int? _SpouseId;
		
		private string _PrimaryAddress2;
		
		private int? _PrimaryResCode;
		
		private int? _PrimaryBadAddrFlag;
		
		private string _BibleFellowshipTeacher;
		
		private int? _BibleFellowshipTeacherId;
		
		private DateTime? _LastContact;
		
		private bool? _InBFClass;
		
		private int? _Grade;
		
		private string _CellPhoneLU;
		
		private string _WorkPhoneLU;
		
		private int? _BibleFellowshipClassId;
		
		private string _Name;
		
		private string _Name2;
		
		private int? _HashNum;
		
		private int? _CampusId;
		
		private string _CellPhoneAC;
		
		private string _PreferredName;
		
		private string _CheckInNotes;
		
		private int? _Age;
		
   		
   		private EntitySet< Contactee> _contactsHad;
		
   		private EntitySet< Contactor> _contactsMade;
		
   		private EntitySet< EnrollmentTransaction> _EnrollmentTransactions;
		
   		private EntitySet< Family> _FamiliesHeaded;
		
   		private EntitySet< Family> _FamiliesHeaded2;
		
   		private EntitySet< MOBSReg> _MOBSRegs;
		
   		private EntitySet< Attend> _Attends;
		
   		private EntitySet< BadET> _BadETs;
		
   		private EntitySet< CardIdentifier> _CardIdentifiers;
		
   		private EntitySet< CheckInTime> _CheckInTimes;
		
   		private EntitySet< Contribution> _Contributions;
		
   		private EntitySet< RecReg> _RecRegs;
		
   		private EntitySet< PendingNotification> _PendingNotifications;
		
   		private EntitySet< PeopleExtra> _PeopleExtras;
		
   		private EntitySet< PrayerSlot> _PrayerSlots;
		
   		private EntitySet< SaleTransaction> _SaleTransactions;
		
   		private EntitySet< TagShare> _TagShares;
		
   		private EntitySet< TaskListOwner> _TaskListOwners;
		
   		private EntitySet< User> _Users;
		
   		private EntitySet< VBSApp> _VBSApps;
		
   		private EntitySet< VolInterestInterestCode> _VolInterestInterestCodes;
		
   		private EntitySet< Volunteer> _Volunteers;
		
   		private EntitySet< VolunteerForm> _VolunteerForms;
		
   		private EntitySet< LoveRespect> _HerLoveRespects;
		
   		private EntitySet< SoulMate> _HerSoulMates;
		
   		private EntitySet< LoveRespect> _HisLoveRespects;
		
   		private EntitySet< SoulMate> _HisSoulMates;
		
   		private EntitySet< OrganizationMember> _OrganizationMembers;
		
   		private EntitySet< TagPerson> _Tags;
		
   		private EntitySet< Tag> _TagsOwned;
		
   		private EntitySet< Task> _Tasks;
		
   		private EntitySet< Task> _TasksAboutPerson;
		
   		private EntitySet< Task> _TasksCoOwned;
		
    	
		private EntityRef< BaptismType> _BaptismType;
		
		private EntityRef< Campu> _Campu;
		
		private EntityRef< DecisionType> _DecisionType;
		
		private EntityRef< DropType> _DropType;
		
		private EntityRef< Family> _Family;
		
		private EntityRef< FamilyPosition> _FamilyPosition;
		
		private EntityRef< Gender> _Gender;
		
		private EntityRef< JoinType> _JoinType;
		
		private EntityRef< MaritalStatus> _MaritalStatus;
		
		private EntityRef< MemberStatus> _MemberStatus;
		
		private EntityRef< Origin> _Origin;
		
		private EntityRef< InterestPoint> _InterestPoint;
		
		private EntityRef< Picture> _Picture;
		
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnPeopleIdChanging(int value);
		partial void OnPeopleIdChanged();
		
		partial void OnCreatedByChanging(int value);
		partial void OnCreatedByChanged();
		
		partial void OnCreatedDateChanging(DateTime? value);
		partial void OnCreatedDateChanged();
		
		partial void OnDropCodeIdChanging(int value);
		partial void OnDropCodeIdChanged();
		
		partial void OnGenderIdChanging(int value);
		partial void OnGenderIdChanged();
		
		partial void OnDoNotMailFlagChanging(bool value);
		partial void OnDoNotMailFlagChanged();
		
		partial void OnDoNotCallFlagChanging(bool value);
		partial void OnDoNotCallFlagChanged();
		
		partial void OnDoNotVisitFlagChanging(bool value);
		partial void OnDoNotVisitFlagChanged();
		
		partial void OnAddressTypeIdChanging(int value);
		partial void OnAddressTypeIdChanged();
		
		partial void OnPhonePrefIdChanging(int value);
		partial void OnPhonePrefIdChanged();
		
		partial void OnMaritalStatusIdChanging(int value);
		partial void OnMaritalStatusIdChanged();
		
		partial void OnPositionInFamilyIdChanging(int value);
		partial void OnPositionInFamilyIdChanged();
		
		partial void OnMemberStatusIdChanging(int value);
		partial void OnMemberStatusIdChanged();
		
		partial void OnFamilyIdChanging(int value);
		partial void OnFamilyIdChanged();
		
		partial void OnBirthMonthChanging(int? value);
		partial void OnBirthMonthChanged();
		
		partial void OnBirthDayChanging(int? value);
		partial void OnBirthDayChanged();
		
		partial void OnBirthYearChanging(int? value);
		partial void OnBirthYearChanged();
		
		partial void OnOriginIdChanging(int? value);
		partial void OnOriginIdChanged();
		
		partial void OnEntryPointIdChanging(int? value);
		partial void OnEntryPointIdChanged();
		
		partial void OnInterestPointIdChanging(int? value);
		partial void OnInterestPointIdChanged();
		
		partial void OnBaptismTypeIdChanging(int? value);
		partial void OnBaptismTypeIdChanged();
		
		partial void OnBaptismStatusIdChanging(int? value);
		partial void OnBaptismStatusIdChanged();
		
		partial void OnDecisionTypeIdChanging(int? value);
		partial void OnDecisionTypeIdChanged();
		
		partial void OnDiscoveryClassStatusIdChanging(int? value);
		partial void OnDiscoveryClassStatusIdChanged();
		
		partial void OnNewMbrClassStatusIdChanging(int? value);
		partial void OnNewMbrClassStatusIdChanged();
		
		partial void OnLetterStatusIdChanging(int? value);
		partial void OnLetterStatusIdChanged();
		
		partial void OnJoinCodeIdChanging(int value);
		partial void OnJoinCodeIdChanged();
		
		partial void OnEnvelopeOptionsIdChanging(int? value);
		partial void OnEnvelopeOptionsIdChanged();
		
		partial void OnBadAddressFlagChanging(bool? value);
		partial void OnBadAddressFlagChanged();
		
		partial void OnAltBadAddressFlagChanging(bool? value);
		partial void OnAltBadAddressFlagChanged();
		
		partial void OnResCodeIdChanging(int? value);
		partial void OnResCodeIdChanged();
		
		partial void OnAltResCodeIdChanging(int? value);
		partial void OnAltResCodeIdChanged();
		
		partial void OnAddressFromDateChanging(DateTime? value);
		partial void OnAddressFromDateChanged();
		
		partial void OnAddressToDateChanging(DateTime? value);
		partial void OnAddressToDateChanged();
		
		partial void OnAltAddressFromDateChanging(DateTime? value);
		partial void OnAltAddressFromDateChanged();
		
		partial void OnAltAddressToDateChanging(DateTime? value);
		partial void OnAltAddressToDateChanged();
		
		partial void OnWeddingDateChanging(DateTime? value);
		partial void OnWeddingDateChanged();
		
		partial void OnOriginDateChanging(DateTime? value);
		partial void OnOriginDateChanged();
		
		partial void OnBaptismSchedDateChanging(DateTime? value);
		partial void OnBaptismSchedDateChanged();
		
		partial void OnBaptismDateChanging(DateTime? value);
		partial void OnBaptismDateChanged();
		
		partial void OnDecisionDateChanging(DateTime? value);
		partial void OnDecisionDateChanged();
		
		partial void OnDiscoveryClassDateChanging(DateTime? value);
		partial void OnDiscoveryClassDateChanged();
		
		partial void OnNewMbrClassDateCompletedChanging(DateTime? value);
		partial void OnNewMbrClassDateCompletedChanged();
		
		partial void OnLetterDateRequestedChanging(DateTime? value);
		partial void OnLetterDateRequestedChanged();
		
		partial void OnLetterDateReceivedChanging(DateTime? value);
		partial void OnLetterDateReceivedChanged();
		
		partial void OnJoinDateChanging(DateTime? value);
		partial void OnJoinDateChanged();
		
		partial void OnDropDateChanging(DateTime? value);
		partial void OnDropDateChanged();
		
		partial void OnDeceasedDateChanging(DateTime? value);
		partial void OnDeceasedDateChanged();
		
		partial void OnTitleCodeChanging(string value);
		partial void OnTitleCodeChanged();
		
		partial void OnFirstNameChanging(string value);
		partial void OnFirstNameChanged();
		
		partial void OnMiddleNameChanging(string value);
		partial void OnMiddleNameChanged();
		
		partial void OnMaidenNameChanging(string value);
		partial void OnMaidenNameChanged();
		
		partial void OnLastNameChanging(string value);
		partial void OnLastNameChanged();
		
		partial void OnSuffixCodeChanging(string value);
		partial void OnSuffixCodeChanged();
		
		partial void OnNickNameChanging(string value);
		partial void OnNickNameChanged();
		
		partial void OnAddressLineOneChanging(string value);
		partial void OnAddressLineOneChanged();
		
		partial void OnAddressLineTwoChanging(string value);
		partial void OnAddressLineTwoChanged();
		
		partial void OnCityNameChanging(string value);
		partial void OnCityNameChanged();
		
		partial void OnStateCodeChanging(string value);
		partial void OnStateCodeChanged();
		
		partial void OnZipCodeChanging(string value);
		partial void OnZipCodeChanged();
		
		partial void OnCountryNameChanging(string value);
		partial void OnCountryNameChanged();
		
		partial void OnStreetNameChanging(string value);
		partial void OnStreetNameChanged();
		
		partial void OnAltAddressLineOneChanging(string value);
		partial void OnAltAddressLineOneChanged();
		
		partial void OnAltAddressLineTwoChanging(string value);
		partial void OnAltAddressLineTwoChanged();
		
		partial void OnAltCityNameChanging(string value);
		partial void OnAltCityNameChanged();
		
		partial void OnAltStateCodeChanging(string value);
		partial void OnAltStateCodeChanged();
		
		partial void OnAltZipCodeChanging(string value);
		partial void OnAltZipCodeChanged();
		
		partial void OnAltCountryNameChanging(string value);
		partial void OnAltCountryNameChanged();
		
		partial void OnAltStreetNameChanging(string value);
		partial void OnAltStreetNameChanged();
		
		partial void OnCellPhoneChanging(string value);
		partial void OnCellPhoneChanged();
		
		partial void OnWorkPhoneChanging(string value);
		partial void OnWorkPhoneChanged();
		
		partial void OnEmailAddressChanging(string value);
		partial void OnEmailAddressChanged();
		
		partial void OnOtherPreviousChurchChanging(string value);
		partial void OnOtherPreviousChurchChanged();
		
		partial void OnOtherNewChurchChanging(string value);
		partial void OnOtherNewChurchChanged();
		
		partial void OnSchoolOtherChanging(string value);
		partial void OnSchoolOtherChanged();
		
		partial void OnEmployerOtherChanging(string value);
		partial void OnEmployerOtherChanged();
		
		partial void OnOccupationOtherChanging(string value);
		partial void OnOccupationOtherChanged();
		
		partial void OnHobbyOtherChanging(string value);
		partial void OnHobbyOtherChanged();
		
		partial void OnSkillOtherChanging(string value);
		partial void OnSkillOtherChanged();
		
		partial void OnInterestOtherChanging(string value);
		partial void OnInterestOtherChanged();
		
		partial void OnLetterStatusNotesChanging(string value);
		partial void OnLetterStatusNotesChanged();
		
		partial void OnCommentsChanging(string value);
		partial void OnCommentsChanged();
		
		partial void OnChristAsSaviorChanging(bool value);
		partial void OnChristAsSaviorChanged();
		
		partial void OnMemberAnyChurchChanging(bool? value);
		partial void OnMemberAnyChurchChanged();
		
		partial void OnInterestedInJoiningChanging(bool value);
		partial void OnInterestedInJoiningChanged();
		
		partial void OnPleaseVisitChanging(bool value);
		partial void OnPleaseVisitChanged();
		
		partial void OnInfoBecomeAChristianChanging(bool value);
		partial void OnInfoBecomeAChristianChanged();
		
		partial void OnContributionsStatementChanging(bool value);
		partial void OnContributionsStatementChanged();
		
		partial void OnModifiedByChanging(int? value);
		partial void OnModifiedByChanged();
		
		partial void OnModifiedDateChanging(DateTime? value);
		partial void OnModifiedDateChanged();
		
		partial void OnPictureIdChanging(int? value);
		partial void OnPictureIdChanged();
		
		partial void OnContributionOptionsIdChanging(int? value);
		partial void OnContributionOptionsIdChanged();
		
		partial void OnPrimaryCityChanging(string value);
		partial void OnPrimaryCityChanged();
		
		partial void OnPrimaryZipChanging(string value);
		partial void OnPrimaryZipChanged();
		
		partial void OnPrimaryAddressChanging(string value);
		partial void OnPrimaryAddressChanged();
		
		partial void OnPrimaryStateChanging(string value);
		partial void OnPrimaryStateChanged();
		
		partial void OnHomePhoneChanging(string value);
		partial void OnHomePhoneChanged();
		
		partial void OnSpouseIdChanging(int? value);
		partial void OnSpouseIdChanged();
		
		partial void OnPrimaryAddress2Changing(string value);
		partial void OnPrimaryAddress2Changed();
		
		partial void OnPrimaryResCodeChanging(int? value);
		partial void OnPrimaryResCodeChanged();
		
		partial void OnPrimaryBadAddrFlagChanging(int? value);
		partial void OnPrimaryBadAddrFlagChanged();
		
		partial void OnBibleFellowshipTeacherChanging(string value);
		partial void OnBibleFellowshipTeacherChanged();
		
		partial void OnBibleFellowshipTeacherIdChanging(int? value);
		partial void OnBibleFellowshipTeacherIdChanged();
		
		partial void OnLastContactChanging(DateTime? value);
		partial void OnLastContactChanged();
		
		partial void OnInBFClassChanging(bool? value);
		partial void OnInBFClassChanged();
		
		partial void OnGradeChanging(int? value);
		partial void OnGradeChanged();
		
		partial void OnCellPhoneLUChanging(string value);
		partial void OnCellPhoneLUChanged();
		
		partial void OnWorkPhoneLUChanging(string value);
		partial void OnWorkPhoneLUChanged();
		
		partial void OnBibleFellowshipClassIdChanging(int? value);
		partial void OnBibleFellowshipClassIdChanged();
		
		partial void OnNameChanging(string value);
		partial void OnNameChanged();
		
		partial void OnName2Changing(string value);
		partial void OnName2Changed();
		
		partial void OnHashNumChanging(int? value);
		partial void OnHashNumChanged();
		
		partial void OnCampusIdChanging(int? value);
		partial void OnCampusIdChanged();
		
		partial void OnCellPhoneACChanging(string value);
		partial void OnCellPhoneACChanged();
		
		partial void OnPreferredNameChanging(string value);
		partial void OnPreferredNameChanged();
		
		partial void OnCheckInNotesChanging(string value);
		partial void OnCheckInNotesChanged();
		
		partial void OnAgeChanging(int? value);
		partial void OnAgeChanged();
		
    #endregion
		public Person()
		{
			
			this._contactsHad = new EntitySet< Contactee>(new Action< Contactee>(this.attach_contactsHad), new Action< Contactee>(this.detach_contactsHad)); 
			
			this._contactsMade = new EntitySet< Contactor>(new Action< Contactor>(this.attach_contactsMade), new Action< Contactor>(this.detach_contactsMade)); 
			
			this._EnrollmentTransactions = new EntitySet< EnrollmentTransaction>(new Action< EnrollmentTransaction>(this.attach_EnrollmentTransactions), new Action< EnrollmentTransaction>(this.detach_EnrollmentTransactions)); 
			
			this._FamiliesHeaded = new EntitySet< Family>(new Action< Family>(this.attach_FamiliesHeaded), new Action< Family>(this.detach_FamiliesHeaded)); 
			
			this._FamiliesHeaded2 = new EntitySet< Family>(new Action< Family>(this.attach_FamiliesHeaded2), new Action< Family>(this.detach_FamiliesHeaded2)); 
			
			this._MOBSRegs = new EntitySet< MOBSReg>(new Action< MOBSReg>(this.attach_MOBSRegs), new Action< MOBSReg>(this.detach_MOBSRegs)); 
			
			this._Attends = new EntitySet< Attend>(new Action< Attend>(this.attach_Attends), new Action< Attend>(this.detach_Attends)); 
			
			this._BadETs = new EntitySet< BadET>(new Action< BadET>(this.attach_BadETs), new Action< BadET>(this.detach_BadETs)); 
			
			this._CardIdentifiers = new EntitySet< CardIdentifier>(new Action< CardIdentifier>(this.attach_CardIdentifiers), new Action< CardIdentifier>(this.detach_CardIdentifiers)); 
			
			this._CheckInTimes = new EntitySet< CheckInTime>(new Action< CheckInTime>(this.attach_CheckInTimes), new Action< CheckInTime>(this.detach_CheckInTimes)); 
			
			this._Contributions = new EntitySet< Contribution>(new Action< Contribution>(this.attach_Contributions), new Action< Contribution>(this.detach_Contributions)); 
			
			this._RecRegs = new EntitySet< RecReg>(new Action< RecReg>(this.attach_RecRegs), new Action< RecReg>(this.detach_RecRegs)); 
			
			this._PendingNotifications = new EntitySet< PendingNotification>(new Action< PendingNotification>(this.attach_PendingNotifications), new Action< PendingNotification>(this.detach_PendingNotifications)); 
			
			this._PeopleExtras = new EntitySet< PeopleExtra>(new Action< PeopleExtra>(this.attach_PeopleExtras), new Action< PeopleExtra>(this.detach_PeopleExtras)); 
			
			this._PrayerSlots = new EntitySet< PrayerSlot>(new Action< PrayerSlot>(this.attach_PrayerSlots), new Action< PrayerSlot>(this.detach_PrayerSlots)); 
			
			this._SaleTransactions = new EntitySet< SaleTransaction>(new Action< SaleTransaction>(this.attach_SaleTransactions), new Action< SaleTransaction>(this.detach_SaleTransactions)); 
			
			this._TagShares = new EntitySet< TagShare>(new Action< TagShare>(this.attach_TagShares), new Action< TagShare>(this.detach_TagShares)); 
			
			this._TaskListOwners = new EntitySet< TaskListOwner>(new Action< TaskListOwner>(this.attach_TaskListOwners), new Action< TaskListOwner>(this.detach_TaskListOwners)); 
			
			this._Users = new EntitySet< User>(new Action< User>(this.attach_Users), new Action< User>(this.detach_Users)); 
			
			this._VBSApps = new EntitySet< VBSApp>(new Action< VBSApp>(this.attach_VBSApps), new Action< VBSApp>(this.detach_VBSApps)); 
			
			this._VolInterestInterestCodes = new EntitySet< VolInterestInterestCode>(new Action< VolInterestInterestCode>(this.attach_VolInterestInterestCodes), new Action< VolInterestInterestCode>(this.detach_VolInterestInterestCodes)); 
			
			this._Volunteers = new EntitySet< Volunteer>(new Action< Volunteer>(this.attach_Volunteers), new Action< Volunteer>(this.detach_Volunteers)); 
			
			this._VolunteerForms = new EntitySet< VolunteerForm>(new Action< VolunteerForm>(this.attach_VolunteerForms), new Action< VolunteerForm>(this.detach_VolunteerForms)); 
			
			this._HerLoveRespects = new EntitySet< LoveRespect>(new Action< LoveRespect>(this.attach_HerLoveRespects), new Action< LoveRespect>(this.detach_HerLoveRespects)); 
			
			this._HerSoulMates = new EntitySet< SoulMate>(new Action< SoulMate>(this.attach_HerSoulMates), new Action< SoulMate>(this.detach_HerSoulMates)); 
			
			this._HisLoveRespects = new EntitySet< LoveRespect>(new Action< LoveRespect>(this.attach_HisLoveRespects), new Action< LoveRespect>(this.detach_HisLoveRespects)); 
			
			this._HisSoulMates = new EntitySet< SoulMate>(new Action< SoulMate>(this.attach_HisSoulMates), new Action< SoulMate>(this.detach_HisSoulMates)); 
			
			this._OrganizationMembers = new EntitySet< OrganizationMember>(new Action< OrganizationMember>(this.attach_OrganizationMembers), new Action< OrganizationMember>(this.detach_OrganizationMembers)); 
			
			this._Tags = new EntitySet< TagPerson>(new Action< TagPerson>(this.attach_Tags), new Action< TagPerson>(this.detach_Tags)); 
			
			this._TagsOwned = new EntitySet< Tag>(new Action< Tag>(this.attach_TagsOwned), new Action< Tag>(this.detach_TagsOwned)); 
			
			this._Tasks = new EntitySet< Task>(new Action< Task>(this.attach_Tasks), new Action< Task>(this.detach_Tasks)); 
			
			this._TasksAboutPerson = new EntitySet< Task>(new Action< Task>(this.attach_TasksAboutPerson), new Action< Task>(this.detach_TasksAboutPerson)); 
			
			this._TasksCoOwned = new EntitySet< Task>(new Action< Task>(this.attach_TasksCoOwned), new Action< Task>(this.detach_TasksCoOwned)); 
			
			
			this._BaptismType = default(EntityRef< BaptismType>); 
			
			this._Campu = default(EntityRef< Campu>); 
			
			this._DecisionType = default(EntityRef< DecisionType>); 
			
			this._DropType = default(EntityRef< DropType>); 
			
			this._Family = default(EntityRef< Family>); 
			
			this._FamilyPosition = default(EntityRef< FamilyPosition>); 
			
			this._Gender = default(EntityRef< Gender>); 
			
			this._JoinType = default(EntityRef< JoinType>); 
			
			this._MaritalStatus = default(EntityRef< MaritalStatus>); 
			
			this._MemberStatus = default(EntityRef< MemberStatus>); 
			
			this._Origin = default(EntityRef< Origin>); 
			
			this._InterestPoint = default(EntityRef< InterestPoint>); 
			
			this._Picture = default(EntityRef< Picture>); 
			
			OnCreated();
		}

		
    #region Columns
		
		[Column(Name="PeopleId", UpdateCheck=UpdateCheck.Never, Storage="_PeopleId", AutoSync=AutoSync.OnInsert, DbType="int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int PeopleId
		{
			get { return this._PeopleId; }

			set
			{
				if (this._PeopleId != value)
				{
				
                    this.OnPeopleIdChanging(value);
					this.SendPropertyChanging();
					this._PeopleId = value;
					this.SendPropertyChanged("PeopleId");
					this.OnPeopleIdChanged();
				}

			}

		}

		
		[Column(Name="CreatedBy", UpdateCheck=UpdateCheck.Never, Storage="_CreatedBy", DbType="int NOT NULL")]
		public int CreatedBy
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

		
		[Column(Name="DropCodeId", UpdateCheck=UpdateCheck.Never, Storage="_DropCodeId", DbType="int NOT NULL")]
		public int DropCodeId
		{
			get { return this._DropCodeId; }

			set
			{
				if (this._DropCodeId != value)
				{
				
					if (this._DropType.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnDropCodeIdChanging(value);
					this.SendPropertyChanging();
					this._DropCodeId = value;
					this.SendPropertyChanged("DropCodeId");
					this.OnDropCodeIdChanged();
				}

			}

		}

		
		[Column(Name="GenderId", UpdateCheck=UpdateCheck.Never, Storage="_GenderId", DbType="int NOT NULL")]
		public int GenderId
		{
			get { return this._GenderId; }

			set
			{
				if (this._GenderId != value)
				{
				
					if (this._Gender.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnGenderIdChanging(value);
					this.SendPropertyChanging();
					this._GenderId = value;
					this.SendPropertyChanged("GenderId");
					this.OnGenderIdChanged();
				}

			}

		}

		
		[Column(Name="DoNotMailFlag", UpdateCheck=UpdateCheck.Never, Storage="_DoNotMailFlag", DbType="bit NOT NULL")]
		public bool DoNotMailFlag
		{
			get { return this._DoNotMailFlag; }

			set
			{
				if (this._DoNotMailFlag != value)
				{
				
                    this.OnDoNotMailFlagChanging(value);
					this.SendPropertyChanging();
					this._DoNotMailFlag = value;
					this.SendPropertyChanged("DoNotMailFlag");
					this.OnDoNotMailFlagChanged();
				}

			}

		}

		
		[Column(Name="DoNotCallFlag", UpdateCheck=UpdateCheck.Never, Storage="_DoNotCallFlag", DbType="bit NOT NULL")]
		public bool DoNotCallFlag
		{
			get { return this._DoNotCallFlag; }

			set
			{
				if (this._DoNotCallFlag != value)
				{
				
                    this.OnDoNotCallFlagChanging(value);
					this.SendPropertyChanging();
					this._DoNotCallFlag = value;
					this.SendPropertyChanged("DoNotCallFlag");
					this.OnDoNotCallFlagChanged();
				}

			}

		}

		
		[Column(Name="DoNotVisitFlag", UpdateCheck=UpdateCheck.Never, Storage="_DoNotVisitFlag", DbType="bit NOT NULL")]
		public bool DoNotVisitFlag
		{
			get { return this._DoNotVisitFlag; }

			set
			{
				if (this._DoNotVisitFlag != value)
				{
				
                    this.OnDoNotVisitFlagChanging(value);
					this.SendPropertyChanging();
					this._DoNotVisitFlag = value;
					this.SendPropertyChanged("DoNotVisitFlag");
					this.OnDoNotVisitFlagChanged();
				}

			}

		}

		
		[Column(Name="AddressTypeId", UpdateCheck=UpdateCheck.Never, Storage="_AddressTypeId", DbType="int NOT NULL")]
		public int AddressTypeId
		{
			get { return this._AddressTypeId; }

			set
			{
				if (this._AddressTypeId != value)
				{
				
                    this.OnAddressTypeIdChanging(value);
					this.SendPropertyChanging();
					this._AddressTypeId = value;
					this.SendPropertyChanged("AddressTypeId");
					this.OnAddressTypeIdChanged();
				}

			}

		}

		
		[Column(Name="PhonePrefId", UpdateCheck=UpdateCheck.Never, Storage="_PhonePrefId", DbType="int NOT NULL")]
		public int PhonePrefId
		{
			get { return this._PhonePrefId; }

			set
			{
				if (this._PhonePrefId != value)
				{
				
                    this.OnPhonePrefIdChanging(value);
					this.SendPropertyChanging();
					this._PhonePrefId = value;
					this.SendPropertyChanged("PhonePrefId");
					this.OnPhonePrefIdChanged();
				}

			}

		}

		
		[Column(Name="MaritalStatusId", UpdateCheck=UpdateCheck.Never, Storage="_MaritalStatusId", DbType="int NOT NULL")]
		public int MaritalStatusId
		{
			get { return this._MaritalStatusId; }

			set
			{
				if (this._MaritalStatusId != value)
				{
				
					if (this._MaritalStatus.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnMaritalStatusIdChanging(value);
					this.SendPropertyChanging();
					this._MaritalStatusId = value;
					this.SendPropertyChanged("MaritalStatusId");
					this.OnMaritalStatusIdChanged();
				}

			}

		}

		
		[Column(Name="PositionInFamilyId", UpdateCheck=UpdateCheck.Never, Storage="_PositionInFamilyId", DbType="int NOT NULL")]
		public int PositionInFamilyId
		{
			get { return this._PositionInFamilyId; }

			set
			{
				if (this._PositionInFamilyId != value)
				{
				
					if (this._FamilyPosition.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnPositionInFamilyIdChanging(value);
					this.SendPropertyChanging();
					this._PositionInFamilyId = value;
					this.SendPropertyChanged("PositionInFamilyId");
					this.OnPositionInFamilyIdChanged();
				}

			}

		}

		
		[Column(Name="MemberStatusId", UpdateCheck=UpdateCheck.Never, Storage="_MemberStatusId", DbType="int NOT NULL")]
		public int MemberStatusId
		{
			get { return this._MemberStatusId; }

			set
			{
				if (this._MemberStatusId != value)
				{
				
					if (this._MemberStatus.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnMemberStatusIdChanging(value);
					this.SendPropertyChanging();
					this._MemberStatusId = value;
					this.SendPropertyChanged("MemberStatusId");
					this.OnMemberStatusIdChanged();
				}

			}

		}

		
		[Column(Name="FamilyId", UpdateCheck=UpdateCheck.Never, Storage="_FamilyId", DbType="int NOT NULL")]
		public int FamilyId
		{
			get { return this._FamilyId; }

			set
			{
				if (this._FamilyId != value)
				{
				
					if (this._Family.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnFamilyIdChanging(value);
					this.SendPropertyChanging();
					this._FamilyId = value;
					this.SendPropertyChanged("FamilyId");
					this.OnFamilyIdChanged();
				}

			}

		}

		
		[Column(Name="BirthMonth", UpdateCheck=UpdateCheck.Never, Storage="_BirthMonth", DbType="int")]
		public int? BirthMonth
		{
			get { return this._BirthMonth; }

			set
			{
				if (this._BirthMonth != value)
				{
				
                    this.OnBirthMonthChanging(value);
					this.SendPropertyChanging();
					this._BirthMonth = value;
					this.SendPropertyChanged("BirthMonth");
					this.OnBirthMonthChanged();
				}

			}

		}

		
		[Column(Name="BirthDay", UpdateCheck=UpdateCheck.Never, Storage="_BirthDay", DbType="int")]
		public int? BirthDay
		{
			get { return this._BirthDay; }

			set
			{
				if (this._BirthDay != value)
				{
				
                    this.OnBirthDayChanging(value);
					this.SendPropertyChanging();
					this._BirthDay = value;
					this.SendPropertyChanged("BirthDay");
					this.OnBirthDayChanged();
				}

			}

		}

		
		[Column(Name="BirthYear", UpdateCheck=UpdateCheck.Never, Storage="_BirthYear", DbType="int")]
		public int? BirthYear
		{
			get { return this._BirthYear; }

			set
			{
				if (this._BirthYear != value)
				{
				
                    this.OnBirthYearChanging(value);
					this.SendPropertyChanging();
					this._BirthYear = value;
					this.SendPropertyChanged("BirthYear");
					this.OnBirthYearChanged();
				}

			}

		}

		
		[Column(Name="OriginId", UpdateCheck=UpdateCheck.Never, Storage="_OriginId", DbType="int")]
		public int? OriginId
		{
			get { return this._OriginId; }

			set
			{
				if (this._OriginId != value)
				{
				
					if (this._Origin.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnOriginIdChanging(value);
					this.SendPropertyChanging();
					this._OriginId = value;
					this.SendPropertyChanged("OriginId");
					this.OnOriginIdChanged();
				}

			}

		}

		
		[Column(Name="EntryPointId", UpdateCheck=UpdateCheck.Never, Storage="_EntryPointId", DbType="int")]
		public int? EntryPointId
		{
			get { return this._EntryPointId; }

			set
			{
				if (this._EntryPointId != value)
				{
				
                    this.OnEntryPointIdChanging(value);
					this.SendPropertyChanging();
					this._EntryPointId = value;
					this.SendPropertyChanged("EntryPointId");
					this.OnEntryPointIdChanged();
				}

			}

		}

		
		[Column(Name="InterestPointId", UpdateCheck=UpdateCheck.Never, Storage="_InterestPointId", DbType="int")]
		public int? InterestPointId
		{
			get { return this._InterestPointId; }

			set
			{
				if (this._InterestPointId != value)
				{
				
					if (this._InterestPoint.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnInterestPointIdChanging(value);
					this.SendPropertyChanging();
					this._InterestPointId = value;
					this.SendPropertyChanged("InterestPointId");
					this.OnInterestPointIdChanged();
				}

			}

		}

		
		[Column(Name="BaptismTypeId", UpdateCheck=UpdateCheck.Never, Storage="_BaptismTypeId", DbType="int")]
		public int? BaptismTypeId
		{
			get { return this._BaptismTypeId; }

			set
			{
				if (this._BaptismTypeId != value)
				{
				
					if (this._BaptismType.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnBaptismTypeIdChanging(value);
					this.SendPropertyChanging();
					this._BaptismTypeId = value;
					this.SendPropertyChanged("BaptismTypeId");
					this.OnBaptismTypeIdChanged();
				}

			}

		}

		
		[Column(Name="BaptismStatusId", UpdateCheck=UpdateCheck.Never, Storage="_BaptismStatusId", DbType="int")]
		public int? BaptismStatusId
		{
			get { return this._BaptismStatusId; }

			set
			{
				if (this._BaptismStatusId != value)
				{
				
                    this.OnBaptismStatusIdChanging(value);
					this.SendPropertyChanging();
					this._BaptismStatusId = value;
					this.SendPropertyChanged("BaptismStatusId");
					this.OnBaptismStatusIdChanged();
				}

			}

		}

		
		[Column(Name="DecisionTypeId", UpdateCheck=UpdateCheck.Never, Storage="_DecisionTypeId", DbType="int")]
		public int? DecisionTypeId
		{
			get { return this._DecisionTypeId; }

			set
			{
				if (this._DecisionTypeId != value)
				{
				
					if (this._DecisionType.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnDecisionTypeIdChanging(value);
					this.SendPropertyChanging();
					this._DecisionTypeId = value;
					this.SendPropertyChanged("DecisionTypeId");
					this.OnDecisionTypeIdChanged();
				}

			}

		}

		
		[Column(Name="DiscoveryClassStatusId", UpdateCheck=UpdateCheck.Never, Storage="_DiscoveryClassStatusId", DbType="int")]
		public int? DiscoveryClassStatusId
		{
			get { return this._DiscoveryClassStatusId; }

			set
			{
				if (this._DiscoveryClassStatusId != value)
				{
				
                    this.OnDiscoveryClassStatusIdChanging(value);
					this.SendPropertyChanging();
					this._DiscoveryClassStatusId = value;
					this.SendPropertyChanged("DiscoveryClassStatusId");
					this.OnDiscoveryClassStatusIdChanged();
				}

			}

		}

		
		[Column(Name="NewMbrClassStatusId", UpdateCheck=UpdateCheck.Never, Storage="_NewMbrClassStatusId", DbType="int")]
		public int? NewMbrClassStatusId
		{
			get { return this._NewMbrClassStatusId; }

			set
			{
				if (this._NewMbrClassStatusId != value)
				{
				
                    this.OnNewMbrClassStatusIdChanging(value);
					this.SendPropertyChanging();
					this._NewMbrClassStatusId = value;
					this.SendPropertyChanged("NewMbrClassStatusId");
					this.OnNewMbrClassStatusIdChanged();
				}

			}

		}

		
		[Column(Name="LetterStatusId", UpdateCheck=UpdateCheck.Never, Storage="_LetterStatusId", DbType="int")]
		public int? LetterStatusId
		{
			get { return this._LetterStatusId; }

			set
			{
				if (this._LetterStatusId != value)
				{
				
                    this.OnLetterStatusIdChanging(value);
					this.SendPropertyChanging();
					this._LetterStatusId = value;
					this.SendPropertyChanged("LetterStatusId");
					this.OnLetterStatusIdChanged();
				}

			}

		}

		
		[Column(Name="JoinCodeId", UpdateCheck=UpdateCheck.Never, Storage="_JoinCodeId", DbType="int NOT NULL")]
		public int JoinCodeId
		{
			get { return this._JoinCodeId; }

			set
			{
				if (this._JoinCodeId != value)
				{
				
					if (this._JoinType.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnJoinCodeIdChanging(value);
					this.SendPropertyChanging();
					this._JoinCodeId = value;
					this.SendPropertyChanged("JoinCodeId");
					this.OnJoinCodeIdChanged();
				}

			}

		}

		
		[Column(Name="EnvelopeOptionsId", UpdateCheck=UpdateCheck.Never, Storage="_EnvelopeOptionsId", DbType="int")]
		public int? EnvelopeOptionsId
		{
			get { return this._EnvelopeOptionsId; }

			set
			{
				if (this._EnvelopeOptionsId != value)
				{
				
                    this.OnEnvelopeOptionsIdChanging(value);
					this.SendPropertyChanging();
					this._EnvelopeOptionsId = value;
					this.SendPropertyChanged("EnvelopeOptionsId");
					this.OnEnvelopeOptionsIdChanged();
				}

			}

		}

		
		[Column(Name="BadAddressFlag", UpdateCheck=UpdateCheck.Never, Storage="_BadAddressFlag", DbType="bit")]
		public bool? BadAddressFlag
		{
			get { return this._BadAddressFlag; }

			set
			{
				if (this._BadAddressFlag != value)
				{
				
                    this.OnBadAddressFlagChanging(value);
					this.SendPropertyChanging();
					this._BadAddressFlag = value;
					this.SendPropertyChanged("BadAddressFlag");
					this.OnBadAddressFlagChanged();
				}

			}

		}

		
		[Column(Name="AltBadAddressFlag", UpdateCheck=UpdateCheck.Never, Storage="_AltBadAddressFlag", DbType="bit")]
		public bool? AltBadAddressFlag
		{
			get { return this._AltBadAddressFlag; }

			set
			{
				if (this._AltBadAddressFlag != value)
				{
				
                    this.OnAltBadAddressFlagChanging(value);
					this.SendPropertyChanging();
					this._AltBadAddressFlag = value;
					this.SendPropertyChanged("AltBadAddressFlag");
					this.OnAltBadAddressFlagChanged();
				}

			}

		}

		
		[Column(Name="ResCodeId", UpdateCheck=UpdateCheck.Never, Storage="_ResCodeId", DbType="int")]
		public int? ResCodeId
		{
			get { return this._ResCodeId; }

			set
			{
				if (this._ResCodeId != value)
				{
				
                    this.OnResCodeIdChanging(value);
					this.SendPropertyChanging();
					this._ResCodeId = value;
					this.SendPropertyChanged("ResCodeId");
					this.OnResCodeIdChanged();
				}

			}

		}

		
		[Column(Name="AltResCodeId", UpdateCheck=UpdateCheck.Never, Storage="_AltResCodeId", DbType="int")]
		public int? AltResCodeId
		{
			get { return this._AltResCodeId; }

			set
			{
				if (this._AltResCodeId != value)
				{
				
                    this.OnAltResCodeIdChanging(value);
					this.SendPropertyChanging();
					this._AltResCodeId = value;
					this.SendPropertyChanged("AltResCodeId");
					this.OnAltResCodeIdChanged();
				}

			}

		}

		
		[Column(Name="AddressFromDate", UpdateCheck=UpdateCheck.Never, Storage="_AddressFromDate", DbType="datetime")]
		public DateTime? AddressFromDate
		{
			get { return this._AddressFromDate; }

			set
			{
				if (this._AddressFromDate != value)
				{
				
                    this.OnAddressFromDateChanging(value);
					this.SendPropertyChanging();
					this._AddressFromDate = value;
					this.SendPropertyChanged("AddressFromDate");
					this.OnAddressFromDateChanged();
				}

			}

		}

		
		[Column(Name="AddressToDate", UpdateCheck=UpdateCheck.Never, Storage="_AddressToDate", DbType="datetime")]
		public DateTime? AddressToDate
		{
			get { return this._AddressToDate; }

			set
			{
				if (this._AddressToDate != value)
				{
				
                    this.OnAddressToDateChanging(value);
					this.SendPropertyChanging();
					this._AddressToDate = value;
					this.SendPropertyChanged("AddressToDate");
					this.OnAddressToDateChanged();
				}

			}

		}

		
		[Column(Name="AltAddressFromDate", UpdateCheck=UpdateCheck.Never, Storage="_AltAddressFromDate", DbType="datetime")]
		public DateTime? AltAddressFromDate
		{
			get { return this._AltAddressFromDate; }

			set
			{
				if (this._AltAddressFromDate != value)
				{
				
                    this.OnAltAddressFromDateChanging(value);
					this.SendPropertyChanging();
					this._AltAddressFromDate = value;
					this.SendPropertyChanged("AltAddressFromDate");
					this.OnAltAddressFromDateChanged();
				}

			}

		}

		
		[Column(Name="AltAddressToDate", UpdateCheck=UpdateCheck.Never, Storage="_AltAddressToDate", DbType="datetime")]
		public DateTime? AltAddressToDate
		{
			get { return this._AltAddressToDate; }

			set
			{
				if (this._AltAddressToDate != value)
				{
				
                    this.OnAltAddressToDateChanging(value);
					this.SendPropertyChanging();
					this._AltAddressToDate = value;
					this.SendPropertyChanged("AltAddressToDate");
					this.OnAltAddressToDateChanged();
				}

			}

		}

		
		[Column(Name="WeddingDate", UpdateCheck=UpdateCheck.Never, Storage="_WeddingDate", DbType="datetime")]
		public DateTime? WeddingDate
		{
			get { return this._WeddingDate; }

			set
			{
				if (this._WeddingDate != value)
				{
				
                    this.OnWeddingDateChanging(value);
					this.SendPropertyChanging();
					this._WeddingDate = value;
					this.SendPropertyChanged("WeddingDate");
					this.OnWeddingDateChanged();
				}

			}

		}

		
		[Column(Name="OriginDate", UpdateCheck=UpdateCheck.Never, Storage="_OriginDate", DbType="datetime")]
		public DateTime? OriginDate
		{
			get { return this._OriginDate; }

			set
			{
				if (this._OriginDate != value)
				{
				
                    this.OnOriginDateChanging(value);
					this.SendPropertyChanging();
					this._OriginDate = value;
					this.SendPropertyChanged("OriginDate");
					this.OnOriginDateChanged();
				}

			}

		}

		
		[Column(Name="BaptismSchedDate", UpdateCheck=UpdateCheck.Never, Storage="_BaptismSchedDate", DbType="datetime")]
		public DateTime? BaptismSchedDate
		{
			get { return this._BaptismSchedDate; }

			set
			{
				if (this._BaptismSchedDate != value)
				{
				
                    this.OnBaptismSchedDateChanging(value);
					this.SendPropertyChanging();
					this._BaptismSchedDate = value;
					this.SendPropertyChanged("BaptismSchedDate");
					this.OnBaptismSchedDateChanged();
				}

			}

		}

		
		[Column(Name="BaptismDate", UpdateCheck=UpdateCheck.Never, Storage="_BaptismDate", DbType="datetime")]
		public DateTime? BaptismDate
		{
			get { return this._BaptismDate; }

			set
			{
				if (this._BaptismDate != value)
				{
				
                    this.OnBaptismDateChanging(value);
					this.SendPropertyChanging();
					this._BaptismDate = value;
					this.SendPropertyChanged("BaptismDate");
					this.OnBaptismDateChanged();
				}

			}

		}

		
		[Column(Name="DecisionDate", UpdateCheck=UpdateCheck.Never, Storage="_DecisionDate", DbType="datetime")]
		public DateTime? DecisionDate
		{
			get { return this._DecisionDate; }

			set
			{
				if (this._DecisionDate != value)
				{
				
                    this.OnDecisionDateChanging(value);
					this.SendPropertyChanging();
					this._DecisionDate = value;
					this.SendPropertyChanged("DecisionDate");
					this.OnDecisionDateChanged();
				}

			}

		}

		
		[Column(Name="DiscoveryClassDate", UpdateCheck=UpdateCheck.Never, Storage="_DiscoveryClassDate", DbType="datetime")]
		public DateTime? DiscoveryClassDate
		{
			get { return this._DiscoveryClassDate; }

			set
			{
				if (this._DiscoveryClassDate != value)
				{
				
                    this.OnDiscoveryClassDateChanging(value);
					this.SendPropertyChanging();
					this._DiscoveryClassDate = value;
					this.SendPropertyChanged("DiscoveryClassDate");
					this.OnDiscoveryClassDateChanged();
				}

			}

		}

		
		[Column(Name="NewMbrClassDateCompleted", UpdateCheck=UpdateCheck.Never, Storage="_NewMbrClassDateCompleted", DbType="datetime")]
		public DateTime? NewMbrClassDateCompleted
		{
			get { return this._NewMbrClassDateCompleted; }

			set
			{
				if (this._NewMbrClassDateCompleted != value)
				{
				
                    this.OnNewMbrClassDateCompletedChanging(value);
					this.SendPropertyChanging();
					this._NewMbrClassDateCompleted = value;
					this.SendPropertyChanged("NewMbrClassDateCompleted");
					this.OnNewMbrClassDateCompletedChanged();
				}

			}

		}

		
		[Column(Name="LetterDateRequested", UpdateCheck=UpdateCheck.Never, Storage="_LetterDateRequested", DbType="datetime")]
		public DateTime? LetterDateRequested
		{
			get { return this._LetterDateRequested; }

			set
			{
				if (this._LetterDateRequested != value)
				{
				
                    this.OnLetterDateRequestedChanging(value);
					this.SendPropertyChanging();
					this._LetterDateRequested = value;
					this.SendPropertyChanged("LetterDateRequested");
					this.OnLetterDateRequestedChanged();
				}

			}

		}

		
		[Column(Name="LetterDateReceived", UpdateCheck=UpdateCheck.Never, Storage="_LetterDateReceived", DbType="datetime")]
		public DateTime? LetterDateReceived
		{
			get { return this._LetterDateReceived; }

			set
			{
				if (this._LetterDateReceived != value)
				{
				
                    this.OnLetterDateReceivedChanging(value);
					this.SendPropertyChanging();
					this._LetterDateReceived = value;
					this.SendPropertyChanged("LetterDateReceived");
					this.OnLetterDateReceivedChanged();
				}

			}

		}

		
		[Column(Name="JoinDate", UpdateCheck=UpdateCheck.Never, Storage="_JoinDate", DbType="datetime")]
		public DateTime? JoinDate
		{
			get { return this._JoinDate; }

			set
			{
				if (this._JoinDate != value)
				{
				
                    this.OnJoinDateChanging(value);
					this.SendPropertyChanging();
					this._JoinDate = value;
					this.SendPropertyChanged("JoinDate");
					this.OnJoinDateChanged();
				}

			}

		}

		
		[Column(Name="DropDate", UpdateCheck=UpdateCheck.Never, Storage="_DropDate", DbType="datetime")]
		public DateTime? DropDate
		{
			get { return this._DropDate; }

			set
			{
				if (this._DropDate != value)
				{
				
                    this.OnDropDateChanging(value);
					this.SendPropertyChanging();
					this._DropDate = value;
					this.SendPropertyChanged("DropDate");
					this.OnDropDateChanged();
				}

			}

		}

		
		[Column(Name="DeceasedDate", UpdateCheck=UpdateCheck.Never, Storage="_DeceasedDate", DbType="datetime")]
		public DateTime? DeceasedDate
		{
			get { return this._DeceasedDate; }

			set
			{
				if (this._DeceasedDate != value)
				{
				
                    this.OnDeceasedDateChanging(value);
					this.SendPropertyChanging();
					this._DeceasedDate = value;
					this.SendPropertyChanged("DeceasedDate");
					this.OnDeceasedDateChanged();
				}

			}

		}

		
		[Column(Name="TitleCode", UpdateCheck=UpdateCheck.Never, Storage="_TitleCode", DbType="varchar(10)")]
		public string TitleCode
		{
			get { return this._TitleCode; }

			set
			{
				if (this._TitleCode != value)
				{
				
                    this.OnTitleCodeChanging(value);
					this.SendPropertyChanging();
					this._TitleCode = value;
					this.SendPropertyChanged("TitleCode");
					this.OnTitleCodeChanged();
				}

			}

		}

		
		[Column(Name="FirstName", UpdateCheck=UpdateCheck.Never, Storage="_FirstName", DbType="varchar(25) NOT NULL")]
		public string FirstName
		{
			get { return this._FirstName; }

			set
			{
				if (this._FirstName != value)
				{
				
                    this.OnFirstNameChanging(value);
					this.SendPropertyChanging();
					this._FirstName = value;
					this.SendPropertyChanged("FirstName");
					this.OnFirstNameChanged();
				}

			}

		}

		
		[Column(Name="MiddleName", UpdateCheck=UpdateCheck.Never, Storage="_MiddleName", DbType="varchar(15)")]
		public string MiddleName
		{
			get { return this._MiddleName; }

			set
			{
				if (this._MiddleName != value)
				{
				
                    this.OnMiddleNameChanging(value);
					this.SendPropertyChanging();
					this._MiddleName = value;
					this.SendPropertyChanged("MiddleName");
					this.OnMiddleNameChanged();
				}

			}

		}

		
		[Column(Name="MaidenName", UpdateCheck=UpdateCheck.Never, Storage="_MaidenName", DbType="varchar(20)")]
		public string MaidenName
		{
			get { return this._MaidenName; }

			set
			{
				if (this._MaidenName != value)
				{
				
                    this.OnMaidenNameChanging(value);
					this.SendPropertyChanging();
					this._MaidenName = value;
					this.SendPropertyChanged("MaidenName");
					this.OnMaidenNameChanged();
				}

			}

		}

		
		[Column(Name="LastName", UpdateCheck=UpdateCheck.Never, Storage="_LastName", DbType="varchar(30) NOT NULL")]
		public string LastName
		{
			get { return this._LastName; }

			set
			{
				if (this._LastName != value)
				{
				
                    this.OnLastNameChanging(value);
					this.SendPropertyChanging();
					this._LastName = value;
					this.SendPropertyChanged("LastName");
					this.OnLastNameChanged();
				}

			}

		}

		
		[Column(Name="SuffixCode", UpdateCheck=UpdateCheck.Never, Storage="_SuffixCode", DbType="varchar(10)")]
		public string SuffixCode
		{
			get { return this._SuffixCode; }

			set
			{
				if (this._SuffixCode != value)
				{
				
                    this.OnSuffixCodeChanging(value);
					this.SendPropertyChanging();
					this._SuffixCode = value;
					this.SendPropertyChanged("SuffixCode");
					this.OnSuffixCodeChanged();
				}

			}

		}

		
		[Column(Name="NickName", UpdateCheck=UpdateCheck.Never, Storage="_NickName", DbType="varchar(15)")]
		public string NickName
		{
			get { return this._NickName; }

			set
			{
				if (this._NickName != value)
				{
				
                    this.OnNickNameChanging(value);
					this.SendPropertyChanging();
					this._NickName = value;
					this.SendPropertyChanged("NickName");
					this.OnNickNameChanged();
				}

			}

		}

		
		[Column(Name="AddressLineOne", UpdateCheck=UpdateCheck.Never, Storage="_AddressLineOne", DbType="varchar(40)")]
		public string AddressLineOne
		{
			get { return this._AddressLineOne; }

			set
			{
				if (this._AddressLineOne != value)
				{
				
                    this.OnAddressLineOneChanging(value);
					this.SendPropertyChanging();
					this._AddressLineOne = value;
					this.SendPropertyChanged("AddressLineOne");
					this.OnAddressLineOneChanged();
				}

			}

		}

		
		[Column(Name="AddressLineTwo", UpdateCheck=UpdateCheck.Never, Storage="_AddressLineTwo", DbType="varchar(40)")]
		public string AddressLineTwo
		{
			get { return this._AddressLineTwo; }

			set
			{
				if (this._AddressLineTwo != value)
				{
				
                    this.OnAddressLineTwoChanging(value);
					this.SendPropertyChanging();
					this._AddressLineTwo = value;
					this.SendPropertyChanged("AddressLineTwo");
					this.OnAddressLineTwoChanged();
				}

			}

		}

		
		[Column(Name="CityName", UpdateCheck=UpdateCheck.Never, Storage="_CityName", DbType="varchar(20)")]
		public string CityName
		{
			get { return this._CityName; }

			set
			{
				if (this._CityName != value)
				{
				
                    this.OnCityNameChanging(value);
					this.SendPropertyChanging();
					this._CityName = value;
					this.SendPropertyChanged("CityName");
					this.OnCityNameChanged();
				}

			}

		}

		
		[Column(Name="StateCode", UpdateCheck=UpdateCheck.Never, Storage="_StateCode", DbType="varchar(20)")]
		public string StateCode
		{
			get { return this._StateCode; }

			set
			{
				if (this._StateCode != value)
				{
				
                    this.OnStateCodeChanging(value);
					this.SendPropertyChanging();
					this._StateCode = value;
					this.SendPropertyChanged("StateCode");
					this.OnStateCodeChanged();
				}

			}

		}

		
		[Column(Name="ZipCode", UpdateCheck=UpdateCheck.Never, Storage="_ZipCode", DbType="varchar(15)")]
		public string ZipCode
		{
			get { return this._ZipCode; }

			set
			{
				if (this._ZipCode != value)
				{
				
                    this.OnZipCodeChanging(value);
					this.SendPropertyChanging();
					this._ZipCode = value;
					this.SendPropertyChanged("ZipCode");
					this.OnZipCodeChanged();
				}

			}

		}

		
		[Column(Name="CountryName", UpdateCheck=UpdateCheck.Never, Storage="_CountryName", DbType="varchar(30)")]
		public string CountryName
		{
			get { return this._CountryName; }

			set
			{
				if (this._CountryName != value)
				{
				
                    this.OnCountryNameChanging(value);
					this.SendPropertyChanging();
					this._CountryName = value;
					this.SendPropertyChanged("CountryName");
					this.OnCountryNameChanged();
				}

			}

		}

		
		[Column(Name="StreetName", UpdateCheck=UpdateCheck.Never, Storage="_StreetName", DbType="varchar(40)")]
		public string StreetName
		{
			get { return this._StreetName; }

			set
			{
				if (this._StreetName != value)
				{
				
                    this.OnStreetNameChanging(value);
					this.SendPropertyChanging();
					this._StreetName = value;
					this.SendPropertyChanged("StreetName");
					this.OnStreetNameChanged();
				}

			}

		}

		
		[Column(Name="AltAddressLineOne", UpdateCheck=UpdateCheck.Never, Storage="_AltAddressLineOne", DbType="varchar(40)")]
		public string AltAddressLineOne
		{
			get { return this._AltAddressLineOne; }

			set
			{
				if (this._AltAddressLineOne != value)
				{
				
                    this.OnAltAddressLineOneChanging(value);
					this.SendPropertyChanging();
					this._AltAddressLineOne = value;
					this.SendPropertyChanged("AltAddressLineOne");
					this.OnAltAddressLineOneChanged();
				}

			}

		}

		
		[Column(Name="AltAddressLineTwo", UpdateCheck=UpdateCheck.Never, Storage="_AltAddressLineTwo", DbType="varchar(40)")]
		public string AltAddressLineTwo
		{
			get { return this._AltAddressLineTwo; }

			set
			{
				if (this._AltAddressLineTwo != value)
				{
				
                    this.OnAltAddressLineTwoChanging(value);
					this.SendPropertyChanging();
					this._AltAddressLineTwo = value;
					this.SendPropertyChanged("AltAddressLineTwo");
					this.OnAltAddressLineTwoChanged();
				}

			}

		}

		
		[Column(Name="AltCityName", UpdateCheck=UpdateCheck.Never, Storage="_AltCityName", DbType="varchar(20)")]
		public string AltCityName
		{
			get { return this._AltCityName; }

			set
			{
				if (this._AltCityName != value)
				{
				
                    this.OnAltCityNameChanging(value);
					this.SendPropertyChanging();
					this._AltCityName = value;
					this.SendPropertyChanged("AltCityName");
					this.OnAltCityNameChanged();
				}

			}

		}

		
		[Column(Name="AltStateCode", UpdateCheck=UpdateCheck.Never, Storage="_AltStateCode", DbType="varchar(20)")]
		public string AltStateCode
		{
			get { return this._AltStateCode; }

			set
			{
				if (this._AltStateCode != value)
				{
				
                    this.OnAltStateCodeChanging(value);
					this.SendPropertyChanging();
					this._AltStateCode = value;
					this.SendPropertyChanged("AltStateCode");
					this.OnAltStateCodeChanged();
				}

			}

		}

		
		[Column(Name="AltZipCode", UpdateCheck=UpdateCheck.Never, Storage="_AltZipCode", DbType="varchar(15)")]
		public string AltZipCode
		{
			get { return this._AltZipCode; }

			set
			{
				if (this._AltZipCode != value)
				{
				
                    this.OnAltZipCodeChanging(value);
					this.SendPropertyChanging();
					this._AltZipCode = value;
					this.SendPropertyChanged("AltZipCode");
					this.OnAltZipCodeChanged();
				}

			}

		}

		
		[Column(Name="AltCountryName", UpdateCheck=UpdateCheck.Never, Storage="_AltCountryName", DbType="varchar(30)")]
		public string AltCountryName
		{
			get { return this._AltCountryName; }

			set
			{
				if (this._AltCountryName != value)
				{
				
                    this.OnAltCountryNameChanging(value);
					this.SendPropertyChanging();
					this._AltCountryName = value;
					this.SendPropertyChanged("AltCountryName");
					this.OnAltCountryNameChanged();
				}

			}

		}

		
		[Column(Name="AltStreetName", UpdateCheck=UpdateCheck.Never, Storage="_AltStreetName", DbType="varchar(40)")]
		public string AltStreetName
		{
			get { return this._AltStreetName; }

			set
			{
				if (this._AltStreetName != value)
				{
				
                    this.OnAltStreetNameChanging(value);
					this.SendPropertyChanging();
					this._AltStreetName = value;
					this.SendPropertyChanged("AltStreetName");
					this.OnAltStreetNameChanged();
				}

			}

		}

		
		[Column(Name="CellPhone", UpdateCheck=UpdateCheck.Never, Storage="_CellPhone", DbType="varchar(20)")]
		public string CellPhone
		{
			get { return this._CellPhone; }

			set
			{
				if (this._CellPhone != value)
				{
				
                    this.OnCellPhoneChanging(value);
					this.SendPropertyChanging();
					this._CellPhone = value;
					this.SendPropertyChanged("CellPhone");
					this.OnCellPhoneChanged();
				}

			}

		}

		
		[Column(Name="WorkPhone", UpdateCheck=UpdateCheck.Never, Storage="_WorkPhone", DbType="varchar(20)")]
		public string WorkPhone
		{
			get { return this._WorkPhone; }

			set
			{
				if (this._WorkPhone != value)
				{
				
                    this.OnWorkPhoneChanging(value);
					this.SendPropertyChanging();
					this._WorkPhone = value;
					this.SendPropertyChanged("WorkPhone");
					this.OnWorkPhoneChanged();
				}

			}

		}

		
		[Column(Name="EmailAddress", UpdateCheck=UpdateCheck.Never, Storage="_EmailAddress", DbType="varchar(50)")]
		public string EmailAddress
		{
			get { return this._EmailAddress; }

			set
			{
				if (this._EmailAddress != value)
				{
				
                    this.OnEmailAddressChanging(value);
					this.SendPropertyChanging();
					this._EmailAddress = value;
					this.SendPropertyChanged("EmailAddress");
					this.OnEmailAddressChanged();
				}

			}

		}

		
		[Column(Name="OtherPreviousChurch", UpdateCheck=UpdateCheck.Never, Storage="_OtherPreviousChurch", DbType="varchar(60)")]
		public string OtherPreviousChurch
		{
			get { return this._OtherPreviousChurch; }

			set
			{
				if (this._OtherPreviousChurch != value)
				{
				
                    this.OnOtherPreviousChurchChanging(value);
					this.SendPropertyChanging();
					this._OtherPreviousChurch = value;
					this.SendPropertyChanged("OtherPreviousChurch");
					this.OnOtherPreviousChurchChanged();
				}

			}

		}

		
		[Column(Name="OtherNewChurch", UpdateCheck=UpdateCheck.Never, Storage="_OtherNewChurch", DbType="varchar(60)")]
		public string OtherNewChurch
		{
			get { return this._OtherNewChurch; }

			set
			{
				if (this._OtherNewChurch != value)
				{
				
                    this.OnOtherNewChurchChanging(value);
					this.SendPropertyChanging();
					this._OtherNewChurch = value;
					this.SendPropertyChanged("OtherNewChurch");
					this.OnOtherNewChurchChanged();
				}

			}

		}

		
		[Column(Name="SchoolOther", UpdateCheck=UpdateCheck.Never, Storage="_SchoolOther", DbType="varchar(60)")]
		public string SchoolOther
		{
			get { return this._SchoolOther; }

			set
			{
				if (this._SchoolOther != value)
				{
				
                    this.OnSchoolOtherChanging(value);
					this.SendPropertyChanging();
					this._SchoolOther = value;
					this.SendPropertyChanged("SchoolOther");
					this.OnSchoolOtherChanged();
				}

			}

		}

		
		[Column(Name="EmployerOther", UpdateCheck=UpdateCheck.Never, Storage="_EmployerOther", DbType="varchar(60)")]
		public string EmployerOther
		{
			get { return this._EmployerOther; }

			set
			{
				if (this._EmployerOther != value)
				{
				
                    this.OnEmployerOtherChanging(value);
					this.SendPropertyChanging();
					this._EmployerOther = value;
					this.SendPropertyChanged("EmployerOther");
					this.OnEmployerOtherChanged();
				}

			}

		}

		
		[Column(Name="OccupationOther", UpdateCheck=UpdateCheck.Never, Storage="_OccupationOther", DbType="varchar(40)")]
		public string OccupationOther
		{
			get { return this._OccupationOther; }

			set
			{
				if (this._OccupationOther != value)
				{
				
                    this.OnOccupationOtherChanging(value);
					this.SendPropertyChanging();
					this._OccupationOther = value;
					this.SendPropertyChanged("OccupationOther");
					this.OnOccupationOtherChanged();
				}

			}

		}

		
		[Column(Name="HobbyOther", UpdateCheck=UpdateCheck.Never, Storage="_HobbyOther", DbType="varchar(40)")]
		public string HobbyOther
		{
			get { return this._HobbyOther; }

			set
			{
				if (this._HobbyOther != value)
				{
				
                    this.OnHobbyOtherChanging(value);
					this.SendPropertyChanging();
					this._HobbyOther = value;
					this.SendPropertyChanged("HobbyOther");
					this.OnHobbyOtherChanged();
				}

			}

		}

		
		[Column(Name="SkillOther", UpdateCheck=UpdateCheck.Never, Storage="_SkillOther", DbType="varchar(40)")]
		public string SkillOther
		{
			get { return this._SkillOther; }

			set
			{
				if (this._SkillOther != value)
				{
				
                    this.OnSkillOtherChanging(value);
					this.SendPropertyChanging();
					this._SkillOther = value;
					this.SendPropertyChanged("SkillOther");
					this.OnSkillOtherChanged();
				}

			}

		}

		
		[Column(Name="InterestOther", UpdateCheck=UpdateCheck.Never, Storage="_InterestOther", DbType="varchar(40)")]
		public string InterestOther
		{
			get { return this._InterestOther; }

			set
			{
				if (this._InterestOther != value)
				{
				
                    this.OnInterestOtherChanging(value);
					this.SendPropertyChanging();
					this._InterestOther = value;
					this.SendPropertyChanged("InterestOther");
					this.OnInterestOtherChanged();
				}

			}

		}

		
		[Column(Name="LetterStatusNotes", UpdateCheck=UpdateCheck.Never, Storage="_LetterStatusNotes", DbType="varchar(256)")]
		public string LetterStatusNotes
		{
			get { return this._LetterStatusNotes; }

			set
			{
				if (this._LetterStatusNotes != value)
				{
				
                    this.OnLetterStatusNotesChanging(value);
					this.SendPropertyChanging();
					this._LetterStatusNotes = value;
					this.SendPropertyChanged("LetterStatusNotes");
					this.OnLetterStatusNotesChanged();
				}

			}

		}

		
		[Column(Name="Comments", UpdateCheck=UpdateCheck.Never, Storage="_Comments", DbType="varchar(256)")]
		public string Comments
		{
			get { return this._Comments; }

			set
			{
				if (this._Comments != value)
				{
				
                    this.OnCommentsChanging(value);
					this.SendPropertyChanging();
					this._Comments = value;
					this.SendPropertyChanged("Comments");
					this.OnCommentsChanged();
				}

			}

		}

		
		[Column(Name="ChristAsSavior", UpdateCheck=UpdateCheck.Never, Storage="_ChristAsSavior", DbType="bit NOT NULL")]
		public bool ChristAsSavior
		{
			get { return this._ChristAsSavior; }

			set
			{
				if (this._ChristAsSavior != value)
				{
				
                    this.OnChristAsSaviorChanging(value);
					this.SendPropertyChanging();
					this._ChristAsSavior = value;
					this.SendPropertyChanged("ChristAsSavior");
					this.OnChristAsSaviorChanged();
				}

			}

		}

		
		[Column(Name="MemberAnyChurch", UpdateCheck=UpdateCheck.Never, Storage="_MemberAnyChurch", DbType="bit")]
		public bool? MemberAnyChurch
		{
			get { return this._MemberAnyChurch; }

			set
			{
				if (this._MemberAnyChurch != value)
				{
				
                    this.OnMemberAnyChurchChanging(value);
					this.SendPropertyChanging();
					this._MemberAnyChurch = value;
					this.SendPropertyChanged("MemberAnyChurch");
					this.OnMemberAnyChurchChanged();
				}

			}

		}

		
		[Column(Name="InterestedInJoining", UpdateCheck=UpdateCheck.Never, Storage="_InterestedInJoining", DbType="bit NOT NULL")]
		public bool InterestedInJoining
		{
			get { return this._InterestedInJoining; }

			set
			{
				if (this._InterestedInJoining != value)
				{
				
                    this.OnInterestedInJoiningChanging(value);
					this.SendPropertyChanging();
					this._InterestedInJoining = value;
					this.SendPropertyChanged("InterestedInJoining");
					this.OnInterestedInJoiningChanged();
				}

			}

		}

		
		[Column(Name="PleaseVisit", UpdateCheck=UpdateCheck.Never, Storage="_PleaseVisit", DbType="bit NOT NULL")]
		public bool PleaseVisit
		{
			get { return this._PleaseVisit; }

			set
			{
				if (this._PleaseVisit != value)
				{
				
                    this.OnPleaseVisitChanging(value);
					this.SendPropertyChanging();
					this._PleaseVisit = value;
					this.SendPropertyChanged("PleaseVisit");
					this.OnPleaseVisitChanged();
				}

			}

		}

		
		[Column(Name="InfoBecomeAChristian", UpdateCheck=UpdateCheck.Never, Storage="_InfoBecomeAChristian", DbType="bit NOT NULL")]
		public bool InfoBecomeAChristian
		{
			get { return this._InfoBecomeAChristian; }

			set
			{
				if (this._InfoBecomeAChristian != value)
				{
				
                    this.OnInfoBecomeAChristianChanging(value);
					this.SendPropertyChanging();
					this._InfoBecomeAChristian = value;
					this.SendPropertyChanged("InfoBecomeAChristian");
					this.OnInfoBecomeAChristianChanged();
				}

			}

		}

		
		[Column(Name="ContributionsStatement", UpdateCheck=UpdateCheck.Never, Storage="_ContributionsStatement", DbType="bit NOT NULL")]
		public bool ContributionsStatement
		{
			get { return this._ContributionsStatement; }

			set
			{
				if (this._ContributionsStatement != value)
				{
				
                    this.OnContributionsStatementChanging(value);
					this.SendPropertyChanging();
					this._ContributionsStatement = value;
					this.SendPropertyChanged("ContributionsStatement");
					this.OnContributionsStatementChanged();
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

		
		[Column(Name="PictureId", UpdateCheck=UpdateCheck.Never, Storage="_PictureId", DbType="int")]
		public int? PictureId
		{
			get { return this._PictureId; }

			set
			{
				if (this._PictureId != value)
				{
				
					if (this._Picture.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnPictureIdChanging(value);
					this.SendPropertyChanging();
					this._PictureId = value;
					this.SendPropertyChanged("PictureId");
					this.OnPictureIdChanged();
				}

			}

		}

		
		[Column(Name="ContributionOptionsId", UpdateCheck=UpdateCheck.Never, Storage="_ContributionOptionsId", DbType="int")]
		public int? ContributionOptionsId
		{
			get { return this._ContributionOptionsId; }

			set
			{
				if (this._ContributionOptionsId != value)
				{
				
                    this.OnContributionOptionsIdChanging(value);
					this.SendPropertyChanging();
					this._ContributionOptionsId = value;
					this.SendPropertyChanged("ContributionOptionsId");
					this.OnContributionOptionsIdChanged();
				}

			}

		}

		
		[Column(Name="PrimaryCity", UpdateCheck=UpdateCheck.Never, Storage="_PrimaryCity", DbType="varchar(50)", IsDbGenerated=true)]
		public string PrimaryCity
		{
			get { return this._PrimaryCity; }

			set
			{
				if (this._PrimaryCity != value)
				{
				
                    this.OnPrimaryCityChanging(value);
					this.SendPropertyChanging();
					this._PrimaryCity = value;
					this.SendPropertyChanged("PrimaryCity");
					this.OnPrimaryCityChanged();
				}

			}

		}

		
		[Column(Name="PrimaryZip", UpdateCheck=UpdateCheck.Never, Storage="_PrimaryZip", DbType="varchar(11)", IsDbGenerated=true)]
		public string PrimaryZip
		{
			get { return this._PrimaryZip; }

			set
			{
				if (this._PrimaryZip != value)
				{
				
                    this.OnPrimaryZipChanging(value);
					this.SendPropertyChanging();
					this._PrimaryZip = value;
					this.SendPropertyChanged("PrimaryZip");
					this.OnPrimaryZipChanged();
				}

			}

		}

		
		[Column(Name="PrimaryAddress", UpdateCheck=UpdateCheck.Never, Storage="_PrimaryAddress", DbType="varchar(60)", IsDbGenerated=true)]
		public string PrimaryAddress
		{
			get { return this._PrimaryAddress; }

			set
			{
				if (this._PrimaryAddress != value)
				{
				
                    this.OnPrimaryAddressChanging(value);
					this.SendPropertyChanging();
					this._PrimaryAddress = value;
					this.SendPropertyChanged("PrimaryAddress");
					this.OnPrimaryAddressChanged();
				}

			}

		}

		
		[Column(Name="PrimaryState", UpdateCheck=UpdateCheck.Never, Storage="_PrimaryState", DbType="varchar(5)", IsDbGenerated=true)]
		public string PrimaryState
		{
			get { return this._PrimaryState; }

			set
			{
				if (this._PrimaryState != value)
				{
				
                    this.OnPrimaryStateChanging(value);
					this.SendPropertyChanging();
					this._PrimaryState = value;
					this.SendPropertyChanged("PrimaryState");
					this.OnPrimaryStateChanged();
				}

			}

		}

		
		[Column(Name="HomePhone", UpdateCheck=UpdateCheck.Never, Storage="_HomePhone", DbType="varchar(20)")]
		public string HomePhone
		{
			get { return this._HomePhone; }

			set
			{
				if (this._HomePhone != value)
				{
				
                    this.OnHomePhoneChanging(value);
					this.SendPropertyChanging();
					this._HomePhone = value;
					this.SendPropertyChanged("HomePhone");
					this.OnHomePhoneChanged();
				}

			}

		}

		
		[Column(Name="SpouseId", UpdateCheck=UpdateCheck.Never, Storage="_SpouseId", DbType="int", IsDbGenerated=true)]
		public int? SpouseId
		{
			get { return this._SpouseId; }

			set
			{
				if (this._SpouseId != value)
				{
				
                    this.OnSpouseIdChanging(value);
					this.SendPropertyChanging();
					this._SpouseId = value;
					this.SendPropertyChanged("SpouseId");
					this.OnSpouseIdChanged();
				}

			}

		}

		
		[Column(Name="PrimaryAddress2", UpdateCheck=UpdateCheck.Never, Storage="_PrimaryAddress2", DbType="varchar(60)", IsDbGenerated=true)]
		public string PrimaryAddress2
		{
			get { return this._PrimaryAddress2; }

			set
			{
				if (this._PrimaryAddress2 != value)
				{
				
                    this.OnPrimaryAddress2Changing(value);
					this.SendPropertyChanging();
					this._PrimaryAddress2 = value;
					this.SendPropertyChanged("PrimaryAddress2");
					this.OnPrimaryAddress2Changed();
				}

			}

		}

		
		[Column(Name="PrimaryResCode", UpdateCheck=UpdateCheck.Never, Storage="_PrimaryResCode", DbType="int", IsDbGenerated=true)]
		public int? PrimaryResCode
		{
			get { return this._PrimaryResCode; }

			set
			{
				if (this._PrimaryResCode != value)
				{
				
                    this.OnPrimaryResCodeChanging(value);
					this.SendPropertyChanging();
					this._PrimaryResCode = value;
					this.SendPropertyChanged("PrimaryResCode");
					this.OnPrimaryResCodeChanged();
				}

			}

		}

		
		[Column(Name="PrimaryBadAddrFlag", UpdateCheck=UpdateCheck.Never, Storage="_PrimaryBadAddrFlag", DbType="int", IsDbGenerated=true)]
		public int? PrimaryBadAddrFlag
		{
			get { return this._PrimaryBadAddrFlag; }

			set
			{
				if (this._PrimaryBadAddrFlag != value)
				{
				
                    this.OnPrimaryBadAddrFlagChanging(value);
					this.SendPropertyChanging();
					this._PrimaryBadAddrFlag = value;
					this.SendPropertyChanged("PrimaryBadAddrFlag");
					this.OnPrimaryBadAddrFlagChanged();
				}

			}

		}

		
		[Column(Name="BibleFellowshipTeacher", UpdateCheck=UpdateCheck.Never, Storage="_BibleFellowshipTeacher", DbType="varchar(100)", IsDbGenerated=true)]
		public string BibleFellowshipTeacher
		{
			get { return this._BibleFellowshipTeacher; }

			set
			{
				if (this._BibleFellowshipTeacher != value)
				{
				
                    this.OnBibleFellowshipTeacherChanging(value);
					this.SendPropertyChanging();
					this._BibleFellowshipTeacher = value;
					this.SendPropertyChanged("BibleFellowshipTeacher");
					this.OnBibleFellowshipTeacherChanged();
				}

			}

		}

		
		[Column(Name="BibleFellowshipTeacherId", UpdateCheck=UpdateCheck.Never, Storage="_BibleFellowshipTeacherId", DbType="int", IsDbGenerated=true)]
		public int? BibleFellowshipTeacherId
		{
			get { return this._BibleFellowshipTeacherId; }

			set
			{
				if (this._BibleFellowshipTeacherId != value)
				{
				
                    this.OnBibleFellowshipTeacherIdChanging(value);
					this.SendPropertyChanging();
					this._BibleFellowshipTeacherId = value;
					this.SendPropertyChanged("BibleFellowshipTeacherId");
					this.OnBibleFellowshipTeacherIdChanged();
				}

			}

		}

		
		[Column(Name="LastContact", UpdateCheck=UpdateCheck.Never, Storage="_LastContact", DbType="datetime", IsDbGenerated=true)]
		public DateTime? LastContact
		{
			get { return this._LastContact; }

			set
			{
				if (this._LastContact != value)
				{
				
                    this.OnLastContactChanging(value);
					this.SendPropertyChanging();
					this._LastContact = value;
					this.SendPropertyChanged("LastContact");
					this.OnLastContactChanged();
				}

			}

		}

		
		[Column(Name="InBFClass", UpdateCheck=UpdateCheck.Never, Storage="_InBFClass", DbType="bit", IsDbGenerated=true)]
		public bool? InBFClass
		{
			get { return this._InBFClass; }

			set
			{
				if (this._InBFClass != value)
				{
				
                    this.OnInBFClassChanging(value);
					this.SendPropertyChanging();
					this._InBFClass = value;
					this.SendPropertyChanged("InBFClass");
					this.OnInBFClassChanged();
				}

			}

		}

		
		[Column(Name="Grade", UpdateCheck=UpdateCheck.Never, Storage="_Grade", DbType="int")]
		public int? Grade
		{
			get { return this._Grade; }

			set
			{
				if (this._Grade != value)
				{
				
                    this.OnGradeChanging(value);
					this.SendPropertyChanging();
					this._Grade = value;
					this.SendPropertyChanged("Grade");
					this.OnGradeChanged();
				}

			}

		}

		
		[Column(Name="CellPhoneLU", UpdateCheck=UpdateCheck.Never, Storage="_CellPhoneLU", DbType="char(7)")]
		public string CellPhoneLU
		{
			get { return this._CellPhoneLU; }

			set
			{
				if (this._CellPhoneLU != value)
				{
				
                    this.OnCellPhoneLUChanging(value);
					this.SendPropertyChanging();
					this._CellPhoneLU = value;
					this.SendPropertyChanged("CellPhoneLU");
					this.OnCellPhoneLUChanged();
				}

			}

		}

		
		[Column(Name="WorkPhoneLU", UpdateCheck=UpdateCheck.Never, Storage="_WorkPhoneLU", DbType="char(7)")]
		public string WorkPhoneLU
		{
			get { return this._WorkPhoneLU; }

			set
			{
				if (this._WorkPhoneLU != value)
				{
				
                    this.OnWorkPhoneLUChanging(value);
					this.SendPropertyChanging();
					this._WorkPhoneLU = value;
					this.SendPropertyChanged("WorkPhoneLU");
					this.OnWorkPhoneLUChanged();
				}

			}

		}

		
		[Column(Name="BibleFellowshipClassId", UpdateCheck=UpdateCheck.Never, Storage="_BibleFellowshipClassId", DbType="int", IsDbGenerated=true)]
		public int? BibleFellowshipClassId
		{
			get { return this._BibleFellowshipClassId; }

			set
			{
				if (this._BibleFellowshipClassId != value)
				{
				
                    this.OnBibleFellowshipClassIdChanging(value);
					this.SendPropertyChanging();
					this._BibleFellowshipClassId = value;
					this.SendPropertyChanged("BibleFellowshipClassId");
					this.OnBibleFellowshipClassIdChanged();
				}

			}

		}

		
		[Column(Name="Name", UpdateCheck=UpdateCheck.Never, Storage="_Name", DbType="varchar(56)", IsDbGenerated=true)]
		public string Name
		{
			get { return this._Name; }

			set
			{
				if (this._Name != value)
				{
				
                    this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}

			}

		}

		
		[Column(Name="Name2", UpdateCheck=UpdateCheck.Never, Storage="_Name2", DbType="varchar(57)", IsDbGenerated=true)]
		public string Name2
		{
			get { return this._Name2; }

			set
			{
				if (this._Name2 != value)
				{
				
                    this.OnName2Changing(value);
					this.SendPropertyChanging();
					this._Name2 = value;
					this.SendPropertyChanged("Name2");
					this.OnName2Changed();
				}

			}

		}

		
		[Column(Name="HashNum", UpdateCheck=UpdateCheck.Never, Storage="_HashNum", DbType="int", IsDbGenerated=true)]
		public int? HashNum
		{
			get { return this._HashNum; }

			set
			{
				if (this._HashNum != value)
				{
				
                    this.OnHashNumChanging(value);
					this.SendPropertyChanging();
					this._HashNum = value;
					this.SendPropertyChanged("HashNum");
					this.OnHashNumChanged();
				}

			}

		}

		
		[Column(Name="CampusId", UpdateCheck=UpdateCheck.Never, Storage="_CampusId", DbType="int")]
		public int? CampusId
		{
			get { return this._CampusId; }

			set
			{
				if (this._CampusId != value)
				{
				
					if (this._Campu.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnCampusIdChanging(value);
					this.SendPropertyChanging();
					this._CampusId = value;
					this.SendPropertyChanged("CampusId");
					this.OnCampusIdChanged();
				}

			}

		}

		
		[Column(Name="CellPhoneAC", UpdateCheck=UpdateCheck.Never, Storage="_CellPhoneAC", DbType="char(3)")]
		public string CellPhoneAC
		{
			get { return this._CellPhoneAC; }

			set
			{
				if (this._CellPhoneAC != value)
				{
				
                    this.OnCellPhoneACChanging(value);
					this.SendPropertyChanging();
					this._CellPhoneAC = value;
					this.SendPropertyChanged("CellPhoneAC");
					this.OnCellPhoneACChanged();
				}

			}

		}

		
		[Column(Name="PreferredName", UpdateCheck=UpdateCheck.Never, Storage="_PreferredName", DbType="varchar(25)", IsDbGenerated=true)]
		public string PreferredName
		{
			get { return this._PreferredName; }

			set
			{
				if (this._PreferredName != value)
				{
				
                    this.OnPreferredNameChanging(value);
					this.SendPropertyChanging();
					this._PreferredName = value;
					this.SendPropertyChanged("PreferredName");
					this.OnPreferredNameChanged();
				}

			}

		}

		
		[Column(Name="CheckInNotes", UpdateCheck=UpdateCheck.Never, Storage="_CheckInNotes", DbType="varchar(1000)")]
		public string CheckInNotes
		{
			get { return this._CheckInNotes; }

			set
			{
				if (this._CheckInNotes != value)
				{
				
                    this.OnCheckInNotesChanging(value);
					this.SendPropertyChanging();
					this._CheckInNotes = value;
					this.SendPropertyChanged("CheckInNotes");
					this.OnCheckInNotesChanged();
				}

			}

		}

		
		[Column(Name="Age", UpdateCheck=UpdateCheck.Never, Storage="_Age", DbType="int", IsDbGenerated=true)]
		public int? Age
		{
			get { return this._Age; }

			set
			{
				if (this._Age != value)
				{
				
                    this.OnAgeChanging(value);
					this.SendPropertyChanging();
					this._Age = value;
					this.SendPropertyChanged("Age");
					this.OnAgeChanged();
				}

			}

		}

		
    #endregion
        
    #region Foreign Key Tables
   		
   		[Association(Name="contactsHad__person", Storage="_contactsHad", OtherKey="PeopleId")]
   		public EntitySet< Contactee> contactsHad
   		{
   		    get { return this._contactsHad; }

			set	{ this._contactsHad.Assign(value); }

   		}

		
   		[Association(Name="contactsMade__person", Storage="_contactsMade", OtherKey="PeopleId")]
   		public EntitySet< Contactor> contactsMade
   		{
   		    get { return this._contactsMade; }

			set	{ this._contactsMade.Assign(value); }

   		}

		
   		[Association(Name="ENROLLMENT_TRANSACTION_PPL_FK", Storage="_EnrollmentTransactions", OtherKey="PeopleId")]
   		public EntitySet< EnrollmentTransaction> EnrollmentTransactions
   		{
   		    get { return this._EnrollmentTransactions; }

			set	{ this._EnrollmentTransactions.Assign(value); }

   		}

		
   		[Association(Name="FamiliesHeaded__HeadOfHousehold", Storage="_FamiliesHeaded", OtherKey="HeadOfHouseholdId")]
   		public EntitySet< Family> FamiliesHeaded
   		{
   		    get { return this._FamiliesHeaded; }

			set	{ this._FamiliesHeaded.Assign(value); }

   		}

		
   		[Association(Name="FamiliesHeaded2__HeadOfHouseholdSpouse", Storage="_FamiliesHeaded2", OtherKey="HeadOfHouseholdSpouseId")]
   		public EntitySet< Family> FamiliesHeaded2
   		{
   		    get { return this._FamiliesHeaded2; }

			set	{ this._FamiliesHeaded2.Assign(value); }

   		}

		
   		[Association(Name="FK_Attender_People", Storage="_MOBSRegs", OtherKey="PeopleId")]
   		public EntitySet< MOBSReg> MOBSRegs
   		{
   		    get { return this._MOBSRegs; }

			set	{ this._MOBSRegs.Assign(value); }

   		}

		
   		[Association(Name="FK_AttendWithAbsents_TBL_PEOPLE_TBL", Storage="_Attends", OtherKey="PeopleId")]
   		public EntitySet< Attend> Attends
   		{
   		    get { return this._Attends; }

			set	{ this._Attends.Assign(value); }

   		}

		
   		[Association(Name="FK_BadET_People", Storage="_BadETs", OtherKey="PeopleId")]
   		public EntitySet< BadET> BadETs
   		{
   		    get { return this._BadETs; }

			set	{ this._BadETs.Assign(value); }

   		}

		
   		[Association(Name="FK_CardIdentifiers_People", Storage="_CardIdentifiers", OtherKey="PeopleId")]
   		public EntitySet< CardIdentifier> CardIdentifiers
   		{
   		    get { return this._CardIdentifiers; }

			set	{ this._CardIdentifiers.Assign(value); }

   		}

		
   		[Association(Name="FK_CheckInTimes_People", Storage="_CheckInTimes", OtherKey="PeopleId")]
   		public EntitySet< CheckInTime> CheckInTimes
   		{
   		    get { return this._CheckInTimes; }

			set	{ this._CheckInTimes.Assign(value); }

   		}

		
   		[Association(Name="FK_Contribution_People", Storage="_Contributions", OtherKey="PeopleId")]
   		public EntitySet< Contribution> Contributions
   		{
   		    get { return this._Contributions; }

			set	{ this._Contributions.Assign(value); }

   		}

		
   		[Association(Name="FK_Participant_People", Storage="_RecRegs", OtherKey="PeopleId")]
   		public EntitySet< RecReg> RecRegs
   		{
   		    get { return this._RecRegs; }

			set	{ this._RecRegs.Assign(value); }

   		}

		
   		[Association(Name="FK_PendingNotifications_People", Storage="_PendingNotifications", OtherKey="PeopleId")]
   		public EntitySet< PendingNotification> PendingNotifications
   		{
   		    get { return this._PendingNotifications; }

			set	{ this._PendingNotifications.Assign(value); }

   		}

		
   		[Association(Name="FK_PeopleExtra_People", Storage="_PeopleExtras", OtherKey="PeopleId")]
   		public EntitySet< PeopleExtra> PeopleExtras
   		{
   		    get { return this._PeopleExtras; }

			set	{ this._PeopleExtras.Assign(value); }

   		}

		
   		[Association(Name="FK_PrayerSlot_People", Storage="_PrayerSlots", OtherKey="PeopleId")]
   		public EntitySet< PrayerSlot> PrayerSlots
   		{
   		    get { return this._PrayerSlots; }

			set	{ this._PrayerSlots.Assign(value); }

   		}

		
   		[Association(Name="FK_SaleTransaction_People", Storage="_SaleTransactions", OtherKey="PeopleId")]
   		public EntitySet< SaleTransaction> SaleTransactions
   		{
   		    get { return this._SaleTransactions; }

			set	{ this._SaleTransactions.Assign(value); }

   		}

		
   		[Association(Name="FK_TagShare_People", Storage="_TagShares", OtherKey="PeopleId")]
   		public EntitySet< TagShare> TagShares
   		{
   		    get { return this._TagShares; }

			set	{ this._TagShares.Assign(value); }

   		}

		
   		[Association(Name="FK_TaskListOwners_PEOPLE_TBL", Storage="_TaskListOwners", OtherKey="PeopleId")]
   		public EntitySet< TaskListOwner> TaskListOwners
   		{
   		    get { return this._TaskListOwners; }

			set	{ this._TaskListOwners.Assign(value); }

   		}

		
   		[Association(Name="FK_Users_People", Storage="_Users", OtherKey="PeopleId")]
   		public EntitySet< User> Users
   		{
   		    get { return this._Users; }

			set	{ this._Users.Assign(value); }

   		}

		
   		[Association(Name="FK_VBSApp_People", Storage="_VBSApps", OtherKey="PeopleId")]
   		public EntitySet< VBSApp> VBSApps
   		{
   		    get { return this._VBSApps; }

			set	{ this._VBSApps.Assign(value); }

   		}

		
   		[Association(Name="FK_VolInterestInterestCodes_People", Storage="_VolInterestInterestCodes", OtherKey="PeopleId")]
   		public EntitySet< VolInterestInterestCode> VolInterestInterestCodes
   		{
   		    get { return this._VolInterestInterestCodes; }

			set	{ this._VolInterestInterestCodes.Assign(value); }

   		}

		
   		[Association(Name="FK_Volunteer_PEOPLE_TBL", Storage="_Volunteers", OtherKey="PeopleId")]
   		public EntitySet< Volunteer> Volunteers
   		{
   		    get { return this._Volunteers; }

			set	{ this._Volunteers.Assign(value); }

   		}

		
   		[Association(Name="FK_VolunteerForm_PEOPLE_TBL", Storage="_VolunteerForms", OtherKey="PeopleId")]
   		public EntitySet< VolunteerForm> VolunteerForms
   		{
   		    get { return this._VolunteerForms; }

			set	{ this._VolunteerForms.Assign(value); }

   		}

		
   		[Association(Name="HerLoveRespects__Her", Storage="_HerLoveRespects", OtherKey="HerId")]
   		public EntitySet< LoveRespect> HerLoveRespects
   		{
   		    get { return this._HerLoveRespects; }

			set	{ this._HerLoveRespects.Assign(value); }

   		}

		
   		[Association(Name="HerSoulMates__Her", Storage="_HerSoulMates", OtherKey="HerId")]
   		public EntitySet< SoulMate> HerSoulMates
   		{
   		    get { return this._HerSoulMates; }

			set	{ this._HerSoulMates.Assign(value); }

   		}

		
   		[Association(Name="HisLoveRespects__Him", Storage="_HisLoveRespects", OtherKey="HimId")]
   		public EntitySet< LoveRespect> HisLoveRespects
   		{
   		    get { return this._HisLoveRespects; }

			set	{ this._HisLoveRespects.Assign(value); }

   		}

		
   		[Association(Name="HisSoulMates__Him", Storage="_HisSoulMates", OtherKey="HimId")]
   		public EntitySet< SoulMate> HisSoulMates
   		{
   		    get { return this._HisSoulMates; }

			set	{ this._HisSoulMates.Assign(value); }

   		}

		
   		[Association(Name="ORGANIZATION_MEMBERS_PPL_FK", Storage="_OrganizationMembers", OtherKey="PeopleId")]
   		public EntitySet< OrganizationMember> OrganizationMembers
   		{
   		    get { return this._OrganizationMembers; }

			set	{ this._OrganizationMembers.Assign(value); }

   		}

		
   		[Association(Name="Tags__Person", Storage="_Tags", OtherKey="PeopleId")]
   		public EntitySet< TagPerson> Tags
   		{
   		    get { return this._Tags; }

			set	{ this._Tags.Assign(value); }

   		}

		
   		[Association(Name="TagsOwned__PersonOwner", Storage="_TagsOwned", OtherKey="PeopleId")]
   		public EntitySet< Tag> TagsOwned
   		{
   		    get { return this._TagsOwned; }

			set	{ this._TagsOwned.Assign(value); }

   		}

		
   		[Association(Name="Tasks__Owner", Storage="_Tasks", OtherKey="OwnerId")]
   		public EntitySet< Task> Tasks
   		{
   		    get { return this._Tasks; }

			set	{ this._Tasks.Assign(value); }

   		}

		
   		[Association(Name="TasksAboutPerson__AboutWho", Storage="_TasksAboutPerson", OtherKey="WhoId")]
   		public EntitySet< Task> TasksAboutPerson
   		{
   		    get { return this._TasksAboutPerson; }

			set	{ this._TasksAboutPerson.Assign(value); }

   		}

		
   		[Association(Name="TasksCoOwned__CoOwner", Storage="_TasksCoOwned", OtherKey="CoOwnerId")]
   		public EntitySet< Task> TasksCoOwned
   		{
   		    get { return this._TasksCoOwned; }

			set	{ this._TasksCoOwned.Assign(value); }

   		}

		
	#endregion
	
	#region Foreign Keys
    	
		[Association(Name="FK_People_BaptismType", Storage="_BaptismType", ThisKey="BaptismTypeId", IsForeignKey=true)]
		public BaptismType BaptismType
		{
			get { return this._BaptismType.Entity; }

			set
			{
				BaptismType previousValue = this._BaptismType.Entity;
				if (((previousValue != value) 
							|| (this._BaptismType.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._BaptismType.Entity = null;
						previousValue.People.Remove(this);
					}

					this._BaptismType.Entity = value;
					if (value != null)
					{
						value.People.Add(this);
						
						this._BaptismTypeId = value.Id;
						
					}

					else
					{
						
						this._BaptismTypeId = default(int?);
						
					}

					this.SendPropertyChanged("BaptismType");
				}

			}

		}

		
		[Association(Name="FK_People_Campus", Storage="_Campu", ThisKey="CampusId", IsForeignKey=true)]
		public Campu Campu
		{
			get { return this._Campu.Entity; }

			set
			{
				Campu previousValue = this._Campu.Entity;
				if (((previousValue != value) 
							|| (this._Campu.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._Campu.Entity = null;
						previousValue.People.Remove(this);
					}

					this._Campu.Entity = value;
					if (value != null)
					{
						value.People.Add(this);
						
						this._CampusId = value.Id;
						
					}

					else
					{
						
						this._CampusId = default(int?);
						
					}

					this.SendPropertyChanged("Campu");
				}

			}

		}

		
		[Association(Name="FK_People_DecisionType", Storage="_DecisionType", ThisKey="DecisionTypeId", IsForeignKey=true)]
		public DecisionType DecisionType
		{
			get { return this._DecisionType.Entity; }

			set
			{
				DecisionType previousValue = this._DecisionType.Entity;
				if (((previousValue != value) 
							|| (this._DecisionType.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._DecisionType.Entity = null;
						previousValue.People.Remove(this);
					}

					this._DecisionType.Entity = value;
					if (value != null)
					{
						value.People.Add(this);
						
						this._DecisionTypeId = value.Id;
						
					}

					else
					{
						
						this._DecisionTypeId = default(int?);
						
					}

					this.SendPropertyChanged("DecisionType");
				}

			}

		}

		
		[Association(Name="FK_People_DropType", Storage="_DropType", ThisKey="DropCodeId", IsForeignKey=true)]
		public DropType DropType
		{
			get { return this._DropType.Entity; }

			set
			{
				DropType previousValue = this._DropType.Entity;
				if (((previousValue != value) 
							|| (this._DropType.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._DropType.Entity = null;
						previousValue.People.Remove(this);
					}

					this._DropType.Entity = value;
					if (value != null)
					{
						value.People.Add(this);
						
						this._DropCodeId = value.Id;
						
					}

					else
					{
						
						this._DropCodeId = default(int);
						
					}

					this.SendPropertyChanged("DropType");
				}

			}

		}

		
		[Association(Name="FK_People_Families", Storage="_Family", ThisKey="FamilyId", IsForeignKey=true)]
		public Family Family
		{
			get { return this._Family.Entity; }

			set
			{
				Family previousValue = this._Family.Entity;
				if (((previousValue != value) 
							|| (this._Family.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._Family.Entity = null;
						previousValue.People.Remove(this);
					}

					this._Family.Entity = value;
					if (value != null)
					{
						value.People.Add(this);
						
						this._FamilyId = value.FamilyId;
						
					}

					else
					{
						
						this._FamilyId = default(int);
						
					}

					this.SendPropertyChanged("Family");
				}

			}

		}

		
		[Association(Name="FK_People_FamilyPosition", Storage="_FamilyPosition", ThisKey="PositionInFamilyId", IsForeignKey=true)]
		public FamilyPosition FamilyPosition
		{
			get { return this._FamilyPosition.Entity; }

			set
			{
				FamilyPosition previousValue = this._FamilyPosition.Entity;
				if (((previousValue != value) 
							|| (this._FamilyPosition.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._FamilyPosition.Entity = null;
						previousValue.People.Remove(this);
					}

					this._FamilyPosition.Entity = value;
					if (value != null)
					{
						value.People.Add(this);
						
						this._PositionInFamilyId = value.Id;
						
					}

					else
					{
						
						this._PositionInFamilyId = default(int);
						
					}

					this.SendPropertyChanged("FamilyPosition");
				}

			}

		}

		
		[Association(Name="FK_People_Gender", Storage="_Gender", ThisKey="GenderId", IsForeignKey=true)]
		public Gender Gender
		{
			get { return this._Gender.Entity; }

			set
			{
				Gender previousValue = this._Gender.Entity;
				if (((previousValue != value) 
							|| (this._Gender.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._Gender.Entity = null;
						previousValue.People.Remove(this);
					}

					this._Gender.Entity = value;
					if (value != null)
					{
						value.People.Add(this);
						
						this._GenderId = value.Id;
						
					}

					else
					{
						
						this._GenderId = default(int);
						
					}

					this.SendPropertyChanged("Gender");
				}

			}

		}

		
		[Association(Name="FK_People_JoinType", Storage="_JoinType", ThisKey="JoinCodeId", IsForeignKey=true)]
		public JoinType JoinType
		{
			get { return this._JoinType.Entity; }

			set
			{
				JoinType previousValue = this._JoinType.Entity;
				if (((previousValue != value) 
							|| (this._JoinType.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._JoinType.Entity = null;
						previousValue.People.Remove(this);
					}

					this._JoinType.Entity = value;
					if (value != null)
					{
						value.People.Add(this);
						
						this._JoinCodeId = value.Id;
						
					}

					else
					{
						
						this._JoinCodeId = default(int);
						
					}

					this.SendPropertyChanged("JoinType");
				}

			}

		}

		
		[Association(Name="FK_People_MaritalStatus", Storage="_MaritalStatus", ThisKey="MaritalStatusId", IsForeignKey=true)]
		public MaritalStatus MaritalStatus
		{
			get { return this._MaritalStatus.Entity; }

			set
			{
				MaritalStatus previousValue = this._MaritalStatus.Entity;
				if (((previousValue != value) 
							|| (this._MaritalStatus.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._MaritalStatus.Entity = null;
						previousValue.People.Remove(this);
					}

					this._MaritalStatus.Entity = value;
					if (value != null)
					{
						value.People.Add(this);
						
						this._MaritalStatusId = value.Id;
						
					}

					else
					{
						
						this._MaritalStatusId = default(int);
						
					}

					this.SendPropertyChanged("MaritalStatus");
				}

			}

		}

		
		[Association(Name="FK_People_MemberStatus", Storage="_MemberStatus", ThisKey="MemberStatusId", IsForeignKey=true)]
		public MemberStatus MemberStatus
		{
			get { return this._MemberStatus.Entity; }

			set
			{
				MemberStatus previousValue = this._MemberStatus.Entity;
				if (((previousValue != value) 
							|| (this._MemberStatus.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._MemberStatus.Entity = null;
						previousValue.People.Remove(this);
					}

					this._MemberStatus.Entity = value;
					if (value != null)
					{
						value.People.Add(this);
						
						this._MemberStatusId = value.Id;
						
					}

					else
					{
						
						this._MemberStatusId = default(int);
						
					}

					this.SendPropertyChanged("MemberStatus");
				}

			}

		}

		
		[Association(Name="FK_People_Origin", Storage="_Origin", ThisKey="OriginId", IsForeignKey=true)]
		public Origin Origin
		{
			get { return this._Origin.Entity; }

			set
			{
				Origin previousValue = this._Origin.Entity;
				if (((previousValue != value) 
							|| (this._Origin.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._Origin.Entity = null;
						previousValue.People.Remove(this);
					}

					this._Origin.Entity = value;
					if (value != null)
					{
						value.People.Add(this);
						
						this._OriginId = value.Id;
						
					}

					else
					{
						
						this._OriginId = default(int?);
						
					}

					this.SendPropertyChanged("Origin");
				}

			}

		}

		
		[Association(Name="FK_PEOPLE_TBL_InterestPoint", Storage="_InterestPoint", ThisKey="InterestPointId", IsForeignKey=true)]
		public InterestPoint InterestPoint
		{
			get { return this._InterestPoint.Entity; }

			set
			{
				InterestPoint previousValue = this._InterestPoint.Entity;
				if (((previousValue != value) 
							|| (this._InterestPoint.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._InterestPoint.Entity = null;
						previousValue.People.Remove(this);
					}

					this._InterestPoint.Entity = value;
					if (value != null)
					{
						value.People.Add(this);
						
						this._InterestPointId = value.Id;
						
					}

					else
					{
						
						this._InterestPointId = default(int?);
						
					}

					this.SendPropertyChanged("InterestPoint");
				}

			}

		}

		
		[Association(Name="FK_PEOPLE_TBL_Picture", Storage="_Picture", ThisKey="PictureId", IsForeignKey=true)]
		public Picture Picture
		{
			get { return this._Picture.Entity; }

			set
			{
				Picture previousValue = this._Picture.Entity;
				if (((previousValue != value) 
							|| (this._Picture.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._Picture.Entity = null;
						previousValue.People.Remove(this);
					}

					this._Picture.Entity = value;
					if (value != null)
					{
						value.People.Add(this);
						
						this._PictureId = value.PictureId;
						
					}

					else
					{
						
						this._PictureId = default(int?);
						
					}

					this.SendPropertyChanged("Picture");
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

   		
		private void attach_contactsHad(Contactee entity)
		{
			this.SendPropertyChanging();
			entity.person = this;
		}

		private void detach_contactsHad(Contactee entity)
		{
			this.SendPropertyChanging();
			entity.person = null;
		}

		
		private void attach_contactsMade(Contactor entity)
		{
			this.SendPropertyChanging();
			entity.person = this;
		}

		private void detach_contactsMade(Contactor entity)
		{
			this.SendPropertyChanging();
			entity.person = null;
		}

		
		private void attach_EnrollmentTransactions(EnrollmentTransaction entity)
		{
			this.SendPropertyChanging();
			entity.Person = this;
		}

		private void detach_EnrollmentTransactions(EnrollmentTransaction entity)
		{
			this.SendPropertyChanging();
			entity.Person = null;
		}

		
		private void attach_FamiliesHeaded(Family entity)
		{
			this.SendPropertyChanging();
			entity.HeadOfHousehold = this;
		}

		private void detach_FamiliesHeaded(Family entity)
		{
			this.SendPropertyChanging();
			entity.HeadOfHousehold = null;
		}

		
		private void attach_FamiliesHeaded2(Family entity)
		{
			this.SendPropertyChanging();
			entity.HeadOfHouseholdSpouse = this;
		}

		private void detach_FamiliesHeaded2(Family entity)
		{
			this.SendPropertyChanging();
			entity.HeadOfHouseholdSpouse = null;
		}

		
		private void attach_MOBSRegs(MOBSReg entity)
		{
			this.SendPropertyChanging();
			entity.Person = this;
		}

		private void detach_MOBSRegs(MOBSReg entity)
		{
			this.SendPropertyChanging();
			entity.Person = null;
		}

		
		private void attach_Attends(Attend entity)
		{
			this.SendPropertyChanging();
			entity.Person = this;
		}

		private void detach_Attends(Attend entity)
		{
			this.SendPropertyChanging();
			entity.Person = null;
		}

		
		private void attach_BadETs(BadET entity)
		{
			this.SendPropertyChanging();
			entity.Person = this;
		}

		private void detach_BadETs(BadET entity)
		{
			this.SendPropertyChanging();
			entity.Person = null;
		}

		
		private void attach_CardIdentifiers(CardIdentifier entity)
		{
			this.SendPropertyChanging();
			entity.Person = this;
		}

		private void detach_CardIdentifiers(CardIdentifier entity)
		{
			this.SendPropertyChanging();
			entity.Person = null;
		}

		
		private void attach_CheckInTimes(CheckInTime entity)
		{
			this.SendPropertyChanging();
			entity.Person = this;
		}

		private void detach_CheckInTimes(CheckInTime entity)
		{
			this.SendPropertyChanging();
			entity.Person = null;
		}

		
		private void attach_Contributions(Contribution entity)
		{
			this.SendPropertyChanging();
			entity.Person = this;
		}

		private void detach_Contributions(Contribution entity)
		{
			this.SendPropertyChanging();
			entity.Person = null;
		}

		
		private void attach_RecRegs(RecReg entity)
		{
			this.SendPropertyChanging();
			entity.Person = this;
		}

		private void detach_RecRegs(RecReg entity)
		{
			this.SendPropertyChanging();
			entity.Person = null;
		}

		
		private void attach_PendingNotifications(PendingNotification entity)
		{
			this.SendPropertyChanging();
			entity.Person = this;
		}

		private void detach_PendingNotifications(PendingNotification entity)
		{
			this.SendPropertyChanging();
			entity.Person = null;
		}

		
		private void attach_PeopleExtras(PeopleExtra entity)
		{
			this.SendPropertyChanging();
			entity.Person = this;
		}

		private void detach_PeopleExtras(PeopleExtra entity)
		{
			this.SendPropertyChanging();
			entity.Person = null;
		}

		
		private void attach_PrayerSlots(PrayerSlot entity)
		{
			this.SendPropertyChanging();
			entity.Person = this;
		}

		private void detach_PrayerSlots(PrayerSlot entity)
		{
			this.SendPropertyChanging();
			entity.Person = null;
		}

		
		private void attach_SaleTransactions(SaleTransaction entity)
		{
			this.SendPropertyChanging();
			entity.Person = this;
		}

		private void detach_SaleTransactions(SaleTransaction entity)
		{
			this.SendPropertyChanging();
			entity.Person = null;
		}

		
		private void attach_TagShares(TagShare entity)
		{
			this.SendPropertyChanging();
			entity.Person = this;
		}

		private void detach_TagShares(TagShare entity)
		{
			this.SendPropertyChanging();
			entity.Person = null;
		}

		
		private void attach_TaskListOwners(TaskListOwner entity)
		{
			this.SendPropertyChanging();
			entity.Person = this;
		}

		private void detach_TaskListOwners(TaskListOwner entity)
		{
			this.SendPropertyChanging();
			entity.Person = null;
		}

		
		private void attach_Users(User entity)
		{
			this.SendPropertyChanging();
			entity.Person = this;
		}

		private void detach_Users(User entity)
		{
			this.SendPropertyChanging();
			entity.Person = null;
		}

		
		private void attach_VBSApps(VBSApp entity)
		{
			this.SendPropertyChanging();
			entity.Person = this;
		}

		private void detach_VBSApps(VBSApp entity)
		{
			this.SendPropertyChanging();
			entity.Person = null;
		}

		
		private void attach_VolInterestInterestCodes(VolInterestInterestCode entity)
		{
			this.SendPropertyChanging();
			entity.Person = this;
		}

		private void detach_VolInterestInterestCodes(VolInterestInterestCode entity)
		{
			this.SendPropertyChanging();
			entity.Person = null;
		}

		
		private void attach_Volunteers(Volunteer entity)
		{
			this.SendPropertyChanging();
			entity.Person = this;
		}

		private void detach_Volunteers(Volunteer entity)
		{
			this.SendPropertyChanging();
			entity.Person = null;
		}

		
		private void attach_VolunteerForms(VolunteerForm entity)
		{
			this.SendPropertyChanging();
			entity.Person = this;
		}

		private void detach_VolunteerForms(VolunteerForm entity)
		{
			this.SendPropertyChanging();
			entity.Person = null;
		}

		
		private void attach_HerLoveRespects(LoveRespect entity)
		{
			this.SendPropertyChanging();
			entity.Her = this;
		}

		private void detach_HerLoveRespects(LoveRespect entity)
		{
			this.SendPropertyChanging();
			entity.Her = null;
		}

		
		private void attach_HerSoulMates(SoulMate entity)
		{
			this.SendPropertyChanging();
			entity.Her = this;
		}

		private void detach_HerSoulMates(SoulMate entity)
		{
			this.SendPropertyChanging();
			entity.Her = null;
		}

		
		private void attach_HisLoveRespects(LoveRespect entity)
		{
			this.SendPropertyChanging();
			entity.Him = this;
		}

		private void detach_HisLoveRespects(LoveRespect entity)
		{
			this.SendPropertyChanging();
			entity.Him = null;
		}

		
		private void attach_HisSoulMates(SoulMate entity)
		{
			this.SendPropertyChanging();
			entity.Him = this;
		}

		private void detach_HisSoulMates(SoulMate entity)
		{
			this.SendPropertyChanging();
			entity.Him = null;
		}

		
		private void attach_OrganizationMembers(OrganizationMember entity)
		{
			this.SendPropertyChanging();
			entity.Person = this;
		}

		private void detach_OrganizationMembers(OrganizationMember entity)
		{
			this.SendPropertyChanging();
			entity.Person = null;
		}

		
		private void attach_Tags(TagPerson entity)
		{
			this.SendPropertyChanging();
			entity.Person = this;
		}

		private void detach_Tags(TagPerson entity)
		{
			this.SendPropertyChanging();
			entity.Person = null;
		}

		
		private void attach_TagsOwned(Tag entity)
		{
			this.SendPropertyChanging();
			entity.PersonOwner = this;
		}

		private void detach_TagsOwned(Tag entity)
		{
			this.SendPropertyChanging();
			entity.PersonOwner = null;
		}

		
		private void attach_Tasks(Task entity)
		{
			this.SendPropertyChanging();
			entity.Owner = this;
		}

		private void detach_Tasks(Task entity)
		{
			this.SendPropertyChanging();
			entity.Owner = null;
		}

		
		private void attach_TasksAboutPerson(Task entity)
		{
			this.SendPropertyChanging();
			entity.AboutWho = this;
		}

		private void detach_TasksAboutPerson(Task entity)
		{
			this.SendPropertyChanging();
			entity.AboutWho = null;
		}

		
		private void attach_TasksCoOwned(Task entity)
		{
			this.SendPropertyChanging();
			entity.CoOwner = this;
		}

		private void detach_TasksCoOwned(Task entity)
		{
			this.SendPropertyChanging();
			entity.CoOwner = null;
		}

		
	}

}

