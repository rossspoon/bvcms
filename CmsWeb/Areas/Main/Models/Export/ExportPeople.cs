using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using CmsData;
using UtilityExtensions;
using System.Data.SqlClient;

namespace CmsWeb.Models
{
    public class ExportPeople
    {
        public static IEnumerable FetchExcelLibraryList(int queryid)
        {
            var Db = DbUtil.Db;
            var query = Db.PeopleQuery(queryid);
            var q = from p in query
                    let om = p.OrganizationMembers.SingleOrDefault(om => om.OrganizationId == p.BibleFellowshipClassId)
                    select new
                    {
                        PeopleId = p.PeopleId,
                        FirstName = p.PreferredName,
                        LastName = p.LastName,
                        Address = p.PrimaryAddress,
                        City = p.PrimaryCity,
                        State = p.PrimaryState,
                        Zip = p.PrimaryZip.FmtZip(),
                        Email = p.EmailAddress,
                        BirthDate = Util.FormatBirthday(p.BirthYear, p.BirthMonth, p.BirthDay),
                        HomePhone = p.HomePhone.FmtFone(),
                        CellPhone = p.CellPhone.FmtFone(),
                        WorkPhone = p.WorkPhone.FmtFone(),
                        MemberStatus = p.MemberStatus.Description,
                        Married = p.MaritalStatus.Description,
                    };
            return q;
        }
        public static IEnumerable FetchExcelList(int queryid, int maximumRows)
        {
            var Db = DbUtil.Db;
            var query = Db.PeopleQuery(queryid);
            var q = from p in query
                    let om = p.OrganizationMembers.SingleOrDefault(om => om.OrganizationId == p.BibleFellowshipClassId)
                    let oid = p.PeopleExtras.FirstOrDefault(pe => pe.Field == "OtherId").Data
                    select new
                    {
                        PeopleId = p.PeopleId,
                        Title = p.TitleCode,
                        FirstName = p.PreferredName,
                        LastName = p.LastName,
                        Address = p.PrimaryAddress,
                        Address2 = p.PrimaryAddress2,
                        City = p.PrimaryCity,
                        State = p.PrimaryState,
                        Zip = p.PrimaryZip.FmtZip(),
                        Email = p.EmailAddress,
                        BirthDate = Util.FormatBirthday(p.BirthYear, p.BirthMonth, p.BirthDay),
                        BirthDay = Util.FormatBirthday(null, p.BirthMonth, p.BirthDay),
                        JoinDate = p.JoinDate.FormatDate(),
                        HomePhone = p.HomePhone.FmtFone(),
                        CellPhone = p.CellPhone.FmtFone(),
                        WorkPhone = p.WorkPhone.FmtFone(),
                        MemberStatus = p.MemberStatus.Description,
                        Age = p.Age.ToString(),
                        Married = p.MaritalStatus.Description,
                        Wedding = p.WeddingDate.FormatDate(),
                        FamilyId = p.FamilyId,
                        FamilyPosition = p.FamilyPosition.Description,
                        Gender = p.Gender.Description,
                        School = p.SchoolOther,
                        Grade = p.Grade.ToString(),
                        FellowshipLeader = p.BFClass.LeaderName,
                        AttendPctBF = (om == null ? 0 : om.AttendPct == null ? 0 : om.AttendPct.Value),
                        FellowshipClass = (om == null ? "" : om.Organization.OrganizationName),
                        AltName = p.AltName,
                        OtherId = oid ?? ""
                    };
            return q.Take(maximumRows);
        }
        public static IEnumerable ExcelContributions(int? qid, DateTime startdt, DateTime enddt)
        {
            int[] ReturnedReversedTypes = new int[] 
            { 
                (int)Contribution.TypeCode.ReturnedCheck, 
                (int)Contribution.TypeCode.Reversed 
            };
            var q = DbUtil.Db.PeopleQuery(qid.Value);
            var q2 = from c in DbUtil.Db.Contributions
                     let sp = c.Person.Family.People.SingleOrDefault(ss => ss.PeopleId == c.Person.SpouseId)
                     where q.Any(p => p.PeopleId == c.PeopleId
                         || (p.SpouseId == c.PeopleId
                            && sp.ContributionOptionsId == 2 && p.ContributionOptionsId == 2))
                     let f = c.Person.Family
                     where c.PledgeFlag != true
                     where c.ContributionStatusId == 0
                     where c.ContributionAmount > 0
                     where !ReturnedReversedTypes.Contains(c.ContributionTypeId)
                     where c.ContributionDate >= startdt && c.ContributionDate <= enddt
                     select new
                     {
                         f.FamilyId,
                         GiverId = c.PeopleId ?? 0,
                         HeadOfHouseholdId = (f.HeadOfHouseholdId == sp.PeopleId ? sp.PeopleId : c.PeopleId) ?? 0,
                         Id = c.ContributionId,
                         Name = c.Person.Name2,
                         Amount = c.ContributionAmount ?? 0,
                         Date = c.ContributionDate,
                         c.FundId,
                         c.ContributionFund.FundName,
                         c.BundleDetails.First().BundleHeader.BundleHeaderType.Description
                         //IsSpouse = f.HeadOfHouseholdId == sp.PeopleId ? true : false,
                     };
            return q2;
        }
        public static IEnumerable ExcelContributionTotals(int? qid, DateTime startdt, DateTime enddt)
        {
            int[] ReturnedReversedTypes = new int[] 
            { 
                (int)Contribution.TypeCode.ReturnedCheck, 
                (int)Contribution.TypeCode.Reversed 
            };
            var q = DbUtil.Db.PeopleQuery(qid.Value);
            var q2 = from p in q
                     let sp = p.Family.People.SingleOrDefault(ss => ss.PeopleId == p.SpouseId && ss.ContributionOptionsId == 2 && p.ContributionOptionsId == 2)
                     where p.PeopleId == p.Family.HeadOfHouseholdId || p.ContributionOptionsId != 2
                     let t = DbUtil.Db.GetTotalContributions(p.PeopleId, sp.PeopleId, startdt, enddt)
                     from r in t
                     select new
                     {
                         PeopleId = p.PeopleId,
                         Count = r.Cnt ?? 0,
                         Amount = r.Amt ?? 0m,
                         Pledged = r.Plg ?? 0m,
                         Name = p.Name2,
                         Name2 = sp.Name2 ?? "",
                         Fund = r.Fund
                     };
            return q2;
        }
        public static IEnumerable FetchExcelListFamilyMembers(int? qid)
        {
            var q = DbUtil.Db.PeopleQuery(qid.Value);
            var q2 = from pp in q
                     group pp by pp.FamilyId into g
                     from p in g.First().Family.People
                     let om = p.OrganizationMembers.SingleOrDefault(om => om.OrganizationId == p.BibleFellowshipClassId)
                     orderby p.FamilyId, p.PositionInFamilyId
                     select new
                     {
                         PeopleId = p.PeopleId,
                         Title = p.TitleCode,
                         FirstName = p.PreferredName,
                         LastName = p.LastName,
                         Address = p.PrimaryAddress,
                         Address2 = p.PrimaryAddress2,
                         City = p.PrimaryCity,
                         State = p.PrimaryState,
                         Zip = p.PrimaryZip.FmtZip(),
                         Email = p.EmailAddress,
                         BirthDate = Util.FormatBirthday(p.BirthYear, p.BirthMonth, p.BirthDay),
                         BirthDay = Util.FormatBirthday(null, p.BirthMonth, p.BirthDay),
                         JoinDate = p.JoinDate.FormatDate(),
                         HomePhone = p.HomePhone.FmtFone(),
                         CellPhone = p.CellPhone.FmtFone(),
                         WorkPhone = p.WorkPhone.FmtFone(),
                         MemberStatus = p.MemberStatus.Description,
                         Age = p.Age.ToString(),
                         School = p.SchoolOther,
                         Married = p.MaritalStatus.Description,
                         FamilyId = p.FamilyId,
                         FamilyPosition = p.PositionInFamilyId,
                         Grade = p.Grade.ToString(),
                         FellowshipLeader = p.BFClass.LeaderName,
                         AttendPctBF = (om == null ? 0 : om.AttendPct == null ? 0 : om.AttendPct.Value),
                         FellowshipClass = (om == null ? "" : om.Organization.OrganizationName),
                         AltName = p.AltName,
                     };
            return q2;
        }
        public static IEnumerable FetchExcelListFamily(int queryid)
        {
            var Db = DbUtil.Db;
            var query = Db.PeopleQuery(queryid);

            var q = from f in Db.Families
                    where query.Any(ff => ff.FamilyId == f.FamilyId)
                    let p = Db.People.Single(pp => pp.PeopleId == f.HeadOfHouseholdId)
                    let spouse = Db.People.SingleOrDefault(sp => sp.PeopleId == f.HeadOfHouseholdSpouseId)
                    let children = from pp in f.People
                                   where pp.PeopleId != f.HeadOfHouseholdId
                                   where pp.PeopleId != (f.HeadOfHouseholdSpouseId ?? 0)
                                   where pp.PositionInFamilyId == 30
                                   orderby pp.LastName == p.LastName ? 1 : 2, pp.Age descending
                                   select pp.LastName == p.LastName ? pp.PreferredName : pp.Name
                    select new
                    {
                        FamilyId = p.FamilyId,
                        LastName = p.LastName,
                        LabelName = (spouse == null ? p.PreferredName : p.PreferredName + " & " + spouse.PreferredName),
                        Children = string.Join(", ", children),
                        Address = p.AddrCityStateZip,
                        HomePhone = p.HomePhone.FmtFone(),
                        Email = p.EmailAddress,
                    };
            return q;
        }
        public static IEnumerable FetchExcelListFamily2(int queryid)
        {
            var Db = DbUtil.Db;
            var query = Db.PeopleQuery(queryid);

            var q = from p in Db.People
                    where query.Any(ff => ff.FamilyId == p.FamilyId)
                    orderby p.LastName, p.FamilyId, p.FirstName
                    where p.DeceasedDate == null
                    let om = p.OrganizationMembers.SingleOrDefault(om => om.OrganizationId == p.BibleFellowshipClassId)
                    select new
                    {
                        FamilyId = p.FamilyId,
                        PeopleId = p.PeopleId,
                        LastName = p.LastName,
                        FirstName = p.PreferredName,
                        Position = p.FamilyPosition.Description,
                        Married = p.MaritalStatus.Description,
                        Title = p.TitleCode,
                        Address = p.PrimaryAddress,
                        Address2 = p.PrimaryAddress2,
                        City = p.PrimaryCity,
                        State = p.PrimaryState,
                        Zip = p.PrimaryZip.FmtZip(),
                        Email = p.EmailAddress,
                        BirthDate = Util.FormatBirthday(p.BirthYear, p.BirthMonth, p.BirthDay),
                        BirthDay = Util.FormatBirthday(null, p.BirthMonth, p.BirthDay),
                        JoinDate = p.JoinDate.FormatDate(),
                        HomePhone = p.HomePhone.FmtFone(),
                        CellPhone = p.CellPhone.FmtFone(),
                        WorkPhone = p.WorkPhone.FmtFone(),
                        MemberStatus = p.MemberStatus.Description,
                        FellowshipLeader = p.BFClass.LeaderName,
                        Age = p.Age.ToString(),
                        School = p.SchoolOther,
                        Grade = p.Grade.ToString(),
                        AttendPctBF = (om == null ? 0 : om.AttendPct == null ? 0 : om.AttendPct.Value),
                    };
            return q;
        }
        public static IEnumerable FetchExcelListPics(int queryid, int maximumRows)
        {
            var Db = DbUtil.Db;
            var query = Db.PeopleQuery(queryid);
            var q = from p in query
                    let om = p.OrganizationMembers.SingleOrDefault(om => om.OrganizationId == p.BibleFellowshipClassId)
                    let spouse = Db.People.Where(pp => pp.PeopleId == p.SpouseId).Select(pp => pp.PreferredName).SingleOrDefault()
                    select new
                    {
                        PeopleId = p.PeopleId,
                        Title = p.TitleCode,
                        FirstName = p.PreferredName,
                        LastName = p.LastName,
                        Address = p.PrimaryAddress,
                        Address2 = p.PrimaryAddress2,
                        City = p.PrimaryCity,
                        State = p.PrimaryState,
                        Zip = p.PrimaryZip.FmtZip(),
                        Email = p.EmailAddress,
                        BirthDate = p.BirthMonth + "/" + p.BirthDay + "/" + p.BirthYear,
                        BirthDay = " " + p.BirthMonth + "/" + p.BirthDay,
                        Anniversary = " " + p.WeddingDate.Value.Month + "/" + p.WeddingDate.Value.Day,
                        JoinDate = p.JoinDate.FormatDate(),
                        JoinType = p.JoinType.Description,
                        HomePhone = p.HomePhone.FmtFone(),
                        CellPhone = p.CellPhone.FmtFone(),
                        WorkPhone = p.WorkPhone.FmtFone(),
                        MemberStatus = p.MemberStatus.Description,
                        FellowshipLeader = p.BFClass.LeaderName,
                        Spouse = spouse,
                        Age = p.Age.ToString(),
                        School = p.SchoolOther,
                        Grade = p.Grade.ToString(),
                        AttendPctBF = (om == null ? 0 : om.AttendPct == null ? 0 : om.AttendPct.Value),
                        Married = p.MaritalStatus.Description,
                        FamilyId = p.FamilyId,
                        Image = p.PictureId == null ? Util.ServerLink("/images/unknown.jpg") :
                            Util.ServerLink("/Image.aspx?portrait=1&w=160&h=200&id=" + p.Picture.LargeId)
                    };
            return q.Take(maximumRows);
        }
        public static IEnumerable ExportExtraValues(int qid)
        {
            var name = "ExtraExcelResult " + DateTime.Now;
            var tag = DbUtil.Db.PopulateSpecialTag(qid, DbUtil.TagTypeId_ExtraValues);

            var cmd = new SqlCommand("dbo.ExtraValues {0}".Fmt(tag.Id));
            cmd.Connection = new SqlConnection(Util.ConnectionString);
            cmd.Connection.Open();
            return cmd.ExecuteReader();
        }
    }
}