using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CmsData
{
    public partial class NewContact
    {
        public enum ContactTypeCode
        {
            PersonalVisit = 1,
            PhoneCall = 2,
            LetterSent = 3,
            CardSent = 4,
            EmailSent = 5,
            InfoPackSent = 6,
            Other = 7,
            PhoneIn = 11,
            SurveyEE = 12,
        }
        public ContactTypeCode ContactTypeEnum
        {
            get { return (ContactTypeCode)ContactTypeId; }
            set { ContactTypeId = (int)value; }
        }
        public enum ContactReasonCode
        {
            Unknown = 99,
            Bereavement = 100,
            Health = 110,
            Personal = 120,
            OutReach = 130,
            ComeAndSee = 131,
            InReach = 140,
            Information = 150,
            Other = 160,
        }
        public ContactReasonCode ContactReasonEnum
        {
            get { return (ContactReasonCode)ContactReasonId; }
            set { ContactReasonId = (int)value; }
        }
        public static int AddContact(int qid)
        {
            var q = DbUtil.Db.PeopleQuery(qid);
            if (q.Count() > 100)
                return -1;
            if (q.Count() == 0)
                return 0;
            var c = new NewContact { ContactDate = DateTime.Now.Date };
            c.CreatedDate = c.ContactDate;
            c.ContactTypeId = (int)NewContact.ContactTypeCode.Other;
            c.ContactReasonId = (int)NewContact.ContactReasonCode.Other;
            foreach (var p in q)
                c.contactees.Add(new Contactee { PeopleId = p.PeopleId });
            DbUtil.Db.NewContacts.InsertOnSubmit(c);
            DbUtil.Db.SubmitChanges();
            return c.ContactId;
        }

    }
}
