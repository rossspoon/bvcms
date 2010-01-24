using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using UtilityExtensions;

namespace CMSWeb.Models
{
    public class PersonContactsReceivedModel
    {
        public Person person;
        public PagerModel2 Pager { get; set; }
        public PersonContactsReceivedModel(int id)
        {
            person = DbUtil.Db.LoadPersonById(id);
            Pager = new PagerModel2(Count);
        }
        private IQueryable<NewContact> _contacts;
        private IQueryable<NewContact> FetchContacts()
        {
            if (_contacts == null)
                _contacts = from c in DbUtil.Db.NewContacts
                    where c.contactees.Any(p => p.PeopleId == person.PeopleId)
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
            //q = ApplySort(q, Pager.Sort);

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
        private IQueryable<NewContact> ApplySort(IQueryable<NewContact> q, string sortExpression)
        {
            switch (sortExpression)
            {
            }
            return q;
        }
    }
}
