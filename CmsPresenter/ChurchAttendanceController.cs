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
    public class ChurchAttendanceController : ChurchAttendanceConstants
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
            public int ProgramId { get; set; }
        }
        private List<MeetInfo> qlist;
        private void LoadMeetings(DateTime sunday)
        {
            if (qlist != null)
                return;
            string misctag = DbUtil.MiscTagsString;
            var q = from m in DbUtil.Db.Meetings
                    where m.MeetingDate.Value.Date == sunday
                    where m.NumPresent > 0
                    select new MeetInfo
                    {
                        OrganizationId = m.OrganizationId,
                        NumPresent = m.NumPresent,
                        MeetingDate = m.MeetingDate.Value,
                        ProgramId = m.Organization.DivOrgs.First(t => t.Division.Program.Name != misctag).Division.ProgId.Value
                    };
            qlist = q.ToList();
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<WorshipAttendInfo> ChurchAttendance(DateTime sunday)
        {
            var sunday1200 = sunday.AddHours(12);
            LoadMeetings(sunday);

            var q2 = from m in qlist
                     where m.MeetingDate < sunday1200
                     where MorningWorship.Contains(m.OrganizationId)
                         || ExtendedSessions.Contains(m.OrganizationId)
                         || ChoirTags.Contains(m.ProgramId)
                     let m1 = ExtendedSessions.Contains(m.OrganizationId) ? "dExtended Session" :
                        m.ProgramId == VocalTagId ? "bChoir" :
                        m.ProgramId == OrchestraTagId ? "cOrchestra" :
                        "a" + m.MeetingDate.TimeOfDay.Hours.ToString().PadLeft(2, ' ') + ":" + m.MeetingDate.Minute.ToString().PadLeft(2, '0') + " AM"
                     group m by m1 into g
                     orderby g.Key
                     select new WorshipAttendInfo
                     {
                         Name = g.Key.Substring(1),
                         Count = g.Sum(m => m.NumPresent),
                     };
            var list = q2.ToList();
            if (list.Count > 0)
                list.Add(new WorshipAttendInfo
                {
                    Name = "Total",
                    Count = q2.Sum(i => i.Count)
                });
            int pm = qlist.Where(m => EveningWorship.Contains(m.OrganizationId)).Sum(m => m.NumPresent);
            if (pm > 0)
            {
                list.Add(new WorshipAttendInfo { Name = "&nbsp;" });
                list.Add(new WorshipAttendInfo
                {
                    Name = "6:00 P.M.",
                    Count = pm
                });
            }

            return list;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<WorshipAttendInfo> BFCAttendance(DateTime sunday)
        {
            LoadMeetings(sunday);
            var q2 = from m in qlist
                     where m.ProgramId == BFCProgramTagId
                     group m by m.MeetingDate into g
                     orderby g.Key
                     select new WorshipAttendInfo
                     {
                         Name = g.Key.Hour + ":" + g.Key.Minute.ToString().PadLeft(2, '0') + " A.M.",
                         Count = g.Sum(m => m.NumPresent)
                     };
            var list = q2.ToList();
            if (list.Count > 0)
                list.Add(new WorshipAttendInfo
                {
                    Name = "Total",
                    Count = q2.Sum(i => i.Count)
                });
            return list;
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

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<WorshipAttendInfo> WednesdayAttendance(DateTime sunday)
        {
            var wednesday = sunday.AddDays(-4).AddHours(17);

            var q3 = from m in DbUtil.Db.Meetings
                     where m.MeetingDate.Value.Date == wednesday.Date
                     where m.MeetingDate.Value >= wednesday
                     where m.NumPresent > 0
                     let div = m.Organization.DivOrgs.First(t => t.Division.Program.Name != DbUtil.MiscTagsString).Division
                     group m by
                        WedWorship.Contains(m.OrganizationId) ? "Prayer and Praise Service" :
                        div.Id == CrownTagId ? "Crown Ministry" :
                        div.Id == StringsId ? "Strings Group Class" :
                        div.Id == WedBFSmallGroups ? "BF Small Groups" :
                        discipleLifeTags.Contains(div.Id) ? "Disciple Life" :
                        div.ProgId == VocalTagId ? "Adult Choir" :
                        div.ProgId == OrchestraTagId ? "Orchestra" :
                        div.ProgId == GradedChoirTagId ? "Graded Choirs" :
                        div.ProgId == CareTagId ? "Care" :
                        m.Organization.OrganizationName into g
                     orderby g.Key
                     select new WorshipAttendInfo
                     {
                         Name = g.Key,
                         Count = g.Sum(m => m.NumPresent)
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
