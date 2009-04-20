using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilityExtensions;

namespace CmsData
{
    internal static class AuditUtility
    {
        private static void ProcessAuditFields(IList<System.Object> list, bool IsInsert)
        {
            foreach (var item in list)
            {
                var entity = item as IAuditable;
                if (entity != null)
                {
                    var dtNow = Util.Now;
                    if (IsInsert)
                    {
                        entity.CreatedBy = Util.UserId;
                        entity.CreatedDate = dtNow;
                    }
                    else
                    {
                        entity.ModifiedBy = Util.UserId;
                        entity.ModifiedDate = dtNow;
                    }
                }
            }
        }

        internal static void ProcessInserts(IList<System.Object> list)
        {
            ProcessAuditFields(list, true);
        }

        internal static void ProcessUpdates(IList<System.Object> list)
        {
            ProcessAuditFields(list, false);
        }
        //public int[] x = new int[] 
        //{   
        //    2, null,
        //    3, null,
        //    5023, null,
        //    5140, null,

        //    5000, 26287,
        //    5001, 819918,
        //    5002, 850653,
        //    5160, 809025,
        //    5162, 32626,
        //    5700, 812506,
        //    5740, 807302,
        //    5760, 827539,
        //    6044, 46529,
        //    6365, 16901,
        //    11830, 899254
        //};
    }
}