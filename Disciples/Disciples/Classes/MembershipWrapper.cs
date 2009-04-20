using System.ComponentModel;
using System.Collections.Generic;
using System.Web.Security;
using BTeaData;
using System.Linq;
using System;
using BellevueTeachers;

[DataObject(true)]
public class MembershipWrapper
{
    public class UserInfo
    {
        public bool IsApproved { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastActivityDate { get; set; }
        public string Name { get; set; }
    }
    private const string DefaultSort = "lastactivity DESC";
    [DataObjectMethod(DataObjectMethodType.Select, false)]
    public static List<UserInfo> GetAllUsers(string sort, string search)
    {
        if (sort == "")
            sort = DefaultSort;
        var list = new List<UserInfo>();
        var q = DbUtil.Db.Users.Select(u => u);
        foreach (var mu in q)
        {
            mu.LastActivityDate = PageVisit.LastVisit(mu.Username).ConvPST2CST();
            var name = mu.LastName + ", " + mu.FirstName;
            if (search.HasValue())
                search = search.ToLower();
            if (!search.HasValue() || name.ToLower().StartsWith(search) || mu.Username.ToLower() == search)
                list.Add(new UserInfo
                {
                    CreationDate = mu.CreationDate.Value,
                    IsApproved = mu.IsApproved,
                    Email = mu.EmailAddress,
                    LastActivityDate = mu.LastActivityDate.Value,
                    UserName = mu.Username,
                    Name = name,
                });
        }
        list.Sort(new MembershipUserComparer(sort));
        return list;

    }
    [DataObjectMethod(DataObjectMethodType.Delete, true)]
    public bool Delete(string user, string username)
    {
        var q = from ru in DbUtil.Db.UserRoles
                where ru.User.Username == username
                select ru;
        DbUtil.Db.UserRoles.DeleteAllOnSubmit(q);
        DbUtil.Db.SubmitChanges();
        return Membership.DeleteUser(username);
    }
    public class MembershipUserComparer : IComparer<UserInfo>
    {
        string sort;
        int dir = 1;
        public MembershipUserComparer(string sort)
        {
            if (sort == "")
                return;
            var a = sort.Split(new char[] { ' ' });
            this.sort = a[0];
            if (a.Length == 2 && a[1] == "DESC")
                dir = -1;
        }
        int IComparer<UserInfo>.Compare(UserInfo x, UserInfo y)
        {
            switch (sort)
            {
                case "lastactivity":
                    return x.LastActivityDate.CompareTo(y.LastActivityDate) * -1;
                case "user":
                    return x.UserName.CompareTo(y.UserName) * dir;
                case "created":
                    return x.CreationDate.CompareTo(y.CreationDate) * dir;
                case "email":
                    return x.Email.CompareTo(y.Email) * dir;
                case "name":
                    return x.Name.CompareTo(y.Name) * dir;
            }
            return 0;
        }
    }
}
