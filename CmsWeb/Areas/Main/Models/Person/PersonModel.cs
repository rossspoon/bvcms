using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using System.Web.Mvc;
using UtilityExtensions;
using CMSPresenter;

namespace CMSWeb.Models.PersonPage
{
    public class PersonModel
    {
        public PersonInfo displayperson;
        public PersonModel(int? id)
        {
            displayperson = PersonInfo.GetPersonInfo(id);
            vol = DbUtil.Db.Volunteers.FirstOrDefault(v => v.PeopleId == id.Value);
            if (vol == null)
                vol = new Volunteer();
        }
        private Person _person;
        public Person Person
        {
            get
            {
                if (_person == null)
                    _person = DbUtil.Db.LoadPersonById(displayperson.PeopleId);
                return _person;
            }
        }

        public Volunteer vol;
        public string Name
        {
            get { return displayperson.Name; }
            set { displayperson.Name = value; }
        }
        public int? ckorg;
        public bool CanCheckIn
        {
            get
            {
                ckorg = (int?)HttpContext.Current.Session["CheckInOrgId"];
                return ckorg.HasValue;
            }
        }
        public class FamilyMember
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int? Age { get; set; }
            public string Color { get; set; }
            public string PositionInFamily { get; set; }
            public string SpouseIndicator { get; set; }
        }
        public IEnumerable<FamilyMember> FamilyMembers()
        {
            var q = from m in DbUtil.Db.People
                    where m.FamilyId == displayperson.FamilyId
                    orderby
                        m.PeopleId == m.Family.HeadOfHouseholdId ? 1 :
                        m.PeopleId == m.Family.HeadOfHouseholdSpouseId ? 2 :
                        3, m.Age descending, m.Name2
                    select new FamilyMember
                    {
                        Id = m.PeopleId,
                        Name = m.Name,
                        Age = m.Age,
                        Color = m.DeceasedDate != null ? "red" : "#336699",
                        PositionInFamily = m.FamilyPosition.Code,
                        SpouseIndicator = m.PeopleId == displayperson.SpouseId ? "*" : "&nbsp;"
                    };
            return q;
        }
        public string addrtab { get { return displayperson.PrimaryAddr.Name; } }

    }
}
