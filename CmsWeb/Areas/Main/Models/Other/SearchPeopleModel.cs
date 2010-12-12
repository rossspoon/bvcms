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
using System.Data.Linq;

namespace CmsWeb.Models
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
        public int GenderId { get; set; }
        public bool AddToExisting { get; set; }
        public int? ExistingFamilyMember { get; set; }
        public int? EntryPoint { get; set; }
        public int? Origin { get; set; }
        public int MaritalStatusId { get; set; }
        public int CampusId { get; set; }
        public SearchPeopleModel()
        {
            MaritalStatusId = 99;
            GenderId = 99;
        }
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

        private IQueryable<Person> ApplySearch()
        {
            var Db = DbUtil.Db;
            var query = Db.People.Select(p => p);
            if (Util2.OrgMembersOnly)
                query = Db.OrgMembersOnlyTag2().People(Db);

            if (MemberStatusId.HasValue && MemberStatusId > 0)
                query = from p in query
                        where p.MemberStatusId == MemberStatusId
                        select p;
            if (Name.HasValue())
            {
                string First, Last;
                Person.NameSplit(Name, out First, out Last);
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
            if (GenderId != 99)
                query = query.Where(p => p.GenderId == GenderId);
            if (CampusId != 0)
                query = query.Where(p => p.CampusId == CampusId);
            if (MaritalStatusId != 99)
                query = query.Where(p => p.MaritalStatusId == MaritalStatusId);

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
        public IEnumerable<SelectListItem> CampusCodes()
        {
            var q = from c in DbUtil.Db.Campus
                    select new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Description
                    };
            var list = q.ToList();
            list.Insert(0, new SelectListItem
            {
                Value = "0",
                Text = "(not specified)"
            });
            return list;
        }
        public IEnumerable<SelectListItem> MaritalStatusCodes()
        {
            var q = from c in DbUtil.Db.MaritalStatuses
                    select new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Description
                    };
            var list = q.ToList();
            list.Insert(0, new SelectListItem
            {
                Value = "99",
                Text = "(not specified)",
                Selected = true
            });
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
            list.Insert(0, new SelectListItem
            {
                Value = "99",
                Text = "(not specified)",
                Selected = true
            });
            return list;
        }
        public int? AddNewPerson()
        {
            string name;
            if (Name.HasValue())
                name = Name.Trim();
            else
                name = "New Person";

            Family fam;
            var PrimaryCount = 0;
            if (AddToExisting)
            {
                var q = from per in DbUtil.Db.People
                        where per.PeopleId == ExistingFamilyMember
                        select per.Family;
                fam = q.SingleOrDefault();
                if (fam == null)
                    return null;
                PrimaryCount = fam.People.Where(c => c.PositionInFamilyId == (int)Family.PositionInFamily.PrimaryAdult).Count();
                if (name.StartsWith("New"))
                    name = fam.HeadOfHousehold.LastName;
            }
            else // new single family
            {
                fam = new Family();
                DbUtil.Db.Families.InsertOnSubmit(fam);
                fam.HomePhone = Communication.GetDigits();
                if (Address.HasValue())
                {
                    var m = AddrRegex.Match(Address);
                    fam.AddressLineOne = m.Groups["addr"].Value;
                    fam.CityName = m.Groups["city"].Value;
                    fam.StateCode = m.Groups["state"].Value;
                    fam.ZipCode = m.Groups["zip"].Value;
                }
                DbUtil.Db.SubmitChanges();
            }
            Person p;
            p = Person.Add(fam, (int)Family.PositionInFamily.PrimaryAdult,
                null, name, DateOfBirth, false, GenderId, Origin ?? 0, EntryPoint);
            var age = p.GetAge();
            p.MaritalStatusId = MaritalStatusId;
            p.FixTitle();
            if (PrimaryCount == 2)
                p.PositionInFamilyId = (int)Family.PositionInFamily.SecondaryAdult;
            if (age < 18 && p.MaritalStatusId == (int)Person.MaritalStatusCode.Single)
                p.PositionInFamilyId = (int)Family.PositionInFamily.Child;
            if (AddToExisting)
                p.CellPhone = Communication.GetDigits();
            p.CampusId = CampusId;
            DbUtil.Db.SubmitChanges();
            DbUtil.Db.Refresh(RefreshMode.OverwriteCurrentValues, p);
            return p.PeopleId;
        }

        public string ValidateAddNew()
        {
            if (AddToExisting && !ExistingFamilyMember.HasValue)
                return "Must Select a family first";

            string first, last;
            Person.NameSplit(Name, out first, out last);
            if (!first.HasValue() || !last.HasValue())
                return "need first and last name when adding";

            DateTime dt;
            if (DateOfBirth != "na" && !Util.DateValid(DateOfBirth, out dt))
                return "Must have a valid birthday when adding or \"na\"";

            if (GenderId == 99)
                return "Must have a gender when adding";

            if (CampusId == 0)
                return "Must have a CampusId when adding";

            if (AddToExisting) // existing family
                if (Address.HasValue())
                    return "Address should be blank when adding to existing family";

            if (!AddToExisting && Address.HasValue() && !AddrRegex.IsMatch(Address))
                return "Address needs to be formatted as: number street; city, state zip";

            if (Communication.HasValue() && Communication.GetDigits() == Communication.FmtFone())
                return "need valid phone number (7 or 10 digits)";

            if (MaritalStatusId == 99)
                return "need to choose a marital status";

            return null;
        }
        public static Person FindPerson(string first, string last, DateTime? DOB, string email, string phone, out int count)
        {
            count = 0;
            if (!first.HasValue() || !last.HasValue())
                return null;
            first = first.Trim();
            last = last.Trim();
            var fone = Util.GetDigits(phone);
            var ctx = new CMSDataContext(Util.ConnectionString);
            ctx.SetNoLock();
            var q = from p in ctx.People
                    where (p.FirstName == first || p.NickName == first || p.MiddleName == first)
                    where (p.LastName == last || p.MaidenName == last)
                    select p;
            var list = q.ToList();
            count = list.Count;
            if (count == 0) // not going to find anything
            {
                ctx.Dispose();
                return null;
            }

            if (DOB.HasValue && DOB > DateTime.MinValue)
            {
                var dt = DOB.Value;
                if (dt > Util.Now)
                    dt = dt.AddYears(-100);
                var q2 = from p in q
                         where p.BirthDay == dt.Day && p.BirthMonth == dt.Month && p.BirthYear == dt.Year
                         select p;
                count = q2.Count();
                if (count == 1) // use only birthday if there and unique
                    return PersonFound(ctx, q2);
            }
            if (email.HasValue())
            {
                var q2 = from p in q
                         where p.EmailAddress == email
                         select p;
                count = q2.Count();
                if (count == 1)
                    return PersonFound(ctx, q2);
            }
            if (phone.HasValue())
            {
                var q2 = from p in q
                         where p.CellPhone.Contains(fone) || p.Family.HomePhone.Contains(fone)
                         select p;
                count = q2.Count();
                if (count == 1)
                    return PersonFound(ctx, q2);
            }
            return null;
        }
        private static Person PersonFound(CMSDataContext ctx, IQueryable<Person> q)
        {
            var pid = q.Select(p => p.PeopleId).SingleOrDefault();
            ctx.Dispose();
            return DbUtil.Db.LoadPersonById(pid);
        }
        public static void ValidateFindPerson(ModelStateDictionary modelState,
            string first,
            string last,
            DateTime? birthday,
            string email, 
            string phone)
        {
            if (!first.HasValue())
                modelState.AddModelError("first", "first name required");
            if (!last.HasValue())
                modelState.AddModelError("last", "last name required");

            int n = 0;
            if (birthday.HasValue && birthday > DateTime.MinValue)
                n++;
            if (Util.ValidEmail(email))
                n++;
            var d = phone.GetDigits().Length;
            if (phone.HasValue() && d == 10)
                n++;
            if(n == 0)
                modelState.AddModelError("dob", "valid birth date, email or phone required");

            if (!Util.ValidEmail(email))
                modelState.AddModelError("email", "valid email required");
            if (phone.HasValue() && d != 10)
                modelState.AddModelError("phone", "10 digits required");
        }
        public static string NotFoundText
        {
            get
            {
                return @"We are trying to find this person's record.<br />
The first and last names of the individual must match a record.<br />
Then one of <i>birthday, email</i> or <i>phone</i> must match.<br />
We may not have your birthday, so try leaving it blank.<br />
Try different spellings or a nickname too.<br />
<span style='color: green;'><i>After a couple of tries, you will have the option to Register a New record</i></span>";
            }
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
