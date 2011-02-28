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
            None = 9,
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
        public string SpouseName(CMSDataContext Db)
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
        public void MovePersonStuff(CMSDataContext Db, int otherid)
        {
            var toperson = Db.People.Single(p => p.PeopleId == otherid);
            foreach (var om in this.OrganizationMembers)
            {
                var om2 = OrganizationMember.InsertOrgMembers(Db, om.OrganizationId, otherid, om.MemberTypeId, om.EnrollmentDate.Value, om.InactiveDate, om.Pending ?? false);
                om2.CreatedBy = om.CreatedBy;
                om2.CreatedDate = om.CreatedDate;
                om2.AttendPct = om.AttendPct;
                om2.AttendStr = om.AttendStr;
                om2.LastAttended = om.LastAttended;
                om2.Request = om.Request;
                om2.Grade = om.Grade;
                om2.Amount = om.Amount;
                om2.RegisterEmail = om.RegisterEmail;
                om2.ShirtSize = om.ShirtSize;
                om2.Tickets = om.Tickets;
                om2.UserData = om.UserData;
                Db.SubmitChanges();
                foreach (var m in om.OrgMemMemTags)
                    om2.OrgMemMemTags.Add(new OrgMemMemTag { MemberTagId = m.MemberTagId });
                Db.SubmitChanges();
                Db.OrgMemMemTags.DeleteAllOnSubmit(om.OrgMemMemTags);
                Db.SubmitChanges();
            }
            Db.OrganizationMembers.DeleteAllOnSubmit(this.OrganizationMembers);
            Db.SubmitChanges();

            foreach (var et in this.EnrollmentTransactions)
                et.PeopleId = otherid;
            Db.SubmitChanges();

            var q = from a in Db.Attends
                    where a.PeopleId == this.PeopleId
                    let oa = Db.Attends.SingleOrDefault(a2 => a2.MeetingId == a.MeetingId && a2.PeopleId == otherid)
                    where oa == null
                    select a;
            var list = q.ToList();
            foreach (var a in list)
            {
                a.PeopleId = otherid;
                Db.SubmitChanges();
            }

            foreach (var c in this.Contributions)
                c.PeopleId = otherid;
            foreach (var u in this.Users)
                u.PeopleId = otherid;
            foreach (var v in this.Volunteers)
            {
                var vv = new Volunteer
                {
                    PeopleId = otherid,
                    Children = v.Children,
                    Comments = v.Comments,
                    Leader = v.Leader,
                    ProcessedDate = v.ProcessedDate,
                    Standard = v.Standard,
                    StatusId = v.StatusId,
                };
                Db.Volunteers.InsertOnSubmit(vv);
            }
            foreach (var v in this.VolunteerForms)
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

            var torecreg = toperson.RecRegs.SingleOrDefault();
            var frrecreg = RecRegs.SingleOrDefault();
            if (torecreg == null && frrecreg != null)
                frrecreg.PeopleId = otherid;

            foreach (var sale in this.SaleTransactions)
                sale.PeopleId = otherid;
        }
        public bool PurgePerson(CMSDataContext Db)
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
        public string FromEmail
        {
            get
            {
                if (EmailAddress.HasValue())
                    return Name + " <" + EmailAddress + ">";
                return String.Empty;
            }
        }
        public string FromEmail2
        {
            get
            {
                if (EmailAddress2.HasValue())
                    return Name + " <" + EmailAddress2 + ">";
                return String.Empty;
            }
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
        public static Person Add(Family fam,
            int position,
            Tag tag,
            string firstname,
            string nickname,
            string lastname,
            string dob,
            int MarriedCode,
            int gender,
            int originId,
            int? EntryPointId)
        {
            return Person.Add(DbUtil.Db, true, fam, position, tag, firstname, nickname, lastname, dob, MarriedCode, gender, originId, EntryPointId);
        }
        public static Person Add(CMSDataContext Db, bool SendNotices, Family fam, int position, Tag tag, string firstname, string nickname, string lastname, string dob, int MarriedCode, int gender, int originId, int? EntryPointId)
        {
            var p = new Person();
            p.CreatedDate = Util.Now;
            Db.People.InsertOnSubmit(p);
            p.PositionInFamilyId = position;
            p.AddressTypeId = 10;

            if (firstname.HasValue())
                p.FirstName = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(firstname);
            else
                p.FirstName = "?";
            if (nickname.HasValue())
                p.NickName = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(nickname);
            if (lastname.HasValue())
                p.LastName = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(lastname);
            else
                p.LastName = "?";
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
                Db.Families.InsertOnSubmit(fam);
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
                var tag2 = Db.FetchOrCreateTag("JustAdded", Util.UserPeopleId, DbUtil.TagTypeId_Personal);
                tag2.PersonTags.Add(new TagPerson { Person = p });
            }
            p.OriginId = originId;
            p.EntryPointId = EntryPointId;
            p.FixTitle();
            Db.SubmitChanges();
            if (SendNotices)
            {
                var NewPeopleManagerId = Db.Settings.SingleOrDefault(ss => ss.Id == "NewPeopleManagerId").SettingX.ToInt2();
                if (!NewPeopleManagerId.HasValue)
                    NewPeopleManagerId = 1;
                if (Util.UserPeopleId.HasValue
                        && Util.UserPeopleId.Value != NewPeopleManagerId
                        && !HttpContext.Current.User.IsInRole("OrgMembersOnly")
                        && HttpContext.Current.User.IsInRole("Access"))
                    Task.AddNewPerson(p.PeopleId);
                else
                {
                    var em = Db.NewPeopleEmailAddress;
                    Util.Email(Util.Smtp(), Util.SysFromEmail, em, "Just Added Person on " + Db.Host, "{0} ({1})".Fmt(p.Name, p.PeopleId));
                }
            }
            return p;
        }
        public static Person Add(Family fam, int position, Tag tag, string firstname, string nickname, string lastname, string dob, bool Married, int gender, int originId, int? EntryPointId)
        {
            return Add(fam, position, tag, firstname, nickname, lastname, dob, Married ? 20 : 10, gender, originId, EntryPointId);
        }
        public List<Duplicate> PossibleDuplicates()
        {
            var fone = Util.GetDigits(Util.PickFirst(CellPhone, HomePhone));
            using (var ctx = new CMSDataContext(Util.ConnectionString))
            {
                ctx.SetNoLock();
                string street = GetStreet(ctx) ?? "--";
                var nick = NickName ?? "--";
                var maid = MaidenName ?? "--";
                var em = EmailAddress ?? "--";
                if (!em.HasValue())
                    em = "--";
                var bd = BirthDay ?? -1;
                var bm = BirthMonth ?? -1;
                var byr = BirthYear ?? -1;
                var q = from p in ctx.People
                        let firstmatch = p.FirstName == FirstName || (p.NickName ?? "") == FirstName || (p.MiddleName ?? "") == FirstName
                                    || p.FirstName == nick || (p.NickName ?? "") == nick || (p.MiddleName ?? "") == nick
                        let lastmatch = p.LastName == LastName || (p.MaidenName ?? "") == LastName
                                    || (p.MaidenName ?? "") == maid || p.LastName == maid
                        let nobday = (p.BirthMonth == null && p.BirthYear == null && p.BirthDay == null)
                                    || (BirthMonth == null && BirthYear == null && BirthDay == null)
                        let bdmatch = (p.BirthDay ?? -2) == bd && (p.BirthMonth ?? -2) == bm && (p.BirthYear ?? -2) == byr
                        let bdmatchpart = (p.BirthDay ?? -2) == bd && (p.BirthMonth ?? -2) == bm
                        let emailmatch = p.EmailAddress != null && p.EmailAddress == em
                        let addrmatch = (p.AddressLineOne ?? "").Contains(street) || (p.Family.AddressLineOne ?? "").Contains(street)
                        let phonematch = (p.CellPhoneLU == CellPhoneLU
                                            || p.CellPhoneLU == Family.HomePhoneLU
                                            || p.CellPhone == WorkPhoneLU
                                            || p.Family.HomePhoneLU == CellPhoneLU
                                            || p.Family.HomePhoneLU == Family.HomePhoneLU
                                            || p.Family.HomePhoneLU == WorkPhoneLU
                                            || p.WorkPhoneLU == CellPhoneLU
                                            || p.WorkPhoneLU == Family.HomePhoneLU
                                            || p.WorkPhoneLU == WorkPhoneLU)
                        let samefamily = p.FamilyId == FamilyId
                        let nmatches = samefamily ? 0 :
                                        (firstmatch ? 1 : 0)
                                        + (bdmatch ? 1 : 0)
                                        + (emailmatch ? 1 : 0)
                                        + (phonematch ? 1 : 0)
                                        + (addrmatch ? 1 : 0)
                        where (lastmatch && nmatches >= 3)
                                || ((firstmatch && lastmatch && bdmatchpart) && p.PeopleId != PeopleId)
                        select new Duplicate
                                                {
                                                    PeopleId = p.PeopleId,
                                                    First = p.FirstName,
                                                    Last = p.LastName,
                                                    Nick = p.NickName,
                                                    Middle = p.MiddleName,
                                                    BMon = p.BirthMonth,
                                                    BDay = p.BirthDay,
                                                    BYear = p.BirthYear,
                                                    Email = p.EmailAddress,
                                                    FamAddr = p.Family.AddressLineOne,
                                                    PerAddr = p.AddressLineOne,
                                                    Member = p.MemberStatus.Description
                                                };
                var list = q.ToList();
                return list;
            }
        }
        public class Duplicate
        {
            public bool s0 { get; set; }
            public bool s1 { get; set; }
            public bool s2 { get; set; }
            public bool s3 { get; set; }
            public bool s4 { get; set; }
            public bool s5 { get; set; }
            public bool s6 { get; set; }
            public int PeopleId { get; set; }
            public string First { get; set; }
            public string Last { get; set; }
            public string Nick { get; set; }
            public string Middle { get; set; }
            public string Maiden { get; set; }
            public int? BMon { get; set; }
            public int? BDay { get; set; }
            public int? BYear { get; set; }
            public string Email { get; set; }
            public string FamAddr { get; set; }
            public string PerAddr { get; set; }
            public string Member { get; set; }
        }
        public List<Duplicate> PossibleDuplicates2()
        {
            using (var ctx = new CMSDataContext(Util.ConnectionString))
            {
                ctx.SetNoLock();
                string street = GetStreet(ctx) ?? "--";
                var nick = NickName ?? "--";
                var maid = MaidenName ?? "--";
                var em = EmailAddress ?? "--";
                if (!em.HasValue())
                    em = "--";
                var bd = BirthDay ?? -1;
                var bm = BirthMonth ?? -1;
                var byr = BirthYear ?? -1;
                var q = from p in ctx.People
                        let firstmatch = p.FirstName == FirstName || (p.NickName ?? "") == FirstName || (p.MiddleName ?? "") == FirstName
                                    || p.FirstName == nick || (p.NickName ?? "") == nick || (p.MiddleName ?? "") == nick
                        let lastmatch = p.LastName == LastName || (p.MaidenName ?? "") == LastName
                                    || (p.MaidenName ?? "") == maid || p.LastName == maid
                        let nobday = (p.BirthMonth == null && p.BirthYear == null && p.BirthDay == null)
                                    || (BirthMonth == null && BirthYear == null && BirthDay == null)
                        let bdmatch = (p.BirthDay ?? -2) == bd && (p.BirthMonth ?? -2) == bm && (p.BirthYear ?? -2) == byr
                        let bdmatchpart = (p.BirthDay ?? -2) == bd && (p.BirthMonth ?? -2) == bm
                        let emailmatch = p.EmailAddress != null && p.EmailAddress == em
                        let addrmatch = (p.AddressLineOne ?? "").Contains(street) || (p.Family.AddressLineOne ?? "").Contains(street)
                        let s1 = firstmatch && bdmatchpart
                        let s2 = firstmatch && bdmatch
                        let s3 = firstmatch && lastmatch && nobday
                        let s4 = firstmatch && addrmatch
                        let s5 = firstmatch && emailmatch
                        let s6 = lastmatch && bdmatch
                        where p.FamilyId != FamilyId && (s1 || s2 || s3 || s4 || s5 || s6)
                        select new Duplicate
                        {
                            s1 = s1,
                            s2 = s2,
                            s3 = s3,
                            s4 = s4,
                            s5 = s5,
                            s6 = s6,
                            PeopleId = p.PeopleId,
                            First = p.FirstName,
                            Last = p.LastName,
                            Nick = p.NickName,
                            Middle = p.MiddleName,
                            BMon = p.BirthMonth,
                            BDay = p.BirthDay,
                            BYear = p.BirthYear,
                            Email = p.EmailAddress,
                            FamAddr = p.Family.AddressLineOne,
                            PerAddr = p.AddressLineOne,
                            Member = p.MemberStatus.Description
                        };
                try
                {
                    var list = q.ToList();
                    var t = new Duplicate
                    {
                        s0 = true,
                        PeopleId = PeopleId,
                        First = FirstName,
                        Last = LastName,
                        Nick = NickName,
                        Middle = MiddleName,
                        BMon = BirthMonth,
                        BDay = BirthDay,
                        BYear = BirthYear,
                        Email = EmailAddress,
                        FamAddr = Family.AddressLineOne,
                        PerAddr = AddressLineOne,
                        Member = MemberStatus.Description
                    };
                    list.Insert(0, t);

                    return list;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        private string GetStreet(CMSDataContext db)
        {
            if (!PrimaryAddress.HasValue())
                return null;
            try
            {
                var s = PrimaryAddress.Replace(".", "");
                var a = s.SplitStr(" ");
                var la = a.ToList();
                if (la[0].AllDigits())
                    la.RemoveAt(0);
                var quadrants = new string[] { "N", "NORTH", "S", "SOUTH", "E", "EAST", "W", "WEST", "NE", "NORTHEAST", "NW", "NORTHWEST", "SE", "SOUTHEAST", "SW", "SOUTHWEST" };
                if (quadrants.Contains(a[0].ToUpper()))
                    la.RemoveAt(0);
                la.Reverse();
                if (la[0].AllDigits())
                    la.RemoveAt(0);
                if (la[0].StartsWith("#"))
                    la.RemoveAt(0);
                var apt = new string[] { "APARTMENT", "APT", "BUILDING", "BLDG", "DEPARTMENT", "DEPT", "FLOOR", "FL", "HANGAR", "HNGR", "LOT", "LOT", "PIER", "PIER", "ROOM", "RM", "SLIP", "SLIP", "SPACE", "SPC", "STOP", "STOP", "SUITE", "STE", "TRAILER", "TRLR", "UNIT", "UNIT", "UPPER", "UPPR",
        	                    "BASEMENT","BSMT", "FRONT","FRNT", "LOBBY","LBBY", "LOWER","LOWR", "OFFICE","OFC", "PENTHOUSE","PH", "REAR", "SIDE" };
                if (apt.Contains(la[0].ToUpper()))
                    la.RemoveAt(0);
                if (db.StreetTypes.Any(t => t.Type == la[0]))
                    la.RemoveAt(0);
                la.Reverse();
                var street = string.Join(" ", la);
                return street;
            }
            catch (Exception)
            {
                return null;
            }
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
            return Util.EncryptForUrl("{0}|{1}".Fmt(PeopleId, FromEmail));
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
        //internal static int FindResCode(string zipcode)
        //{
        //    if (zipcode.HasValue() && zipcode.Length >= 5)
        //    {
        //        var z5 = zipcode.Substring(0, 5);
        //        var z = DbUtil.Db.Zips.SingleOrDefault(zip => z5 == zip.ZipCode);
        //        if (z == null)
        //            return 30;
        //        return z.MetroMarginalCode ?? 30;
        //    }
        //    return 30;
        //}
        private bool? _CanUserEditAll;
        public bool CanUserEditAll
        {
            get
            {
                if (!_CanUserEditAll.HasValue)
                    _CanUserEditAll = HttpContext.Current.User.IsInRole("Edit");
                return _CanUserEditAll.Value;
            }
        }
        private bool? _CanUserEditFamilyAddress;
        public bool CanUserEditFamilyAddress
        {
            get
            {
                if (!_CanUserEditFamilyAddress.HasValue)
                    _CanUserEditFamilyAddress = CanUserEditAll
                        || Util.UserPeopleId == Family.HeadOfHouseholdId
                        || Util.UserPeopleId == Family.HeadOfHouseholdSpouseId;
                return _CanUserEditFamilyAddress.Value;
            }
        }
        private bool? _CanUserEditBasic;
        public bool CanUserEditBasic
        {
            get
            {
                if (!_CanUserEditBasic.HasValue)
                    _CanUserEditBasic = CanUserEditFamilyAddress
                        || Util.UserPeopleId == PeopleId;
                return _CanUserEditBasic.Value;
            }
        }
        private bool? _CanUserSee;
        public bool CanUserSee
        {
            get
            {
                if (!_CanUserSee.HasValue)
                    _CanUserSee = CanUserEditBasic
                        || Family.People.Any(m => m.PeopleId == Util.UserPeopleId);
                return _CanUserSee.Value;
            }
        }

        //partial void OnZipCodeChanged()
        //{
        //    ResCodeId = FindResCode(Db, ZipCode);
        //}
        //partial void OnAltZipCodeChanged()
        //{
        //    AltResCodeId = FindResCode(Db, AltZipCode);
        //}
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