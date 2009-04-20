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
    public class QueryController
    {
        private CMSDataContext Db;
        public QueryController()
        {
            Db = new CMSDataContext(Util.ConnectionString);
        }
        int savedQueryCount = 0;
        public int SavedQueryCount(bool onlyMine, string sortExpression, int maximumRows, int startRowIndex)
        {
            return savedQueryCount;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<SavedQueryInfo> FetchSavedQueries(bool onlyMine, string sortExpression, int maximumRows, int startRowIndex)
        {
            var isdev = Roles.IsUserInRole("Developer");
            var q = from c in Db.QueryBuilderClauses
                    where c.SavedBy == Util.UserName || ((c.IsPublic || isdev) && !onlyMine)
                    where c.SavedBy != null || (c.GroupId == null && c.Field == "Group" && isdev && c.Clauses.Count() > 0)
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

        public int count;
        public int Count(int startRowIndex, int maximumRows,
            string sortExpression, int QueryId)
        {
            return count;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<TaggedPersonInfo> FetchPeopleList(int startRowIndex, int maximumRows,
            string sortExpression, int QueryId)
        {
            var q = PersonQuery(QueryId);
            count = q.Count();
            if (sortExpression.HasValue())
                q = PersonSearchController.ApplySort(q, sortExpression);
            q = q.Skip(startRowIndex).Take(maximumRows);
            return new PersonSearchController().FetchPeopleList(q);
        }
        public string QueryDescription { get; set; }
        public IQueryable<Person> PersonQuery(int QueryId)
        {
            var Qb = Db.LoadQueryById(QueryId);
            var q = Db.People.Where(Qb.Predicate());
            QueryDescription = Qb.Description;
            return q;
        }
        public void TagAll(int QueryId)
        {
            var Qb = Db.LoadQueryById(QueryId);
            var q = Db.People.Where(Qb.Predicate());
            Db.TagAll(q);
        }
        public void UnTagAll(int QueryId)
        {
            var Qb = Db.LoadQueryById(QueryId);
            var q = Db.People.Where(Qb.Predicate());
            Db.UnTagAll(q);
        }
        public NewContact AddContact(int QueryId)
        {
            var Qb = Db.LoadQueryById(QueryId);
            var q = Db.People.Where(Qb.Predicate());
            var c = new NewContact { ContactDate = Util.Now.Date };
            c.CreatedDate = c.ContactDate;
            c.ContactTypeId = (int)NewContact.ContactTypeCode.Other;
            c.ContactReasonId = (int)NewContact.ContactReasonCode.Other;
            if (q.Count() > 500)
                return null;
            foreach (var p in q)
            {
                c.contactees.Add(new Contactee { PeopleId = p.PeopleId });
            }
            Db.NewContacts.InsertOnSubmit(c);
            Db.SubmitChanges();
            return c;
        }
    }
}
