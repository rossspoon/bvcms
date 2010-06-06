using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using System.Web.Mvc;
using UtilityExtensions;
using CMSPresenter;
using System.Text.RegularExpressions;

namespace CMSWeb.Models
{
    public class OrgMembersDialogModel
    {
        public int orgid { get; set; }
        public bool inactives { get; set; }
        public bool pendings { get; set; }
        public int? sg { get; set; }

        public int memtype { get; set; }
        public int tag { get; set; }
        public DateTime? inactivedt { get; set; }

        public int MemberType { get; set; }
        public DateTime? InactiveDate { get; set; }
        public bool Pending { get; set; }

        private IList<int> list = new List<int>();
        public IList<int> List
        {
            get { return list; }
            set { list = value; }
        }
        public IEnumerable<SelectListItem> Tags()
        {
            var cv = new CodeValueController();
            var tg = QueryModel.ConvertToSelect(cv.UserTags(Util.UserPeopleId), "Id").ToList();
            tg.Insert(0, new SelectListItem { Value = "0", Text = "(not specified)" });
            return tg;
        }
        private List<SelectListItem> mtypes;
        private List<SelectListItem> MemberTypes()
        {
            if (mtypes == null)
            {
                var q = from mt in DbUtil.Db.MemberTypes
                        where mt.Id != (int)OrganizationMember.MemberTypeCode.Visitor
                        where mt.Id != (int)OrganizationMember.MemberTypeCode.VisitingMember
                        orderby mt.Description
                        select new SelectListItem
                        {
                            Value = mt.Id.ToString(),
                            Text = mt.Description,
                        };
                mtypes = q.ToList();
            }
            return mtypes;
        }
        public IEnumerable<SelectListItem> MemberTypeCodesWithDrop()
        {
            var mt = MemberTypes().ToList();
            mt.Insert(0, new SelectListItem { Value = "-1", Text = "Drop" });
            mt.Insert(0, new SelectListItem { Value = "0", Text = "(not specified)" });
            return mt;
        }
        public IEnumerable<SelectListItem> MemberTypeCodesWithNotSpecified()
        {
            var mt = MemberTypes().ToList();
            mt.Insert(0, new SelectListItem { Value = "0", Text = "(not specified)" });
            return mt;
        }

        public int count;
        public IEnumerable<PersonDialogSearchInfo> FetchOrgMemberList()
        {
            var q = OrgMembers();
            if (memtype != 0)
                q = q.Where(om => om.MemberTypeId == memtype);
            if (tag > 0)
                q = q.Where(om => om.Person.Tags.Any(t => t.Id == tag));
            if (inactivedt.HasValue)
                q = q.Where(om => om.InactiveDate == inactivedt);

            count = q.Count();
            var q1 = q.OrderBy(m => m.Person.Name2);
            var q2 = from m in q1
                     let p = m.Person
                     select new PersonDialogSearchInfo
                     {
                         PeopleId = m.PeopleId,
                         Name = p.Name,
                         LastName = p.LastName,
                         JoinDate = p.JoinDate,
                         BirthDate = p.DOB,
                         Address = p.PrimaryAddress,
                         CityStateZip = p.CityStateZip,
                         HomePhone = p.HomePhone.FmtFone(),
                         CellPhone = p.CellPhone.FmtFone(),
                         WorkPhone = p.WorkPhone.FmtFone(),
                         Email = p.EmailAddress,
                         Age = p.Age,
                         MemberStatus = p.MemberStatus.Description,
                         ischecked = list.Contains(m.PeopleId)
                     };
            return q2;
        }
        public IQueryable<OrganizationMember> OrgMembers()
        {
            int inactive = (int)OrganizationMember.MemberTypeCode.InActive;
            var q = from om in DbUtil.Db.OrganizationMembers
                    where om.OrganizationId == orgid
                    where om.OrgMemMemTags.Any(g => g.MemberTagId == sg) || (sg ?? 0) == 0
                    where (om.Pending ?? false) == pendings
                    where (inactives && om.MemberTypeId == inactive)
                        || (!inactives && om.MemberTypeId != inactive)
                    select om;
            return q;
        }
        public class PersonDialogSearchInfo
        {
            public int PeopleId { get; set; }
            public string Name { get; set; }
            public string LastName { get; set; }
            public DateTime? JoinDate { get; set; }
            public string Email { get; set; }
            public string BirthDate { get; set; }
            public string Address { get; set; }
            public string CityStateZip { get; set; }
            public string HomePhone { get; set; }
            public string CellPhone { get; set; }
            public string WorkPhone { get; set; }
            public int? Age { get; set; }
            public string MemberStatus { get; set; }
            public bool ischecked { get; set; }
            public string Checked()
            {
                return ischecked ? "checked='checked'" : "";
            }

            public string ToolTip
            {
                get
                {
                    return "{0} ({1})|Cell Phone: {2}|Work Phone: {3}|Home Phone: {4}|BirthDate: {5:d}|Join Date: {6:d}|Status: {7}|Email: {8}"
                        .Fmt(Name, PeopleId, CellPhone, WorkPhone, HomePhone, BirthDate, JoinDate, MemberStatus, Email);
                }
            }
        }
    }
}
