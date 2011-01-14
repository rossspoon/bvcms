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
	[Table(Name="dbo.Organizations")]
	public partial class Organization : INotifyPropertyChanging, INotifyPropertyChanged
	{
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
	#region Private Fields
		
		private int _OrganizationId;
		
		private int _CreatedBy;
		
		private DateTime _CreatedDate;
		
		private int _OrganizationStatusId;
		
		private int? _DivisionId;
		
		private int? _LeaderMemberTypeId;
		
		private int? _GradeAgeStart;
		
		private int? _GradeAgeEnd;
		
		private int? _RollSheetVisitorWks;
		
		private int _AttendTrkLevelId;
		
		private int _SecurityTypeId;
		
		private int _AttendClassificationId;
		
		private DateTime? _FirstMeetingDate;
		
		private DateTime? _LastMeetingDate;
		
		private DateTime? _OrganizationClosedDate;
		
		private string _Location;
		
		private string _OrganizationName;
		
		private int? _ModifiedBy;
		
		private DateTime? _ModifiedDate;
		
		private int? _ScheduleId;
		
		private int? _EntryPointId;
		
		private int? _ParentOrgId;
		
		private bool _AllowAttendOverlap;
		
		private int? _MemberCount;
		
		private int? _LeaderId;
		
		private string _LeaderName;
		
		private bool? _ClassFilled;
		
		private int? _OnLineCatalogSort;
		
		private string _PendingLoc;
		
		private bool? _CanSelfCheckin;
		
		private int? _NumCheckInLabels;
		
		private int? _CampusId;
		
		private bool? _AllowNonCampusCheckIn;
		
		private int? _NumWorkerCheckInLabels;
		
		private DateTime? _SchedTime;
		
		private int? _SchedDay;
		
		private DateTime? _MeetingTime;
		
		private bool? _ShowOnlyRegisteredAtCheckIn;
		
		private int? _Limit;
		
		private string _EmailAddresses;
		
		private string _RegType;
		
		private string _EmailMessage;
		
		private string _EmailSubject;
		
		private string _Instructions;
		
		private int? _GenderId;
		
		private decimal? _Fee;
		
		private string _Description;
		
		private DateTime? _BirthDayStart;
		
		private DateTime? _BirthDayEnd;
		
		private DateTime? _VisitorDate;
		
		private decimal? _Deposit;
		
		private decimal? _ShirtFee;
		
		private decimal? _ExtraFee;
		
		private DateTime? _LastDayBeforeExtra;
		
		private bool? _AskTylenolEtc;
		
		private bool? _AskAllergies;
		
		private bool? _AskShirtSize;
		
		private bool? _AskRequest;
		
		private bool? _AskParents;
		
		private bool? _AskEmContact;
		
		private bool? _AskMedical;
		
		private bool? _AskInsurance;
		
		private bool? _AllowLastYearShirt;
		
		private bool? _AskDoctor;
		
		private bool? _AskCoaching;
		
		private bool? _AskChurch;
		
		private bool? _AskGrade;
		
		private string _Terms;
		
		private bool? _AskTickets;
		
		private decimal? _MaximumFee;
		
		private string _AskOptions;
		
		private string _AgeFee;
		
		private bool? _AllowOnlyOne;
		
		private int? _RegistrationTypeId;
		
		private string _AgeGroups;
		
		private string _ValidateOrgs;
		
		private bool? _MemberOnly;
		
		private string _YesNoQuestions;
		
		private string _OrgMemberFees;
		
		private string _ExtraQuestions;
		
		private string _PhoneNumber;
		
		private string _GradeOptions;
		
		private string _LinkGroupsFromOrgs;
		
		private bool? _RegistrationClosed;
		
		private bool? _AllowKioskRegister;
		
		private string _RequestLabel;
		
		private string _WorshipGroupCodes;
		
		private bool? _IsBibleFellowshipOrg;
		
		private string _ExtraOptions;
		
		private string _Shell;
		
		private string _ShirtSizes;
		
		private bool? _NoSecurityLabel;
		
		private bool? _AlwaysSecurityLabel;
		
		private string _MenuItems;
		
		private string _OptionsLabel;
		
		private string _ExtraOptionsLabel;
		
		private int? _DaysToIgnoreHistory;
		
		private string _GroupToJoin;
		
		private bool? _GiveOrgMembAccess;
		
		private string _NumItemsLabel;
		
		private bool? _NotReqDOB;
		
		private bool? _NotReqAddr;
		
		private bool? _NotReqZip;
		
		private bool? _NotReqPhone;
		
		private bool? _NotReqGender;
		
		private bool? _NotReqMarital;
		
		private string _GradeLabel;
		
   		
   		private EntitySet< Person> _BFMembers;
		
   		private EntitySet< Organization> _ChildOrgs;
		
   		private EntitySet< EnrollmentTransaction> _EnrollmentTransactions;
		
   		private EntitySet< Attend> _Attends;
		
   		private EntitySet< BadET> _BadETs;
		
   		private EntitySet< CheckInTime> _CheckInTimes;
		
   		private EntitySet< Coupon> _Coupons;
		
   		private EntitySet< DivOrg> _DivOrgs;
		
   		private EntitySet< LoveRespect> _LoveRespects;
		
   		private EntitySet< Meeting> _Meetings;
		
   		private EntitySet< MemberTag> _MemberTags;
		
   		private EntitySet< OrganizationMember> _OrganizationMembers;
		
    	
		private EntityRef< Organization> _ParentOrg;
		
		private EntityRef< AttendTrackLevel> _AttendTrackLevel;
		
		private EntityRef< Campu> _Campu;
		
		private EntityRef< Division> _Division;
		
		private EntityRef< Gender> _Gender;
		
		private EntityRef< RegistrationType> _RegistrationType;
		
		private EntityRef< EntryPoint> _EntryPoint;
		
		private EntityRef< OrganizationStatus> _OrganizationStatus;
		
	#endregion
	
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
		
		partial void OnOrganizationIdChanging(int value);
		partial void OnOrganizationIdChanged();
		
		partial void OnCreatedByChanging(int value);
		partial void OnCreatedByChanged();
		
		partial void OnCreatedDateChanging(DateTime value);
		partial void OnCreatedDateChanged();
		
		partial void OnOrganizationStatusIdChanging(int value);
		partial void OnOrganizationStatusIdChanged();
		
		partial void OnDivisionIdChanging(int? value);
		partial void OnDivisionIdChanged();
		
		partial void OnLeaderMemberTypeIdChanging(int? value);
		partial void OnLeaderMemberTypeIdChanged();
		
		partial void OnGradeAgeStartChanging(int? value);
		partial void OnGradeAgeStartChanged();
		
		partial void OnGradeAgeEndChanging(int? value);
		partial void OnGradeAgeEndChanged();
		
		partial void OnRollSheetVisitorWksChanging(int? value);
		partial void OnRollSheetVisitorWksChanged();
		
		partial void OnAttendTrkLevelIdChanging(int value);
		partial void OnAttendTrkLevelIdChanged();
		
		partial void OnSecurityTypeIdChanging(int value);
		partial void OnSecurityTypeIdChanged();
		
		partial void OnAttendClassificationIdChanging(int value);
		partial void OnAttendClassificationIdChanged();
		
		partial void OnFirstMeetingDateChanging(DateTime? value);
		partial void OnFirstMeetingDateChanged();
		
		partial void OnLastMeetingDateChanging(DateTime? value);
		partial void OnLastMeetingDateChanged();
		
		partial void OnOrganizationClosedDateChanging(DateTime? value);
		partial void OnOrganizationClosedDateChanged();
		
		partial void OnLocationChanging(string value);
		partial void OnLocationChanged();
		
		partial void OnOrganizationNameChanging(string value);
		partial void OnOrganizationNameChanged();
		
		partial void OnModifiedByChanging(int? value);
		partial void OnModifiedByChanged();
		
		partial void OnModifiedDateChanging(DateTime? value);
		partial void OnModifiedDateChanged();
		
		partial void OnScheduleIdChanging(int? value);
		partial void OnScheduleIdChanged();
		
		partial void OnEntryPointIdChanging(int? value);
		partial void OnEntryPointIdChanged();
		
		partial void OnParentOrgIdChanging(int? value);
		partial void OnParentOrgIdChanged();
		
		partial void OnAllowAttendOverlapChanging(bool value);
		partial void OnAllowAttendOverlapChanged();
		
		partial void OnMemberCountChanging(int? value);
		partial void OnMemberCountChanged();
		
		partial void OnLeaderIdChanging(int? value);
		partial void OnLeaderIdChanged();
		
		partial void OnLeaderNameChanging(string value);
		partial void OnLeaderNameChanged();
		
		partial void OnClassFilledChanging(bool? value);
		partial void OnClassFilledChanged();
		
		partial void OnOnLineCatalogSortChanging(int? value);
		partial void OnOnLineCatalogSortChanged();
		
		partial void OnPendingLocChanging(string value);
		partial void OnPendingLocChanged();
		
		partial void OnCanSelfCheckinChanging(bool? value);
		partial void OnCanSelfCheckinChanged();
		
		partial void OnNumCheckInLabelsChanging(int? value);
		partial void OnNumCheckInLabelsChanged();
		
		partial void OnCampusIdChanging(int? value);
		partial void OnCampusIdChanged();
		
		partial void OnAllowNonCampusCheckInChanging(bool? value);
		partial void OnAllowNonCampusCheckInChanged();
		
		partial void OnNumWorkerCheckInLabelsChanging(int? value);
		partial void OnNumWorkerCheckInLabelsChanged();
		
		partial void OnSchedTimeChanging(DateTime? value);
		partial void OnSchedTimeChanged();
		
		partial void OnSchedDayChanging(int? value);
		partial void OnSchedDayChanged();
		
		partial void OnMeetingTimeChanging(DateTime? value);
		partial void OnMeetingTimeChanged();
		
		partial void OnShowOnlyRegisteredAtCheckInChanging(bool? value);
		partial void OnShowOnlyRegisteredAtCheckInChanged();
		
		partial void OnLimitChanging(int? value);
		partial void OnLimitChanged();
		
		partial void OnEmailAddressesChanging(string value);
		partial void OnEmailAddressesChanged();
		
		partial void OnRegTypeChanging(string value);
		partial void OnRegTypeChanged();
		
		partial void OnEmailMessageChanging(string value);
		partial void OnEmailMessageChanged();
		
		partial void OnEmailSubjectChanging(string value);
		partial void OnEmailSubjectChanged();
		
		partial void OnInstructionsChanging(string value);
		partial void OnInstructionsChanged();
		
		partial void OnGenderIdChanging(int? value);
		partial void OnGenderIdChanged();
		
		partial void OnFeeChanging(decimal? value);
		partial void OnFeeChanged();
		
		partial void OnDescriptionChanging(string value);
		partial void OnDescriptionChanged();
		
		partial void OnBirthDayStartChanging(DateTime? value);
		partial void OnBirthDayStartChanged();
		
		partial void OnBirthDayEndChanging(DateTime? value);
		partial void OnBirthDayEndChanged();
		
		partial void OnVisitorDateChanging(DateTime? value);
		partial void OnVisitorDateChanged();
		
		partial void OnDepositChanging(decimal? value);
		partial void OnDepositChanged();
		
		partial void OnShirtFeeChanging(decimal? value);
		partial void OnShirtFeeChanged();
		
		partial void OnExtraFeeChanging(decimal? value);
		partial void OnExtraFeeChanged();
		
		partial void OnLastDayBeforeExtraChanging(DateTime? value);
		partial void OnLastDayBeforeExtraChanged();
		
		partial void OnAskTylenolEtcChanging(bool? value);
		partial void OnAskTylenolEtcChanged();
		
		partial void OnAskAllergiesChanging(bool? value);
		partial void OnAskAllergiesChanged();
		
		partial void OnAskShirtSizeChanging(bool? value);
		partial void OnAskShirtSizeChanged();
		
		partial void OnAskRequestChanging(bool? value);
		partial void OnAskRequestChanged();
		
		partial void OnAskParentsChanging(bool? value);
		partial void OnAskParentsChanged();
		
		partial void OnAskEmContactChanging(bool? value);
		partial void OnAskEmContactChanged();
		
		partial void OnAskMedicalChanging(bool? value);
		partial void OnAskMedicalChanged();
		
		partial void OnAskInsuranceChanging(bool? value);
		partial void OnAskInsuranceChanged();
		
		partial void OnAllowLastYearShirtChanging(bool? value);
		partial void OnAllowLastYearShirtChanged();
		
		partial void OnAskDoctorChanging(bool? value);
		partial void OnAskDoctorChanged();
		
		partial void OnAskCoachingChanging(bool? value);
		partial void OnAskCoachingChanged();
		
		partial void OnAskChurchChanging(bool? value);
		partial void OnAskChurchChanged();
		
		partial void OnAskGradeChanging(bool? value);
		partial void OnAskGradeChanged();
		
		partial void OnTermsChanging(string value);
		partial void OnTermsChanged();
		
		partial void OnAskTicketsChanging(bool? value);
		partial void OnAskTicketsChanged();
		
		partial void OnMaximumFeeChanging(decimal? value);
		partial void OnMaximumFeeChanged();
		
		partial void OnAskOptionsChanging(string value);
		partial void OnAskOptionsChanged();
		
		partial void OnAgeFeeChanging(string value);
		partial void OnAgeFeeChanged();
		
		partial void OnAllowOnlyOneChanging(bool? value);
		partial void OnAllowOnlyOneChanged();
		
		partial void OnRegistrationTypeIdChanging(int? value);
		partial void OnRegistrationTypeIdChanged();
		
		partial void OnAgeGroupsChanging(string value);
		partial void OnAgeGroupsChanged();
		
		partial void OnValidateOrgsChanging(string value);
		partial void OnValidateOrgsChanged();
		
		partial void OnMemberOnlyChanging(bool? value);
		partial void OnMemberOnlyChanged();
		
		partial void OnYesNoQuestionsChanging(string value);
		partial void OnYesNoQuestionsChanged();
		
		partial void OnOrgMemberFeesChanging(string value);
		partial void OnOrgMemberFeesChanged();
		
		partial void OnExtraQuestionsChanging(string value);
		partial void OnExtraQuestionsChanged();
		
		partial void OnPhoneNumberChanging(string value);
		partial void OnPhoneNumberChanged();
		
		partial void OnGradeOptionsChanging(string value);
		partial void OnGradeOptionsChanged();
		
		partial void OnLinkGroupsFromOrgsChanging(string value);
		partial void OnLinkGroupsFromOrgsChanged();
		
		partial void OnRegistrationClosedChanging(bool? value);
		partial void OnRegistrationClosedChanged();
		
		partial void OnAllowKioskRegisterChanging(bool? value);
		partial void OnAllowKioskRegisterChanged();
		
		partial void OnRequestLabelChanging(string value);
		partial void OnRequestLabelChanged();
		
		partial void OnWorshipGroupCodesChanging(string value);
		partial void OnWorshipGroupCodesChanged();
		
		partial void OnIsBibleFellowshipOrgChanging(bool? value);
		partial void OnIsBibleFellowshipOrgChanged();
		
		partial void OnExtraOptionsChanging(string value);
		partial void OnExtraOptionsChanged();
		
		partial void OnShellChanging(string value);
		partial void OnShellChanged();
		
		partial void OnShirtSizesChanging(string value);
		partial void OnShirtSizesChanged();
		
		partial void OnNoSecurityLabelChanging(bool? value);
		partial void OnNoSecurityLabelChanged();
		
		partial void OnAlwaysSecurityLabelChanging(bool? value);
		partial void OnAlwaysSecurityLabelChanged();
		
		partial void OnMenuItemsChanging(string value);
		partial void OnMenuItemsChanged();
		
		partial void OnOptionsLabelChanging(string value);
		partial void OnOptionsLabelChanged();
		
		partial void OnExtraOptionsLabelChanging(string value);
		partial void OnExtraOptionsLabelChanged();
		
		partial void OnDaysToIgnoreHistoryChanging(int? value);
		partial void OnDaysToIgnoreHistoryChanged();
		
		partial void OnGroupToJoinChanging(string value);
		partial void OnGroupToJoinChanged();
		
		partial void OnGiveOrgMembAccessChanging(bool? value);
		partial void OnGiveOrgMembAccessChanged();
		
		partial void OnNumItemsLabelChanging(string value);
		partial void OnNumItemsLabelChanged();
		
		partial void OnNotReqDOBChanging(bool? value);
		partial void OnNotReqDOBChanged();
		
		partial void OnNotReqAddrChanging(bool? value);
		partial void OnNotReqAddrChanged();
		
		partial void OnNotReqZipChanging(bool? value);
		partial void OnNotReqZipChanged();
		
		partial void OnNotReqPhoneChanging(bool? value);
		partial void OnNotReqPhoneChanged();
		
		partial void OnNotReqGenderChanging(bool? value);
		partial void OnNotReqGenderChanged();
		
		partial void OnNotReqMaritalChanging(bool? value);
		partial void OnNotReqMaritalChanged();
		
		partial void OnGradeLabelChanging(string value);
		partial void OnGradeLabelChanged();
		
    #endregion
		public Organization()
		{
			
			this._BFMembers = new EntitySet< Person>(new Action< Person>(this.attach_BFMembers), new Action< Person>(this.detach_BFMembers)); 
			
			this._ChildOrgs = new EntitySet< Organization>(new Action< Organization>(this.attach_ChildOrgs), new Action< Organization>(this.detach_ChildOrgs)); 
			
			this._EnrollmentTransactions = new EntitySet< EnrollmentTransaction>(new Action< EnrollmentTransaction>(this.attach_EnrollmentTransactions), new Action< EnrollmentTransaction>(this.detach_EnrollmentTransactions)); 
			
			this._Attends = new EntitySet< Attend>(new Action< Attend>(this.attach_Attends), new Action< Attend>(this.detach_Attends)); 
			
			this._BadETs = new EntitySet< BadET>(new Action< BadET>(this.attach_BadETs), new Action< BadET>(this.detach_BadETs)); 
			
			this._CheckInTimes = new EntitySet< CheckInTime>(new Action< CheckInTime>(this.attach_CheckInTimes), new Action< CheckInTime>(this.detach_CheckInTimes)); 
			
			this._Coupons = new EntitySet< Coupon>(new Action< Coupon>(this.attach_Coupons), new Action< Coupon>(this.detach_Coupons)); 
			
			this._DivOrgs = new EntitySet< DivOrg>(new Action< DivOrg>(this.attach_DivOrgs), new Action< DivOrg>(this.detach_DivOrgs)); 
			
			this._LoveRespects = new EntitySet< LoveRespect>(new Action< LoveRespect>(this.attach_LoveRespects), new Action< LoveRespect>(this.detach_LoveRespects)); 
			
			this._Meetings = new EntitySet< Meeting>(new Action< Meeting>(this.attach_Meetings), new Action< Meeting>(this.detach_Meetings)); 
			
			this._MemberTags = new EntitySet< MemberTag>(new Action< MemberTag>(this.attach_MemberTags), new Action< MemberTag>(this.detach_MemberTags)); 
			
			this._OrganizationMembers = new EntitySet< OrganizationMember>(new Action< OrganizationMember>(this.attach_OrganizationMembers), new Action< OrganizationMember>(this.detach_OrganizationMembers)); 
			
			
			this._ParentOrg = default(EntityRef< Organization>); 
			
			this._AttendTrackLevel = default(EntityRef< AttendTrackLevel>); 
			
			this._Campu = default(EntityRef< Campu>); 
			
			this._Division = default(EntityRef< Division>); 
			
			this._Gender = default(EntityRef< Gender>); 
			
			this._RegistrationType = default(EntityRef< RegistrationType>); 
			
			this._EntryPoint = default(EntityRef< EntryPoint>); 
			
			this._OrganizationStatus = default(EntityRef< OrganizationStatus>); 
			
			OnCreated();
		}

		
    #region Columns
		
		[Column(Name="OrganizationId", UpdateCheck=UpdateCheck.Never, Storage="_OrganizationId", AutoSync=AutoSync.OnInsert, DbType="int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int OrganizationId
		{
			get { return this._OrganizationId; }

			set
			{
				if (this._OrganizationId != value)
				{
				
                    this.OnOrganizationIdChanging(value);
					this.SendPropertyChanging();
					this._OrganizationId = value;
					this.SendPropertyChanged("OrganizationId");
					this.OnOrganizationIdChanged();
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

		
		[Column(Name="CreatedDate", UpdateCheck=UpdateCheck.Never, Storage="_CreatedDate", DbType="datetime NOT NULL")]
		public DateTime CreatedDate
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

		
		[Column(Name="OrganizationStatusId", UpdateCheck=UpdateCheck.Never, Storage="_OrganizationStatusId", DbType="int NOT NULL")]
		public int OrganizationStatusId
		{
			get { return this._OrganizationStatusId; }

			set
			{
				if (this._OrganizationStatusId != value)
				{
				
					if (this._OrganizationStatus.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnOrganizationStatusIdChanging(value);
					this.SendPropertyChanging();
					this._OrganizationStatusId = value;
					this.SendPropertyChanged("OrganizationStatusId");
					this.OnOrganizationStatusIdChanged();
				}

			}

		}

		
		[Column(Name="DivisionId", UpdateCheck=UpdateCheck.Never, Storage="_DivisionId", DbType="int")]
		public int? DivisionId
		{
			get { return this._DivisionId; }

			set
			{
				if (this._DivisionId != value)
				{
				
					if (this._Division.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnDivisionIdChanging(value);
					this.SendPropertyChanging();
					this._DivisionId = value;
					this.SendPropertyChanged("DivisionId");
					this.OnDivisionIdChanged();
				}

			}

		}

		
		[Column(Name="LeaderMemberTypeId", UpdateCheck=UpdateCheck.Never, Storage="_LeaderMemberTypeId", DbType="int")]
		public int? LeaderMemberTypeId
		{
			get { return this._LeaderMemberTypeId; }

			set
			{
				if (this._LeaderMemberTypeId != value)
				{
				
                    this.OnLeaderMemberTypeIdChanging(value);
					this.SendPropertyChanging();
					this._LeaderMemberTypeId = value;
					this.SendPropertyChanged("LeaderMemberTypeId");
					this.OnLeaderMemberTypeIdChanged();
				}

			}

		}

		
		[Column(Name="GradeAgeStart", UpdateCheck=UpdateCheck.Never, Storage="_GradeAgeStart", DbType="int")]
		public int? GradeAgeStart
		{
			get { return this._GradeAgeStart; }

			set
			{
				if (this._GradeAgeStart != value)
				{
				
                    this.OnGradeAgeStartChanging(value);
					this.SendPropertyChanging();
					this._GradeAgeStart = value;
					this.SendPropertyChanged("GradeAgeStart");
					this.OnGradeAgeStartChanged();
				}

			}

		}

		
		[Column(Name="GradeAgeEnd", UpdateCheck=UpdateCheck.Never, Storage="_GradeAgeEnd", DbType="int")]
		public int? GradeAgeEnd
		{
			get { return this._GradeAgeEnd; }

			set
			{
				if (this._GradeAgeEnd != value)
				{
				
                    this.OnGradeAgeEndChanging(value);
					this.SendPropertyChanging();
					this._GradeAgeEnd = value;
					this.SendPropertyChanged("GradeAgeEnd");
					this.OnGradeAgeEndChanged();
				}

			}

		}

		
		[Column(Name="RollSheetVisitorWks", UpdateCheck=UpdateCheck.Never, Storage="_RollSheetVisitorWks", DbType="int")]
		public int? RollSheetVisitorWks
		{
			get { return this._RollSheetVisitorWks; }

			set
			{
				if (this._RollSheetVisitorWks != value)
				{
				
                    this.OnRollSheetVisitorWksChanging(value);
					this.SendPropertyChanging();
					this._RollSheetVisitorWks = value;
					this.SendPropertyChanged("RollSheetVisitorWks");
					this.OnRollSheetVisitorWksChanged();
				}

			}

		}

		
		[Column(Name="AttendTrkLevelId", UpdateCheck=UpdateCheck.Never, Storage="_AttendTrkLevelId", DbType="int NOT NULL")]
		public int AttendTrkLevelId
		{
			get { return this._AttendTrkLevelId; }

			set
			{
				if (this._AttendTrkLevelId != value)
				{
				
					if (this._AttendTrackLevel.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnAttendTrkLevelIdChanging(value);
					this.SendPropertyChanging();
					this._AttendTrkLevelId = value;
					this.SendPropertyChanged("AttendTrkLevelId");
					this.OnAttendTrkLevelIdChanged();
				}

			}

		}

		
		[Column(Name="SecurityTypeId", UpdateCheck=UpdateCheck.Never, Storage="_SecurityTypeId", DbType="int NOT NULL")]
		public int SecurityTypeId
		{
			get { return this._SecurityTypeId; }

			set
			{
				if (this._SecurityTypeId != value)
				{
				
                    this.OnSecurityTypeIdChanging(value);
					this.SendPropertyChanging();
					this._SecurityTypeId = value;
					this.SendPropertyChanged("SecurityTypeId");
					this.OnSecurityTypeIdChanged();
				}

			}

		}

		
		[Column(Name="AttendClassificationId", UpdateCheck=UpdateCheck.Never, Storage="_AttendClassificationId", DbType="int NOT NULL")]
		public int AttendClassificationId
		{
			get { return this._AttendClassificationId; }

			set
			{
				if (this._AttendClassificationId != value)
				{
				
                    this.OnAttendClassificationIdChanging(value);
					this.SendPropertyChanging();
					this._AttendClassificationId = value;
					this.SendPropertyChanged("AttendClassificationId");
					this.OnAttendClassificationIdChanged();
				}

			}

		}

		
		[Column(Name="FirstMeetingDate", UpdateCheck=UpdateCheck.Never, Storage="_FirstMeetingDate", DbType="datetime")]
		public DateTime? FirstMeetingDate
		{
			get { return this._FirstMeetingDate; }

			set
			{
				if (this._FirstMeetingDate != value)
				{
				
                    this.OnFirstMeetingDateChanging(value);
					this.SendPropertyChanging();
					this._FirstMeetingDate = value;
					this.SendPropertyChanged("FirstMeetingDate");
					this.OnFirstMeetingDateChanged();
				}

			}

		}

		
		[Column(Name="LastMeetingDate", UpdateCheck=UpdateCheck.Never, Storage="_LastMeetingDate", DbType="datetime")]
		public DateTime? LastMeetingDate
		{
			get { return this._LastMeetingDate; }

			set
			{
				if (this._LastMeetingDate != value)
				{
				
                    this.OnLastMeetingDateChanging(value);
					this.SendPropertyChanging();
					this._LastMeetingDate = value;
					this.SendPropertyChanged("LastMeetingDate");
					this.OnLastMeetingDateChanged();
				}

			}

		}

		
		[Column(Name="OrganizationClosedDate", UpdateCheck=UpdateCheck.Never, Storage="_OrganizationClosedDate", DbType="datetime")]
		public DateTime? OrganizationClosedDate
		{
			get { return this._OrganizationClosedDate; }

			set
			{
				if (this._OrganizationClosedDate != value)
				{
				
                    this.OnOrganizationClosedDateChanging(value);
					this.SendPropertyChanging();
					this._OrganizationClosedDate = value;
					this.SendPropertyChanged("OrganizationClosedDate");
					this.OnOrganizationClosedDateChanged();
				}

			}

		}

		
		[Column(Name="Location", UpdateCheck=UpdateCheck.Never, Storage="_Location", DbType="varchar(40)")]
		public string Location
		{
			get { return this._Location; }

			set
			{
				if (this._Location != value)
				{
				
                    this.OnLocationChanging(value);
					this.SendPropertyChanging();
					this._Location = value;
					this.SendPropertyChanged("Location");
					this.OnLocationChanged();
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

		
		[Column(Name="ScheduleId", UpdateCheck=UpdateCheck.Never, Storage="_ScheduleId", DbType="int")]
		public int? ScheduleId
		{
			get { return this._ScheduleId; }

			set
			{
				if (this._ScheduleId != value)
				{
				
                    this.OnScheduleIdChanging(value);
					this.SendPropertyChanging();
					this._ScheduleId = value;
					this.SendPropertyChanged("ScheduleId");
					this.OnScheduleIdChanged();
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
				
					if (this._EntryPoint.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnEntryPointIdChanging(value);
					this.SendPropertyChanging();
					this._EntryPointId = value;
					this.SendPropertyChanged("EntryPointId");
					this.OnEntryPointIdChanged();
				}

			}

		}

		
		[Column(Name="ParentOrgId", UpdateCheck=UpdateCheck.Never, Storage="_ParentOrgId", DbType="int")]
		public int? ParentOrgId
		{
			get { return this._ParentOrgId; }

			set
			{
				if (this._ParentOrgId != value)
				{
				
					if (this._ParentOrg.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnParentOrgIdChanging(value);
					this.SendPropertyChanging();
					this._ParentOrgId = value;
					this.SendPropertyChanged("ParentOrgId");
					this.OnParentOrgIdChanged();
				}

			}

		}

		
		[Column(Name="AllowAttendOverlap", UpdateCheck=UpdateCheck.Never, Storage="_AllowAttendOverlap", DbType="bit NOT NULL")]
		public bool AllowAttendOverlap
		{
			get { return this._AllowAttendOverlap; }

			set
			{
				if (this._AllowAttendOverlap != value)
				{
				
                    this.OnAllowAttendOverlapChanging(value);
					this.SendPropertyChanging();
					this._AllowAttendOverlap = value;
					this.SendPropertyChanged("AllowAttendOverlap");
					this.OnAllowAttendOverlapChanged();
				}

			}

		}

		
		[Column(Name="MemberCount", UpdateCheck=UpdateCheck.Never, Storage="_MemberCount", DbType="int")]
		public int? MemberCount
		{
			get { return this._MemberCount; }

			set
			{
				if (this._MemberCount != value)
				{
				
                    this.OnMemberCountChanging(value);
					this.SendPropertyChanging();
					this._MemberCount = value;
					this.SendPropertyChanged("MemberCount");
					this.OnMemberCountChanged();
				}

			}

		}

		
		[Column(Name="LeaderId", UpdateCheck=UpdateCheck.Never, Storage="_LeaderId", DbType="int")]
		public int? LeaderId
		{
			get { return this._LeaderId; }

			set
			{
				if (this._LeaderId != value)
				{
				
                    this.OnLeaderIdChanging(value);
					this.SendPropertyChanging();
					this._LeaderId = value;
					this.SendPropertyChanged("LeaderId");
					this.OnLeaderIdChanged();
				}

			}

		}

		
		[Column(Name="LeaderName", UpdateCheck=UpdateCheck.Never, Storage="_LeaderName", DbType="varchar(50)")]
		public string LeaderName
		{
			get { return this._LeaderName; }

			set
			{
				if (this._LeaderName != value)
				{
				
                    this.OnLeaderNameChanging(value);
					this.SendPropertyChanging();
					this._LeaderName = value;
					this.SendPropertyChanged("LeaderName");
					this.OnLeaderNameChanged();
				}

			}

		}

		
		[Column(Name="ClassFilled", UpdateCheck=UpdateCheck.Never, Storage="_ClassFilled", DbType="bit")]
		public bool? ClassFilled
		{
			get { return this._ClassFilled; }

			set
			{
				if (this._ClassFilled != value)
				{
				
                    this.OnClassFilledChanging(value);
					this.SendPropertyChanging();
					this._ClassFilled = value;
					this.SendPropertyChanged("ClassFilled");
					this.OnClassFilledChanged();
				}

			}

		}

		
		[Column(Name="OnLineCatalogSort", UpdateCheck=UpdateCheck.Never, Storage="_OnLineCatalogSort", DbType="int")]
		public int? OnLineCatalogSort
		{
			get { return this._OnLineCatalogSort; }

			set
			{
				if (this._OnLineCatalogSort != value)
				{
				
                    this.OnOnLineCatalogSortChanging(value);
					this.SendPropertyChanging();
					this._OnLineCatalogSort = value;
					this.SendPropertyChanged("OnLineCatalogSort");
					this.OnOnLineCatalogSortChanged();
				}

			}

		}

		
		[Column(Name="PendingLoc", UpdateCheck=UpdateCheck.Never, Storage="_PendingLoc", DbType="varchar(40)")]
		public string PendingLoc
		{
			get { return this._PendingLoc; }

			set
			{
				if (this._PendingLoc != value)
				{
				
                    this.OnPendingLocChanging(value);
					this.SendPropertyChanging();
					this._PendingLoc = value;
					this.SendPropertyChanged("PendingLoc");
					this.OnPendingLocChanged();
				}

			}

		}

		
		[Column(Name="CanSelfCheckin", UpdateCheck=UpdateCheck.Never, Storage="_CanSelfCheckin", DbType="bit")]
		public bool? CanSelfCheckin
		{
			get { return this._CanSelfCheckin; }

			set
			{
				if (this._CanSelfCheckin != value)
				{
				
                    this.OnCanSelfCheckinChanging(value);
					this.SendPropertyChanging();
					this._CanSelfCheckin = value;
					this.SendPropertyChanged("CanSelfCheckin");
					this.OnCanSelfCheckinChanged();
				}

			}

		}

		
		[Column(Name="NumCheckInLabels", UpdateCheck=UpdateCheck.Never, Storage="_NumCheckInLabels", DbType="int")]
		public int? NumCheckInLabels
		{
			get { return this._NumCheckInLabels; }

			set
			{
				if (this._NumCheckInLabels != value)
				{
				
                    this.OnNumCheckInLabelsChanging(value);
					this.SendPropertyChanging();
					this._NumCheckInLabels = value;
					this.SendPropertyChanged("NumCheckInLabels");
					this.OnNumCheckInLabelsChanged();
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

		
		[Column(Name="AllowNonCampusCheckIn", UpdateCheck=UpdateCheck.Never, Storage="_AllowNonCampusCheckIn", DbType="bit")]
		public bool? AllowNonCampusCheckIn
		{
			get { return this._AllowNonCampusCheckIn; }

			set
			{
				if (this._AllowNonCampusCheckIn != value)
				{
				
                    this.OnAllowNonCampusCheckInChanging(value);
					this.SendPropertyChanging();
					this._AllowNonCampusCheckIn = value;
					this.SendPropertyChanged("AllowNonCampusCheckIn");
					this.OnAllowNonCampusCheckInChanged();
				}

			}

		}

		
		[Column(Name="NumWorkerCheckInLabels", UpdateCheck=UpdateCheck.Never, Storage="_NumWorkerCheckInLabels", DbType="int")]
		public int? NumWorkerCheckInLabels
		{
			get { return this._NumWorkerCheckInLabels; }

			set
			{
				if (this._NumWorkerCheckInLabels != value)
				{
				
                    this.OnNumWorkerCheckInLabelsChanging(value);
					this.SendPropertyChanging();
					this._NumWorkerCheckInLabels = value;
					this.SendPropertyChanged("NumWorkerCheckInLabels");
					this.OnNumWorkerCheckInLabelsChanged();
				}

			}

		}

		
		[Column(Name="SchedTime", UpdateCheck=UpdateCheck.Never, Storage="_SchedTime", DbType="datetime")]
		public DateTime? SchedTime
		{
			get { return this._SchedTime; }

			set
			{
				if (this._SchedTime != value)
				{
				
                    this.OnSchedTimeChanging(value);
					this.SendPropertyChanging();
					this._SchedTime = value;
					this.SendPropertyChanged("SchedTime");
					this.OnSchedTimeChanged();
				}

			}

		}

		
		[Column(Name="SchedDay", UpdateCheck=UpdateCheck.Never, Storage="_SchedDay", DbType="int")]
		public int? SchedDay
		{
			get { return this._SchedDay; }

			set
			{
				if (this._SchedDay != value)
				{
				
                    this.OnSchedDayChanging(value);
					this.SendPropertyChanging();
					this._SchedDay = value;
					this.SendPropertyChanged("SchedDay");
					this.OnSchedDayChanged();
				}

			}

		}

		
		[Column(Name="MeetingTime", UpdateCheck=UpdateCheck.Never, Storage="_MeetingTime", DbType="datetime")]
		public DateTime? MeetingTime
		{
			get { return this._MeetingTime; }

			set
			{
				if (this._MeetingTime != value)
				{
				
                    this.OnMeetingTimeChanging(value);
					this.SendPropertyChanging();
					this._MeetingTime = value;
					this.SendPropertyChanged("MeetingTime");
					this.OnMeetingTimeChanged();
				}

			}

		}

		
		[Column(Name="ShowOnlyRegisteredAtCheckIn", UpdateCheck=UpdateCheck.Never, Storage="_ShowOnlyRegisteredAtCheckIn", DbType="bit")]
		public bool? ShowOnlyRegisteredAtCheckIn
		{
			get { return this._ShowOnlyRegisteredAtCheckIn; }

			set
			{
				if (this._ShowOnlyRegisteredAtCheckIn != value)
				{
				
                    this.OnShowOnlyRegisteredAtCheckInChanging(value);
					this.SendPropertyChanging();
					this._ShowOnlyRegisteredAtCheckIn = value;
					this.SendPropertyChanged("ShowOnlyRegisteredAtCheckIn");
					this.OnShowOnlyRegisteredAtCheckInChanged();
				}

			}

		}

		
		[Column(Name="Limit", UpdateCheck=UpdateCheck.Never, Storage="_Limit", DbType="int")]
		public int? Limit
		{
			get { return this._Limit; }

			set
			{
				if (this._Limit != value)
				{
				
                    this.OnLimitChanging(value);
					this.SendPropertyChanging();
					this._Limit = value;
					this.SendPropertyChanged("Limit");
					this.OnLimitChanged();
				}

			}

		}

		
		[Column(Name="EmailAddresses", UpdateCheck=UpdateCheck.Never, Storage="_EmailAddresses", DbType="varchar(200)")]
		public string EmailAddresses
		{
			get { return this._EmailAddresses; }

			set
			{
				if (this._EmailAddresses != value)
				{
				
                    this.OnEmailAddressesChanging(value);
					this.SendPropertyChanging();
					this._EmailAddresses = value;
					this.SendPropertyChanged("EmailAddresses");
					this.OnEmailAddressesChanged();
				}

			}

		}

		
		[Column(Name="RegType", UpdateCheck=UpdateCheck.Never, Storage="_RegType", DbType="varchar(50)")]
		public string RegType
		{
			get { return this._RegType; }

			set
			{
				if (this._RegType != value)
				{
				
                    this.OnRegTypeChanging(value);
					this.SendPropertyChanging();
					this._RegType = value;
					this.SendPropertyChanged("RegType");
					this.OnRegTypeChanged();
				}

			}

		}

		
		[Column(Name="EmailMessage", UpdateCheck=UpdateCheck.Never, Storage="_EmailMessage", DbType="varchar")]
		public string EmailMessage
		{
			get { return this._EmailMessage; }

			set
			{
				if (this._EmailMessage != value)
				{
				
                    this.OnEmailMessageChanging(value);
					this.SendPropertyChanging();
					this._EmailMessage = value;
					this.SendPropertyChanged("EmailMessage");
					this.OnEmailMessageChanged();
				}

			}

		}

		
		[Column(Name="EmailSubject", UpdateCheck=UpdateCheck.Never, Storage="_EmailSubject", DbType="varchar(100)")]
		public string EmailSubject
		{
			get { return this._EmailSubject; }

			set
			{
				if (this._EmailSubject != value)
				{
				
                    this.OnEmailSubjectChanging(value);
					this.SendPropertyChanging();
					this._EmailSubject = value;
					this.SendPropertyChanged("EmailSubject");
					this.OnEmailSubjectChanged();
				}

			}

		}

		
		[Column(Name="Instructions", UpdateCheck=UpdateCheck.Never, Storage="_Instructions", DbType="varchar")]
		public string Instructions
		{
			get { return this._Instructions; }

			set
			{
				if (this._Instructions != value)
				{
				
                    this.OnInstructionsChanging(value);
					this.SendPropertyChanging();
					this._Instructions = value;
					this.SendPropertyChanged("Instructions");
					this.OnInstructionsChanged();
				}

			}

		}

		
		[Column(Name="GenderId", UpdateCheck=UpdateCheck.Never, Storage="_GenderId", DbType="int")]
		public int? GenderId
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

		
		[Column(Name="Fee", UpdateCheck=UpdateCheck.Never, Storage="_Fee", DbType="money")]
		public decimal? Fee
		{
			get { return this._Fee; }

			set
			{
				if (this._Fee != value)
				{
				
                    this.OnFeeChanging(value);
					this.SendPropertyChanging();
					this._Fee = value;
					this.SendPropertyChanged("Fee");
					this.OnFeeChanged();
				}

			}

		}

		
		[Column(Name="Description", UpdateCheck=UpdateCheck.Never, Storage="_Description", DbType="varchar")]
		public string Description
		{
			get { return this._Description; }

			set
			{
				if (this._Description != value)
				{
				
                    this.OnDescriptionChanging(value);
					this.SendPropertyChanging();
					this._Description = value;
					this.SendPropertyChanged("Description");
					this.OnDescriptionChanged();
				}

			}

		}

		
		[Column(Name="BirthDayStart", UpdateCheck=UpdateCheck.Never, Storage="_BirthDayStart", DbType="datetime")]
		public DateTime? BirthDayStart
		{
			get { return this._BirthDayStart; }

			set
			{
				if (this._BirthDayStart != value)
				{
				
                    this.OnBirthDayStartChanging(value);
					this.SendPropertyChanging();
					this._BirthDayStart = value;
					this.SendPropertyChanged("BirthDayStart");
					this.OnBirthDayStartChanged();
				}

			}

		}

		
		[Column(Name="BirthDayEnd", UpdateCheck=UpdateCheck.Never, Storage="_BirthDayEnd", DbType="datetime")]
		public DateTime? BirthDayEnd
		{
			get { return this._BirthDayEnd; }

			set
			{
				if (this._BirthDayEnd != value)
				{
				
                    this.OnBirthDayEndChanging(value);
					this.SendPropertyChanging();
					this._BirthDayEnd = value;
					this.SendPropertyChanged("BirthDayEnd");
					this.OnBirthDayEndChanged();
				}

			}

		}

		
		[Column(Name="VisitorDate", UpdateCheck=UpdateCheck.Never, Storage="_VisitorDate", DbType="datetime", IsDbGenerated=true)]
		public DateTime? VisitorDate
		{
			get { return this._VisitorDate; }

			set
			{
				if (this._VisitorDate != value)
				{
				
                    this.OnVisitorDateChanging(value);
					this.SendPropertyChanging();
					this._VisitorDate = value;
					this.SendPropertyChanged("VisitorDate");
					this.OnVisitorDateChanged();
				}

			}

		}

		
		[Column(Name="Deposit", UpdateCheck=UpdateCheck.Never, Storage="_Deposit", DbType="money")]
		public decimal? Deposit
		{
			get { return this._Deposit; }

			set
			{
				if (this._Deposit != value)
				{
				
                    this.OnDepositChanging(value);
					this.SendPropertyChanging();
					this._Deposit = value;
					this.SendPropertyChanged("Deposit");
					this.OnDepositChanged();
				}

			}

		}

		
		[Column(Name="ShirtFee", UpdateCheck=UpdateCheck.Never, Storage="_ShirtFee", DbType="money")]
		public decimal? ShirtFee
		{
			get { return this._ShirtFee; }

			set
			{
				if (this._ShirtFee != value)
				{
				
                    this.OnShirtFeeChanging(value);
					this.SendPropertyChanging();
					this._ShirtFee = value;
					this.SendPropertyChanged("ShirtFee");
					this.OnShirtFeeChanged();
				}

			}

		}

		
		[Column(Name="ExtraFee", UpdateCheck=UpdateCheck.Never, Storage="_ExtraFee", DbType="money")]
		public decimal? ExtraFee
		{
			get { return this._ExtraFee; }

			set
			{
				if (this._ExtraFee != value)
				{
				
                    this.OnExtraFeeChanging(value);
					this.SendPropertyChanging();
					this._ExtraFee = value;
					this.SendPropertyChanged("ExtraFee");
					this.OnExtraFeeChanged();
				}

			}

		}

		
		[Column(Name="LastDayBeforeExtra", UpdateCheck=UpdateCheck.Never, Storage="_LastDayBeforeExtra", DbType="datetime")]
		public DateTime? LastDayBeforeExtra
		{
			get { return this._LastDayBeforeExtra; }

			set
			{
				if (this._LastDayBeforeExtra != value)
				{
				
                    this.OnLastDayBeforeExtraChanging(value);
					this.SendPropertyChanging();
					this._LastDayBeforeExtra = value;
					this.SendPropertyChanged("LastDayBeforeExtra");
					this.OnLastDayBeforeExtraChanged();
				}

			}

		}

		
		[Column(Name="AskTylenolEtc", UpdateCheck=UpdateCheck.Never, Storage="_AskTylenolEtc", DbType="bit")]
		public bool? AskTylenolEtc
		{
			get { return this._AskTylenolEtc; }

			set
			{
				if (this._AskTylenolEtc != value)
				{
				
                    this.OnAskTylenolEtcChanging(value);
					this.SendPropertyChanging();
					this._AskTylenolEtc = value;
					this.SendPropertyChanged("AskTylenolEtc");
					this.OnAskTylenolEtcChanged();
				}

			}

		}

		
		[Column(Name="AskAllergies", UpdateCheck=UpdateCheck.Never, Storage="_AskAllergies", DbType="bit")]
		public bool? AskAllergies
		{
			get { return this._AskAllergies; }

			set
			{
				if (this._AskAllergies != value)
				{
				
                    this.OnAskAllergiesChanging(value);
					this.SendPropertyChanging();
					this._AskAllergies = value;
					this.SendPropertyChanged("AskAllergies");
					this.OnAskAllergiesChanged();
				}

			}

		}

		
		[Column(Name="AskShirtSize", UpdateCheck=UpdateCheck.Never, Storage="_AskShirtSize", DbType="bit")]
		public bool? AskShirtSize
		{
			get { return this._AskShirtSize; }

			set
			{
				if (this._AskShirtSize != value)
				{
				
                    this.OnAskShirtSizeChanging(value);
					this.SendPropertyChanging();
					this._AskShirtSize = value;
					this.SendPropertyChanged("AskShirtSize");
					this.OnAskShirtSizeChanged();
				}

			}

		}

		
		[Column(Name="AskRequest", UpdateCheck=UpdateCheck.Never, Storage="_AskRequest", DbType="bit")]
		public bool? AskRequest
		{
			get { return this._AskRequest; }

			set
			{
				if (this._AskRequest != value)
				{
				
                    this.OnAskRequestChanging(value);
					this.SendPropertyChanging();
					this._AskRequest = value;
					this.SendPropertyChanged("AskRequest");
					this.OnAskRequestChanged();
				}

			}

		}

		
		[Column(Name="AskParents", UpdateCheck=UpdateCheck.Never, Storage="_AskParents", DbType="bit")]
		public bool? AskParents
		{
			get { return this._AskParents; }

			set
			{
				if (this._AskParents != value)
				{
				
                    this.OnAskParentsChanging(value);
					this.SendPropertyChanging();
					this._AskParents = value;
					this.SendPropertyChanged("AskParents");
					this.OnAskParentsChanged();
				}

			}

		}

		
		[Column(Name="AskEmContact", UpdateCheck=UpdateCheck.Never, Storage="_AskEmContact", DbType="bit")]
		public bool? AskEmContact
		{
			get { return this._AskEmContact; }

			set
			{
				if (this._AskEmContact != value)
				{
				
                    this.OnAskEmContactChanging(value);
					this.SendPropertyChanging();
					this._AskEmContact = value;
					this.SendPropertyChanged("AskEmContact");
					this.OnAskEmContactChanged();
				}

			}

		}

		
		[Column(Name="AskMedical", UpdateCheck=UpdateCheck.Never, Storage="_AskMedical", DbType="bit")]
		public bool? AskMedical
		{
			get { return this._AskMedical; }

			set
			{
				if (this._AskMedical != value)
				{
				
                    this.OnAskMedicalChanging(value);
					this.SendPropertyChanging();
					this._AskMedical = value;
					this.SendPropertyChanged("AskMedical");
					this.OnAskMedicalChanged();
				}

			}

		}

		
		[Column(Name="AskInsurance", UpdateCheck=UpdateCheck.Never, Storage="_AskInsurance", DbType="bit")]
		public bool? AskInsurance
		{
			get { return this._AskInsurance; }

			set
			{
				if (this._AskInsurance != value)
				{
				
                    this.OnAskInsuranceChanging(value);
					this.SendPropertyChanging();
					this._AskInsurance = value;
					this.SendPropertyChanged("AskInsurance");
					this.OnAskInsuranceChanged();
				}

			}

		}

		
		[Column(Name="AllowLastYearShirt", UpdateCheck=UpdateCheck.Never, Storage="_AllowLastYearShirt", DbType="bit")]
		public bool? AllowLastYearShirt
		{
			get { return this._AllowLastYearShirt; }

			set
			{
				if (this._AllowLastYearShirt != value)
				{
				
                    this.OnAllowLastYearShirtChanging(value);
					this.SendPropertyChanging();
					this._AllowLastYearShirt = value;
					this.SendPropertyChanged("AllowLastYearShirt");
					this.OnAllowLastYearShirtChanged();
				}

			}

		}

		
		[Column(Name="AskDoctor", UpdateCheck=UpdateCheck.Never, Storage="_AskDoctor", DbType="bit")]
		public bool? AskDoctor
		{
			get { return this._AskDoctor; }

			set
			{
				if (this._AskDoctor != value)
				{
				
                    this.OnAskDoctorChanging(value);
					this.SendPropertyChanging();
					this._AskDoctor = value;
					this.SendPropertyChanged("AskDoctor");
					this.OnAskDoctorChanged();
				}

			}

		}

		
		[Column(Name="AskCoaching", UpdateCheck=UpdateCheck.Never, Storage="_AskCoaching", DbType="bit")]
		public bool? AskCoaching
		{
			get { return this._AskCoaching; }

			set
			{
				if (this._AskCoaching != value)
				{
				
                    this.OnAskCoachingChanging(value);
					this.SendPropertyChanging();
					this._AskCoaching = value;
					this.SendPropertyChanged("AskCoaching");
					this.OnAskCoachingChanged();
				}

			}

		}

		
		[Column(Name="AskChurch", UpdateCheck=UpdateCheck.Never, Storage="_AskChurch", DbType="bit")]
		public bool? AskChurch
		{
			get { return this._AskChurch; }

			set
			{
				if (this._AskChurch != value)
				{
				
                    this.OnAskChurchChanging(value);
					this.SendPropertyChanging();
					this._AskChurch = value;
					this.SendPropertyChanged("AskChurch");
					this.OnAskChurchChanged();
				}

			}

		}

		
		[Column(Name="AskGrade", UpdateCheck=UpdateCheck.Never, Storage="_AskGrade", DbType="bit")]
		public bool? AskGrade
		{
			get { return this._AskGrade; }

			set
			{
				if (this._AskGrade != value)
				{
				
                    this.OnAskGradeChanging(value);
					this.SendPropertyChanging();
					this._AskGrade = value;
					this.SendPropertyChanged("AskGrade");
					this.OnAskGradeChanged();
				}

			}

		}

		
		[Column(Name="Terms", UpdateCheck=UpdateCheck.Never, Storage="_Terms", DbType="varchar")]
		public string Terms
		{
			get { return this._Terms; }

			set
			{
				if (this._Terms != value)
				{
				
                    this.OnTermsChanging(value);
					this.SendPropertyChanging();
					this._Terms = value;
					this.SendPropertyChanged("Terms");
					this.OnTermsChanged();
				}

			}

		}

		
		[Column(Name="AskTickets", UpdateCheck=UpdateCheck.Never, Storage="_AskTickets", DbType="bit")]
		public bool? AskTickets
		{
			get { return this._AskTickets; }

			set
			{
				if (this._AskTickets != value)
				{
				
                    this.OnAskTicketsChanging(value);
					this.SendPropertyChanging();
					this._AskTickets = value;
					this.SendPropertyChanged("AskTickets");
					this.OnAskTicketsChanged();
				}

			}

		}

		
		[Column(Name="MaximumFee", UpdateCheck=UpdateCheck.Never, Storage="_MaximumFee", DbType="money")]
		public decimal? MaximumFee
		{
			get { return this._MaximumFee; }

			set
			{
				if (this._MaximumFee != value)
				{
				
                    this.OnMaximumFeeChanging(value);
					this.SendPropertyChanging();
					this._MaximumFee = value;
					this.SendPropertyChanged("MaximumFee");
					this.OnMaximumFeeChanged();
				}

			}

		}

		
		[Column(Name="AskOptions", UpdateCheck=UpdateCheck.Never, Storage="_AskOptions", DbType="varchar(300)")]
		public string AskOptions
		{
			get { return this._AskOptions; }

			set
			{
				if (this._AskOptions != value)
				{
				
                    this.OnAskOptionsChanging(value);
					this.SendPropertyChanging();
					this._AskOptions = value;
					this.SendPropertyChanged("AskOptions");
					this.OnAskOptionsChanged();
				}

			}

		}

		
		[Column(Name="AgeFee", UpdateCheck=UpdateCheck.Never, Storage="_AgeFee", DbType="varchar(60)")]
		public string AgeFee
		{
			get { return this._AgeFee; }

			set
			{
				if (this._AgeFee != value)
				{
				
                    this.OnAgeFeeChanging(value);
					this.SendPropertyChanging();
					this._AgeFee = value;
					this.SendPropertyChanged("AgeFee");
					this.OnAgeFeeChanged();
				}

			}

		}

		
		[Column(Name="AllowOnlyOne", UpdateCheck=UpdateCheck.Never, Storage="_AllowOnlyOne", DbType="bit")]
		public bool? AllowOnlyOne
		{
			get { return this._AllowOnlyOne; }

			set
			{
				if (this._AllowOnlyOne != value)
				{
				
                    this.OnAllowOnlyOneChanging(value);
					this.SendPropertyChanging();
					this._AllowOnlyOne = value;
					this.SendPropertyChanged("AllowOnlyOne");
					this.OnAllowOnlyOneChanged();
				}

			}

		}

		
		[Column(Name="RegistrationTypeId", UpdateCheck=UpdateCheck.Never, Storage="_RegistrationTypeId", DbType="int")]
		public int? RegistrationTypeId
		{
			get { return this._RegistrationTypeId; }

			set
			{
				if (this._RegistrationTypeId != value)
				{
				
					if (this._RegistrationType.HasLoadedOrAssignedValue)
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
				
                    this.OnRegistrationTypeIdChanging(value);
					this.SendPropertyChanging();
					this._RegistrationTypeId = value;
					this.SendPropertyChanged("RegistrationTypeId");
					this.OnRegistrationTypeIdChanged();
				}

			}

		}

		
		[Column(Name="AgeGroups", UpdateCheck=UpdateCheck.Never, Storage="_AgeGroups", DbType="varchar(100)")]
		public string AgeGroups
		{
			get { return this._AgeGroups; }

			set
			{
				if (this._AgeGroups != value)
				{
				
                    this.OnAgeGroupsChanging(value);
					this.SendPropertyChanging();
					this._AgeGroups = value;
					this.SendPropertyChanged("AgeGroups");
					this.OnAgeGroupsChanged();
				}

			}

		}

		
		[Column(Name="ValidateOrgs", UpdateCheck=UpdateCheck.Never, Storage="_ValidateOrgs", DbType="varchar(60)")]
		public string ValidateOrgs
		{
			get { return this._ValidateOrgs; }

			set
			{
				if (this._ValidateOrgs != value)
				{
				
                    this.OnValidateOrgsChanging(value);
					this.SendPropertyChanging();
					this._ValidateOrgs = value;
					this.SendPropertyChanged("ValidateOrgs");
					this.OnValidateOrgsChanged();
				}

			}

		}

		
		[Column(Name="MemberOnly", UpdateCheck=UpdateCheck.Never, Storage="_MemberOnly", DbType="bit")]
		public bool? MemberOnly
		{
			get { return this._MemberOnly; }

			set
			{
				if (this._MemberOnly != value)
				{
				
                    this.OnMemberOnlyChanging(value);
					this.SendPropertyChanging();
					this._MemberOnly = value;
					this.SendPropertyChanged("MemberOnly");
					this.OnMemberOnlyChanged();
				}

			}

		}

		
		[Column(Name="YesNoQuestions", UpdateCheck=UpdateCheck.Never, Storage="_YesNoQuestions", DbType="varchar(500)")]
		public string YesNoQuestions
		{
			get { return this._YesNoQuestions; }

			set
			{
				if (this._YesNoQuestions != value)
				{
				
                    this.OnYesNoQuestionsChanging(value);
					this.SendPropertyChanging();
					this._YesNoQuestions = value;
					this.SendPropertyChanged("YesNoQuestions");
					this.OnYesNoQuestionsChanged();
				}

			}

		}

		
		[Column(Name="OrgMemberFees", UpdateCheck=UpdateCheck.Never, Storage="_OrgMemberFees", DbType="varchar(30)")]
		public string OrgMemberFees
		{
			get { return this._OrgMemberFees; }

			set
			{
				if (this._OrgMemberFees != value)
				{
				
                    this.OnOrgMemberFeesChanging(value);
					this.SendPropertyChanging();
					this._OrgMemberFees = value;
					this.SendPropertyChanged("OrgMemberFees");
					this.OnOrgMemberFeesChanged();
				}

			}

		}

		
		[Column(Name="ExtraQuestions", UpdateCheck=UpdateCheck.Never, Storage="_ExtraQuestions", DbType="varchar(500)")]
		public string ExtraQuestions
		{
			get { return this._ExtraQuestions; }

			set
			{
				if (this._ExtraQuestions != value)
				{
				
                    this.OnExtraQuestionsChanging(value);
					this.SendPropertyChanging();
					this._ExtraQuestions = value;
					this.SendPropertyChanged("ExtraQuestions");
					this.OnExtraQuestionsChanged();
				}

			}

		}

		
		[Column(Name="PhoneNumber", UpdateCheck=UpdateCheck.Never, Storage="_PhoneNumber", DbType="varchar(25)")]
		public string PhoneNumber
		{
			get { return this._PhoneNumber; }

			set
			{
				if (this._PhoneNumber != value)
				{
				
                    this.OnPhoneNumberChanging(value);
					this.SendPropertyChanging();
					this._PhoneNumber = value;
					this.SendPropertyChanged("PhoneNumber");
					this.OnPhoneNumberChanged();
				}

			}

		}

		
		[Column(Name="GradeOptions", UpdateCheck=UpdateCheck.Never, Storage="_GradeOptions", DbType="varchar(300)")]
		public string GradeOptions
		{
			get { return this._GradeOptions; }

			set
			{
				if (this._GradeOptions != value)
				{
				
                    this.OnGradeOptionsChanging(value);
					this.SendPropertyChanging();
					this._GradeOptions = value;
					this.SendPropertyChanged("GradeOptions");
					this.OnGradeOptionsChanged();
				}

			}

		}

		
		[Column(Name="LinkGroupsFromOrgs", UpdateCheck=UpdateCheck.Never, Storage="_LinkGroupsFromOrgs", DbType="varchar(50)")]
		public string LinkGroupsFromOrgs
		{
			get { return this._LinkGroupsFromOrgs; }

			set
			{
				if (this._LinkGroupsFromOrgs != value)
				{
				
                    this.OnLinkGroupsFromOrgsChanging(value);
					this.SendPropertyChanging();
					this._LinkGroupsFromOrgs = value;
					this.SendPropertyChanged("LinkGroupsFromOrgs");
					this.OnLinkGroupsFromOrgsChanged();
				}

			}

		}

		
		[Column(Name="RegistrationClosed", UpdateCheck=UpdateCheck.Never, Storage="_RegistrationClosed", DbType="bit")]
		public bool? RegistrationClosed
		{
			get { return this._RegistrationClosed; }

			set
			{
				if (this._RegistrationClosed != value)
				{
				
                    this.OnRegistrationClosedChanging(value);
					this.SendPropertyChanging();
					this._RegistrationClosed = value;
					this.SendPropertyChanged("RegistrationClosed");
					this.OnRegistrationClosedChanged();
				}

			}

		}

		
		[Column(Name="AllowKioskRegister", UpdateCheck=UpdateCheck.Never, Storage="_AllowKioskRegister", DbType="bit")]
		public bool? AllowKioskRegister
		{
			get { return this._AllowKioskRegister; }

			set
			{
				if (this._AllowKioskRegister != value)
				{
				
                    this.OnAllowKioskRegisterChanging(value);
					this.SendPropertyChanging();
					this._AllowKioskRegister = value;
					this.SendPropertyChanged("AllowKioskRegister");
					this.OnAllowKioskRegisterChanged();
				}

			}

		}

		
		[Column(Name="RequestLabel", UpdateCheck=UpdateCheck.Never, Storage="_RequestLabel", DbType="varchar(50)")]
		public string RequestLabel
		{
			get { return this._RequestLabel; }

			set
			{
				if (this._RequestLabel != value)
				{
				
                    this.OnRequestLabelChanging(value);
					this.SendPropertyChanging();
					this._RequestLabel = value;
					this.SendPropertyChanged("RequestLabel");
					this.OnRequestLabelChanged();
				}

			}

		}

		
		[Column(Name="WorshipGroupCodes", UpdateCheck=UpdateCheck.Never, Storage="_WorshipGroupCodes", DbType="varchar(100)")]
		public string WorshipGroupCodes
		{
			get { return this._WorshipGroupCodes; }

			set
			{
				if (this._WorshipGroupCodes != value)
				{
				
                    this.OnWorshipGroupCodesChanging(value);
					this.SendPropertyChanging();
					this._WorshipGroupCodes = value;
					this.SendPropertyChanged("WorshipGroupCodes");
					this.OnWorshipGroupCodesChanged();
				}

			}

		}

		
		[Column(Name="IsBibleFellowshipOrg", UpdateCheck=UpdateCheck.Never, Storage="_IsBibleFellowshipOrg", DbType="bit")]
		public bool? IsBibleFellowshipOrg
		{
			get { return this._IsBibleFellowshipOrg; }

			set
			{
				if (this._IsBibleFellowshipOrg != value)
				{
				
                    this.OnIsBibleFellowshipOrgChanging(value);
					this.SendPropertyChanging();
					this._IsBibleFellowshipOrg = value;
					this.SendPropertyChanged("IsBibleFellowshipOrg");
					this.OnIsBibleFellowshipOrgChanged();
				}

			}

		}

		
		[Column(Name="ExtraOptions", UpdateCheck=UpdateCheck.Never, Storage="_ExtraOptions", DbType="varchar(300)")]
		public string ExtraOptions
		{
			get { return this._ExtraOptions; }

			set
			{
				if (this._ExtraOptions != value)
				{
				
                    this.OnExtraOptionsChanging(value);
					this.SendPropertyChanging();
					this._ExtraOptions = value;
					this.SendPropertyChanged("ExtraOptions");
					this.OnExtraOptionsChanged();
				}

			}

		}

		
		[Column(Name="Shell", UpdateCheck=UpdateCheck.Never, Storage="_Shell", DbType="varchar(20)")]
		public string Shell
		{
			get { return this._Shell; }

			set
			{
				if (this._Shell != value)
				{
				
                    this.OnShellChanging(value);
					this.SendPropertyChanging();
					this._Shell = value;
					this.SendPropertyChanged("Shell");
					this.OnShellChanged();
				}

			}

		}

		
		[Column(Name="ShirtSizes", UpdateCheck=UpdateCheck.Never, Storage="_ShirtSizes", DbType="varchar(300)")]
		public string ShirtSizes
		{
			get { return this._ShirtSizes; }

			set
			{
				if (this._ShirtSizes != value)
				{
				
                    this.OnShirtSizesChanging(value);
					this.SendPropertyChanging();
					this._ShirtSizes = value;
					this.SendPropertyChanged("ShirtSizes");
					this.OnShirtSizesChanged();
				}

			}

		}

		
		[Column(Name="NoSecurityLabel", UpdateCheck=UpdateCheck.Never, Storage="_NoSecurityLabel", DbType="bit")]
		public bool? NoSecurityLabel
		{
			get { return this._NoSecurityLabel; }

			set
			{
				if (this._NoSecurityLabel != value)
				{
				
                    this.OnNoSecurityLabelChanging(value);
					this.SendPropertyChanging();
					this._NoSecurityLabel = value;
					this.SendPropertyChanged("NoSecurityLabel");
					this.OnNoSecurityLabelChanged();
				}

			}

		}

		
		[Column(Name="AlwaysSecurityLabel", UpdateCheck=UpdateCheck.Never, Storage="_AlwaysSecurityLabel", DbType="bit")]
		public bool? AlwaysSecurityLabel
		{
			get { return this._AlwaysSecurityLabel; }

			set
			{
				if (this._AlwaysSecurityLabel != value)
				{
				
                    this.OnAlwaysSecurityLabelChanging(value);
					this.SendPropertyChanging();
					this._AlwaysSecurityLabel = value;
					this.SendPropertyChanged("AlwaysSecurityLabel");
					this.OnAlwaysSecurityLabelChanged();
				}

			}

		}

		
		[Column(Name="MenuItems", UpdateCheck=UpdateCheck.Never, Storage="_MenuItems", DbType="varchar(2500)")]
		public string MenuItems
		{
			get { return this._MenuItems; }

			set
			{
				if (this._MenuItems != value)
				{
				
                    this.OnMenuItemsChanging(value);
					this.SendPropertyChanging();
					this._MenuItems = value;
					this.SendPropertyChanged("MenuItems");
					this.OnMenuItemsChanged();
				}

			}

		}

		
		[Column(Name="OptionsLabel", UpdateCheck=UpdateCheck.Never, Storage="_OptionsLabel", DbType="varchar(50)")]
		public string OptionsLabel
		{
			get { return this._OptionsLabel; }

			set
			{
				if (this._OptionsLabel != value)
				{
				
                    this.OnOptionsLabelChanging(value);
					this.SendPropertyChanging();
					this._OptionsLabel = value;
					this.SendPropertyChanged("OptionsLabel");
					this.OnOptionsLabelChanged();
				}

			}

		}

		
		[Column(Name="ExtraOptionsLabel", UpdateCheck=UpdateCheck.Never, Storage="_ExtraOptionsLabel", DbType="varchar(50)")]
		public string ExtraOptionsLabel
		{
			get { return this._ExtraOptionsLabel; }

			set
			{
				if (this._ExtraOptionsLabel != value)
				{
				
                    this.OnExtraOptionsLabelChanging(value);
					this.SendPropertyChanging();
					this._ExtraOptionsLabel = value;
					this.SendPropertyChanged("ExtraOptionsLabel");
					this.OnExtraOptionsLabelChanged();
				}

			}

		}

		
		[Column(Name="DaysToIgnoreHistory", UpdateCheck=UpdateCheck.Never, Storage="_DaysToIgnoreHistory", DbType="int")]
		public int? DaysToIgnoreHistory
		{
			get { return this._DaysToIgnoreHistory; }

			set
			{
				if (this._DaysToIgnoreHistory != value)
				{
				
                    this.OnDaysToIgnoreHistoryChanging(value);
					this.SendPropertyChanging();
					this._DaysToIgnoreHistory = value;
					this.SendPropertyChanged("DaysToIgnoreHistory");
					this.OnDaysToIgnoreHistoryChanged();
				}

			}

		}

		
		[Column(Name="GroupToJoin", UpdateCheck=UpdateCheck.Never, Storage="_GroupToJoin", DbType="varchar(50)")]
		public string GroupToJoin
		{
			get { return this._GroupToJoin; }

			set
			{
				if (this._GroupToJoin != value)
				{
				
                    this.OnGroupToJoinChanging(value);
					this.SendPropertyChanging();
					this._GroupToJoin = value;
					this.SendPropertyChanged("GroupToJoin");
					this.OnGroupToJoinChanged();
				}

			}

		}

		
		[Column(Name="GiveOrgMembAccess", UpdateCheck=UpdateCheck.Never, Storage="_GiveOrgMembAccess", DbType="bit")]
		public bool? GiveOrgMembAccess
		{
			get { return this._GiveOrgMembAccess; }

			set
			{
				if (this._GiveOrgMembAccess != value)
				{
				
                    this.OnGiveOrgMembAccessChanging(value);
					this.SendPropertyChanging();
					this._GiveOrgMembAccess = value;
					this.SendPropertyChanged("GiveOrgMembAccess");
					this.OnGiveOrgMembAccessChanged();
				}

			}

		}

		
		[Column(Name="NumItemsLabel", UpdateCheck=UpdateCheck.Never, Storage="_NumItemsLabel", DbType="varchar(50)")]
		public string NumItemsLabel
		{
			get { return this._NumItemsLabel; }

			set
			{
				if (this._NumItemsLabel != value)
				{
				
                    this.OnNumItemsLabelChanging(value);
					this.SendPropertyChanging();
					this._NumItemsLabel = value;
					this.SendPropertyChanged("NumItemsLabel");
					this.OnNumItemsLabelChanged();
				}

			}

		}

		
		[Column(Name="NotReqDOB", UpdateCheck=UpdateCheck.Never, Storage="_NotReqDOB", DbType="bit")]
		public bool? NotReqDOB
		{
			get { return this._NotReqDOB; }

			set
			{
				if (this._NotReqDOB != value)
				{
				
                    this.OnNotReqDOBChanging(value);
					this.SendPropertyChanging();
					this._NotReqDOB = value;
					this.SendPropertyChanged("NotReqDOB");
					this.OnNotReqDOBChanged();
				}

			}

		}

		
		[Column(Name="NotReqAddr", UpdateCheck=UpdateCheck.Never, Storage="_NotReqAddr", DbType="bit")]
		public bool? NotReqAddr
		{
			get { return this._NotReqAddr; }

			set
			{
				if (this._NotReqAddr != value)
				{
				
                    this.OnNotReqAddrChanging(value);
					this.SendPropertyChanging();
					this._NotReqAddr = value;
					this.SendPropertyChanged("NotReqAddr");
					this.OnNotReqAddrChanged();
				}

			}

		}

		
		[Column(Name="NotReqZip", UpdateCheck=UpdateCheck.Never, Storage="_NotReqZip", DbType="bit")]
		public bool? NotReqZip
		{
			get { return this._NotReqZip; }

			set
			{
				if (this._NotReqZip != value)
				{
				
                    this.OnNotReqZipChanging(value);
					this.SendPropertyChanging();
					this._NotReqZip = value;
					this.SendPropertyChanged("NotReqZip");
					this.OnNotReqZipChanged();
				}

			}

		}

		
		[Column(Name="NotReqPhone", UpdateCheck=UpdateCheck.Never, Storage="_NotReqPhone", DbType="bit")]
		public bool? NotReqPhone
		{
			get { return this._NotReqPhone; }

			set
			{
				if (this._NotReqPhone != value)
				{
				
                    this.OnNotReqPhoneChanging(value);
					this.SendPropertyChanging();
					this._NotReqPhone = value;
					this.SendPropertyChanged("NotReqPhone");
					this.OnNotReqPhoneChanged();
				}

			}

		}

		
		[Column(Name="NotReqGender", UpdateCheck=UpdateCheck.Never, Storage="_NotReqGender", DbType="bit")]
		public bool? NotReqGender
		{
			get { return this._NotReqGender; }

			set
			{
				if (this._NotReqGender != value)
				{
				
                    this.OnNotReqGenderChanging(value);
					this.SendPropertyChanging();
					this._NotReqGender = value;
					this.SendPropertyChanged("NotReqGender");
					this.OnNotReqGenderChanged();
				}

			}

		}

		
		[Column(Name="NotReqMarital", UpdateCheck=UpdateCheck.Never, Storage="_NotReqMarital", DbType="bit")]
		public bool? NotReqMarital
		{
			get { return this._NotReqMarital; }

			set
			{
				if (this._NotReqMarital != value)
				{
				
                    this.OnNotReqMaritalChanging(value);
					this.SendPropertyChanging();
					this._NotReqMarital = value;
					this.SendPropertyChanged("NotReqMarital");
					this.OnNotReqMaritalChanged();
				}

			}

		}

		
		[Column(Name="GradeLabel", UpdateCheck=UpdateCheck.Never, Storage="_GradeLabel", DbType="varchar(50)")]
		public string GradeLabel
		{
			get { return this._GradeLabel; }

			set
			{
				if (this._GradeLabel != value)
				{
				
                    this.OnGradeLabelChanging(value);
					this.SendPropertyChanging();
					this._GradeLabel = value;
					this.SendPropertyChanged("GradeLabel");
					this.OnGradeLabelChanged();
				}

			}

		}

		
    #endregion
        
    #region Foreign Key Tables
   		
   		[Association(Name="BFMembers__BFClass", Storage="_BFMembers", OtherKey="BibleFellowshipClassId")]
   		public EntitySet< Person> BFMembers
   		{
   		    get { return this._BFMembers; }

			set	{ this._BFMembers.Assign(value); }

   		}

		
   		[Association(Name="ChildOrgs__ParentOrg", Storage="_ChildOrgs", OtherKey="ParentOrgId")]
   		public EntitySet< Organization> ChildOrgs
   		{
   		    get { return this._ChildOrgs; }

			set	{ this._ChildOrgs.Assign(value); }

   		}

		
   		[Association(Name="ENROLLMENT_TRANSACTION_ORG_FK", Storage="_EnrollmentTransactions", OtherKey="OrganizationId")]
   		public EntitySet< EnrollmentTransaction> EnrollmentTransactions
   		{
   		    get { return this._EnrollmentTransactions; }

			set	{ this._EnrollmentTransactions.Assign(value); }

   		}

		
   		[Association(Name="FK_AttendWithAbsents_TBL_ORGANIZATIONS_TBL", Storage="_Attends", OtherKey="OrganizationId")]
   		public EntitySet< Attend> Attends
   		{
   		    get { return this._Attends; }

			set	{ this._Attends.Assign(value); }

   		}

		
   		[Association(Name="FK_BadET_Organizations", Storage="_BadETs", OtherKey="OrgId")]
   		public EntitySet< BadET> BadETs
   		{
   		    get { return this._BadETs; }

			set	{ this._BadETs.Assign(value); }

   		}

		
   		[Association(Name="FK_CheckInTimes_Organizations", Storage="_CheckInTimes", OtherKey="OrganizationId")]
   		public EntitySet< CheckInTime> CheckInTimes
   		{
   		    get { return this._CheckInTimes; }

			set	{ this._CheckInTimes.Assign(value); }

   		}

		
   		[Association(Name="FK_Coupons_Organizations", Storage="_Coupons", OtherKey="OrgId")]
   		public EntitySet< Coupon> Coupons
   		{
   		    get { return this._Coupons; }

			set	{ this._Coupons.Assign(value); }

   		}

		
   		[Association(Name="FK_DivOrg_Organizations", Storage="_DivOrgs", OtherKey="OrgId")]
   		public EntitySet< DivOrg> DivOrgs
   		{
   		    get { return this._DivOrgs; }

			set	{ this._DivOrgs.Assign(value); }

   		}

		
   		[Association(Name="FK_LoveRespect_Organizations", Storage="_LoveRespects", OtherKey="OrgId")]
   		public EntitySet< LoveRespect> LoveRespects
   		{
   		    get { return this._LoveRespects; }

			set	{ this._LoveRespects.Assign(value); }

   		}

		
   		[Association(Name="FK_MEETINGS_TBL_ORGANIZATIONS_TBL", Storage="_Meetings", OtherKey="OrganizationId")]
   		public EntitySet< Meeting> Meetings
   		{
   		    get { return this._Meetings; }

			set	{ this._Meetings.Assign(value); }

   		}

		
   		[Association(Name="FK_MemberTags_Organizations", Storage="_MemberTags", OtherKey="OrgId")]
   		public EntitySet< MemberTag> MemberTags
   		{
   		    get { return this._MemberTags; }

			set	{ this._MemberTags.Assign(value); }

   		}

		
   		[Association(Name="ORGANIZATION_MEMBERS_ORG_FK", Storage="_OrganizationMembers", OtherKey="OrganizationId")]
   		public EntitySet< OrganizationMember> OrganizationMembers
   		{
   		    get { return this._OrganizationMembers; }

			set	{ this._OrganizationMembers.Assign(value); }

   		}

		
	#endregion
	
	#region Foreign Keys
    	
		[Association(Name="ChildOrgs__ParentOrg", Storage="_ParentOrg", ThisKey="ParentOrgId", IsForeignKey=true)]
		public Organization ParentOrg
		{
			get { return this._ParentOrg.Entity; }

			set
			{
				Organization previousValue = this._ParentOrg.Entity;
				if (((previousValue != value) 
							|| (this._ParentOrg.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._ParentOrg.Entity = null;
						previousValue.ChildOrgs.Remove(this);
					}

					this._ParentOrg.Entity = value;
					if (value != null)
					{
						value.ChildOrgs.Add(this);
						
						this._ParentOrgId = value.OrganizationId;
						
					}

					else
					{
						
						this._ParentOrgId = default(int?);
						
					}

					this.SendPropertyChanged("ParentOrg");
				}

			}

		}

		
		[Association(Name="FK_Organizations_AttendTrackLevel", Storage="_AttendTrackLevel", ThisKey="AttendTrkLevelId", IsForeignKey=true)]
		public AttendTrackLevel AttendTrackLevel
		{
			get { return this._AttendTrackLevel.Entity; }

			set
			{
				AttendTrackLevel previousValue = this._AttendTrackLevel.Entity;
				if (((previousValue != value) 
							|| (this._AttendTrackLevel.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._AttendTrackLevel.Entity = null;
						previousValue.Organizations.Remove(this);
					}

					this._AttendTrackLevel.Entity = value;
					if (value != null)
					{
						value.Organizations.Add(this);
						
						this._AttendTrkLevelId = value.Id;
						
					}

					else
					{
						
						this._AttendTrkLevelId = default(int);
						
					}

					this.SendPropertyChanged("AttendTrackLevel");
				}

			}

		}

		
		[Association(Name="FK_Organizations_Campus", Storage="_Campu", ThisKey="CampusId", IsForeignKey=true)]
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
						previousValue.Organizations.Remove(this);
					}

					this._Campu.Entity = value;
					if (value != null)
					{
						value.Organizations.Add(this);
						
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

		
		[Association(Name="FK_Organizations_Division", Storage="_Division", ThisKey="DivisionId", IsForeignKey=true)]
		public Division Division
		{
			get { return this._Division.Entity; }

			set
			{
				Division previousValue = this._Division.Entity;
				if (((previousValue != value) 
							|| (this._Division.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._Division.Entity = null;
						previousValue.Organizations.Remove(this);
					}

					this._Division.Entity = value;
					if (value != null)
					{
						value.Organizations.Add(this);
						
						this._DivisionId = value.Id;
						
					}

					else
					{
						
						this._DivisionId = default(int?);
						
					}

					this.SendPropertyChanged("Division");
				}

			}

		}

		
		[Association(Name="FK_Organizations_Gender", Storage="_Gender", ThisKey="GenderId", IsForeignKey=true)]
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
						previousValue.Organizations.Remove(this);
					}

					this._Gender.Entity = value;
					if (value != null)
					{
						value.Organizations.Add(this);
						
						this._GenderId = value.Id;
						
					}

					else
					{
						
						this._GenderId = default(int?);
						
					}

					this.SendPropertyChanged("Gender");
				}

			}

		}

		
		[Association(Name="FK_Organizations_RegistrationType", Storage="_RegistrationType", ThisKey="RegistrationTypeId", IsForeignKey=true)]
		public RegistrationType RegistrationType
		{
			get { return this._RegistrationType.Entity; }

			set
			{
				RegistrationType previousValue = this._RegistrationType.Entity;
				if (((previousValue != value) 
							|| (this._RegistrationType.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._RegistrationType.Entity = null;
						previousValue.Organizations.Remove(this);
					}

					this._RegistrationType.Entity = value;
					if (value != null)
					{
						value.Organizations.Add(this);
						
						this._RegistrationTypeId = value.Id;
						
					}

					else
					{
						
						this._RegistrationTypeId = default(int?);
						
					}

					this.SendPropertyChanged("RegistrationType");
				}

			}

		}

		
		[Association(Name="FK_ORGANIZATIONS_TBL_EntryPoint", Storage="_EntryPoint", ThisKey="EntryPointId", IsForeignKey=true)]
		public EntryPoint EntryPoint
		{
			get { return this._EntryPoint.Entity; }

			set
			{
				EntryPoint previousValue = this._EntryPoint.Entity;
				if (((previousValue != value) 
							|| (this._EntryPoint.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._EntryPoint.Entity = null;
						previousValue.Organizations.Remove(this);
					}

					this._EntryPoint.Entity = value;
					if (value != null)
					{
						value.Organizations.Add(this);
						
						this._EntryPointId = value.Id;
						
					}

					else
					{
						
						this._EntryPointId = default(int?);
						
					}

					this.SendPropertyChanged("EntryPoint");
				}

			}

		}

		
		[Association(Name="FK_ORGANIZATIONS_TBL_OrganizationStatus", Storage="_OrganizationStatus", ThisKey="OrganizationStatusId", IsForeignKey=true)]
		public OrganizationStatus OrganizationStatus
		{
			get { return this._OrganizationStatus.Entity; }

			set
			{
				OrganizationStatus previousValue = this._OrganizationStatus.Entity;
				if (((previousValue != value) 
							|| (this._OrganizationStatus.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if (previousValue != null)
					{
						this._OrganizationStatus.Entity = null;
						previousValue.Organizations.Remove(this);
					}

					this._OrganizationStatus.Entity = value;
					if (value != null)
					{
						value.Organizations.Add(this);
						
						this._OrganizationStatusId = value.Id;
						
					}

					else
					{
						
						this._OrganizationStatusId = default(int);
						
					}

					this.SendPropertyChanged("OrganizationStatus");
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

   		
		private void attach_BFMembers(Person entity)
		{
			this.SendPropertyChanging();
			entity.BFClass = this;
		}

		private void detach_BFMembers(Person entity)
		{
			this.SendPropertyChanging();
			entity.BFClass = null;
		}

		
		private void attach_ChildOrgs(Organization entity)
		{
			this.SendPropertyChanging();
			entity.ParentOrg = this;
		}

		private void detach_ChildOrgs(Organization entity)
		{
			this.SendPropertyChanging();
			entity.ParentOrg = null;
		}

		
		private void attach_EnrollmentTransactions(EnrollmentTransaction entity)
		{
			this.SendPropertyChanging();
			entity.Organization = this;
		}

		private void detach_EnrollmentTransactions(EnrollmentTransaction entity)
		{
			this.SendPropertyChanging();
			entity.Organization = null;
		}

		
		private void attach_Attends(Attend entity)
		{
			this.SendPropertyChanging();
			entity.Organization = this;
		}

		private void detach_Attends(Attend entity)
		{
			this.SendPropertyChanging();
			entity.Organization = null;
		}

		
		private void attach_BadETs(BadET entity)
		{
			this.SendPropertyChanging();
			entity.Organization = this;
		}

		private void detach_BadETs(BadET entity)
		{
			this.SendPropertyChanging();
			entity.Organization = null;
		}

		
		private void attach_CheckInTimes(CheckInTime entity)
		{
			this.SendPropertyChanging();
			entity.Organization = this;
		}

		private void detach_CheckInTimes(CheckInTime entity)
		{
			this.SendPropertyChanging();
			entity.Organization = null;
		}

		
		private void attach_Coupons(Coupon entity)
		{
			this.SendPropertyChanging();
			entity.Organization = this;
		}

		private void detach_Coupons(Coupon entity)
		{
			this.SendPropertyChanging();
			entity.Organization = null;
		}

		
		private void attach_DivOrgs(DivOrg entity)
		{
			this.SendPropertyChanging();
			entity.Organization = this;
		}

		private void detach_DivOrgs(DivOrg entity)
		{
			this.SendPropertyChanging();
			entity.Organization = null;
		}

		
		private void attach_LoveRespects(LoveRespect entity)
		{
			this.SendPropertyChanging();
			entity.Organization = this;
		}

		private void detach_LoveRespects(LoveRespect entity)
		{
			this.SendPropertyChanging();
			entity.Organization = null;
		}

		
		private void attach_Meetings(Meeting entity)
		{
			this.SendPropertyChanging();
			entity.Organization = this;
		}

		private void detach_Meetings(Meeting entity)
		{
			this.SendPropertyChanging();
			entity.Organization = null;
		}

		
		private void attach_MemberTags(MemberTag entity)
		{
			this.SendPropertyChanging();
			entity.Organization = this;
		}

		private void detach_MemberTags(MemberTag entity)
		{
			this.SendPropertyChanging();
			entity.Organization = null;
		}

		
		private void attach_OrganizationMembers(OrganizationMember entity)
		{
			this.SendPropertyChanging();
			entity.Organization = this;
		}

		private void detach_OrganizationMembers(OrganizationMember entity)
		{
			this.SendPropertyChanging();
			entity.Organization = null;
		}

		
	}

}

