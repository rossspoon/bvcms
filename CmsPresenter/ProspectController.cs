using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using CmsData;
using CmsData.View;
using System.Collections;
using UtilityExtensions;
using CMSPresenter.InfoClasses;

namespace CMSPresenter
{
    public class ProspectController
    {
        private CMSDataContext Db;

        private List<CodeValueItem> AttendCodes;
        private List<MemberTypeItem> MemberCodes;
        public ProspectController()
        {
            Db = DbUtil.Db;
            var c = new CodeValueController();
            AttendCodes = c.AttendanceTypeCodes();
            MemberCodes = CodeValueController.MemberTypeCodes2();
        }

        public IEnumerable<ProspectInfo> ListByQuery(int qid)
        {
            var q = Db.PeopleQuery(qid);
            return FetchList(q);
        }

        private static IEnumerable<ProspectInfo> FetchList(IQueryable<Person> query)
        {
            var Db = query.GetDataContext() as CMSDataContext;
            var q = from p in query
                    orderby p.PrimaryZip, p.LastName, p.Name
                    select new ProspectInfo
                    {
                        Name = p.Name,
                        Address = p.PrimaryAddress,
                        Address2 = p.PrimaryAddress2,
                        Age = p.Age != null ? p.Age.ToString() : "",
                        MemberStatus = p.MemberStatus.Description,
                        CityStateZip = Util.FormatCSZ4(p.PrimaryCity, p.PrimaryState, p.PrimaryZip),
                        Email = p.EmailAddress,
                        PeopleId = p.PeopleId,
                        Gender = p.GenderId == 1 ? "Male" : p.GenderId == 2 ? "Female" : "",
                        MaritalStatus = p.MaritalStatus.Description,
                        PositionInFamily = p.FamilyPosition.Description,
                        Origin = p.Origin.Description,
                        BFCStatus = "",
                        Comment = p.Comments,
                        ChristAsSavior = p.ChristAsSavior ? "Prayed to receive Christ as Savior" : "",
                        InterestedInJoining = p.InterestedInJoining ? "Interested in joining Church" : "",
                        PleaseVisit = p.PleaseVisit ? "Requests a visit" : "",
                        InfoBecomeAChristian = p.InfoBecomeAChristian ? "Interested in becoming a Christian" : "",
                        CellPhone = p.CellPhone,
                        HomePhone = p.HomePhone,
                        WorkPhone = p.WorkPhone,
                    };
            return q;

         }

        //[DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<FamilyMember> FamilyMembers(int PersonId)
        {
            var q = from p in Db.People
                    from m in p.Family.People
                    where p.PeopleId == PersonId
                      && m.DeceasedDate == null
                    orderby m.PositionInFamilyId, m.Age descending
                    select new FamilyMember
                   {
                       Id = m.PeopleId,
                       Name = m.Name,
                       Age = m.Age,
                       Deceased = m.DeceasedDate != null,
                       PositionInFamily = m.FamilyPosition.Description,
                       MemberStatus = m.MemberStatus.Description
                   };
            return q;
        }


        //[DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<OrganizationView> EnrollData(int pid)
        {
            var dt = Util.Now.Date.AddDays(1);
            var q = from o in Db.Organizations
                    from om in o.OrganizationMembers
                    where om.PeopleId == pid
                       && dt > om.EnrollmentDate
                    let l = Db.People.SingleOrDefault(p => p.PeopleId == o.LeaderId)
                    orderby o.OrganizationName
                    select new OrganizationView
                    {
                        Id = o.OrganizationId,
                        Name = o.OrganizationName,
                        Location = o.Location,
                        LeaderName = l.Name,
                        MeetingTime = o.MeetingTime,
                        MemberType = om.MemberType.Description,
                        LeaderId = o.LeaderId,
                        EnrollDate = om.EnrollmentDate,
                        DivisionName = o.DivOrgs.FirstOrDefault().Division.Name
                    };
            return q;
        }

        //[DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<OrganizationView> PreviousEnrollData(int pid)
        {
            var q = from o in Db.Organizations
                    from etd in o.EnrollmentTransactions
                    where etd.TransactionTypeId >= 4 && etd.PeopleId == pid
                    where etd.TransactionStatus == false
                    let ete = Db.EnrollmentTransactions.SingleOrDefault(ete => ete.TransactionId == etd.EnrollmentTransactionId)
                    orderby o.OrganizationName
                    select new OrganizationView
                    {
                        Id = etd.OrganizationId,
                        Name = etd.OrganizationName,
                        Location = o.Location,
                        LeaderName = o.LeaderName,
                        MeetingTime = o.MeetingTime,
                        LeaderId = o.LeaderId,
                        EnrollDate = ete.TransactionDate,
                        DropDate = etd.TransactionDate,
                        MemberType = ete.MemberType.Description,
                    };
            return q;
        }
        //[DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<OrganizationView> PendingEnrollData(int pid)
        {
            var dt = Util.Now;
            var q = from o in Db.Organizations
                    from et in o.EnrollmentTransactions
                    where et.TransactionTypeId == 1 && et.PeopleId == pid
                        && et.TransactionDate > dt
                    where et.TransactionStatus == false
                    orderby o.OrganizationName
                    select new OrganizationView
                    {
                        Id = et.OrganizationId,
                        Name = et.OrganizationName,
                        Location = o.Location,
                        LeaderName = o.LeaderName,
                        MeetingTime = o.MeetingTime,
                        LeaderId = o.LeaderId,
                        EnrollDate = et.TransactionDate,
                        MemberType = et.MemberType.Description,
                    };
            return q;
        }

        public IEnumerable<ProspectAttendInfo> AttendHistory(int pid)
        {
            var q = Db.Attends.Where(a => a.PeopleId == pid && a.AttendanceFlag == true);
            var q2 = from a in q
                     let o = a.Organization
                     orderby a.MeetingDate descending, o.OrganizationName
                     select new ProspectAttendInfo
                     {
                         MeetingId = a.MeetingId,
                         OrganizationId = a.Meeting.OrganizationId,
                         OrganizationName = o.OrganizationName,
                         Teacher = o.LeaderName,
                         AttendType = AttendCodes.ItemValue(a.AttendanceTypeId),
                         //MeetingName = o.Tags.First().Tag.Name + ": " + a.MeetingName,
                         MeetingName = o.DivOrgs.First().Division.Name + ": " + o.OrganizationName,
                         //MeetingName = o.OrganizationName + ":" + o.LeaderName + (o.Location == "" ? "" : ", " + o.Location),
                         MeetingDate = a.MeetingDate,
                         MemberType = MemberCodes.ItemValue(a.MemberTypeId),
                     };
            return q2.Take(10);
        }
        public class OrganizationView
        {
            public string Name { get; set; }
            public int Id { get; set; }
            public string Location { get; set; }
            public string LeaderName { get; set; }
            public DateTime? MeetingTime { get; set; }
            public string Schedule { get { return "{0:dddd h:mm tt}".Fmt(MeetingTime); } }
            public int? LeaderId { get; set; }
            public string MemberType { get; set; }
            public DateTime? EnrollDate { get; set; }
            public DateTime? DropDate { get; set; }
            public string DivisionName { get; set; }
            public decimal? AttendPct { get; set; }
        }
    }
}
