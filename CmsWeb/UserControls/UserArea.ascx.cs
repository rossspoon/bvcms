using System;
using System.Web.UI;
using UtilityExtensions;
using CMSPresenter;
using System.Data.Linq;
using System.Linq;
using CmsData;

namespace CmsWeb
{
    public partial class UserArea : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HiddenField1.Value = Util.UserName;
            TagLink.ToolTip = Util.UserId.ToString();
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            //BindTags();
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            TagLink.Text = Util.CurrentTagName;
        }

        protected void LoginStatus1_LoggingOut(object sender, System.Web.UI.WebControls.LoginCancelEventArgs e)
        {
            Session.Abandon();
        }
   }
}