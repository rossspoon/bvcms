using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using UtilityExtensions;

namespace CMSWeb.Models
{
    public class PersonPendingEnrollmentsModel
    {
        public Person person;
        public PersonPendingEnrollmentsModel(int id)
        {
            person = DbUtil.Db.LoadPersonById(id);
        }
        public IEnumerable<OrgMemberInfo> PendingEnrollments()
        {
            var dt = Util.Now;
            var q = from o in DbUtil.Db.Organizations
                    from om in o.OrganizationMembers
                    where om.PeopleId == person.PeopleId && om.Pending.Value == true
                    let leader = DbUtil.Db.People.SingleOrDefault(p => p.PeopleId == o.LeaderId)
                    let div = om.Organization.DivOrgs.FirstOrDefault(d => d.Division.Program.Name != DbUtil.MiscTagsString).Division
                    orderby o.OrganizationName
                    select new OrgMemberInfo
                    {
                        OrgId = o.OrganizationId,
                        PeopleId = om.PeopleId,
                        Name = o.OrganizationName,
                        Location = o.Location,
                        LeaderName = leader.Name,
                        MeetingTime = o.MeetingTime,
                        LeaderId = o.LeaderId,
                        EnrollDate = om.EnrollmentDate,
                        MemberType = om.MemberType.Description,
                        DivisionName = div.Program.Name + "/" + div.Name,
                    };
            return q;
        }
    }
}
