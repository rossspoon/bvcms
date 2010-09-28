/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CmsData;
using UtilityExtensions;
using System.ComponentModel;
using System.Collections;
using System.Threading;
using System.Web;

namespace CMSPresenter
{
    [DataObject]
    public class MailingController
    {
        public bool UseTitles { get; set; }

        public IEnumerable<MailingInfo> FetchIndividualList(string sortExpression, int QueryId)
        {
            var q = DbUtil.Db.PeopleQuery(QueryId);
            var q2 = from p in q
                     where p.DeceasedDate == null
                     where p.PrimaryBadAddrFlag != 1
                     where p.DoNotMailFlag == false
                     select new MailingInfo
                     {
                         Address = p.PrimaryAddress,
                         Address2 = p.PrimaryAddress2,
                         CityStateZip = Util.FormatCSZ4(p.PrimaryCity, p.PrimaryState, p.PrimaryZip),
                         City = p.PrimaryCity,
                         State = p.PrimaryState,
                         Zip = p.PrimaryZip,
                         LabelName = UseTitles ? (p.TitleCode != null ? p.TitleCode + " " + p.Name : p.Name) : p.Name,
                         Name = p.Name,
                         LastName = p.LastName,
                         PeopleId = p.PeopleId
                     };
            q2 = ApplySort(q2, sortExpression);
            return q2;
        }

        private int _FamilyCount;
        public int FamilyCount(int startRowIndex, int maximumRows, string sortExpression, int QueryId)
        {
            return _FamilyCount;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<MailingInfo> FetchFamilyList(int startRowIndex, int maximumRows, string sortExpression, int QueryId)
        {
            var q1 = DbUtil.Db.PeopleQuery(QueryId);
            var q = from f in DbUtil.Db.Families
                    where q1.Any(p => p.FamilyId == f.FamilyId)
                    select f.People.Single(fm => fm.PeopleId == f.HeadOfHouseholdId);

            _FamilyCount = q.Count();
            var q2 = from h in q
                     let spouse = DbUtil.Db.People.SingleOrDefault(sp => sp.PeopleId == h.SpouseId)
                     where h.DeceasedDate == null
                     where h.PrimaryBadAddrFlag != 1
                     where h.DoNotMailFlag == false
                     select new MailingInfo
                     {
                         Address = h.PrimaryAddress,
                         Address2 = h.PrimaryAddress2,
                         CityStateZip = Util.FormatCSZ4(h.PrimaryCity, h.PrimaryState, h.PrimaryZip),
                         City = h.PrimaryCity,
                         State = h.PrimaryState,
                         Zip = h.PrimaryZip,
                         LabelName = (h.Family.CoupleFlag == 1 ? (UseTitles ? (h.TitleCode != null ? h.TitleCode + " and Mrs. " + h.Name
                                                                                                    : "Mr. and Mrs. " + h.Name)
                                                                             : h.PreferredName + " and " + spouse.PreferredName + " " + h.LastName) :
                                      h.Family.CoupleFlag == 2 ? ("The " + h.Name + " Family") :
                                      h.Family.CoupleFlag == 3 ? ("The " + h.Name + " Family") :
                                      h.Family.CoupleFlag == 4 ? (h.Name + " & Family") :
                                      UseTitles ? (h.TitleCode != null ? h.TitleCode + " " + h.Name : h.Name) : h.Name),
                         Name = h.Name,
                         LastName = h.LastName,
                         PeopleId = h.PeopleId
                     };
            q2 = ApplySort(q2, sortExpression);
            if (maximumRows == 0)
                return q2;
            return q2.Skip(startRowIndex).Take(maximumRows);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<MailingInfo> FetchFamilyList(string sortExpression, int QueryId)
        {
            int startRowIndex = 0;
            int maximumRows = 0;
            return FetchFamilyList(startRowIndex, maximumRows, sortExpression, QueryId);
        }

        public IEnumerable<MailingInfo> FetchParentsOfList(string sortExpression, int QueryId)
        {
            var q = DbUtil.Db.PeopleQuery(QueryId);
            var q2 = from p in q
                     where p.DeceasedDate == null
                     where p.PrimaryBadAddrFlag != 1
                     where p.DoNotMailFlag == false
                     select new MailingInfo
                     {
                         Address = p.PrimaryAddress,
                         Address2 = p.PrimaryAddress2,
                         CityStateZip = Util.FormatCSZ4(p.PrimaryCity, p.PrimaryState, p.PrimaryZip),
                         City = p.PrimaryCity,
                         State = p.PrimaryState,
                         Zip = p.PrimaryZip,
                         LabelName = (p.PositionInFamilyId == 30 ? ("Parents Of " + p.Name) : UseTitles ? (p.TitleCode != null ? p.TitleCode + " " + p.Name : p.Name) : p.Name),
                         Name = p.Name,
                         LastName = p.LastName,
                         PeopleId = p.PeopleId
                     };
            q2 = ApplySort(q2, sortExpression);
            return q2;
        }

        private const string STR_Couples = "couples";

        private static IQueryable<Person> EliminateCoupleDoublets(IQueryable<Person> q)
        {
            var q1 = from p in q
                     // exclude wife who has a husband who is already in the list
                     where !(p.GenderId == 2 && p.SpouseId != null && q.Any(pp => pp.PeopleId == p.SpouseId))
                     where p.DeceasedDate == null
                     where (p.BadAddressFlag == null || p.BadAddressFlag == false)
                     where p.DoNotMailFlag == false
                     select p;
            return q1;
        }

        public IEnumerable<MailingInfo> FetchCouplesEitherList(string sortExpression, int QueryId)
        {
            var q = DbUtil.Db.PopulateSpecialTag(QueryId, DbUtil.TagTypeId_CouplesHelper).People();
            var q1 = EliminateCoupleDoublets(q);
            var q2 = from p in q1
                     let spouse = DbUtil.Db.People.SingleOrDefault(sp => sp.PeopleId == p.SpouseId)
                     select new MailingInfo
                     {
                         Address = p.PrimaryAddress,
                         Address2 = p.PrimaryAddress2,
                         CityStateZip = Util.FormatCSZ4(p.PrimaryCity, p.PrimaryState, p.PrimaryZip),
                         City = p.PrimaryCity,
                         State = p.PrimaryState,
                         Zip = p.PrimaryZip,
                         LabelName = (spouse == null ? (UseTitles ? (p.TitleCode != null ? p.TitleCode + " " + p.Name : p.Name) : p.Name) :
                             (p.Family.HeadOfHouseholdId == p.PeopleId ?
                                 (UseTitles ? (p.TitleCode != null ? p.TitleCode + " and Mrs. " + p.Name : "Mr. and Mrs. " + p.Name)
                                             : (p.PreferredName + " and " + spouse.PreferredName + " " + p.LastName)) :
                                 (UseTitles ? (spouse.TitleCode != null ? spouse.TitleCode + " and Mrs. " + spouse.Name : "Mr. and Mrs. " + spouse.Name)
                                             : (spouse.PreferredName + " and " + p.PreferredName + " " + spouse.LastName)))),
                         Name = p.Name,
                         LastName = p.LastName,
                         PeopleId = p.PeopleId
                     };
            q2 = ApplySort(q2, sortExpression);
            return q2;
        }

        public IEnumerable<MailingInfo> FetchCouplesBothList(string sortExpression, int QueryId)
        {
            var q = DbUtil.Db.PopulateSpecialTag(QueryId, DbUtil.TagTypeId_CouplesHelper).People();
            var q1 = EliminateCoupleDoublets(q);
            var q2 = from p in q1
                     // get spouse if in the query
                     let spouse = q.SingleOrDefault(sp => sp.PeopleId == p.SpouseId)
                     select new MailingInfo
                     {
                         Address = p.PrimaryAddress,
                         Address2 = p.PrimaryAddress2,
                         CityStateZip = Util.FormatCSZ4(p.PrimaryCity, p.PrimaryState, p.PrimaryZip),
                         City = p.PrimaryCity,
                         State = p.PrimaryState,
                         Zip = p.PrimaryZip,
                         LabelName = (spouse == null ? (UseTitles ? (p.TitleCode != null ? p.TitleCode + " " + p.Name : p.Name) : p.Name) :
                             (p.Family.HeadOfHouseholdId == p.PeopleId ?
                                 (UseTitles ? (p.TitleCode != null ? p.TitleCode + " and Mrs. " + p.Name : "Mr. and Mrs. " + p.Name)
                                             : (p.PreferredName + " and " + spouse.PreferredName + " " + p.LastName)) :
                                 (UseTitles ? (spouse.TitleCode != null ? spouse.TitleCode + " and Mrs. " + spouse.Name : "Mr. and Mrs. " + spouse.Name)
                                             : (spouse.PreferredName + " and " + p.PreferredName + " " + spouse.LastName)))),
                         Name = p.Name,
                         LastName = p.LastName,
                         PeopleId = p.PeopleId
                     };
            q2 = ApplySort(q2, sortExpression);
            return q2;
        }

        public IQueryable<MailingInfo> ApplySort(IQueryable<MailingInfo> query, string sortExpression)
        {
            switch (sortExpression)
            {
                case "Name":
                    return query.OrderBy(mi => mi.LastName);
                //break;
                default:
                    break;
            }
            return query;
        }

        public IEnumerable FetchExcelCouplesBoth(int QueryId, int maximumRows)
        {
            var q = DbUtil.Db.PopulateSpecialTag(QueryId, DbUtil.TagTypeId_CouplesHelper).People();
            var q1 = EliminateCoupleDoublets(q);
            var q2 = from p in q1
                     // get spouse if in the query
                     let spouse = q.SingleOrDefault(sp => sp.PeopleId == p.SpouseId)
                     select new
                     {
                         PeopleId = p.PeopleId,
                         LabelName = (spouse == null ? (UseTitles ? (p.TitleCode != null ? p.TitleCode + " " + p.Name : p.Name) : p.Name) :
                             (p.Family.HeadOfHouseholdId == p.PeopleId ?
                                 (UseTitles ? (p.TitleCode != null ? p.TitleCode + " and Mrs. " + p.Name : "Mr. and Mrs. " + p.Name)
                                             : (p.PreferredName + " and " + spouse.PreferredName + " " + p.LastName)) :
                                 (UseTitles ? (spouse.TitleCode != null ? spouse.TitleCode + " and Mrs. " + spouse.Name : "Mr. and Mrs. " + spouse.Name)
                                             : (spouse.PreferredName + " and " + p.PreferredName + " " + spouse.LastName)))),
                         FirstName = p.PreferredName,
                         FirstNameSpouse = spouse != null ? spouse.PreferredName : "",
                         LastName = p.LastName,
                         Address = p.PrimaryAddress,
                         Address2 = p.PrimaryAddress2,
                         City = p.PrimaryCity,
                         State = p.PrimaryState,
                         Zip = p.PrimaryZip.FmtZip(),
                         Email = p.EmailAddress,
                         EmailSpouse = spouse != null ? spouse.EmailAddress : "",
                         HomePhone = p.Family.HomePhone.FmtFone(),
                         MemberStatus = p.MemberStatus.Description,
                         Employer = p.EmployerOther,
                     };
            return q2.Take(maximumRows);
        }

        public IEnumerable FetchExcelCouplesEither(int QueryId, int maximumRows)
        {
            var q = DbUtil.Db.PopulateSpecialTag(QueryId, DbUtil.TagTypeId_CouplesHelper).People();
            var q1 = EliminateCoupleDoublets(q);
            var q2 = from p in q1
                     let spouse = DbUtil.Db.People.SingleOrDefault(sp => sp.PeopleId == p.SpouseId)
                     select new
                     {
                         PeopleId = p.PeopleId,
                         LabelName = (spouse == null ? (UseTitles ? (p.TitleCode != null ? p.TitleCode + " " + p.Name : p.Name) : p.Name) :
                             (p.Family.HeadOfHouseholdId == p.PeopleId ?
                                 (UseTitles ? (p.TitleCode != null ? p.TitleCode + " and Mrs. " + p.Name : "Mr. and Mrs. " + p.Name)
                                             : (p.PreferredName + " and " + spouse.PreferredName + " " + p.LastName)) :
                                 (UseTitles ? (spouse.TitleCode != null ? spouse.TitleCode + " and Mrs. " + spouse.Name : "Mr. and Mrs. " + spouse.Name)
                                             : (spouse.PreferredName + " and " + p.PreferredName + " " + spouse.LastName)))),
                         FirstName = p.PreferredName,
                         FirstNameSpouse = spouse != null ? spouse.PreferredName : "",
                         LastName = p.LastName,
                         Address = p.PrimaryAddress,
                         Address2 = p.PrimaryAddress2,
                         City = p.PrimaryCity,
                         State = p.PrimaryState,
                         Zip = p.PrimaryZip.FmtZip(),
                         Email = p.EmailAddress,
                         EmailSpouse = spouse != null ? spouse.EmailAddress : "",
                         MemberStatus = p.MemberStatus.Description,
                         Employer = p.EmployerOther,
                         HomePhone = p.HomePhone.FmtFone(),
                         CellPhone = p.CellPhone.FmtFone(),
                         WorkPhone = p.WorkPhone.FmtFone(),
                     };
            return q2.Take(maximumRows);
        }

        public IEnumerable FetchExcelFamily(int QueryId, int maximumRows)
        {
            var qp = DbUtil.Db.PeopleQuery(QueryId);

            var q = from f in DbUtil.Db.Families
                    where qp.Any(p => p.FamilyId == f.FamilyId)
                    select f.People.Single(fm => fm.PeopleId == f.HeadOfHouseholdId);

            return (from h in q
                    let spouse = DbUtil.Db.People.SingleOrDefault(sp => sp.PeopleId == h.SpouseId)
                    where h.DeceasedDate == null
                    where h.PrimaryBadAddrFlag != 1
                    where h.DoNotMailFlag == false
                    select new
                    {
                        LabelName = (h.Family.CoupleFlag == 1 ? (UseTitles ? (h.TitleCode != null ? h.TitleCode + " and Mrs. " + h.Name
                                                                                                   : "Mr. and Mrs. " + h.Name)
                                                                            : h.PreferredName + " and " + spouse.PreferredName + " " + h.LastName) :
                                     h.Family.CoupleFlag == 2 ? ("The " + h.Name + " Family") :
                                     h.Family.CoupleFlag == 3 ? ("The " + h.Name + " Family") :
                                     h.Family.CoupleFlag == 4 ? (h.Name + " & Family") :
                                     UseTitles ? (h.TitleCode != null ? h.TitleCode + " " + h.Name : h.Name) : h.Name),
                        Name = h.Name,
                        LastName = h.LastName,
                        Address = h.PrimaryAddress,
                        Address2 = h.PrimaryAddress2,
                        CityStateZip = Util.FormatCSZ4(h.PrimaryCity, h.PrimaryState, h.PrimaryZip),
                        City = h.PrimaryCity,
                        State = h.PrimaryState,
                        Zip = h.PrimaryZip,
                    }).Take(maximumRows);
        }

        public IEnumerable FetchExcelParents(int QueryId, int maximumRows)
        {
            var q = DbUtil.Db.PeopleQuery(QueryId);
            var q2 = from p in q
                     where p.DeceasedDate == null
                     where p.PrimaryBadAddrFlag != 1
                     where p.DoNotMailFlag == false
                     let hohemail = p.Family.HeadOfHousehold.EmailAddress
                     select new
                     {
                         LabelName = (p.PositionInFamilyId == 30 ? ("Parents Of " + p.Name) : p.TitleCode != null ? p.TitleCode + " " + p.Name : p.Name),
                         Name = p.Name,
                         LastName = p.LastName,
                         Address = p.PrimaryAddress,
                         Address2 = p.PrimaryAddress2,
                         CityStateZip = Util.FormatCSZ4(p.PrimaryCity, p.PrimaryState, p.PrimaryZip),
                         City = p.PrimaryCity,
                         State = p.PrimaryState,
                         Zip = p.PrimaryZip,
                         ParentEmail = (p.PositionInFamilyId == 30 ?
                            (hohemail != null && hohemail != "" ?
                                hohemail
                                : p.Family.HeadOfHouseholdSpouse.EmailAddress)
                            : p.EmailAddress)
                     };
            return q2.Take(maximumRows);
        }
    }
}
