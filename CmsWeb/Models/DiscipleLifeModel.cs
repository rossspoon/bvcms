using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using CmsData;
using System.Web.Mvc;
using System.Text;
using System.Configuration;
using UtilityExtensions;
using System.Data.Linq.SqlClient;
using CMSPresenter;

namespace CMSWeb.Models
{
    public class DiscipleLifeModel
    {
        public Division division { get; set; }
        public int? divid
        {
            get
            {
                return division.Id;
            }
            set
            {
                division = DbUtil.Db.Divisions.Single(d => d.Id == value);
            }
        }
        public CmsData.Organization organization { get; set; }
        public int? OrgId
        {
            get
            {
                return organization == null ? 0 : organization.OrganizationId;
            }
            set
            {
                organization = DbUtil.Db.Organizations.SingleOrDefault(o => o.OrganizationId == value);
            }
        }

        public string first { get; set; }
        public string last { get; set; }
        public string dob { get; set; }
        private DateTime birthday;

        public string phone { get; set; }
        public string email { get; set; }
        public bool preferredEmail { get; set; }
        public bool shownew { get; set; }
        public int? gender { get; set; }
        public string addr { get; set; }
        public string zip { get; set; }
        public string city { get; set; }
        public string state { get; set; }

        public Person person { get; set; }

        public int FindMember()
        {
            person = DbUtil.Db.People.SingleOrDefault(pp => pp.PeopleId == first.Substring(1).ToInt());
            if (person != null)
                return 1;
            var fone = Util.GetDigits(phone);
            var q = from p in DbUtil.Db.People
                    where (p.FirstName == first || p.NickName == first || p.MiddleName == first)
                    where (p.LastName == last || p.MaidenName == last)
                    where p.Family.People.Any(pp => pp.CellPhone.Contains(fone) || p.WorkPhone.Contains(fone)) || p.Family.HomePhone.Contains(fone)
                    where p.BirthDay == birthday.Day && p.BirthMonth == birthday.Month && p.BirthYear == birthday.Year
                    select p;
            var count = q.Count();
            person = null;
            if (count == 1)
                person = q.Single();
            return count;
        }

        public void ValidateModel(ModelStateDictionary modelState)
        {
            if (first.StartsWith("p") && first.Substring(1).ToInt() > 0 && !last.HasValue())
                return;

            first = first.Trim();
            last = last.Trim();
            if (OrgId == 0)
                modelState.AddModelError("OrgId", "must select a class");
            if (!first.HasValue())
                modelState.AddModelError("first", "first name required");
            if (!last.HasValue())
                modelState.AddModelError("last", "last name required");
            if (!Util.DateValid(dob, out birthday))
                modelState.AddModelError("dob", "valid birth date required");

            var d = phone.GetDigits().Length;
            if (d != 7 && d != 10)
                modelState.AddModelError("phone", "7 or 10 digits");
            if (!email.HasValue() || !Util.ValidEmail(email))
                modelState.AddModelError("email", "Please specify a valid email address.");
            if (shownew)
            {
                if (!gender.HasValue)
                    modelState.AddModelError("gender2", "gender required");
                if (!addr.HasValue())
                    modelState.AddModelError("addr", "need address");
                if (zip.GetDigits().Length != 5)
                    modelState.AddModelError("zip", "need 5 digit zip");
                if (!city.HasValue())
                    modelState.AddModelError("city", "need city");
                if (!state.HasValue())
                    modelState.AddModelError("state", "need state");
            }
        }

        internal bool EnrollInOrg(Person person)
        {
            var member = DbUtil.Db.OrganizationMembers.SingleOrDefault(om =>
                om.OrganizationId == OrgId && om.PeopleId == person.PeopleId);
            if (member == null)
                OrganizationController.InsertOrgMembers(
                    OrgId.Value,
                    person.PeopleId,
                    (int)OrganizationMember.MemberTypeCode.Member,
                    DateTime.Today, null, false);
            return true;
        }

        internal void AddPerson()
        {
            var f = new Family
            {
                AddressLineOne = addr,
                CityName = city,
                StateCode = state,
                ZipCode = zip,
                HomePhone = phone,
            };
            person = Person.Add(f, 30,
                null, first, null, last, dob, false, gender.Value, 
                    DbUtil.Settings("DiscLifeOrigin").ToInt(), 
                    DbUtil.Settings("DiscLifeEntry").ToInt());
            DbUtil.Db.SubmitChanges();
        }

        private class RecItem
        {
            public int? StartAge { get; set; }
            public int? EndAge { get; set; }
            public int? OrgId { get; set; }
            public string AgeDate { get; set; }
            public DateTime agedate
            { 
                get 
                { 
                    var dt = new DateTime[3];
                    dt[0] = DateTime.Parse(AgeDate);
                    dt[1] = dt[0].AddYears(1);
                    dt[2] = dt[0].AddYears(-1);
                    var now = DateTime.Now;
                    var q = from d in dt
                            orderby Math.Abs(d.Subtract(now).TotalDays)
                            select d;
                    var r = q.First();
                    return r;
                }
            }
        }
        public IEnumerable<SelectListItem> StateList()
        {
            var q = from r in DbUtil.Db.StateLookups
                    select new SelectListItem
                    {
                        Text = r.StateCode,
                        Selected = r.StateCode == "TN",
                    };
            return q;
        }
        public IEnumerable<SelectListItem> Classes()
        {
            var q = from o in DbUtil.Db.Organizations
                    where o.DivOrgs.Any(od => od.DivId == divid)
                    where o.OrganizationStatusId == (int)CmsData.Organization.OrgStatusCode.Active
                    where o.OnLineCatalogSort != null
                    where (o.ClassFilled ?? false) == false
                    orderby o.OnLineCatalogSort
                    select new SelectListItem
                    {
                        Value = o.OrganizationId.ToString(),
                        Text = "{0}:{1}, {2} [{3:M/d}-{4:M/d}]".Fmt(
                            o.OrganizationName, o.LeaderName, o.Location, o.FirstMeetingDate, o.LastMeetingDate)
                    };
            var list = q.ToList();
            list.Insert(0, new SelectListItem { Text = "(select a class)", Value = "0" });
            return list;
        }
        public IEnumerable<String> FilledClasses()
        {
            var q = from o in DbUtil.Db.Organizations
                    where o.DivOrgs.Any(od => od.DivId == divid)
                    where o.OnLineCatalogSort != null
                    where (o.ClassFilled ?? false) == true
                    orderby o.OnLineCatalogSort
                    select "{0}:{1}, {2} [{3:M/d}-{4:M/d}]".Fmt(
                            o.OrganizationName, o.LeaderName, o.Location, o.FirstMeetingDate, o.LastMeetingDate);
            return q;
        }
    }
}
