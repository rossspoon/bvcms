/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using UtilityExtensions;
using CmsData;
using System.Collections;
using System.Text.RegularExpressions;

namespace CMSPresenter
{
    [DataObject]
    public class PersonSearchController
    {
        public PersonSearchController()
        {
            TagTypeId = DbUtil.TagTypeId_Personal;
            TagName = Util.CurrentTagName;
            TagOwner = Util.CurrentTagOwnerId;
        }
        public int count;

        public int TagTypeId { get; set; }
        public string TagName { get; set; }
        public int? TagOwner { get; set; }

        public IEnumerable<TaggedPersonInfo> FetchPeopleList(IQueryable<Person> query)
        {
            var q = from p in query
                    select new TaggedPersonInfo
                    {
                        PeopleId = p.PeopleId,
                        Name = p.Name,
                        //JoinDate = p.JoinDate,
                        BirthDate = Util.FormatBirthday(p.BirthYear, p.BirthMonth, p.BirthDay),
                        Address = p.PrimaryAddress,
                        Address2 = p.PrimaryAddress2,
                        CityStateZip = Util.FormatCSZ(p.PrimaryCity, p.PrimaryState, p.PrimaryZip),
                        HomePhone = p.HomePhone,
                        CellPhone = p.CellPhone,
                        WorkPhone = p.WorkPhone,
                        PhonePref = p.PhonePrefId,
                        MemberStatus = p.MemberStatus.Description,
                        Email = p.EmailAddress,
                        BFTeacher = p.BFClass.LeaderName,
                        BFTeacherId = p.BFClass.LeaderId,
                        Age = p.Age.ToString(),
                        HasTag = p.Tags.Any(t => t.Tag.Name == TagName && t.Tag.PeopleId == TagOwner && t.Tag.TypeId == TagTypeId),
                    };
            return q;
        }
        public static IEnumerable FetchExcelList(int queryid, int maximumRows)
        {
            var Db = DbUtil.Db;
            var qB = Db.LoadQueryById(queryid);
            var query = Db.People.Where(qB.Predicate());
            var q = from p in query
                    let om = p.OrganizationMembers.SingleOrDefault(om => om.OrganizationId == p.BibleFellowshipClassId)
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
                        BFTeacher = p.BFClass.LeaderName,
                        Age = p.Age.ToString(),
                        School = p.SchoolOther,
                        Grade = p.Grade.ToString(),
                        AttendPctBF = (om == null ? 0 : om.AttendPct == null ? 0 : om.AttendPct.Value),
                        Married = p.MaritalStatus.Description,
                        FamilyId = p.FamilyId
                    };
            return q.Take(maximumRows);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<TaggedPersonInfo> FetchPeopleList(int startRowIndex, int maximumRows, string sortExpression,
            string namesearch, string commsearch, string addrsearch, int memstatus, int tag, string dob, int gender, int orgid, int campus, bool usersonly, int marital)
        {
            DbUtil.Db.SetNoLock();
            var query = ApplySearch(namesearch, addrsearch, commsearch, memstatus, tag, dob, gender, orgid, campus, usersonly, marital);
            count = query.Count();
            if (TagTypeId == DbUtil.TagTypeId_AddSelected)
            {
                query = ApplySort(query, sortExpression);
                var t = DbUtil.Db.FetchOrCreateTag(TagName, TagOwner, TagTypeId);
                query = t.People().Union(query);
            }
            else
                query = ApplySort(query, sortExpression);
            query = query.Skip(startRowIndex).Take(maximumRows);
            return FetchPeopleList(query);
        }

        public int Count(int startRowIndex, int maximumRows, string sortExpression,
            string namesearch, string commsearch, string addrsearch, int memstatus, int tag, string dob, int gender, int orgid, int campus, bool usersonly, int marital)
        {
            return count;
        }

        private static void NameSplit(string name, out string First, out string Last)
        {
            var a = name.Split(' ');
            First = "";
            if (a.Length > 1)
            {
                First = a[0];
                Last = a[1];
            }
            else
                Last = a[0];

        }
        public static Regex AddrRegex = new Regex(
        @"\A(?<addr>.*);\s*(?<city>.*),\s+(?<state>[A-Z]*)\s+(?<zip>\d{5}(-\d{4})?)\z");

        public static IQueryable<Person> ApplySearch(
            string name, string addr, string comm, int memstatus, int tag, string dob, int gender, int orgid, int campus, bool usersonly, int marital)
        {
            var query = DbUtil.Db.People.Select(p => p);
            if (Util.OrgMembersOnly)
                query = DbUtil.Db.OrgMembersOnlyTag.People();
            if (usersonly)
                query = query.Where(p => p.Users.Count() > 0);

            if (memstatus > 0)
                query = from p in query
                        where p.MemberStatusId == memstatus
                        select p;
            if (name.HasValue())
            {
                string First, Last;
                NameSplit(name, out First, out Last);
                if (First.HasValue())
                    query = from p in query
                            where (p.LastName.StartsWith(Last) || p.MaidenName.StartsWith(Last))
                            && (p.FirstName.StartsWith(First) || p.NickName.StartsWith(First) || p.MiddleName.StartsWith(First))
                            select p;
                else
                    if (Last.AllDigits())
                        query = from p in query
                                where p.PeopleId == Last.ToInt()
                                select p;
                    else
                        query = from p in query
                                where p.LastName.StartsWith(Last) || p.MaidenName.StartsWith(Last)
                                select p;
            }
            if (addr.IsNotNull())
            {
                if (PersonSearchController.AddrRegex.IsMatch(addr))
                {
                    var m = PersonSearchController.AddrRegex.Match(addr);
                    addr = m.Groups["addr"].Value;
                }
                addr = addr.Trim();
                if (addr.HasValue())
                    query = from p in query
                            where p.Family.AddressLineOne.Contains(addr)
                               || p.Family.AddressLineTwo.Contains(addr)
                               || p.Family.CityName.Contains(addr)
                               || p.Family.ZipCode.Contains(addr)
                            select p;
            }
            if (comm.IsNotNull())
            {
                comm = comm.Trim();
                if (comm.HasValue())
                    query = from p in query
                            where p.CellPhone.Contains(comm) || p.EmailAddress.Contains(comm)
                            || p.Family.HomePhone.Contains(comm)
                            || p.WorkPhone.Contains(comm)
                            select p;
            }
            if (tag > 0)
                query = from p in query
                        where p.Tags.Any(t => t.Id == tag)
                        select p;
            if (dob.HasValue() && dob != "na")
            {
                DateTime dt;
                if (DateTime.TryParse(dob, out dt))
                    if (Regex.IsMatch(dob, @"\d+/\d+/\d+"))
                        query = query.Where(p => p.BirthDay == dt.Day && p.BirthMonth == dt.Month && p.BirthYear == dt.Year);
                    else
                        query = query.Where(p => p.BirthDay == dt.Day && p.BirthMonth == dt.Month);
                else
                {
                    int n;
                    if (int.TryParse(dob, out n))
                        if (n >= 1 && n <= 12)
                            query = query.Where(p => p.BirthMonth == n);
                        else
                            query = query.Where(p => p.BirthYear == n);
                }
            }
            if (orgid > 0)
                query = query.Where(p => p.OrganizationMembers.Any(om => om.OrganizationId == orgid));
            if (campus > 0)
                query = query.Where(p => p.CampusId == campus);
            if (gender != 99)
                query = query.Where(p => p.GenderId == gender);
            if (marital != 99)
                query = query.Where(p => p.MaritalStatusId == marital);

            return query;
        }

        public static IQueryable<Person> ApplySort(IQueryable<Person> query, string sort)
        {
            switch (sort)
            {
                case "Member":
                    query = from p in query
                            orderby p.MemberStatus.Code,
                            p.LastName,
                            p.FirstName,
                            p.PeopleId
                            select p;
                    break;
                case "Address":
                    query = from p in query
                            orderby p.PrimaryState,
                            p.PrimaryCity,
                            p.PrimaryAddress,
                            p.PeopleId
                            select p;
                    break;
                case "BFTeacher":
                    query = from p in query
                            orderby p.BFClass.LeaderName,
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
                case "Member DESC":
                    query = from p in query
                            orderby p.MemberStatus.Code descending,
                            p.LastName descending,
                            p.FirstName descending,
                            p.PeopleId descending
                            select p;
                    break;
                case "Address DESC":
                    query = from p in query
                            orderby p.PrimaryState descending,
                            p.PrimaryCity descending,
                            p.PrimaryAddress descending,
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
                case "BFTeacher DESC":
                    query = from p in query
                            orderby p.BFClass.LeaderName descending,
                            p.LastName descending,
                            p.FirstName descending,
                            p.PeopleId descending
                            select p;
                    break;
                case "DOB DESC":
                    query = from p in query
                            orderby p.BirthMonth descending, p.BirthDay descending,
                            p.LastName descending, p.FirstName descending
                            select p;
                    break;
                case "Name":
                default:
                    query = from p in query
                            orderby  p.LastName,
                            p.FirstName,
                            p.PeopleId
                            select p;
                    break;
            }
            return query;
        }
    }
}
