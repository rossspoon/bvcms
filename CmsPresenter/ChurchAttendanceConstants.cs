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
                        "WorshipGroundFloor930", 
                        "WorshipBalcony1100", 
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
    }
}
