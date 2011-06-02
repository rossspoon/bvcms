using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CmsData;
using System.Web.Mvc;
using UtilityExtensions;
using CMSPresenter;

namespace CmsWeb.Models.PersonPage
{
    public class FamilyModel
    {
        public int familyid { get; set; }
        private Family _family;
        public Family Family
        {
            get
            {
                if (_family == null)
                    _family = DbUtil.Db.Families.SingleOrDefault(ff => ff.FamilyId == familyid);
                return _family;
            }
        }
        public class FamilyMemberInfo
        {
            public int PeopleId { get; set; }
            public string Name { get; set; }
            public string FamilyPosition { get; set; }
            public int? Age { get; set; }
            public string MemberStatus { get; set; }
            public bool Deceased { get; set; }
        }
        public IEnumerable<FamilyMemberInfo> Members()
        {
            var q = from m in Family.People
                    orderby m.PeopleId == Family.HeadOfHouseholdId ? 1 :
                            m.PeopleId == Family.HeadOfHouseholdSpouseId ? 2 :
                            3, m.Age descending, m.Name2
                    select new FamilyMemberInfo
                    {
                        PeopleId = m.PeopleId,
                        Name = m.Name,
                        FamilyPosition = m.FamilyPosition.Description,
                        Age = m.Age,
                        MemberStatus = m.MemberStatus.Description,
                        Deceased = m.Deceased
                    };
            return q;
        }
        public class RelatedFamilyInfo
        {
            public int Id { get; set; }
            public int Id1 { get; set; }
            public int Id2 { get; set; }
            public int PeopleId { get; set; }
            public string Description { get; set; }
            public string Name { get; set; }
        }
         public IEnumerable<RelatedFamilyInfo> RelatedFamilies()
         {
             var rf1 = from rf in Family.RelatedFamilies1
                       select new RelatedFamilyInfo
                       {
                           Id = familyid,
                           Id1 = rf.FamilyId,
                           Id2 = rf.RelatedFamilyId,
                           PeopleId = rf.RelatedFamily2.HeadOfHousehold.PeopleId,
                           Name = "The " + rf.RelatedFamily2.HeadOfHousehold.Name + " Family",
                           Description = rf.FamilyRelationshipDesc
                       };
             var rf2 = from rf in Family.RelatedFamilies2
                       select new RelatedFamilyInfo
                       {
                           Id = familyid,
                           Id1 = rf.FamilyId,
                           Id2 = rf.RelatedFamilyId,
                           PeopleId = rf.RelatedFamily1.HeadOfHousehold.PeopleId,
                           Name = "The " + rf.RelatedFamily1.HeadOfHousehold.Name + " Family",
                           Description = rf.FamilyRelationshipDesc
                       };
             var q = rf1.Union(rf2);
             return q;
         }
    }
}
