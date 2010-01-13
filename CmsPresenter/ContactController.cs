/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Linq;
using System.Text;
using CmsData;
using CmsData.View;
using CMSPresenter.InfoClasses;
using UtilityExtensions;
using System.Web;

namespace CMSPresenter
{
    public class ContactController
    {
        private CMSDataContext Db;
        public ContactController()
        {
            Db = DbUtil.Db;
        }

        public IEnumerable<ContactInfo> ContactList(int pid)
        {
            var Teacher = Db.People.Where(p => p.PeopleId == pid).Select(p => p.BFClass.LeaderName).SingleOrDefault();
            var q = from c in Db.NewContacts
                    where c.contactees.Any(p => p.PeopleId == pid)
                    orderby c.ContactDate descending
                    select new ContactInfo
                    {
                        ContactId = c.ContactId,
                        Comments = c.Comments,
                        ContactDate = c.ContactDate,
                        ContactReason = c.NewContactReason.Description,
                        Program = c.Ministry.MinistryDescription,
                        Teacher = Teacher,
                        TypeOfContact = c.NewContactType.Description
                    };
            return q;
        }

        private int _contactsCount;

        public int ContactsCount(int pid, string sortExpression, int maximumRows, int startRowIndex)
        {
            return _contactsCount;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<ContactInfo> ContactedList(int startRowIndex, int maximumRows, string sortExpression, int pid)
        {
            var q = from c in Db.NewContacts
                    where c.contactees.Any(p => p.PeopleId == pid)
                    orderby c.ContactDate descending
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
            _contactsCount = q.Count();
            return q.Skip(startRowIndex).Take(maximumRows);
        }

        private int _contactsMadeCount;

        public int ContactsMadeCount(int pid, string sortExpression, int maximumRows, int startRowIndex)
        {
            return _contactsMadeCount;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<ContactInfo> ContactsMadeList(int startRowIndex, int maximumRows, string sortExpression, int pid)
        {
            var q = from c in Db.NewContacts
                    where c.contactsMakers.Any(p => p.PeopleId == pid)
                    orderby c.ContactDate descending
                    select c;
            _contactsMadeCount = q.Count();
            var q2 = from c in q.Skip(startRowIndex).Take(maximumRows)
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
            return q2;
        }


        private int _contacteeCount;

        public int ContacteeCount(int cid, string sortExpression, int maximumRows, int startRowIndex)
        {
            return _contacteeCount;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<ContacteeInfo> ContacteeList(int startRowIndex, int maximumRows, string sortExpression, int cid)
        {
            var q = from c in Db.Contactees
                    where c.ContactId == cid
                    select c;
            var isdev = HttpContext.Current.User.IsInRole("Developer");
            _contacteeCount = q.Count();
            var q2 = from c in q.Skip(startRowIndex).Take(maximumRows)
                     let task = Db.Tasks.FirstOrDefault(t =>
                         t.WhoId == c.PeopleId && t.SourceContactId == cid)
                     select new ContacteeInfo
                     {
                         ContactId = c.ContactId,
                         PeopleId = c.PeopleId,
                         PrayedForPerson = c.PrayedForPerson,
                         ProfessionOfFaith = c.ProfessionOfFaith,
                         Name = c.person.Name,
                         TaskId = task.Id,
                     };
            return q2;
        }
        public bool CanViewComments(int cid)
        {
            if (!Util.OrgMembersOnly)
                return true;

            var q = from c in Db.Contactees
                    where c.ContactId == cid
                    select c.PeopleId;
            var q2 = from c in Db.Contactors
                     where c.ContactId == cid
                     select c.PeopleId;
            var a = q.Union(q2).ToArray();

            var t = DbUtil.Db.OrgMembersOnlyTag.People().Any(p => a.Contains(p.PeopleId));
            return t;
        }

        private int _contactorCount;

        public int ContactorCount(int cid, string sortExpression, int maximumRows, int startRowIndex)
        {
            return _contactorCount;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<ContactorInfo> ContactorList(int startRowIndex, int maximumRows, string sortExpression, int cid)
        {
            var q = from c in Db.Contactors
                    where c.ContactId == cid
                    select c;
            _contactorCount = q.Count();
            var q2 = from c in q.Skip(startRowIndex).Take(maximumRows)
                     select new ContactorInfo
                     {
                         ContactId = c.ContactId,
                         PeopleId = c.PeopleId,
                         Name = c.person.Name
                     };
            return q2;
        }

        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public void DeleteContactee(int ContactId, int PeopleId)
        {
            var contactee = Db.Contactees.SingleOrDefault(c => c.ContactId == ContactId && c.PeopleId == PeopleId);
            if (contactee == null)
                return;
            Db.Contactees.DeleteOnSubmit(contactee);
            Db.SubmitChanges();
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public void UpdateContactee(int ContactId, int PeopleId, bool ProfessionOfFaith, bool PrayedForPerson)
        {
            var contactee = Db.Contactees.Single(c => c.ContactId == ContactId && c.PeopleId == PeopleId);
            contactee.ProfessionOfFaith = ProfessionOfFaith;
            contactee.PrayedForPerson = PrayedForPerson;
            Db.SubmitChanges();
        }

        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public void DeleteContactor(int ContactId, int PeopleId)
        {
            var contactor = Db.Contactors.SingleOrDefault(c => c.ContactId == ContactId && c.PeopleId == PeopleId);
            if (contactor == null)
                return;
            Db.Contactors.DeleteOnSubmit(contactor);
            Db.SubmitChanges();
        }

        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public void DeleteContact(int ContactId)
        {

            var contact = Db.NewContacts.Single(c => c.ContactId == ContactId);
            foreach (var t in contact.TasksAssigned)
                t.SourceContactId = null;
            foreach (var t in contact.TasksCompleted)
                t.CompletedContactId = null;
            Db.NewContacts.DeleteOnSubmit(contact);
            var contactees = Db.Contactees.Where(tees => tees.ContactId == ContactId);
            Db.Contactees.DeleteAllOnSubmit(contactees);
            var contactors = Db.Contactors.Where(tors => tors.ContactId == ContactId);
            Db.Contactors.DeleteAllOnSubmit(contactors);
            Db.SubmitChanges();
        }
    }
}
