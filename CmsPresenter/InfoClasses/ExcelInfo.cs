using System;

namespace CMSPresenter
{
    public class ExcelInfo : PersonInfo
    {
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string School { get; set; }
        public string Grade { get; set; }
        public decimal AttendPct { get; set; }
        public string Title { get; set; }
    }
}
