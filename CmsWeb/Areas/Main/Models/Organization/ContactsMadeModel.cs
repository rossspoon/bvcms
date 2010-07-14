using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using UtilityExtensions;

namespace CmsWeb.Models.OrganizationPage
{
    public class PersonContactsMadeModel
    {
        public CmsData.Person person;
        public PagerModel2 Pager { get; set; }
        public PersonContactsMadeModel(int id)
        {
            person = DbUtil.Db.LoadPersonById(id);
            Pager = new PagerModel2(Count);
        }
        private IQueryable<NewContact> _contacts;
        private IQueryable<NewContact> FetchContacts()
        {
            if (_contacts == null)
                _contacts = from c in DbUtil.Db.NewContacts
                    where c.contactsMakers.Any(p => p.PeopleId == person.PeopleId)
                    orderby c.ContactDate descending
                    select c;
            return _contacts;
        }
        int? _count;
        public int Count()
        {
            if (!_count.HasValue)
                _count = FetchContacts().Count();
            return _count.Value;
        }
        public IEnumerable<ContactInfo> Contacts()
        {
            var q = FetchContacts();
            var q2 = from c in q
                     select new ContactInfo
                     {
                         ContactId = c.ContactId,
                         Comments = c.Comments,
                         ContactDate = c.ContactDate,
                         ContactReason = c.NewContactReason.Description,
                         Program = "",
                         Teacher = "",
                         TypeOfContact = c.NewContactType.Description
                     };
            return q2.Skip(Pager.StartRow).Take(Pager.PageSize);
        }
    }
}
