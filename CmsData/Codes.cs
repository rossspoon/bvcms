using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CmsData.Codes
{
    public class OriginCode
    {
        public const int Visit = 10;
        public const int Request = 40;
        public const int PhoneIn = 50;
        public const int SurveyEE = 60;
        public const int Enrollment = 70;
        public const int Contribution = 90;
        public const int NewFamilyMember = 100;
    }
    public static class DecisionCode
    {
        public const int Unknown = 0;
        public const int ProfessionForMembership = 10;
        public const int ProfessionNotForMembership = 20;
        public const int Letter = 30;
        public const int Statement = 40;
        public const int StatementReqBaptism = 50;
        public const int Cancelled = 60;
    }
    public static class MemberStatusCode
    {
        public const int Member = 10;
        public const int NotMember = 20;
        public const int Pending = 30;
        public const int Previous = 40;
        public const int JustAdded = 50;
    }
    public static class NewMemberClassStatusCode
    {
        public const int NotSpecified = 0;
        public const int Pending = 10;
        public const int Attended = 20;
        public const int AdminApproval = 30;
        public const int GrandFathered = 40;
        public const int ExemptedChild = 50;
        public const int Unknown = 9;
    }
    public static class BaptismTypeCode
    {
        public const int NotSpecified = 0;
        public const int Original = 10; // first time at pof
        public const int Subsequent = 20; // already member
        public const int Biological = 30; // children of members
        public const int NonMember = 40; // pof, baptism, but not joining
        public const int Required = 50; // statement, not dunked
    }
    public static class BaptismStatusCode
    {
        public const int NotSpecified = 0;
        public const int Scheduled = 10;
        public const int NotScheduled = 20;
        public const int Completed = 30;
        public const int Canceled = 40;
    }
    public static class JoinTypeCode
    {
        public const int Unknown = 0;
        public const int BaptismPOF = 10;
        public const int BaptismSRB = 20;
        public const int BaptismBIO = 30;
        public const int Statement = 40;
        public const int Letter = 50;
    }
    public static class DropTypeCode
    {
        public const int Administrative = 20;
        public const int AnotherDenomination = 60;
        public const int LetteredOut = 40;
        public const int Requested = 50;
        public const int Other = 98;
        public const int NotDropped = 0;
        public const int Duplicate = 10;
        public const int Deceased = 30;
    }
    public static class EnvelopeOptionCode
    {
        public const int None = 9;
        public const int Individual = 1;
        public const int Joint = 2;
    }
    public static class MaritalStatusCode
    {
        public const int Unknown = 0;
        public const int Single = 10;
        public const int Married = 20;
        public const int Separated = 30;
        public const int Divorced = 40;
        public const int Widowed = 50;
    }
    public static class PositionInFamily
    {
        public const int PrimaryAdult = 10;
        public const int SecondaryAdult = 20;
        public const int Child = 30;
    }
    public static class MemberTypeCode
    {
        public const int Administrator = 100;
        public const int President = 101;
        public const int Leader = 140;
        public const int AssistantLeader = 142;
        public const int Teacher = 160;
        public const int AssistantTeacher = 161;
        public const int Member = 220;
        public const int InActive = 230;
        public const int VisitingMember = 300;
        public const int Visitor = 310;
        public const int InServiceMember = 500;
        public const int VIP = 700;
        public const int Drop = -1;
    }
    public static class OrgStatusCode
    {
        public const int Active = 30;
        public const int Inactive = 40;
    }
    public static class AttendTrackLevelCode
    {
        public const int None = 0;
        public const int Headcount = 10;
        public const int Individual = 20;
        public const int Registered = 30;
    }
    public static class AttendanceClassificationCode
    {
        public const int Normal = 0;
    }
    public static class RegistrationEnum
    {
        public const int None = 0;
        public const int JoinOrganization = 1;
        public const int AttendMeeting = 2;
        public const int UserSelectsOrganization = 3;
        public const int ComputeOrganizationByAge = 4;
        public const int CreateAccount = 5;
        public const int ChooseSlot = 6;
        public const int ManageSubscriptions = 7;
    }
    public static class AttendTypeCode
    {
        public const int Absent = 0;
        public const int Leader = 10;
        public const int Volunteer = 20;
        public const int Member = 30;
        public const int VisitingMember = 40;
        public const int RecentVisitor = 50;
        public const int NewVisitor = 60;
        public const int InService = 70;
        public const int Offsite = 80;
        public const int Group = 90;
        public const int Homebound = 100;
        public const int OtherClass = 110;
    };
    public class TaskStatusCode
    {
        public const int Active = 10;
        public const int Waiting = 20;
        public const int Someday = 30;
        public const int Complete = 40;
        public const int Pending = 50;
        public const int Redelegated = 60;
    }
    public class ContactTypeCode
    {
        public const int PersonalVisit = 1;
        public const int PhoneCall = 2;
        public const int LetterSent = 3;
        public const int CardSent = 4;
        public const int EmailSent = 5;
        public const int InfoPackSent = 6;
        public const int Other = 7;
        public const int PhoneIn = 11;
        public const int SurveyEE = 12;
    }
    public class ContactReasonCode
    {
        public const int Unknown = 99;
        public const int Bereavement = 100;
        public const int Health = 110;
        public const int Personal = 120;
        public const int OutReach = 130;
        public const int ComeAndSee = 131;
        public const int InReach = 140;
        public const int Information = 150;
        public const int Other = 160;
    }
}