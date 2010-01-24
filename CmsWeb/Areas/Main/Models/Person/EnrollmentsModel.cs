using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using UtilityExtensions;

namespace CMSWeb.Models
{
    public class PersonEnrollmentsModel
    {
        public Person person;
        public PagerModel2 Pager { get; set; }
        public PersonEnrollmentsModel(int id)
        {
            person = DbUtil.Db.LoadPersonById(id);
            Pager = new PagerModel2(Count);
        }
        private IQueryable<OrganizationMember> _enrollments;
        private IQueryable<OrganizationMember> FetchEnrollments()
        {
            if (_enrollments == null)
                _enrollments = from om in DbUtil.Db.OrganizationMembers
                               where om.PeopleId == person.PeopleId
                               where (om.Pending ?? false) == false
                               where !(om.Organization.SecurityTypeId == 3 && Util.OrgMembersOnly)
                               select om;
            return _enrollments;
        }
        int? _count;
        public int Count()
        {
            if (!_count.HasValue)
                _count = FetchEnrollments().Count();
            return _count.Value;
        }
        public IEnumerable<OrgMemberInfo> Enrollments()
        {
            var q = FetchEnrollments();
            q = ApplySort(q, Pager.Sort);

            var q2 = from om in q
                     let div = om.Organization.DivOrgs.FirstOrDefault(d => d.Division.Program.Name != DbUtil.MiscTagsString).Division
                     select new OrgMemberInfo
                     {
                         OrgId = om.OrganizationId,
                         PeopleId = om.PeopleId,
                         Name = om.Organization.OrganizationName,
                         Location = om.Organization.Location,
                         LeaderName = om.Organization.LeaderName,
                         MeetingTime = om.Organization.MeetingTime,
                         MemberType = om.MemberType.Description,
                         LeaderId = om.Organization.LeaderId,
                         EnrollDate = om.EnrollmentDate,
                         AttendPct = (om.AttendPct == null ? 0 : om.AttendPct.Value),
                         DivisionName = div.Program.Name + "/" + div.Name,
                     };
            return q2.Skip(Pager.StartRow).Take(Pager.PageSize);
        }
        private IQueryable<OrganizationMember> ApplySort(IQueryable<OrganizationMember> q, string sortExpression)
        {
            switch (sortExpression)
            {
                case "Name":
                    q = q.OrderBy(om => om.Organization.OrganizationName);
                    break;
                case "EnrollDate":
                    q = q.OrderBy(om => om.EnrollmentDate);
                    break;
                case "Name DESC":
                    q = q.OrderByDescending(om => om.Organization.OrganizationName);
                    break;
                case "EnrollDate DESC":
                    q = q.OrderByDescending(om => om.EnrollmentDate);
                    break;
            }
            return q;
        }
    }
}
