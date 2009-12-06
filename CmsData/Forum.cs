using System;
using System.Web;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using UtilityExtensions;

namespace CmsData
{
    public partial class Forum
    {
        public string GroupName
        {
            get { return Group.LoadById(GroupId.Value).Name; }
        }
        public bool IsAdmin
        {
            get { return Group.LoadById(GroupId.Value).IsAdmin; }
        }
        public bool IsMember
        {
            get { return Group.LoadById(GroupId.Value).IsMember; }
        }
        public ForumEntry NewEntry(string title, string entry)
        {
            return DbUtil.Db.ForumNewEntry(Id, null, title, entry,
                Util.Now, HttpContext.Current.User.Identity.Name).Single();
        }
        public static Forum LoadFromId(int id)
        {
            return DbUtil.Db.Forums.SingleOrDefault(f => f.Id == id);
        }
    }
    public class ForumController
    {
        private static string user
        {
            get { return HttpContext.Current.User.Identity.Name; }
        }
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public IEnumerable<Forum> FetchAllForUser()
        {
            var list = Group.FetchIdsForUser();
            return DbUtil.Db.Forums.Where(f => list.Contains(f.GroupId.Value));
        }
        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public void Update(int Id, string Description)
        {
            var f = Forum.LoadFromId(Id);
            f.Description = Description;
            DbUtil.Db.SubmitChanges();
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public void Delete(int Id)
        {
            var f = Forum.LoadFromId(Id);
            var db = DbUtil.Db;
            foreach (var fe in f.ForumEntries)
                db.ForumUserReads.DeleteAllOnSubmit(fe.ForumUserReads);
            db.ForumEntries.DeleteAllOnSubmit(f.ForumEntries);
            db.Forums.DeleteOnSubmit(f);
            DbUtil.Db.SubmitChanges();
        }
    }
}