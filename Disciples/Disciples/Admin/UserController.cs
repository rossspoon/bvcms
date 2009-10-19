using System;
using System.Web.Security;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DiscData;
using DiscData.View;
using UtilityExtensions;
using System.Configuration;

[DataObject(true)]
public class UserController
{
    private DiscDataContext Db;
    public UserController()
    {
        Db = new DiscDataContext(ConfigurationManager.ConnectionStrings["Disc"].ConnectionString);
    }

    [DataObjectMethod(DataObjectMethodType.Update, true)]
    public void Update(bool IsApproved, bool MustChangePassword, bool IsLockedOut, int PeopleId, string EmailAddress, string Username, string PasswordSetOnly, int UserId)
    {
        var user = Db.Users.Single(u => u.UserId == UserId);
        if (PasswordSetOnly.HasValue())
        {
            BVMembershipProvider.provider.AdminOverride = true;
            var mu = BVMembershipProvider.provider.GetUser(user.Username, false);
            mu.UnlockUser();
            mu.ChangePassword(mu.ResetPassword(), PasswordSetOnly);
            user.TempPassword = PasswordSetOnly;
            BVMembershipProvider.provider.AdminOverride = false;
        }
        user.Username = Username;
        user.EmailAddress = EmailAddress;
        user.IsApproved = IsApproved;
        user.MustChangePassword = MustChangePassword;
        if (user.IsLockedOut ^ IsLockedOut)
            user.LastLockedOutDate = DateTime.Now;
        user.IsLockedOut = IsLockedOut;
        if (PeopleId > 0)
            user.PeopleId = PeopleId;
        Db.SubmitChanges();
    }
    [DataObjectMethod(DataObjectMethodType.Insert, true)]
    public void Insert(bool IsApproved, bool MustChangePassword, bool IsLockedOut, int PeopleId, string EmailAddress, string Username, string PasswordSetOnly)
    {
        int? pid = null;
        if (PeopleId > 0)
            pid = PeopleId;

        BVMembershipProvider.provider.AdminOverride = true;
        var user = BVMembershipProvider.provider.NewUser(
            Username,
            PasswordSetOnly,
            EmailAddress,
            IsApproved,
            pid);
        BVMembershipProvider.provider.AdminOverride = false;
        user.MustChangePassword = MustChangePassword;
        Db.Users.InsertOnSubmit(user);
        Db.SubmitChanges();
    }

    private int count;
    public int Count(string name, string sortExpression, int startIndex, int maximumRows)
    {
        return count;
    }
    [DataObjectMethod(DataObjectMethodType.Select, false)]
    public IEnumerable<UserList> GetUsers(string name, string sortExpression, int startIndex, int maximumRows)
    {
        var q = from u in Db.ViewUserLists
                select u;

        if (name.HasValue())
            q = q.Where(u => u.LastName.StartsWith(name));
        count = q.Count();

        if (!sortExpression.HasValue())
            sortExpression = "IsOnLine";

        switch (sortExpression)
        {
            case "Pid":
                q = q.OrderBy(u => u.PeopleId).ThenBy(u => u.Username);
                break;
            case "Name":
                q = q.OrderBy(u => u.LastName).ThenBy(u => u.FirstName);
                break;
            case "LastActivityDate":
                q = q.OrderBy(u => u.LastVisit);
                break;
            case "IsApproved":
                q = q.OrderBy(u => u.IsApproved);
                break;
            case "Username":
                q = q.OrderBy(u => u.Username);
                break;
            case "MustChangePassword":
                q = q.OrderBy(u => u.MustChangePassword);
                break;
            case "Pid DESC":
                q = q.OrderByDescending(u => u.PeopleId).ThenByDescending(u => u.Username);
                break;
            case "Name DESC":
                q = q.OrderByDescending(u => u.LastName).ThenByDescending(u => u.FirstName);
                break;
            case "LastActivityDate DESC":
            case "IsOnLine":
            case "IsOnLine DESC":
                q = q.OrderByDescending(u => u.LastVisit);
                break;
            case "IsApproved DESC":
                q = q.OrderByDescending(u => u.IsApproved);
                break;
            case "Username DESC":
                q = q.OrderByDescending(u => u.Username);
                break;
            case "MustChangePassword DESC":
                q = q.OrderByDescending(u => u.MustChangePassword);
                break;
        }
        q = q.Skip(startIndex).Take(maximumRows);

        return q;
    }
    [DataObjectMethod(DataObjectMethodType.Select, true)]
    public IEnumerable<OptOut> FetchOptOuts(string username)
    {
        var q = from bn in DbUtil.Db.BlogNotifications
                let user = bn.Blog.BlogPosts.OrderByDescending(p => p.EntryDate).First().User
                //let user = post != null ? post.User : null
                let poster = user != null ? user.FirstName + " " + user.LastName : ""
                where bn.User.Username == username
                select new OptOut
                {
                    BlogId = bn.BlogId,
                    Checked = true,
                    Title = bn.Blog.Title,
                    Poster = poster
                };
        return q;
    }
    public static void DeleteOptOutOnSubmit(int blogid, int userid)
    {
        var bn = DbUtil.Db.BlogNotifications.Single(n =>
            n.BlogId == blogid && n.UserId == userid);
        DbUtil.Db.BlogNotifications.DeleteOnSubmit(bn);
    }
}
public class OptOut
{
    public bool Checked { get; set; }
    public int BlogId { get; set; }
    public string Title { get; set; }
    public string Poster { get; set; }
}

