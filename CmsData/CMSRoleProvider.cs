/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Web.Security;
using System.Collections.Specialized;

using UtilityExtensions;
using System.Linq;
using CmsData;
using System.Web;
using System.Web.Caching;
using System.Collections.Generic;

namespace CmsData
{
    public class CMSRoleProvider : RoleProvider
    {
        public static CMSRoleProvider provider
        {
            get { return Roles.Provider as CMSRoleProvider; }
        }
        public override string ApplicationName { get { return "cms"; } set { } }
        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            if (name == null || name.Length == 0)
                name = "CMSRoleProvider";
            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "CMS Role provider");
            }
            base.Initialize(name, config);
        }

        public override void AddUsersToRoles(string[] usernames, string[] rolenames)
        {
            var Db = DbUtil.Db;
            var qu = Db.Users.Where(u => usernames.Contains(u.Username));
            var qr = Db.Roles.Where(r => rolenames.Contains(r.RoleName));
            foreach (var user in qu)
                foreach (var role in qr)
                    user.UserRoles.Add(new UserRole { Role = role });
            Db.SubmitChanges();
        }
        public override void CreateRole(string rolename)
        {
            var Db = DbUtil.Db;
            Db.Roles.InsertOnSubmit(new Role { RoleName = rolename });
            Db.SubmitChanges();
        }

        public override bool DeleteRole(string rolename, bool throwOnPopulatedRole)
        {
            var Db = DbUtil.Db;
            var role = Db.Roles.Single(r => r.RoleName == rolename);
            Db.UserRoles.DeleteAllOnSubmit(role.UserRoles);
            Db.Roles.DeleteOnSubmit(role);
            Db.SubmitChanges();
            return true;
        }

        public override string[] GetAllRoles()
        {
            var Db = DbUtil.Db;
            return Db.Roles.Select(r => r.RoleName).ToArray();
        }

        public override string[] GetRolesForUser(string username)
        {
            username = Util.GetUserName(username);
            string key = username + "_roles_";
            var a = HttpRuntime.Cache[key] as string[];
            if (a == null)
            {
                var q = from r in DbUtil.Db.UserRoles
                        where r.User.Username == username
                        select r.Role.RoleName;
                a = q.ToArray();
                HttpRuntime.Cache.Insert(key, a, null,
                    DateTime.Now.AddSeconds(5), Cache.NoSlidingExpiration);
            }
            return a;
        }

        public override string[] GetUsersInRole(string rolename)
        {
            var Db = DbUtil.Db;
            var q = from u in Db.Users
                    where u.UserRoles.Any(ur => ur.Role.RoleName == rolename)
                    select u.Username;
            return q.ToArray();
        }

        public IEnumerable<User> GetRoleUsers(string rolename)
        {
            var Db = DbUtil.Db;
            var q = from u in Db.Users
                    where u.UserRoles.Any(ur => ur.Role.RoleName == rolename)
                    select u;
            return q;
        }

        public override bool IsUserInRole(string username, string rolename)
        {
            username = Util.GetUserName(username);
            var Db = DbUtil.Db;
            var q = from ur in Db.UserRoles
                    where rolename == ur.Role.RoleName
                    where username == ur.User.Username
                    select ur;
            return q.Count() > 0;
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] rolenames)
        {
            var Db = DbUtil.Db;
            var q = from ur in Db.UserRoles
                    where rolenames.Contains(ur.Role.RoleName) && usernames.Contains(ur.User.Username)
                    select ur;
            Db.UserRoles.DeleteAllOnSubmit(q);
            Db.SubmitChanges();
        }

        public override bool RoleExists(string rolename)
        {
            var Db = DbUtil.Db;
            return Db.Roles.Count(r => r.RoleName == rolename) > 0;
        }

        public override string[] FindUsersInRole(string rolename, string usernameToMatch)
        {
            var Db = DbUtil.Db;
            var q = from u in Db.Users
                    where u.UserRoles.Any(ur => ur.Role.RoleName == rolename)
                    select u;
            bool left = usernameToMatch.StartsWith("%");
            bool right = usernameToMatch.EndsWith("%");
            usernameToMatch = usernameToMatch.Trim('%');
            if (left && right)
                q = q.Where(u => u.Username.Contains(usernameToMatch));
            else if (left)
                q = q.Where(u => u.Username.EndsWith(usernameToMatch));
            else if (right)
                q = q.Where(u => u.Username.StartsWith(usernameToMatch));
            return q.Select(u => u.Username).ToArray();
        }
    }
}
