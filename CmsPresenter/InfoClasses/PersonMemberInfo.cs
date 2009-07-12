using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMSPresenter
{
    public class PersonMemberInfo : TaggedPersonInfo
    {
        public string MemberType { get; set; }
        public string MemberTypeCode { get; set; }
        public int MemberTypeId { get; set; }
        public DateTime? InactiveDate { get; set; }
        public decimal? AttendPct { get; set; }
        public DateTime? LastAttended { get; set; }
        public DateTime? Joined { get; set; }
        public DateTime? Dropped { get; set; }
        public GroupSelect FromTab { get; set; }
    }
}
