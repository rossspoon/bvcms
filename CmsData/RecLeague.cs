using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilityExtensions;
using System.Text.RegularExpressions;

namespace CmsData
{
    public partial class RecLeague
    {
        public DateTime agedate
        {
            get
            {
                if (Regex.IsMatch(AgeDate, @"\A(?:\A(0?[1-9]|1[012])[-/](0?[1-9]|[12][0-9]|3[01])\s*\z)\Z"))
                {
                    var dt = new DateTime[3];
                    dt[0] = DateTime.Parse(AgeDate);
                    dt[1] = dt[0].AddYears(1);
                    dt[2] = dt[0].AddYears(-1);
                    var now = Util.Now;
                    var q = from d in dt
                            orderby Math.Abs(d.Subtract(now).TotalDays)
                            select d;
                    var r = q.First();
                    return r;
                }
                return DateTime.Parse(AgeDate);
            }
        }
    }
}
