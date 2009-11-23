/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UtilityExtensions;
using CmsData;
using System.Web.Mvc;
using System.Web.Routing;
using System.Text.RegularExpressions;
using System.Threading;

namespace CMSWebCommon.Models
{
    public class SearchPeopleModel
    {
        public int? OrgId { get; set; }
        public string Name { get; set; }
        public string Communication { get; set; }
        public string Address { get; set; }
        public string DateOfBirth { get; set; }
        public int? TagId { get; set; }
        public int? MemberStatusId { get; set; }
        public int? GenderId { get; set; }
        public bool AddToExisting { get; set; }
        public int? ExistingFamilyMember { get; set; }
        public int? EntryPoint { get; set; }
        public int? Origin { get; set; }

        public string Sort { get; set; }
        private int? _Page;
        public int? Page
        {
            get { return _Page ?? 0; }
            set { _Page = value; }
        }
        public int StartRow
        {
            get { return Page.Value * PageSize.Value; }
        }
        public int? PageSize
        {
            get
            {
                var ps = Util.Cookie("PageSize", "10").ToInt();
                return ps;
            }
            set
            {
                if (value.HasValue)
                    Util.Cookie("PageSize", value.Value.ToString(), 360);
            }
        }

        private Regex AddrRegex = new Regex(
        @"\A(?<addr>.*);\s*(?<city>.*),\s+(?<state>[A-Z]*)\s+(?<zip>\d{5}(-\d{4})?)\z");

        private IEnumerable<SearchPeopleInfo> PeopleList(IQueryable<Person> query)
        {
            var q = from p in query
                    select new SearchPeopleInfo
                    {
                        PeopleId = p.PeopleId,
                        Name = p.Name,
                        Address = p.PrimaryAddress,
                        CityStateZip = p.PrimaryCity + ", " + p.PrimaryState + " " + p.PrimaryZip.Substring(0, 5),
                        Age = p.Age,
                        JoinDate = p.JoinDate,
                        BirthDate = p.BirthMonth + "/" + p.BirthDay + "/" + p.BirthYear,
                        HomePhone = p.HomePhone,
                        CellPhone = p.CellPhone,
                        WorkPhone = p.WorkPhone,
                        MemberStatus = p.MemberStatus.Description,
                    };
            return q;
        }

        public IEnumerable<SearchPeopleInfo> PeopleList()
        {
            var query = ApplySearch();
            query = ApplySort(query, Sort).Skip(StartRow).Take(PageSize.Value);
            return PeopleList(query);
        }

        private int? count;
        public int Count
        {
            get
            {
                if (!count.HasValue)
                    count = ApplySearch().Count();
                return count.Value;
            }
        }

        private static void NameSplit(string name, out string First, out string Last)
        {
            First = null;
            Last = null;
            if (name == null)
                return;
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
        private IQueryable<Person> ApplySearch()
        {
            var Db = DbUtil.Db;
            var query = Db.People.Select(p => p);
            if (Util.OrgMembersOnly)
                query = Db.OrgMembersOnlyTag.People();

            if (MemberStatusId.HasValue && MemberStatusId > 0)
                query = from p in query
                        where p.MemberStatusId == MemberStatusId
                        select p;
            if (Name.HasValue())
            {
                string First, Last;
                NameSplit(Name, out First, out Last);
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
            if (Address.IsNotNull())
            {
                Address = Address.Trim();
                if (Address.HasValue())
                    query = from p in query
                            where p.Family.AddressLineOne.Contains(Address)
                               || p.Family.AddressLineTwo.Contains(Address)
                               || p.Family.CityName.Contains(Address)
                               || p.Family.ZipCode.Contains(Address)
                            select p;
            }
            if (Communication.IsNotNull())
            {
                Communication = Communication.Trim();
                if (Communication.HasValue())
                    query = from p in query
                            where p.CellPhone.Contains(Communication) || p.EmailAddress.Contains(Communication)
                            || p.Family.HomePhone.Contains(Communication)
                            || p.WorkPhone.Contains(Communication)
                            select p;
            }
            if (TagId.HasValue && TagId > 0)
                query = from p in query
                        where p.Tags.Any(t => t.Id == TagId)
                        select p;
            if (DateOfBirth.HasValue())
            {
                DateTime dt;
                if (DateTime.TryParse(DateOfBirth, out dt))
                    if (Regex.IsMatch(DateOfBirth, @"\d+/\d+/\d+"))
                        query = query.Where(p => p.BirthDay == dt.Day && p.BirthMonth == dt.Month && p.BirthYear == dt.Year);
                    else
                        query = query.Where(p => p.BirthDay == dt.Day && p.BirthMonth == dt.Month);
                else
                {
                    int n;
                    if (int.TryParse(DateOfBirth, out n))
                        if (n >= 1 && n <= 12)
                            query = query.Where(p => p.BirthMonth == n);
                        else
                            query = query.Where(p => p.BirthYear == n);
                }
            }
            if (OrgId.HasValue && OrgId > 0)
                query = query.Where(p => p.OrganizationMembers.Any(om => om.OrganizationId == OrgId));
            if (GenderId.HasValue && GenderId != 99)
                query = query.Where(p => p.GenderId == GenderId);

            return query;
        }

        public IQueryable<Person> ApplySort(IQueryable<Person> query, string sort)
        {
            if (!sort.HasValue())
                sort = "Date";
            switch (sort)
            {
                case "Id":
                    query = from c in query
                            orderby c.PeopleId
                            select c;
                    break;
                case "Name":
                    query = from c in query
                            orderby c.Name2
                            select c;
                    break;
                case "CityStateZip":
                    query = from c in query
                            orderby c.PrimaryCity
                            select c;
                    break;
                case "Age":
                    query = from c in query
                            orderby c.Age
                            select c;
                    break;
            }
            return query;
        }

        public IEnumerable<SelectListItem> UserTags()
        {
            var Db = DbUtil.Db;
            Db.TagCurrent(); // make sure the current tag exists
            int userPeopleId = Util.UserPeopleId.Value;
            var q1 = from t in Db.Tags
                     where t.PeopleId == userPeopleId
                     where t.TypeId == DbUtil.TagTypeId_Personal
                     orderby t.Name
                     select new SelectListItem
                     {
                         Value = t.Id.ToString(),
                         Text = t.Name
                     };
            var q2 = from t in Db.Tags
                     where t.PeopleId != userPeopleId
                     where t.TagShares.Any(ts => ts.PeopleId == userPeopleId)
                     where t.TypeId == DbUtil.TagTypeId_Personal
                     orderby t.PersonOwner.Name2, t.Name
                     let op = Db.People.SingleOrDefault(p => p.PeopleId == t.PeopleId)
                     select new SelectListItem
                     {
                         Value = t.Id.ToString(),
                         Text = op.Name + ":" + t.Name
                     };
            var q = q1.Union(q2);
            var list = q.ToList();
            list.Insert(0, new SelectListItem { Value = "0", Text = "(not specified)" });
            return list;
        }
        public IEnumerable<SelectListItem> MemberStatusCodes()
        {
            var q = from ms in DbUtil.Db.MemberStatuses
                    select new SelectListItem
                    {
                        Value = ms.Id.ToString(),
                        Text = ms.Description
                    };
            var list = q.ToList();
            list.Insert(0, new SelectListItem { Value = "0", Text = "(not specified)" });
            return list;
        }

        public IEnumerable<SelectListItem> GenderCodes()
        {
            var q = from ms in DbUtil.Db.Genders
                    select new SelectListItem
                    {
                        Value = ms.Id.ToString(),
                        Text = ms.Description
                    };
            var list = q.ToList();
            list.Insert(0, new SelectListItem { Value = "99", Text = "(not specified)" });
            return list;
        }
        public int? AddNewPerson()
        {
            var Db = DbUtil.Db;
            var p = new Person { AddressTypeId = 10 };
            p.EntryPointId = EntryPoint;
            p.OriginId = Origin;
            Db.People.InsertOnSubmit(p);
            DateTime bdt;
            if (DateTime.TryParse(DateOfBirth, out bdt))
            {
                p.BirthDay = bdt.Day;
                p.BirthMonth = bdt.Month;
                if (Regex.IsMatch(DateOfBirth, @"\d+/\d+/\d+"))
                {
                    p.BirthYear = bdt.Year;
                    if (bdt.AddYears(18) > Util.Now)
                        p.PositionInFamilyId = 30;
                }
            }
            if (AddToExisting) // existing family
            {
                var q = from per in Db.People
                        where per.PeopleId == ExistingFamilyMember
                        select per.Family;
                p.Family = q.SingleOrDefault();
                p.CellPhone = Communication.GetDigits();
                if (p.Family == null)
                    return null;
                if (p.PositionInFamilyId == 0)
                {
                    var cnt = p.Family.People.Where(c =>
                        c.PositionInFamilyId == (int)Family.PositionInFamily.PrimaryAdult).Count();
                    if (cnt < 2) // room for primary adult?
                        p.PositionInFamilyId = (int)Family.PositionInFamily.PrimaryAdult;
                    else
                        p.PositionInFamilyId = (int)Family.PositionInFamily.SecondaryAdult;
                }
            }
            else // new single family
            {
                p.Family = new Family();
                p.Family.HomePhone = Communication.GetDigits();
                if (Address.HasValue())
                {
                    var m = AddrRegex.Match(Address);
                    p.Family.AddressLineOne = m.Groups["addr"].Value;
                    p.Family.CityName = m.Groups["city"].Value;
                    p.Family.StateCode = m.Groups["state"].Value;
                    p.Family.ZipCode = m.Groups["zip"].Value;
                }
                if (p.PositionInFamilyId == 0)
                    p.PositionInFamilyId = (int)Family.PositionInFamily.PrimaryAdult;
            }
            string First, Last;
            NameSplit(Name, out First, out Last);
            p.FirstName = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(First);
            p.LastName = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(Last);
            p.GenderId = GenderId ?? 0;
            if (p.GenderId == 99)
                p.GenderId = 0;

            p.MemberStatusId = (int)Person.MemberStatusCode.JustAdded;
            var tag = Db.FetchOrCreateTag("JustAdded", Util.UserPeopleId, DbUtil.TagTypeId_Personal);
            tag.PersonTags.Add(new TagPerson { Person = p });
            p.FixTitle();
            Db.SubmitChanges();
            return p.PeopleId;
        }

        public string ValidateAddNew()
        {
            DateTime dt;
            if (Util.DateValid(DateOfBirth, out dt))
                return "need valid birthday";
            string first, last;
            NameSplit(Name, out first, out last);
            if (!first.HasValue() || !last.HasValue())
                return "need first and last name";
            if (Communication.HasValue() && Communication.GetDigits() == Communication.FmtFone())
                return "need valid phone number (7 or 10 digits)";
            if (AddToExisting) // existing family
                if (Address.HasValue())
                    return "Address should be blank when adding to existing family";
            if (!AddToExisting && Address.HasValue() && !AddrRegex.IsMatch(Address))
                return "Address needs to be formatted as: number street; city, state zip";
            return null;
        }
        public static Person FindPerson(string phone, string first, string last, DateTime DOB, out int count)
        {
            first = first.Trim();
            last = last.Trim();
            var fone = Util.GetDigits(phone);
            var q = from p in DbUtil.Db.People
                    where (p.FirstName == first || p.NickName == first || p.MiddleName == first)
                    where (p.LastName == last || p.MaidenName == last)
                    where p.BirthDay == DOB.Day && p.BirthMonth == DOB.Month && p.BirthYear == DOB.Year
                    select p;
            count = q.Count();
            if (count > 1)
            {
                q = from p in q
                    where p.CellPhone.Contains(fone) || p.Family.HomePhone.Contains(fone)
                    select p;
                count = q.Count();
            }
            Person person = null;
            if (count == 1)
                person = q.Single();
            return person;
        }
        public static void ValidateFindPerson(ModelStateDictionary modelState, 
            string first, 
            string last, 
            DateTime birthday, 
            string phone)
        {
            if (!first.HasValue())
                modelState.AddModelError("first", "first name required");
            if (!last.HasValue())
                modelState.AddModelError("last", "last name required");
            if (birthday.Equals(DateTime.MinValue))
                modelState.AddModelError("dob", "valid birth date required");
            var d = phone.GetDigits().Length;
            if (phone.HasValue() && d != 7 && d != 10)
                modelState.AddModelError("phone", "7 or 10 digits");
        }

    }
    public class SearchPeopleInfo
    {
        public int PeopleId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string CityStateZip { get; set; }
        public int? Age { get; set; }
        public string HomePhone { get; set; }
        public string CellPhone { get; set; }
        public string WorkPhone { get; set; }
        public string MemberStatus { get; set; }
        public DateTime? JoinDate { get; set; }
        public string BirthDate { get; set; }
        public string ToolTip
        {
            get
            {
                return "ID: {0} \nMobile Phone: {1} \nWork Phone: {2} \nHome Phone: {3} \nBirthDate: {4:d} \nJoin Date: {5:d} \nStatus: {6}"
                    .Fmt(PeopleId, CellPhone, WorkPhone, HomePhone, BirthDate, JoinDate, MemberStatus);
            }
        }
    }
}
