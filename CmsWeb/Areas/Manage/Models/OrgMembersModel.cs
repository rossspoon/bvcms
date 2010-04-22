using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Data.Linq;
using System.Web;
using CmsData;
using UtilityExtensions;
using System.Web.Mvc;
using CMSPresenter;

namespace CMSWeb.Models
{
    public class OrgMembersModel
    {
        public bool MembersOnly { get; set; }
        public bool SmallGroupsToo { get; set; }
        public int TargetId { get; set; }
        public int SourceId { get; set; }
        public int ProgId { get; set; }
        public int DivId { get; set; }
        public string Sort { get; set; }
        public string Dir { get; set; }

        private IList<string> list = new List<string>();
        public IList<string> List
        {
            get { return list; }
            set { list = value; }
        }

        public IEnumerable<SelectListItem> Programs()
        {
            var q = from c in DbUtil.Db.Programs
                    orderby c.Name
                    select new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    };
            var list = q.ToList();
            list.Insert(0, new SelectListItem
            {
                Value = "0",
                Text = "(not specified)",
            });
            return list;
        }
        public IEnumerable<SelectListItem> Divisions()
        {
            var q = from d in DbUtil.Db.Divisions
                    where d.ProgId == ProgId
                    orderby d.Name
                    select new SelectListItem
                    {
                        Value = d.Id.ToString(),
                        Text = d.Name
                    };
            var list = q.ToList();
            list.Insert(0, new SelectListItem
            {
                Value = "0",
                Text = "(not specified)",
            });
            return list;
        }
        public IEnumerable<SelectListItem> Organizations()
        {
            var q = from s in DbUtil.Db.Organizations
                    where s.DivisionId == DivId
                    orderby s.OrganizationName
                    select new SelectListItem
                    {
                        Value = s.OrganizationId.ToString(),
                        Text = s.OrganizationName
                    };
            var list = q.ToList();
            list.Insert(0, new SelectListItem { Value = "0", Text = "(not specified)" });
            return list;
        }
        public IEnumerable<SelectListItem> Organizations2()
        {
            var q = from s in DbUtil.Db.Organizations
                    where s.DivisionId == DivId
                    orderby s.OrganizationName
                    select new SelectListItem
                    {
                        Value = s.OrganizationId.ToString(),
                        Text = s.OrganizationName + " (" + s.OrganizationMembers.Count(m => m.Person.GenderId == 1) + "+" + s.OrganizationMembers.Count(m => m.Person.GenderId == 2) + "=" + s.OrganizationMembers.Count() + ")"
                    };
            var list = q.ToList();
            list.Insert(0, new SelectListItem { Value = "0", Text = "(not specified)" });
            return list;
        }
        public IEnumerable<MemberInfo> Members()
        {
            var q = ApplySort();
            var q2 = from om in q
                    select new MemberInfo
                    {
                        PeopleId = om.PeopleId,
                        Age = om.Person.Age,
                        Gender = om.Person.Gender.Code,
                        Grade = om.Person.Grade,
                        OrgId = om.OrganizationId,
                        Request = om.Request,
                        Name = om.Person.Name,
                        isChecked = om.OrganizationId == TargetId,
                        MemberStatus = om.MemberType.Description,
                        OrgName = om.Organization.OrganizationName,
                    };
            return q2;
        }
        public IEnumerable<OrganizationMember> ApplySort()
        {
            var q = from om in DbUtil.Db.OrganizationMembers
                    where om.Organization.DivisionId == DivId
                    where SourceId == 0 || om.OrganizationId == SourceId
                    where !MembersOnly || om.MemberTypeId == (int)OrganizationMember.MemberTypeCode.Member
                    select om;

            if (Dir == "asc")
                switch (Sort)
                {
                    default:
                    case "Name":
                        q = from om in q
                            orderby om.Person.Name2
                            select om;
                        break;
                    case "Organization":
                        q = from om in q
                            orderby om.Organization.OrganizationName, om.Person.Name2
                            select om;
                        break;
                    case "Grade":
                        q = from om in q
                            orderby om.Person.Grade, om.Organization.OrganizationName, om.Person.Name2
                            select om;
                        break;
                }
            else
                switch (Sort)
                {
                    default:
                    case "Name":
                        q = from om in q
                            orderby om.Person.Name2 descending
                            select om;
                        break;
                    case "Organization":
                        q = from om in q
                            orderby om.Organization.OrganizationName descending, om.Person.Name2
                            select om;
                        break;
                    case "Grade":
                        q = from om in q
                            orderby om.Person.Grade descending, om.Organization.OrganizationName, om.Person.Name2
                            select om;
                        break;
                }
            return q;
        }
        public void Move()
        {
            foreach (var i in List)
            {
                var a = i.Split('.');
                var pid = a[0].ToInt();
                var oid = a[1].ToInt();
                var om = DbUtil.Db.OrganizationMembers.Single(m => m.PeopleId == pid && m.OrganizationId == oid);
                var sg = om.OrgMemMemTags.Select(mt => mt.MemberTag.Name).ToList();
                var tom = DbUtil.Db.OrganizationMembers.SingleOrDefault(m => m.PeopleId == pid && m.OrganizationId == TargetId);
                if (tom == null)
                    tom = OrganizationMember.InsertOrgMembers(TargetId, pid, (int)OrganizationMember.MemberTypeCode.Member, om.EnrollmentDate.Value, om.InactiveDate, om.Pending ?? false);
                tom.Request = om.Request;
                tom.Amount = om.Amount;
                tom.UserData = om.UserData;
                tom.Grade = om.Grade;
                tom.MemberTypeId = om.MemberTypeId;
                tom.ShirtSize = om.ShirtSize;
                tom.Tickets = om.Tickets;
                foreach (var s in sg)
                    tom.AddToGroup(s);
                om.Drop();
                DbUtil.Db.SubmitChanges();
            }
        }
        public class MemberInfo
        {
            public int PeopleId { get; set; }
            public string Name { get; set; }
            public int OrgId { get; set; }
            public string OrgName { get; set; }
            public string MemberStatus { get; set; }
            public int? Grade { get; set; }
            public int? Age { get; set; }
            public string Gender { get; set; }
            public string Request { get; set; }
            public bool isChecked { get; set; }
            public string Checked
            {
                get { return isChecked ? "checked='checked'" : ""; }
            }
        }
    }
}
