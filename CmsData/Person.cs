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
                if (SpouseId.HasValue)
                    return Family.People.Single(p => p.PeopleId == SpouseId.Value).Name;
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
        public int GetAge()
        {
            int years;
            var dt0 = GetBirthdate();
            if (!dt0.HasValue)
                return -1;
            var dt = dt0.Value;
            years = Util.Now.Year - dt.Year;
            if (Util.Now.Month < dt.Month || (Util.Now.Month == dt.Month && Util.Now.Day < dt.Day))
                years--;
            return years;
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
            Db.People.DeleteOnSubmit(this);
            Db.SubmitChanges();
        }
        public void MovePersonStuff(int otherid)
        {
            var toperson = Db.People.Single(p => p.PeopleId == otherid);
            foreach (var om in this.OrganizationMembers)
            {
                var om2 = Db.OrganizationMembers.SingleOrDefault(o => o.PeopleId == otherid && o.OrganizationId == om.OrganizationId);
                if (om2 == null)
                {
                    om.Organization.OrganizationMembers.Add(
                        new OrganizationMember
                        {
                            CreatedBy = om.CreatedBy,
                            CreatedDate = om.CreatedDate,
                            EnrollmentDate = om.EnrollmentDate,
                            InactiveDate = om.InactiveDate,
                            MemberTypeId = om.MemberTypeId,
                            ModifiedBy = Util.UserId1,
                            ModifiedDate = Util.Now,
                            PeopleId = otherid,
                            AttendPct = om.AttendPct,
                            AttendStr = om.AttendStr,
                            LastAttended = om.LastAttended,
                            Pending = om.Pending,
                        });
                    Db.OrganizationMembers.DeleteOnSubmit(om);
                }
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
            foreach (var sm in this.HisSoulMates)
                sm.HimId = otherid;
            foreach (var sm in this.HerSoulMates)
                sm.HerId = otherid;
            foreach (var rec in this.RecRegs)
                rec.PeopleId = otherid;
            foreach (var sale in this.SaleTransactions)
                sale.PeopleId = otherid;
        }
        public bool PurgePerson()
        {
            try
            {
                Db.PurgePerson(PeopleId);
            }
            catch
            {
                return false;
            }
            return true;
        }
        public bool Deceased
        {
            get { return DeceasedDate.HasValue; }
        }
        public static void NameSplit(string name, out string First, out string Last)
        {
            First = "";
            Last = "";
            if (!name.HasValue())
                return;
            var a = name.Trim().Split(' ');
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
        public static Person Add(Family fam, int position, Tag tag, string firstname, string nickname, string lastname, string dob, int MarriedCode, int gender, int originId, int? EntryPointId)
        {
            var p = new Person();
            DbUtil.Db.People.InsertOnSubmit(p);
            p.PositionInFamilyId = position;
            p.AddressTypeId = 10;

            if (firstname.HasValue())
                p.FirstName = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(firstname);
            if (nickname.HasValue())
                p.NickName = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(nickname);
            if (lastname.HasValue())
                p.LastName = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(lastname);

            p.GenderId = gender;
            if (p.GenderId == 99)
                p.GenderId = 0;
            p.MaritalStatusId = MarriedCode;

            DateTime dt;
            if (Util.DateValid(dob, out dt))
            {
                p.BirthDay = dt.Day;
                p.BirthMonth = dt.Month;
                p.BirthYear = dt.Year;
                if (p.GetAge() < 18)
                    p.MaritalStatusId = (int)Person.MaritalStatusCode.Single;
            }
            else if (DateTime.TryParse(dob, out dt))
            {
                p.BirthDay = dt.Day;
                p.BirthMonth = dt.Month;
                if (Regex.IsMatch(dob, @"\d+[-/]\d+[-/]\d+"))
                {
                    p.BirthYear = dt.Year;
                    if (p.GetAge() < 18)
                        p.MaritalStatusId = (int)Person.MaritalStatusCode.Single;
                }
            }

            p.MemberStatusId = (int)Person.MemberStatusCode.JustAdded;
            fam.People.Add(p);
            if (tag != null)
                tag.PersonTags.Add(new TagPerson { Person = p });
            if (Util.UserPeopleId.HasValue)
            {
                var tag2 = DbUtil.Db.FetchOrCreateTag("JustAdded", Util.UserPeopleId, DbUtil.TagTypeId_Personal);
                tag2.PersonTags.Add(new TagPerson { Person = p });
            }
            p.OriginId = originId;
            p.EntryPointId = EntryPointId;
            p.FixTitle();
            DbUtil.Db.SubmitChanges();
            if (Util.UserPeopleId.HasValue)
                Task.AddNewPerson(p.PeopleId);
            else
            {
                var npm = DbUtil.Db.NewPeopleManager;
                var em = DbUtil.SystemEmailAddress;
                var name = String.Empty;
                if (npm != null)
                {
                    em = npm.EmailAddress;
                    name = npm.Name;
                }
                Util.Email(em, name, em,
                        "Just Added Person on " + Util.Host,
                        "<a href='{0}Person.aspx?id={2}'>{1} ({2})</a>"
                        .Fmt(Util.ResolveServerUrl("~/"), p.Name, p.PeopleId));
            }
            return p;
        }
        public static Person Add(Family fam, int position, Tag tag, string firstname, string nickname, string lastname, string dob, bool Married, int gender, int originId, int? EntryPointId)
        {
            return Add(fam, position, tag, firstname, nickname, lastname, dob, Married ? 20 : 10, gender, originId, EntryPointId);
        }
        public void FixTitle()
        {
            if (GenderId == 1)
                TitleCode = "Mr.";
            else if (GenderId == 2)
                if (MaritalStatusId == 20 || MaritalStatusId == 50)
                    TitleCode = "Mrs.";
                else
                    TitleCode = "Ms.";
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