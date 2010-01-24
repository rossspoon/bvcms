using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using UtilityExtensions;

namespace CMSWeb.Models
{
    public class PersonPrevEnrollmentsModel
    {
        public Person person;
        public PagerModel2 Pager { get; set; }
        public PersonPrevEnrollmentsModel(int id)
        {
            person = DbUtil.Db.LoadPersonById(id);
            Pager = new PagerModel2(Count);
        }
        private IQueryable<EnrollmentTransaction> _enrollments;
        private IQueryable<EnrollmentTransaction> FetchPrevEnrollments()
        {
            if (_enrollments == null)
                _enrollments = from etd in DbUtil.Db.EnrollmentTransactions
                    where etd.TransactionStatus == false
                    where etd.PeopleId == person.PeopleId
                    where etd.TransactionTypeId >= 4
                    where !(etd.Organization.SecurityTypeId == 3 && Util.OrgMembersOnly)
                    select etd;
            return _enrollments;
        }
        int? _count;
        public int Count()
        {
            if (!_count.HasValue)
                _count = FetchPrevEnrollments().Count();
            return _count.Value;
        }
        public IEnumerable<OrgMemberInfo> PrevEnrollments()
        {
            var q = FetchPrevEnrollments();
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
                         AttendPct = om.AttendancePercentage,
                         DivisionName = div.Program.Name + "/" + div.Name,
                     };
            return q2.Skip(Pager.StartRow).Take(Pager.PageSize);
        }
        private IQueryable<EnrollmentTransaction> ApplySort(IQueryable<EnrollmentTransaction> q, string sortExpression)
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
