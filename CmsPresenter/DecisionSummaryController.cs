/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
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
using System.Web;
using System.Collections;
using System.Diagnostics;
using CmsData.Codes;

namespace CMSPresenter
{
    public class TypeCountInfo
    {
        public string Id { get; set; }
        public string Desc { get; set; }
        public int? Count { get; set; }
        public string CssClass { get { return Desc == "Total" ? "TotalLine" : ""; } }
    }
    public class PersonInfo2
    {
        public int PeopleId { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }

    [DataObject]
    public class DecisionSummaryController
    {
        private int[] decisionTypes = new int[] 
        { 
            DecisionCode.Unknown,
            DecisionCode.ProfessionNotForMembership,
            DecisionCode.Cancelled,
        };
        IEnumerable<TypeCountInfo> Total(int? count)
        {
            if (!count.HasValue || count.Value == 0)
                return new TypeCountInfo[] { };
            return new TypeCountInfo[]  
            { 
                new TypeCountInfo() { Id="All", Desc = "Total", Count = count ?? 0 } 
            };
        }
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public IEnumerable<TypeCountInfo> DecisionsByType(DateTime? dt1, DateTime? dt2)
        {
            if (!dt1.HasValue)
                return null;
            // member decisions
            var q = from p in DbUtil.Db.People
                    where p.DecisionDate >= dt1 && p.DecisionDate < (dt2 ?? dt1).Value.AddDays(1)
                    where p.DecisionTypeId != null
                    where !decisionTypes.Contains(p.DecisionTypeId.Value)
                    group p by p.DecisionTypeId + "," + p.DecisionType.Code into g
                    orderby g.Key
                    select new TypeCountInfo
                    {
                        Id = g.Key,
                        Desc = g.First().DecisionType.Description,
                        Count = g.Count(),
                    };
            // non member decisions
            var q2 = from p in DbUtil.Db.People
                     where p.DecisionDate >= dt1 && p.DecisionDate < (dt2 ?? dt1).Value.AddDays(1)
                     where p.DecisionTypeId == null ||
                         decisionTypes.Contains(p.DecisionTypeId.Value)
                     group p by p.DecisionTypeId + "," + p.DecisionType.Code into g
                     orderby g.Key
                     select new TypeCountInfo
                     {
                         Id = g.Key,
                         Desc = g.First().DecisionType.Description,
                         Count = g.Count(),
                     };
            return q.ToList().Union(Total(q.Sum(t => t.Count))).Union(q2);
        }
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public IEnumerable<TypeCountInfo> BaptismsByAge(DateTime? dt1, DateTime? dt2)
        {
            if (!dt1.HasValue)
                return null;
            var q = from p in DbUtil.Db.People
                    let agerange = DbUtil.Db.BaptismAgeRange(p.Age ?? 0)
                    where p.BaptismDate >= dt1 && p.BaptismDate < (dt2 ?? dt1).Value.AddDays(1)
                    group p by agerange into g
                    orderby g.Key
                    select new TypeCountInfo
                    {
                        Id = g.Key,
                        Desc = g.Key,
                        Count = g.Count(),
                    };
            return q.ToList().Union(Total(q.Sum(t => t.Count)));
        }
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public IEnumerable<TypeCountInfo> BaptismsByType(DateTime? dt1, DateTime? dt2)
        {
            if (!dt1.HasValue)
                return null;
            var q = from p in DbUtil.Db.People
                    where p.BaptismDate >= dt1 && p.BaptismDate < (dt2 ?? dt1).Value.AddDays(1)
                    group p by p.BaptismTypeId + "," + p.BaptismType.Code into g
                    orderby g.Key
                    select new TypeCountInfo
                    {
                        Id = g.Key,
                        Desc = g.First().BaptismType.Description,
                        Count = g.Count(),
                    };
            return q.ToList().Union(Total(q.Sum(t => t.Count)));
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public IEnumerable<TypeCountInfo> NewMemberByType(DateTime? dt1, DateTime? dt2)
        {
            if (!dt1.HasValue)
                return null;
            var q = from p in DbUtil.Db.People
                    where p.JoinDate >= dt1 && p.JoinDate < (dt2 ?? dt1).Value.AddDays(1)
                    group p by p.JoinCodeId + "," + p.JoinType.Code into g
                    orderby g.Key
                    select new TypeCountInfo
                    {
                        Id = g.Key,
                        Desc = g.First().JoinType.Description,
                        Count = g.Count(),
                    };
            return q.ToList().Union(Total(q.Sum(t => t.Count)));
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public IEnumerable<TypeCountInfo> DroppedMemberByType(DateTime? dt1, DateTime? dt2)
        {
            if (!dt1.HasValue)
                return null;
            var q = from p in DbUtil.Db.People
                    where p.DropDate >= dt1 && p.DropDate < (dt2 ?? dt1).Value.AddDays(1)
                    group p by p.DropCodeId + "," + p.DropType.Code into g
                    orderby g.Key
                    select new TypeCountInfo
                    {
                        Id = g.Key,
                        Desc = g.First().DropType.Description,
                        Count = g.Count(),
                    };
            return q.ToList().Union(Total(q.Sum(t => t.Count)));
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public IEnumerable<TypeCountInfo> DroppedMemberByChurch(DateTime? dt1, DateTime? dt2)
        {
            if (!dt1.HasValue)
                return null;
            var q0 = from p in DbUtil.Db.People
                     where p.DropDate >= dt1 && p.DropDate < (dt2 ?? dt1).Value.AddDays(1)
                     select p;
            var count = (float)q0.Count();
            var q1 = from p in q0
                     group p by p.OtherNewChurch into g
                     select new TypeCountInfo
                     {
                         Desc = g.Key,
                         Count = g.Count()
                     };
            var q2 = from g in q1
                     let c = (g.Desc == "" || g.Desc == null) ? "Unknown" : g.Desc
                     group g by c into gg
                     select new TypeCountInfo
                     {
                         Desc = gg.Key,
                         Count = gg.Sum(t => t.Count)
                     };
            var q = q2.OrderByDescending(t => t.Count);
            return q.ToList().Union(Total(q.Sum(t => t.Count)));
        }
    }
}
