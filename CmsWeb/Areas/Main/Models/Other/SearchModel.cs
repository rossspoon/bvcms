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
    public class SearchModel
    {
        public string type { get; set; }
        private string[] noaddtypes = { "relatedfamily", "contactor", "taskdelegate", "owner", "taskdelegate2" };
        private string[] usersonlytypes = { "taskdelegate", "owner", "taskdelegate2" };
        public bool CanAdd { get { return !noaddtypes.Contains(type); } }
        public string from { get; set; }
        public int? typeid { get; set; }
        public bool UsersOnly { get { return usersonlytypes.Contains(type); } }

        private IList<SearchPersonModel> list = new List<SearchPersonModel>();
        public IList<SearchPersonModel> List
        {
            get { return list; }
            set { list = value; }
        }
        public string name { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string dob { get; set; }

        public string HelpLink(string page)
        {
            return "http://wiki.bvcms.com/help.SearchAdd_{0}.ashx".Fmt(page);
        }
        public string HelpLinkWithType(string page)
        {
            return "http://wiki.bvcms.com/help.SearchAdd_{0}_{1}.ashx".Fmt(page, type);
        }
        private IEnumerable<PeopleInfo> PeopleList(IQueryable<Person> query)
        {
            var q = from p in query
                    orderby p.Name2
                    select new PeopleInfo
                    {
                        PeopleId = p.PeopleId,
                        FamilyId = p.FamilyId,
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
                        Email = p.EmailAddress
                    };
            return q;
        }

        public IEnumerable<PeopleInfo> PeopleList()
        {
            return PeopleList(ApplySearch().Take(Showcount));
        }

        private int? _count;
        public int Count
        {
            get
            {
                if (!_count.HasValue)
                    _count = ApplySearch().Count();
                return _count.Value;
            }
        }
        private const int SHOWCOUNT = 20;
        public int Showcount
        {
            get { return Count > SHOWCOUNT ? SHOWCOUNT : Count; }
        }

        private IQueryable<Person> query = null;
        private IQueryable<Person> ApplySearch()
        {
            if (query.IsNotNull())
                return query;
            var Db = DbUtil.Db;
            query = Db.People.Select(p => p);
            if (UsersOnly)
                query = from p in query
                        where p.Users.Count() > 0
                        select p;

            if (name.HasValue())
            {
                string First, Last;
                Person.NameSplit(name, out First, out Last);
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
            if (address.IsNotNull())
            {
                address = address.Trim();
                if (address.HasValue())
                    query = from p in query
                            where p.Family.AddressLineOne.Contains(address)
                               || p.Family.AddressLineTwo.Contains(address)
                               || p.Family.CityName.Contains(address)
                               || p.Family.ZipCode.Contains(address)
                            select p;
            }
            if (phone.IsNotNull())
            {
                phone = phone.Trim();
                if (phone.HasValue())
                    query = from p in query
                            where p.CellPhone.Contains(phone) || p.EmailAddress.Contains(phone)
                            || p.Family.HomePhone.Contains(phone)
                            || p.WorkPhone.Contains(phone)
                            select p;
            }
            if (dob.HasValue())
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
            return query;
        }
        public class PeopleInfo
        {
            public int PeopleId { get; set; }
            public int FamilyId { get; set; }
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
            public string Email { get; set; }
            public string ToolTip
            {
                get
                {
                    return "{0}|PeopleId: {1}|Mobile Phone: {2}|Work Phone: {3}|Home Phone: {4}|BirthDate: {5:d}|Join Date: {6:d}|Status: {7}|{8}"
                        .Fmt(Name, PeopleId, CellPhone.FmtFone(), WorkPhone.FmtFone(), HomePhone.FmtFone(), BirthDate, JoinDate, MemberStatus, Email);
                }
            }
        }
    }
}
