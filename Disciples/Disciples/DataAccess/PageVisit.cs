using System;
using System.Web.Security;
using System.Linq;

namespace BTeaData
{
    public partial class PageVisit
    {
        //public static DateTime LastVisit(string user)
        //{
        //    var db = DbUtil.Db;
        //    var q = from pv in db.PageVisits
        //            where pv.UserId == user
        //            select pv.VisitTime;
        //    var vt = q.Max();
        //    if (!vt.HasValue)
        //    {
        //        var mu = Util.GetUser(user);
        //        vt = mu.LastLoginDate;
        //    }
        //    return vt.Value;
        //}
        //public DateTime VisitTimeCST
        //{
        //    get
        //    {
        //        if (VisitTime.HasValue)
        //            return VisitTime.Value.ConvPST2CST();
        //        else
        //            return CreatedOn.ConvPST2CST();
        //    }
        //}
    } 
}