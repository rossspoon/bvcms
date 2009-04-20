using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using System.Web.Mvc;
using System.Web.Routing;
using System.Text.RegularExpressions;

namespace Prayer.Models
{
    public class PeopleInfo
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
                return "ID: {0}\nMobile Phone: {1}\nWork Phone: {2}\nHome Phone: {3}\nBirthDate: {4:d}\nJoin Date: {5:d}\nStatus: {6}"
                    .Fmt(PeopleId, CellPhone, WorkPhone, HomePhone, BirthDate, JoinDate, MemberStatus);
            }
        }
    }

    public interface ISearchPeopleFormBindable
    {
        int? OrgId { get; set; }
        string Name { get; set; }
        string Communication { get; set; }
        string Address { get; set; }
        string DateOfBirth { get; set; }
        int? TagId { get; set; }
        int? MemberStatusId { get; set; }
        int? GenderId { get; set; }
        string Sort { get; set; }
        int? PageSize { get; set; }
        int? Page { get; set; }
    }
    public class SearchPeopleModel : ISearchPeopleFormBindable
    {
        public int? OrgId { get; set; }
        public string Name { get; set; }
        public string Communication { get; set; }
        public string Address { get; set; }
        public string DateOfBirth { get; set; }
        public int? TagId { get; set; }
        public int? MemberStatusId { get; set; }
        public int? GenderId { get; set; }

        public string Sort { get; set; }
        private int? _Page;
        public int? Page
        {
            get { return _Page ?? 1; }
            set { _Page = value; }
        }
        public int StartRow
        {
            get { return (Page.Value - 1) * PageSize.Value; }
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

        private CMSDataContext Db;

        public SearchPeopleModel()
        {
            Db = CmsData.DbUtil.Db;
        }

        public IEnumerable<PeopleInfo> PeopleList(IQueryable<Person> query)
        {
            var q = from p in query
                    select new PeopleInfo
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

        public IEnumerable<PeopleInfo> PeopleList()
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

        private void NameSplit(string name, out string First, out string Last)
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
        private IQueryable<Person> ApplySearch()
        {
            var query = DbUtil.Db.People.Select(p => p);

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

        public string GetContacteeList(int ContactId)
        {
            var q = from c in Db.Contactees
                    where c.ContactId == ContactId
                    select c.person.Name;
            q = q.Take(3);
            return string.Join(", ", q.ToArray());
        }
        public IEnumerable<SelectListItem> UserTags()
        {
            Db.TagCurrent(); // make sure the current tag exists
            int userPeopleId = Util.CurrentUser.PeopleId.Value;
            var q1 = from t in Db.Tags
                     where t.PeopleId == userPeopleId
                     where t.TypeId == 1
                     orderby t.Name
                     select new SelectListItem
                     {
                         Value = t.Id.ToString(),
                         Text = t.Name
                     };
            var q2 = from t in Db.Tags
                     where t.PeopleId != userPeopleId
                     where t.TagShares.Any(ts => ts.PeopleId == userPeopleId)
                     where t.TypeId == 1
                     orderby t.PersonOwner.Name2, t.Name
                     let op = Db.People.SingleOrDefault(p => p.PeopleId == t.PeopleId)
                     select new SelectListItem
                     {
                         Value = t.Id.ToString(),
                         Text = op.Name + ":" + t.Name
                     };
            var q = q1.Union(q2);
            return q.WithNotSpecified();
        }
        public IEnumerable<SelectListItem> MemberStatusCodes()
        {
            var q = from ms in Db.MemberStatuses
                    select new SelectListItem
                    {
                        Value = ms.Id.ToString(),
                        Text = ms.Description
                    };
            return q.WithNotSpecified();
        }
        public IEnumerable<SelectListItem> GenderCodes()
        {
            var q = from ms in Db.Genders
                    select new SelectListItem
                    {
                        Value = ms.Id.ToString(),
                        Text = ms.Description
                    };
            return q.WithNotSpecified("99");
        }
        public PagerModel pagerModel()
        {
            return new PagerModel
            {
                Page = Page.Value,
                PageSize = PageSize.Value,
                Action = "List",
                Controller = "Task",
                Count = Count,
            };
        }
    }
}
