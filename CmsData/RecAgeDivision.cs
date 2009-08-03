using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CmsData
{
    public partial class RecAgeDivision
    {
        public DateTime agedate
        {
            get
            {
                var dt = new DateTime[3];
                dt[0] = DateTime.Parse(AgeDate);
                dt[1] = dt[0].AddYears(1);
                dt[2] = dt[0].AddYears(-1);
                var now = DateTime.Now;
                var q = from d in dt
                        orderby Math.Abs(d.Subtract(now).TotalDays)
                        select d;
                var r = q.First();
                return r;
            }
        }
    }
}
