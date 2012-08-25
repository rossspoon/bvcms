using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CmsData.Codes;
using UtilityExtensions;

namespace CmsData
{
    public partial class Contact
    {
        public static int AddContact(int qid)
        {
            var q = DbUtil.Db.PeopleQuery(qid);
            if (q.Count() > 100)
                return -1;
            if (q.Count() == 0)
                return 0;
            var c = new Contact 
			{ 
				ContactDate = DateTime.Now.Date, 
				CreatedBy = Util.UserId1,
	            CreatedDate = DateTime.Now,
	            ContactTypeId = ContactTypeCode.Other,
	            ContactReasonId = ContactReasonCode.Other,
			};
            foreach (var p in q)
                c.contactees.Add(new Contactee { PeopleId = p.PeopleId });
            DbUtil.Db.Contacts.InsertOnSubmit(c);
            DbUtil.Db.SubmitChanges();
            return c.ContactId;
        }

    }
}
