using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using UtilityExtensions;

namespace CmsWeb.Areas.People.Models.Person
{
    public class PendingEnrollments
    {
        readonly int PeopleId;
        public CmsData.Person person { get; set; }
        public CmsWeb.Models.PagerModel2 Pager { get; set; }
        public PendingEnrollments(int id)
        {
            PeopleId = id;
            Pager = new CmsWeb.Models.PagerModel2(Count);
            person = DbUtil.Db.LoadPersonById(id);
        }
        int? _count;
        public int Count()
        {
            if (!_count.HasValue)
                _count = Fetch().Count();
            return _count.Value;
        }
        private IQueryable<OrganizationMember> _enrollments;
        private IQueryable<OrganizationMember> Fetch()
        {
            if (_enrollments == null)
                _enrollments = DbUtil.Db.OrganizationMembers.Where(tt => tt.OrganizationId == -1);
            _count = 0;
            return _enrollments;
        }

        public IEnumerable<OrgMemberInfo> List()
        {
            var dt = Util.Now;
            var roles = DbUtil.Db.CurrentRoles();
            var q = from om in Fetch()
                    select new OrgMemberInfo();
//
//                    let sc = o.OrgSchedules.FirstOrDefault() // SCHED
//                    from om in o.OrganizationMembers
//                    where om.PeopleId == PeopleId && om.Pending.Value == true
//				    where o.LimitToRole == null || roles.Contains(o.LimitToRole)
//                    let leader = DbUtil.Db.People.SingleOrDefault(p => p.PeopleId == o.LeaderId)
//                    orderby o.OrganizationName
//                    select new OrgMemberInfo
//                    {
//                        OrgId = o.OrganizationId,
//                        PeopleId = om.PeopleId,
//                        Name = o.OrganizationName,
//                        Location = o.Location,
//                        LeaderName = leader.Name,
//                        MeetingTime = sc.MeetingTime,
//                        LeaderId = o.LeaderId,
//                        EnrollDate = om.EnrollmentDate,
//                        MemberType = om.MemberType.Description,
//                        DivisionName = om.Organization.Division.Program.Name + "/" + om.Organization.Division.Name,
//                    };
            return q;
        }
    }
}
