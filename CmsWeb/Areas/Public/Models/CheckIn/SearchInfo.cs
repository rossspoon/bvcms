using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UtilityExtensions;

namespace CmsWeb.Models
{
    public class SearchInfo2
    {
        public string first { get; set; }
        public string last { get; set; }
        public string goesby { get; set; }
        public string dob { get; set; }
        public string cell { get; set; }
        public string home { get; set; }
        public string email { get; set; }
        public string addr { get; set; }
        public string zip { get; set; }
        public int gender { get; set; }
        public int marital { get; set; }
        public int? age { get; set; }
        public int fid { get; set; }
        public int pid { get; set; }

        public string allergies { get; set; }
        public int? grade { get; set; }
        public string parent { get; set; }
        public string emfriend { get; set; }
        public string emphone { get; set; }
        public bool activeother { get; set; }

        public string GetDisplay()
        {
            return "{0} {1}({2}) {3} {4} / {5}".Fmt(
                       goesby.HasValue() ? goesby : first, 
                       last, 
                       age, 
                       Util.FmtFone7(home, "h"), 
                       Util.FmtFone7(cell,"c"), 
                       addr);
        }
        public string church { get; set; }
    }
}
