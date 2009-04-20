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

namespace CMSPresenter
{
    public class ChurchAttendanceConstants
    {
        public int CareTagId
        {
            get
            {
                var q = from i in DbUtil.Db.ChurchAttReportIds where i.Name == "CareTagId" select i.Id;
                return q.SingleOrDefault();
            }
        }
        public int VocalTagId
        {
            get
            {
                var q = from i in DbUtil.Db.ChurchAttReportIds where i.Name == "VocalTagId" select i.Id;
                return q.SingleOrDefault();
            }
        }
        public int OrchestraTagId
        {
            get
            {
                var q = from i in DbUtil.Db.ChurchAttReportIds where i.Name == "OrchestraTagId" select i.Id;
                return q.SingleOrDefault();
            }
        }
        public int GradedChoirTagId
        {
            get
            {
                var q = from i in DbUtil.Db.ChurchAttReportIds where i.Name == "GradedChoirTagId" select i.Id;
                return q.SingleOrDefault();
            }
        }
        public int CrownTagId
        {
            get
            {
                var q = from i in DbUtil.Db.ChurchAttReportIds where i.Name == "CrownTagId" select i.Id;
                return q.SingleOrDefault();
            }
        }
        public int StringsId
        {
            get
            {
                var q = from i in DbUtil.Db.ChurchAttReportIds where i.Name == "StringsId" select i.Id;
                return q.SingleOrDefault();
            }
        }
        public int WedBFSmallGroups
        {
            get
            {
                var q = from i in DbUtil.Db.ChurchAttReportIds where i.Name == "WedBFSmallGroups" select i.Id;
                return q.SingleOrDefault();
            }
        }
        public int BFCProgramTagId
        {
            get
            {
                var q = from i in DbUtil.Db.ChurchAttReportIds where i.Name == "BFCProgramTagId" select i.Id;
                return q.SingleOrDefault();
            }
        }
        public int[] discipleLifeTags
        {
            get
            {
                var names = new string[]
                { 
                    "DiscipleLifeAdultsTagId", 
                    "DiscipleLifeStudentsTagId", 
                    "DiscipleLifeChildrenTagId", 
                    "StudentMiscTagId", 
                };
                var q = from i in DbUtil.Db.ChurchAttReportIds
                        where names.Contains(i.Name)
                        select i.Id;
                return q.ToArray();
            }
        }
        public int[] MorningWorship
        {
            get
            {
                var names = new string[]
                { 
                    "WorshipBalcony930", 
                    "ChildrensWorship930Gr12", 
                    "ChildrensWorship930K4", 
                    "ChildrensWorship930K5", 
                    "WorshipGroundFloor930", 
                    "WorshipBalcony1100", 
                    "ChildrensWorship1100Gr12", 
                    "ChildrensWorship1100K4", 
                    "ChildrensWorship1100K5", 
                    "WorshipGroundFloor1100", 
                };
                var q = from i in DbUtil.Db.ChurchAttReportIds
                        where names.Contains(i.Name)
                        select i.Id;
                return q.ToArray();
            }
        }
        public int[] EveningWorship
        {
            get
            {
                var names = new string[] 
                { 
                    "WorshipBalcony600", 
                    "WorshipGroundFloor600",
                };
                var q = from i in DbUtil.Db.ChurchAttReportIds
                        where names.Contains(i.Name)
                        select i.Id;
                return q.ToArray();
            }
        }
        public int[] ExtendedSessions
        {
            get
            {
                var names = new string[] 
                { 
                    "ExtendedSession930", 
                    "ExtendedSession1100",
                };
                var q = from i in DbUtil.Db.ChurchAttReportIds
                        where names.Contains(i.Name)
                        select i.Id;
                return q.ToArray();
            }
        }
        public int[] ChoirTags
        {
            get
            {
                var names = new string[] 
                { 
                    "VocalTagId", 
                    "OrchestraTagId",
                };
                var q = from i in DbUtil.Db.ChurchAttReportIds
                        where names.Contains(i.Name)
                        select i.Id;
                return q.ToArray();
            }
        }
        public int[] WedWorship
        {
            get
            {
                var names = new string[] 
                { 
                    "WedBalcony630", 
                    "WedGroundFloor630",
                    "WedFellowshipHall630",
                };
                var q = from i in DbUtil.Db.ChurchAttReportIds
                        where names.Contains(i.Name)
                        select i.Id;
                return q.ToArray();
            }
        }
        public int[] guestAttendanceTypes = new int[] 
        { 
            (int)CmsData.Attend.AttendTypeCode.NewVisitor, 
            (int)CmsData.Attend.AttendTypeCode.RecentVisitor 
        };
        public int[] guestsOutOfTown
        {
            get
            {
                var names = new string[] 
                { 
                    "NonResGuests1100", 
                    "NonResGuests930" 
                };
                var q = from i in DbUtil.Db.ChurchAttReportIds
                        where names.Contains(i.Name)
                        select i.Id;
                return q.ToArray();
            }
        }
        public int[] GuestCentralOutsideOrgs
        {
            get
            {
                var names = new string[] 
                { 
                    "GuestCentralNonRes1100", 
                    "GuestCentralNonRes930" 
                };
                var q = from i in DbUtil.Db.ChurchAttReportIds
                        where names.Contains(i.Name)
                        select i.Id;
                return q.ToArray();
            }
        }
        public int[] GuestCentralMetroOrgs
        {
            get
            {
                var names = new string[] 
                { 
                    "GuestCentralMetro930", 
                    "GuestCentralMetro1100" 
                };
                var q = from i in DbUtil.Db.ChurchAttReportIds
                        where names.Contains(i.Name)
                        select i.Id;
                return q.ToArray();
            }
        }
        protected CMSDataContext Db = DbUtil.Db;
        public bool HasData(DateTime sunday)
        {
            var q = from m in DbUtil.Db.Meetings
                    where m.MeetingDate.Value.Date == sunday
                    where MorningWorship.Contains(m.OrganizationId)
                    select m.NumPresent;
            var list = q.ToList();
            return list.Sum() > 0;
        }
    }
}
