using System;
using System.Collections.Generic;
using System.Linq;
using CmsData;
using CmsWeb.Models;

namespace CmsWeb.Areas.People.Models.Person
{
    public class PersonFamilyModel
    {
        public CmsData.Person person;
        public PagerModel2 Pager { get; set; }
        public PersonFamilyModel(int id)
        {
            person = DbUtil.Db.LoadPersonById(id);
            Pager = new PagerModel2(Count);
            Pager.pagesize = 10;
            Pager.ShowPageSize = false;
        }
        private IQueryable<CmsData.Person> _members;
        private IQueryable<CmsData.Person> FetchMembers()
        {
            if (_members == null)
            {
                var mindt = DateTime.Parse("1/1/1900");
                _members = from m in DbUtil.Db.People
                           where m.FamilyId == person.FamilyId
                           orderby
                                m.DeceasedDate ?? mindt,
                                m.PositionInFamilyId,
                                m.PositionInFamilyId == 10 ? m.GenderId : 0,
                                m.Age descending, m.Name2
                           select m;
            }
            return _members;
        }
        int? _count;
        public int Count()
        {
            if (!_count.HasValue)
                _count = FetchMembers().Count();
            return _count.Value;
        }
        public IEnumerable<CmsWeb.Models.PersonPage.FamilyMemberInfo> Members()
        {
            var q = FetchMembers();
            var q2 = from m in q
                     select new CmsWeb.Models.PersonPage.FamilyMemberInfo
                     {
                        Id = m.PeopleId,
                        Name = m.Name,
                        Age = m.Age,
                        Color = m.DeceasedDate != null ? "red" : "auto",
                        PositionInFamily = m.FamilyPosition.Code,
                        SpouseIndicator = m.PeopleId == person.SpouseId ? "*" : "&nbsp;",
                        Email = m.EmailAddress
                     };
            return q2.Skip(Pager.StartRow).Take(Pager.PageSize);
        }
    }
}
