/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using UtilityExtensions;
using System.Web.Mvc;
using CmsData;
using CMSPresenter;
using System.Data.Linq.SqlClient;

namespace CmsWeb.Models
{
    public class MemberDirectoryModel : PagerModel2
    {
        public string Name { get; set; }
        public int OrgId { get; set; }
        public string OrgName { get; set; }

        public MemberDirectoryModel()
        {
            GetCount = Count;
            Sort = "Family";
        }
        public MemberDirectoryModel(int oid)
            : this()
        {
            OrgId = oid;
            OrgName = DbUtil.Db.Organizations.Where(oo => oo.OrganizationId == oid).Select(oo => oo.OrganizationName).Single();
        }

        private IQueryable<Person> members;
        public IEnumerable<DirectoryInfo> MemberList()
        {
            members = FetchMembers();
            if (!count.HasValue)
                count = Count();

            var q1 = members.AsQueryable();

            if (Sort == "Birthday")
                q1 = from p in q1
                     orderby DbUtil.Db.NextBirthday(p.PeopleId)
                     select p;
            else
            {
                var qf = (from p in members
                          let famname = p.Family.People.Single(hh => hh.PeopleId == hh.Family.HeadOfHouseholdId).Name2
                          group p by new { famname, p.FamilyId } into g
                          orderby g.Key.famname, g.Key.FamilyId
                          select g.Max(pp => pp.FamilyId)).Skip(StartRow).Take(PageSize); ;
                q1 = from p in q1
                     where qf.Contains(p.FamilyId)
                     let pos = (p.PositionInFamilyId == 10 ? p.GenderId : 1000 - (p.Age ?? 0))
                     let famname = p.Family.People.Single(hh => hh.PeopleId == hh.Family.HeadOfHouseholdId).Name2
                     orderby famname, p.FamilyId, p.PositionInFamilyId == 10 ? p.GenderId : 1000 - (p.Age ?? 0)
                     select p;
            }

            var q2 = from p in q1
                     select new DirectoryInfo()
                     {
                         Family = p.LastName,
                         FamilyId = p.FamilyId,
                         Name = p.PreferredName,
                         Suffix = p.SuffixCode,
                         Birthday = p.BirthDate.ToString2("m"),
                         Address = p.PrimaryAddress,
                         Address2 = p.PrimaryAddress2,
                         CityStateZip = p.CityStateZip,
                         Cell = p.CellPhone.FmtFone(),
                         Home = p.HomePhone.FmtFone(),
                         Email = p.SendEmailAddress1 != false ? p.EmailAddress : "",
                         Email2 = p.SendEmailAddress2 == true ? p.EmailAddress2 : "",
                     };

            return q2;
        }

        private int? count;
        public int Count()
        {
            if (!count.HasValue)
            {
                if (Sort == "Family")
                    count = (from pp in FetchMembers()
                             group pp by pp.FamilyId into g
                             select g.Key).Count();
                else
                    count = FetchMembers().Count();
            }
            return count.Value;
        }
        private IQueryable<Person> FetchMembers()
        {
            if (members != null)
                return members;

            members = from p in DbUtil.Db.People
                      where p.OrganizationMembers.Any(mm => mm.OrganizationId == OrgId)
                      select p;
            if (Name.HasValue())
                members = from p in members
                          where p.Name.Contains(Name)
                          select p;
            return members;
        }

        public class DirectoryInfo
        {
            public string Family { get; set; }
            public int FamilyId { get; set; }
            public string Name { get; set; }
            public string Suffix { get; set; }
            public string Birthday { get; set; }
            public string Address { get; set; }
            public string Address2 { get; set; }
            public string CityStateZip { get; set; }
            public string Home { get; set; }
            public string Cell { get; set; }
            public string Email { get; set; }
            public string Email2 { get; set; }
        }

        public MvcHtmlString AddDiv(string s)
        {
            if (s.HasValue())
                return new MvcHtmlString("<div>{0}</div>\n".Fmt(s));
            return null;
        }
    }
}
