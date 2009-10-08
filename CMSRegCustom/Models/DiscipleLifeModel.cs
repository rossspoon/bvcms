﻿using System;
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

namespace CMSRegCustom.Models
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
        private DateTime birthday;

        public string phone { get; set; }
        public string homecell { get; set; }
        public string email { get; set; }
        public bool preferredEmail { get; set; }
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
            var fone = Util.GetDigits(phone);

            first = first.Trim();
            last = last.Trim();
            var q = from p in DbUtil.Db.People
                    where (p.FirstName == first || p.NickName == first || p.MiddleName == first)
                    where (p.LastName == last || p.MaidenName == last)
                    where p.BirthDay == birthday.Day && p.BirthMonth == birthday.Month && p.BirthYear == birthday.Year
                    select p;
            var count = q.Count();
            if (count > 1)
                q = from p in q
                    where p.CellPhone.Contains(fone)
                            || p.WorkPhone.Contains(fone)
                            || p.Family.HomePhone.Contains(fone)
                    select p;
            count = q.Count();

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
            else if (!homecell.HasValue())
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
            if (married == 2 || dob.Age().ToInt() >= 18)
                pos = (int)Family.PositionInFamily.PrimaryAdult;
            person = Person.Add(f, pos,
                null, first, null, last, dob, married == 2, gender.Value, 
                    DbUtil.Settings("DiscLifeOrigin", "0").ToInt(), 
                    DbUtil.Settings("DiscLifeEntry", "0").ToInt());
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