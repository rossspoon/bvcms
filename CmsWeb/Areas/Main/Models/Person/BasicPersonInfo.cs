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
    public class BasicPersonInfo
    {
        private CodeValueController cv = new CodeValueController();
        public int PeopleId { get; set; }
        public string NickName { get; set; }
        public string Title { get; set; }
        public string First { get; set; }
        public string Middle { get; set; }
        public string Last { get; set; }
        public string Suffix { get; set; }
        public string Maiden { get; set; }
        public int GenderId { get; set; }

        public string HomePhone { get; set; }
        public string CellPhone { get; set; }
        public string WorkPhone { get; set; }
        public string EmailAddress { get; set; }
        public string School { get; set; }
        public string Grade { get; set; }
        public string Employer { get; set; }
        public string Occupation { get; set; }
        public bool DoNotCallFlag { get; set; }
        public bool DoNotVisitFlag { get; set; }
        public bool DoNotMailFlag { get; set; }

        public int? CampusId { get; set; }
        public int MemberStatusId { get; set; }
        public DateTime? JoinDate { get; set; }
        public int MaritalStatusId { get; set; }
        public string Spouse { get; set; }
        public DateTime? WeddingDate { get; set; }
        public string Birthday { get; set; }
        public DateTime? DeceasedDate { get; set; }
        public string Age { get; set; }

        public string Campus
        {
            get { return cv.AllCampuses0().ItemValue(CampusId ?? 0); }

        }
        public string Gender
        {
            get { return cv.GenderCodes().ItemValue(GenderId); }
            
        }
        public string MaritalStatus
        {
            get { return cv.MaritalStatusCodes().ItemValue(MaritalStatusId); }
            
        }
        public string MemberStatus
        {
            get { return cv.MemberStatusCodes().ItemValue(MemberStatusId); }
            
        }
        public string DoNotCall
        {
            get { return DoNotCallFlag ? "Do Not Call" : ""; }
            
        }
        public string DoNotVisit
        {
            get { return DoNotVisitFlag ? "Do Not Visit" : ""; }
            
        }
        public string DoNotMail
        {
            get { return DoNotMailFlag ? "Do Not Mail" : ""; }
            
        }

        public static BasicPersonInfo GetBasicPersonInfo(int? id)
        {
            var q = from p in DbUtil.Db.People
                    where p.PeopleId == id
                    select new BasicPersonInfo
                    {
                        Age = p.Age.ToString(),
                        Birthday = p.DOB,
                        CampusId = p.CampusId ?? 0,
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
                        HomePhone = p.Family.HomePhone.FmtFone(),
                        JoinDate = p.JoinDate,
                        Last = p.LastName,
                        Maiden = p.MaidenName,
                        MaritalStatusId = p.MaritalStatusId,
                        MemberStatusId = p.MemberStatusId,
                        Middle = p.MiddleName,
                        NickName = p.NickName,
                        Occupation = p.OccupationOther,
                        PeopleId = p.PeopleId,
                        School = p.SchoolOther,
                        Spouse = p.SpouseName,
                        Suffix = p.SuffixCode,
                        Title = p.TitleCode,
                        WeddingDate = p.WeddingDate,
                        WorkPhone = p.WorkPhone.FmtFone(),
                    };
            return q.Single();
        }

        public void UpdatePerson()
        {
            if (CampusId == 0)
                CampusId = null;
            var p = DbUtil.Db.LoadPersonById(PeopleId);
            p.DOB = Birthday;
            p.CampusId = CampusId;
            p.DeceasedDate = DeceasedDate;
            p.DoNotCallFlag = DoNotCallFlag;
            p.DoNotMailFlag = DoNotMailFlag;
            p.DoNotVisitFlag = DoNotVisitFlag;
            p.EmailAddress = EmailAddress;
            p.FirstName = First;
            p.LastName = Last;
            p.GenderId = GenderId;
            p.Grade = Grade.ToInt2();
            p.CellPhone = CellPhone.GetDigits();
            p.Family.HomePhone = HomePhone.GetDigits();
            p.MaidenName = Maiden;
            p.MaritalStatusId = MaritalStatusId;
            p.MiddleName = Middle;
            p.NickName = NickName;
            p.OccupationOther = Occupation;
            p.SchoolOther = School;
            p.SuffixCode = Suffix;
            p.EmployerOther = Employer;
            p.TitleCode = Title;
            p.WeddingDate = WeddingDate;
            p.WorkPhone = WorkPhone.GetDigits();
            if (p.DeceasedDateChanged)
                p.MemberProfileAutomation();
            DbUtil.Db.SubmitChanges();
        }
        public static IEnumerable<SelectListItem> TitleCodes()
        {
            var cv = new CodeValueController();
            return QueryModel.ConvertToSelect(cv.TitleCodes(), "Id");
        }
        public static IEnumerable<SelectListItem> GenderCodes()
        {
            var cv = new CodeValueController();
            return QueryModel.ConvertToSelect(cv.GenderCodes(), "Id");
        }
        public static IEnumerable<SelectListItem> Campuses()
        {
            var cv = new CodeValueController();
            return QueryModel.ConvertToSelect(cv.AllCampuses0(), "Id");
        }
        public static IEnumerable<SelectListItem> MemberStatuses()
        {
            var cv = new CodeValueController();
            return QueryModel.ConvertToSelect(cv.MemberStatusCodes(), "Id");
        }
        public static IEnumerable<SelectListItem> MaritalStatuses()
        {
            var cv = new CodeValueController();
            return QueryModel.ConvertToSelect(cv.MaritalStatusCodes(), "Id");
        }
    }
}
