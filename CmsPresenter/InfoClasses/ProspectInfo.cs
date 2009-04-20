using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMSPresenter.InfoClasses
{
    public class ProspectInfo : TaggedPersonInfo
    {
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public string PositionInFamily { get; set;}
        public string Origin { get; set; }
        public string BFCStatus { get; set; }
        public string Comment { get; set; }
        public string ChristAsSavior { get; set; }
        public string InterestedInJoining { get; set; }
        public string InfoBecomeAChristian { get; set; }
        public string PleaseVisit { get; set; }

    }
}
