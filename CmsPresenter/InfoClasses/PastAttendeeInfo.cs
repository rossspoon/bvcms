using System;
using UtilityExtensions;

namespace CMSPresenter
{
    public class PastAttendeeInfo : TaggedPersonInfo
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Street { get; set; }
        public string Street2 { get;set; }
        public string EmailHome { get; set; }
        public string Phone { get; set; }
        public string Birthday { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public int? AttendCt { get; set; }
        public DateTime? LastAttend { get; set; }
        public string Status { get; set; } //attendance type
        public string AttendStr { get; set; }
        public decimal? AttendPct { get; set; }
        public string Fullname { get; set; }

    }
}
