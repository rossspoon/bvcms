using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using UtilityExtensions;

namespace CmsWeb.Models
{
    public class ContactModel
    {
        private CmsData.Contact _Contact;
        public CmsData.Contact contact
        {
            get
            {
                return _Contact;
            }
            set
            {
                _Contact = value;
            }
        }
        public int Id
        {
            get { return contact.ContactId; }
            set
            {
                var q = from c in DbUtil.Db.Contacts
                        where c.ContactId == value
                        select new
                        {
                            contact = c,
                            Ministry = c.Ministry.MinistryDescription,
                            Type = c.ContactType.Description,
                            Reason = c.ContactReason.Description,
                        };
                var cc = q.SingleOrDefault();
                if (cc == null)
                    throw new Exception("Contact not found");
                Ministry = cc.Ministry;
                ContactType = cc.Type;
                Reason = cc.Reason;
                contact = cc.contact;
            }
        }
        public string ContactType { get; set; }
        public string Reason { get; set; }
        public string Ministry { get; set; }
    }
}