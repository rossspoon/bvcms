using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using System.Web.Mvc;
using UtilityExtensions;

namespace CMSWeb.Models
{
    public class PersonModel
    {
        public class PersonInfo
        {
            public string Address { get; set; }
            public string Address2 { get; set; }
            public string EmailAddress { get; set; }
            public string HomePhone { get; set; }
            public int PeopleId { get; set; }
            public int FamilyId { get; set; }
            public int? SpouseId { get; set; }
            public string NickName { get; set; }
            public string Title { get; set; }
            public string First { get; set; }
            public string Middle { get; set; }
            public string Last { get; set; }
            public string Suffix { get; set; }
            public string Maiden { get; set; }
            public string Gender { get; set; }
            public string CellPhone { get; set; }
            public string WorkPhone { get; set; }
            public string School { get; set; }
            public string Grade { get; set; }
            public string Employer { get; set; }
            public string Occupation { get; set; }
            public string Campus { get; set; }
            public string JoinDate { get; set; }
            public string BaptismStatus { get; set; }
            public string BaptismDate { get; set; }
            public string MaritalStatus { get; set; }
            public string Spouse { get; set; }
            public string WeddingDate { get; set; }
            public string Birthday { get; set; }
            public bool Deceased { get; set; }
            public string DeceasedDate { get; set; }
            public string Age { get; set; }
            public string DoNotCall { get; set; }
            public string DoNotVisit { get; set; }
            public string DoNotMail { get; set; }
            public string StatementOption { get; set; }
            public string EnvelopeOption { get; set; }
            public string AddressType { get; set; }
            public string DecisionType { get; set; }
            public string DecisionDate { get; set; }
            public string JoinType { get; set; }
            public string BaptismType { get; set; }
            public string BaptismSchedDate { get; set; }
            public string PrevChurch { get; set; }
            public string MemberStatus { get; set; }
            public string DropType { get; set; }
            public string DropDate { get; set; }
            public string NewChurch { get; set; }
            public string NewMemberClassStatus { get; set; }
            public string NewMemberClassDate { get; set; }
            public string Name { get; set; }
            public int? SmallPicId { get; set; }
            public string LetterStatus { get; set; }
            public string LetterRequested { get; set; }
            public string LetterReceived { get; set; }
            public string LetterNotes { get; set; }
            public string InterestPoint { get; set; }
            public string Origin { get; set; }
            public string EntryPoint { get; set; }
            public bool MemberAnyChurch { get; set; }
            public bool ChristAsSavior { get; set; }
            public bool PleaseVisit { get; set; }
            public bool InterestedInJoining { get; set; }
            public bool SendInfo { get; set; }
            public string Comments { get; set; }
            private Address _PrimaryAddr;
            public Address PrimaryAddr
            {
                get
                {
                    if (_PrimaryAddr == null)
                        if (FamilyAddr.Preferred)
                            _PrimaryAddr = FamilyAddr;
                        else if (PersonalAddr.Preferred)
                            _PrimaryAddr = PersonalAddr;
                        else if (AltFamilyAddr.Preferred)
                            _PrimaryAddr = AltFamilyAddr;
                        else if (AltPersonalAddr.Preferred)
                            _PrimaryAddr = AltPersonalAddr;
                    return _PrimaryAddr;
                }
            }
            public Address FamilyAddr { get; set; }
            public Address AltFamilyAddr { get; set; }
            public Address PersonalAddr { get; set; }
            public Address AltPersonalAddr { get; set; }
            public PersonContactsReceivedModel contacts;
            public PersonContactsMadeModel contactsmade;
            public IEnumerable<TaskModel.TasksAbout> tasks;
        }
        public class Address
        {
            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string Zip { get; set; }
            public string CityStateZip()
            {
                return Util.FormatCSZ4(City, State, Zip);
            }
            public string AddrCityStateZip()
            {
                return Address1 + ";" + CityStateZip();
            }
            public string Addr2CityStateZip()
            {
                return Address2 + ";" + CityStateZip();
            }
            public bool? BadAddress { get; set; }
            public string ResCode { get; set; }
            public bool Preferred { get; set; }
            public DateTime? FromDt { get; set; }
            public DateTime? ToDt { get; set; }
            public string Name { get; set; }
        }
        public PersonInfo person;
        public PersonModel(int? id)
        {
            var q = from p in DbUtil.Db.People
                    where p.PeopleId == id
                    select new PersonInfo
                    {
                        AddressType = p.AddressTypeId.ToString(),
                        Address = p.PrimaryAddress,
                        Address2 = p.PrimaryAddress2,
                        Age = p.Age.ToString(),
                        BaptismSchedDate = p.BaptismSchedDate.FormatDate(),
                        BaptismType = p.BaptismType.Description,
                        BaptismStatus = p.BaptismStatus.Description,
                        BaptismDate = p.BaptismDate.FormatDate(),
                        Birthday = p.DOB,
                        Campus = p.Campu.Description,
                        CellPhone = p.CellPhone.FmtFone(),
                        Deceased = p.Deceased,
                        DeceasedDate = p.DeceasedDate.FormatDate(),
                        DecisionDate = p.DecisionDate.FormatDate(),
                        DecisionType = p.DecisionType.Description,
                        DoNotCall = p.DoNotCallFlag ? "Do Not Call" : "",
                        DoNotMail = p.DoNotMailFlag ? "Do Not Mail" : "",
                        DoNotVisit = p.DoNotVisitFlag ? "Do Not Visit" : "",
                        DropDate = p.DropDate.FormatDate(),
                        DropType = p.DropType.Description,
                        EmailAddress = p.EmailAddress,
                        Employer = p.EmployerOther,
                        EnvelopeOption = p.EnvelopeOption.Description,
                        FamilyId = p.FamilyId,
                        First = p.FirstName,
                        Gender = p.Gender.Description,
                        Grade = p.Grade.ToString(),
                        HomePhone = p.Family.HomePhone.FmtFone(),
                        JoinDate = p.JoinDate.FormatDate(),
                        JoinType = p.JoinType.Description,
                        Last = p.LastName,
                        Maiden = p.MaidenName,
                        MaritalStatus = p.MaritalStatus.Description,
                        MemberStatus = p.MemberStatus.Description,
                        Middle = p.MiddleName,
                        Name = p.Name,
                        NewChurch = p.OtherNewChurch,
                        NewMemberClassDate = p.DiscoveryClassDate.FormatDate(),
                        NewMemberClassStatus = p.DiscoveryClassStatus.Description,
                        NickName = p.NickName,
                        Occupation = p.OccupationOther,
                        PeopleId = p.PeopleId,
                        SmallPicId = p.Picture.SmallId,
                        PrevChurch = p.OtherPreviousChurch,
                        School = p.SchoolOther,
                        Spouse = p.SpouseName,
                        SpouseId = p.SpouseId,
                        StatementOption = p.ContributionStatementOption.Description,
                        Suffix = p.SuffixCode,
                        Title = p.TitleCode,
                        WeddingDate = p.WeddingDate.FormatDate(),
                        WorkPhone = p.WorkPhone.FmtFone(),
                        LetterStatus = p.MemberLetterStatus.Description,
                        LetterReceived = p.LetterDateReceived.FormatDate(),
                        LetterRequested = p.LetterDateRequested.FormatDate(),
                        LetterNotes = p.LetterStatusNotes,
                        InterestPoint = p.InterestPoint.Description,
                        Origin = p.Origin.Description,
                        EntryPoint = p.EntryPoint.Description,
                        ChristAsSavior = p.ChristAsSavior,
                        Comments = p.Comments,
                        InterestedInJoining = p.InterestedInJoining,
                        MemberAnyChurch = p.MemberAnyChurch ?? false,
                        PleaseVisit = p.PleaseVisit,
                        SendInfo = p.InfoBecomeAChristian,
                        FamilyAddr = new Address
                        {
                            Address1 = p.Family.AddressLineOne,
                            Address2 = p.Family.AddressLineTwo,
                            City = p.Family.CityName,
                            State = p.Family.StateCode,
                            Zip = p.Family.ZipCode,
                            BadAddress = p.Family.BadAddressFlag,
                            ResCode = p.Family.ResidentCode.Description,
                            FromDt = p.Family.AddressFromDate,
                            ToDt = p.Family.AddressToDate,
                            Preferred = p.AddressTypeId == 10,
                            Name = "FamilyAddr",
                        },
                        AltFamilyAddr = new Address
                        {
                            Address1 = p.Family.AltAddressLineOne,
                            Address2 = p.Family.AltAddressLineTwo,
                            City = p.Family.AltCityName,
                            State = p.Family.AltStateCode,
                            Zip = p.Family.AltZipCode,
                            BadAddress = p.Family.AltBadAddressFlag,
                            ResCode = p.Family.ResidentCode.Description,
                            FromDt = p.Family.AddressFromDate,
                            ToDt = p.Family.AddressToDate,
                            Preferred = p.AddressTypeId == 20,
                            Name = "AltFamilyAddr",
                        },
                        PersonalAddr = new Address
                        {
                            Address1 = p.AddressLineOne,
                            Address2 = p.AddressLineTwo,
                            City = p.CityName,
                            State = p.StateCode,
                            Zip = p.ZipCode,
                            BadAddress = p.BadAddressFlag,
                            ResCode = p.ResidentCode.Description,
                            FromDt = p.AddressFromDate,
                            ToDt = p.AddressToDate,
                            Preferred = p.AddressTypeId == 30,
                            Name = "PersonalAddr",
                        },
                        AltPersonalAddr = new Address
                        {
                            Address1 = p.AltAddressLineOne,
                            Address2 = p.AltAddressLineTwo,
                            City = p.AltCityName,
                            State = p.AltStateCode,
                            Zip = p.AltZipCode,
                            BadAddress = p.AltBadAddressFlag,
                            ResCode = p.AltResidentCode.Description,
                            FromDt = p.AltAddressFromDate,
                            ToDt = p.AltAddressToDate,
                            Preferred = p.AddressTypeId == 40,
                            Name = "AltPersonalAddr",
                        },
                    };
            person = q.Single();
            enrollments = new PersonEnrollmentsModel(id.Value);
            prevEnrollments = new PersonPrevEnrollmentsModel(id.Value);
            pendingEnrollments = new PersonPendingEnrollmentsModel(id.Value);
            attendances = new PersonAttendHistoryModel(id.Value);
            person.contacts = new PersonContactsReceivedModel(id.Value);
            person.contactsmade = new PersonContactsMadeModel(id.Value);
            var m = new TaskModel();
            person.tasks = m.TasksAboutList(id.Value);
            vol = DbUtil.Db.Volunteers.FirstOrDefault(v => v.PeopleId == id.Value);
            if (vol == null)
                vol = new Volunteer();
        }
        public Volunteer vol;
        public PersonEnrollmentsModel enrollments;
        public PersonPrevEnrollmentsModel prevEnrollments;
        public PersonPendingEnrollmentsModel pendingEnrollments;
        public PersonAttendHistoryModel attendances;
        public string Name
        {
            get { return person.Name; }
            set { person.Name = value; }
        }
        public IEnumerable<SelectListItem> PreferredAddresses()
        {
            var q = from a in DbUtil.Db.AddressTypes
                    select new SelectListItem
                    {
                        Text = a.Description,
                        Value = a.Id.ToString(),
                    };
            return q;
        }
        private RecReg recreg;
        public bool HasRecReg
        {
            get
            {
                if (recreg == null)
                    if (HttpContext.Current.User.IsInRole("Attendance"))
                    {
                        var q = from rr in DbUtil.Db.RecRegs
                                where rr.PeopleId == person.PeopleId
                                orderby rr.Uploaded descending
                                select rr;
                        recreg = q.FirstOrDefault();
                    }
                return recreg != null;
            }
        }
        public string RecRegLink
        {
            get
            {
                if (recreg != null)
                    return "/Recreation/Detail/{0}".Fmt(recreg.Id);
                return "";
            }
        }
        private int? ckorg;
        public bool CanCheckIn
        {
            get
            {
                ckorg = (int?)HttpContext.Current.Session["CheckInOrgId"];
                return ckorg.HasValue;
            }
        }
        public string CheckInLink
        {
            get { return "/CheckIn/CheckIn/{0}?pid={1}".Fmt(ckorg, person.PeopleId); }
        }
        public bool IsFinance
        {
            get
            {
                return HttpContext.Current.User.IsInRole("Finance");
            }
        }
        public string ContributionsLink
        {
            get { return "/Contributions/Years.aspx?id={0}".Fmt(person.PeopleId); }
        }
        public bool IsAdmin
        {
            get { return HttpContext.Current.User.IsInRole("Admin"); }
        }
        public bool IsEdit
        {
            get { return HttpContext.Current.User.IsInRole("Edit"); }
        }
        public class FamilyMember
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int? Age { get; set; }
            public string Color { get; set; }
            public string PositionInFamily { get; set; }
            public string SpouseIndicator { get; set; }
        }
        public IEnumerable<FamilyMember> FamilyMembers()
        {
            var q = from m in DbUtil.Db.People
                    where m.FamilyId == person.FamilyId
                    orderby
                        m.PeopleId == m.Family.HeadOfHouseholdId ? 1 :
                        m.PeopleId == m.Family.HeadOfHouseholdSpouseId ? 2 :
                        3, m.Age descending, m.Name2
                    select new FamilyMember
                    {
                        Id = m.PeopleId,
                        Name = m.Name,
                        Age = m.Age,
                        Color = m.DeceasedDate != null ? "red" : "black",
                        PositionInFamily = m.FamilyPosition.Code,
                        SpouseIndicator = m.PeopleId == person.SpouseId ? "*" : "&nbsp;"
                    };
            return q;
        }
        public string addrtab { get { return person.PrimaryAddr.Name; } }
        public class AddressInfo
        {
            public string AddressLine1 { get; set; }
            public string AddressLine2 { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string Zip { get; set; }
            public bool? BadAddressFlag { get; set; }
            public string BadAddress
            {
                get { return (BadAddressFlag ?? false) ? "checked=\"checked\"" : ""; }
            }
            public int? ResCodeId { get; set; }
            public string ResCode
            {
                get
                {
                    if (ResCodeId.HasValue)
                        return ResCodes().Single(rc => rc.Value == ResCodeId.ToString()).Text;
                    return "(not specified)";
                }
            }
            public bool PreferredFlag { get; set; }
            public string Preferred
            {
                get { return PreferredFlag ? "checked=\"checked\"" : ""; }
            }
            public DateTime? FromDate { get; set; }
            public DateTime? ToDate { get; set; }
        }
        public static IEnumerable<SelectListItem> ResCodes()
        {
            var q = from rc in DbUtil.Db.ResidentCodes
                    orderby rc.Id
                    select new SelectListItem
                    {
                        Value = rc.Id.ToString(),
                        Text = rc.Description
                    };
            return q;
        }
    }
}
