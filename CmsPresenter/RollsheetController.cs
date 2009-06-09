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

namespace CMSPresenter
{
    public class RollsheetController
    {
        public IEnumerable<OrganizationInfo> FetchOrgsList(string name, int DivId, int SchedId, int StatusId)
        {
            return (new OrganizationSearchController().FetchOrganizationExcelList(name, DivId, SchedId, StatusId));
        }

        public IEnumerable<OrganizationInfo> FetchOrgsList(int oid, DateTime MeetingDate)
        {
            return (new AttendenceController().GetOrganizationInfo(oid));
        }

        public IEnumerable<PersonMemberInfo> FetchOrgMembers (int orgid)
        {
            var q = from om in DbUtil.Db.OrganizationMembers
                    where om.OrganizationId == orgid
                    where om.MemberTypeId != (int)OrganizationMember.MemberTypeCode.InActive
                    where om.EnrollmentDate <= Util.Now
                    orderby om.Person.Name2
                    select om;
            var q2 = OrganizationController.FetchPeopleList(q);
            return q2;
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
                    )
                    where !p.OrganizationMembers.Any(om => om.OrganizationId == orgid)
                    select p.PeopleId;

            var r = from pid in q
                    join p in DbUtil.Db.People on pid equals p.PeopleId
                    let f = p.Family
                    orderby p.Name2, p.Name
                    select new PersonVisitorInfo
                    {
                        VisitorType = p.MemberStatusId == (int)Person.MemberStatusCode.Member ? "VM" : "VS",
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
                        LastAttended = DbUtil.Db.LastAttended(orgid, p.PeopleId),
                        HasTag = p.Tags.Any(t => t.Tag.Name == Util.CurrentTagName && t.Tag.PeopleId == Util.CurrentTagOwnerId),
                        NameParent1 = f.HohName,
                        NameParent2 = p.Family.People.Where(x => x.FamilyPosition.Id == 10 & x.PeopleId != f.HeadOfHouseholdId).FirstOrDefault().Name,
                    };


            return r;
        }
    }
}