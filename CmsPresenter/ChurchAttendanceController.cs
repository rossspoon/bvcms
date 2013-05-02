/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Linq;
using System.Text;
using CmsData;
using CmsData.View;
using CMSPresenter.InfoClasses;
using UtilityExtensions;
using System.Data.Linq.SqlClient;

namespace CMSPresenter
{
    [DataObject]
    public class ChurchAttendanceController 
    {

        public class WorshipAttendInfo
        {
            public int? Count { get; set; }
            public string Name { get; set; }

        }

        private class MeetInfo
        {
            public int OrganizationId { get; set; }
            public int NumPresent { get; set; }
            public DateTime MeetingDate { get; set; }
            public string Name { get; set; }
            public int? ProgramId { get; set; }
        }
        private List<MeetInfo> qlist;
        private void LoadMeetings(DateTime sunday)
        {
            if (qlist != null)
                return;
            var q = from m in DbUtil.Db.Meetings
                    where m.MeetingDate.Value.Date == sunday
                    where m.NumPresent > 0
                    select new MeetInfo
                    {
                        OrganizationId = m.OrganizationId,
                        Name = m.Organization.OrganizationName,
                        NumPresent = m.NumPresent,
                        MeetingDate = m.MeetingDate.Value,
                        ProgramId = m.Organization.Division.ProgId.Value
                    };
            qlist = q.ToList();
        }

        public IEnumerable<WorshipAttendInfo> Baptisms(DateTime sunday)
        {
            var dt1 = sunday.AddDays(-4);
            var dt2 = sunday.AddDays(2);
            var q3 = from p in DbUtil.Db.People
                     where p.BaptismDate >= dt1 && p.BaptismDate <= dt2
                     group p by p.BaptismType.Description into g
                     orderby g.Key
                     select new WorshipAttendInfo
                     {
                         Name = g.Key,
                         Count = g.Count()
                     };
            var list = q3.ToList();
            if (list.Count > 0)
                list.Add(new WorshipAttendInfo
                {
                    Name = "Total",
                    Count = q3.Sum(i => i.Count)
                });
            return list;
        }

        public IEnumerable<WorshipAttendInfo> Decisions(DateTime sunday)
        {
            var dt1 = sunday.AddDays(-4);
            var dt2 = sunday.AddDays(2);
            var q3 = from p in DbUtil.Db.People
                     where p.DecisionDate >= dt1 && p.DecisionDate <= dt2
                     group p by p.DecisionType.Description into g
                     orderby g.Key
                     select new WorshipAttendInfo
                     {
                         Name = g.Key,
                         Count = g.Count()
                     };
            var list = q3.ToList();
            if (list.Count > 0)
                list.Add(new WorshipAttendInfo
                {
                    Name = "Total",
                    Count = q3.Sum(i => i.Count)
                });
            return list;
        }
    }
}
