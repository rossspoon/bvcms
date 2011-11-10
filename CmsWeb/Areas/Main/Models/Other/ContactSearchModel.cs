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
using System.Web.Mvc;
using System.Web.Routing;
using System.Text.RegularExpressions;
using System.Threading;
using System.Data.Linq;
using CmsData;
using System.Collections;
using CMSPresenter;

namespace CmsWeb.Models
{
    public class ContactSearchModel : PagerModel2
    {
        public string ContacteeName { get; set; }
        public string ContactorName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? ContactType { get; set; }
        public int? ContactReason { get; set; }
        public int? Status { get; set; }
        public int? Ministry { get; set; }

        public ContactSearchModel()
        {
            GetCount = Count;
        }


        private IQueryable<CmsData.Contact> contacts;
        public IEnumerable<ContactInfo> ContactList()
        {
            contacts = FetchContacts();
            if (!_count.HasValue)
                _count = contacts.Count();
            contacts = ApplySort(contacts).Skip(StartRow).Take(PageSize);
            return ContactList(contacts);
        }
        public IEnumerable<ContactInfo> ContactList(IQueryable<CmsData.Contact> query)
        {
            var q = from o in query
                    select new ContactInfo
                    {
                        ContactId = o.ContactId,
                        ContactDate = o.ContactDate,
                        ContactReason = o.ContactReason.Description,
                        TypeOfContact = o.ContactType.Description,
                        ContacteeList = string.Join(", ", (from c in DbUtil.Db.Contactees
                                                           where c.ContactId == o.ContactId
                                                           select c.person.Name).Take(3))
                    };
            return q;
        }

        private int? _count;
        public int Count()
        {
            if (!_count.HasValue)
                _count = FetchContacts().Count();
            return _count.Value;
        }
        private IQueryable<CmsData.Contact> FetchContacts()
        {
            if (contacts != null)
                return contacts;

            contacts = DbUtil.Db.Contacts.AsQueryable();
            if (ContacteeName.HasValue())
                contacts = from c in contacts
                           where c.contactees.Any(p => p.person.Name.Contains(ContacteeName))
                           select c;

            if (ContactorName.HasValue())
                contacts = from c in contacts
                           where c.contactsMakers.Any(p => p.person.Name.Contains(ContactorName))
                           select c;

            DateTime startDateRange;
            DateTime endDateRange;
            if (StartDate.HasValue)
            {
                startDateRange = StartDate.Value;
                if (EndDate.HasValue)
                    endDateRange = EndDate.Value.AddHours(+24);
                else
                    endDateRange = StartDate.Value.AddHours(+24);

            }
            else if (EndDate.HasValue)
            {
                startDateRange = DateTime.Parse("01/01/1800");
                endDateRange = EndDate.Value.AddHours(+24);
            }
            else
            {
                startDateRange = DateTime.Parse("01/01/1800");
                endDateRange = Util.Now.Date.AddHours(+24);
            }

            contacts = from c in contacts
                       where c.ContactDate >= startDateRange && c.ContactDate < endDateRange
                       select c;

            if ((ContactReason ?? 0) != 0)
                contacts = from c in contacts
                           where c.ContactReasonId == ContactReason
                           select c;

            if ((ContactType ?? 0) != 0)
                contacts = from c in contacts
                           where c.ContactTypeId == ContactType
                           select c;

            if ((Ministry ?? 0) != 0)
                contacts = from c in contacts
                        where c.MinistryId == Ministry
                        select c;

            return contacts;
        }
        public IQueryable<CmsData.Contact> ApplySort(IQueryable<CmsData.Contact> query)
        {
            if ((Direction ?? "desc") == "asc")
                switch (Sort)
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
                }
            else
                switch(Sort ?? "Date")
                {
                    case "ID":
                        query = from c in query
                                orderby c.ContactId descending
                                select c;
                        break;
                    case "Date":
                        query = from c in query
                                orderby c.ContactDate descending
                                select c;
                        break;
                    case "Reason":
                        query = from c in query
                                orderby c.ContactReasonId descending, c.ContactDate descending
                                select c;
                        break;
                    case "Type":
                        query = from c in query
                                orderby c.ContactTypeId descending, c.ContactDate descending
                                select c;
                        break;
                }
            return query;
        }
        public SelectList ContactTypes()
        {
            return new SelectList(new CodeValueController().ContactTypeCodes0(),
                "Id", "Value", ContactType.ToString());
        }
        public SelectList ContactReasons()
        {
            return new SelectList(new CodeValueController().ContactReasonCodes0(),
                "Id", "Value", ContactReason.ToString());
        }
        public SelectList Ministries()
        {
            return new SelectList(new CodeValueController().Ministries0(),
                "Id", "Value", Ministry.ToString());
        }

        public class ContactInfo
        {
            public int ContactId { get; set; }
            public string Comments { get; set; }
            public DateTime ContactDate { get; set; }
            public string TypeOfContact { get; set; }
            public string ContactReason { get; set; }
            public string ContacteeList { get; set; }
        }
    }
}
