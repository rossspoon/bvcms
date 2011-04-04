using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using System.Web.Mvc;
using UtilityExtensions;
using CMSPresenter;

namespace CmsWeb.Models.PersonPage
{
    public class PersonInfo
    {
        public BasicPersonInfo basic { get; set; }
        public MemberInfo member { get; set; }
        public GrowthInfo growth { get; set; }
        public MemberNotesInfo membernotes { get; set; }
        public IEnumerable<FamilyMemberInfo> familymembers { get; set; }

        public int PeopleId { get; set; }
        public int FamilyId { get; set; }
        public int? SpouseId { get; set; }

        public bool Deceased { get; set; }
        public string Name { get; set; }
        public int? SmallPicId { get; set; }

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
                return _PrimaryAddr;
            }
        }
        public AddressInfo FamilyAddr { get; set; }
        public AddressInfo PersonalAddr { get; set; }
        public static PersonInfo GetPersonInfo(int? id)
        {
            var p = DbUtil.Db.LoadPersonById(id.Value);
            if (p == null)
                return null;
            var rr = p.RecRegs.SingleOrDefault();

            var pi = new PersonInfo
            {
                PeopleId = p.PeopleId,
                AddressTypeId = p.AddressTypeId,
                Deceased = p.Deceased,
                FamilyId = p.FamilyId,
                Name = p.Name,
                SmallPicId = p.PictureId.HasValue ? p.Picture.SmallId : null,
                SpouseId = p.SpouseId,

                member = new MemberInfo
                {
                    PeopleId = p.PeopleId,
                    BaptismSchedDate = p.BaptismSchedDate,
                    BaptismTypeId = p.BaptismTypeId ?? 0,
                    BaptismStatusId = p.BaptismStatusId ?? 0,
                    BaptismDate = p.BaptismDate,
                    DecisionDate = p.DecisionDate,
                    DecisionTypeId = p.DecisionTypeId ?? 0,
                    DropDate = p.DropDate,
                    DropTypeId = p.DropCodeId,
                    EnvelopeOptionId = p.EnvelopeOptionsId ?? 0,
                    StatementOptionId = p.ContributionOptionsId ?? 0,
                    JoinTypeId = p.JoinCodeId,
                    NewChurch = p.OtherNewChurch,
                    PrevChurch = p.OtherPreviousChurch,
                    NewMemberClassDate = p.DiscoveryClassDate,
                    NewMemberClassStatusId = p.DiscoveryClassStatusId ?? 0,
                    MemberStatusId = p.MemberStatusId,
                    JoinDate = p.JoinDate,
                },
                basic = new BasicPersonInfo
                {
                    PeopleId = p.PeopleId,
                    person = p,
                    Age = p.Age.ToString(),
                    Birthday = p.DOB,
                    CampusId = p.CampusId ?? 0,
                    CellPhone = p.CellPhone.FmtFone(),
                    DeceasedDate = p.DeceasedDate,
                    DoNotCallFlag = p.DoNotCallFlag,
                    DoNotMailFlag = p.DoNotMailFlag,
                    DoNotVisitFlag = p.DoNotVisitFlag,
                    EmailAddress = p.EmailAddress,
                    SendEmailAddress1 = p.SendEmailAddress1 ?? true,
                    EmailAddress2 = p.EmailAddress2,
                    SendEmailAddress2 = p.SendEmailAddress2 ?? false,
                    Employer = p.EmployerOther,
                    First = p.FirstName,
                    GenderId = p.GenderId,
                    Created = p.CreatedDate,
                    Grade = p.Grade.ToString(),
                    HomePhone = p.Family.HomePhone,
                    JoinDate = p.JoinDate,
                    Last = p.LastName,
                    AltName = p.AltName,
                    Maiden = p.MaidenName,
                    MaritalStatusId = p.MaritalStatusId,
                    MemberStatusId = p.MemberStatusId,
                    Middle = p.MiddleName,
                    NickName = p.NickName,
                    Occupation = p.OccupationOther,
                    School = p.SchoolOther,
                    Spouse = p.SpouseName(DbUtil.Db),
                    Suffix = p.SuffixCode,
                    Title = p.TitleCode,
                    WeddingDate = p.WeddingDate,
                    WorkPhone = p.WorkPhone,
                },
                growth = new GrowthInfo
                {
                    PeopleId = p.PeopleId,
                    InterestPointId = p.InterestPointId ?? 0,
                    OriginId = p.OriginId ?? 0,
                    EntryPointId = p.EntryPointId ?? 0,
                    ChristAsSavior = p.ChristAsSavior,
                    Comments = p.Comments,
                    InterestedInJoining = p.InterestedInJoining,
                    MemberAnyChurch = p.MemberAnyChurch,
                    PleaseVisit = p.PleaseVisit,
                    SendInfo = p.InfoBecomeAChristian,
                },
                membernotes = new MemberNotesInfo
                {
                    PeopleId = p.PeopleId,
                    LetterStatusId = p.LetterStatusId ?? 0,
                    LetterReceived = p.LetterDateReceived,
                    LetterRequested = p.LetterDateRequested,
                    LetterNotes = p.LetterStatusNotes,
                },
                FamilyAddr = new AddressInfo
                {
                    Name = "FamilyAddr",
                    PeopleId = p.PeopleId,
                    person = p,
                    Address1 = p.Family.AddressLineOne,
                    Address2 = p.Family.AddressLineTwo,
                    City = p.Family.CityName,
                    State = p.Family.StateCode,
                    Zip = p.Family.ZipCode,
                    BadAddress = p.Family.BadAddressFlag,
                    ResCodeId = p.Family.ResCodeId ?? 0,
                    FromDt = p.Family.AddressFromDate,
                    ToDt = p.Family.AddressToDate,
                    Preferred = p.AddressTypeId == 10,
                },
                PersonalAddr = new AddressInfo
                {
                    Name = "PersonalAddr",
                    PeopleId = p.PeopleId,
                    person = p,
                    Address1 = p.AddressLineOne,
                    Address2 = p.AddressLineTwo,
                    City = p.CityName,
                    State = p.StateCode,
                    Zip = p.ZipCode,
                    BadAddress = p.BadAddressFlag,
                    ResCodeId = p.ResCodeId ?? 0,
                    FromDt = p.AddressFromDate,
                    ToDt = p.AddressToDate,
                    Preferred = p.AddressTypeId == 30,
                },
                familymembers = from m in p.Family.People
                                orderby
                                    m.PeopleId == p.Family.HeadOfHouseholdId ? 1 :
                                    m.PeopleId == p.Family.HeadOfHouseholdSpouseId ? 2 :
                                    3, m.Age descending, m.Name2
                                select new FamilyMemberInfo
                                {
                                    Id = m.PeopleId,
                                    Name = m.Name,
                                    Age = m.Age,
                                    Color = m.DeceasedDate != null ? "red" : "auto",
                                    PositionInFamily = m.FamilyPosition.Code,
                                    SpouseIndicator = m.PeopleId == p.SpouseId ? "*" : "&nbsp;",
                                    Email = m.EmailAddress
                                }
            };
            return pi;
        }
    }
}
