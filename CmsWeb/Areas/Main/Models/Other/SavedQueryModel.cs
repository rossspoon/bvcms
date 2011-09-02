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
        public bool isdev { get; set; }
        public bool onlyMine { get; set; }
        public bool showscratchpads { get; set; }
        public int savedQueryCount { get; set; }
        public string sort { get; set; }

        public IEnumerable<SavedQueryInfo> FetchQueries()
        {
            isdev = Roles.IsUserInRole("Developer");
            var q = from c in DbUtil.Db.QueryBuilderClauses
                    where c.SavedBy == Util.UserName || ((c.IsPublic || isdev) && !onlyMine)
                    where c.SavedBy != null || (c.GroupId == null && c.Field == "Group" && isdev && c.Clauses.Count() > 0)
                    where !c.Description.Contains("scratchpad") || showscratchpads
                    select c;
            savedQueryCount = q.Count();
            //switch (sort)
            //{
            //    case "IsPublic":
            //        q = from c in q
            //            orderby c.IsPublic, c.SavedBy, c.Description
            //            select c;
            //        break;
            //    case "Description":
            //        q = from c in q
            //            orderby c.Description
            //            select c;
            //        break;
            //    case "LastUpdated":
            //        q = from c in q
            //            orderby c.CreatedOn
            //            select c;
            //        break;
            //    case "User":
            //        q = from c in q
            //            orderby c.SavedBy, c.Description
            //            select c;
            //        break;
            //    case "IsPublic DESC":
            //        q = from c in q
            //            orderby c.IsPublic descending, c.SavedBy, c.Description
            //            select c;
            //        break;
            //    case "Description DESC":
            //        q = from c in q
            //            orderby c.Description descending
            //            select c;
            //        break;
            //    case "LastUpdated DESC":
            //        q = from c in q
            //            orderby c.CreatedOn descending
            //            select c;
            //        break;
            //    case "User DESC":
            //        q = from c in q
            //            orderby c.SavedBy descending, c.Description
            //            select c;
            //        break;
            //}
            var q2 = from c in q
                     select new SavedQueryInfo
                     {
                         QueryId = c.QueryId,
                         Description = c.Description,
                         IsPublic = c.IsPublic,
                         LastUpdated = c.CreatedOn,
                         User = c.SavedBy
                     };
            return q2;

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