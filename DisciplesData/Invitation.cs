using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;

namespace DiscData
{
    public partial class Invitation
    {
        public string GroupName
        {
            get { return Group.LoadById(GroupId).Name; }
        }
        public static IEnumerable<Invitation> LoadBySecretCode(string code)
        {
            return from i in DbUtil.Db.Invitations where code == i.Password select i;
        }
    }
    public partial class InvitationController
    {
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public IEnumerable<Invitation> FetchInvitesForGroup(int Id)
        {
            return Group.LoadById(Id).Invitations;
        }
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public IEnumerable<Invitation> FetchAll(string sortExpression)
        {
            switch (sortExpression)
            {
                case "Password":
                    return DbUtil.Db.Invitations.OrderBy(i => i.Password).ThenBy(i => i.GroupName);
                case "Expires":
                    return DbUtil.Db.Invitations.OrderBy(i => i.Expires);
                case "Groupname":
                    return DbUtil.Db.Invitations.OrderBy(i => i.GroupName).ThenBy(i => i.Password);
                case "Password DESC":
                    return DbUtil.Db.Invitations.OrderByDescending(i => i.Password).ThenBy(i => i.GroupName);
                case "Expires DESC":
                    return DbUtil.Db.Invitations.OrderByDescending(i => i.Expires);
                case "Groupname DESC":
                    return DbUtil.Db.Invitations.OrderByDescending(i => i.GroupName).ThenBy(i => i.Password);
            }
            return DbUtil.Db.Invitations;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public void Delete(string password, int groupid)
        {
            var i = DbUtil.Db.Invitations.Single(inv => inv.Password == password && inv.GroupId == groupid);
            DbUtil.Db.Invitations.DeleteOnSubmit(i);
            DbUtil.Db.SubmitChanges();
        }
    }
}