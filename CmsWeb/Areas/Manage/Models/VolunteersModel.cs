using System;
using System.Collections.Generic;
using System.Linq;
using UtilityExtensions;
using CmsData;
using System.Data;
using System.Web.Mvc;

namespace CmsWeb.Models
{
    public class VolunteersModel
    {
        public int? QueryId { get; set; }
        public string Org { get; set; }
        public string View { get; set; }

        public string Sort { get; set; }
        public string Dir { get; set; }

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
            get { return DbUtil.Db.UserPreference("PageSize", "10").ToInt(); }
            set
            {
                if (value.HasValue)
                    DbUtil.Db.SetUserPreference("PageSize", value);
            }
        }
        private int? count;
        public int Count
        {
            get
            {
                if (!count.HasValue)
                    count = 1; //ApplySearch().Count();
                return count.Value;
            }
        }

        public IEnumerable<VolunteerInterest> FetchVolunteers()
        {
            var orgkeys = Person.OrgKeys(View);

            var q = DbUtil.Db.People.Where(p => p.VolInterestInterestCodes.Count() > 0);
            var Qb = DbUtil.Db.LoadQueryById(QueryId);
            if (Qb != null)
                q = DbUtil.Db.People.Where(Qb.Predicate());

            if (Org == "na")
                q = from p in q
                    where p.VolInterestInterestCodes.Count(c => orgkeys.Contains(c.VolInterestCode.Org)) == 0
                    select p;
            else
                q = from p in q
                    where Org == "ns" || Org == null || 
                        p.VolInterestInterestCodes.Any(vi => vi.VolInterestCode.Org == Org)
                    select p;

            //if (SmallGroup != "ns")
            //    q = from p in q
            //        where p.OrganizationMembers.Any(m => m.OrgMemMemTags.Any(mt => mt.MemberTag.Name == SmallGroup))
            //        select p;
            if (!Sort.HasValue())
            {
                Sort = "Name";
                Dir = "asc";
            }
            count = q.Count();
            if (Dir == "asc")
                switch (Sort)
                {
                    case "Name":
                        q = q.OrderBy(p => p.Name2);
                        break;
                    case "Application":
                        q = from p in q
                            let cva = p.Volunteers.OrderByDescending(vo => vo.ProcessedDate).FirstOrDefault()
                            orderby (cva == null || cva.StatusId != 10) ? 0 : 1
                            select p;
                        break;
                }
            else
                switch (Sort)
                {
                    case "Name":
                        q = q.OrderByDescending(p => p.Name2);
                        break;
                    case "Application":
                        q = from p in q
                            let cva = p.Volunteers.OrderByDescending(vo => vo.ProcessedDate).FirstOrDefault()
                            orderby (cva == null || cva.StatusId != 10) ? 1 : 0
                            select p;
                        break;
                }

            var q2 = from p in q
                     let cva = p.Volunteers.OrderByDescending(vo => vo.ProcessedDate).FirstOrDefault()
                     select new VolunteerInterest
                     {
                         Name = p.Name,
                         PeopleId = p.PeopleId,
                         Application = (cva == null || cva.StatusId != 10) ? "no" : "yes",
                         Interests = string.Join("; ",
                            (from c in p.VolInterestInterestCodes
                             where orgkeys.Contains(c.VolInterestCode.Org)
                            group c by c.VolInterestCode.Org into g
                            select g.Key).ToArray()),
                     };

            return q2.Skip(StartRow).Take(PageSize.Value);
        }
        public IEnumerable<SelectListItem> Interests()
        {
            var q = from c in DbUtil.Db.VolInterestInterestCodes
                    group c by new { c.VolInterestCode.Org, c.PeopleId } into g
                    select g.Key;
            var qq = from i in q
                     group i by i.Org into g
                     orderby g.Key
                     select new SelectListItem
                     {
                        Text = g.Key + "(" + g.Count() + ")",
                        Value = g.Key,
                    };
            var list = qq.ToList();
            list.Insert(0, new SelectListItem { Text = "No Interests", Value = "na" });
            list.Insert(0, new SelectListItem { Text = "(not specified)", Value = "ns" });
            return list;
        }
        public static IEnumerable<SelectListItem> Views()
        {
            var q = from c in DbUtil.Db.Contents
                    where c.Name.StartsWith("Volunteer-") && c.Name.EndsWith(".view")
                    let name = c.Name.Substring(10, c.Name.Length - 10 - 5)
                    orderby name
                    select new SelectListItem { Text = name };
            var list = q.ToList();
            list.Insert(0, new SelectListItem { Text = "(not specified)", Value = "ns" });
            return list;
        }
        //public string SmallGroup { get; set; }
        //public IEnumerable<SelectListItem> SmallGroups()
        //{
        //    var orgkeys = Person.OrgKeys(View);

        //    var q = DbUtil.Db.People.Where(p => p.VolInterestInterestCodes.Count() > 0);
        //    var Qb = DbUtil.Db.LoadQueryById(QueryId);
        //    if (Qb != null)
        //        q = DbUtil.Db.People.Where(Qb.Predicate());

        //    if (Org == "na")
        //        q = from p in q
        //            where p.VolInterestInterestCodes.Count(c => orgkeys.Contains(c.VolInterestCode.Org)) == 0
        //            select p;
        //    else
        //        q = from p in q
        //            where Org == "ns" || Org == null || 
        //                p.VolInterestInterestCodes.Any(vi => vi.VolInterestCode.Org == Org)
        //            select p;

        //    var q2 = from g in DbUtil.Db.OrgMemMemTags
        //             where q.Select(p => p.PeopleId).Contains(g.PeopleId)
        //             select new SelectListItem
        //             {
        //                 Text = g.MemberTag.Name,
        //             };
        //    var list = q2.ToList();
        //    list.Insert(0, new SelectListItem { Text = "(not specified)", Value = "ns" });
        //    return list;
        //}
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
    public class VolunteerInterest
    {
        public string Name { get; set; }
        public int PeopleId { get; set; }
        public string Interests { get; set; }
        public string Application { get; set; }
    }
}
