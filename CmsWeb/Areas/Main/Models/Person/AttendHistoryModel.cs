using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using UtilityExtensions;

namespace CMSWeb.Models
{
    public class PersonAttendHistoryModel
    {
        public Person person;
        public PagerModel2 Pager { get; set; }
        public PersonAttendHistoryModel(int id)
        {
            person = DbUtil.Db.LoadPersonById(id);
            Pager = new PagerModel2(Count);
        }
        public bool future { get; set; }
        private IQueryable<Attend> _attends;
        private IQueryable<Attend> FetchAttends()
        {
            if (_attends == null)
            {
                var midnight = Util.Now.Date.AddDays(1);
                _attends = from a in DbUtil.Db.Attends
                           where a.PeopleId == person.PeopleId
                           where !(a.Meeting.Organization.SecurityTypeId == 3 && Util.OrgMembersOnly)
                           select a;
                if (future)
                {
                    _attends = from a in _attends
                               where a.MeetingDate >= midnight
                               where a.Registered == true || a.AttendanceFlag == true
                               select a;
                }
                else
                {
                    _attends = from a in _attends
                               where a.MeetingDate < midnight
                               where a.AttendanceFlag == true
                               select a;
                }
            }
            return _attends;
        }
        int? _count;
        public int Count()
        {
            if (!_count.HasValue)
                _count = FetchAttends().Count();
            return _count.Value;
        }
        public IEnumerable<AttendInfo> Attendances()
        {
            var q = FetchAttends();
            q = ApplySort(q, Pager.Sort);

            q = q.Skip(Pager.StartRow).Take(Pager.PageSize);
            var q2 = from a in q
                     let o = a.Meeting.Organization
                     select new AttendInfo
                     {
                         MeetingId = a.MeetingId,
                         OrganizationId = a.Meeting.OrganizationId,
                         OrganizationName = CmsData.Organization
                            .FormatOrgName(o.OrganizationName, o.LeaderName,
                                o.Location),
                         AttendType = a.AttendType.Description ?? "(null)",
                         MeetingName = o.DivOrgs.First(d => d.Division.Program.Name != DbUtil.MiscTagsString).Division.Name + ": " + o.OrganizationName,
                         MeetingDate = a.MeetingDate,
                         MemberType = a.MemberType.Description ?? "(null)",
                     };
            return q2;
        }
        private static IQueryable<Attend> ApplySort(IQueryable<Attend> q, string sort)
        {
            switch (sort)
            {
                case "MemberType":
                    q = q.OrderBy(a => a.MemberTypeId);
                    break;
                case "AttendType":
                    q = q.OrderBy(a => a.AttendanceTypeId);
                    break;
                case "Name":
                    q = q.OrderBy(a => a.Person.LastName).ThenBy(a => a.Person.FirstName);
                    break;
                case "MemberType DESC":
                    q = q.OrderByDescending(a => a.MemberTypeId);
                    break;
                case "AttendType DESC":
                    q = q.OrderByDescending(a => a.AttendanceTypeId);
                    break;
                case "Name DESC":
                    q = q.OrderByDescending(a => a.Person.LastName).ThenByDescending(a => a.Person.FirstName);
                    break;
                case "Organization":
                    q = q.OrderBy(a => a.Meeting.OrganizationId).ThenBy(a => a.MeetingDate);
                    break;
                case "MeetingDate":
                    q = q.OrderBy(a => a.MeetingDate);
                    break;
                case "Organization DESC":
                    q = q.OrderByDescending(a => a.Meeting.OrganizationId).ThenBy(a => a.MeetingDate);
                    break;
                case "MeetingDate DESC":
                default:
                    q = q.OrderByDescending(a => a.MeetingDate);
                    break;
            }
            return q;
        }
    }
}
