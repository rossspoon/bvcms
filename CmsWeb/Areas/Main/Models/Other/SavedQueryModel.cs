using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using CmsData;
using UtilityExtensions;

namespace CmsWeb.Models
{
    public class SavedQueryModel
    {
        public PagerModel2 Pager { get; set; }
        public bool isdev { get; set; }
        public bool onlyMine { get; set; }
        public bool showscratchpads { get; set; }

        public SavedQueryModel()
        {
            Pager = new PagerModel2(Count);
            Pager.Direction = "asc";
            Pager.Sort = "Name";
        }
        private int? _count;
        public int Count()
        {
            if (!_count.HasValue)
                _count = fetchqueries().Count();
            return _count.Value;
        }
        private IQueryable<QueryBuilderClause> _queries;
        private IQueryable<QueryBuilderClause> fetchqueries()
        {
            if (_queries != null)
                return _queries;
            isdev = Roles.IsUserInRole("Developer");
            _queries = from c in DbUtil.Db.QueryBuilderClauses
                       where c.SavedBy == Util.UserName || ((c.IsPublic || isdev) && !onlyMine)
                       where c.SavedBy != null || (c.GroupId == null && c.Field == "Group" && isdev && c.Clauses.Count() > 0)
                       where !c.Description.Contains("scratchpad") || showscratchpads
                       select c;
            return _queries;
        }
        public IEnumerable<SavedQueryInfo> FetchQueries()
        {
            var q = fetchqueries();
            var q2 = ApplySort(q).Skip(Pager.StartRow).Take(Pager.PageSize);
            var q3 = from c in q2
                     select new SavedQueryInfo
                     {
                         QueryId = c.QueryId,
                         Description = c.Description,
                         IsPublic = c.IsPublic,
                         LastUpdated = c.CreatedOn,
                         User = c.SavedBy
                     };
            return q3;
        }
        private IEnumerable<QueryBuilderClause> ApplySort(IQueryable<QueryBuilderClause> q)
        {
            switch (Pager.Direction)
            {
                case "asc":
                    switch (Pager.Sort)
                    {
                        case "Public":
                            q = from c in q
                                orderby c.IsPublic, c.SavedBy, c.Description
                                select c;
                            break;
                        case "Description":
                            q = from c in q
                                orderby c.Description
                                select c;
                            break;
                        case "LastUpdated":
                            q = from c in q
                                orderby c.CreatedOn
                                select c;
                            break;
                        case "Owner":
                            q = from c in q
                                orderby c.SavedBy, c.Description
                                select c;
                            break;
                    }
                    break;
                case "desc":
                    switch (Pager.Sort)
                    {
                        case "Public":
                            q = from c in q
                                orderby c.IsPublic descending, c.SavedBy, c.Description
                                select c;
                            break;
                        case "Description":
                            q = from c in q
                                orderby c.Description descending
                                select c;
                            break;
                        case "LastUpdated":
                            q = from c in q
                                orderby c.CreatedOn descending
                                select c;
                            break;
                        case "Owner":
                            q = from c in q
                                orderby c.SavedBy descending, c.Description
                                select c;
                            break;
                    }
                    break;
            }
            return q;
        }
    }
    public class SavedQueryInfo
    {
        public int QueryId { get; set; }
        public bool IsPublic { get; set; }
        public string User { get; set; }
        public string Description { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}