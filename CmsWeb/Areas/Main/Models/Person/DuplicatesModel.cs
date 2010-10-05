using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using UtilityExtensions;

namespace CmsWeb.Models.PersonPage
{
    public class DuplicatesModel
    {
        private int PeopleId;
        public Person person { get; set; }
        public DuplicatesModel(int id)
        {
            PeopleId = id;
            person = DbUtil.Db.LoadPersonById(id);
        }
        public IQueryable<DuplicateInfo> FetchDuplicates()
        {
            var dups = person.PossibleDuplicates();
            var q = from p in DbUtil.Db.People
                    where dups.Contains(p.PeopleId)
                    select new DuplicateInfo
                    {
                        PeopleId = p.PeopleId,
                        Name = p.Name,
                        Address = p.PrimaryAddress,
                        Cell = p.CellPhone,
                        Home = p.HomePhone,
                        Email = p.EmailAddress,
                        Age = p.Age,
                        DOB = p.BirthDate
                    };
            return q;
        }
        public class DuplicateInfo
        {
            public int PeopleId { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public string Cell { get; set; }
            public string Home { get; set; }
            public string Email { get; set; }
            public int? Age { get; set; }
            public DateTime? DOB { get; set; }
        }
    }
}
