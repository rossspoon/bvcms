using System;
using System.Collections.Generic;
using System.Linq;
using UtilityExtensions;
using CmsData;
using System.Data;
using System.Web.Mvc;

namespace CMSWeb.Models
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


/*

4	_Greeter	BLM	Greeter
30	_City_of_Memphis_Cleanup_Project	BLM	City of Memphis Clean-up Project
32	_Impact_Ministries_Food_Pantry_Thrift_Store	BLM	Impact Ministries - Food Pantry/Thrift Store
34	_ICare_Baptist_East_and_St_Francis_Bartlett	BLM	I-Care Baptist East and St. Francis Bartlett
35	_Prayer_Room	BLM	Prayer Room
36	_Bella_Vista_Painting	BLM	Bella Vista - Painting
37	_Bella_Vista_Fence_Installation	BLM	Bella Vista - Fence Installation
38	_Manassas_High_School_Project	BLM	Manassas High School Project
39	_Cordova_High_School_Project	BLM	Cordova High School Project
40	_Arlington_Sheriff_Sub_Station_Cookout	BLM	Arlington Sheriff's Sub-Station Cookout
41	_Kennedy_Home_Project	BLM	Kennedy Home Project
42	_Rhea_Avenue_Project	BLM	Rhea Avenue Project
43	_Confidential_Care_for_Women_Project	BLM	Confidential Care for Women Project
44	_Treadwell_Elementary_Project	BLM	Treadwell Elementary Project
45	_Dillard_Home_Project	BLM	Dillard Home Project
46	_Grace_House_of_Memphis	BLM	Grace House of Memphis
47	_Knowledge_Quest_Project	BLM	Knowledge Quest Project
48	_Bellevue_Loves_Our_Housekeeping_Staff	BLM	Bellevue Loves Our Housekeeping Staff
49	_Cancer_Survivor_Park_Project	BLM	Cancer Survivor's Park Project
50	_Audubon_Park_Project	BLM	Audubon Park Project
51	_Shepherds_Haven_Project	BLM	Shepherd's Haven Project
52	_TN_Baptist_Childrens_Home_Bartlett_Project	BLM	TN Baptist Children's Home Bartlett Project
63	_Bellevue_Loves_Memphis	BLM	Bellevue Loves Memphis
*/