/* Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CmsData;
using System.Data.Linq;
using System.ComponentModel;
using UtilityExtensions;
using CMSPresenter.InfoClasses;
using System.Web;

namespace CMSPresenter
{
    public class FamilyController
    {
        private int _count; 

        private static IEnumerable<FamilyMember> FetchList(IQueryable<Family> query)
        {
            var Db = query.GetDataContext() as CMSDataContext;
            var q = from f in query
                    from p in f.People
                    let hoh = p.PeopleId == f.HeadOfHouseholdId ? 1 : p.PeopleId == f.HeadOfHouseholdSpouseId ? 2 : 3
                    orderby hoh, p.Age descending, p.LastName, p.Name
                    select new FamilyMember{
                        Age = p.Age,
                        Id = p.FamilyId,
                        Deceased = p.DeceasedDate != null,
                        MemberStatus = p.MemberStatus.Description,
                        Name = p.Name,
                        PositionInFamilyId = p.FamilyPosition.Id,
                        PositionInFamily = p.FamilyPosition.Description,
                        PeopleId = p.PeopleId,
                    };
            return q;

        }


         public IEnumerable<FamilyMember> GetFamilyMembers(int id)
         {
             var q = from f in DbUtil.Db.Families
                     where f.FamilyId == id
                     select f;
             _count = q.Count();
             return FetchList(q);
         }

         public IEnumerable<RelatedFamily> GetRelatedFamilies(int id)
         {
             var rf1 = from rf in DbUtil.Db.Families.Single(f => f.FamilyId == id).RelatedFamilies1
                       select rf;
             var rf2 = from rf in DbUtil.Db.Families.Single(f => f.FamilyId == id).RelatedFamilies2
                       select rf;
             var q = rf1.Union(rf2);

             return q;
         }

         [DataObjectMethod(DataObjectMethodType.Update, false)]
         public void UpdateFamilyMember(int PeopleId, int PositionInFamilyId)
         {
             var p = DbUtil.Db.People.Single(a => a.PeopleId == PeopleId);
             var c = DbUtil.Db.People.Count(a => a.FamilyId == p.FamilyId 
                 && a.PositionInFamilyId == (int)Family.PositionInFamily.PrimaryAdult);
             if (!(PositionInFamilyId == (int)Family.PositionInFamily.PrimaryAdult && c > 1))
             {
                p.PositionInFamilyId = PositionInFamilyId;
                DbUtil.Db.SubmitChanges();
             }

         }

         [DataObjectMethod(DataObjectMethodType.Update, false)]
         public void UpdateFamilyRelation(int FamilyId, int RelatedFamilyId, string FamilyRelationshipDesc)
         {
             Family f;
             RelatedFamily fr;

             f = DbUtil.Db.Families.Single(g => g.FamilyId == FamilyId);
             fr = f.RelatedFamilies1.First(a => a.RelatedFamilyId == RelatedFamilyId);
             if (fr == null) fr = f.RelatedFamilies2.First(a => a.FamilyId == RelatedFamilyId);
             fr.FamilyRelationshipDesc = FamilyRelationshipDesc ?? "";
             DbUtil.Db.SubmitChanges();

         }
        //public IEnumerable<FamilyMember> ListByQuery(int qid)
        //{
        //    var Db = new CMSDataContext(Util.ConnectionString);
        //    var qB = Db.LoadQueryById(qid);
        //    var q = Db.Families.Where(qB.Predicate());
        //    return FetchList(q);
        //}
         public static void AddRelatedFamily(int familyId, int relatedPersonId)
         {
             var p = DbUtil.Db.LoadPersonById(relatedPersonId);
             var rf = new RelatedFamily
             {
                 FamilyId = familyId,
                 RelatedFamilyId = p.FamilyId,
                 FamilyRelationshipDesc = "Add Description",
                 CreatedBy = Util.UserId1,
                 CreatedDate = DateTime.Now,
             };
             DbUtil.Db.RelatedFamilies.InsertOnSubmit(rf);
             DbUtil.Db.SubmitChanges();
         }
    }
}
