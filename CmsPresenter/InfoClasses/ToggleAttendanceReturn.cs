using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMSPresenter.InfoClasses
{
    [Serializable]
    public class ToggleAttendanceReturn
    {
        public bool AttendanceFlag;
        public string ControlId;

        //public string numPresent;
        //public string numRecentVisitors;
        //public string numNewVisitors;
        //public string numVisitingMembers;
        //public string numMembers;
        //public string errormsg;
    }
}
