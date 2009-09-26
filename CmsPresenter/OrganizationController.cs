/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using UtilityExtensions;
using CmsData;
using System.ComponentModel;
using System.Collections;
using System.Text;
using System.Web;

namespace CMSPresenter
{
    public class PersonVisitorInfo : TaggedPersonInfo
    {
        public DateTime? LastAttended { get; set; }
        public string NameParent1 { get; set; }
        public string NameParent2 { get; set; }
        public string VisitorType { get; set; }
    }
    public class GroupInfo
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
    public enum GroupSelect
    {
        Active,
        Inactive,
        Pending,
        Previous
    }
    [DataObject]
    public class OrganizationController
    {
        private int _count;
        private Dictionary<int, CodeValueItem> dict = (new CodeValueController()).AttendanceTrackLevelCodes().ToDictionary(cv => cv.Id);
        private int[] ChildSecurityRollSheets = new int[] { 4, 5, 6, 7 };

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<PersonMemberInfo> OrgMembers(int OrganizationId, GroupSelect Select, int GroupId, string sortExpression, int maximumRows, int startRowIndex)
        {
            int inactive = (int)OrganizationMember.MemberTypeCode.InActive;
            bool Active = true;
            bool Pending = false;
            switch (Select)
            {
                case GroupSelect.Inactive:
                    Active = false;
                    Pending = false;
                    break;
                case GroupSelect.Pending:
                    Active = true;
                    Pending = true;
                    break;
                case GroupSelect.Previous:
                    return PrevOrgMembers(OrganizationId, sortExpression, maximumRows, startRowIndex);
            }
            var q = from om in DbUtil.Db.OrganizationMembers
                    where om.OrganizationId == OrganizationId
                    where om.OrgMemMemTags.Any(mt => mt.MemberTagId == GroupId) || GroupId <= 0
                    where om.OrgMemMemTags.Count() == 0 || GroupId != -1
                    where (Active && om.MemberTypeId != inactive)
                        || (!Active && om.MemberTypeId == inactive)
                    where (Pending && om.Pending == true)
                        || (!Pending && (om.Pending ?? false) == false)
                    select om;
            _count = q.Count();
            q = ApplySort(q, sortExpression);
            var q2 = FetchPeopleList(q.Skip(startRowIndex).Take(maximumRows), Select);
            return q2;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<PersonMemberInfo> PrevOrgMembers(int OrganizationId, string sortExpression, int maximumRows, int startRowIndex)
        {
            var q = from etd in DbUtil.Db.EnrollmentTransactions
                    let mdt = DbUtil.Db.EnrollmentTransactions.Where(m =>
                        m.PeopleId == etd.PeopleId
                        && m.OrganizationId == OrganizationId
                        && m.TransactionTypeId > 3
                        && m.TransactionStatus == false).Select(m => m.TransactionDate).Max()
                    where etd.TransactionStatus == false
                    where etd.TransactionDate == mdt
                    where etd.OrganizationId == OrganizationId
                    where etd.TransactionTypeId >= 4
                    where !etd.Person.OrganizationMembers.Any(om => om.OrganizationId == OrganizationId)
                    select etd;
            _count = q.Count();
            q = ApplySort(q, sortExpression);
            var q2 = FetchPeopleList(q.Skip(startRowIndex).Take(maximumRows));
            return q2;
        }
        private int visitorCount;
        public int VisitorCount(int orgid, string sortExpression, int maximumRows, int startRowIndex)
        {
            return visitorCount;
        }
        public IEnumerable<PersonVisitorInfo> Visitors(int orgid, string sortExpression, int maximumRows, int startRowIndex)
        {
            var qb = DbUtil.Db.QueryBuilderVisitedCurrentOrg();
            var q = DbUtil.Db.People.Where(qb.Predicate());
            q = q.Where(p => !p.OrganizationMembers.Any(om => om.OrganizationId == orgid));
            visitorCount = q.Count();
            q = ApplySort(q, orgid, sortExpression);
            var q2 = FetchVisitorList(q.Skip(startRowIndex).Take(maximumRows), orgid);

            return q2;
        }
        public int Count(int OrganizationId, GroupSelect Select, int GroupId, string sortExpression, int maximumRows, int startRowIndex)
        {
            return _count;
        }

        public IEnumerable<OrganizationInfo> GetOrganizationInfo(int orgid)
        {
            return GetOrganizationInfo(orgid, Util.Now.Date);
        }
        public IEnumerable<OrganizationInfo> GetOrganizationInfo(int orgid, DateTime MeetingDate)
        {
            var Db = DbUtil.Db;
            return from o in Db.Organizations
                   where o.OrganizationId == orgid
                   let LookbackDt = MeetingDate.AddDays(-7 * o.RollSheetVisitorWks ?? 3)
                   select new OrganizationInfo
                   {
                       AttendanceTrackingLevel = dict[o.AttendTrkLevelId].Value,
                       DivisionName = o.DivOrgs.FirstOrDefault(d => d.Division.Program.Name != DbUtil.MiscTagsString).Division.Name,
                       FirstMeetingDate = Util.FormatDate(o.FirstMeetingDate),
                       LastMeetingDate = Util.FormatDate(o.LastMeetingDate),
                       LeaderId = o.LeaderId,
                       LeaderName = o.LeaderName,
                       MemberCount = o.OrganizationMembers.Count(om =>
                           om.MemberTypeId != (int)OrganizationMember.MemberTypeCode.InActive),
                       OrganizationId = o.OrganizationId,
                       OrganizationName = o.OrganizationName,
                       OrganizationStatus = o.OrganizationStatusId,
                       VisitorCount = (from p in Db.People
                                       where p.Attends.Any(a =>
                                           a.AttendanceFlag == true
                                           && !p.OrganizationMembers.Any(om => om.OrganizationId == o.OrganizationId)
                                           && AttendController.VisitAttendTypes.Contains(a.AttendanceTypeId.Value)
                                           && a.OrganizationId == o.OrganizationId
                                           && a.MeetingDate <= MeetingDate
                                           && a.MeetingDate >= LookbackDt)
                                       select p).Count(),
                       Schedule = o.WeeklySchedule.Description,
                   };
        }

        public static IEnumerable<PersonMemberInfo> FetchPeopleList(IQueryable<OrganizationMember> query, GroupSelect fromtab)
        {
            var tagownerid = Util.CurrentTagOwnerId;
            var q = from om in query
                    let p = om.Person
                    select new PersonMemberInfo
                    {
                        PeopleId = p.PeopleId,
                        Name = p.Name,
                        Name2 = p.Name2,
                        //JoinDate = p.JoinDate,
                        BirthDate = Util.FormatBirthday(
                            p.BirthYear,
                            p.BirthMonth,
                            p.BirthDay),
                        Address = p.PrimaryAddress,
                        Address2 = p.PrimaryAddress2,
                        CityStateZip = Util.FormatCSZ(p.PrimaryCity, p.PrimaryState, p.PrimaryZip),
                        PhonePref = p.PhonePrefId,
                        HomePhone = p.HomePhone,
                        CellPhone = p.CellPhone,
                        WorkPhone = p.WorkPhone,
                        MemberStatus = p.MemberStatus.Description,
                        Email = p.EmailAddress,
                        BFTeacher = p.BibleFellowshipTeacher,
                        BFTeacherId = p.BibleFellowshipTeacherId,
                        Age = p.Age.ToString(),
                        MemberTypeCode = om.MemberType.Code,
                        MemberType = om.MemberType.Description,
                        MemberTypeId = om.MemberTypeId,
                        InactiveDate = om.InactiveDate,
                        AttendPct = om.AttendPct,
                        LastAttended = om.LastAttended,
                        HasTag = p.Tags.Any(t => t.Tag.Name == Util.CurrentTagName && t.Tag.PeopleId == tagownerid),
                        FromTab = fromtab,
                        Joined = om.EnrollmentDate,
                    };
            return q;
        }
        public static IEnumerable<PersonMemberInfo> FetchPeopleList(IQueryable<EnrollmentTransaction> query)
        {
            var tagownerid = Util.CurrentTagOwnerId;
            var q = from m in query
                    let p = m.Person
                    select new PersonMemberInfo
                    {
                        PeopleId = p.PeopleId,
                        Name = p.Name,
                        Name2 = p.Name2,
                        //JoinDate = p.JoinDate,
                        BirthDate = Util.FormatBirthday(
                            p.BirthYear,
                            p.BirthMonth,
                            p.BirthDay),
                        Address = p.PrimaryAddress,
                        Address2 = p.PrimaryAddress2,
                        CityStateZip = Util.FormatCSZ(p.PrimaryCity, p.PrimaryState, p.PrimaryZip),
                        PhonePref = p.PhonePrefId,
                        HomePhone = p.HomePhone,
                        CellPhone = p.CellPhone,
                        WorkPhone = p.WorkPhone,
                        MemberStatus = p.MemberStatus.Description,
                        Email = p.EmailAddress,
                        BFTeacher = p.BibleFellowshipTeacher,
                        BFTeacherId = p.BibleFellowshipTeacherId,
                        Age = p.Age.ToString(),
                        MemberTypeCode = m.MemberType.Code,
                        MemberType = m.MemberType.Description,
                        MemberTypeId = m.MemberTypeId,
                        AttendPct = m.AttendancePercentage,
                        HasTag = p.Tags.Any(t => t.Tag.Name == Util.CurrentTagName && t.Tag.PeopleId == tagownerid),
                        FromTab = GroupSelect.Previous,
                        Joined = m.EnrollmentDate,
                        Dropped = m.TransactionDate
                    };
            return q;
        }
        public IEnumerable<PersonVisitorInfo> FetchVisitorList(IQueryable<Person> query, int orgid)
        {
            var tagownerid = Util.CurrentTagOwnerId;
            var q = from p in query
                    select new PersonVisitorInfo
                    {
                        VisitorType = p.MemberStatusId == (int)Person.MemberStatusCode.Member ? "VM" : "VS",
                        PeopleId = p.PeopleId,
                        Name = p.Name,
                        Name2 = p.Name2,
                        BirthDate = Util.FormatBirthday(
                            p.BirthYear,
                            p.BirthMonth,
                            p.BirthDay),
                        Address = p.PrimaryAddress,
                        Address2 = p.PrimaryAddress2,
                        CityStateZip = Util.FormatCSZ(p.PrimaryCity, p.PrimaryState, p.PrimaryZip),
                        PhonePref = p.PhonePrefId,
                        HomePhone = p.HomePhone,
                        CellPhone = p.CellPhone,
                        WorkPhone = p.WorkPhone,
                        MemberStatus = p.MemberStatus.Description,
                        Email = p.EmailAddress,
                        BFTeacher = p.BibleFellowshipTeacher,
                        BFTeacherId = p.BibleFellowshipTeacherId,
                        Age = p.Age.ToString(),
                        LastAttended = DbUtil.Db.LastAttended(orgid, p.PeopleId),
                        HasTag = p.Tags.Any(t => t.Tag.Name == Util.CurrentTagName && t.Tag.PeopleId == tagownerid),
                        //NameParent1 = p.Family.People.FirstOrDefault(x => x.PeopleId == p.Family.HeadOfHouseholdId).Name,
                        //NameParent2 = p.Family.People.FirstOrDefault(x => x.PositionInFamilyId == 10 & x.PeopleId != p.Family.HeadOfHouseholdId).Name,
                    };
            return q;
        }

        public Organization CloneOrg(int orgid)
        {
            var org = DbUtil.Db.Organizations.Single(o => o.OrganizationId == orgid);
            var neworg = new Organization
            {
                AttendTrkLevelId = org.AttendTrkLevelId,
                CreatedDate = DateTime.Now,
                CreatedBy = Util.UserId1,
                DivisionId = org.DivisionId,
                LeaderMemberTypeId = org.LeaderMemberTypeId,
                OrganizationName = org.OrganizationName + " (copy)",
                ScheduleId = org.ScheduleId,
                SecurityTypeId = org.SecurityTypeId,
                EntryPointId = org.EntryPointId,
                OrganizationStatusId = org.OrganizationStatusId,
                AllowAttendOverlap = org.AllowAttendOverlap,
                AttendClassificationId = org.AttendClassificationId,
                CanSelfCheckin = org.CanSelfCheckin,
                GradeRangeStart = org.GradeRangeStart,
                GradeRangeEnd = org.GradeRangeEnd,
                NumCheckInLabels = org.NumCheckInLabels,
                CampusId = org.CampusId,
                FirstMeetingDate = org.FirstMeetingDate,
            };
            DbUtil.Db.Organizations.InsertOnSubmit(neworg);
            foreach (var div in org.DivOrgs)
                neworg.DivOrgs.Add(new DivOrg { Organization = neworg, DivId = div.DivId });
            DbUtil.Db.SubmitChanges();
            return neworg;
        }
        public static IQueryable<EnrollmentTransaction> ApplySort(IQueryable<EnrollmentTransaction> query, string sort)
        {
            switch (sort)
            {
                case "MemberStatus":
                    query = from om in query
                            let p = om.Person
                            orderby p.MemberStatus.Code,
                            p.LastName,
                            p.FirstName,
                            p.PeopleId
                            select om;
                    break;
                case "MemberType":
                    query = from om in query
                            let p = om.Person
                            orderby om.MemberType.Code,
                            p.LastName,
                            p.FirstName,
                            p.PeopleId
                            select om;
                    break;
                case "Address":
                    query = from om in query
                            let p = om.Person
                            orderby p.Family.StateCode,
                            p.Family.CityName,
                            p.Family.AddressLineOne,
                            p.PeopleId
                            select om;
                    break;
                case "BFTeacher":
                    query = from om in query
                            let p = om.Person
                            orderby p.BibleFellowshipTeacher,
                            p.LastName,
                            p.FirstName,
                            p.PeopleId
                            select om;
                    break;
                case "AttendPct":
                    query = from om in query
                            orderby om.AttendancePercentage
                            select om;
                    break;
                case "Age":
                    query = from om in query
                            let p = om.Person
                            orderby p.BirthYear, p.BirthMonth, p.BirthDay
                            select om;
                    break;
                case "DOB":
                    query = from om in query
                            let p = om.Person
                            orderby p.BirthMonth, p.BirthDay,
                            p.LastName, p.FirstName
                            select om;
                    break;
                case "MemberStatus DESC":
                    query = from om in query
                            let p = om.Person
                            orderby p.MemberStatus.Code descending,
                            p.LastName descending,
                            p.FirstName descending,
                            p.PeopleId descending
                            select om;
                    break;
                case "MemberType DESC":
                    query = from om in query
                            let p = om.Person
                            orderby om.MemberType.Code descending,
                            p.LastName descending,
                            p.FirstName descending,
                            p.PeopleId descending
                            select om;
                    break;
                case "Address DESC":
                    query = from om in query
                            let p = om.Person
                            orderby p.Family.StateCode descending,
                                   p.Family.CityName descending,
                                   p.Family.AddressLineOne descending,
                                   p.PeopleId descending
                            select om;
                    break;
                case "BFTeacher DESC":
                    query = from om in query
                            let p = om.Person
                            orderby p.BibleFellowshipTeacher descending,
                            p.LastName descending,
                            p.FirstName descending,
                            p.PeopleId descending
                            select om;
                    break;
                case "AttendPct DESC":
                    query = from om in query
                            orderby om.AttendancePercentage descending
                            select om;
                    break;
                case "Name DESC":
                    query = from om in query
                            let p = om.Person
                            orderby p.LastName descending,
                            p.LastName descending,
                            p.PeopleId descending
                            select om;
                    break;
                case "DOB DESC":
                    query = from om in query
                            let p = om.Person
                            orderby p.BirthMonth descending, p.BirthDay descending,
                            p.LastName descending, p.FirstName descending
                            select om;
                    break;
                case "Age DESC":
                    query = from om in query
                            let p = om.Person
                            orderby p.BirthYear descending, p.BirthMonth descending, p.BirthDay descending
                            select om;
                    break;
                case "Name":
                default:
                    query = from om in query
                            let p = om.Person
                            orderby p.LastName,
                            p.FirstName,
                            p.PeopleId
                            select om;
                    break;
            }
            return query;
        }
        public static IQueryable<OrganizationMember> ApplySort(IQueryable<OrganizationMember> query, string sort)
        {
            switch (sort)
            {
                case "MemberStatus":
                    query = from om in query
                            let p = om.Person
                            orderby p.MemberStatus.Code,
                            p.LastName,
                            p.FirstName,
                            p.PeopleId
                            select om;
                    break;
                case "MemberType":
                    query = from om in query
                            let p = om.Person
                            orderby om.MemberType.Code,
                            p.LastName,
                            p.FirstName,
                            p.PeopleId
                            select om;
                    break;
                case "Address":
                    query = from om in query
                            let p = om.Person
                            orderby p.Family.StateCode,
                            p.Family.CityName,
                            p.Family.AddressLineOne,
                            p.PeopleId
                            select om;
                    break;
                case "BFTeacher":
                    query = from om in query
                            let p = om.Person
                            orderby p.BibleFellowshipTeacher,
                            p.LastName,
                            p.FirstName,
                            p.PeopleId
                            select om;
                    break;
                case "AttendPct":
                    query = from om in query
                            orderby om.AttendPct
                            select om;
                    break;
                case "Age":
                    query = from om in query
                            let p = om.Person
                            orderby p.BirthYear, p.BirthMonth, p.BirthDay
                            select om;
                    break;
                case "DOB":
                    query = from om in query
                            let p = om.Person
                            orderby p.BirthMonth, p.BirthDay,
                            p.LastName, p.FirstName
                            select om;
                    break;
                case "LastAttended":
                    query = from om in query
                            let p = om.Person
                            orderby om.LastAttended, p.LastName, p.FirstName
                            select om;
                    break;
                case "MemberStatus DESC":
                    query = from om in query
                            let p = om.Person
                            orderby p.MemberStatus.Code descending,
                            p.LastName descending,
                            p.FirstName descending,
                            p.PeopleId descending
                            select om;
                    break;
                case "MemberType DESC":
                    query = from om in query
                            let p = om.Person
                            orderby om.MemberType.Code descending,
                            p.LastName descending,
                            p.FirstName descending,
                            p.PeopleId descending
                            select om;
                    break;
                case "Address DESC":
                    query = from om in query
                            let p = om.Person
                            orderby p.Family.StateCode descending,
                                   p.Family.CityName descending,
                                   p.Family.AddressLineOne descending,
                                   p.PeopleId descending
                            select om;
                    break;
                case "BFTeacher DESC":
                    query = from om in query
                            let p = om.Person
                            orderby p.BibleFellowshipTeacher descending,
                            p.LastName descending,
                            p.FirstName descending,
                            p.PeopleId descending
                            select om;
                    break;
                case "AttendPct DESC":
                    query = from om in query
                            orderby om.AttendPct descending
                            select om;
                    break;
                case "Name DESC":
                    query = from om in query
                            let p = om.Person
                            orderby p.LastName descending,
                            p.LastName descending,
                            p.PeopleId descending
                            select om;
                    break;
                case "DOB DESC":
                    query = from om in query
                            let p = om.Person
                            orderby p.BirthMonth descending, p.BirthDay descending,
                            p.LastName descending, p.FirstName descending
                            select om;
                    break;
                case "LastAttended DESC":
                    query = from om in query
                            let p = om.Person
                            orderby om.LastAttended descending, p.LastName descending, p.FirstName descending
                            select om;
                    break;
                case "Age DESC":
                    query = from om in query
                            let p = om.Person
                            orderby p.BirthYear descending, p.BirthMonth descending, p.BirthDay descending
                            select om;
                    break;
                case "Name":
                default:
                    query = from om in query
                            let p = om.Person
                            orderby p.LastName,
                            p.FirstName,
                            p.PeopleId
                            select om;
                    break;
            }
            return query;
        }
        private static IQueryable<Person> ApplySort(IQueryable<Person> query, int orgid, string sort)
        {
            switch (sort)
            {
                case "MemberStatus":
                    query = from p in query
                            orderby p.MemberStatus.Code,
                            p.LastName,
                            p.FirstName,
                            p.PeopleId
                            select p;
                    break;
                case "Address":
                    query = from p in query
                            orderby p.Family.StateCode,
                            p.Family.CityName,
                            p.Family.AddressLineOne,
                            p.PeopleId
                            select p;
                    break;
                case "BFTeacher":
                    query = from p in query
                            orderby p.BibleFellowshipTeacher,
                            p.LastName,
                            p.FirstName,
                            p.PeopleId
                            select p;
                    break;
                case "DOB":
                    query = from p in query
                            orderby p.BirthMonth, p.BirthDay,
                            p.LastName, p.FirstName
                            select p;
                    break;
                case "LastAttended":
                    query = from p in query
                            orderby DbUtil.Db.LastAttended(orgid, p.PeopleId), p.LastName, p.FirstName
                            select p;
                    break;
                case "MemberStatus DESC":
                    query = from p in query
                            orderby p.MemberStatus.Code descending,
                            p.LastName descending,
                            p.FirstName descending,
                            p.PeopleId descending
                            select p;
                    break;
                case "Address DESC":
                    query = from p in query
                            orderby p.Family.StateCode descending,
                                   p.Family.CityName descending,
                                   p.Family.AddressLineOne descending,
                                   p.PeopleId descending
                            select p;
                    break;
                case "BFTeacher DESC":
                    query = from p in query
                            orderby p.BibleFellowshipTeacher descending,
                            p.LastName descending,
                            p.FirstName descending,
                            p.PeopleId descending
                            select p;
                    break;
                case "Name DESC":
                    query = from p in query
                            orderby p.LastName descending,
                            p.LastName descending,
                            p.PeopleId descending
                            select p;
                    break;
                case "DOB DESC":
                    query = from p in query
                            orderby p.BirthMonth descending, p.BirthDay descending,
                            p.LastName descending, p.FirstName descending
                            select p;
                    break;
                case "LastAttended DESC":
                    query = from p in query
                            orderby DbUtil.Db.LastAttended(orgid, p.PeopleId) descending,
                            p.LastName descending, p.FirstName descending
                            select p;
                    break;
                case "Name":
                default:
                    query = from p in query
                            orderby p.LastName,
                            p.FirstName,
                            p.PeopleId
                            select p;
                    break;
            }
            return query;
        }
        public void SendNotices(
            int progid,
            int divid,
            int orgid,
            DateTime date,
            ITaskNotify notify)
        {
            var q = from m in DbUtil.Db.OrganizationMembers
                    where m.OrganizationId == orgid || orgid == 0
                    where m.Organization.DivOrgs.Any(t => t.DivId == divid) || divid == 0
                    where m.Organization.DivOrgs.Any(t => t.Division.ProgId == progid) || progid == 0
                    where m.Organization.Meetings.Any(meeting => meeting.MeetingDate.Value.Date == date.Date)
                    let u = m.Person.Users.FirstOrDefault()
                    where u != null
                    group m by m.PeopleId into g
                    select g;
            var sb2 = new StringBuilder("Notices sent to:</br>\n<table>\n");
            foreach (var g in q)
            {
                var person = g.First().Person;
                var sb = new StringBuilder("The following meetings are ready to be viewed:<br/>\n");
                foreach (var om in g)
                {
                    var q2 = from mt in DbUtil.Db.Meetings
                             where mt.OrganizationId == om.OrganizationId
                             where mt.MeetingDate.Value.Date == date.Date
                             select new
                              {
                                  mt.MeetingId,
                                  mt.Organization.OrganizationName,
                                  mt.MeetingDate,
                                  mt.Organization.LeaderName,
                                  mt.Organization.Location
                              };
                    foreach (var mt in q2)
                    {
                        string orgname = Organization.FormatOrgName(mt.OrganizationName, mt.LeaderName, mt.Location);
                        sb.AppendFormat("<a href='{0}/Meeting.aspx?id={1}'>{2} - {3}</a><br/>\n",
                            DbUtil.TaskHost, mt.MeetingId, orgname, mt.MeetingDate);
                        sb2.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2:M/d/yy h:mmtt}</td></tr>\n",
                            person.Name, orgname, mt.MeetingDate);
                    }
                }
                notify.EmailNotification(DbUtil.Db.CurrentUser.Person, person, "Attendance reports ready for viewing on CMS", sb.ToString());
            }
            sb2.Append("</table>\n");
            notify.EmailNotification(DbUtil.Db.CurrentUser.Person, DbUtil.Db.CurrentUser.Person, "Attendance emails sent", sb2.ToString());
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<MemberTag> FetchMemberGroups(int oid)
        {
            var q = from g in DbUtil.Db.MemberTags
                    where g.OrgId == oid
                    orderby g.Name
                    select g;
            return q;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<MemberTag> FetchMemberGroups2(int oid)
        {
            var list = FetchMemberGroups(oid).ToList();
            list.Insert(0, new MemberTag { Id = -1, Name = "(not assigned)" });
            list.Insert(0, new MemberTag { Id = 0, Name = "(not specified)" });
            return list;
        }
    }
}
