using System;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Security;
using System.Net.Mail;
using UtilityExtensions;

namespace CmsData
{
    public partial class Group
    {
        private const string CONTEXTUSER = "ContextUser";
        private const string BLOGGER = "Blogger";
        private const string ADMIN = "Admin";
        private const string MEMBER = "Member";

        public static User ContextUser
        {
            get
            {
                if (HttpContext.Current.Items.Contains(CONTEXTUSER))
                    return HttpContext.Current.Items[CONTEXTUSER] as User;
                else
                    return DbUtil.Db.CurrentUser;
            }
            set
            {
                if (value == null)
                    HttpContext.Current.Items.Remove(CONTEXTUSER);
                else
                    HttpContext.Current.Items[CONTEXTUSER] = value;
            }
        }

        public static Group LoadById(int id)
        {
            return DbUtil.Db.Groups.SingleOrDefault(m => m.Id == id);
        }
        public static Group LoadByName(string name)
        {
            return DbUtil.Db.Groups.FirstOrDefault(m => m.Name == name);
        }
        public static Group LoadByRole(string role)
        {
            try
            {
                string groupid = Regex.Match(role, "R(\\d+)-.*", RegexOptions.Multiline).Groups[1].Value;
                int id = int.Parse(groupid);
                return DbUtil.Db.Groups.SingleOrDefault(m => m.Id == id);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Bad role name for group", ex);
            }
        }
        public void AddInvitation(string passcode)
        {
            var i = new Invitation();
            i.Password = passcode;
            i.Expires = Util.Now.AddMonths(2);
            Invitations.Add(i);
        }

        public bool HasWelcomeText
        {
            get { return ContentId.HasValue; }
        }

        public void SetAdmin(User user, bool value)
        {
            SetRole(user, ADMIN, value);
        }
        public bool IsAdmin
        {
            get { return IsUserAdmin(ContextUser); }
        }
        public bool IsUserAdmin(User user)
        {
            return IsUserInRole(user, ADMIN);
        }
        public void SetBlogger(User user, bool value)
        {
            SetRole(user, BLOGGER, value);
        }
        public bool IsBlogger
        {
            get { return IsUserBlogger(ContextUser); }
        }
        public bool IsUserBlogger(User user)
        {
            return IsUserInRole(user, BLOGGER);
        }
        public void SetMember(User user, bool value)
        {
            SetRole(user, MEMBER, value);
        }
        public bool IsMember
        {
            get { return IsUserMember(ContextUser); }
        }
        public bool IsUserMember(User user)
        {
            return IsUserInRole(user, MEMBER);
        }
        public bool IsUserInRole(User user, string role)
        {
            return GetRoleUser(user, role) != null;
        }
        public UserGroupRole GetRoleUser(User user, string role)
        {
            if (user == null)
                return null;
            var q = from ru in DbUtil.Db.UserGroupRoles
                    where ru.UserId == user.UserId && ru.GroupRole.RoleName == role
                    where ru.GroupRole.GroupId == this.Id
                    select ru;
            return q.SingleOrDefault();
        }
        public static bool IsUserAdmin(User user, string group)
        {
            var g = LoadByName(group);
            if (g == null)
                return false;
            var q = from ru in DbUtil.Db.UserGroupRoles
                    where ru.UserId == user.UserId && ru.GroupRole.RoleName == ADMIN
                    where ru.GroupRole.GroupId == g.Id
                    select ru;
            return q.SingleOrDefault() != null;
        }
        public static ParaContent GetNewWelcome()
        {
            var w = ContentService.GetContent("default2_welcome");
            var welcome = new ParaContent();
            welcome.Body = w.Body;
            welcome.ContentName = "groupwelcometext";
            if (DbUtil.Db.CurrentUser != null)
                welcome.CreatedById = DbUtil.Db.CurrentUser.UserId;
            welcome.CreatedOn = Util.Now;
            welcome.Title = w.Title;
            DbUtil.Db.ParaContents.InsertOnSubmit(welcome);
            return welcome;
        }
        public static void InsertWithRolesOnSubmit(string name)
        {
            var g = new Group();
            g.Name = name;
            g.CreateRole(MEMBER);
            g.CreateRole(ADMIN);
            g.CreateRole(BLOGGER);
            g.WelcomeText = GetNewWelcome();
            DbUtil.Db.Groups.InsertOnSubmit(g);
        }
        private GroupRole GetRole(string name)
        {
            return GroupRoles.SingleOrDefault(r => r.RoleName == name);
        }
        private void CreateRole(string name)
        {
            if (GetRole(name) != null)
                return;
            var role = new GroupRole();
            role.RoleName = name;
            this.GroupRoles.Add(role);
        }
        public void DeleteWithRoleOnSubmit()
        {
            foreach (var r in GroupRoles)
                DbUtil.Db.UserGroupRoles.DeleteAllOnSubmit(r.UserGroupRoles);
            DbUtil.Db.GroupRoles.DeleteAllOnSubmit(GroupRoles);
            DbUtil.Db.Invitations.DeleteAllOnSubmit(this.Invitations);
            DbUtil.Db.ParaContents.DeleteOnSubmit(this.WelcomeText);
            DbUtil.Db.Groups.DeleteOnSubmit(this);
        }
        private void AddUserToRole(User user, string role)
        {
            var r = GetRole(role);
            var ru = new UserGroupRole();
            ru.UserId = user.UserId;
            r.UserGroupRoles.Add(ru);
        }
        private void RemoveUserFromRole(User user, string role)
        {
            var ru = GetRoleUser(user, role);
            DbUtil.Db.UserGroupRoles.DeleteOnSubmit(ru);
        }
        private void SetRole(User user, string rolename, bool value)
        {
            bool inrole = IsUserInRole(user, rolename);
            if (value && !inrole)
                AddUserToRole(user, rolename);
            else if (!value && inrole)
                RemoveUserFromRole(user, rolename);
        }
        public static IEnumerable<Group> FetchAllGroups()
        {
            return FetchGroups(GroupType.Member, FetchType.AllGroups);
        }
        public static IEnumerable<Group> FetchAllGroupsWhereAdmin()
        {
            return from g in FetchAllGroups()
                   where g.IsAdmin 
                   || HttpContext.Current.User.IsInRole("BlogAdministrator")
                   select g;
        }
        public static IEnumerable<Group> FetchUserGroups()
        {
            return FetchGroups(GroupType.Member, FetchType.UserOnly);
        }
        public static IEnumerable<Group> FetchAdminGroups()
        {
            return FetchGroups(GroupType.Admin, FetchType.UserOnly);
        }
        private static IEnumerable<Group> FetchGroups(GroupType gtype, FetchType FetchType)
        {
            if (FetchType == FetchType.AllGroups)
                return new GroupController().FetchAll();
            string role = GroupPostfix(gtype);
            bool isadmin = HttpContext.Current.User.IsInRole("BlogAdministrator");
            return from g in DbUtil.Db.Groups
                   where //isadmin || 
                        g.GroupRoles.Any(r => r.RoleName == role
                           && r.UserGroupRoles.Any(ur => ur.UserId == ContextUser.UserId))
                   select g;
        }
        public static IEnumerable<int> FetchIdsForUser()
        {
            return FetchIds(GroupType.Member);
        }
        public static IEnumerable<int> FetchIdsForAdmin()
        {
            return FetchIds(GroupType.Admin);
        }
        private static IEnumerable<int> FetchIds(GroupType gtype)
        {
            return FetchGroups(gtype, FetchType.UserOnly).Select(g => g.Id);
        }

        private static string GroupPostfix(GroupType type)
        {
            switch (type)
            {
                case GroupType.Member:
                    return MEMBER;
                case GroupType.Admin:
                    return ADMIN;
                case GroupType.Blogger:
                    return BLOGGER;
                default:
                    return MEMBER;
            }
        }
        public static IEnumerable<User> GetUsersInGroup(int groupid)
        {
            return from u in DbUtil.Db.Users
                   where u.UserGroupRoles.Any(ur => ur.GroupRole.GroupId == groupid)
                   select u;
        }
        public static IEnumerable<User> GetUsersInRole(string role)
        {
            return from u in DbUtil.Db.Users
                   where u.UserRoles.Any(ur => ur.Role.RoleName == role)
                   select u;
        }
        public IEnumerable<User> GetUsersInRole(GroupType gtype)
        {
            var role = GroupPostfix(gtype);
            return from r in GroupRoles
                   from ru in r.UserGroupRoles
                   where r.RoleName == role
                   select ru.User;
        }
        public void NotifyNewUser(string newuserid)
        {
            var n = 0;
            var u = DbUtil.Db.Users.Single(uu => uu.Username == newuserid);
            var subject = "New user in Group: " + Name;
            var body = "<br>--<br>{0}, {1} is a new user in group={3} with id={4} and birthday={5:d}.<br>--<br>"
                    .Fmt(u.EmailAddress, u.Name, "", Name, newuserid, u.Person.BirthDay);
            var from = new MailAddress("bbcms01@bellevue.org");

            foreach (var mu in GetUsersInRole(GroupType.Admin))
                Util.SendMsg(Util.SysFromEmail, Util.CmsHost, from, subject, body, mu.Name, mu.EmailAddress, 0);
        }
    }
    public enum FetchType
    {
        AllGroups,
        UserOnly
    }

    public class UserGroupInfo
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public bool IsMember { get; set; }
        public bool IsBlogger { get; set; }
        public bool IsAdmin { get; set; }
    }
    public class GroupController
    {
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public IEnumerable<Group> FetchAll()
        {
            return DbUtil.Db.Groups;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<Group> FetchAllGroups()
        {
            return Group.FetchAllGroups();
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<Group> FetchAllGroupsWhereAdmin()
        {
            return Group.FetchAllGroupsWhereAdmin();
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<Group> FetchUserGroups()
        {
            return Group.FetchUserGroups();
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<Group> FetchAdminGroups()
        {
            return Group.FetchAdminGroups();
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public void DeleteGroup(string Name)
        {
            var group = DbUtil.Db.Groups.FirstOrDefault(g => g.Name == Name);
            group.DeleteWithRoleOnSubmit();
            DbUtil.Db.SubmitChanges();
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<User> GetUsersInGroup(int id)
        {
            return Group.GetUsersInGroup(id);
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public IEnumerable<UserGroupInfo> GetUsers(int groupid)
        {
            var g = Group.LoadById(groupid);
            var q = from u in DbUtil.Db.Users
                    orderby u.Name2
                    select new UserGroupInfo
                    {
                        Name = u.Name2,
                        UserName = u.Username,
                        IsAdmin = g.IsUserAdmin(u),
                        IsBlogger = g.IsUserBlogger(u),
                        IsMember = g.IsUserMember(u)
                    };
            return q;
        }

    }
    public enum GroupType
    {
        Admin,
        Blogger,
        Member
    }
}