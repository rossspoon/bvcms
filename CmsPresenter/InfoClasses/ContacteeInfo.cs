using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMSPresenter.InfoClasses
{
    public class ContacteeInfo
    {
        public int ContactId { get; set; }
        public int PeopleId { get; set; }
        public string Name { get; set; }
        public bool? ProfessionOfFaith { get; set; }
        public bool? PrayedForPerson { get; set; }
        public int? TaskId { get; set; }
        public bool HasTask { get { return TaskId.HasValue; } }
        public bool NoTask { get { return !TaskId.HasValue; } }
    }
}
