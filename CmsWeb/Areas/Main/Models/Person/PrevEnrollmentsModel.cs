using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using UtilityExtensions;

namespace CmsWeb.Models.PersonPage
{
    public class PersonPrevEnrollmentsModel
    {
        private int PeopleId;
        public Person person { get; set; }
        public PagerModel2 Pager { get; set; }
        public PersonPrevEnrollmentsModel(int id)
        {
            PeopleId = id;
            person = DbUtil.Db.LoadPersonById(id);
            Pager = new PagerModel2(Count);
        }
        private IQueryable<EnrollmentTransaction> _enrollments;
        private IQueryable<EnrollmentTransaction> FetchPrevEnrollments()
        {
            if (_enrollments == null)
            {
                var limitvisibility = Util2.OrgMembersOnly || Util2.OrgLeadersOnly
                    || !HttpContext.Current.User.IsInRole("Access");
                _enrollments = from etd in DbUtil.Db.EnrollmentTransactions
                               where etd.TransactionStatus == false
                               where etd.PeopleId == PeopleId
                               where etd.TransactionTypeId >= 4
                               where !(limitvisibility && etd.Organization.SecurityTypeId == 3)
                               select etd;
            }
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
            var q = ApplySort();
            q = q.Skip(Pager.StartRow).Take(Pager.PageSize);
            var q2 = from om in q
                     select new OrgMemberInfo
                     {
                         OrgId = om.OrganizationId,
                         PeopleId = om.PeopleId,
                         Name = om.OrganizationName,
                         MemberType = om.MemberType.Description,
                         EnrollDate = om.FirstTransaction.TransactionDate,
                         AttendPct = om.AttendancePercentage,
                         DivisionName = om.Organization.Division.Program.Name + "/" + om.Organization.Division.Name,
                     };
            return q2;
        }
        private IQueryable<EnrollmentTransaction> ApplySort()
        {
            var q = FetchPrevEnrollments();
            switch (Pager.SortExpression)
            {
                case "Organization":
                    q = q.OrderBy(om => om.Organization.OrganizationName);
                    break;
                case "Enroll Date":
                    q = q.OrderBy(om => om.FirstTransaction.TransactionDate);
                    break;
                case "Organization desc":
                    q = q.OrderByDescending(om => om.Organization.OrganizationName);
                    break;
                case "Enroll Date desc":
                    q = q.OrderByDescending(om => om.FirstTransaction.TransactionDate);
                    break;
            }
            return q;
        }
    }
}
