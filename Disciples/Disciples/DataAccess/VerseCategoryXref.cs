using System;
using System.Linq;

namespace BTeaData
{
    public partial class VerseCategoryXref
    {
        public static void DeleteOnSubmit(int VerseCategoryId, int VerseId)
        {
            var cx = Load(VerseCategoryId, VerseId);
            DbUtil.Db.VerseCategoryXrefs.DeleteOnSubmit(cx);
        }
        public static void InsertOnSubmit(int VerseCategoryId, int VerseId)
        {
            var cx = new VerseCategoryXref();
            cx.VerseCategoryId = VerseCategoryId;
            cx.VerseId = VerseId;
            DbUtil.Db.VerseCategoryXrefs.InsertOnSubmit(cx);
        }
        public static VerseCategoryXref Load(int catid, int verseid)
        {
            return DbUtil.Db.VerseCategoryXrefs
                .SingleOrDefault(cx => cx.VerseCategoryId == catid && cx.VerseId == verseid);
        }
    }
}