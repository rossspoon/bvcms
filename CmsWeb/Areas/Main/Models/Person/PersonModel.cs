using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using System.Web.Mvc;
using UtilityExtensions;
using CMSPresenter;

namespace CMSWeb.Models
{
    public class PersonModel
    {
        public class PersonInfo : BasicPersonInfo
        {
            private CodeValueController cv = new CodeValueController();
            public int FamilyId { get; set; }
            public int? SpouseId { get; set; }

            public int? BaptismStatusId { get; set; }
            public int? DecisionTypeId { get; set; }
            public int? JoinTypeId { get; set; }
            public int? BaptismTypeId { get; set; }
            public int? LetterStatusId { get; set; }
            public int? InterestPointId { get; set; }
            public int? OriginId { get; set; }
            public int? EntryPointId { get; set; }
            public int? DropTypeId { get; set; }
            public int? NewMemberClassStatusId { get; set; }
            public int? StatementOptionId { get; set; }
            public int? EnvelopeOptionId { get; set; }
            public int AddressTypeId { get; set; }

            public string BaptismStatus
            {
                get { return cv.BaptismStatuses().ItemValue(BaptismStatusId); }
                
            }
            public string DecisionType
            {
                get { return cv.DecisionCodes().ItemValue(DecisionTypeId); }
                
            }
            public string JoinType
            {
                get { return cv.JoinTypes().ItemValue(JoinTypeId); }
                
            }
            public string BaptismType
            {
                get { return cv.BaptismTypes().ItemValue(BaptismTypeId); }
                
            }
            public string LetterStatus
            {
                get { return cv.LetterStatusCodes().ItemValue(LetterStatusId); }
                
            }
            public string InterestPoint
            {
                get { return cv.InterestPoints().ItemValue(InterestPointId); }
                
            }
            public string Origin
            {
                get { return cv.Origins().ItemValue(OriginId); }
                
            }
            public string EntryPoint
            {
                get { return cv.EntryPoints().ItemValue(EntryPointId); }
                
            }
            public string DropType
            {
                get { return cv.DropTypes().ItemValue(DropTypeId); }
                
            }
            public string NewMemberClassStatus
            {
                get { return cv.DiscoveryClassStatusCodes().ItemValue(NewMemberClassStatusId); }
                
            }
            public string StatementOption
            {
                get { return cv.EnvelopeOptions().ItemValue(StatementOptionId); }
                
            }
            public string EnvelopeOption
            {
                get { return cv.EnvelopeOptions().ItemValue(EnvelopeOptionId); }
            }
            //public string AddressType
            //{
            //    get { return cv.AddressTypeCodes().ItemValue(AddressTypeId); }
            //}

            public string BaptismDate { get; set; }
            public bool Deceased { get; set; }
            public string DecisionDate { get; set; }
            public string BaptismSchedDate { get; set; }
            public string PrevChurch { get; set; }
            public string DropDate { get; set; }
            public string NewChurch { get; set; }
            public string NewMemberClassDate { get; set; }
            public string Name { get; set; }
            public int? SmallPicId { get; set; }
            public string LetterRequested { get; set; }
            public string LetterReceived { get; set; }
            public string LetterNotes { get; set; }
            public bool MemberAnyChurch { get; set; }
            public bool ChristAsSavior { get; set; }
            public bool PleaseVisit { get; set; }
            public bool InterestedInJoining { get; set; }
            public bool SendInfo { get; set; }
            public string Comments { get; set; }
            private AddressInfo _PrimaryAddr;
            public AddressInfo PrimaryAddr
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
            public AddressInfo FamilyAddr { get; set; }
            public AddressInfo AltFamilyAddr { get; set; }
            public AddressInfo PersonalAddr { get; set; }
            public AddressInfo AltPersonalAddr { get; set; }
            public PersonContactsReceivedModel contacts;
            public PersonContactsMadeModel contactsmade;
            public IEnumerable<TaskModel.TasksAbout> tasks;
        }
        public PersonInfo displayperson;
        public PersonModel(int? id)
        {
            var q = from p in DbUtil.Db.People
                    where p.PeopleId == id
                    select new PersonInfo
                    {
                        AddressTypeId = p.AddressTypeId,
                        Age = p.Age.ToString(),
                        BaptismSchedDate = p.BaptismSchedDate.FormatDate(),
                        BaptismTypeId = p.BaptismTypeId,
                        BaptismStatusId = p.BaptismStatusId,
                        BaptismDate = p.BaptismDate.FormatDate(),
                        Birthday = p.DOB,
                        CampusId = p.CampusId,
                        CellPhone = p.CellPhone.FmtFone(),
                        Deceased = p.Deceased,
                        DeceasedDate = p.DeceasedDate,
                        DecisionDate = p.DecisionDate.FormatDate(),
                        DecisionTypeId = p.DecisionType.Id,
                        DoNotCallFlag = p.DoNotCallFlag,
                        DoNotMailFlag = p.DoNotMailFlag,
                        DoNotVisitFlag = p.DoNotVisitFlag,
                        DropDate = p.DropDate.FormatDate(),
                        DropTypeId = p.DropType.Id,
                        EmailAddress = p.EmailAddress,
                        Employer = p.EmployerOther,
                        EnvelopeOptionId = p.EnvelopeOptionsId,
                        FamilyId = p.FamilyId,
                        First = p.FirstName,
                        GenderId = p.GenderId,
                        Grade = p.Grade.ToString(),
                        HomePhone = p.Family.HomePhone,
                        JoinDate = p.JoinDate,
                        JoinTypeId = p.JoinCodeId,
                        Last = p.LastName,
                        Maiden = p.MaidenName,
                        MaritalStatusId = p.MaritalStatusId,
                        MemberStatusId = p.MemberStatusId,
                        Middle = p.MiddleName,
                        Name = p.Name,
                        NewChurch = p.OtherNewChurch,
                        NewMemberClassDate = p.DiscoveryClassDate.FormatDate(),
                        NewMemberClassStatusId = p.DiscoveryClassStatusId,
                        NickName = p.NickName,
                        Occupation = p.OccupationOther,
                        PeopleId = p.PeopleId,
                        SmallPicId = p.Picture.SmallId,
                        PrevChurch = p.OtherPreviousChurch,
                        School = p.SchoolOther,
                        Spouse = p.SpouseName,
                        SpouseId = p.SpouseId,
                        StatementOptionId = p.ContributionOptionsId,
                        Suffix = p.SuffixCode,
                        Title = p.TitleCode,
                        WeddingDate = p.WeddingDate,
                        WorkPhone = p.WorkPhone,
                        LetterStatusId = p.LetterStatusId,
                        LetterReceived = p.LetterDateReceived.FormatDate(),
                        LetterRequested = p.LetterDateRequested.FormatDate(),
                        LetterNotes = p.LetterStatusNotes,
                        InterestPointId = p.InterestPointId,
                        OriginId = p.OriginId,
                        EntryPointId = p.EntryPointId,
                        ChristAsSavior = p.ChristAsSavior,
                        Comments = p.Comments,
                        InterestedInJoining = p.InterestedInJoining,
                        MemberAnyChurch = p.MemberAnyChurch ?? false,
                        PleaseVisit = p.PleaseVisit,
                        SendInfo = p.InfoBecomeAChristian,
                        FamilyAddr = new AddressInfo
                        {
                            Name = "FamilyAddr",
                            PeopleId = p.PeopleId,
                            Address1 = p.Family.AddressLineOne,
                            Address2 = p.Family.AddressLineTwo,
                            City = p.Family.CityName,
                            State = p.Family.StateCode,
                            Zip = p.Family.ZipCode,
                            BadAddress = p.Family.BadAddressFlag,
                            ResCodeId = p.Family.ResCodeId,
                            FromDt = p.Family.AddressFromDate,
                            ToDt = p.Family.AddressToDate,
                            Preferred = p.AddressTypeId == 10,
                        },
                        AltFamilyAddr = new AddressInfo
                        {
                            Name = "AltFamilyAddr",
                            PeopleId = p.PeopleId,
                            Address1 = p.Family.AltAddressLineOne,
                            Address2 = p.Family.AltAddressLineTwo,
                            City = p.Family.AltCityName,
                            State = p.Family.AltStateCode,
                            Zip = p.Family.AltZipCode,
                            BadAddress = p.Family.AltBadAddressFlag,
                            ResCodeId = p.Family.AltResCodeId,
                            FromDt = p.Family.AltAddressFromDate,
                            ToDt = p.Family.AltAddressToDate,
                            Preferred = p.AddressTypeId == 20,
                        },
                        PersonalAddr = new AddressInfo
                        {
                            Name = "PersonalAddr",
                            PeopleId = p.PeopleId,
                            Address1 = p.AddressLineOne,
                            Address2 = p.AddressLineTwo,
                            City = p.CityName,
                            State = p.StateCode,
                            Zip = p.ZipCode,
                            BadAddress = p.BadAddressFlag,
                            ResCodeId = p.ResCodeId,
                            FromDt = p.AddressFromDate,
                            ToDt = p.AddressToDate,
                            Preferred = p.AddressTypeId == 30,
                        },
                        AltPersonalAddr = new AddressInfo
                        {
                            Name = "AltPersonalAddr",
                            PeopleId = p.PeopleId,
                            Address1 = p.AltAddressLineOne,
                            Address2 = p.AltAddressLineTwo,
                            City = p.AltCityName,
                            State = p.AltStateCode,
                            Zip = p.AltZipCode,
                            BadAddress = p.AltBadAddressFlag,
                            ResCodeId = p.AltResCodeId,
                            FromDt = p.AltAddressFromDate,
                            ToDt = p.AltAddressToDate,
                            Preferred = p.AddressTypeId == 40,
                        },
                    };
            displayperson = q.Single();
            displayperson.contacts = new PersonContactsReceivedModel(id.Value);
            displayperson.contactsmade = new PersonContactsMadeModel(id.Value);
            var m = new TaskModel();
            displayperson.tasks = m.TasksAboutList(id.Value);
            vol = DbUtil.Db.Volunteers.FirstOrDefault(v => v.PeopleId == id.Value);
            if (vol == null)
                vol = new Volunteer();
        }
        private Person _person;
        public Person Person
        {
            get
            {
                if (_person == null)
                    _person = DbUtil.Db.LoadPersonById(displayperson.PeopleId);
                return _person;
            }
        }

        public Volunteer vol;
        public string Name
        {
            get { return displayperson.Name; }
            set { displayperson.Name = value; }
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
        public int? recregid { get; set; }
        public bool HasRecReg
        {
            get
            {
                if (!recregid.HasValue)
                    if (HttpContext.Current.User.IsInRole("Attendance"))
                    {
                        var q = from rr in DbUtil.Db.RecRegs
                                where rr.PeopleId == displayperson.PeopleId
                                orderby rr.Uploaded descending
                                select rr.Id;
                        recregid = q.FirstOrDefault();
                    }
                return recregid.HasValue && recregid > 0;
            }
        }
        public int? ckorg;
        public bool CanCheckIn
        {
            get
            {
                ckorg = (int?)HttpContext.Current.Session["CheckInOrgId"];
                return ckorg.HasValue;
            }
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
                    where m.FamilyId == displayperson.FamilyId
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
                        SpouseIndicator = m.PeopleId == displayperson.SpouseId ? "*" : "&nbsp;"
                    };
            return q;
        }
        public string addrtab { get { return displayperson.PrimaryAddr.Name; } }
        public static IEnumerable<SelectListItem> GenderCodes()
        {
            var q = from i in DbUtil.Db.Genders
                    orderby i.Id
                    select new SelectListItem
                    {
                        Value = i.Id.ToString(),
                        Text = i.Description
                    };
            return q;
        }
        public static IEnumerable<SelectListItem> Campuses()
        {
            var q = from i in DbUtil.Db.Campus
                    orderby i.Id
                    select new SelectListItem
                    {
                        Value = i.Id.ToString(),
                        Text = i.Description
                    };
            return q;
        }
        public static IEnumerable<SelectListItem> MemberStatuses()
        {
            var q = from i in DbUtil.Db.MemberStatuses
                    orderby i.Id
                    select new SelectListItem
                    {
                        Value = i.Id.ToString(),
                        Text = i.Description
                    };
            return q;
        }
        public static IEnumerable<SelectListItem> MaritalStatuses()
        {
            var q = from i in DbUtil.Db.MaritalStatuses
                    orderby i.Id
                    select new SelectListItem
                    {
                        Value = i.Id.ToString(),
                        Text = i.Description
                    };
            return q;
        }
    }
}
