using System;
using System.Web.UI;
using UtilityExtensions;
using CMSPresenter;
using System.Data.Linq;
using System.Linq;
using CmsData;

namespace CMSWeb
{
    public partial class UserArea : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HiddenField1.Value = Util.UserName;
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

   }
}