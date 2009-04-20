/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CmsData;
using CmsData.View;
using System.ComponentModel;
using System.Collections;
using UtilityExtensions;
using CMSPresenter.InfoClasses;

namespace CMSPresenter
{
    [DataObject]
    public class ContactSearchController
    {
        private CMSDataContext Db;

        public ContactSearchController()
        {
            Db = new CMSDataContext(Util.ConnectionString);
        }

        public int count;

        public static IEnumerable<ContactInfo> FetchContactList(IQueryable<NewContact> query)
        {
            var q = from c in query
                    select new ContactInfo
                    {
                        ContactId = c.ContactId,
                        ContactDate = c.ContactDate,
                        Comments = c.Comments,
                        ContactReason = c.NewContactReason.Description,
                        TypeOfContact = c.NewContactType.Description,
                    };
            return q;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<ContactInfo> FetchContactList(int startRowIndex,
            int maximumRows,
            string sortExpression,
            string contacteeNameSearch,
            string contactorNameSearch,
            string startDate,
            string endDate,
            int reasonCode,
            int typeCode,
            int ministryCode)
        {
            var sDate = startDate.ToDate();
            var eDate = endDate.ToDate();
            var query = from c in Db.NewContacts select c;
            query = ApplySearch(query, contacteeNameSearch, contactorNameSearch, sDate, eDate, reasonCode, typeCode, ministryCode);
            count = query.Count();
            query = ApplySort(query, sortExpression).Skip(startRowIndex).Take(maximumRows);
            return FetchContactList(query);
        }

        public int Count(int startRowIndex, 
            int maximumRows, 
            string sortExpression, 
            string contacteeNameSearch, 
            string contactorNameSearch, 
            string startDate, 
            string endDate, 
            int reasonCode, 
            int typeCode, 
            int ministryCode)
        {
            return count;
        }

        private static IQueryable<NewContact> ApplySearch(IQueryable<NewContact> query, 
            string contacteeNameSearch, 
            string contactorNameSearch, 
            DateTime? startDate, 
            DateTime? endDate, 
            int reasonCode, 
            int typeCode,
            int ministryCode)
        {
            if (contacteeNameSearch.HasValue())
                query = from c in query
                        where c.contactees.Any(p => p.person.Name.Contains(contacteeNameSearch))
                        select c;

            if (contactorNameSearch.HasValue())
                query = from c in query
                        where c.contactsMakers.Any(p => p.person.Name.Contains(contactorNameSearch))
                        select c;

            // Date Range stuff...
            DateTime startDateRange;
            DateTime endDateRange;
            if (startDate.HasValue)
            {
                startDateRange = startDate.Value;
                if (endDate.HasValue)
                    endDateRange = endDate.Value.AddHours(+24);
                else
                    endDateRange = startDate.Value.AddHours(+24);

            }
            else if (endDate.HasValue)
            {
                startDateRange = DateTime.Parse("01/01/1800");
                endDateRange = endDate.Value.AddHours(+24);
            }
            else
            {
                startDateRange = DateTime.Parse("01/01/1800");
                endDateRange = Util.Now.Date.AddHours(+24);
            }

            query = from c in query
                    where c.ContactDate >= startDateRange && c.ContactDate < endDateRange
                    select c;

            if (reasonCode != 0)
                query = from c in query
                        where c.ContactReasonId == reasonCode
                        select c;

            if (typeCode != 0)
                query = from c in query
                        where c.ContactTypeId == typeCode
                        select c;

            if (ministryCode != 0)
                query = from c in query
                        where c.MinistryId == ministryCode
                        select c;
            return query;
        }

        public static IQueryable<NewContact> ApplySort(IQueryable<NewContact> query, string sort)
        {
            if (!sort.HasValue())
                sort = "Date DESC";
            switch (sort)
            {
                case "ID":
                    query = from c in query
                            orderby c.ContactId
                            select c;
                    break;
                case "Date":
                    query = from c in query
                            orderby c.ContactDate
                            select c;
                    break;
                case "Reason":
                    query = from c in query
                            orderby c.ContactReasonId, c.ContactDate descending
                            select c;
                    break;
                case "Type":
                    query = from c in query
                            orderby c.ContactTypeId, c.ContactDate descending
                            select c;
                    break;
                case "ID DESC":
                    query = from c in query
                            orderby c.ContactId descending
                            select c;
                    break;
                case "Date DESC":
                    query = from c in query
                            orderby c.ContactDate descending
                            select c;
                    break;
                case "Reason DESC":
                    query = from c in query
                            orderby c.ContactReasonId descending, c.ContactDate descending
                            select c;
                    break;
                case "Type DESC":
                    query = from c in query
                            orderby c.ContactTypeId descending, c.ContactDate descending
                            select c;
                    break;
            }
            return query;
        }

        public string GetContacteeList(int ContactId)
        {
            var q = from c in Db.Contactees
                    where c.ContactId == ContactId
                    select c.person.Name;
            q = q.Take(3);
            return string.Join(", ", q.ToArray());
        }

    }
}
