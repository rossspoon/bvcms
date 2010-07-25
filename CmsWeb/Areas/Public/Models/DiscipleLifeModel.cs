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

namespace CmsWeb.Models
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
                division = DbUtil.Db.Divisions.SingleOrDefault(d => d.Id == value);
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
        private DateTime _Birthday;
        private DateTime birthday
        {
            get
            {
                if (_Birthday == DateTime.MinValue)
                    Util.DateValid(dob, out _Birthday);
                return _Birthday;
            }
        }

        public string phone { get; set; }
        public string homecell { get; set; }
        public string email { get; set; }
        public bool shownew { get; set; }
        public int? gender { get; set; }
        public int? married { get; set; }
        public string addr { get; set; }
        public string zip { get; set; }
        public string city { get; set; }
        public string state { get; set; }

        public Person person { get; set; }

        public int FindMember()
        {
            int count;
            person = CmsWeb.Models.SearchPeopleModel
                .FindPerson(first, last, birthday, email, phone, out count);
            return count;
        }

        public void ValidateModel(ModelStateDictionary modelState)
        {
            CmsWeb.Models.SearchPeopleModel
                .ValidateFindPerson(modelState, first, last, birthday, email, phone);

            if (!phone.HasValue())
                modelState.AddModelError("phone", "phone required");
            if (OrgId == 0)
                modelState.AddModelError("OrgId", "must select a class");

            if (!homecell.HasValue())
                modelState.AddModelError("phone", "Home or Cell required");
            if (!email.HasValue() || !Util.ValidEmail(email))
                modelState.AddModelError("email", "Please specify a valid email address.");
            if (shownew)
            {
                if (!gender.HasValue)
                    modelState.AddModelError("gender2", "gender required");
                if (!married.HasValue)
                    modelState.AddModelError("married2", "marital status required");
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
                OrganizationMember.InsertOrgMembers(
                    OrgId.Value,
                    person.PeopleId,
                    (int)OrganizationMember.MemberTypeCode.Member,
                    DateTime.Today, null, false);
            return true;
        }

        internal void AddPerson()
        {
            var org = DbUtil.Db.Organizations.SingleOrDefault(o => o.OrganizationId == OrgId);

            var f = new Family
            {
                AddressLineOne = addr,
                CityName = city,
                StateCode = state,
                ZipCode = zip,
            };
            var pos = 30;
            person = Person.Add(f, pos,
                null, first, null, last, dob, married == 2, gender.Value, 
                    DbUtil.Settings("DiscLifeOrigin", "0").ToInt(), 
                    DbUtil.Settings("DiscLifeEntry", "0").ToInt());
            if (married == 2 || dob.Age().ToInt() >= 18)
                person.PositionInFamilyId = (int)Family.PositionInFamily.PrimaryAdult;
            switch (homecell)
            {
                case "h":
                    f.HomePhone = phone.GetDigits();
                    break;
                case "c":
                    person.CellPhone = phone.GetDigits();
                    break;
            }
            person.EmailAddress = email;
            person.CampusId = org.CampusId;
            DbUtil.Db.SubmitChanges();
            DbUtil.Db.Refresh(RefreshMode.OverwriteCurrentValues, person);
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
                    var now = Util.Now;
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
                        Selected = r.StateCode == DbUtil.Settings("DefaultState", "TN"),
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
                        Text = ClassName(o)
                    };
            var list = q.ToList();
            if (list.Count == 1)
                return list;
            list.Insert(0, new SelectListItem { Text = "(select a class)", Value = "0" });
            return list;
        }
        private static string ClassName(CmsData.Organization o)
        {
            var lead = o.LeaderName;
            if (lead.HasValue())
                lead = ": " + lead;
            var loc = o.Location;
            if (loc.HasValue())
                loc = " ({0})".Fmt(loc);
            var dt1 = o.FirstMeetingDate;
            var dt2 = o.LastMeetingDate;
            var dt = "";
            if (dt1.HasValue && dt2.HasValue)
                dt = ", {0:MMM d}-{1:MMM d}".Fmt(dt1, dt2);
            else if (dt1.HasValue)
                dt = ", {0:MMM d}".Fmt(dt1);

            return o.OrganizationName + lead + dt + loc;
        }
        public IEnumerable<String> FilledClasses()
        {
            var q = from o in DbUtil.Db.Organizations
                    where o.DivOrgs.Any(od => od.DivId == divid)
                    where o.OnLineCatalogSort != null
                    where (o.ClassFilled ?? false) == true
                    orderby o.OnLineCatalogSort
                    select ClassName(o);
            return q;
        }
        public static IEnumerable<SelectListItem> MaritalStatuses()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value="0", Text="(choose)" },
                new SelectListItem { Value="10", Text="Single" },
                new SelectListItem { Value="20", Text="Married" },
                new SelectListItem { Value="30", Text="Separated" },
                new SelectListItem { Value="40", Text="Divorced" },
                new SelectListItem { Value="50", Text="Widowed" },
            };
        }
    }
}
