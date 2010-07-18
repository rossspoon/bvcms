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
using System.Data.Linq.SqlClient;
using System.Web;

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
            //InactiveMember = 100,
            //DeletedMember = 110,
            //DeletedProspect = 120,
            //ProspectActive = 130,
            //ProspectInactive = 140,
            //MiscActive = 150,
            //MiscInactive = 160,
            //Unknown = 170
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
        public string CityStateZip5
        {
            get { return Util.FormatCSZ(PrimaryCity, PrimaryState, PrimaryZip); }
        }
        public string AddrCityStateZip
        {
            get { return PrimaryAddress + " " + CityStateZip; }
        }
        public string Addr2CityStateZip
        {
            get { return PrimaryAddress2 + " " + CityStateZip; }
        }
        public string SpouseName
        {
            get
            {
                if (SpouseId.HasValue)
                {
                    var q = from p in Db.People
                            where p.PeopleId == SpouseId
                            select p.Name;
                    return q.SingleOrDefault();
                }
                return "";
            }
        }
        public DateTime? BirthDate
        {
            get
            {
                DateTime dt;
                if (DateTime.TryParse(DOB, out dt))
                    return dt;
                return null;
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
                    om2 = new OrganizationMember
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
                        Request = om.Request,
                        Grade = om.Grade,
                        Amount = om.Amount,
                        RegisterEmail = om.RegisterEmail,
                        ShirtSize = om.ShirtSize,
                        Tickets = om.Tickets,
                        UserData = om.UserData,
                    };
                    om.Organization.OrganizationMembers.Add(om2);
                    foreach (var m in om.OrgMemMemTags)
                    {
                        om2.OrgMemMemTags.Add(new OrgMemMemTag { MemberTagId = m.MemberTagId });
                        Db.OrgMemMemTags.DeleteOnSubmit(m);
                    }
                }
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
            foreach (var sm in this.HisSoulMates)
                sm.HimId = otherid;
            foreach (var sm in this.HerSoulMates)
                sm.HerId = otherid;

            var torecreg = toperson.RecRegs.SingleOrDefault();
            var frrecreg = RecRegs.SingleOrDefault();
            if (torecreg == null && frrecreg != null)
                frrecreg.PeopleId = otherid;

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
            p.CreatedDate = Util.Now;
            DbUtil.Db.People.InsertOnSubmit(p);
            p.PositionInFamilyId = position;
            p.AddressTypeId = 10;

            if (firstname.HasValue())
                p.FirstName = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(firstname);
            else
                p.FirstName = "Unknown";
            if (nickname.HasValue())
                p.NickName = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(nickname);
            if (lastname.HasValue())
                p.LastName = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(lastname);
            p.FirstName = p.FirstName.Truncate(25);
            p.MiddleName = p.MiddleName.Truncate(15);
            p.NickName = p.NickName.Truncate(15);
            p.LastName = p.LastName.Truncate(30);

            p.GenderId = gender;
            if (p.GenderId == 99)
                p.GenderId = 0;
            p.MaritalStatusId = MarriedCode;

            DateTime dt;
            if (Util.DateValid(dob, out dt))
            {
                if (dt > Util.Now)
                    dt = dt.AddYears(-100);
                p.BirthDay = dt.Day;
                p.BirthMonth = dt.Month;
                p.BirthYear = dt.Year;
                if (p.GetAge() < 18 && MarriedCode == 0)
                    p.MaritalStatusId = (int)Person.MaritalStatusCode.Single;
            }
            else if (DateTime.TryParse(dob, out dt))
            {
                p.BirthDay = dt.Day;
                p.BirthMonth = dt.Month;
                if (Regex.IsMatch(dob, @"\d+[-/]\d+[-/]\d+"))
                {
                    p.BirthYear = dt.Year;
                    if (p.GetAge() < 18 && MarriedCode == 0)
                        p.MaritalStatusId = (int)Person.MaritalStatusCode.Single;
                }
            }

            p.MemberStatusId = (int)Person.MemberStatusCode.JustAdded;
            if (fam == null)
            {
                fam = new Family();
                DbUtil.Db.Families.InsertOnSubmit(fam);
                p.Family = fam;
            }
            else
                fam.People.Add(p);

            var PrimaryCount = fam.People.Where(c => c.PositionInFamilyId == (int)Family.PositionInFamily.PrimaryAdult).Count();
            if (PrimaryCount > 2 && p.PositionInFamilyId == (int)Family.PositionInFamily.PrimaryAdult)
                p.PositionInFamilyId = (int)Family.PositionInFamily.SecondaryAdult;

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
            if (Util.UserPeopleId.HasValue
                    && Util.UserPeopleId.Value != DbUtil.NewPeopleManagerId
                    && !HttpContext.Current.User.IsInRole("OrgMembersOnly"))
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
                if (em != null)
                    Util.Email(em, name, em,
                            "Just Added Person on " + Util.Host,
                            "<a href='{0}Person/Index/{2}'>{1} ({2})</a>"
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
        public string OptOutKey(string FromEmail)
        {
            return Util.Encrypt("{0}|{1}".Fmt(PeopleId, FromEmail));
        }

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
        public static void Tag(int PeopleId, string TagName, int? OwnerId, int TagTypeId)
        {
            var Db = DbUtil.Db;
            var tag = Db.FetchOrCreateTag(TagName, OwnerId, TagTypeId);
            var tp = Db.TagPeople.SingleOrDefault(t => t.Id == tag.Id && t.PeopleId == PeopleId);
            var isperson = Db.People.Count(p => p.PeopleId == PeopleId) > 0;
            if (tp == null && isperson)
                tag.PersonTags.Add(new TagPerson { PeopleId = PeopleId });
        }
        public static void UnTag(int PeopleId, string TagName, int? OwnerId, int TagTypeId)
        {
            var Db = DbUtil.Db;
            var tag = Db.FetchOrCreateTag(TagName, OwnerId, TagTypeId);
            var tp = Db.TagPeople.SingleOrDefault(t => t.Id == tag.Id && t.PeopleId == PeopleId);
            if (tp != null)
                Db.TagPeople.DeleteOnSubmit(tp);
        }
        partial void OnNickNameChanged()
        {
            if (NickName != null && NickName.Trim() == String.Empty)
                NickName = null;
        }
        private bool _DecisionTypeIdChanged;
        public bool DecisionTypeIdChanged
        {
            get { return _DecisionTypeIdChanged; }
        }
        partial void OnDecisionTypeIdChanged()
        {
            _DecisionTypeIdChanged = true;
        }
        private bool _DiscoveryClassStatusIdChanged;
        public bool DiscoveryClassStatusIdChanged
        {
            get { return _DiscoveryClassStatusIdChanged; }
        }
        partial void OnDiscoveryClassStatusIdChanged()
        {
            _DiscoveryClassStatusIdChanged = true;
        }
        private bool _BaptismStatusIdChanged;
        public bool BaptismStatusIdChanged
        {
            get { return _BaptismStatusIdChanged; }
        }
        partial void OnBaptismStatusIdChanged()
        {
            _BaptismStatusIdChanged = true;
        }
        private bool _DeceasedDateChanged;
        public bool DeceasedDateChanged
        {
            get { return _DeceasedDateChanged; }
        }
        partial void OnDeceasedDateChanged()
        {
            _DeceasedDateChanged = true;
        }
        private bool _DropCodeIdChanged;
        public bool DropCodeIdChanged
        {
            get { return _DropCodeIdChanged; }
        }
        partial void OnDropCodeIdChanged()
        {
            _DropCodeIdChanged = true;
        }
        internal static int FindResCode(string zipcode)
        {
            if (zipcode.HasValue() && zipcode.Length >= 5)
            {
                var z5 = zipcode.Substring(0, 5);
                var z = DbUtil.Db.Zips.SingleOrDefault(zip => z5 == zip.ZipCode);
                if (z == null)
                    return 30;
                return z.MetroMarginalCode ?? 30;
            }
            return 30;
        }
        partial void OnZipCodeChanged()
        {
            ResCodeId = FindResCode(ZipCode);
        }
        partial void OnAltZipCodeChanged()
        {
            AltResCodeId = FindResCode(AltZipCode);
        }
        public RecReg GetRecReg()
        {
            var rr = RecRegs.SingleOrDefault();
            if (rr == null)
            {
                rr = new RecReg();
                RecRegs.Add(rr);
            }
            return rr;
        }
        public NewContact AddContactReceived(DateTime dt, NewContact.ContactTypeCode type, NewContact.ContactReasonCode reason, string notes)
        {
            var c = new NewContact
            {
                CreatedDate = Util.Now,
                CreatedBy = Util.UserId1,
                ContactDate = dt,
                ContactTypeId = (int)type,
                ContactReasonId = (int)reason,
                Comments = notes
            };
            DbUtil.Db.NewContacts.InsertOnSubmit(c);
            c.contactees.Add(new Contactee { PeopleId = PeopleId });
            DbUtil.Db.SubmitChanges();
            return c;
        }
    }
}