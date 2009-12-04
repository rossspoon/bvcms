using System;
using System.Web;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;

namespace DiscData
{
    public partial class VerseCategory
    {
        public VerseCategory CopyWithVerses()
        {
            var cc = new VerseCategory();
            cc.Name = " A new copy of " + Name;
            cc.CreatedBy = DbUtil.Db.CurrentUser.UserId;
            cc.CreatedOn = Util.Now;
            DbUtil.Db.VerseCategories.InsertOnSubmit(cc);
            foreach (VerseCategoryXref x1 in this.VerseCategoryXrefs)
            {
                var x2 = new VerseCategoryXref();
                x2.VerseId = x1.VerseId;
                cc.VerseCategoryXrefs.Add(x2);
            }
            return cc;
        }
        public static VerseCategory LoadByName(string name, string owner)
        {
            return DbUtil.Db.VerseCategories
                .SingleOrDefault(cat => cat.User.Username == owner && cat.Name == name);
        }
        public static VerseCategory LoadById(int id)
        {
            return DbUtil.Db.VerseCategories
                .SingleOrDefault(cat => cat.Id == id);
        }
        public bool HasVerse(int VerseId)
        {
            return this.VerseCategoryXrefs.Count(vx => vx.VerseId == VerseId) > 0;
        }
        public void AddVerse(Verse v)
        {
            var x = VerseCategoryXref.Load(this.Id, v.Id);
            if (x == null)
            {
                x = new VerseCategoryXref();
                x.Verse = v;
                this.VerseCategoryXrefs.Add(x);
            }
        }

    }
    [DataObject]
    public class VerseCategoryController
    {
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public void Insert(string Name)
        {
            var c = new VerseCategory();
            c.Name = Name;
            c.CreatedBy = DbUtil.Db.CurrentUser.UserId;
            c.CreatedOn = Util.Now;
            DbUtil.Db.VerseCategories.InsertOnSubmit(c);
            DbUtil.Db.SubmitChanges();
        }
        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public void Update(string Name, int id)
        {
            var c = VerseCategory.LoadById(id);
            c.Name = Name;
            c.ModifiedOn = Util.Now;
            DbUtil.Db.SubmitChanges();
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public void DeleteDeep(int id)
        {
            var c = VerseCategory.LoadById(id);
            DbUtil.Db.VerseCategoryXrefs.DeleteAllOnSubmit(c.VerseCategoryXrefs);
            DbUtil.Db.VerseCategories.DeleteOnSubmit(c);
            DbUtil.Db.SubmitChanges();
        }
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public IEnumerable<View.VerseCategoriesView> GetCategoriesForOwner(bool includeAdmin, string Owner)
        {
            var owners = new List<string>();
            owners.Add(Owner);
            if (includeAdmin)
            {
                owners.Clear();
                var u = DbUtil.Db.GetUser(Owner);
                var q = from uu in DbUtil.Db.Users
                        from ur in uu.UserRoles
                        where ur.Role.UserRoles.Any(rr => rr.UserId == u.UserId)
                        group ur by ur.User into g
                        select g.Key.Username;
                owners.AddRange(q);
                owners.Add("admin");
            }
            if(DbUtil.Db.ViewVerseCategoriesViews.Count(c => c.Username == Owner) == 0)
            {
                var cc = VerseCategory.LoadByName("Starter Set", "admin").CopyWithVerses();
                cc.Name = "Starter Set";
                cc.CreatedBy = DbUtil.Db.GetUser(Owner).UserId;
                cc.CreatedOn = Util.Now;
                DbUtil.Db.SubmitChanges();
            }
            return from cv in DbUtil.Db.ViewVerseCategoriesViews
                   where owners.Contains(cv.Username)
                   where !(cv.Name == "Starter Set" && cv.Username != Owner && cv.Username != "Admin")
                   orderby cv.Username, cv.DisplayName
                   select cv;
        }
    }
}