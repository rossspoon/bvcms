/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Linq;
using UtilityExtensions;
using System.Text;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Linq.Expressions;
using System.ComponentModel;
using System.Transactions;
using System.Text.RegularExpressions;
using System.Threading;

namespace CmsData
{
    public partial class Person : IAuditable
    {
        public enum EntryPointCode
        {
            BibleFellowship = 10,
            Enrollment = 15,
            Worship = 20,
            Christmas = 30,
            Easter = 40,
            Activities = 50,
            VBS = 60,
            Music = 70,
            Fall = 80,
            SoulMateLive = 82,
            Other = 98,
        }
        public OriginCode EntryPointEnum
        {
            get { return (OriginCode)EntryPointId; }
            set { EntryPointId = (int)value; }
        }
        public enum OriginCode
        {
            Visit = 10,
            Request = 40,
            PhoneIn = 50,
            SurveyEE = 60,
            Enrollment = 70,
            Contribution = 90,
            NewFamilyMember = 100,
        }
        public OriginCode OriginEnum
        {
            get { return (OriginCode)OriginId; }
            set { OriginId = (int)value; }
        }
        public enum DecisionCode
        {
            Unknown = 0,
            ProfessionForMembership = 10,
            ProfessionNotForMembership = 20,
            Letter = 30,
            Statement = 40,
            StatementReqBaptism = 50,
            Cancelled = 60,
        }
        public enum MemberStatusCode
        {
            Member = 10,
            NotMember = 20,
            Pending = 30,
            Previous = 40,
            JustAdded = 50,
        }
        public enum DiscoveryClassStatusCode
        {
            NotSpecified = 0,
            Pending = 10,
            Attended = 20,
            AdminApproval = 30,
            GrandFathered = 40,
            ExemptedChild = 50,
            Unknown = 99
        }
        public enum BaptismTypeCode
        {
            NotSpecified = 0,
            Original = 10, // first time at pof
            Subsequent = 20, // already member
            Biological = 30, // children of members
            NonMember = 40, // pof, baptism, but not joining
            Required = 50, // statement, not dunked
        }
        public enum BaptismStatusCode
        {
            NotSpecified = 0,
            Scheduled = 10,
            NotScheduled = 20,
            Completed = 30,
            Canceled = 40,
        }
        public enum PositionInFamilyCode
        {
            Primary = 10,
            Secondary = 20,
            Child = 30
        }
        public enum JoinTypeCode
        {
            Unknown = 0,
            BaptismPOF = 10,
            BaptismSRB = 20,
            BaptismBIO = 30,
            Statement = 40,
            Letter = 50,
        }
        public enum DropTypeCode
        {
            NotDropped = 0,
            Duplicate = 10,
            Administrative = 20,
            Deceased = 30,
            LetteredOut = 40,
            Requested = 50,
            AnotherDenomination = 60,
            Other = 98,
        }
        public enum EnvelopeOptionCode
        {
            None = 0,
            Individual = 1,
            Joint = 2,
        }
        public enum MaritalStatusCode
        {
            Unknown = 0,
            Single = 10,
            Married = 20,
            Separated = 30,
            Divorced = 40,
            Widowed = 50,
        }
/* Origins
10		Visit						Worship or BFClass Visit
30		Referral					see Request
40		Request						Task, use this for Referral too
50		Deacon Telephone			Contact, type = phoned in
60		Survey (EE)					Contact, EE
70		Enrollment					Member of org
80		Membership Decision			Contact, Type=Worship Visit
90		Contribution				-1 peopleid in Excel with Name?
98		Other						Task, use task description
*/
        public string CityStateZip
        {
            get { return Util.FormatCSZ4(PrimaryCity, PrimaryState, PrimaryZip); }
        }
        public string AddrCityStateZip
        {
            get { return PrimaryAddress + ";" + CityStateZip; }
        }
        public string Addr2CityStateZip
        {
            get { return PrimaryAddress2 + ";" + CityStateZip; }
        }
        public string SpouseName
        {
            get
            {
                if (SpouseId.HasValue) return Family.People.Single(p => p.PeopleId == SpouseId.Value).Name;
                return "";
            }
        }
        public string DOB
        {
            get
            { return Util.FormatBirthday(BirthYear, BirthMonth, BirthDay); }
            set
            {
                // reset all values before replacing b/c replacement may be partial
                BirthDay = null;
                BirthMonth = null;
                BirthYear = null;
                DateTime dt;
                if (DateTime.TryParse(value, out dt))
                {
                    BirthDay = dt.Day;
                    BirthMonth = dt.Month;
                    if (Regex.IsMatch(value, @"\d+/\d+/\d+"))
                        BirthYear = dt.Year;
                }
                else
                {
                    int n;
                    if (int.TryParse(value, out n))
                        if (n >= 1 && n <= 12)
                            BirthMonth = n;
                        else
                            BirthYear = n;
                }
            }
        }
        public DateTime? GetBirthdate()
        {
            DateTime dt;
            if (DateTime.TryParse(DOB, out dt))
                return dt;
            return null;
        }

        private CMSDataContext _Db;
        public CMSDataContext Db
        {
            get
            {
                if (_Db == null)
                    _Db = this.GetDataContext() as CMSDataContext;
                return _Db;
            }
        }
        public void DeletePerson()
        {
            Db.TagPeople.DeleteAllOnSubmit(Tags);
            var f = this.Family;
            Db.People.DeleteOnSubmit(this);
            Db.SubmitChanges();
            Db.Refresh(RefreshMode.OverwriteCurrentValues, f);
            if (f.People.Count() == 0)
            {
                Db.Families.DeleteOnSubmit(f);
                Db.SubmitChanges();
            }
        }
        public void MovePersonStuff(int otherid)
        {
            var toperson = Db.People.Single(p => p.PeopleId == otherid);
            foreach (var om in this.OrganizationMembers)
            {
                om.Organization.OrganizationMembers.Add(
                    new OrganizationMember
                    {
                        CreatedBy = om.CreatedBy,
                        CreatedDate = om.CreatedDate,
                        EnrollmentDate = om.EnrollmentDate,
                        InactiveDate = om.InactiveDate,
                        MemberTypeId = om.MemberTypeId,
                        ModifiedBy = Util.UserId,
                        ModifiedDate = DateTime.Now,
                        PeopleId = otherid,
                        VipWeek1 = om.VipWeek1,
                        VipWeek2 = om.VipWeek2,
                        VipWeek3 = om.VipWeek3,
                        VipWeek4 = om.VipWeek4,
                        VipWeek5 = om.VipWeek5,
                    });
                Db.OrganizationMembers.DeleteOnSubmit(om);
            }
            foreach (var et in this.EnrollmentTransactions)
                et.PeopleId = otherid;
            var q = from a in this.Attends
                    where !toperson.Attends.Any(a2 => a2.MeetingId == a.MeetingId)
                    select a;
            foreach (var a in q)
            {
                var n = new Attend
                {
                    PeopleId = otherid,
                    MeetingId = a.MeetingId,
                    OrganizationId = a.OrganizationId,
                    MeetingDate = a.MeetingDate,
                    AttendanceFlag = a.AttendanceFlag,
                    OtherOrgId = a.OtherOrgId,
                    OtherAttends = a.OtherAttends,
                    AttendanceTypeId = a.AttendanceTypeId,
                    CreatedBy = a.CreatedBy,
                    CreatedDate = a.CreatedDate,
                    MemberTypeId = a.MemberTypeId,
                };
                Db.Attends.DeleteOnSubmit(a);
                Db.Attends.InsertOnSubmit(n);
            }
            var q2 = from a in this.Attends
                     where toperson.Attends.Any(a2 => a2.MeetingId == a.MeetingId)
                     select a;
            Db.Attends.DeleteAllOnSubmit(q2);
            foreach (var c in this.Contributions)
                c.PeopleId = otherid;
            foreach (var v in this.VolunteerForms)
                v.PeopleId = otherid;
            foreach (var v in this.Volunteers)
                v.PeopleId = otherid;
            foreach (var c in this.contactsMade)
            {
                var cp = Db.Contactors.SingleOrDefault(c2 => c2.PeopleId == otherid && c.ContactId == c2.ContactId);
                if (cp == null)
                    c.contact.contactsMakers.Add(new Contactor { PeopleId = otherid });
                Db.Contactors.DeleteOnSubmit(c);
            }
            foreach (var c in this.contactsHad)
            {
                var cp = Db.Contactees.SingleOrDefault(c2 => c2.PeopleId == otherid && c.ContactId == c2.ContactId);
                if (cp == null)
                    c.contact.contactees.Add(new Contactee { PeopleId = otherid });
                Db.Contactees.DeleteOnSubmit(c);
            }
            foreach (var f in this.VBSApps)
                f.PeopleId = otherid;
        }
        public bool PurgePerson()
        {
            try
            {
                Db.PurgePerson(PeopleId);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public bool Deceased
        {
            get { return DeceasedDate.HasValue; }
        }
        private static void NameSplit(string name, out string First, out string Last)
        {
            var a = name.Split(' ');
            First = "";
            if (a.Length > 1)
            {
                First = a[0];
                Last = a[1];
            }
            else
                Last = a[0];

        }
        public static Person Add(Family fam, int position, Tag tag, string name, string dob, bool Married, int gender, int originId, int? EntryPointId)
        {
            string First, Last;
            NameSplit(name, out First, out Last);
            if (!First.HasValue() || Married)
                switch (gender)
                {
                    case 0: First = "A"; break;
                    case 1: if (!First.HasValue()) First = "Husbander"; break;
                    case 2: First = "Wifey"; break;
                }
            return Add(fam, position, tag, First, null, Last, dob, Married, gender, originId, EntryPointId);
        }
        public static Person Add(Family fam, int position, Tag tag, string firstname, string nickname, string lastname, string dob, bool Married, int gender, int originId, int? EntryPointId)
        {
            var p = new Person();
            DbUtil.Db.People.InsertOnSubmit(p);
            p.PositionInFamilyId = position;
            p.AddressTypeId = 10;

            p.FirstName = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(firstname);
            p.NickName = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(nickname);
            p.LastName = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(lastname);

            p.GenderId = gender;
            if (p.GenderId == 99)
                p.GenderId = 0;
            if (Married)
                p.MaritalStatusId = (int)Person.MaritalStatusCode.Married;

            DateTime dt;
            if (DateTime.TryParse(dob, out dt))
            {
                p.BirthDay = dt.Day;
                p.BirthMonth = dt.Month;
                if (Regex.IsMatch(dob, @"\d+/\d+/\d+"))
                    p.BirthYear = dt.Year;
            }

            p.MemberStatusId = (int)Person.MemberStatusCode.JustAdded;
            fam.People.Add(p);
            if(tag != null)
                tag.PersonTags.Add(new TagPerson { Person = p });
            var tag2 = DbUtil.Db.FetchOrCreateTag("JustAdded", Util.UserPeopleId, DbUtil.TagTypeId_Personal);
            tag2.PersonTags.Add(new TagPerson { Person = p });
            p.OriginId = originId;
            p.EntryPointId = EntryPointId;
            return p;
        }

        #region Tags
        public static bool ToggleTag(int PeopleId, string TagName, int? OwnerId, int TagTypeId)
        {
            var Db = DbUtil.Db;
            var tag = Db.FetchOrCreateTag(TagName, OwnerId, TagTypeId);
            var tp = Db.TagPeople.SingleOrDefault(t => t.Id == tag.Id && t.PeopleId == PeopleId);
            if (tp == null)
            {
                tag.PersonTags.Add(new TagPerson { PeopleId = PeopleId });
                return true;
            }
            Db.TagPeople.DeleteOnSubmit(tp);
            return false;
        }
        //private string GetTagString(int tagtype, string user)
        //{
        //    var sb = new StringBuilder();
        //    var q = from tp in Tags
        //            where tp.Tag.PeopleId == user && tp.Tag.TypeId == tagtype
        //            select tp;
        //    if (user.HasValue())
        //        q = q.Where(tp => tp.Tag.Owner == user);
        //    foreach (var tp in q)
        //        sb.Append(tp.Tag.Name + ";");
        //    if (sb.Length > 0)
        //        sb.Remove(sb.Length - 1, 1);
        //    return sb.ToString();
        //}
        //private void AddRemoveTags(string value, int tagtype, string user)
        //{
        //    var TagSet = Tags.Where(t => t.Tag.TypeId == tagtype);
        //    if (user.HasValue())
        //        TagSet = TagSet.Where(t => t.Tag.Owner == user);

        //    var tagnames = value.Split(';');

        //    var TagDeletes = TagSet.Where(t => !tagnames.Contains(t.Tag.Name));
        //    Db.TagPeople.DeleteAllOnSubmit(TagDeletes);

        //    var TagAdds = from tagname in tagnames
        //                  join t in TagSet on tagname equals t.Tag.Name into joined
        //                  from t in joined.DefaultIfEmpty()
        //                  where t == null
        //                  select tagname;

        //    foreach (var tagname in TagAdds)
        //    {
        //        var tag = Db.Tags.SingleOrDefault(t =>
        //            (t.Owner == user || !user.HasValue()) && t.Name == tagname && t.TypeId == tagtype);

        //        if (tag == null)
        //        {
        //            tag = new Tag { Name = tagname, Owner = user, TypeId = tagtype };
        //            Db.Tags.InsertOnSubmit(tag);
        //        }
        //        Tags.Add(new TagPerson { Tag = tag });
        //    }
        //}
        partial void OnNickNameChanged()
        {
            if (NickName != null && NickName.Trim() == String.Empty)
                NickName = null;
        }
        #endregion
    }
}
