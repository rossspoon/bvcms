using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UtilityExtensions;

namespace CMSWeb.Models
{
    public class SearchInfo
    {
        public string Name { get; set; }
        public int? Age { get; set; }
        public string Address { get; set; }
        public string CellPhone { get; set; }
        public string HomePhone { get; set; }
        public string GetDisplay()
        {
            return "{0}({1}) {2} {3} / {4}".Fmt(Name, Age, Util.FmtFone7(HomePhone, "h"), Util.FmtFone7(CellPhone,"c"), Address);
        }
    }
}
