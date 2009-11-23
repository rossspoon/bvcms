using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using UtilityExtensions;
using CmsData;
using System.Web.Mvc;

namespace CMSWeb.Models
{
    public class CheckInRecModel
    {
        public class PersonInfo
        {
            public int? PeopleId { get; set; }
            public string Name { get; set; }
            public string Birthday { get; set; }
            public int ImageId { get; set; }
            public string Addr1 { get; set; }
            public string Addr2 { get; set; }
            public string CityStateZip { get; set; }
            public string Phone { get; set; }
            public string Cell { get; set; }
            public string Email { get; set; }
            public string School { get; set; }
            public int? Year { get; set; }
            public string ImageUrl
            {
                get { return "/Image.aspx?portrait=1&id=" + ImageId; }
            }
        }
        public int OrgId { get; set; }
        public string OrgName { get; set; }
        public PersonInfo person { get; set; }
        public CheckInRecModel(int orgId, int? pid)
        {
            var q = from o in DbUtil.Db.Organizations
                    where o.OrganizationId == orgId
                    select o.OrganizationName;
            OrgName = q.Single();
            var q2 = from p in DbUtil.Db.People
                     where p.PeopleId == pid
                     select new PersonInfo
                     {
                         PeopleId = p.PeopleId,
                         Name = p.Name,
                         Birthday = p.DOB,
                         ImageId = p.Picture.MediumId ?? 0,
                         Addr1 = p.PrimaryAddress,
                         Addr2 = p.PrimaryAddress2,
                         CityStateZip = p.CityStateZip,
                         Phone = p.HomePhone.FmtFone("H "),
                         Cell = p.CellPhone.FmtFone("C "),
                         Email = p.EmailAddress,
                         School = p.SchoolOther,
                         Year = p.Grade,
                     };
            person = q2.SingleOrDefault();
            if (person == null)
                person = new PersonInfo
                {
                    PeopleId = 0,
                    Name = "not found",
                };
            OrgId = orgId;
        }
        public string WithBreak(string s)
        {
            if (s.HasValue())
                return s + "<br />";
            return string.Empty;
        }
    }
}
