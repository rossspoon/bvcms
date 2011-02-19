using System;
using System.Collections.Generic;
using System.Linq;
using UtilityExtensions;
using System.Text;

namespace CmsData
{
    public partial class Organization
    {
        public enum OrgStatusCode
        {
            Active = 30,
            Inactive = 40,
        }
        public enum AttendTrackLevelCode
        {
            None = 0,
            Headcount = 10,
            Individual = 20,
            Registered = 30
        }
        public enum AttendanceClassificationCode
        {
            Normal = 0,
        }
        public enum RegistrationEnum
        {
            None = 0,
            JoinOrganization = 1,
            AttendMeeting = 2,
            UserSelectsOrganization = 3,
            ComputeOrganizationByAge = 4,
            CreateAccount = 5,
            ChooseSlot = 6,
            ManageSubscriptions = 7,
        }
        public static string FormatOrgName(string name, string leader, string loc)
        {
            if (loc.HasValue())
                loc = ", " + loc;
            if (leader.HasValue())
                leader = ":" + leader;
            return "{0}{1}{2}".Fmt(name, leader, loc);
        }

        public string FullName
        {
            get { return FormatOrgName(OrganizationName, LeaderName, Location); }
        }
        public string FullName2
        {
            get { return DivisionName + ", " + FormatOrgName(OrganizationName, LeaderName, Location); }
        }

        private string _TagString;
        public string TagString()
        {
            if (_TagString == null)
            {
                var sb = new StringBuilder();
                var q = from d in DivOrgs
                        orderby d.Division.Name
                        select d.Division.Name;
                foreach (var name in q)
                    sb.Append(name + ",");
                if (sb.Length > 0)
                    sb.Remove(sb.Length - 1, 1);
                _TagString = sb.ToString();
            }
            return _TagString;
        }
        public void SetTagString(CMSDataContext Db, string value)
        {
            if (!value.HasValue())
            {
                Db.DivOrgs.DeleteAllOnSubmit(DivOrgs);
                return;
            }
            var a = value.Split(',');
            var qdelete = from d in DivOrgs
                          where !a.Contains(d.Division.Name)
                          select d;
            Db.DivOrgs.DeleteAllOnSubmit(qdelete);

            var q = from s in a
                    join d2 in DivOrgs on s equals d2.Division.Name into g
                    from d in g.DefaultIfEmpty()
                    where d == null
                    select s;

            foreach (var s in q)
            {
                var div = Db.Divisions.FirstOrDefault(d => d.Name == s);
                if (div == null)
                {
                    div = new Division { Name = s };
                    string misctags = DbUtil.Db.Setting("MiscTagsString", "Misc Tags");
                    var prog = Db.Programs.SingleOrDefault(p => p.Name == misctags);
                    if (prog == null)
                    {
                        prog = new Program { Name = misctags };
                        Db.Programs.InsertOnSubmit(prog);
                    }
                    div.Program = prog;
                }
                DivOrgs.Add(new DivOrg { Division = div });
            }
            _TagString = value;
        }
        public bool ToggleTag(CMSDataContext Db, int divid)
        {
            var divorg = DivOrgs.SingleOrDefault(d => d.DivId == divid);
            if (divorg == null)
            {
                DivOrgs.Add(new DivOrg { DivId = divid });
                return true;
            }
            DivOrgs.Remove(divorg);
            if (DivisionId == divid)
                DivisionId = null;
            Db.DivOrgs.DeleteOnSubmit(divorg);
            return false;
        }

        public bool PurgeOrg(CMSDataContext Db)
        {
            try
            {
                Db.PurgeOrganization(OrganizationId);
            }
            catch
            {
                return false;
            }
            return true;
        }
        public void CopySettings(CMSDataContext Db, int fromid)
        {
            var frorg = Db.LoadOrganizationById(fromid);
            AgeFee = frorg.AgeFee;
            AgeGroups = frorg.AgeGroups;
            AllowLastYearShirt = frorg.AllowLastYearShirt;
            AllowNonCampusCheckIn = frorg.AllowNonCampusCheckIn;
            AllowOnlyOne = frorg.AllowOnlyOne;
            AskAllergies = frorg.AskAllergies;
            AskChurch = frorg.AskChurch;
            AskCoaching = frorg.AskCoaching;
            AskDoctor = frorg.AskDoctor;
            AskEmContact = frorg.AskEmContact;
            AskGrade = frorg.AskGrade;
            AskInsurance = frorg.AskInsurance;
            AskOptions = frorg.AskOptions;
            AskParents = frorg.AskParents;
            AskRequest = frorg.AskRequest;
            AskShirtSize = frorg.AskShirtSize;
            AskTickets = frorg.AskTickets;
            AskTylenolEtc = frorg.AskTylenolEtc;
            CanSelfCheckin = frorg.CanSelfCheckin;
            Deposit = frorg.Deposit;
            EmailAddresses = frorg.EmailAddresses;
            EmailMessage = frorg.EmailMessage;
            EmailSubject = frorg.EmailSubject;
            ExtraFee = frorg.ExtraFee;
            ExtraOptions = frorg.ExtraOptions;
            ExtraOptionsLabel = frorg.ExtraOptionsLabel;
            ExtraQuestions = frorg.ExtraQuestions;
            Fee = frorg.Fee;
            FirstMeetingDate = frorg.FirstMeetingDate;
            GenderId = frorg.GenderId;
            GradeOptions = frorg.GradeOptions;
            Instructions = frorg.Instructions;
            LastDayBeforeExtra = frorg.LastDayBeforeExtra;
            LastMeetingDate = frorg.LastMeetingDate;
            Limit = frorg.Limit;
            LinkGroupsFromOrgs = frorg.LinkGroupsFromOrgs;
            MaximumFee = frorg.MaximumFee;
            MemberOnly = frorg.MemberOnly;
            NoSecurityLabel = frorg.NoSecurityLabel;
            NotReqAddr = frorg.NotReqAddr;
            NotReqDOB = frorg.NotReqDOB;
            NotReqGender = frorg.NotReqGender;
            NotReqMarital = frorg.NotReqMarital;
            NotReqPhone = frorg.NotReqPhone;
            NotReqZip = frorg.NotReqZip;
            NumCheckInLabels = frorg.NumCheckInLabels;
            NumItemsLabel = frorg.NumItemsLabel;
            NumWorkerCheckInLabels = frorg.NumWorkerCheckInLabels;
            OptionsLabel = frorg.OptionsLabel;
            OrgMemberFees = frorg.OrgMemberFees;
            PhoneNumber = frorg.PhoneNumber;
            RegistrationTypeId = frorg.RegistrationTypeId;
            RequestLabel = frorg.RequestLabel;
            SecurityTypeId = frorg.SecurityTypeId;
            ShirtFee = frorg.ShirtFee;
            ShirtSizes = frorg.ShirtSizes;
            Terms = frorg.Terms;
            ValidateOrgs = frorg.ValidateOrgs;
            YesNoQuestions = frorg.YesNoQuestions;
            Db.SubmitChanges();
        }
        public Organization CloneOrg(CMSDataContext Db)
        {
            var neworg = new Organization
            {
                AttendTrkLevelId = AttendTrkLevelId,
                CreatedDate = Util.Now,
                CreatedBy = Util.UserId1,
                DivisionId = DivisionId,
                LeaderMemberTypeId = LeaderMemberTypeId,
                OrganizationName = OrganizationName + " (copy)",
                ScheduleId = ScheduleId,
                EntryPointId = EntryPointId,
                OrganizationStatusId = OrganizationStatusId,
                AllowAttendOverlap = AllowAttendOverlap,
                AttendClassificationId = AttendClassificationId,
                GradeAgeStart = GradeAgeStart,
                GradeAgeEnd = GradeAgeEnd,
                CampusId = CampusId,
                SchedDay = SchedDay,
                SchedTime = SchedTime,
                IsBibleFellowshipOrg = IsBibleFellowshipOrg,
                AttendTrackLevel = AttendTrackLevel,
                RollSheetVisitorWks = RollSheetVisitorWks,
            };
            Db.Organizations.InsertOnSubmit(neworg);
            foreach (var div in DivOrgs)
                neworg.DivOrgs.Add(new DivOrg { Organization = neworg, DivId = div.DivId });
            Db.SubmitChanges();
            neworg.CopySettings(Db, this.OrganizationId);
            return neworg;
        }
        public static DateTime? GetDateFromScheduleId(int id)
        {
            int dw = id / 10000 - 1;
            id %= 10000;
            if (dw == 0)
                dw = 7;
            int hour = id / 100;
            int min = id % 100;
            if (hour > 0)
                return new DateTime(1900, 1, dw, hour, min, 0);
            return null;
        }
        public string DivisionName
        {
            get
            {
                return Division != null ?
                    (Division.Program != null ? Division.Program.Name : "no program")
                        + ":" + Division.Name :
                       "<span style='color:red'>need a main division</span>";
            }
        }
    }
}
