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
            ComputeOrganizationByAge = 4
        }
        public static string FormatOrgName(string name, string leader, string loc)
        {
            if (loc.HasValue())
                loc = ", " + loc;
            //return "{0}:{1}{2} ({3})".Fmt(name, leader, loc, count);
            return "{0}:{1}{2}".Fmt(name, leader, loc);
        }

        public string FullName
        {
            get { return FormatOrgName(OrganizationName, LeaderName, Location); }
        }
        public string FullName2
        {
            get { return Division.Name + ", " + FormatOrgName(OrganizationName, LeaderName, Location); }
        }
        private CMSDataContext _Db;
        public CMSDataContext Db
        {
            get
            {
                if (_Db == null)
                    _Db = this.GetDataContext() as CMSDataContext;
                return _Db;
            }
        }

        private string _TagString;
        public string TagString
        {
            get
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
            set
            {
                if (!value.HasValue())
                    return;
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
                        string misctags = DbUtil.Settings("MiscTagsString", "Misc Tags");
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
        }
        public bool ToggleTag(int divid, bool main)
        {
            var divorg = DivOrgs.SingleOrDefault(d => d.DivId == divid);
            if (divorg == null)
            {
                DivOrgs.Add(new DivOrg { DivId = divid });
                if (main)
                    DivisionId = divid;
                return true;
            }
            DivOrgs.Remove(divorg);
            if (DivisionId == divid)
                DivisionId = null;
            Db.DivOrgs.DeleteOnSubmit(divorg);
            return false;
        }

        public bool PurgeOrg()
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
        public Organization CloneOrg()
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
                SecurityTypeId = SecurityTypeId,
                EntryPointId = EntryPointId,
                OrganizationStatusId = OrganizationStatusId,
                AllowAttendOverlap = AllowAttendOverlap,
                AttendClassificationId = AttendClassificationId,
                CanSelfCheckin = CanSelfCheckin,
                GradeAgeStart = GradeAgeStart,
                GradeAgeEnd = GradeAgeEnd,
                NumCheckInLabels = NumCheckInLabels,
                CampusId = CampusId,
                FirstMeetingDate = FirstMeetingDate,
                SchedDay = SchedDay,
                SchedTime = SchedTime,
                EmailSubject = EmailSubject,
                EmailMessage = EmailMessage,
                EmailAddresses = EmailAddresses,
                RegType = RegType,
                Limit = Limit,
                AllowNonCampusCheckIn = AllowNonCampusCheckIn,
                AttendTrackLevel = AttendTrackLevel,
                RollSheetVisitorWks = RollSheetVisitorWks,
                NumWorkerCheckInLabels = NumWorkerCheckInLabels,
            };
            DbUtil.Db.Organizations.InsertOnSubmit(neworg);
            foreach (var div in DivOrgs)
                neworg.DivOrgs.Add(new DivOrg { Organization = neworg, DivId = div.DivId });
            DbUtil.Db.SubmitChanges();
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
            if(hour > 0)
                return new DateTime(1900, 1, dw, hour, min, 0);
            return null;
        }
    }
}
