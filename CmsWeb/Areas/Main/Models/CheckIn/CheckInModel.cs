using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using UtilityExtensions;
using CmsData;
using System.Web.Mvc;

namespace CMSWeb.Models
{
    public class CheckInModel
    {
        public List<FamilyInfo> Match(string id, int campus, int thisday)
        {
            DbUtil.Db.SetNoLock();
            var ph = Util.GetDigits(id).PadLeft(10, '0');
            var p7 = ph.Substring(3);
            var ac = ph.Substring(0, 3);
            var q1 = from f in DbUtil.Db.Families
                     where f.HeadOfHousehold.DeceasedDate == null
                     where f.HomePhoneLU.StartsWith(p7)
                        || f.People.Any(p => p.CellPhoneLU.StartsWith(p7))
                     orderby f.FamilyId
                     select new FamilyInfo
                     {
                         FamilyId = f.FamilyId,
                         AreaCode = f.HomePhoneAC,
                         Name = f.HeadOfHousehold.Name,
                         Phone = id
                     };
            var matches = q1.ToList();
            if (matches.Count > 1)
                matches = matches.Where(m => m.AreaCode == ac || ac == "000").ToList();
            return matches;
        }
        public List<Attendee> FamilyMembers(int id, int campus, int thisday)
        {
            DbUtil.Db.SetNoLock();
            var now = Util.Now;
            // get org members first
            var members =
                from om in DbUtil.Db.OrganizationMembers
                let Hour = DbUtil.Db.GetTodaysMeetingHour(om.OrganizationId, thisday)
                let CheckedIn = DbUtil.Db.GetAttendedTodaysMeeting(om.OrganizationId, thisday, om.PeopleId)
                where om.Organization.CanSelfCheckin.Value
                where om.Organization.CampusId == campus || campus == 0
                where om.Person.FamilyId == id
                where om.Person.DeceasedDate == null
                where Hour != null
                select new Attendee
                {
                    Id = om.PeopleId,
                    Position = om.Person.PositionInFamilyId,
                    MemberVisitor = "M",
                    Name = om.Person.Name,
                    First = om.Person.PreferredName,
                    Last = om.Person.LastName,
                    BYear = om.Person.BirthYear,
                    BMon = om.Person.BirthMonth,
                    BDay = om.Person.BirthDay,
                    Class = om.Organization.OrganizationName,
                    Leader = om.Organization.LeaderName,
                    OrgId = om.OrganizationId,
                    Location = om.Organization.Location,
                    Age = om.Person.Age ?? 0,
                    Gender = om.Person.Gender.Code,
                    NumLabels = om.MemberTypeId ==
                        (int)CmsData.OrganizationMember.MemberTypeCode.Member ?
                            (om.Organization.NumCheckInLabels ?? 1)
                            : (om.Organization.NumWorkerCheckInLabels ?? 0),
                    Hour = Hour,
                    CheckedIn = CheckedIn ?? false,

                    goesby = om.Person.NickName,
                    email = om.Person.EmailAddress,
                    addr = om.Person.Family.AddressLineOne,
                    zip = om.Person.Family.ZipCode,
                    home = om.Person.Family.HomePhone,
                    cell = om.Person.CellPhone,
                    marital = om.Person.MaritalStatusId,
                    gender = om.Person.GenderId,
                    CampusId = om.Person.CampusId,
                };

            // now get recent visitors

            var visitors =
                from a in DbUtil.Db.Attends
                let Hour1 = DbUtil.Db.GetTodaysMeetingHour(a.OrganizationId, thisday)
                where a.Person.FamilyId == id
                where a.Person.DeceasedDate == null
                where a.Organization.CanSelfCheckin.Value
                where a.Organization.CampusId == campus || campus == 0
                where a.AttendanceFlag && a.MeetingDate >= a.Organization.VisitorDate.Value.Date
                where Attend.VisitAttendTypes.Contains(a.AttendanceTypeId.Value)
                where !a.Organization.OrganizationMembers.Any(om => om.PeopleId == a.PeopleId)
                where Hour1 != null
                group a by new { a.PeopleId, a.OrganizationId } into g
                let a = g.OrderByDescending(att => att.MeetingDate).First()
                select new Attendee
                {
                    Id = a.PeopleId,
                    Position = a.Person.PositionInFamilyId,
                    MemberVisitor = "V",
                    Name = a.Person.Name,
                    First = a.Person.PreferredName,
                    Last = a.Person.LastName,
                    BYear = a.Person.BirthYear,
                    BMon = a.Person.BirthMonth,
                    BDay = a.Person.BirthDay,
                    Class = a.Organization.OrganizationName,
                    Leader = a.Organization.LeaderName,
                    OrgId = a.OrganizationId,
                    //OrgId = (a.Organization.CanSelfCheckin ?? false) ? a.OrganizationId : 0,
                    Location = a.Organization.Location,
                    Age = a.Person.Age ?? 0,
                    Gender = a.Person.Gender.Code,
                    NumLabels = a.Organization.NumCheckInLabels ?? 1,
                    Hour = DbUtil.Db.GetTodaysMeetingHour(a.OrganizationId, thisday),
                    CheckedIn = DbUtil.Db.GetAttendedTodaysMeeting(a.OrganizationId, thisday, a.PeopleId) ?? false,

                    goesby = a.Person.NickName,
                    email = a.Person.EmailAddress,
                    addr = a.Person.Family.AddressLineOne,
                    zip = a.Person.Family.ZipCode,
                    home = a.Person.Family.HomePhone,
                    cell = a.Person.CellPhone,
                    marital = a.Person.MaritalStatusId,
                    gender = a.Person.GenderId,
                    CampusId = a.Person.CampusId,
                };

            var list = members.ToList();
            list.AddRange(visitors.ToList());

            // now get rest of family
            const string PleaseVisit = "No self check-in meetings available";
            var VisitorOrgName = PleaseVisit;
            var VisitorOrgId = 0;
            var VisitorOrgHour = (DateTime?)null;
            // find a org on campus that allows an older, new visitor to check in to
            var qv = from o in DbUtil.Db.Organizations
                     where o.CampusId == campus || campus == 0
                     where o.CanSelfCheckin == true
                     where o.AllowNonCampusCheckIn == true
                     where o.SchedDay == thisday
                     select o;
            var vo = qv.FirstOrDefault();
            if (vo != null)
            {
                VisitorOrgName = vo.OrganizationName;
                VisitorOrgId = vo.OrganizationId;
                VisitorOrgHour = vo.SchedTime;
            }
            var otherfamily =
                from p in DbUtil.Db.People
                where p.FamilyId == id
                where p.DeceasedDate == null
                where !list.Select(a => a.Id).Contains(p.PeopleId)
                let oldervisitor = (p.CampusId != campus || campus == 0) && p.Age > 12
                select new Attendee
                {
                    Id = p.PeopleId,
                    Position = p.PositionInFamilyId,
                    Name = p.Name,
                    First = p.PreferredName,
                    Last = p.LastName,
                    BYear = p.BirthYear,
                    BMon = p.BirthMonth,
                    BDay = p.BirthDay,
                    Class = oldervisitor ? VisitorOrgName : PleaseVisit,
                    OrgId = oldervisitor ? VisitorOrgId : 0,
                    Age = p.Age ?? 0,
                    Gender = p.Gender.Code,
                    NumLabels = 1,
                    Hour = VisitorOrgHour,

                    goesby = p.NickName,
                    email = p.EmailAddress,
                    addr = p.Family.AddressLineOne,
                    zip = p.Family.ZipCode,
                    home = p.Family.HomePhone,
                    cell = p.CellPhone,
                    marital = p.MaritalStatusId,
                    gender = p.GenderId,
                    CampusId = p.CampusId,
                };
            list.AddRange(otherfamily.ToList());
            var list2 = list.OrderBy(a => a.Position)
                .ThenByDescending(a => a.Position == 10 ? a.Gender : "U")
                .ThenBy(a => a.Age).ToList();
            return list2;
        }
        public IEnumerable<Campu> Campuses()
        {
            var q = from c in DbUtil.Db.Campus
                    where c.Organizations.Any(o => o.CanSelfCheckin == true)
                    orderby c.Id
                    select c;
            return q;
        }
        public void RecordAttend(int PeopleId, int OrgId, bool Present, int thisday)
        {
            var q = from o in DbUtil.Db.Organizations
                    where o.OrganizationId == OrgId
                    select new
                    {
                        MeetingId = DbUtil.Db.GetTodaysMeetingId(OrgId, thisday),
                        MeetingTime = DbUtil.Db.GetTodaysMeetingHour(OrgId, thisday),
                        o.AttendTrkLevelId,
                        o.Location
                    };
            var info = q.Single();
            var meeting = DbUtil.Db.Meetings.SingleOrDefault(m => m.MeetingId == info.MeetingId);
            if (meeting == null)
            {
                meeting = new CmsData.Meeting
                {
                    OrganizationId = OrgId,
                    MeetingDate = info.MeetingTime,
                    CreatedDate = Util.Now,
                    CreatedBy = Util.UserId1,
                    GroupMeetingFlag = info.AttendTrkLevelId 
                        == (int)CmsData.Organization.AttendTrackLevelCode.Headcount,
                    Location = info.Location,
                };
                DbUtil.Db.Meetings.InsertOnSubmit(meeting);
                DbUtil.Db.SubmitChanges();
            }
            Attend.RecordAttendance(PeopleId, meeting.MeetingId, Present);
            DbUtil.Db.UpdateMeetingCounters(meeting.MeetingId);
        }
    }
}
