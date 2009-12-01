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
        private int? _OpportunityId;
        public int? OpportunityId
        {
            get
            {
                if (_OpportunityId != null)
                    return _OpportunityId;
                _OpportunityId = DbUtil.Db.UserPreference("OpportunityId", "1").ToInt2();
                return _OpportunityId;
            }
            set
            {
                _OpportunityId = value;
                DbUtil.Db.SetUserPreference("OpportunityId", value);
                Opportunity = DbUtil.Db.VolOpportunities.SingleOrDefault(o => o.Id == value);
            }
        }
        public VolOpportunity Opportunity;
        public string reportcontent
        {
            get
            {
                if (Opportunity == null)
                    return string.Empty;
                return DbUtil.Content(Opportunity.FormContent + "Report").Body;
            }
        }

        public int InterestId { get; set; }

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
            var q = from i in DbUtil.Db.VolInterests
                    where i.OpportunityCode == OpportunityId
                    where InterestId == 0 || i.VolInterestInterestCodes.Any(vi => vi.VolInterestCode.Id == InterestId)
                    select i;

            count = q.Count();
            if (Dir == "asc")
                switch (Sort)
                {
                    case "Date":
                        q = q.OrderBy(i => i.Created);
                        break;
                    case "Name":
                        q = q.OrderBy(i => i.Person.Name2);
                        break;
                    case "Application":
                        q = from i in q
                            let cva = i.Person.Volunteers.OrderByDescending(vo => vo.ProcessedDate).FirstOrDefault()
                            orderby (cva == null || cva.StatusId != 10) ? 0 : 1
                            select i;
                        break;
                }
            else
                switch (Sort)
                {
                    case "Date":
                        q = q.OrderByDescending(i => i.Created);
                        break;
                    case "Name":
                        q = q.OrderByDescending(i => i.Person.Name2);
                        break;
                    case "Application":
                        q = from i in q
                            let cva = i.Person.Volunteers.OrderByDescending(vo => vo.ProcessedDate).FirstOrDefault()
                            orderby (cva == null || cva.StatusId != 10) ? 1 : 0
                            select i;
                        break;
                }

            var q2 = from i in q
                     let cva = i.Person.Volunteers.OrderByDescending(vo => vo.ProcessedDate).FirstOrDefault()
                     select new VolunteerInterest
                     {
                         Id = i.Id,
                         Created = i.Created.Value,
                         Name = i.Person.Name,
                         Opportunity = i.VolOpportunity.UrlKey,
                         OpportunityId = i.OpportunityCode.Value,
                         PeopleId = i.PeopleId.Value,
                         Interests = string.Join("; ",
                           i.VolInterestInterestCodes.Select(vi =>
                               vi.VolInterestCode.Description).ToArray()),
                         Answer = i.Question,
                         Application = (cva == null || cva.StatusId != 10) ? "no" : "yes"
                     };

            return q2.Skip(StartRow).Take(PageSize.Value);
        }
        public IEnumerable<SelectListItem> Opportunities()
        {
            var q = from o in DbUtil.Db.VolOpportunities
                    orderby o.Description
                    select new SelectListItem
                    {
                        Text = o.Description,
                        Value = o.Id.ToString(),
                    };
            var list = q.ToList();
            list.Insert(0, new SelectListItem { Text = "(Choose Opportunity)", Value = "0", Selected = true });
            return list;
        }
        public IEnumerable<SelectListItem> Interests()
        {
            var q = from c in DbUtil.Db.VolInterestCodes
                    where c.OpportunityId == OpportunityId
                    orderby c.Description
                    select new SelectListItem
                    {
                        Text = c.Description + "(" + c.VolInterestInterestCodes.Count() + ")",
                        Value = c.Id.ToString(),
                    };
            var list = q.ToList();
            list.Insert(0, new SelectListItem { Text = "(not specified)", Value = "0", Selected = true });
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
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public string Name { get; set; }
        public int PeopleId { get; set; }
        public string Opportunity { get; set; }
        public int OpportunityId { get; set; }
        public string Interests { get; set; }
        public string Answer { get; set; }
        public string Application { get; set; }
    }
}
