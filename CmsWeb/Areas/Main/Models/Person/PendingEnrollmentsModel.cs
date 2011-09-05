using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using UtilityExtensions;

namespace CmsWeb.Models.PersonPage
{
    public class PersonPendingEnrollmentsModel
    {
        int PeopleId;
        public Person person { get; set; }
        public PersonPendingEnrollmentsModel(int id)
        {
            PeopleId = id;
            person = DbUtil.Db.LoadPersonById(id);
        }
        public IEnumerable<OrgMemberInfo> PendingEnrollments()
        {
            var dt = Util.Now;
            var q = from o in DbUtil.Db.Organizations
                    let sc = o.OrgSchedules.FirstOrDefault() // SCHED
                    from om in o.OrganizationMembers
                    where om.PeopleId == PeopleId && om.Pending.Value == true
                    let leader = DbUtil.Db.People.SingleOrDefault(p => p.PeopleId == o.LeaderId)
                    orderby o.OrganizationName
                    select new OrgMemberInfo
                    {
                        OrgId = o.OrganizationId,
                        PeopleId = om.PeopleId,
                        Name = o.OrganizationName,
                        Location = o.Location,
                        LeaderName = leader.Name,
                        MeetingTime = sc.MeetingTime,
                        LeaderId = o.LeaderId,
                        EnrollDate = om.EnrollmentDate,
                        MemberType = om.MemberType.Description,
                        DivisionName = om.Organization.Division.Program.Name + "/" + om.Organization.Division.Name,
                    };
            return q;
        }
    }
}
