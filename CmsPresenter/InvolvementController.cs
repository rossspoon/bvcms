using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CmsData;
using System.Data.Linq;
using System.ComponentModel;
using UtilityExtensions;
using CMSPresenter.InfoClasses;
using System.Collections;

namespace CMSPresenter
{
    public class InvolvementController
    {
        public class ActivityInfo
        {
            public string Name { get; set; }
            public decimal? Pct { get; set; }
        }
        public static IEnumerable<InvovementInfo> InvolvementList(int queryid)
        {
            var Db = DbUtil.Db;
            var qB = Db.LoadQueryById(queryid);
            var q = Db.People.Where(qB.Predicate());
            var q2 = from p in q
                     orderby p.LastName, p.FirstName
                     let spouse = Db.People.SingleOrDefault(w => p.SpouseId == w.PeopleId)
                     let om = p.OrganizationMembers.SingleOrDefault(m => m.OrganizationId == p.BibleFellowshipClassId)
                     select new InvovementInfo
                     {
                         PeopleId = p.PeopleId,
                         Addr = p.PrimaryAddress,
                         Addr2 = p.PrimaryAddress2,
                         City = p.PrimaryCity,
                         State = p.PrimaryState,
                         Zip = p.PrimaryZip,
                         Name = p.Name,
                         HomePhone = p.HomePhone,
                         WorkPhone = p.WorkPhone,
                         CellPhone = p.CellPhone,
                         DivName = om.Organization.DivOrgs.FirstOrDefault(d => 
                             d.Division.Program.Name != DbUtil.MiscTagsString).Division.Name,
                         OrgName = om.Organization.OrganizationName,
                         Teacher = p.BibleFellowshipTeacher,
                         MemberType = om.MemberType.Description,
                         AttendPct = om.AttendPct,

                         Age = p.Age,
                         Spouse = spouse != null ? spouse.FirstName : "",

                         activities = from m in p.OrganizationMembers
                                      where m.Organization.SecurityTypeId != 3 || !Util.OrgMembersOnly
                                      select new ActivityInfo
                                      {
                                          Name = m.Organization.OrganizationName,
                                          Pct = m.AttendPct,
                                      },

                         JoinInfo = p.JoinType.Description + " , " + p.JoinDate.ToString().Substring(0, 11),
                         Notes = "",
                         OfficeUseOnly = "",
                         LastName = p.LastName,
                         FirstName = p.PreferredName
                     };
            return q2;
        }
        public static IEnumerable ChildrenList(int queryid, int maximumRows)
        {
            var Db = DbUtil.Db;
            var qB = Db.LoadQueryById(queryid);
            var q = Db.People.Where(qB.Predicate());
            var q2 = from p in q
                     select new
                     {
                         PeopleId = p.PeopleId,
                         Title = p.TitleCode,
                         FirstName = p.NickName == null ? p.FirstName : p.NickName,
                         LastName = p.LastName,
                         Address = p.PrimaryAddress,
                         Address2 = p.PrimaryAddress2,
                         City = p.PrimaryCity,
                         State = p.PrimaryState,
                         Zip = p.PrimaryZip.FmtZip(),
                         Email = p.EmailAddress,
                         BirthDate = Util.FormatBirthday(p.BirthYear, p.BirthMonth, p.BirthDay),
                         JoinDate = p.JoinDate.FormatDate(),
                         HomePhone = p.HomePhone.FmtFone(),
                         CellPhone = p.CellPhone.FmtFone(),
                         WorkPhone = p.WorkPhone.FmtFone(),
                         PhonePref = p.PhonePrefId,
                         MemberStatus = p.MemberStatus.Description,
                         BFTeacher = p.BibleFellowshipTeacher,
                         Age = p.Age.ToString(),
                         School = p.SchoolOther,
                         Grade = p.Grade.ToString(),
                         EmergencyContact = string.Join(" ", (from om in p.OrganizationMembers
                                                              where om.EmergencyContactInfo != null
                                                              select om.EmergencyContactInfo).ToArray()),
                         WhoBringsChild = string.Join(" ", (from om in p.OrganizationMembers
                                                            where om.WhoBringsChildInfo != null
                                                            select om.WhoBringsChildInfo).ToArray()),
                     };
            return q2.Take(maximumRows);
        }
        public static IEnumerable ChurchList(int queryid, int maximumRows)
        {
            var Db = DbUtil.Db;
            var qB = Db.LoadQueryById(queryid);
            var q = Db.People.Where(qB.Predicate());
            var q2 = from p in q
                     select new
                     {
                         PeopleId = p.PeopleId,
                         Title = p.TitleCode,
                         FirstName = p.NickName == null ? p.FirstName : p.NickName,
                         LastName = p.LastName,
                         Address = p.PrimaryAddress,
                         Address2 = p.PrimaryAddress2,
                         City = p.PrimaryCity,
                         State = p.PrimaryState,
                         Zip = p.PrimaryZip.FmtZip(),
                         Email = p.EmailAddress,
                         BirthDate = Util.FormatBirthday(p.BirthYear, p.BirthMonth, p.BirthDay),
                         JoinDate = p.JoinDate.FormatDate(),
                         HomePhone = p.HomePhone.FmtFone(),
                         CellPhone = p.CellPhone.FmtFone(),
                         WorkPhone = p.WorkPhone.FmtFone(),
                         PhonePref = p.PhonePrefId,
                         MemberStatus = p.MemberStatus.Description,
                         BFTeacher = p.BibleFellowshipTeacher,
                         Age = p.Age.ToString(),
                         PrevChurch = p.OtherPreviousChurch,
                         NewChurch = p.OtherNewChurch,
                     };
            return q2.Take(maximumRows);
        }

        public static IEnumerable AttendList(int queryid, int maximumRows)
        {

            var Db = DbUtil.Db;
            var qB = Db.LoadQueryById(queryid);
            var q = Db.People.Where(qB.Predicate());
            var q2 = from p in q
                     let bfm = Db.OrganizationMembers.SingleOrDefault(om => om.OrganizationId == p.BibleFellowshipClassId && om.PeopleId == p.PeopleId)
                     select new
                     {
                         PeopleId = p.PeopleId,
                         Title = p.TitleCode,
                         FirstName = p.NickName == null ? p.FirstName : p.NickName,
                         LastName = p.LastName,
                         Address = p.PrimaryAddress,
                         Address2 = p.PrimaryAddress2,
                         City = p.PrimaryCity,
                         State = p.PrimaryState,
                         Zip = p.PrimaryZip.FmtZip(),
                         Email = p.EmailAddress,
                         BirthDate = Util.FormatBirthday(p.BirthYear, p.BirthMonth, p.BirthDay),
                         JoinDate = p.JoinDate.FormatDate(),
                         HomePhone = p.HomePhone.FmtFone(),
                         CellPhone = p.CellPhone.FmtFone(),
                         WorkPhone = p.WorkPhone.FmtFone(),
                         PhonePref = p.PhonePrefId,
                         MemberStatus = p.MemberStatus.Description,
                         BFTeacher = p.BibleFellowshipTeacher,
                         Age = p.Age.ToString(),
                         School = p.SchoolOther,
                         Grade = p.Grade.ToString(),
                         LastAttend = bfm.LastAttended.ToString(),
                         AttendPct = bfm.AttendPct.ToString(),
                         AttendStr = bfm.AttendStr,
                     };
            return q2.Take(maximumRows);
        }
    }
}
