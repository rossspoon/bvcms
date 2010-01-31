using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using System.Web.Mvc;
using UtilityExtensions;
using CMSPresenter;

namespace CMSWeb.Models.PersonPage
{
    public class PersonInfo
    {
        private CodeValueController cv = new CodeValueController();

        public BasicPersonInfo basic { get; set; }
        public MemberInfo member { get; set; }
        public GrowthInfo growth { get; set; }

        public int PeopleId { get; set; }
        public int FamilyId { get; set; }
        public int? SpouseId { get; set; }

        public int? LetterStatusId { get; set; }
        public string LetterStatus { get { return cv.LetterStatusCodes().ItemValue(LetterStatusId ?? 0); } }

        public bool Deceased { get; set; }
        public string Name { get; set; }
        public int? SmallPicId { get; set; }
        public string LetterRequested { get; set; }
        public string LetterReceived { get; set; }
        public string LetterNotes { get; set; }

        public int AddressTypeId { get; set; }
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
        public static PersonInfo GetPersonInfo(int? id)
        {
            var q = from p in DbUtil.Db.People
                    where p.PeopleId == id
                    select new PersonInfo
                    {
                        PeopleId = p.PeopleId,
                        AddressTypeId = p.AddressTypeId,

                        member = new MemberInfo
                        {
                            PeopleId = p.PeopleId,
                            BaptismSchedDate = p.BaptismSchedDate,
                            BaptismTypeId = p.BaptismTypeId,
                            BaptismStatusId = p.BaptismStatusId,
                            BaptismDate = p.BaptismDate,
                            DecisionDate = p.DecisionDate,
                            DecisionTypeId = p.DecisionType.Id,
                            DropDate = p.DropDate,
                            DropTypeId = p.DropType.Id,
                            EnvelopeOptionId = p.EnvelopeOptionsId,
                            StatementOptionId = p.ContributionOptionsId,
                            JoinTypeId = p.JoinCodeId,
                            NewChurch = p.OtherNewChurch,
                            PrevChurch = p.OtherPreviousChurch,
                            NewMemberClassDate = p.DiscoveryClassDate,
                            NewMemberClassStatusId = p.DiscoveryClassStatusId,
                            MemberStatusId = p.MemberStatusId,
                            JoinDate = p.JoinDate,
                        },
                        basic = new BasicPersonInfo
                        {
                            PeopleId = p.PeopleId,
                            Age = p.Age.ToString(),
                            Birthday = p.DOB,
                            CampusId = p.CampusId,
                            CellPhone = p.CellPhone.FmtFone(),
                            DeceasedDate = p.DeceasedDate,
                            DoNotCallFlag = p.DoNotCallFlag,
                            DoNotMailFlag = p.DoNotMailFlag,
                            DoNotVisitFlag = p.DoNotVisitFlag,
                            EmailAddress = p.EmailAddress,
                            Employer = p.EmployerOther,
                            First = p.FirstName,
                            GenderId = p.GenderId,
                            Grade = p.Grade.ToString(),
                            HomePhone = p.Family.HomePhone,
                            JoinDate = p.JoinDate,
                            Last = p.LastName,
                            Maiden = p.MaidenName,
                            MaritalStatusId = p.MaritalStatusId,
                            MemberStatusId = p.MemberStatusId,
                            Middle = p.MiddleName,
                            NickName = p.NickName,
                            Occupation = p.OccupationOther,
                            School = p.SchoolOther,
                            Spouse = p.SpouseName,
                            Suffix = p.SuffixCode,
                            Title = p.TitleCode,
                            WeddingDate = p.WeddingDate,
                            WorkPhone = p.WorkPhone,
                        },
                        growth = new GrowthInfo
                        {
                            PeopleId = p.PeopleId,
                            InterestPointId = p.InterestPointId,
                            OriginId = p.OriginId,
                            EntryPointId = p.EntryPointId,
                            ChristAsSavior = p.ChristAsSavior,
                            Comments = p.Comments,
                            InterestedInJoining = p.InterestedInJoining,
                            MemberAnyChurch = p.MemberAnyChurch,
                            PleaseVisit = p.PleaseVisit,
                            SendInfo = p.InfoBecomeAChristian,
                        },
                        Deceased = p.Deceased,
                        FamilyId = p.FamilyId,
                        Name = p.Name,
                        SmallPicId = p.Picture.SmallId,
                        SpouseId = p.SpouseId,
                        LetterStatusId = p.LetterStatusId,
                        LetterReceived = p.LetterDateReceived.FormatDate(),
                        LetterRequested = p.LetterDateRequested.FormatDate(),
                        LetterNotes = p.LetterStatusNotes,
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
            return q.Single();
        }
    }
}
