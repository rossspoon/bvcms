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
        int? _CareTagId;
        public int CareTagId
        {
            get
            {
                if (!_CareTagId.HasValue)
                {
                    var q = from i in DbUtil.Db.ChurchAttReportIds where i.Name == "CareTagId" select i.Id;
                    _CareTagId = q.SingleOrDefault();
                }
                return _CareTagId.Value;
            }
        }
        private int? _VocalTagId;
        public int VocalTagId
        {
            get
            {
                if (!_VocalTagId.HasValue)
                {
                    var q = from i in DbUtil.Db.ChurchAttReportIds where i.Name == "VocalTagId" select i.Id;
                    _VocalTagId = q.SingleOrDefault();
                }
                return _VocalTagId.Value;
            }
        }
        private int? _OrchestraTagId;
        public int OrchestraTagId
        {
            get
            {
                if (!_OrchestraTagId.HasValue)
                {
                    var q = from i in DbUtil.Db.ChurchAttReportIds where i.Name == "OrchestraTagId" select i.Id;
                    _OrchestraTagId = q.SingleOrDefault();
                }
                return _OrchestraTagId.Value;
            }
        }
        private int? _GradedChoirTagId;
        public int GradedChoirTagId
        {
            get
            {
                if (!_GradedChoirTagId.HasValue)
                {
                    var q = from i in DbUtil.Db.ChurchAttReportIds where i.Name == "GradedChoirTagId" select i.Id;
                    _GradedChoirTagId = q.SingleOrDefault();
                }
                return _GradedChoirTagId.Value;
            }
        }
        private int? _CrownTagId;
        public int CrownTagId
        {
            get
            {
                if (!_CrownTagId.HasValue)
                {
                    var q = from i in DbUtil.Db.ChurchAttReportIds where i.Name == "CrownTagId" select i.Id;
                    _CrownTagId = q.SingleOrDefault();
                }
                return _CrownTagId.Value;
            }
        }
        private int? _StringsId;
        public int StringsId
        {
            get
            {
                if (!_StringsId.HasValue)
                {
                    var q = from i in DbUtil.Db.ChurchAttReportIds where i.Name == "StringsId" select i.Id;
                    _StringsId = q.SingleOrDefault();
                }
                return _StringsId.Value;
            }
        }
        private int? _WedBFSmallGroups;
        public int WedBFSmallGroups
        {
            get
            {
                if (!_WedBFSmallGroups.HasValue)
                {
                    var q = from i in DbUtil.Db.ChurchAttReportIds where i.Name == "WedBFSmallGroups" select i.Id;
                    _WedBFSmallGroups = q.SingleOrDefault();
                }
                return _WedBFSmallGroups.Value;
            }
        }
        private int? _BFCProgramTagId;
        public int BFCProgramTagId
        {
            get
            {
                if (!_BFCProgramTagId.HasValue)
                {
                    var q = from i in DbUtil.Db.ChurchAttReportIds where i.Name == "BFCProgramTagId" select i.Id;
                    _BFCProgramTagId = q.SingleOrDefault();
                }
                return _BFCProgramTagId.Value;
            }
        }
        private int[] _discipleLifeTags;
        public int[] discipleLifeTags
        {
            get
            {
                if (_discipleLifeTags == null)
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
                    _discipleLifeTags = q.ToArray();
                }
                return _discipleLifeTags;
            }
        }
        private int[] _MorningWorship;
        public int[] MorningWorship
        {
            get
            {
                if (_MorningWorship == null)
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
                    _MorningWorship = q.ToArray();
                }
                return _MorningWorship;
            }
        }
        private int[] _EveningWorship;
        public int[] EveningWorship
        {
            get
            {
                if (_EveningWorship == null)
                {
                    var names = new string[] 
                    { 
                        "WorshipBalcony600", 
                        "WorshipGroundFloor600",
                    };
                    var q = from i in DbUtil.Db.ChurchAttReportIds
                            where names.Contains(i.Name)
                            select i.Id;
                    _EveningWorship = q.ToArray();
                }
                return _EveningWorship;
            }
        }
        private int[] _ExtendedSessions;
        public int[] ExtendedSessions
        {
            get
            {
                if (_ExtendedSessions == null)
                {
                    var names = new string[] 
                    { 
                        "ExtendedSession930", 
                        "ExtendedSession1100",
                    };
                    var q = from i in DbUtil.Db.ChurchAttReportIds
                            where names.Contains(i.Name)
                            select i.Id;
                    _ExtendedSessions = q.ToArray();
                }
                return _ExtendedSessions;
            }
        }
        private int[] _ChoirTags;
        public int[] ChoirTags
        {
            get
            {
                if (_ChoirTags == null)
                {
                    var names = new string[] 
                    { 
                        "VocalTagId", 
                        "OrchestraTagId",
                    };
                    var q = from i in DbUtil.Db.ChurchAttReportIds
                            where names.Contains(i.Name)
                            select i.Id;
                    _ChoirTags = q.ToArray();
                }
                return _ChoirTags;
            }
        }
        private int[] _WedWorship;
        public int[] WedWorship
        {
            get
            {
                if (_WedWorship == null)
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
                    _WedWorship = q.ToArray();
                }
                return _WedWorship;
            }
        }
        public int[] guestAttendanceTypes = new int[] 
        { 
            (int)CmsData.Attend.AttendTypeCode.NewVisitor, 
            (int)CmsData.Attend.AttendTypeCode.RecentVisitor 
        };
        private int[] _guestsOutOfTown;
        public int[] guestsOutOfTown
        {
            get
            {
                if (_guestsOutOfTown == null)
                {
                    var names = new string[] 
                    { 
                        "NonResGuests1100", 
                        "NonResGuests930" 
                    };
                    var q = from i in DbUtil.Db.ChurchAttReportIds
                            where names.Contains(i.Name)
                            select i.Id;
                    _guestsOutOfTown = q.ToArray();
                }
                return _guestsOutOfTown;
            }
        }
        private int[] _GuestCentralOutsideOrgs;
        public int[] GuestCentralOutsideOrgs
        {
            get
            {
                if (_GuestCentralOutsideOrgs == null)
                {
                    var names = new string[] 
                    { 
                        "GuestCentralNonRes1100", 
                        "GuestCentralNonRes930" 
                    };
                    var q = from i in DbUtil.Db.ChurchAttReportIds
                            where names.Contains(i.Name)
                            select i.Id;
                    _GuestCentralOutsideOrgs = q.ToArray();
                }
                return _GuestCentralOutsideOrgs;
            }
        }
        private int[] _GuestCentralMetroOrgs;
        public int[] GuestCentralMetroOrgs
        {
            get
            {
                if (_GuestCentralMetroOrgs == null)
                {
                    var names = new string[] 
                    { 
                        "GuestCentralMetro930", 
                        "GuestCentralMetro1100" 
                    };
                    var q = from i in DbUtil.Db.ChurchAttReportIds
                            where names.Contains(i.Name)
                            select i.Id;
                    _GuestCentralMetroOrgs = q.ToArray();
                }
                return _GuestCentralMetroOrgs;
            }
        }
        protected CMSDataContext Db = DbUtil.Db;
        public DateTime MostRecentAttendedSunday()
        {
            var q = from m in DbUtil.Db.Meetings
                    where m.MeetingDate.Value.Date.DayOfWeek == 0
                    where MorningWorship.Contains(m.OrganizationId)
                    where m.NumPresent > 0
                    orderby m.MeetingDate descending
                    select m.MeetingDate.Value.Date;
            var dt = q.FirstOrDefault();
            return dt == DateTime.MinValue ? DateTime.Today : dt;
        }
    }
}
