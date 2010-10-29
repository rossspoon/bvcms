using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CmsData;
using System.Data.Linq;
using System.ComponentModel;
using UtilityExtensions;
using CMSPresenter.InfoClasses;


namespace CMSPresenter
{
    public class InReachController
    {
        public IEnumerable<InReachInfo> ListByQuery(int qid)
        {
            var q = DbUtil.Db.PeopleQuery(qid);
            return FetchList(q);
        }

        private static IEnumerable<InReachInfo> FetchList(IQueryable<Person> query)
        {
            var Db = query.GetDataContext() as CMSDataContext;
            var q = from p in query
                    orderby p.LastName, p.Name
                    //let attendct = Db.Attendances
                    //             .Count(a => a.Meeting.OrganizationId == p.BibleFellowshipClassId && a.PeopleId == p.PeopleId && a.AttendanceFlag == true)
                    //let lastattend = Db.Attendances
                    //             .Where(a => a.Meeting.OrganizationId == p.BibleFellowshipClassId && a.PeopleId == p.PeopleId && a.AttendanceFlag == true)
                    //             .Max(a => a.MeetingDate)
                    //let status = Db.OrganizationMembers
                    //             .Count(m => m.PeopleId == p.PeopleId && m.OrganizationId == p.BibleFellowshipClassId) == 0 ?
                    //             "visitor" : "member"
                    //let attendpct = Db.OrganizationMembers
                    //             .Where(ap => ap.PeopleId == p.PeopleId && ap.OrganizationId == p.BibleFellowshipClassId)
                    //             .Max(ap => ap.AttendPct)
                    //let attendstr = Db.OrganizationMembers
                    //             .Where(astr => astr.PeopleId == p.PeopleId && astr.OrganizationId == p.BibleFellowshipClassId)
                    //             .Max(astr => astr.AttendStr)
                    //let divisionid = Db.Organizations
                    //              .Where(a => a.OrganizationId == p.BibleFellowshipClassId)
                    //              .Max(b => b.Division.DivisionId)
                    //let divisionname = Db.Organizations.Where(a => a.OrganizationId == p.BibleFellowshipClassId).Max(b => b.Division.DivisionName)
                    select new InReachInfo
                     {
                         PeopleId = p.PeopleId,
                         LastName = p.LastName,
                         FirstName = p.FirstName,
                         Street = p.PrimaryAddress,
                         Street2 = p.PrimaryAddress2,
                         Birthday = Util.FormatBirthday(p.BirthYear, p.BirthMonth, p.BirthDay),
                         EmailHome = p.EmailAddress,
                         Phone = p.HomePhone.FmtFone(),
                         City = p.PrimaryCity,
                         State = p.PrimaryState,
                         Zip = p.PrimaryZip.FmtZip(),
                         AttendPct = (Db.OrganizationMembers
                                     .Where(ap => ap.PeopleId == p.PeopleId && ap.OrganizationId == p.BibleFellowshipClassId)
                                     .Max(ap => ap.AttendPct))/100,
                         AttendStr = (Db.OrganizationMembers
                                     .Where(astr => astr.PeopleId == p.PeopleId && astr.OrganizationId == p.BibleFellowshipClassId)
                                     .Max(astr => astr.AttendStr)).FmtAttendStr(),
                         ContactNotes = "",
                         DivisionName = p.OrganizationMembers.FirstOrDefault(om => 
                             om.OrganizationId == p.BibleFellowshipClassId).Organization
                             .DivOrgs.FirstOrDefault(d => d.Division.Program.Name != DbUtil.MiscTagsString).Division.Name,
                         Fullname = p.Name,
                         OrganizationId = p.BibleFellowshipClassId ?? 0,
                         OrganizationName = p.BFClass.OrganizationName,
                         CellPhone = p.CellPhone ,
                         HomePhone = p.HomePhone,
                         WorkPhone = p.WorkPhone,
                         CityStateZip = Util.FormatCSZ4(p.PrimaryCity,p.PrimaryState,p.PrimaryZip)

                     };

            return q;

        }

        public IEnumerable<InReachInfo> InReachOrgList(int orgid)
        {
            var pids = from om in DbUtil.Db.OrganizationMembers
                       from m in om.Organization.Meetings
                       from a in m.Attends
                       where om.OrganizationId == orgid
                       group new {a, m}  by new {PeopleId = a.PeopleId, OrgId = m.OrganizationId} into g
                       select g into h
                       where h.Max(z => z.a.MeetingDate) < Util.Now.Date.AddDays(-45)
                       select h;

            var q = from p in DbUtil.Db.People
                    join pid in pids on p.PeopleId equals pid.Key.PeopleId
                    let attendct = DbUtil.Db.Attends
                                     .Count(a => a.OrganizationId == pid.Key.OrgId && a.PeopleId == p.PeopleId && a.AttendanceFlag == true)
                    let lastattend = DbUtil.Db.Attends
                                     .Where(a => a.OrganizationId == pid.Key.OrgId && a.PeopleId == p.PeopleId && a.AttendanceFlag == true)
                                     .Max(a => a.MeetingDate)
                    let status = DbUtil.Db.OrganizationMembers
                                     .Count(m => m.PeopleId == p.PeopleId && m.OrganizationId == pid.Key.OrgId) == 0 ?
                                     "visitor" : "member"
                    let attendpct = DbUtil.Db.OrganizationMembers
                                     .Where(ap => ap.PeopleId == p.PeopleId && ap.OrganizationId == pid.Key.OrgId)
                                     .Max(ap => ap.AttendPct)
                    let attendstr = DbUtil.Db.OrganizationMembers
                                     .Where(astr => astr.PeopleId == p.PeopleId && astr.OrganizationId == pid.Key.OrgId)
                                     .Max(astr => astr.AttendStr)
                    let divisionname = DbUtil.Db.Organizations.SingleOrDefault(a => a.OrganizationId == pid.Key.OrgId)
                        .DivOrgs.FirstOrDefault(d => d.Division.Program.Name != DbUtil.MiscTagsString).Division.Name
                     select new InReachInfo
                     {
                         PeopleId = p.PeopleId,
                         LastName = p.LastName,
                         FirstName = p.FirstName,
                         Street = p.PrimaryAddress,
                         Street2 = p.PrimaryAddress2,
                         Birthday = UtilityExtensions.Util.FormatBirthday(p.BirthYear, p.BirthMonth, p.BirthDay),
                         EmailHome = p.EmailAddress,
                         Phone = p.HomePhone.FmtFone(),
                         City = p.PrimaryCity,
                         State = p.PrimaryState,
                         Zip = p.PrimaryZip.FmtZip(),
                         Status = status,
                         AttendCt = attendct,
                         LastAttend = lastattend,
                         AttendPct = attendpct,
                         AttendStr = attendstr.FmtAttendStr(),
                         ContactNotes = "",
                         DivisionName = divisionname,
                         Fullname = p.Name,
                         OrganizationId = pid.Key.OrgId,
                         OrganizationName = DbUtil.Db.Organizations
                                            .Where(a => a.OrganizationId == pid.Key.OrgId)
                                            .Max(b => b.OrganizationName) + " - " + p.BFClass.LeaderName

                     };

                    
            return q;
            
        }
        public IEnumerable<InReachInfo> InReachDivisionList(int divid)
        {
            var pids = from om in DbUtil.Db.OrganizationMembers
                       from m in om.Organization.Meetings
                       from a in m.Attends
                       where om.Organization.DivOrgs.Any(di => di.DivId == divid)
                       group new { a, m } by new { PeopleId = a.PeopleId, OrgId = m.OrganizationId } into g
                       select g into h
                       where h.Max(z => z.a.MeetingDate) < Util.Now.Date.AddDays(-45)
                       select h;

            var q = from p in DbUtil.Db.People
                    join pid in pids on p.PeopleId equals pid.Key.PeopleId
                    let attendct = DbUtil.Db.Attends
                                    .Count(a => a.OrganizationId == pid.Key.OrgId && a.PeopleId == p.PeopleId && a.AttendanceFlag == true)
                    let lastattend = DbUtil.Db.Attends
                                    .Where(a => a.OrganizationId == pid.Key.OrgId && a.PeopleId == p.PeopleId && a.AttendanceFlag == true)
                                    .Max(a => a.MeetingDate)
                    let status = DbUtil.Db.OrganizationMembers
                                    .Count(m => m.PeopleId == p.PeopleId && m.OrganizationId == pid.Key.OrgId) == 0 ?
                                    "visitor" : "member"
                    let attendpct = DbUtil.Db.OrganizationMembers
                                    .Where(ap => ap.PeopleId == p.PeopleId && ap.OrganizationId == pid.Key.OrgId)
                                    .Max(ap => ap.AttendPct)
                    let attendstr = DbUtil.Db.OrganizationMembers
                                    .Where(astr => astr.PeopleId == p.PeopleId && astr.OrganizationId == pid.Key.OrgId)
                                    .Max(astr => astr.AttendStr)
                    let divisionname = p.OrganizationMembers.Single(om => om.OrganizationId == pid.Key.OrgId).Organization
                       .DivOrgs.FirstOrDefault(d => d.Division.Program.Name != DbUtil.MiscTagsString).Division.Name
                    select new InReachInfo
                    {
                        PeopleId = p.PeopleId,
                        LastName = p.LastName,
                        FirstName = p.FirstName,
                        Street = p.PrimaryAddress,
                        Street2 = p.PrimaryAddress2,
                        Birthday = UtilityExtensions.Util.FormatBirthday(p.BirthYear, p.BirthMonth, p.BirthDay),
                        EmailHome = p.EmailAddress,
                        Phone = p.HomePhone.FmtFone(),
                        City = p.PrimaryCity,
                        State = p.PrimaryState,
                        Zip = p.PrimaryZip.FmtZip(),
                        Status = status,
                        AttendCt = attendct,
                        LastAttend = lastattend,
                        AttendPct = attendpct,
                        AttendStr = attendstr.FmtAttendStr(),
                        ContactNotes = "",
                        DivisionName = divisionname,
                        Fullname = p.Name,
                        OrganizationId = pid.Key.OrgId,
                        OrganizationName = DbUtil.Db.Organizations
                                           .Where(a => a.OrganizationId == pid.Key.OrgId)
                                           .Max(b => b.OrganizationName) + " - " + p.BFClass.LeaderName

                    };

            return q;

        }
        public IEnumerable<InReachInfo> InReachQueryList(int qid)
        {
            var Qb = DbUtil.Db.LoadQueryById(qid);

            var q = from p in DbUtil.Db.People
                    join pid in Qb.PeopleIds() on p.PeopleId equals pid
                    let attendct = DbUtil.Db.Attends
                                    .Count(a => a.OrganizationId == p.BibleFellowshipClassId && p.PeopleId == p.PeopleId && a.AttendanceFlag == true)
                    let lastattend = DbUtil.Db.Attends
                                    .Where(a => a.OrganizationId == p.BibleFellowshipClassId && a.PeopleId == p.PeopleId && a.AttendanceFlag == true)
                                    .Max(a => a.MeetingDate)
                    let status = DbUtil.Db.OrganizationMembers
                                    .Count(m => m.PeopleId == p.PeopleId && m.OrganizationId == p.BibleFellowshipClassId) == 0 ? "visitor" : "member"
                    let attendpct = DbUtil.Db.OrganizationMembers
                                    .Where(ap => ap.PeopleId == p.PeopleId && ap.OrganizationId == p.BibleFellowshipClassId)
                                    .Max(ap => ap.AttendPct)
                    let attendstr = DbUtil.Db.OrganizationMembers
                                    .Where(astr => astr.PeopleId == p.PeopleId && astr.OrganizationId == p.BibleFellowshipClassId)
                                    .Max(astr => astr.AttendStr)
                    let divisionname = p.OrganizationMembers.Single(om => om.OrganizationId == p.BibleFellowshipClassId).Organization
                       .DivOrgs.FirstOrDefault(d => d.Division.Program.Name != DbUtil.MiscTagsString).Division.Name
                    select new InReachInfo
                    {
                        PeopleId = p.PeopleId,
                        LastName = p.LastName,
                        FirstName = p.FirstName,
                        Street = p.PrimaryAddress,
                        Street2 = p.PrimaryAddress2,
                        Birthday = Util.FormatBirthday(p.BirthYear, p.BirthMonth, p.BirthDay),
                        EmailHome = p.EmailAddress,
                        Phone = p.HomePhone.FmtFone(),
                        City = p.PrimaryCity,
                        State = p.PrimaryState,
                        Zip = p.PrimaryZip.FmtZip(),
                        Status = status,
                        AttendCt = attendct,
                        LastAttend = lastattend,
                        AttendPct = attendpct,
                        AttendStr = attendstr.FmtAttendStr(),
                        ContactNotes = "",
                        DivisionName = divisionname,
                        Fullname = p.Name,
                        OrganizationId = (int)p.BibleFellowshipClassId,
                        OrganizationName = DbUtil.Db.Organizations
                                           .Where(a => a.OrganizationId == p.BibleFellowshipClassId)
                                           .Max(b => b.OrganizationName) + " - " + p.BFClass.LeaderName

                    };

            return q;
        }
    }
}
