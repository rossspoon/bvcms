using System;
using System.Web.Security;
using UtilityExtensions;
using CmsData;

namespace CmsWeb
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var user = Membership.GetUser(Util.UserName);
        }

        protected void ChangePassword1_ChangedPassword(object sender, EventArgs e)
        {
            CMSMembershipProvider.provider.UserMustChangePassword = false;
        }
    }
}
