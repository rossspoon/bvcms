using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Routing;
using System.Linq;
using CmsData;
using System.Web.Mvc;
using CmsWeb.Code;
using CmsWeb.Models;
using UtilityExtensions;
using System.Text;

namespace CmsWeb.Areas.People.Models.Person
{
    public class EmailInfo
    {
        public EmailInfo(string address, bool send)
        {
            Address = address;
            Send = send;
        }
        public string Address { get; set; }
        public bool Send { get; set; }
    }
    public class CodeInfo
    {
        public CodeInfo(object value, IEnumerable<CodeValueItem> items, string valuefield = "Id")
        {
            Value = value.ToString();
            Items = new SelectList(items, valuefield, "Value", value);
        }
        public string Value { get; set; }
        public SelectList Items { get; set; }
        public override string ToString()
        {
            return Items.Single(ii => ii.Value == Value).Text;
        }
    }

    public class BasicPersonInfo
    {
        private RouteValueDictionary routeVals;
        public RouteValueDictionary RouteVals
        {
            get
            {
                if (routeVals == null)
                    routeVals = new RouteValueDictionary { { "Id", PeopleId }, { "Area", "People" } };
                return routeVals;
            }
        }

        public Dictionary<string, object> Classes(string classes)
        {
            var d = new Dictionary<string, object>();
            d.Add("class", classes);
            return d;
        }

        private CodeValueModel cv = new CodeValueModel();
        public int PeopleId { get; set; }
        public CmsData.Person person { get; set; }

        [UICode("Title", "half")]
        public CodeInfo Title { get; set; }

        [UIText("First Name", "half")]
        public string First { get; set; }

        [UIText("Goes By", "half")]
        public string NickName { get; set; }

        [UIText("Middle Name", "half")]
        public string Middle { get; set; }

        [UIText("Last Name", "half")]
        public string Last { get; set; }

        [UIText("Alt Name", "half")]
        public string AltName { get; set; }

        [UIText("Former Name", "half")]
        public string Maiden { get; set; }

        [UIText("Suffix", "em")]
        public string Suffix { get; set; }

        [UICode("Gender", "half")]
        public CodeInfo Gender { get; set; }

        [UIDate("Birthdate", "half")]
        public string Birthday { get; set; }

        [UICode("Marital", "half")]
        public CodeInfo MaritalStatus { get; set; }

        [UIDate("Wedding Date", "half")]
        public string WeddingDate { get; set; }

        [UIText("Occupation", "half")]
        public string Occupation { get; set; }

        [UIText("Employer", "half")]
        public string Employer { get; set; }

        [UIText("School", "half")]
        public string School { get; set; }

        [UIText("Grade", "half")]
        public string Grade { get; set; }

        [UIText("Do Not Contact", "em", "lg")]
        public string DoNotContactBy
        {
            get
            {
                var list = new List<string>();
                if (DoNotCallFlag)
                    list.Add("by Phone");
                if (DoNotMailFlag)
                    list.Add("by Mail");
                if (DoNotVisitFlag)
                    list.Add("in Person");
                return string.Join(", ", list);
            }
        }

        [UIEmail("Primary Email", "half halfsy")]
        public EmailInfo Email1 { get; set; }
        [UIEmail("Alt Email", "half halfsy")]
        public EmailInfo Email2 { get; set; }

        public string HomePhone { get; set; }

        [UIText("Mobile", "half")]
        public string CellPhone { get; set; }
        [UIText("Work", "half")]
        public string WorkPhone { get; set; }

        public DateTime? Created { get; set; }
        public bool DoNotCallFlag { get; set; }
        public bool DoNotVisitFlag { get; set; }
        public bool DoNotMailFlag { get; set; }
        public bool ReceiveSMS { get; set; }
        public int? CampusId { get; set; }
        public int MemberStatusId { get; set; }
        public DateTime? JoinDate { get; set; }
        public string Spouse { get; set; }
        public DateTime? DeceasedDate { get; set; }
        public string Age { get; set; }

        public bool DoNotPublishPhones { get; set; }



        public string Campus
        {
            get { return cv.AllCampuses0().ItemValue(CampusId ?? 0); }
        }

        public CodeInfo MemberStatus { get; set; }
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
        public bool DoNotContactAny
        {
            get { return DoNotCallFlag || DoNotCallFlag || DoNotVisitFlag; }
        }

        public static BasicPersonInfo GetBasicPersonInfo(int? id)
        {
            var cv = new CodeValueModel();
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
                DoNotPublishPhones = p.DoNotPublishPhones ?? false,
                Email1 = new EmailInfo(p.EmailAddress, p.SendEmailAddress1 ?? true),
                Email2 = new EmailInfo(p.EmailAddress2, p.SendEmailAddress2 ?? false),
                Employer = p.EmployerOther,
                First = p.FirstName,
                Created = p.CreatedDate,
                Grade = p.Grade.ToString(),
                HomePhone = p.Family.HomePhone.FmtFone(),
                JoinDate = p.JoinDate,
                Last = p.LastName,
                AltName = p.AltName,
                Maiden = p.MaidenName,
                Gender = new CodeInfo(p.GenderId, cv.GenderCodesWithUnspecified()),
                MaritalStatus = new CodeInfo(p.MaritalStatusId, cv.MaritalStatusCodes99()),
                MemberStatus = new CodeInfo(p.MemberStatusId, cv.MemberStatusCodes0()),
                Middle = p.MiddleName,
                NickName = p.NickName,
                Occupation = p.OccupationOther,
                PeopleId = p.PeopleId,
                School = p.SchoolOther,
                Spouse = p.SpouseName(DbUtil.Db),
                Suffix = p.SuffixCode,
                Title = new CodeInfo(p.TitleCode, cv.TitleCodes(), "Value"),
                WeddingDate = p.WeddingDate.FormatDate(),
                WorkPhone = p.WorkPhone.FmtFone(),
                ReceiveSMS = p.ReceiveSMS,
            };
            pi.person = p;
            return pi;
        }

        public void UpdatePerson()
        {
            if (CampusId == 0)
                CampusId = null;
            var p = DbUtil.Db.LoadPersonById(PeopleId);
            var psb = new StringBuilder();
            var fsb = new StringBuilder();
            p.UpdateValue(psb, "DOB", Birthday);
            p.UpdateValue(psb, "CampusId", CampusId);
            p.UpdateValue(psb, "DeceasedDate", DeceasedDate);
            p.UpdateValue(psb, "DoNotCallFlag", DoNotCallFlag);
            p.UpdateValue(psb, "DoNotMailFlag", DoNotMailFlag);
            p.UpdateValue(psb, "DoNotVisitFlag", DoNotVisitFlag);
            p.UpdateValue(psb, "DoNotPublishPhones", DoNotPublishPhones);
            p.UpdateValue(psb, "EmailAddress", Email1.Address);
            p.UpdateValue(psb, "EmailAddress2", Email2.Address);
            p.UpdateValue(psb, "SendEmailAddress1", Email1.Send);
            p.UpdateValue(psb, "SendEmailAddress2", Email2.Send);
            p.UpdateValue(psb, "FirstName", First);
            p.UpdateValue(psb, "LastName", Last);
            p.UpdateValue(psb, "AltName", AltName);
            p.UpdateValue(psb, "GenderId", Gender.Value.ToInt2());
            p.UpdateValue(psb, "Grade", Grade.ToInt2());
            p.UpdateValue(psb, "CellPhone", CellPhone.GetDigits());
            p.Family.UpdateValue(fsb, "HomePhone", HomePhone.GetDigits());
            p.UpdateValue(psb, "MaidenName", Maiden);
            p.UpdateValue(psb, "MaritalStatusId", MaritalStatus.Value.ToInt2());
            p.UpdateValue(psb, "MiddleName", Middle);
            p.UpdateValue(psb, "NickName", NickName);
            p.UpdateValue(psb, "OccupationOther", Occupation);
            p.UpdateValue(psb, "SchoolOther", School);
            p.UpdateValue(psb, "SuffixCode", Suffix);
            p.UpdateValue(psb, "EmployerOther", Employer);
            p.UpdateValue(psb, "TitleCode", Title.Value);
            p.UpdateValue(psb, "WeddingDate", WeddingDate.ToDate());
            p.UpdateValue(psb, "WorkPhone", WorkPhone.GetDigits());
            if (p.DeceasedDateChanged)
            {
                var ret = p.MemberProfileAutomation(DbUtil.Db);
                if (ret != "ok")
                    Elmah.ErrorSignal.FromCurrentContext().Raise(
                        new Exception(ret + " for PeopleId:" + p.PeopleId));
            }


            p.LogChanges(DbUtil.Db, psb, Util.UserPeopleId.Value);
            p.Family.LogChanges(DbUtil.Db, fsb, p.PeopleId, Util.UserPeopleId.Value);

            DbUtil.Db.SubmitChanges();

            if (!HttpContext.Current.User.IsInRole("Access"))
                if (psb.Length > 0 || fsb.Length > 0)
                {
                    DbUtil.Db.EmailRedacted(p.FromEmail, DbUtil.Db.GetNewPeopleManagers(),
                        "Basic Person Info Changed on " + Util.Host,
                        "{0} changed the following information for {1} ({2}):<br />\n<table>{3}{4}</table>"
                        .Fmt(Util.UserName, First + " " + Last, PeopleId, psb.ToString(), fsb.ToString()));
                }
        }
        public static IEnumerable<SelectListItem> GenderCodes()
        {
            var cv = new CodeValueModel();
            return QueryModel.ConvertToSelect(cv.GenderCodes(), "Id");
        }
        public static IEnumerable<SelectListItem> Campuses()
        {
            var cv = new CodeValueModel();
            return QueryModel.ConvertToSelect(cv.AllCampuses0(), "Id");
        }
        public static IEnumerable<SelectListItem> MemberStatuses()
        {
            var cv = new CodeValueModel();
            return QueryModel.ConvertToSelect(cv.MemberStatusCodes(), "Id");
        }
        public static IEnumerable<SelectListItem> MaritalStatuses()
        {
            var cv = new CodeValueModel();
            return QueryModel.ConvertToSelect(cv.MaritalStatusCodes(), "Id");
        }
    }
}
