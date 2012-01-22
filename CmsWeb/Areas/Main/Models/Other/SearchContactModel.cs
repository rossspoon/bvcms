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

namespace CmsWeb.Models
{
    public class ContactInfo
    {
        public int ContactId { get; set; }
        public string Comments { get; set; }
        public DateTime ContactDate { get; set; }
        public string TypeOfContact { get; set; }
        public string ContactReason { get; set; }
        public string Program { get; set; }
        public string Teacher { get; set; }
    }
    interface ISearchContactFormBindable
    {
        string ContacteeName { get; set; }
        string ContactorName { get; set; }
        DateTime? StartDate { get; set; }
        DateTime? EndDate { get; set; }
        int? ReasonCode { get; set; }
        int? TypeCode { get; set; }
        int? MinistryCode { get; set; }
        string Sort { get; set; }
        int? PageSize { get; set; }
    }
    public class SearchContactModel : ISearchContactFormBindable
    {
        public string ContacteeName { get; set; }
        public string ContactorName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? ReasonCode { get; set; }
        public int? TypeCode { get; set; }
        public int? MinistryCode { get; set; }
        public string Sort { get; set; }
        private int? _Page;
        public int? Page
        {
            get { return _Page ?? 0; }
            set { _Page = value; }
        }
        private int StartRow
        {
            get { return Page.Value * PageSize.Value; }
        }
        public int? PageSize
        {
            get
            {
                var ps = Util.Cookie("PageSize", "10").ToInt();
                return ps;
            }
            set
            {
                if (value.HasValue)
                    Util.Cookie("PageSize", value.Value.ToString(), 360);
            }
        }

        private CMSDataContext Db;

        public SearchContactModel()
        {
            Db = DbUtil.Db;
        }

        public IEnumerable<ContactInfo> ContactList(IQueryable<CmsData.Contact> query)
        {
            var q = from c in query
                    select new ContactInfo
                    {
                        ContactId = c.ContactId,
                        ContactDate = c.ContactDate,
                        Comments = c.Comments,
                        ContactReason = c.ContactReason.Description,
                        TypeOfContact = c.ContactType.Description,
                    };
            return q;
        }

        public IEnumerable<ContactInfo> ContactList()
        {
            var query = ApplySearch();
            query = ApplySort(query, Sort).Skip(StartRow).Take(PageSize.Value);
            return ContactList(query);
        }

        private int? count;
        public int Count
        {
            get
            {
                if (!count.HasValue)
                    count = ApplySearch().Count();
                return count.Value;
            }
        }

        private IQueryable<CmsData.Contact> ApplySearch()
        {
            var query = from c in Db.Contacts select c;
            if (ContacteeName.HasValue())
                query = from c in query
                        where c.contactees.Any(p => p.person.Name.Contains(ContacteeName))
                        select c;
            if (ContactorName.HasValue())
                query = from c in query
                        where c.contactsMakers.Any(p => p.person.Name.Contains(ContactorName))
                        select c;
            if (StartDate.HasValue)
            {
                query = query.Where(c => c.ContactDate >= StartDate);
                if (EndDate.HasValue)
                    EndDate = EndDate.Value.AddHours(+24);
                else
                    EndDate = StartDate.Value.AddHours(+24);
                query = query.Where(c => c.ContactDate < EndDate);
            }
            else if (EndDate.HasValue)
            {
                EndDate = EndDate.Value.AddHours(+24);
                query = query.Where(c => c.ContactDate < EndDate);
            }
            if (ReasonCode.HasValue && ReasonCode > 0)
                query = query.Where(c => c.ContactReasonId == ReasonCode);
            if (TypeCode.HasValue && TypeCode > 0)
                query = query.Where(c => c.ContactTypeId == TypeCode);
            if (MinistryCode.HasValue && MinistryCode > 0)
                query = query.Where(c => c.MinistryId == MinistryCode);
            return query;
        }

        public IQueryable<CmsData.Contact> ApplySort(IQueryable<CmsData.Contact> query, string sort)
        {
            if (!sort.HasValue())
                sort = "Date";
            switch (sort)
            {
                case "ID":
                    query = from c in query
                            orderby c.ContactId
                            select c;
                    break;
                case "Date":
                    query = from c in query
                            orderby c.ContactDate descending
                            select c;
                    break;
                case "Reason":
                    query = from c in query
                            orderby c.ContactReason.Description, c.ContactDate descending
                            select c;
                    break;
                case "Type":
                    query = from c in query
                            orderby c.ContactType.Description, c.ContactDate descending
                            select c;
                    break;
                case "ID DESC":
                    query = from c in query
                            orderby c.ContactId descending
                            select c;
                    break;
                case "Date DESC":
                    query = from c in query
                            orderby c.ContactDate
                            select c;
                    break;
                case "Reason DESC":
                    query = from c in query
                            orderby c.ContactReason.Description descending, c.ContactDate descending
                            select c;
                    break;
                case "Type DESC":
                    query = from c in query
                            orderby c.ContactType.Description descending, c.ContactDate descending
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
        public IEnumerable<SelectListItem> ContactTypeCodes()
        {
            var q = from c in Db.ContactTypes
                    orderby c.Description
                    select new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Description
                    };
            var list = q.ToList();
            list.Insert(0, new SelectListItem { Value = "0", Text = "(not specified)" });
            return list;
        }
        public IEnumerable<SelectListItem> ReasonTypeCodes()
        {
            var q = from c in Db.ContactReasons
                    orderby c.Description
                    select new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Description
                    };
            var list = q.ToList();
            list.Insert(0, new SelectListItem { Value = "0", Text = "(not specified)" });
            return list;
        }
        public List<SelectListItem> Ministries()
        {
            var q = from m in Db.Ministries
                    orderby m.MinistryName
                    select new SelectListItem
                    {
                        Value = m.MinistryId.ToString(),
                        Text = m.MinistryName
                    };
            var list = q.ToList();
            list.Insert(0, new SelectListItem { Text = "(Not Specified)", Value = "0" });
            return list;
        }
    }
}
