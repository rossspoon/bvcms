/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Web.Security;
using System.Collections.Generic;
using System.ComponentModel;
using CmsData;

namespace CMSPresenter
{
    public class RoleData
    {
        public int NumberOfUsersInRole { get; set; }
        public string RoleName { get; set; }
        public string UserName { get; set; }
        public bool UserInRole { get; set; }
    }
    [DataObject(true)]
    public class RoleController
    {
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static List<RoleData> GetRoles()
        {
            return GetRoles(0, false);
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<RoleData> GetRoles(int UserId, bool showOnlyAssignedRolls)
        {
            string userName = "";
            if (UserId > 0)
                userName = Membership.GetUser(UserId, false).UserName;
            var list = new List<RoleData>();

            var roles = Roles.GetAllRoles();
            foreach (var role in roles)
            {

                var inrole = Roles.IsUserInRole(userName, role);
                if (!showOnlyAssignedRolls || inrole)
                {
                    var users = Roles.GetUsersInRole(role);
                    var rd = new RoleData
                    {
                        RoleName = role,
                        UserName = userName,
                        UserInRole = inrole,
                        NumberOfUsersInRole = users.Length
                    };
                    list.Add(rd);
                }
            }
            return list;
        }
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public void Insert(string roleName)
        {
            if (Roles.RoleExists(roleName) == false)
                Roles.CreateRole(roleName);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public void Delete(string roleName)
        {
            Roles.DeleteRole(roleName);
        }
    }
}