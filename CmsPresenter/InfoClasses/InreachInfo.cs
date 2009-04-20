using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMSPresenter.InfoClasses
{
    public class InReachInfo : PastAttendeeInfo
    {
        public string ContactNotes{ get; set;}
        public string DivisionName { get; set; }
        public string OrganizationName { get; set; }
        public int OrganizationId { get; set; }
    }

}
