using System;
using System.Collections.Generic;
using System.Linq;
using UtilityExtensions;
using System.Text;
using System.Text.RegularExpressions;

namespace CmsData
{
    public partial class Organization
    {
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
            if (DivOrgs.Count == 1)
                return true;
            DivOrgs.Remove(divorg);
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

            //only Copy settings
            NotifyIds = frorg.NotifyIds;
            FirstMeetingDate = frorg.FirstMeetingDate;
            LastDayBeforeExtra = frorg.LastDayBeforeExtra;
            LastMeetingDate = frorg.LastMeetingDate;
            Limit = frorg.Limit;
            RegistrationTypeId = frorg.RegistrationTypeId;

            RegSetting = frorg.RegSetting;

            CopySettings2(frorg, this);
            Db.SubmitChanges();
        }
        private static void CopySettings2(Organization frorg, Organization toorg)
        {
            toorg.AllowNonCampusCheckIn = frorg.AllowNonCampusCheckIn;
            toorg.CanSelfCheckin = frorg.CanSelfCheckin;
            toorg.NumWorkerCheckInLabels = frorg.NumWorkerCheckInLabels;
            toorg.NoSecurityLabel = frorg.NoSecurityLabel;
            toorg.NumCheckInLabels = frorg.NumCheckInLabels;
            toorg.PhoneNumber = frorg.PhoneNumber;
        }
        public Organization CloneOrg(CMSDataContext Db, int? DivisionId)
        {
            var neworg = new Organization
            {
                SecurityTypeId = SecurityTypeId,
                CreatedDate = Util.Now,
                CreatedBy = Util.UserId1,
                DivisionId = DivisionId,
                LeaderMemberTypeId = LeaderMemberTypeId,
                OrganizationName = OrganizationName + " (copy)",
                EntryPointId = EntryPointId,
                OrganizationStatusId = OrganizationStatusId,
                AllowAttendOverlap = AllowAttendOverlap,
                GradeAgeStart = GradeAgeStart,
                CampusId = CampusId,
                IsBibleFellowshipOrg = IsBibleFellowshipOrg,
                RollSheetVisitorWks = RollSheetVisitorWks,
            };
            Db.Organizations.InsertOnSubmit(neworg);
            foreach (var div in DivOrgs)
                neworg.DivOrgs.Add(new DivOrg { Organization = neworg, DivId = div.DivId });
            foreach (var sc in OrgSchedules)
                neworg.OrgSchedules.Add(new OrgSchedule
                {
                    OrganizationId = OrganizationId,
                    AttendCreditId = sc.AttendCreditId,
                    SchedDay = sc.SchedDay,
                    SchedTime = sc.SchedTime,
                    Id = sc.Id
                });

            CopySettings2(this, neworg);
            Db.SubmitChanges();
            return neworg;
        }
        public Organization CloneOrg(CMSDataContext Db)
        {
            return CloneOrg(Db, DivisionId);
        }
        public static DateTime? GetDateFromScheduleId(int id)
        {
            int dw = id / 10000 - 1;
            id %= 10000;
            if (dw == 10) // any day
                dw = DateTime.Today.DayOfWeek.ToInt();
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
