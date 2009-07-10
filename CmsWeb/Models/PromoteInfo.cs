using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMSWeb.Models
{
    public class PromoteInfo
    {
        public bool IsSelected { get; set; }
        public int PeopleId { get; set; }
        public string Name { get; set; }
        public int CurrClassId { get; set; }
        public string CurrClassName { get; set; }
        public int? PendingClassId { get; set; }
        public string PendingClassName { get; set; }
        public decimal? AttendPct { get; set; }
        public string AttendIndicator { get; set; }
        public string Gender { get; set; }
        public string Birthday { get; set; }
    }
}
