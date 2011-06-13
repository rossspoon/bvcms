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
using CmsData;
using System.ComponentModel;
using System.Collections;
using System.Web.Security;
using System.Configuration;

namespace CMSPresenter
{
    [DataObject]
    public class SavedQueryController
    {
        private CMSDataContext Db;
        public SavedQueryController()
        {
            Db = DbUtil.Db;
        }
        int savedQueryCount = 0;
        public int SavedQueryCount(bool onlyMine, bool showscratchpads, string sortExpression, int maximumRows, int startRowIndex)
        {
            return savedQueryCount;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<SavedQueryInfo> FetchSavedQueries(bool onlyMine, bool showscratchpads, string sortExpression, int maximumRows, int startRowIndex)
        {
            var isdev = Roles.IsUserInRole("Developer");
            var q = from c in Db.QueryBuilderClauses
                    where c.SavedBy == Util.UserName || ((c.IsPublic || isdev) && !onlyMine)
                    where c.SavedBy != null || (c.GroupId == null && c.Field == "Group" && isdev && c.Clauses.Count() > 0)
                    where !c.Description.Contains("scratchpad") || showscratchpads
                    select c;
            savedQueryCount = q.Count();
            if (!sortExpression.HasValue())
                sortExpression = "LastUpdated DESC";
            switch (sortExpression)
            {
                case "IsPublic":
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
                case "User":
                    q = from c in q
                        orderby c.SavedBy, c.Description
                        select c;
                    break;
                case "IsPublic DESC":
                    q = from c in q
                        orderby c.IsPublic descending, c.SavedBy, c.Description
                        select c;
                    break;
                case "Description DESC":
                    q = from c in q
                        orderby c.Description descending
                        select c;
                    break;
                case "LastUpdated DESC":
                    q = from c in q
                        orderby c.CreatedOn descending
                        select c;
                    break;
                case "User DESC":
                    q = from c in q
                        orderby c.SavedBy descending, c.Description
                        select c;
                    break;
            }
            var q2 = from c in q
                     select new SavedQueryInfo
                     {
                         QueryId = c.QueryId,
                         Description = c.Description,
                         IsPublic = c.IsPublic,
                         LastUpdated = c.CreatedOn,
                         User = c.SavedBy
                     };
            return q2.Skip(startRowIndex).Take(maximumRows);
        }
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public void Update(int queryId, string Description, bool IsPublic, string User)
        {
            var qb = Db.QueryBuilderClauses.Single(c => c.QueryId == queryId);
            qb.Description = Description;
            qb.IsPublic = IsPublic;
            qb.SavedBy = User;
            Db.SubmitChanges();
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public void Delete(int queryId)
        {
            var qb = Db.QueryBuilderClauses.Single(c => c.QueryId == queryId);
            Db.DeleteQueryBuilderClauseOnSubmit(qb);
            Db.SubmitChanges();
        }
    }
}
