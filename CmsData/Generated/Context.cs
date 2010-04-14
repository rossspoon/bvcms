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
	[DatabaseAttribute(Name="CMS")]
	public partial class CMSDataContext : DataContext
	{
		private static MappingSource mappingSource = new AttributeMappingSource();
		
#region Extensibility Method Definitions
        partial void OnCreated();
		
        partial void InsertActivityLog(ActivityLog instance);
        partial void UpdateActivityLog(ActivityLog instance);
        partial void DeleteActivityLog(ActivityLog instance);
        
        partial void InsertAddress(Address instance);
        partial void UpdateAddress(Address instance);
        partial void DeleteAddress(Address instance);
        
        partial void InsertAddressType(AddressType instance);
        partial void UpdateAddressType(AddressType instance);
        partial void DeleteAddressType(AddressType instance);
        
        partial void InsertAttend(Attend instance);
        partial void UpdateAttend(Attend instance);
        partial void DeleteAttend(Attend instance);
        
        partial void InsertAttendanceClassification(AttendanceClassification instance);
        partial void UpdateAttendanceClassification(AttendanceClassification instance);
        partial void DeleteAttendanceClassification(AttendanceClassification instance);
        
        partial void InsertAttendTrackLevel(AttendTrackLevel instance);
        partial void UpdateAttendTrackLevel(AttendTrackLevel instance);
        partial void DeleteAttendTrackLevel(AttendTrackLevel instance);
        
        partial void InsertAttendType(AttendType instance);
        partial void UpdateAttendType(AttendType instance);
        partial void DeleteAttendType(AttendType instance);
        
        partial void InsertAudit(Audit instance);
        partial void UpdateAudit(Audit instance);
        partial void DeleteAudit(Audit instance);
        
        partial void InsertAuditValue(AuditValue instance);
        partial void UpdateAuditValue(AuditValue instance);
        partial void DeleteAuditValue(AuditValue instance);
        
        partial void InsertBadET(BadET instance);
        partial void UpdateBadET(BadET instance);
        partial void DeleteBadET(BadET instance);
        
        partial void InsertBaptismStatus(BaptismStatus instance);
        partial void UpdateBaptismStatus(BaptismStatus instance);
        partial void DeleteBaptismStatus(BaptismStatus instance);
        
        partial void InsertBaptismType(BaptismType instance);
        partial void UpdateBaptismType(BaptismType instance);
        partial void DeleteBaptismType(BaptismType instance);
        
        partial void InsertBlog(Blog instance);
        partial void UpdateBlog(Blog instance);
        partial void DeleteBlog(Blog instance);
        
        partial void InsertBlogCategory(BlogCategory instance);
        partial void UpdateBlogCategory(BlogCategory instance);
        partial void DeleteBlogCategory(BlogCategory instance);
        
        partial void InsertBlogCategoryXref(BlogCategoryXref instance);
        partial void UpdateBlogCategoryXref(BlogCategoryXref instance);
        partial void DeleteBlogCategoryXref(BlogCategoryXref instance);
        
        partial void InsertBlogComment(BlogComment instance);
        partial void UpdateBlogComment(BlogComment instance);
        partial void DeleteBlogComment(BlogComment instance);
        
        partial void InsertBlogNotify(BlogNotify instance);
        partial void UpdateBlogNotify(BlogNotify instance);
        partial void DeleteBlogNotify(BlogNotify instance);
        
        partial void InsertBlogPost(BlogPost instance);
        partial void UpdateBlogPost(BlogPost instance);
        partial void DeleteBlogPost(BlogPost instance);
        
        partial void InsertBundleDetail(BundleDetail instance);
        partial void UpdateBundleDetail(BundleDetail instance);
        partial void DeleteBundleDetail(BundleDetail instance);
        
        partial void InsertBundleHeader(BundleHeader instance);
        partial void UpdateBundleHeader(BundleHeader instance);
        partial void DeleteBundleHeader(BundleHeader instance);
        
        partial void InsertBundleHeaderType(BundleHeaderType instance);
        partial void UpdateBundleHeaderType(BundleHeaderType instance);
        partial void DeleteBundleHeaderType(BundleHeaderType instance);
        
        partial void InsertBundleStatusType(BundleStatusType instance);
        partial void UpdateBundleStatusType(BundleStatusType instance);
        partial void DeleteBundleStatusType(BundleStatusType instance);
        
        partial void InsertCampu(Campu instance);
        partial void UpdateCampu(Campu instance);
        partial void DeleteCampu(Campu instance);
        
        partial void InsertCardIdentifier(CardIdentifier instance);
        partial void UpdateCardIdentifier(CardIdentifier instance);
        partial void DeleteCardIdentifier(CardIdentifier instance);
        
        partial void InsertCheckInTime(CheckInTime instance);
        partial void UpdateCheckInTime(CheckInTime instance);
        partial void DeleteCheckInTime(CheckInTime instance);
        
        partial void InsertChurchAttReportId(ChurchAttReportId instance);
        partial void UpdateChurchAttReportId(ChurchAttReportId instance);
        partial void DeleteChurchAttReportId(ChurchAttReportId instance);
        
        partial void InsertContactee(Contactee instance);
        partial void UpdateContactee(Contactee instance);
        partial void DeleteContactee(Contactee instance);
        
        partial void InsertContactor(Contactor instance);
        partial void UpdateContactor(Contactor instance);
        partial void DeleteContactor(Contactor instance);
        
        partial void InsertContactPreference(ContactPreference instance);
        partial void UpdateContactPreference(ContactPreference instance);
        partial void DeleteContactPreference(ContactPreference instance);
        
        partial void InsertContent(Content instance);
        partial void UpdateContent(Content instance);
        partial void DeleteContent(Content instance);
        
        partial void InsertContribution(Contribution instance);
        partial void UpdateContribution(Contribution instance);
        partial void DeleteContribution(Contribution instance);
        
        partial void InsertContributionFund(ContributionFund instance);
        partial void UpdateContributionFund(ContributionFund instance);
        partial void DeleteContributionFund(ContributionFund instance);
        
        partial void InsertContributionStatus(ContributionStatus instance);
        partial void UpdateContributionStatus(ContributionStatus instance);
        partial void DeleteContributionStatus(ContributionStatus instance);
        
        partial void InsertContributionType(ContributionType instance);
        partial void UpdateContributionType(ContributionType instance);
        partial void DeleteContributionType(ContributionType instance);
        
        partial void InsertCountryLookup(CountryLookup instance);
        partial void UpdateCountryLookup(CountryLookup instance);
        partial void DeleteCountryLookup(CountryLookup instance);
        
        partial void InsertCoupon(Coupon instance);
        partial void UpdateCoupon(Coupon instance);
        partial void DeleteCoupon(Coupon instance);
        
        partial void InsertDecisionType(DecisionType instance);
        partial void UpdateDecisionType(DecisionType instance);
        partial void DeleteDecisionType(DecisionType instance);
        
        partial void InsertDiscoveryClassStatus(DiscoveryClassStatus instance);
        partial void UpdateDiscoveryClassStatus(DiscoveryClassStatus instance);
        partial void DeleteDiscoveryClassStatus(DiscoveryClassStatus instance);
        
        partial void InsertDivision(Division instance);
        partial void UpdateDivision(Division instance);
        partial void DeleteDivision(Division instance);
        
        partial void InsertDivOrg(DivOrg instance);
        partial void UpdateDivOrg(DivOrg instance);
        partial void DeleteDivOrg(DivOrg instance);
        
        partial void InsertDropType(DropType instance);
        partial void UpdateDropType(DropType instance);
        partial void DeleteDropType(DropType instance);
        
        partial void InsertEmailLog(EmailLog instance);
        partial void UpdateEmailLog(EmailLog instance);
        partial void DeleteEmailLog(EmailLog instance);
        
        partial void InsertEmailOptOut(EmailOptOut instance);
        partial void UpdateEmailOptOut(EmailOptOut instance);
        partial void DeleteEmailOptOut(EmailOptOut instance);
        
        partial void InsertEnrollmentTransaction(EnrollmentTransaction instance);
        partial void UpdateEnrollmentTransaction(EnrollmentTransaction instance);
        partial void DeleteEnrollmentTransaction(EnrollmentTransaction instance);
        
        partial void InsertEntryPoint(EntryPoint instance);
        partial void UpdateEntryPoint(EntryPoint instance);
        partial void DeleteEntryPoint(EntryPoint instance);
        
        partial void InsertEnvelopeOption(EnvelopeOption instance);
        partial void UpdateEnvelopeOption(EnvelopeOption instance);
        partial void DeleteEnvelopeOption(EnvelopeOption instance);
        
        partial void InsertExtraDatum(ExtraDatum instance);
        partial void UpdateExtraDatum(ExtraDatum instance);
        partial void DeleteExtraDatum(ExtraDatum instance);
        
        partial void InsertFamily(Family instance);
        partial void UpdateFamily(Family instance);
        partial void DeleteFamily(Family instance);
        
        partial void InsertFamilyMemberType(FamilyMemberType instance);
        partial void UpdateFamilyMemberType(FamilyMemberType instance);
        partial void DeleteFamilyMemberType(FamilyMemberType instance);
        
        partial void InsertFamilyPosition(FamilyPosition instance);
        partial void UpdateFamilyPosition(FamilyPosition instance);
        partial void DeleteFamilyPosition(FamilyPosition instance);
        
        partial void InsertFamilyRelationship(FamilyRelationship instance);
        partial void UpdateFamilyRelationship(FamilyRelationship instance);
        partial void DeleteFamilyRelationship(FamilyRelationship instance);
        
        partial void InsertForum(Forum instance);
        partial void UpdateForum(Forum instance);
        partial void DeleteForum(Forum instance);
        
        partial void InsertForumEntry(ForumEntry instance);
        partial void UpdateForumEntry(ForumEntry instance);
        partial void DeleteForumEntry(ForumEntry instance);
        
        partial void InsertForumNotify(ForumNotify instance);
        partial void UpdateForumNotify(ForumNotify instance);
        partial void DeleteForumNotify(ForumNotify instance);
        
        partial void InsertForumUserRead(ForumUserRead instance);
        partial void UpdateForumUserRead(ForumUserRead instance);
        partial void DeleteForumUserRead(ForumUserRead instance);
        
        partial void InsertGender(Gender instance);
        partial void UpdateGender(Gender instance);
        partial void DeleteGender(Gender instance);
        
        partial void InsertGenderClass(GenderClass instance);
        partial void UpdateGenderClass(GenderClass instance);
        partial void DeleteGenderClass(GenderClass instance);
        
        partial void InsertGeoCode(GeoCode instance);
        partial void UpdateGeoCode(GeoCode instance);
        partial void DeleteGeoCode(GeoCode instance);
        
        partial void InsertGroup(Group instance);
        partial void UpdateGroup(Group instance);
        partial void DeleteGroup(Group instance);
        
        partial void InsertGroupRole(GroupRole instance);
        partial void UpdateGroupRole(GroupRole instance);
        partial void DeleteGroupRole(GroupRole instance);
        
        partial void InsertInterestPoint(InterestPoint instance);
        partial void UpdateInterestPoint(InterestPoint instance);
        partial void DeleteInterestPoint(InterestPoint instance);
        
        partial void InsertInvitation(Invitation instance);
        partial void UpdateInvitation(Invitation instance);
        partial void DeleteInvitation(Invitation instance);
        
        partial void InsertJoinType(JoinType instance);
        partial void UpdateJoinType(JoinType instance);
        partial void DeleteJoinType(JoinType instance);
        
        partial void InsertLoveRespect(LoveRespect instance);
        partial void UpdateLoveRespect(LoveRespect instance);
        partial void DeleteLoveRespect(LoveRespect instance);
        
        partial void InsertMaritalStatus(MaritalStatus instance);
        partial void UpdateMaritalStatus(MaritalStatus instance);
        partial void DeleteMaritalStatus(MaritalStatus instance);
        
        partial void InsertMeeting(Meeting instance);
        partial void UpdateMeeting(Meeting instance);
        partial void DeleteMeeting(Meeting instance);
        
        partial void InsertMeetingType(MeetingType instance);
        partial void UpdateMeetingType(MeetingType instance);
        partial void DeleteMeetingType(MeetingType instance);
        
        partial void InsertMemberLetterStatus(MemberLetterStatus instance);
        partial void UpdateMemberLetterStatus(MemberLetterStatus instance);
        partial void DeleteMemberLetterStatus(MemberLetterStatus instance);
        
        partial void InsertMemberStatus(MemberStatus instance);
        partial void UpdateMemberStatus(MemberStatus instance);
        partial void DeleteMemberStatus(MemberStatus instance);
        
        partial void InsertMemberTag(MemberTag instance);
        partial void UpdateMemberTag(MemberTag instance);
        partial void DeleteMemberTag(MemberTag instance);
        
        partial void InsertMemberType(MemberType instance);
        partial void UpdateMemberType(MemberType instance);
        partial void DeleteMemberType(MemberType instance);
        
        partial void InsertMinistry(Ministry instance);
        partial void UpdateMinistry(Ministry instance);
        partial void DeleteMinistry(Ministry instance);
        
        partial void InsertMOBSReg(MOBSReg instance);
        partial void UpdateMOBSReg(MOBSReg instance);
        partial void DeleteMOBSReg(MOBSReg instance);
        
        partial void InsertNameSuffix(NameSuffix instance);
        partial void UpdateNameSuffix(NameSuffix instance);
        partial void DeleteNameSuffix(NameSuffix instance);
        
        partial void InsertNameTitle(NameTitle instance);
        partial void UpdateNameTitle(NameTitle instance);
        partial void DeleteNameTitle(NameTitle instance);
        
        partial void InsertNewContact(NewContact instance);
        partial void UpdateNewContact(NewContact instance);
        partial void DeleteNewContact(NewContact instance);
        
        partial void InsertNewContactReason(NewContactReason instance);
        partial void UpdateNewContactReason(NewContactReason instance);
        partial void DeleteNewContactReason(NewContactReason instance);
        
        partial void InsertNewContactType(NewContactType instance);
        partial void UpdateNewContactType(NewContactType instance);
        partial void DeleteNewContactType(NewContactType instance);
        
        partial void InsertOrganizationMember(OrganizationMember instance);
        partial void UpdateOrganizationMember(OrganizationMember instance);
        partial void DeleteOrganizationMember(OrganizationMember instance);
        
        partial void InsertOrganization(Organization instance);
        partial void UpdateOrganization(Organization instance);
        partial void DeleteOrganization(Organization instance);
        
        partial void InsertOrganizationStatus(OrganizationStatus instance);
        partial void UpdateOrganizationStatus(OrganizationStatus instance);
        partial void DeleteOrganizationStatus(OrganizationStatus instance);
        
        partial void InsertOrgMemMemTag(OrgMemMemTag instance);
        partial void UpdateOrgMemMemTag(OrgMemMemTag instance);
        partial void DeleteOrgMemMemTag(OrgMemMemTag instance);
        
        partial void InsertOrigin(Origin instance);
        partial void UpdateOrigin(Origin instance);
        partial void DeleteOrigin(Origin instance);
        
        partial void InsertOtherNotify(OtherNotify instance);
        partial void UpdateOtherNotify(OtherNotify instance);
        partial void DeleteOtherNotify(OtherNotify instance);
        
        partial void InsertPageContent(PageContent instance);
        partial void UpdatePageContent(PageContent instance);
        partial void DeletePageContent(PageContent instance);
        
        partial void InsertPageVisit(PageVisit instance);
        partial void UpdatePageVisit(PageVisit instance);
        partial void DeletePageVisit(PageVisit instance);
        
        partial void InsertParaContent(ParaContent instance);
        partial void UpdateParaContent(ParaContent instance);
        partial void DeleteParaContent(ParaContent instance);
        
        partial void InsertPendingNotification(PendingNotification instance);
        partial void UpdatePendingNotification(PendingNotification instance);
        partial void DeletePendingNotification(PendingNotification instance);
        
        partial void InsertPerson(Person instance);
        partial void UpdatePerson(Person instance);
        partial void DeletePerson(Person instance);
        
        partial void InsertPeopleExtra(PeopleExtra instance);
        partial void UpdatePeopleExtra(PeopleExtra instance);
        partial void DeletePeopleExtra(PeopleExtra instance);
        
        partial void InsertPhonePreference(PhonePreference instance);
        partial void UpdatePhonePreference(PhonePreference instance);
        partial void DeletePhonePreference(PhonePreference instance);
        
        partial void InsertPicture(Picture instance);
        partial void UpdatePicture(Picture instance);
        partial void DeletePicture(Picture instance);
        
        partial void InsertPodCast(PodCast instance);
        partial void UpdatePodCast(PodCast instance);
        partial void DeletePodCast(PodCast instance);
        
        partial void InsertPostalLookup(PostalLookup instance);
        partial void UpdatePostalLookup(PostalLookup instance);
        partial void DeletePostalLookup(PostalLookup instance);
        
        partial void InsertPrayerSlot(PrayerSlot instance);
        partial void UpdatePrayerSlot(PrayerSlot instance);
        partial void DeletePrayerSlot(PrayerSlot instance);
        
        partial void InsertPreference(Preference instance);
        partial void UpdatePreference(Preference instance);
        partial void DeletePreference(Preference instance);
        
        partial void InsertProgram(Program instance);
        partial void UpdateProgram(Program instance);
        partial void DeleteProgram(Program instance);
        
        partial void InsertPromotion(Promotion instance);
        partial void UpdatePromotion(Promotion instance);
        partial void DeletePromotion(Promotion instance);
        
        partial void InsertQueryBuilderClause(QueryBuilderClause instance);
        partial void UpdateQueryBuilderClause(QueryBuilderClause instance);
        partial void DeleteQueryBuilderClause(QueryBuilderClause instance);
        
        partial void InsertReadPlan(ReadPlan instance);
        partial void UpdateReadPlan(ReadPlan instance);
        partial void DeleteReadPlan(ReadPlan instance);
        
        partial void InsertRecLeague(RecLeague instance);
        partial void UpdateRecLeague(RecLeague instance);
        partial void DeleteRecLeague(RecLeague instance);
        
        partial void InsertRecReg(RecReg instance);
        partial void UpdateRecReg(RecReg instance);
        partial void DeleteRecReg(RecReg instance);
        
        partial void InsertRegistrationType(RegistrationType instance);
        partial void UpdateRegistrationType(RegistrationType instance);
        partial void DeleteRegistrationType(RegistrationType instance);
        
        partial void InsertRelatedFamily(RelatedFamily instance);
        partial void UpdateRelatedFamily(RelatedFamily instance);
        partial void DeleteRelatedFamily(RelatedFamily instance);
        
        partial void InsertResidentCode(ResidentCode instance);
        partial void UpdateResidentCode(ResidentCode instance);
        partial void DeleteResidentCode(ResidentCode instance);
        
        partial void InsertRole(Role instance);
        partial void UpdateRole(Role instance);
        partial void DeleteRole(Role instance);
        
        partial void InsertRssFeed(RssFeed instance);
        partial void UpdateRssFeed(RssFeed instance);
        partial void DeleteRssFeed(RssFeed instance);
        
        partial void InsertSaleItem(SaleItem instance);
        partial void UpdateSaleItem(SaleItem instance);
        partial void DeleteSaleItem(SaleItem instance);
        
        partial void InsertSaleTransaction(SaleTransaction instance);
        partial void UpdateSaleTransaction(SaleTransaction instance);
        partial void DeleteSaleTransaction(SaleTransaction instance);
        
        partial void InsertSetting(Setting instance);
        partial void UpdateSetting(Setting instance);
        partial void DeleteSetting(Setting instance);
        
        partial void InsertShirtSize(ShirtSize instance);
        partial void UpdateShirtSize(ShirtSize instance);
        partial void DeleteShirtSize(ShirtSize instance);
        
        partial void InsertSoulMate(SoulMate instance);
        partial void UpdateSoulMate(SoulMate instance);
        partial void DeleteSoulMate(SoulMate instance);
        
        partial void InsertStateLookup(StateLookup instance);
        partial void UpdateStateLookup(StateLookup instance);
        partial void DeleteStateLookup(StateLookup instance);
        
        partial void InsertTag(Tag instance);
        partial void UpdateTag(Tag instance);
        partial void DeleteTag(Tag instance);
        
        partial void InsertTagPerson(TagPerson instance);
        partial void UpdateTagPerson(TagPerson instance);
        partial void DeleteTagPerson(TagPerson instance);
        
        partial void InsertTagShare(TagShare instance);
        partial void UpdateTagShare(TagShare instance);
        partial void DeleteTagShare(TagShare instance);
        
        partial void InsertTagType(TagType instance);
        partial void UpdateTagType(TagType instance);
        partial void DeleteTagType(TagType instance);
        
        partial void InsertTask(Task instance);
        partial void UpdateTask(Task instance);
        partial void DeleteTask(Task instance);
        
        partial void InsertTaskList(TaskList instance);
        partial void UpdateTaskList(TaskList instance);
        partial void DeleteTaskList(TaskList instance);
        
        partial void InsertTaskListOwner(TaskListOwner instance);
        partial void UpdateTaskListOwner(TaskListOwner instance);
        partial void DeleteTaskListOwner(TaskListOwner instance);
        
        partial void InsertTaskStatus(TaskStatus instance);
        partial void UpdateTaskStatus(TaskStatus instance);
        partial void DeleteTaskStatus(TaskStatus instance);
        
        partial void InsertTemporaryToken(TemporaryToken instance);
        partial void UpdateTemporaryToken(TemporaryToken instance);
        partial void DeleteTemporaryToken(TemporaryToken instance);
        
        partial void InsertUploadAuthenticationXref(UploadAuthenticationXref instance);
        partial void UpdateUploadAuthenticationXref(UploadAuthenticationXref instance);
        partial void DeleteUploadAuthenticationXref(UploadAuthenticationXref instance);
        
        partial void InsertUserCanEmailFor(UserCanEmailFor instance);
        partial void UpdateUserCanEmailFor(UserCanEmailFor instance);
        partial void DeleteUserCanEmailFor(UserCanEmailFor instance);
        
        partial void InsertUserGroupRole(UserGroupRole instance);
        partial void UpdateUserGroupRole(UserGroupRole instance);
        partial void DeleteUserGroupRole(UserGroupRole instance);
        
        partial void InsertUserRole(UserRole instance);
        partial void UpdateUserRole(UserRole instance);
        partial void DeleteUserRole(UserRole instance);
        
        partial void InsertUser(User instance);
        partial void UpdateUser(User instance);
        partial void DeleteUser(User instance);
        
        partial void InsertVBSApp(VBSApp instance);
        partial void UpdateVBSApp(VBSApp instance);
        partial void DeleteVBSApp(VBSApp instance);
        
        partial void InsertVerse(Verse instance);
        partial void UpdateVerse(Verse instance);
        partial void DeleteVerse(Verse instance);
        
        partial void InsertVerseCategory(VerseCategory instance);
        partial void UpdateVerseCategory(VerseCategory instance);
        partial void DeleteVerseCategory(VerseCategory instance);
        
        partial void InsertVerseCategoryXref(VerseCategoryXref instance);
        partial void UpdateVerseCategoryXref(VerseCategoryXref instance);
        partial void DeleteVerseCategoryXref(VerseCategoryXref instance);
        
        partial void InsertVolApplicationStatus(VolApplicationStatus instance);
        partial void UpdateVolApplicationStatus(VolApplicationStatus instance);
        partial void DeleteVolApplicationStatus(VolApplicationStatus instance);
        
        partial void InsertVolInterestCode(VolInterestCode instance);
        partial void UpdateVolInterestCode(VolInterestCode instance);
        partial void DeleteVolInterestCode(VolInterestCode instance);
        
        partial void InsertVolInterestInterestCode(VolInterestInterestCode instance);
        partial void UpdateVolInterestInterestCode(VolInterestInterestCode instance);
        partial void DeleteVolInterestInterestCode(VolInterestInterestCode instance);
        
        partial void InsertVolunteer(Volunteer instance);
        partial void UpdateVolunteer(Volunteer instance);
        partial void DeleteVolunteer(Volunteer instance);
        
        partial void InsertVolunteerCode(VolunteerCode instance);
        partial void UpdateVolunteerCode(VolunteerCode instance);
        partial void DeleteVolunteerCode(VolunteerCode instance);
        
        partial void InsertVolunteerForm(VolunteerForm instance);
        partial void UpdateVolunteerForm(VolunteerForm instance);
        partial void DeleteVolunteerForm(VolunteerForm instance);
        
        partial void InsertWord(Word instance);
        partial void UpdateWord(Word instance);
        partial void DeleteWord(Word instance);
        
        partial void InsertZipCode(ZipCode instance);
        partial void UpdateZipCode(ZipCode instance);
        partial void DeleteZipCode(ZipCode instance);
        
        partial void InsertZip(Zip instance);
        partial void UpdateZip(Zip instance);
        partial void DeleteZip(Zip instance);
        
#endregion
		
		public CMSDataContext() : 
				base(System.Configuration.ConfigurationManager.ConnectionStrings["CMS"].ConnectionString, mappingSource)
		{
			OnCreated();
		}

		
		public CMSDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}

		
		public CMSDataContext(IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}

		
		public CMSDataContext(string connection, MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}

		
		public CMSDataContext(IDbConnection connection, MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}

    #region Tables
		
		public Table< ActivityLog> ActivityLogs
		{
			get	{ return this.GetTable< ActivityLog>(); }

		}

		public Table< Address> Addresses
		{
			get	{ return this.GetTable< Address>(); }

		}

		public Table< AddressType> AddressTypes
		{
			get	{ return this.GetTable< AddressType>(); }

		}

		public Table< Attend> Attends
		{
			get	{ return this.GetTable< Attend>(); }

		}

		public Table< AttendanceClassification> AttendanceClassifications
		{
			get	{ return this.GetTable< AttendanceClassification>(); }

		}

		public Table< AttendTrackLevel> AttendTrackLevels
		{
			get	{ return this.GetTable< AttendTrackLevel>(); }

		}

		public Table< AttendType> AttendTypes
		{
			get	{ return this.GetTable< AttendType>(); }

		}

		public Table< Audit> Audits
		{
			get	{ return this.GetTable< Audit>(); }

		}

		public Table< AuditValue> AuditValues
		{
			get	{ return this.GetTable< AuditValue>(); }

		}

		public Table< BadET> BadETs
		{
			get	{ return this.GetTable< BadET>(); }

		}

		public Table< BaptismStatus> BaptismStatuses
		{
			get	{ return this.GetTable< BaptismStatus>(); }

		}

		public Table< BaptismType> BaptismTypes
		{
			get	{ return this.GetTable< BaptismType>(); }

		}

		public Table< Blog> Blogs
		{
			get	{ return this.GetTable< Blog>(); }

		}

		public Table< BlogCategory> BlogCategories
		{
			get	{ return this.GetTable< BlogCategory>(); }

		}

		public Table< BlogCategoryXref> BlogCategoryXrefs
		{
			get	{ return this.GetTable< BlogCategoryXref>(); }

		}

		public Table< BlogComment> BlogComments
		{
			get	{ return this.GetTable< BlogComment>(); }

		}

		public Table< BlogNotify> BlogNotifications
		{
			get	{ return this.GetTable< BlogNotify>(); }

		}

		public Table< BlogPost> BlogPosts
		{
			get	{ return this.GetTable< BlogPost>(); }

		}

		public Table< BundleDetail> BundleDetails
		{
			get	{ return this.GetTable< BundleDetail>(); }

		}

		public Table< BundleHeader> BundleHeaders
		{
			get	{ return this.GetTable< BundleHeader>(); }

		}

		public Table< BundleHeaderType> BundleHeaderTypes
		{
			get	{ return this.GetTable< BundleHeaderType>(); }

		}

		public Table< BundleStatusType> BundleStatusTypes
		{
			get	{ return this.GetTable< BundleStatusType>(); }

		}

		public Table< Campu> Campus
		{
			get	{ return this.GetTable< Campu>(); }

		}

		public Table< CardIdentifier> CardIdentifiers
		{
			get	{ return this.GetTable< CardIdentifier>(); }

		}

		public Table< CheckInTime> CheckInTimes
		{
			get	{ return this.GetTable< CheckInTime>(); }

		}

		public Table< ChurchAttReportId> ChurchAttReportIds
		{
			get	{ return this.GetTable< ChurchAttReportId>(); }

		}

		public Table< Contactee> Contactees
		{
			get	{ return this.GetTable< Contactee>(); }

		}

		public Table< Contactor> Contactors
		{
			get	{ return this.GetTable< Contactor>(); }

		}

		public Table< ContactPreference> ContactPreferences
		{
			get	{ return this.GetTable< ContactPreference>(); }

		}

		public Table< Content> Contents
		{
			get	{ return this.GetTable< Content>(); }

		}

		public Table< Contribution> Contributions
		{
			get	{ return this.GetTable< Contribution>(); }

		}

		public Table< ContributionFund> ContributionFunds
		{
			get	{ return this.GetTable< ContributionFund>(); }

		}

		public Table< ContributionStatus> ContributionStatuses
		{
			get	{ return this.GetTable< ContributionStatus>(); }

		}

		public Table< ContributionType> ContributionTypes
		{
			get	{ return this.GetTable< ContributionType>(); }

		}

		public Table< CountryLookup> CountryLookups
		{
			get	{ return this.GetTable< CountryLookup>(); }

		}

		public Table< Coupon> Coupons
		{
			get	{ return this.GetTable< Coupon>(); }

		}

		public Table< DecisionType> DecisionTypes
		{
			get	{ return this.GetTable< DecisionType>(); }

		}

		public Table< DiscoveryClassStatus> DiscoveryClassStatuses
		{
			get	{ return this.GetTable< DiscoveryClassStatus>(); }

		}

		public Table< Division> Divisions
		{
			get	{ return this.GetTable< Division>(); }

		}

		public Table< DivOrg> DivOrgs
		{
			get	{ return this.GetTable< DivOrg>(); }

		}

		public Table< DropType> DropTypes
		{
			get	{ return this.GetTable< DropType>(); }

		}

		public Table< EmailLog> EmailLogs
		{
			get	{ return this.GetTable< EmailLog>(); }

		}

		public Table< EmailOptOut> EmailOptOuts
		{
			get	{ return this.GetTable< EmailOptOut>(); }

		}

		public Table< EnrollmentTransaction> EnrollmentTransactions
		{
			get	{ return this.GetTable< EnrollmentTransaction>(); }

		}

		public Table< EntryPoint> EntryPoints
		{
			get	{ return this.GetTable< EntryPoint>(); }

		}

		public Table< EnvelopeOption> EnvelopeOptions
		{
			get	{ return this.GetTable< EnvelopeOption>(); }

		}

		public Table< ExtraDatum> ExtraDatas
		{
			get	{ return this.GetTable< ExtraDatum>(); }

		}

		public Table< Family> Families
		{
			get	{ return this.GetTable< Family>(); }

		}

		public Table< FamilyMemberType> FamilyMemberTypes
		{
			get	{ return this.GetTable< FamilyMemberType>(); }

		}

		public Table< FamilyPosition> FamilyPositions
		{
			get	{ return this.GetTable< FamilyPosition>(); }

		}

		public Table< FamilyRelationship> FamilyRelationships
		{
			get	{ return this.GetTable< FamilyRelationship>(); }

		}

		public Table< Forum> Forums
		{
			get	{ return this.GetTable< Forum>(); }

		}

		public Table< ForumEntry> ForumEntries
		{
			get	{ return this.GetTable< ForumEntry>(); }

		}

		public Table< ForumNotify> ForumNotifications
		{
			get	{ return this.GetTable< ForumNotify>(); }

		}

		public Table< ForumUserRead> ForumUserReads
		{
			get	{ return this.GetTable< ForumUserRead>(); }

		}

		public Table< Gender> Genders
		{
			get	{ return this.GetTable< Gender>(); }

		}

		public Table< GenderClass> GenderClasses
		{
			get	{ return this.GetTable< GenderClass>(); }

		}

		public Table< GeoCode> GeoCodes
		{
			get	{ return this.GetTable< GeoCode>(); }

		}

		public Table< Group> Groups
		{
			get	{ return this.GetTable< Group>(); }

		}

		public Table< GroupRole> GroupRoles
		{
			get	{ return this.GetTable< GroupRole>(); }

		}

		public Table< InterestPoint> InterestPoints
		{
			get	{ return this.GetTable< InterestPoint>(); }

		}

		public Table< Invitation> Invitations
		{
			get	{ return this.GetTable< Invitation>(); }

		}

		public Table< JoinType> JoinTypes
		{
			get	{ return this.GetTable< JoinType>(); }

		}

		public Table< LoveRespect> LoveRespects
		{
			get	{ return this.GetTable< LoveRespect>(); }

		}

		public Table< MaritalStatus> MaritalStatuses
		{
			get	{ return this.GetTable< MaritalStatus>(); }

		}

		public Table< Meeting> Meetings
		{
			get	{ return this.GetTable< Meeting>(); }

		}

		public Table< MeetingType> MeetingTypes
		{
			get	{ return this.GetTable< MeetingType>(); }

		}

		public Table< MemberLetterStatus> MemberLetterStatuses
		{
			get	{ return this.GetTable< MemberLetterStatus>(); }

		}

		public Table< MemberStatus> MemberStatuses
		{
			get	{ return this.GetTable< MemberStatus>(); }

		}

		public Table< MemberTag> MemberTags
		{
			get	{ return this.GetTable< MemberTag>(); }

		}

		public Table< MemberType> MemberTypes
		{
			get	{ return this.GetTable< MemberType>(); }

		}

		public Table< Ministry> Ministries
		{
			get	{ return this.GetTable< Ministry>(); }

		}

		public Table< MOBSReg> MOBSRegs
		{
			get	{ return this.GetTable< MOBSReg>(); }

		}

		public Table< NameSuffix> NameSuffixes
		{
			get	{ return this.GetTable< NameSuffix>(); }

		}

		public Table< NameTitle> NameTitles
		{
			get	{ return this.GetTable< NameTitle>(); }

		}

		public Table< NewContact> NewContacts
		{
			get	{ return this.GetTable< NewContact>(); }

		}

		public Table< NewContactReason> NewContactReasons
		{
			get	{ return this.GetTable< NewContactReason>(); }

		}

		public Table< NewContactType> NewContactTypes
		{
			get	{ return this.GetTable< NewContactType>(); }

		}

		public Table< OrganizationMember> OrganizationMembers
		{
			get	{ return this.GetTable< OrganizationMember>(); }

		}

		public Table< Organization> Organizations
		{
			get	{ return this.GetTable< Organization>(); }

		}

		public Table< OrganizationStatus> OrganizationStatuses
		{
			get	{ return this.GetTable< OrganizationStatus>(); }

		}

		public Table< OrgMemMemTag> OrgMemMemTags
		{
			get	{ return this.GetTable< OrgMemMemTag>(); }

		}

		public Table< Origin> Origins
		{
			get	{ return this.GetTable< Origin>(); }

		}

		public Table< OtherNotify> OtherNotifications
		{
			get	{ return this.GetTable< OtherNotify>(); }

		}

		public Table< PageContent> PageContents
		{
			get	{ return this.GetTable< PageContent>(); }

		}

		public Table< PageVisit> PageVisits
		{
			get	{ return this.GetTable< PageVisit>(); }

		}

		public Table< ParaContent> ParaContents
		{
			get	{ return this.GetTable< ParaContent>(); }

		}

		public Table< PendingNotification> PendingNotifications
		{
			get	{ return this.GetTable< PendingNotification>(); }

		}

		public Table< Person> People
		{
			get	{ return this.GetTable< Person>(); }

		}

		public Table< PeopleExtra> PeopleExtras
		{
			get	{ return this.GetTable< PeopleExtra>(); }

		}

		public Table< PhonePreference> PhonePreferences
		{
			get	{ return this.GetTable< PhonePreference>(); }

		}

		public Table< Picture> Pictures
		{
			get	{ return this.GetTable< Picture>(); }

		}

		public Table< PodCast> PodCasts
		{
			get	{ return this.GetTable< PodCast>(); }

		}

		public Table< PostalLookup> PostalLookups
		{
			get	{ return this.GetTable< PostalLookup>(); }

		}

		public Table< PrayerSlot> PrayerSlots
		{
			get	{ return this.GetTable< PrayerSlot>(); }

		}

		public Table< Preference> Preferences
		{
			get	{ return this.GetTable< Preference>(); }

		}

		public Table< Program> Programs
		{
			get	{ return this.GetTable< Program>(); }

		}

		public Table< Promotion> Promotions
		{
			get	{ return this.GetTable< Promotion>(); }

		}

		public Table< QueryBuilderClause> QueryBuilderClauses
		{
			get	{ return this.GetTable< QueryBuilderClause>(); }

		}

		public Table< ReadPlan> ReadPlans
		{
			get	{ return this.GetTable< ReadPlan>(); }

		}

		public Table< RecLeague> RecLeagues
		{
			get	{ return this.GetTable< RecLeague>(); }

		}

		public Table< RecReg> RecRegs
		{
			get	{ return this.GetTable< RecReg>(); }

		}

		public Table< RegistrationType> RegistrationTypes
		{
			get	{ return this.GetTable< RegistrationType>(); }

		}

		public Table< RelatedFamily> RelatedFamilies
		{
			get	{ return this.GetTable< RelatedFamily>(); }

		}

		public Table< ResidentCode> ResidentCodes
		{
			get	{ return this.GetTable< ResidentCode>(); }

		}

		public Table< Role> Roles
		{
			get	{ return this.GetTable< Role>(); }

		}

		public Table< RssFeed> RssFeeds
		{
			get	{ return this.GetTable< RssFeed>(); }

		}

		public Table< SaleItem> SaleItems
		{
			get	{ return this.GetTable< SaleItem>(); }

		}

		public Table< SaleTransaction> SaleTransactions
		{
			get	{ return this.GetTable< SaleTransaction>(); }

		}

		public Table< Setting> Settings
		{
			get	{ return this.GetTable< Setting>(); }

		}

		public Table< ShirtSize> ShirtSizes
		{
			get	{ return this.GetTable< ShirtSize>(); }

		}

		public Table< SoulMate> SoulMates
		{
			get	{ return this.GetTable< SoulMate>(); }

		}

		public Table< StateLookup> StateLookups
		{
			get	{ return this.GetTable< StateLookup>(); }

		}

		public Table< Tag> Tags
		{
			get	{ return this.GetTable< Tag>(); }

		}

		public Table< TagPerson> TagPeople
		{
			get	{ return this.GetTable< TagPerson>(); }

		}

		public Table< TagShare> TagShares
		{
			get	{ return this.GetTable< TagShare>(); }

		}

		public Table< TagType> TagTypes
		{
			get	{ return this.GetTable< TagType>(); }

		}

		public Table< Task> Tasks
		{
			get	{ return this.GetTable< Task>(); }

		}

		public Table< TaskList> TaskLists
		{
			get	{ return this.GetTable< TaskList>(); }

		}

		public Table< TaskListOwner> TaskListOwners
		{
			get	{ return this.GetTable< TaskListOwner>(); }

		}

		public Table< TaskStatus> TaskStatuses
		{
			get	{ return this.GetTable< TaskStatus>(); }

		}

		public Table< TemporaryToken> TemporaryTokens
		{
			get	{ return this.GetTable< TemporaryToken>(); }

		}

		public Table< UploadAuthenticationXref> UploadAuthenticationXrefs
		{
			get	{ return this.GetTable< UploadAuthenticationXref>(); }

		}

		public Table< UserCanEmailFor> UserCanEmailFors
		{
			get	{ return this.GetTable< UserCanEmailFor>(); }

		}

		public Table< UserGroupRole> UserGroupRoles
		{
			get	{ return this.GetTable< UserGroupRole>(); }

		}

		public Table< UserRole> UserRoles
		{
			get	{ return this.GetTable< UserRole>(); }

		}

		public Table< User> Users
		{
			get	{ return this.GetTable< User>(); }

		}

		public Table< VBSApp> VBSApps
		{
			get	{ return this.GetTable< VBSApp>(); }

		}

		public Table< Verse> Verses
		{
			get	{ return this.GetTable< Verse>(); }

		}

		public Table< VerseCategory> VerseCategories
		{
			get	{ return this.GetTable< VerseCategory>(); }

		}

		public Table< VerseCategoryXref> VerseCategoryXrefs
		{
			get	{ return this.GetTable< VerseCategoryXref>(); }

		}

		public Table< VolApplicationStatus> VolApplicationStatuses
		{
			get	{ return this.GetTable< VolApplicationStatus>(); }

		}

		public Table< VolInterestCode> VolInterestCodes
		{
			get	{ return this.GetTable< VolInterestCode>(); }

		}

		public Table< VolInterestInterestCode> VolInterestInterestCodes
		{
			get	{ return this.GetTable< VolInterestInterestCode>(); }

		}

		public Table< Volunteer> Volunteers
		{
			get	{ return this.GetTable< Volunteer>(); }

		}

		public Table< VolunteerCode> VolunteerCodes
		{
			get	{ return this.GetTable< VolunteerCode>(); }

		}

		public Table< VolunteerForm> VolunteerForms
		{
			get	{ return this.GetTable< VolunteerForm>(); }

		}

		public Table< Word> Words
		{
			get	{ return this.GetTable< Word>(); }

		}

		public Table< ZipCode> ZipCodes
		{
			get	{ return this.GetTable< ZipCode>(); }

		}

		public Table< Zip> Zips
		{
			get	{ return this.GetTable< Zip>(); }

		}

	#endregion
	#region Views
		
	    public Table< View.BadETView> ViewBadETViews
	    {
		    get { return this.GetTable< View.BadETView>(); }

	    }

	    public Table< View.BlogCategoriesView> ViewBlogCategoriesViews
	    {
		    get { return this.GetTable< View.BlogCategoriesView>(); }

	    }

	    public Table< View.Church> ViewChurches
	    {
		    get { return this.GetTable< View.Church>(); }

	    }

	    public Table< View.City> ViewCities
	    {
		    get { return this.GetTable< View.City>(); }

	    }

	    public Table< View.ContributionsView> ViewContributionsViews
	    {
		    get { return this.GetTable< View.ContributionsView>(); }

	    }

	    public Table< View.FirstName> ViewFirstNames
	    {
		    get { return this.GetTable< View.FirstName>(); }

	    }

	    public Table< View.FirstName2> ViewFirstName2s
	    {
		    get { return this.GetTable< View.FirstName2>(); }

	    }

	    public Table< View.FirstNick> ViewFirstNicks
	    {
		    get { return this.GetTable< View.FirstNick>(); }

	    }

	    public Table< View.LastName> ViewLastNames
	    {
		    get { return this.GetTable< View.LastName>(); }

	    }

	    public Table< View.Nick> ViewNicks
	    {
		    get { return this.GetTable< View.Nick>(); }

	    }

	    public Table< View.PodcastSummary> ViewPodcastSummaries
	    {
		    get { return this.GetTable< View.PodcastSummary>(); }

	    }

	    public Table< View.Sproc> ViewSprocs
	    {
		    get { return this.GetTable< View.Sproc>(); }

	    }

	    public Table< View.Trigger> ViewTriggers
	    {
		    get { return this.GetTable< View.Trigger>(); }

	    }

	    public Table< View.UserList> ViewUserLists
	    {
		    get { return this.GetTable< View.UserList>(); }

	    }

	    public Table< View.VBSInfo> ViewVBSInfos
	    {
		    get { return this.GetTable< View.VBSInfo>(); }

	    }

	    public Table< View.VerseCategoriesView> ViewVerseCategoriesViews
	    {
		    get { return this.GetTable< View.VerseCategoriesView>(); }

	    }

	    public Table< View.VerseSummary> ViewVerseSummaries
	    {
		    get { return this.GetTable< View.VerseSummary>(); }

	    }

    #endregion
	#region Table Functions
		
		[Function(Name="dbo.AttendMemberTypeAsOf", IsComposable = true)]
		public IQueryable< View.AttendMemberTypeAsOf > AttendMemberTypeAsOf(
            [Parameter(DbType="datetime")] DateTime? from,
            [Parameter(DbType="datetime")] DateTime? to,
            [Parameter(DbType="int")] int? progid,
            [Parameter(DbType="int")] int? divid,
            [Parameter(DbType="int")] int? orgid,
            [Parameter(DbType="bit")] bool? not,
            [Parameter(DbType="varchar")] string ids
            )
		{
			return this.CreateMethodCallQuery< View.AttendMemberTypeAsOf>(this, 
			    ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                from,
                to,
                progid,
                divid,
                orgid,
                not,
                ids
                );
		}

		[Function(Name="dbo.BadEtsList", IsComposable = true)]
		public IQueryable< View.BadEtsList > BadEtsList(
            [Parameter(DbType="int")] int? flag
            )
		{
			return this.CreateMethodCallQuery< View.BadEtsList>(this, 
			    ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                flag
                );
		}

		[Function(Name="dbo.Contributors", IsComposable = true)]
		public IQueryable< View.Contributor > Contributors(
            [Parameter(DbType="datetime")] DateTime? fd,
            [Parameter(DbType="datetime")] DateTime? td,
            [Parameter(DbType="int")] int? pid,
            [Parameter(DbType="int")] int? spid,
            [Parameter(DbType="int")] int? fid
            )
		{
			return this.CreateMethodCallQuery< View.Contributor>(this, 
			    ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                fd,
                td,
                pid,
                spid,
                fid
                );
		}

		[Function(Name="dbo.CsvTable", IsComposable = true)]
		public IQueryable< View.CsvTable > CsvTable(
            [Parameter(DbType="varchar")] string csv
            )
		{
			return this.CreateMethodCallQuery< View.CsvTable>(this, 
			    ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                csv
                );
		}

		[Function(Name="dbo.MembersAsOf", IsComposable = true)]
		public IQueryable< View.MembersAsOf > MembersAsOf(
            [Parameter(DbType="datetime")] DateTime? from,
            [Parameter(DbType="datetime")] DateTime? to,
            [Parameter(DbType="int")] int? progid,
            [Parameter(DbType="int")] int? divid,
            [Parameter(DbType="int")] int? orgid
            )
		{
			return this.CreateMethodCallQuery< View.MembersAsOf>(this, 
			    ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                from,
                to,
                progid,
                divid,
                orgid
                );
		}

		[Function(Name="dbo.QBClauses", IsComposable = true)]
		public IQueryable< View.QBClause > QBClauses(
            [Parameter(DbType="int")] int? qid
            )
		{
			return this.CreateMethodCallQuery< View.QBClause>(this, 
			    ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                qid
                );
		}

		[Function(Name="dbo.TaggedPeople", IsComposable = true)]
		public IQueryable< View.TaggedPerson > TaggedPeople(
            [Parameter(DbType="int")] int? tagid
            )
		{
			return this.CreateMethodCallQuery< View.TaggedPerson>(this, 
			    ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                tagid
                );
		}

		[Function(Name="disc.VerseSummaryForCategory2", IsComposable = true)]
		public IQueryable< View.VerseSummaryForCategory2 > VerseSummaryForCategory2(
            [Parameter(DbType="int")] int? catid
            )
		{
			return this.CreateMethodCallQuery< View.VerseSummaryForCategory2>(this, 
			    ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                catid
                );
		}

    #endregion
	#region Scalar Functions
		
		[Function(Name="dbo.GetAttendedTodaysMeeting", IsComposable = true)]
		[return: Parameter(DbType = "bit")]
		public bool? GetAttendedTodaysMeeting(
            [Parameter(Name = "orgid", DbType="int")] int? orgid,
            [Parameter(Name = "thisday", DbType="int")] int? thisday,
            [Parameter(Name = "pid", DbType="int")] int? pid
            )
		{
			return ((Boolean)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                orgid,
                thisday,
                pid
                ).ReturnValue));
		}

		[Function(Name="dbo.GetTodaysMeetingHour", IsComposable = true)]
		[return: Parameter(DbType = "datetime")]
		public DateTime? GetTodaysMeetingHour(
            [Parameter(Name = "orgid", DbType="int")] int? orgid,
            [Parameter(Name = "thisday", DbType="int")] int? thisday
            )
		{
			return ((DateTime)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                orgid,
                thisday
                ).ReturnValue));
		}

		[Function(Name="dbo.GetTodaysMeetingId", IsComposable = true)]
		[return: Parameter(DbType = "int")]
		public int? GetTodaysMeetingId(
            [Parameter(Name = "orgid", DbType="int")] int? orgid,
            [Parameter(Name = "thisday", DbType="int")] int? thisday
            )
		{
			return ((Int32)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                orgid,
                thisday
                ).ReturnValue));
		}

		[Function(Name="dbo.AttendDesc", IsComposable = true)]
		[return: Parameter(DbType = "varchar")]
		public string AttendDesc(
            [Parameter(Name = "id", DbType="int")] int? id
            )
		{
			return ((String)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                id
                ).ReturnValue));
		}

		[Function(Name="dbo.MemberDesc", IsComposable = true)]
		[return: Parameter(DbType = "varchar")]
		public string MemberDesc(
            [Parameter(Name = "id", DbType="int")] int? id
            )
		{
			return ((String)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                id
                ).ReturnValue));
		}

		[Function(Name="dbo.fn_diagramobjects", IsComposable = true)]
		[return: Parameter(DbType = "int")]
		public int? FnDiagramobjects(
            )
		{
			return ((Int32)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod()))
                ).ReturnValue));
		}

		[Function(Name="dbo.WasDeaconActive2008", IsComposable = true)]
		[return: Parameter(DbType = "bit")]
		public bool? WasDeaconActive2008(
            [Parameter(Name = "pid", DbType="int")] int? pid,
            [Parameter(Name = "dt", DbType="datetime")] DateTime? dt
            )
		{
			return ((Boolean)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                pid,
                dt
                ).ReturnValue));
		}

		[Function(Name="dbo.LastAttend", IsComposable = true)]
		[return: Parameter(DbType = "datetime")]
		public DateTime? LastAttend(
            [Parameter(Name = "orgid", DbType="int")] int? orgid,
            [Parameter(Name = "pid", DbType="int")] int? pid
            )
		{
			return ((DateTime)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                orgid,
                pid
                ).ReturnValue));
		}

		[Function(Name="dbo.LastAttended", IsComposable = true)]
		[return: Parameter(DbType = "datetime")]
		public DateTime? LastAttended(
            [Parameter(Name = "orgid", DbType="int")] int? orgid,
            [Parameter(Name = "pid", DbType="int")] int? pid
            )
		{
			return ((DateTime)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                orgid,
                pid
                ).ReturnValue));
		}

		[Function(Name="dbo.BaptismAgeRange", IsComposable = true)]
		[return: Parameter(DbType = "varchar")]
		public string BaptismAgeRange(
            [Parameter(Name = "age", DbType="int")] int? age
            )
		{
			return ((String)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                age
                ).ReturnValue));
		}

		[Function(Name="dbo.Tool_VarbinaryToVarcharHex", IsComposable = true)]
		[return: Parameter(DbType = "varchar")]
		public string ToolVarbinaryToVarcharHex(
            [Parameter(Name = "VarbinaryValue", DbType="varbinary")] byte[] VarbinaryValue
            )
		{
			return ((String)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                VarbinaryValue
                ).ReturnValue));
		}

		[Function(Name="dbo.DaysSinceAttend", IsComposable = true)]
		[return: Parameter(DbType = "int")]
		public int? DaysSinceAttend(
            [Parameter(Name = "pid", DbType="int")] int? pid,
            [Parameter(Name = "oid", DbType="int")] int? oid
            )
		{
			return ((Int32)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                pid,
                oid
                ).ReturnValue));
		}

		[Function(Name="dbo.SundayForWeek", IsComposable = true)]
		[return: Parameter(DbType = "datetime")]
		public DateTime? SundayForWeek(
            [Parameter(Name = "year", DbType="int")] int? year,
            [Parameter(Name = "week", DbType="int")] int? week
            )
		{
			return ((DateTime)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                year,
                week
                ).ReturnValue));
		}

		[Function(Name="dbo.ScheduleId", IsComposable = true)]
		[return: Parameter(DbType = "int")]
		public int? ScheduleId(
            [Parameter(Name = "day", DbType="int")] int? day,
            [Parameter(Name = "time", DbType="datetime")] DateTime? time
            )
		{
			return ((Int32)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                day,
                time
                ).ReturnValue));
		}

		[Function(Name="dbo.GetScheduleTime", IsComposable = true)]
		[return: Parameter(DbType = "datetime")]
		public DateTime? GetScheduleTime(
            [Parameter(Name = "day", DbType="int")] int? day,
            [Parameter(Name = "time", DbType="datetime")] DateTime? time
            )
		{
			return ((DateTime)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                day,
                time
                ).ReturnValue));
		}

		[Function(Name="dbo.OrganizationMemberCount", IsComposable = true)]
		[return: Parameter(DbType = "int")]
		public int? OrganizationMemberCount(
            [Parameter(Name = "oid", DbType="int")] int? oid
            )
		{
			return ((Int32)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                oid
                ).ReturnValue));
		}

		[Function(Name="dbo.PersonAttendCountOrg", IsComposable = true)]
		[return: Parameter(DbType = "int")]
		public int? PersonAttendCountOrg(
            [Parameter(Name = "pid", DbType="int")] int? pid,
            [Parameter(Name = "oid", DbType="int")] int? oid
            )
		{
			return ((Int32)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                pid,
                oid
                ).ReturnValue));
		}

		[Function(Name="dbo.PrimaryAddress2", IsComposable = true)]
		[return: Parameter(DbType = "varchar")]
		public string PrimaryAddress2(
            [Parameter(Name = "pid", DbType="int")] int? pid
            )
		{
			return ((String)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                pid
                ).ReturnValue));
		}

		[Function(Name="dbo.GetEldestFamilyMember", IsComposable = true)]
		[return: Parameter(DbType = "int")]
		public int? GetEldestFamilyMember(
            [Parameter(Name = "fid", DbType="int")] int? fid
            )
		{
			return ((Int32)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                fid
                ).ReturnValue));
		}

		[Function(Name="dbo.Birthday", IsComposable = true)]
		[return: Parameter(DbType = "datetime")]
		public DateTime? Birthday(
            [Parameter(Name = "pid", DbType="int")] int? pid
            )
		{
			return ((DateTime)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                pid
                ).ReturnValue));
		}

		[Function(Name="disc.VersePos", IsComposable = true)]
		[return: Parameter(DbType = "int")]
		public int? VersePos(
            [Parameter(Name = "id", DbType="int")] int? id
            )
		{
			return ((Int32)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                id
                ).ReturnValue));
		}

		[Function(Name="dbo.HeadOfHouseholdId", IsComposable = true)]
		[return: Parameter(DbType = "int")]
		public int? HeadOfHouseholdId(
            [Parameter(Name = "familyid", DbType="int")] int? familyid
            )
		{
			return ((Int32)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                familyid
                ).ReturnValue));
		}

		[Function(Name="dbo.HeadOfHouseHoldSpouseId", IsComposable = true)]
		[return: Parameter(DbType = "int")]
		public int? HeadOfHouseHoldSpouseId(
            [Parameter(Name = "familyid", DbType="int")] int? familyid
            )
		{
			return ((Int32)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                familyid
                ).ReturnValue));
		}

		[Function(Name="dbo.CoupleFlag", IsComposable = true)]
		[return: Parameter(DbType = "int")]
		public int? CoupleFlag(
            [Parameter(Name = "familyid", DbType="int")] int? familyid
            )
		{
			return ((Int32)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                familyid
                ).ReturnValue));
		}

		[Function(Name="dbo.NextChangeTransactionId2", IsComposable = true)]
		[return: Parameter(DbType = "int")]
		public int? NextChangeTransactionId2(
            [Parameter(Name = "pid", DbType="int")] int? pid,
            [Parameter(Name = "oid", DbType="int")] int? oid,
            [Parameter(Name = "tid", DbType="int")] int? tid,
            [Parameter(Name = "typeid", DbType="int")] int? typeid
            )
		{
			return ((Int32)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                pid,
                oid,
                tid,
                typeid
                ).ReturnValue));
		}

		[Function(Name="dbo.PrimaryCity", IsComposable = true)]
		[return: Parameter(DbType = "varchar")]
		public string PrimaryCity(
            [Parameter(Name = "pid", DbType="int")] int? pid
            )
		{
			return ((String)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                pid
                ).ReturnValue));
		}

		[Function(Name="dbo.PrimaryZip", IsComposable = true)]
		[return: Parameter(DbType = "varchar")]
		public string PrimaryZip(
            [Parameter(Name = "pid", DbType="int")] int? pid
            )
		{
			return ((String)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                pid
                ).ReturnValue));
		}

		[Function(Name="dbo.BibleFellowshipClassId", IsComposable = true)]
		[return: Parameter(DbType = "int")]
		public int? BibleFellowshipClassId(
            [Parameter(Name = "pid", DbType="int")] int? pid
            )
		{
			return ((Int32)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                pid
                ).ReturnValue));
		}

		[Function(Name="dbo.SpouseId", IsComposable = true)]
		[return: Parameter(DbType = "int")]
		public int? SpouseId(
            [Parameter(Name = "peopleid", DbType="int")] int? peopleid
            )
		{
			return ((Int32)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                peopleid
                ).ReturnValue));
		}

		[Function(Name="disc.VerseInCategory", IsComposable = true)]
		[return: Parameter(DbType = "bit")]
		public bool? VerseInCategory(
            [Parameter(Name = "vid", DbType="int")] int? vid,
            [Parameter(Name = "catid", DbType="int")] int? catid
            )
		{
			return ((Boolean)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                vid,
                catid
                ).ReturnValue));
		}

		[Function(Name="dbo.Age", IsComposable = true)]
		[return: Parameter(DbType = "int")]
		public int? Age(
            [Parameter(Name = "pid", DbType="int")] int? pid
            )
		{
			return ((Int32)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                pid
                ).ReturnValue));
		}

		[Function(Name="dbo.PrimaryResCode", IsComposable = true)]
		[return: Parameter(DbType = "int")]
		public int? PrimaryResCode(
            [Parameter(Name = "pid", DbType="int")] int? pid
            )
		{
			return ((Int32)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                pid
                ).ReturnValue));
		}

		[Function(Name="dbo.EntryPointId", IsComposable = true)]
		[return: Parameter(DbType = "int")]
		public int? EntryPointId(
            [Parameter(Name = "pid", DbType="int")] int? pid
            )
		{
			return ((Int32)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                pid
                ).ReturnValue));
		}

		[Function(Name="dbo.PrimaryBadAddressFlag", IsComposable = true)]
		[return: Parameter(DbType = "int")]
		public int? PrimaryBadAddressFlag(
            [Parameter(Name = "pid", DbType="int")] int? pid
            )
		{
			return ((Int32)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                pid
                ).ReturnValue));
		}

		[Function(Name="dbo.NextTranChangeDate", IsComposable = true)]
		[return: Parameter(DbType = "datetime")]
		public DateTime? NextTranChangeDate(
            [Parameter(Name = "pid", DbType="int")] int? pid,
            [Parameter(Name = "oid", DbType="int")] int? oid,
            [Parameter(Name = "tid", DbType="int")] int? tid,
            [Parameter(Name = "typeid", DbType="int")] int? typeid
            )
		{
			return ((DateTime)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                pid,
                oid,
                tid,
                typeid
                ).ReturnValue));
		}

		[Function(Name="dbo.NextChangeTransactionId", IsComposable = true)]
		[return: Parameter(DbType = "int")]
		public int? NextChangeTransactionId(
            [Parameter(Name = "pid", DbType="int")] int? pid,
            [Parameter(Name = "oid", DbType="int")] int? oid,
            [Parameter(Name = "tid", DbType="int")] int? tid,
            [Parameter(Name = "typeid", DbType="int")] int? typeid
            )
		{
			return ((Int32)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                pid,
                oid,
                tid,
                typeid
                ).ReturnValue));
		}

		[Function(Name="dbo.PrimaryAddress", IsComposable = true)]
		[return: Parameter(DbType = "varchar")]
		public string PrimaryAddress(
            [Parameter(Name = "pid", DbType="int")] int? pid
            )
		{
			return ((String)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                pid
                ).ReturnValue));
		}

		[Function(Name="dbo.LastNameCount", IsComposable = true)]
		[return: Parameter(DbType = "int")]
		public int? LastNameCount(
            [Parameter(Name = "last", DbType="varchar")] string last
            )
		{
			return ((Int32)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                last
                ).ReturnValue));
		}

		[Function(Name="dbo.PrimaryState", IsComposable = true)]
		[return: Parameter(DbType = "varchar")]
		public string PrimaryState(
            [Parameter(Name = "pid", DbType="int")] int? pid
            )
		{
			return ((String)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                pid
                ).ReturnValue));
		}

		[Function(Name="dbo.HomePhone", IsComposable = true)]
		[return: Parameter(DbType = "varchar")]
		public string HomePhone(
            [Parameter(Name = "pid", DbType="int")] int? pid
            )
		{
			return ((String)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                pid
                ).ReturnValue));
		}

		[Function(Name="dbo.MemberStatusDescription", IsComposable = true)]
		[return: Parameter(DbType = "varchar")]
		public string MemberStatusDescription(
            [Parameter(Name = "pid", DbType="int")] int? pid
            )
		{
			return ((String)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                pid
                ).ReturnValue));
		}

		[Function(Name="dbo.OrganizationLeaderId", IsComposable = true)]
		[return: Parameter(DbType = "int")]
		public int? OrganizationLeaderId(
            [Parameter(Name = "orgid", DbType="int")] int? orgid
            )
		{
			return ((Int32)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                orgid
                ).ReturnValue));
		}

		[Function(Name="dbo.UserName", IsComposable = true)]
		[return: Parameter(DbType = "varchar")]
		public string UserName(
            [Parameter(Name = "pid", DbType="int")] int? pid
            )
		{
			return ((String)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                pid
                ).ReturnValue));
		}

		[Function(Name="dbo.EnrollmentTransactionId", IsComposable = true)]
		[return: Parameter(DbType = "int")]
		public int? EnrollmentTransactionId(
            [Parameter(Name = "pid", DbType="int")] int? pid,
            [Parameter(Name = "oid", DbType="int")] int? oid,
            [Parameter(Name = "tid", DbType="int")] int? tid,
            [Parameter(Name = "ttid", DbType="int")] int? ttid
            )
		{
			return ((Int32)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                pid,
                oid,
                tid,
                ttid
                ).ReturnValue));
		}

		[Function(Name="dbo.NextBirthday", IsComposable = true)]
		[return: Parameter(DbType = "datetime")]
		public DateTime? NextBirthday(
            [Parameter(Name = "pid", DbType="int")] int? pid
            )
		{
			return ((DateTime)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                pid
                ).ReturnValue));
		}

		[Function(Name="dbo.OrganizationLeaderName", IsComposable = true)]
		[return: Parameter(DbType = "varchar")]
		public string OrganizationLeaderName(
            [Parameter(Name = "orgid", DbType="int")] int? orgid
            )
		{
			return ((String)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                orgid
                ).ReturnValue));
		}

		[Function(Name="dbo.DayAndTime", IsComposable = true)]
		[return: Parameter(DbType = "varchar")]
		public string DayAndTime(
            [Parameter(Name = "dt", DbType="datetime")] DateTime? dt
            )
		{
			return ((String)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                dt
                ).ReturnValue));
		}

		[Function(Name="dbo.DollarRange", IsComposable = true)]
		[return: Parameter(DbType = "int")]
		public int? DollarRange(
            [Parameter(Name = "amt", DbType="decimal")] decimal? amt
            )
		{
			return ((Int32)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                amt
                ).ReturnValue));
		}

		[Function(Name="dbo.FirstMondayOfMonth", IsComposable = true)]
		[return: Parameter(DbType = "datetime")]
		public DateTime? FirstMondayOfMonth(
            [Parameter(Name = "inputDate", DbType="datetime")] DateTime? inputDate
            )
		{
			return ((DateTime)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                inputDate
                ).ReturnValue));
		}

		[Function(Name="dbo.StartsLower", IsComposable = true)]
		[return: Parameter(DbType = "bit")]
		public bool? StartsLower(
            [Parameter(Name = "s", DbType="nvarchar")] string s
            )
		{
			return ((Boolean)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                s
                ).ReturnValue));
		}

		[Function(Name="dbo.LastContact", IsComposable = true)]
		[return: Parameter(DbType = "datetime")]
		public DateTime? LastContact(
            [Parameter(Name = "pid", DbType="int")] int? pid
            )
		{
			return ((DateTime)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                pid
                ).ReturnValue));
		}

		[Function(Name="dbo.DaysSinceContact", IsComposable = true)]
		[return: Parameter(DbType = "int")]
		public int? DaysSinceContact(
            [Parameter(Name = "pid", DbType="int")] int? pid
            )
		{
			return ((Int32)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                pid
                ).ReturnValue));
		}

		[Function(Name="dbo.VBSOrg", IsComposable = true)]
		[return: Parameter(DbType = "int")]
		public int? VBSOrg(
            [Parameter(Name = "pid", DbType="int")] int? pid
            )
		{
			return ((Int32)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                pid
                ).ReturnValue));
		}

		[Function(Name="dbo.OrganizationMemberCount2", IsComposable = true)]
		[return: Parameter(DbType = "int")]
		public int? OrganizationMemberCount2(
            [Parameter(Name = "oid", DbType="int")] int? oid
            )
		{
			return ((Int32)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                oid
                ).ReturnValue));
		}

		[Function(Name="dbo.SchoolGrade", IsComposable = true)]
		[return: Parameter(DbType = "int")]
		public int? SchoolGrade(
            [Parameter(Name = "pid", DbType="int")] int? pid
            )
		{
			return ((Int32)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                pid
                ).ReturnValue));
		}

		[Function(Name="dbo.WidowedDate", IsComposable = true)]
		[return: Parameter(DbType = "datetime")]
		public DateTime? WidowedDate(
            [Parameter(Name = "peopleid", DbType="int")] int? peopleid
            )
		{
			return ((DateTime)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                peopleid
                ).ReturnValue));
		}

		[Function(Name="dbo.UEmail", IsComposable = true)]
		[return: Parameter(DbType = "varchar")]
		public string UEmail(
            [Parameter(Name = "pid", DbType="int")] int? pid
            )
		{
			return ((String)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                pid
                ).ReturnValue));
		}

		[Function(Name="disc.DayOfYear", IsComposable = true)]
		[return: Parameter(DbType = "int")]
		public int? DayOfYear(
            [Parameter(Name = "DateX", DbType="datetime")] DateTime? DateX
            )
		{
			return ((Int32)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                DateX
                ).ReturnValue));
		}

		[Function(Name="disc.GetProfile", IsComposable = true)]
		[return: Parameter(DbType = "varchar")]
		public string GetProfile(
            [Parameter(Name = "Username", DbType="varchar")] string Username,
            [Parameter(Name = "PropertyName", DbType="varchar")] string PropertyName
            )
		{
			return ((String)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                Username,
                PropertyName
                ).ReturnValue));
		}

		[Function(Name="dbo.UName2", IsComposable = true)]
		[return: Parameter(DbType = "varchar")]
		public string UName2(
            [Parameter(Name = "pid", DbType="int")] int? pid
            )
		{
			return ((String)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                pid
                ).ReturnValue));
		}

		[Function(Name="dbo.OneHeadOfHouseholdIsMember", IsComposable = true)]
		[return: Parameter(DbType = "bit")]
		public bool? OneHeadOfHouseholdIsMember(
            [Parameter(Name = "fid", DbType="int")] int? fid
            )
		{
			return ((Boolean)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                fid
                ).ReturnValue));
		}

		[Function(Name="dbo.UName", IsComposable = true)]
		[return: Parameter(DbType = "varchar")]
		public string UName(
            [Parameter(Name = "pid", DbType="int")] int? pid
            )
		{
			return ((String)(this.ExecuteMethodCall(this, 
                ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                pid
                ).ReturnValue));
		}

    #endregion
	#region Stored Procedures
		
		[Function(Name="dbo.ForumNewEntry")]
		public ISingleResult< ForumEntry> ForumNewEntry(
            [Parameter(Name = "forumid", DbType="int")] int? forumid,
            [Parameter(Name = "replytoid", DbType="int")] int? replytoid,
            [Parameter(Name = "title", DbType="nvarchar")] string title,
            [Parameter(Name = "entry", DbType="text")] string entry,
            [Parameter(Name = "created", DbType="datetime")] DateTime? created,
            [Parameter(Name = "createdby", DbType="nvarchar")] string createdby
            )
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())),
                forumid,
                replytoid,
                title,
                entry,
                created,
                createdby
			);
			return ((ISingleResult< ForumEntry>)(result.ReturnValue));
		}

    #endregion
   }

}

