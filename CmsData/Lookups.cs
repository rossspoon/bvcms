using System;
using System.Linq;
using UtilityExtensions;
using System.Text;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Linq.Expressions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CmsData
{
    [MetadataType(typeof(AddressTypeMeta))]
    [ScaffoldTable(true)]
    partial class AddressType
    {
    }
    class AddressTypeMeta
    {
        [ScaffoldColumn(false)]
        public EntitySet<Person> People;
    }
    //--------------------------------------------------------------------
    [ScaffoldTable(true)]
    partial class AttendanceClassification
    {
    }
    [MetadataType(typeof(AttendTrackLevelMeta))]
    [ScaffoldTable(true)]
    partial class AttendTrackLevel
    {
    }
    class AttendTrackLevelMeta
    {
        [ScaffoldColumn(false)]
        public EntitySet<Person> Organizations;
    }
    //--------------------------------------------------------------------
    [MetadataType(typeof(AttendTypeMeta))]
    [ScaffoldTable(true)]
    partial class AttendType
    {
    }
    class AttendTypeMeta
    {
        [ScaffoldColumn(false)]
        public EntitySet<Attend> Attends;
    }
    //--------------------------------------------------------------------
    [MetadataType(typeof(BaptismStatusMeta))]
    [ScaffoldTable(true)]
    partial class BaptismStatus
    {
    }
    class BaptismStatusMeta
    {
        [ScaffoldColumn(false)]
        public EntitySet<Person> People;
    }
    //--------------------------------------------------------------------
    [MetadataType(typeof(EntryPointMeta))]
    [ScaffoldTable(true)]
    partial class EntryPoint
    {
    }
    class EntryPointMeta
    {
        [ScaffoldColumn(false)]
        public EntitySet<Person> People;
        [ScaffoldColumn(false)]
        public EntitySet<Organization> Organizations;
    }
    //--------------------------------------------------------------------
    [MetadataType(typeof(EnvelopeOptionMeta))]
    [ScaffoldTable(true)]
    partial class EnvelopeOption
    {
    }
    class EnvelopeOptionMeta
    {
        [ScaffoldColumn(false)]
        public EntitySet<Person> People;
    }
    //--------------------------------------------------------------------
      [MetadataType(typeof(FamilyMemberTypeMeta))]
  [ScaffoldTable(true)]
    partial class FamilyMemberType
    {
    }
    class FamilyMemberTypeMeta
    {
        //[ScaffoldColumn(false)]
        //public EntitySet<Person> People;
    }
    //--------------------------------------------------------------------
    [ScaffoldTable(true)]
    partial class FamilyPosition
    {
    }
    //--------------------------------------------------------------------
    [ScaffoldTable(true)]
    partial class FamilyRelationship
    {
    }
    //--------------------------------------------------------------------
    [MetadataType(typeof(GenderMeta))]
    [ScaffoldTable(true)]
    partial class Gender
    {
    }
    class GenderMeta
    {
        [ScaffoldColumn(false)]
        public EntitySet<Person> People;
    }
    //--------------------------------------------------------------------
    [MetadataType(typeof(BundleHeaderTypeMeta))]
    [ScaffoldTable(true)]
    partial class BundleHeaderType
    {
    }
    class BundleHeaderTypeMeta
    {
        [ScaffoldColumn(false)]
        public EntitySet<BundleHeader> BundleHeaders;
    }
    //--------------------------------------------------------------------
    [MetadataType(typeof(BundleStatusTypeMeta))]
    [ScaffoldTable(true)]
    partial class BundleStatusType
    {
    }
    class BundleStatusTypeMeta
    {
        [ScaffoldColumn(false)]
        public EntitySet<BundleHeader> BundleHeaders;
    }
    //--------------------------------------------------------------------
    [ScaffoldTable(true)]
    partial class ContactPreference
    {
    }
    //--------------------------------------------------------------------
    [MetadataType(typeof(ContributionStatusMeta))]
    [ScaffoldTable(true)]
    partial class ContributionStatus
    {
    }
    class ContributionStatusMeta
    {
        [ScaffoldColumn(false)]
        public EntitySet<Contribution> Contributions;
    }
    //--------------------------------------------------------------------
    [MetadataType(typeof(ContributionTypeMeta))]
    [ScaffoldTable(true)]
    partial class ContributionType
    {
    }
    class ContributionTypeMeta
    {
        [ScaffoldColumn(false)]
        public EntitySet<Contribution> Contributions;
    }
    //--------------------------------------------------------------------
    [ScaffoldTable(true)]
    partial class CountryLookup
    {
    }
    //--------------------------------------------------------------------
    [MetadataType(typeof(DecisionTypeMeta))]
    [ScaffoldTable(true)]
    partial class DecisionType
    {
    }
    class DecisionTypeMeta
    {
        [ScaffoldColumn(false)]
        public EntitySet<Person> People;
    }
    //--------------------------------------------------------------------
    [MetadataType(typeof(DiscoveryClassStatusMeta))]
    [ScaffoldTable(true)]
    partial class DiscoveryClassStatus
    {
    }
    class DiscoveryClassStatusMeta
    {
        [ScaffoldColumn(false)]
        public EntitySet<Person> People;
    }
    //--------------------------------------------------------------------
    [MetadataType(typeof(DropTypeMeta))]
    [ScaffoldTable(true)]
    partial class DropType
    {
    }
    class DropTypeMeta
    {
        [ScaffoldColumn(false)]
        public EntitySet<Person> People;
    }
    //--------------------------------------------------------------------
    [MetadataType(typeof(InterestPointMeta))]
    [ScaffoldTable(true)]
    partial class InterestPoint
    {
    }
    class InterestPointMeta
    {
        [ScaffoldColumn(false)]
        public EntitySet<Person> People;
    }
    //--------------------------------------------------------------------
    [MetadataType(typeof(JoinTypeMeta))]
    [ScaffoldTable(true)]
    partial class JoinType
    {
    }
    class JoinTypeMeta
    {
        [ScaffoldColumn(false)]
        public EntitySet<Person> People;
    }
    //--------------------------------------------------------------------
     [MetadataType(typeof(MaritalStatusMeta))]
   [ScaffoldTable(true)]
    partial class MaritalStatus
    {
    }
    class MaritalStatusMeta
    {
        [ScaffoldColumn(false)]
        public EntitySet<Person> People;
    }
    //--------------------------------------------------------------------
    [ScaffoldTable(true)]
    partial class MeetingType
    {
    }
    //--------------------------------------------------------------------
    [MetadataType(typeof(MemberLetterStatusMeta))]
    [ScaffoldTable(true)]
    partial class MemberLetterStatus
    {
    }
    class MemberLetterStatusMeta
    {
        [ScaffoldColumn(false)]
        public EntitySet<Person> People;
    }
    //--------------------------------------------------------------------
     [MetadataType(typeof(MemberStatusMeta))]
   [ScaffoldTable(true)]
    partial class MemberStatus
    {
    }
    class MemberStatusMeta
    {
        [ScaffoldColumn(false)]
        public EntitySet<Person> People;
    }
    //--------------------------------------------------------------------
     [MetadataType(typeof(MemberTypeMeta))]
   [ScaffoldTable(true)]
    partial class MemberType
    {
    }
    class MemberTypeMeta
    {
        [ScaffoldColumn(false)]
        public EntitySet<Attend> Attends;
        [ScaffoldColumn(false)]
        public EntitySet<EnrollmentTransaction> EnrollmentTransactions;
        [ScaffoldColumn(false)]
        public EntitySet<OrganizationMember> OrganizationMembers;
    }
    //--------------------------------------------------------------------
    [ScaffoldTable(true)]
    partial class NameSuffix
    {
    }
    //--------------------------------------------------------------------
    [ScaffoldTable(true)]
    partial class NameTitle
    {
    }
    //--------------------------------------------------------------------
    [MetadataType(typeof(NewContactReasonMeta))]
    [ScaffoldTable(true)]
    partial class NewContactReason
    {
    }
    class NewContactReasonMeta
    {
        [ScaffoldColumn(false)]
        public EntitySet<NewContact> NewContacts;
    }
    //--------------------------------------------------------------------
    [MetadataType(typeof(NewContactTypeMeta))]
    [ScaffoldTable(true)]
    partial class NewContactType
    {
    }
    class NewContactTypeMeta
    {
        [ScaffoldColumn(false)]
        public EntitySet<NewContact> NewContacts;
    }
    //--------------------------------------------------------------------
    [MetadataType(typeof(OrganizationStatusMeta))]
    [ScaffoldTable(true)]
    partial class OrganizationStatus
    {
    }
    class OrganizationStatusMeta
    {
        [ScaffoldColumn(false)]
        public EntitySet<Organization> Organizations;
    }
    //--------------------------------------------------------------------
    [MetadataType(typeof(OriginMeta))]
    [ScaffoldTable(true)]
    partial class Origin
    {
    }
    class OriginMeta
    {
        [ScaffoldColumn(false)]
        public EntitySet<Person> People;
    }
    //--------------------------------------------------------------------
    [ScaffoldTable(true)]
     [MetadataType(typeof(PhonePreferenceMeta))]
   partial class PhonePreference
    {
    }
    class PhonePreferenceMeta
    {
        [ScaffoldColumn(false)]
        public EntitySet<Person> People;
    }
    //--------------------------------------------------------------------
    [ScaffoldTable(true)]
    partial class PostalLookup
    {
    }
    //--------------------------------------------------------------------
    [ScaffoldTable(true)]
    partial class ResidentCode
    {
    }
    //--------------------------------------------------------------------
    [ScaffoldTable(true)]
    partial class StateLookup
    {
    }
    //--------------------------------------------------------------------
    [MetadataType(typeof(TaskStatusMeta))]
    [ScaffoldTable(true)]
    partial class TaskStatus
    {
    }
    class TaskStatusMeta
    {
        [ScaffoldColumn(false)]
        public EntitySet<Task> Tasks;
    }
    //--------------------------------------------------------------------
    [MetadataType(typeof(VolApplicationStatusMeta))]
    [ScaffoldTable(true)]
    partial class VolApplicationStatus
    {
    }
    class VolApplicationStatusMeta
    {
        [ScaffoldColumn(false)]
        public EntitySet<Volunteer> Volunteers;
    }
    //--------------------------------------------------------------------
    [ScaffoldTable(true)]
    partial class VolunteerCode
    {
    }
    //--------------------------------------------------------------------
    [MetadataType(typeof(WeeklyScheduleMeta))]
    [ScaffoldTable(true)]
    partial class WeeklySchedule
    {
    }
    class WeeklyScheduleMeta
    {
        [ScaffoldColumn(false)]
        public EntitySet<Organization> Organizations;
    }
    //--------------------------------------------------------------------

    [MetadataType(typeof(TagTypeMeta))]
    [ScaffoldTable(true)]
    partial class TagType
    {
    }
    class TagTypeMeta
    {
        [ScaffoldColumn(false)]
        public EntitySet<Tag> Tags;
    }
    //--------------------------------------------------------------------
    [MetadataType(typeof(ProgramMeta))]
    [ScaffoldTable(true)]
    partial class Program
    {
    }
    class ProgramMeta
    {
        [ScaffoldColumn(false)]
        public EntitySet<Division> Divisions;
        [ScaffoldColumn(false)]
        public EntitySet<MemberTag> MemberTags;
    }
    //--------------------------------------------------------------------
    [MetadataType(typeof(DivisionMeta))]
    [ScaffoldTable(true)]
    partial class Division
    {
    }
    class DivisionMeta
    {
        [ScaffoldColumn(false)]
        public EntitySet<DivOrg> DivOrgs;
        [ScaffoldColumn(false)]
        public EntityRef<Program> Program;
    }

}

