using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilityExtensions;
using System.Web.Security;

namespace CmsData
{
    public partial class User
    {
        private CMSDataContext _Db;
        public CMSDataContext Db
        {
            get
            {
                if (_Db == null)
                    _Db = this.GetDataContext() as CMSDataContext;
                return _Db;
            }
        }
        public bool IsOnLine
        {
            get
            {
                var onlineSpan = new TimeSpan(0, Membership.UserIsOnlineTimeWindow, 0);
                var compareTime = DateTime.Now.Subtract(onlineSpan);
                return LastActivityDate > compareTime && LastActivityDate != CreationDate;
            }
        }
        public string BestName { get { return PeopleId.HasValue? Name2 : Username; } }
        public string[] Roles
        {
            get
            {
                return UserRoles.Select(ur => ur.Role.RoleName).ToArray();
            }
            set
            {
                var qdelete = from r in UserRoles
                              where !value.Contains(r.Role.RoleName)
                              select r;
                Db.UserRoles.DeleteAllOnSubmit(qdelete);

                var q = from s in value
                        join r in UserRoles on s equals r.Role.RoleName into g
                        from t in g.DefaultIfEmpty()
                        where t == null
                        select s;

                foreach (var s in q)
                {
                    var role = Db.Roles.Single(r => r.RoleName == s);
                    UserRoles.Add(new UserRole { Role = role });
                }
            }
        }
        public string PasswordSetOnly
        {
            get { return ""; }
            set
            {
            }
        }
        
        
    }
}
