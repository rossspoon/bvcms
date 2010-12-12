using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using UtilityExtensions;
using CmsData;
using System.Web.Mvc;
using System.Data.Linq.SqlClient;


namespace CmsWeb.Models.iPhone
{
    public class SearchModel
    {
        public string Name { get; set; }
        public string Comm { get; set; }
        public string Addr { get; set; }

        public SearchModel(string name, string comm, string addr)
        {
            Name = name;
            Comm = comm;
            Addr = addr;
        }

        private IEnumerable<PeopleInfo> PeopleList(IQueryable<Person> query)
        {
            var q = from p in query
                    select new PeopleInfo
                    {
                        PeopleId = p.PeopleId,
                        Name = p.Name,
                        First = p.FirstName,
                        Last = p.LastName,
                        Address = p.PrimaryAddress,
                        CityStateZip = p.PrimaryCity + ", " + p.PrimaryState + " " + p.PrimaryZip.Substring(0, 5),
                        Zip = p.PrimaryZip.Substring(0,5),
                        Age = p.Age,
                        BirthDate = p.BirthMonth + "/" + p.BirthDay + "/" + p.BirthYear,
                        HomePhone = p.HomePhone,
                        CellPhone = p.CellPhone,
                        WorkPhone = p.WorkPhone,
                        MemberStatus = p.MemberStatus.Description,
                        Email = p.EmailAddress,
                        HasPicture = p.PictureId != null,
                    };                    
            return q;
        }

        public int Count
        {
            get
            {
                return ApplySearch().Count();
            }
        }
        public IEnumerable<PeopleInfo> PeopleList()
        {
            var q = ApplySearch().OrderBy(p => p.Name2).Take(20);
            return PeopleList(q);
        }

        private IQueryable<Person> query = null;
        private IQueryable<Person> ApplySearch()
        {
            if (query.IsNotNull())
                return query;
            var Db = DbUtil.Db;
            if (Util2.OrgMembersOnly)
                query = DbUtil.Db.OrgMembersOnlyTag2().People(DbUtil.Db);
            else
                query = Db.People.Select(p => p);

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
            if (Addr.IsNotNull())
            {
                Addr = Addr.Trim();
                if (Addr.HasValue())
                    query = from p in query
                            where p.Family.AddressLineOne.Contains(Addr)
                               || p.Family.AddressLineTwo.Contains(Addr)
                               || p.Family.CityName.Contains(Addr)
                               || p.Family.ZipCode.Contains(Addr)
                            select p;
            }
            if (Comm.IsNotNull())
            {
                Comm = Comm.Trim();
                if (Comm.HasValue())
                    query = from p in query
                            where p.CellPhone.Contains(Comm) || p.EmailAddress.Contains(Comm)
                            || p.Family.HomePhone.Contains(Comm)
                            || p.WorkPhone.Contains(Comm)
                            select p;
            }
            return query;
        }
    }
}
