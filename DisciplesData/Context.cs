using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace DiscData
{
    public partial class DiscDataContext
    {
        private const string STR_User = "CurrentUser";
        public User CurrentUser
        {
            get
            {
                return HttpContext.Current.Items[STR_User] as User;
            }
            set
            {
                HttpContext.Current.Items[STR_User] = value;
            }
        }
        public User GetUser(string username)
        {
            return Users.SingleOrDefault(u => u.Username == username);
        }
        public User GetUser(int? id)
        {
            return Users.SingleOrDefault(u => u.UserId == id);
        }

    }
}
