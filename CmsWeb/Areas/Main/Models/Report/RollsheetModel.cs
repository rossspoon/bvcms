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
        public IEnumerable<PersonMemberInfo> FetchOrgMembers (int orgid, int[] groups)
        {
            if (groups == null)
                groups = new int[] { 0 };
            var tagownerid = Util2.CurrentTagOwnerId;
            var q = from om in DbUtil.Db.OrganizationMembers
                    where om.OrganizationId == orgid
                    where om.OrgMemMemTags.Any(mt => groups.Contains(mt.MemberTagId)) || (groups[0] == 0)
                    where !groups.Contains(-1) || (groups.Contains(-1) && om.OrgMemMemTags.Count() == 0)
                    where (om.Pending ?? false) == false
                    where om.MemberTypeId != (int)OrganizationMember.MemberTypeCode.InActive
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

        private static int[] VisitAttendTypes = new int[] 
        { 
            (int)Attend.AttendTypeCode.VisitingMember,
            (int)Attend.AttendTypeCode.RecentVisitor, 
            (int)Attend.AttendTypeCode.NewVisitor 
        };

        public IEnumerable<PersonVisitorInfo> FetchVisitors(int orgid, DateTime MeetingDate)
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
                        && VisitAttendTypes.Contains(a.AttendanceTypeId.Value)
                        && a.MeetingDate >= a.Organization.FirstMeetingDate)
                    where !p.OrganizationMembers.Any(om => om.OrganizationId == orgid)
                    orderby p.Name2, p.Name                           
                    orderby p.LastName, p.FamilyId, p.Name2
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
                        BFTeacher = p.BFClass.LeaderName,
                        BFTeacherId = p.BFClass.LeaderId,
                        Age = p.Age.ToString(),
                        LastAttended = DbUtil.Db.LastAttended(orgid, p.PeopleId),
                        HasTag = p.Tags.Any(t => t.Tag.Name == Util2.CurrentTagName && t.Tag.PeopleId == Util2.CurrentTagOwnerId),
                        NameParent1 = p.Family.HohName,
                        NameParent2 = p.Family.People.Where(x => 
                            x.FamilyPosition.Id == (int)Family.PositionInFamily.PrimaryAdult 
                            && x.PeopleId != p.Family.HeadOfHouseholdId).FirstOrDefault().Name,
                    };
            return q;
        }
    }
}