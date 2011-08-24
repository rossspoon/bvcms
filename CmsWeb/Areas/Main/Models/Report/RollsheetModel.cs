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
using System.Diagnostics;
using CmsData.Codes;

namespace CmsWeb.Areas.Main.Models.Report
{
    public class RollsheetModel
    {
        public class PersonInfo
        {
            public int PeopleId { get; set; }
            public string Name { get; set; }
            public string Name2 { get; set; }
            public string BirthDate { get; set; }
            public string Age { get; set; }
            public string Address { get; set; }
            public string Address2 { get; set; }
            public string CityStateZip { get; set; }
            public int PhonePref { get; set; }
            public string HomePhone { get; set; }
            public string CellPhone { get; set; }
            public string WorkPhone { get; set; }
            public string MemberStatus { get; set; }
            public string Email { get; set; }
            public bool HasTag { get; set; }
            public string BFTeacher { get; set; }
            public int? BFTeacherId { get; set; }
            public DateTime? LastAttended { get; set; }
        }
        public class PersonMemberInfo : PersonInfo
        {
            public string MemberTypeCode { get; set; }
            public string MemberType { get; set; }
            public int MemberTypeId { get; set; }
            public DateTime? InactiveDate { get; set; }
            public decimal? AttendPct { get; set; }
            public DateTime? Joined { get; set; }
        }
        public class PersonVisitorInfo : PersonInfo
        {
            public string VisitorType { get; set; }
            public string NameParent1 { get; set; }
            public string NameParent2 { get; set; }
        }
        // This gets current org members
        public static IEnumerable<PersonMemberInfo> FetchOrgMembers(int orgid, int[] groups)
        {
            if (groups == null)
                groups = new int[] { 0 };
            var tagownerid = Util2.CurrentTagOwnerId;
            var q = from om in DbUtil.Db.OrganizationMembers
                    where om.OrganizationId == orgid
                    where om.OrgMemMemTags.Any(mt => groups.Contains(mt.MemberTagId)) || (groups[0] == 0)
                    where !groups.Contains(-1) || (groups.Contains(-1) && om.OrgMemMemTags.Count() == 0)
                    where (om.Pending ?? false) == false
                    where om.MemberTypeId != MemberTypeCode.InActive
                    where om.EnrollmentDate <= Util.Now
                    orderby om.Person.LastName, om.Person.FamilyId, om.Person.Name2
                    let p = om.Person
                    select new PersonMemberInfo
                    {
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
                        BFTeacher = p.BFClass.LeaderName,
                        BFTeacherId = p.BFClass.LeaderId,
                        Age = p.Age.ToString(),
                        MemberTypeCode = om.MemberType.Code,
                        MemberType = om.MemberType.Description,
                        MemberTypeId = om.MemberTypeId,
                        InactiveDate = om.InactiveDate,
                        AttendPct = om.AttendPct,
                        LastAttended = om.LastAttended,
                        HasTag = p.Tags.Any(t => t.Tag.Name == Util2.CurrentTagName && t.Tag.PeopleId == tagownerid),
                        Joined = om.EnrollmentDate,
                    };
            return q;
        }
        // This gets OrgMembers as of the date of the meeting and 7 days back.
        private static IEnumerable<PersonMemberInfo> FetchOrgMembers(int OrganizationId, DateTime MeetingDate)
        {

            var tagownerid = Util2.CurrentTagOwnerId;
            var q = from p in DbUtil.Db.People
                    let etlist = p.EnrollmentTransactions.Where(ee =>
                        ee.TransactionTypeId <= 3 // things that start a change
                        && ee.TransactionStatus == false
                        && ee.TransactionDate <= MeetingDate // transaction starts <= looked for end
                        && (ee.Pending ?? false) == false
                        && (ee.NextTranChangeDate >= MeetingDate || ee.NextTranChangeDate == null)// transaction ends >= looked for start
                        && ee.OrganizationId == OrganizationId)
                    let enrolled = etlist.OrderByDescending(laet => laet.TransactionDate).FirstOrDefault()
                    where enrolled != null && enrolled.MemberTypeId != MemberTypeCode.InActive
                    orderby p.LastName, p.FamilyId, p.PreferredName
                    select new PersonMemberInfo
                    {
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
                        BFTeacher = p.BFClass.LeaderName,
                        BFTeacherId = p.BFClass.LeaderId,
                        Age = p.Age.ToString(),
                        MemberTypeCode = enrolled.MemberType.Code,
                        MemberType = enrolled.MemberType.Description,
                        MemberTypeId = enrolled.MemberTypeId,
                        Joined = enrolled.EnrollmentDate ?? enrolled.TransactionDate,
                    };
            return q;
        }

        private static int[] VisitAttendTypes = new int[] 
        { 
            AttendTypeCode.VisitingMember,
            AttendTypeCode.RecentVisitor, 
            AttendTypeCode.NewVisitor 
        };

        public static IEnumerable<PersonVisitorInfo> FetchVisitors(int orgid, DateTime MeetingDate)
        {
            var wks = 3; // default lookback
            var org = DbUtil.Db.Organizations.Single(o => o.OrganizationId == orgid);
            if (org.RollSheetVisitorWks.HasValue)
                wks = org.RollSheetVisitorWks.Value;
            var dt = MeetingDate.AddDays(wks * -7);

            var q = from p in DbUtil.Db.People
                    where p.Attends.Any(a => a.AttendanceFlag == true
                        && (a.MeetingDate >= dt && a.MeetingDate <= MeetingDate)
                        && a.OrganizationId == orgid
                        && (a.MeetingDate >= a.Organization.FirstMeetingDate || a.Organization.FirstMeetingDate == null)
                        && VisitAttendTypes.Contains(a.AttendanceTypeId.Value))
                    where !p.OrganizationMembers.Any(om => om.OrganizationId == orgid)
                    orderby p.Name2, p.Name
                    orderby p.LastName, p.FamilyId, p.Name2
                    select new PersonVisitorInfo
                    {
                        VisitorType = p.MemberStatusId == MemberStatusCode.Member ? "VM" : "VS",
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
                        BFTeacher = p.BFClass.LeaderName,
                        BFTeacherId = p.BFClass.LeaderId,
                        Age = p.Age.ToString(),
                        LastAttended = DbUtil.Db.LastAttended(orgid, p.PeopleId),
                        HasTag = p.Tags.Any(t => t.Tag.Name == Util2.CurrentTagName && t.Tag.PeopleId == Util2.CurrentTagOwnerId),
                        NameParent1 = p.Family.HohName,
                        NameParent2 = p.Family.People.Where(x =>
                            x.FamilyPosition.Id == PositionInFamily.PrimaryAdult
                            && x.PeopleId != p.Family.HeadOfHouseholdId).FirstOrDefault().Name,
                    };
            return q;
        }
        public static IEnumerable<AttendInfo> RollList(int? MeetingId, int OrgId, DateTime MeetingDate)
        {
            var q = from a in DbUtil.Db.Attends
                    where a.MeetingId == MeetingId
                    where a.EffAttendFlag == null || a.EffAttendFlag == true
                    select a;

            var qm = FetchOrgMembers(OrgId, MeetingDate);

            var q1 = from p in qm
                     join pa in q on p.PeopleId equals pa.PeopleId into j
                     from pa in j.DefaultIfEmpty()
                     where MeetingDate > p.Joined // they were either a member at the time
                            // or they attended as a member (workaround for bad transaction history)
                            || (pa != null && !VisitAttendTypes.Contains(pa.AttendanceTypeId.Value))
                     select new AttendInfo
                     {
                         PeopleId = p.PeopleId,
                         Name = p.Name2,
                         Attended = pa != null ? pa.AttendanceFlag : false,
                         Member = true,
                         CurrMemberType = p.MemberType,
                         MemberType = pa != null ? (pa.MemberType != null ? pa.MemberType.Description : "") : "",
                         AttendType = pa != null ? (pa.AttendType != null ? pa.AttendType.Description : "") : "",
                         Age = p.Age,
                         OtherAttend = pa != null ? (int?)pa.OtherAttends : null
                     };
            var q2 = from p in FetchVisitors(OrgId, MeetingDate)
                     join pa in q on p.PeopleId equals pa.PeopleId into j
                     from pa in j.DefaultIfEmpty()
                     select new AttendInfo
                     {
                         PeopleId = p.PeopleId,
                         Name = p.Name2,
                         Attended = pa != null ? pa.AttendanceFlag : false,
                         Member = false,
                         CurrMemberType = "",
                         MemberType = pa != null ? (pa.MemberType != null ? pa.MemberType.Description : "") : "",
                         AttendType = pa != null ? (pa.AttendType != null ? pa.AttendType.Description : "") : "",
                         Age = p.Age,
                         OtherAttend = pa != null ? (int?)pa.OtherAttends : null
                     };
            var q3 = from p in q1.Union(q2)
                     select new AttendInfo
                     {
                         PeopleId = p.PeopleId,
                         Name = p.Name,
                         Attended = p.Attended,
                         Member = p.Member,
                         CurrMemberType = p.CurrMemberType,
                         MemberType = p.MemberType,
                         AttendType = p.AttendType,
                         OtherAttend = p.OtherAttend
                     };
            return q3;
        }
        public class AttendInfo
        {
            public int PeopleId { get; set; }
            public string Name { get; set; }
            public string Age { get; set; }
            public bool Attended { get; set; }
            public bool CanAttend { get; set; }
            public bool Member { get; set; }
            public string CurrMemberType { get; set; }
            public string MemberType { get; set; }
            public string AttendType { get; set; }
            public int? OtherAttend { get; set; }
        }
    }
}