using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using System.Web.Mvc;
using UtilityExtensions;
using CMSPresenter;
using System.Text;

namespace CmsWeb.Models.PersonPage
{
    public class BasicPersonInfo
    {
        private CodeValueController cv = new CodeValueController();
        public int PeopleId { get; set; }
        public Person person { get; set; }
        public string NickName { get; set; }
        public string Title { get; set; }
        public string First { get; set; }
        public string Middle { get; set; }
        public string Last { get; set; }
        public string AltName { get; set; }
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
            var p = DbUtil.Db.LoadPersonById(id.Value);
            var pi = new BasicPersonInfo
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
                AltName = p.AltName,
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
            pi.person = p;
            return pi;
        }

        public void UpdatePerson()
        {
            if (CampusId == 0)
                CampusId = null;
            var p = DbUtil.Db.LoadPersonById(PeopleId);
            UpdateValue(p, "DOB", Birthday);;
            UpdateValue(p, "CampusId", CampusId);
            UpdateValue(p, "DeceasedDate", DeceasedDate);
            UpdateValue(p, "DoNotCallFlag", DoNotCallFlag);
            UpdateValue(p, "DoNotMailFlag", DoNotMailFlag);
            UpdateValue(p, "DoNotVisitFlag", DoNotVisitFlag);
            UpdateValue(p, "EmailAddress", EmailAddress);
            UpdateValue(p, "FirstName", First);
            UpdateValue(p, "LastName", Last);
            UpdateValue(p, "AltName", AltName);
            UpdateValue(p, "GenderId", GenderId);
            UpdateValue(p, "Grade", Grade.ToInt2());
            UpdateValue(p, "CellPhone", CellPhone.GetDigits());
            UpdateValue(p.Family, "HomePhone", HomePhone.GetDigits());
            UpdateValue(p, "MaidenName", Maiden);
            UpdateValue(p, "MaritalStatusId", MaritalStatusId);
            UpdateValue(p, "MiddleName", Middle);
            UpdateValue(p, "NickName", NickName);
            UpdateValue(p, "OccupationOther", Occupation);
            UpdateValue(p, "SchoolOther", School);
            UpdateValue(p, "SuffixCode", Suffix);
            UpdateValue(p, "EmployerOther", Employer);
            UpdateValue(p, "TitleCode", Title);
            UpdateValue(p, "WeddingDate", WeddingDate);
            UpdateValue(p, "WorkPhone", WorkPhone.GetDigits());
            if (p.DeceasedDateChanged)
                p.MemberProfileAutomation();

            if (psb.Length > 0)
                p.PeopleExtras.Add(new PeopleExtra
                {
                    Field = "BasicPersonInfo",
                    Data = psb.ToString(),
                    TransactionTime = Util.Now
                });
            if (fsb.Length > 0)
                p.PeopleExtras.Add(new PeopleExtra
                {
                    Field = "HomePhone",
                    Data = fsb.ToString(),
                    TransactionTime = Util.Now
                });
            DbUtil.Db.SubmitChanges();
            if (!HttpContext.Current.User.IsInRole("Access"))
                if (psb.Length > 0 || fsb.Length > 0)
                {
                    var smtp = Util.Smtp();        
                    DbUtil.Email2(smtp, p.EmailAddress, DbUtil.NewPeopleEmailAddress,
                        "Basic People Info Changed",
                        "{0} changed the following information:\n{1}\n{2}"
                        .Fmt(psb.ToString(),fsb.ToString()));
                }
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
        private StringBuilder fsb = new StringBuilder();
        private void UpdateValue(Family f, string field, object value)
        {
            var o = Util.GetProperty(f, field);
            if (o == null && value == null)
                return;
            if (o != null && o.Equals(value))
                return;
            fsb.AppendFormat("{0}: {1} -> {2}\n", field, o, value ?? "(null)");
            Util.SetProperty(f, field, value);
        }
        private StringBuilder psb = new StringBuilder();
        private void UpdateValue(Person p, string field, object value)
        {
            var o = Util.GetProperty(p, field);
            if (o == null && value == null)
                return;
            if (o != null && o.Equals(value))
                return;
            psb.AppendFormat("{0}: {1} -> {2}\n", field, o, value ?? "(null)");
            Util.SetProperty(p, field, value);
        }
    }
}
