using System;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using System.Web;

namespace BTeaData
{
    [DataObject]
    public class VerseSummaryController
    {
        private int count = 0;

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public IEnumerable<View.VerseSummaryForCategory2> Select(
            int startRowIndex, int maximumRows)
        {
            int? cat = HttpContext.Current.Request.QueryString<int?>("cat");
            var q = from vs in DbUtil.Db.VerseSummaryForCategory2(cat.Value)
                    orderby vs.Book, vs.Chapter, vs.VerseNum
                    select vs;
            count = q.Count();
            return q.Skip(startRowIndex).Take(maximumRows);
        }
        public int GetCount(int startRowIndex, int maximumRows)
        {
            return count;
        }
    }
}
